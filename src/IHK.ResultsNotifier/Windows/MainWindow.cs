﻿using System;
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

using HtmlAgilityPack;
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
        private NetworkClient networkClient;
        private HtmlParser parser;
        private Audio audio;


        public MainWindow(NetworkClient client)
        {
            InitializeComponent();
            networkClient = client;
            parser = new HtmlParser();
            audio = new Audio(FILE_SOUND_PATH, true);
        }

        private async void MainWindow_Load(object sender, EventArgs e)
        {
            Log("Successfully logged in!", Color.DarkGreen);
            Log("Loading exams results...");

            TableData<string> resultsTable = await GetExamResults();
            dashboard.TableData.Swap(resultsTable);

            File.WriteAllBytes(FILE_SOUND_PATH, Resources.new_results_DE);
            await Task.Run(() => audio.Init());
        }

        private async Task<TableData<string>> GetExamResults()
        {
            loader.Show();
            await Utility.SimulateWork(TimeSpan.FromSeconds(1));

            string content = await RetrieveHtmlContent();
            string xpath = XPathDefines.IHK_RESULTS_TABLE;
            TableData<string> results = await ParseHtmlContent(content, xpath);
            
            loader.Hide();

            return results;
        }

        private async Task<string> RetrieveHtmlContent()
        {
            string content = String.Empty;
            //string content = File.ReadAllText(@"C:\1\test\ihk.html");
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

        private async Task<TableData<string>> ParseHtmlContent(string content, string xpath)
        {
            HtmlNode tableNode = await Task.Run(() => parser.GetHtmlNode(content, xpath));

            return await Task.Run(() => parser.ParseHtmlTableData(tableNode));
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
