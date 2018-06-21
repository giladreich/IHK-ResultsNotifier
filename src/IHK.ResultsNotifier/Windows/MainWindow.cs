using System;
using System.Drawing;
using System.IO;
using System.Media;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Custom;
using HtmlAgilityPack;
using IHK.ResultsNotifier.Utils;


namespace IHK.ResultsNotifier.Windows
{
    public partial class MainWindow : CustomForm
    {
        public static readonly string FILE_PATH_TABLE = Path.Combine(Path.GetTempPath(), "IHK-Results.txt");

        private const int MIN_MINUTES_RESULTS_CHECK    = 5;
        private const int ALERT_COUNT_WHEN_NEW_RESULTS = 10;

        private readonly WebClientIHK webClient;
        private readonly HtmlParser parser; 
        private Worker worker;


        public MainWindow(WebClientIHK webClient)
        {
            InitializeComponent();
            this.webClient = webClient;
            parser = new HtmlParser();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            Log("Successfully logged in!", Color.DarkGreen);

            TableData<string> resultsTable = GetExamResults();
            dashboard.TableData.Swap(resultsTable);
        }

        private TableData<string> GetExamResults()
        {
            //string content = File.ReadAllText(@"C:\1\test\ihk.html");
            string content = webClient.GetExamResultsDocument();
            string xpath = "//*[@id=\"outer\"]/div[2]/div[4]/div[4]";

            HtmlNode tableNode = parser.GetHtmlNode(content, xpath);

            return parser.ParseHtmlTableData(tableNode);
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

        private void StartListening()
        {
            TableData<string>.SerializeToFile(dashboard.TableData, FILE_PATH_TABLE);

            int.TryParse(tbxMinutes.Text, out int checkEveryXTime);
            ValidateLoopTime(ref checkEveryXTime);

            do
            {
                this.InvokeSafe(() => Log("Updating exams results."));
                TableData<string> newData = GetExamResults();

                if (!dashboard.TableData.SequenceEqual(newData))
                {
                    this.InvokeSafe(() => dashboard.TableData.Swap(newData));
                    this.InvokeSafe(() => Log("Wohooo....New results are available!!!!!", Color.DarkGreen));

                    for (int i = 0; i < ALERT_COUNT_WHEN_NEW_RESULTS && worker.IsWorking; i++)
                    {
                        this.InvokeSafe(Activate);
                        SystemSounds.Beep.Play();
                        worker.Sleep(TimeSpan.FromSeconds(3));
                    }

                    TableData<string>.SerializeToFile(newData, FILE_PATH_TABLE);
                }
                else
                {
                    this.InvokeSafe(() => Log("Boring...nothing new.", Color.DarkRed));
                }

#if DEBUG
                worker.Sleep(TimeSpan.FromSeconds(checkEveryXTime));
#else
                worker.Sleep(TimeSpan.FromMinutes(checkEveryXTime));
#endif
            } while (worker.IsWorking);

        }

        private bool ValidateLoopTime(ref int minutes)
        {
            if (minutes < MIN_MINUTES_RESULTS_CHECK)
            {
                string msg =
                    $"Wow...Seriously? less than {MIN_MINUTES_RESULTS_CHECK} minutes? " +
                    $"Minimum is set to {MIN_MINUTES_RESULTS_CHECK} mins, sorry...";

                this.InvokeSafe(() => Log(msg, Color.DarkOrange));
                this.InvokeSafe(() => tbxMinutes.Text = MIN_MINUTES_RESULTS_CHECK.ToString());
                minutes = MIN_MINUTES_RESULTS_CHECK;

                return false;
            }

            return true;
        }

        private void btnClearLog_Click(object sender, EventArgs e) => tbxLogs.Clear();

        private void Log(string message, Color? color = null)
        {
            tbxLogs.SelectionColor = color ?? SystemColors.WindowText;
            tbxLogs.SelectedText   = $"[{Utility.TimeStamp}] - {message}\n";
            tbxLogs.ScrollToCaret();
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (worker != null && worker.IsWorking)
            {
                Log("Cleaning up worker thread before application closes.");
                worker.Stop();
                worker.Dispose();

                // Little cheat ;) So the logs get the chance to appear before closing.
                new Worker(() => Thread.Sleep(2000)).Start(true);
                GC.Collect();
            }

            if (File.Exists(FILE_PATH_TABLE))
                File.Delete(FILE_PATH_TABLE);

            webClient?.Dispose();
            Owner?.Show();
        } 

    }
}
