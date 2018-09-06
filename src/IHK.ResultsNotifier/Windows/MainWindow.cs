using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Media;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Custom;
using System.Net.Http;
using System.Security.Authentication;

using IHK.ResultsNotifier.Properties;
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
        private readonly User user;

        private Worker worker;
        private NetworkClient networkClient;
        private HtmlParser parser;
        private Audio audio;


        public MainWindow(NetworkClient client, User user)
        {
            InitializeComponent();
            this.user = user;
            networkClient = client;
            parser = new HtmlParser();
        }

        private async void MainWindow_Load(object sender, EventArgs e)
        {
            Log("Successfully logged in!", Color.DarkGreen);
            Log("Loading exams results...");

            string content = await RetrieveHtmlContent();
            string xpath = XPathDefines.IHK_CANDIDATE_DATA;

            string username = await Task.Run(() => parser.GetUsername(content, xpath));
            lblLoggedInAs.Text($"You are logged in as {username}");

            TableData<string> resultsTable = await GetExamResults(content);
            dashboard.TableData.Clone(resultsTable);
        }

        private async Task<TableData<string>> GetExamResults(string htmlContent = null)
        {
            loader.Show();
            await Utility.SimulateWork(TimeSpan.FromSeconds(1));

            string content = htmlContent ?? await RetrieveHtmlContent();
            string xpath = XPathDefines.IHK_RESULTS_TABLE;

            if (String.IsNullOrEmpty(content))
                return null;

            TableData<string> results = await Task.Run(() => parser.GetExamResultsTable(content, xpath));

            loader.Hide();

            return results;
        }

        private async Task<string> RetrieveHtmlContent()
        {
            string content = String.Empty;
#if LOCAL_TEST
            content = await Task.Run(() => File.ReadAllText(@"..\..\..\IHK.ResultsNotifier.Tests\html_content_sample\IHK_Results_Page.html"));
            return content;
#endif
            try
            {
                content = await networkClient.GetExamResultsDocument();
            }
            catch (HttpRequestException ex)
            {
                RequestDataFallback("Lost connection or could not reach the server. Please check your connection.", 
                    ex, "INFO", MessageBoxIcon.Exclamation);
            }
            catch (AuthenticationException ex)
            {
                // Probably User's session expired. Running another attemp to authenticate the user and retrieve results.
                try {
                    await networkClient.AuthenticateUser(user);
                    if (networkClient.IsAuthenticated)
                        return await networkClient.GetExamResultsDocument();
                } catch { /*ignored*/ }

                RequestDataFallback("User authentication lost. Please try to relog." , ex, "INFO",
                    MessageBoxIcon.Exclamation);
            }
            catch (Exception ex)
            {
                RequestDataFallback("Unknown error has occurred (4).", ex, "ERROR", MessageBoxIcon.Error);
            }

            return content;
        }

        private void RequestDataFallback(string message, Exception ex, string caption, MessageBoxIcon icon)
        {
            loader.Hide();
            this.InvokeSafe(() =>
                MessageBox.Show($"{message}\n"
                                + ex.Message, caption, MessageBoxButtons.OK, icon));
            this.Close(true);
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
                    Log("Retrieving exams results...");
                    TableData<string> newData = await GetExamResults();
                    if (newData == null) continue;

                    if (!dashboard.TableData.SequenceEqual(newData))
                    {
                        dashboard.TableData.Clone(newData);
                        TableData<string>.SerializeToFile(newData, FILE_TABLE_PATH);
                        InitializeAudio();
                        NotifyNewResults();
                    }
                    else
                    {
                        Log("Boring...nothing new.", Color.DarkRed);
                    }

                    if (!worker.IsWorking) break;
#if DEBUG
                    worker.Sleep(TimeSpan.FromSeconds(checkEveryXTime));
#else
                    worker.Sleep(TimeSpan.FromMinutes(checkEveryXTime));
#endif
                } while (worker.IsWorking);
            } // unlocks mutex end scope
        }

        private void InitializeAudio()
        {
            if (!File.Exists(FILE_SOUND_PATH))
                File.WriteAllBytes(FILE_SOUND_PATH, Resources.new_results_DE);

            audio = new Audio(FILE_SOUND_PATH, true);
        }

        private void NotifyNewResults()
        {
            Log("Wohooo....New results are available!!!!!", Color.DarkGreen);
            audio.Play();
            Color tmpColor = FormBorders.Color;
            this.FormBordersColor(Color.PaleGreen);

            for (int i = 0; i < ALERT_COUNT_WHEN_NEW_RESULTS && worker.IsWorking; i++)
            {
                this.InvokeSafe(Activate);
                SystemSounds.Beep.Play();
                worker.Sleep(TimeSpan.FromSeconds(3));
            }

            audio.Stop();
            audio.Dispose();
            this.FormBordersColor(tmpColor);
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

                this.InvokeSafe(() => btnStartStop.PerformClick());
                Log("Cleaning up background threads before application closes.");

                await Utility.SimulateWork(TimeSpan.FromSeconds(4));
                this.Close(true);
            }

            Dispose();
            DeleteTempFiles();

            Owner?.Visible(true);
        }

        private static void DeleteTempFiles()
        {
            List<string> filesToDelete = new List<string>
            {
                FILE_TABLE_PATH, FILE_SOUND_PATH
            };

            try
            {
                foreach (string file in filesToDelete)
                {
                    if (File.Exists(file))
                        File.Delete(file);
                }
            }
            catch (IOException ex)
            {
                DeleteFilesFallback(ex);
            }
            catch (Exception ex)
            {
                DeleteFilesFallback(ex);
            }
        }

        private static void DeleteFilesFallback(Exception ex)
        {
            MessageBox.Show("Failed to delete temporary files -> " + ex.Message,
                            "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public new void Dispose()
        {
            worker?.Dispose();
            parser?.Dispose();
            audio?.Dispose();
        }
    }
}
