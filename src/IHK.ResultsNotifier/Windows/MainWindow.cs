using System;
using System.Drawing;
using System.IO;
using System.Media;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Custom;
using HtmlAgilityPack;
using IHK.ResultsNotifier.Utils;
using Nito.AsyncEx;


namespace IHK.ResultsNotifier.Windows
{
    public partial class MainWindow : CustomForm
    {
        public static readonly string FILE_TABLE_PATH = Path.Combine(Path.GetTempPath(), "IHK-Results.txt");
        public static readonly string FILE_SOUND_PATH = Path.Combine(Path.GetTempPath(), "new_results_DE.mp3");

        private const int MIN_MINUTES_RESULTS_CHECK    = 5;
        private const int ALERT_COUNT_WHEN_NEW_RESULTS = 8;

        private readonly AsyncLock mutex = new AsyncLock();

        private Worker worker;
        private WebClientIHK webClient;
        private HtmlParser parser;
        private Audio audio;


        public MainWindow(WebClientIHK client)
        {
            InitializeComponent();
            webClient = client;
            parser = new HtmlParser();
            audio = new Audio(FILE_SOUND_PATH, true);
        }

        private async void MainWindow_Load(object sender, EventArgs e)
        {
            Log("Successfully logged in!", Color.DarkGreen);
            Log("Loading exams results...");

            TableData<string> resultsTable = await GetExamResults();
            dashboard.TableData.Swap(resultsTable);

            File.WriteAllBytes(FILE_SOUND_PATH, Properties.Resources.new_results_DE);
            audio.Init();
        }

        private async Task<TableData<string>> GetExamResults()
        {
            loader.Show();
            await Utility.SimulateWork(1000);

            //string content = File.ReadAllText(@"C:\1\test\ihk.html");
            string content = await webClient.GetExamResultsDocument();
            string xpath   = "//*[@id=\"outer\"]/div[2]/div[4]/div[4]";

            HtmlNode tableNode        = await Utility.StartTask(() => parser.GetHtmlNode(content, xpath));
            TableData<string> results = await Utility.StartTask(() => parser.ParseHtmlTableData(tableNode));

            loader.Hide();

            return results;
        }

        private void btnStartStop_Click(object sender, EventArgs e)
        {
            if (btnStartStop.IsActivated)
            {
                tbxMinutes.Enabled = false;
                Log("Starting to listen for new results...");
                Log($"Request to update results will be sent to the server every {tbxMinutes.Text} minutes.");
                worker = new Worker(StartListening).Start();
            }
            else
            {
                tbxMinutes.Enabled = true;
                Log("Stopping to listen for results...");
                worker.Stop();
            }
        }

        private async void StartListening()
        {            
            TableData<string>.SerializeToFile(dashboard.TableData, FILE_TABLE_PATH);

            using (await mutex.LockAsync())
            {
                int.TryParse(tbxMinutes.Text, out int checkEveryXTime);
                ValidateLoopTime(ref checkEveryXTime);

                do
                {
                    Log("Updating exams results.");
                    TableData<string> newData = await GetExamResults();

                    if (!dashboard.TableData.SequenceEqual(newData))
                    {
                        dashboard.TableData.Swap(newData);
                        Log("Wohooo....New results are available!!!!!", Color.DarkGreen);
                        audio.Play();

                        for (int i = 0; i < ALERT_COUNT_WHEN_NEW_RESULTS && worker.IsWorking; i++)
                        {
                            this.InvokeSafe(Activate);
                            SystemSounds.Beep.Play();
                            worker.Sleep(TimeSpan.FromSeconds(3));
                        }

                        audio.Stop();
                        TableData<string>.SerializeToFile(newData, FILE_TABLE_PATH);
                    }
                    else
                    {
                        Log("Boring...nothing new.", Color.DarkRed);
                    }

#if DEBUG
                    worker.Sleep(TimeSpan.FromSeconds(checkEveryXTime));
#else
                    worker.Sleep(TimeSpan.FromMinutes(checkEveryXTime));
#endif

                } while (worker.IsWorking);
            } // unlocks mutex end scope
        }

        private bool ValidateLoopTime(ref int minutes)
        {
            if (minutes >= MIN_MINUTES_RESULTS_CHECK)
                return true;

            string msg =
                $"Wow...Seriously? less than {MIN_MINUTES_RESULTS_CHECK} minutes? " +
                $"Minimum is set to {MIN_MINUTES_RESULTS_CHECK} mins, sorry...";

            Log(msg, Color.DarkOrange);
            tbxMinutes.Text(MIN_MINUTES_RESULTS_CHECK.ToString());
            minutes = MIN_MINUTES_RESULTS_CHECK;

            return false;
        }

        private void Log(string message, Color? color = null)
        {
            this.InvokeSafe(() =>
            {
                tbxLogs.SelectionColor = color ?? SystemColors.WindowText;
                tbxLogs.SelectedText   = $"[{Utility.TimeStamp}] - {message}\n";
                tbxLogs.ScrollToCaret();
            });
        }

        private void btnClearLog_Click(object sender, EventArgs e) => tbxLogs.Clear();

        private async void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (btnStartStop.IsActivated)
            {
                // If any threads are working, delay the closing event after cleanup.
                e.Cancel = true;

                Log("Cleaning up background threads before application closes.");
                this.InvokeSafe(() => btnStartStop.PerformClick());

                await Utility.SimulateWork(4000);
                this.InvokeSafe(Close);
            }

            Dispose();
            DeleteTempFiles();

            Owner?.Visible(true);
        }

        public new void Dispose()
        {
            worker?.Dispose();
            webClient?.Dispose();
            parser?.Dispose();
            audio?.Dispose();
        }

        private static void DeleteTempFiles()
        {
            try
            {
                if (File.Exists(FILE_SOUND_PATH))
                    File.Delete(FILE_SOUND_PATH);

                if (File.Exists(FILE_TABLE_PATH))
                    File.Delete(FILE_TABLE_PATH);
            }
            catch (IOException e)
            {
                MessageBox.Show("Failed to delete temporary files -> " + e.Message, 
                                "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
