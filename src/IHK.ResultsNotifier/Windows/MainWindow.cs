using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Windows.Forms;
using System.Windows.Forms.Custom;
using HtmlAgilityPack;
using IHK.ResultsNotifier.Utils;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace IHK.ResultsNotifier.Windows
{
    public partial class MainWindow : CustomForm
    {
        private const int HTML_FIRST_RESULT_COLUMN_IDX  = 3;
        private const int HTML_MAX_TABLE_ROWS           = 8;
        private const int HTML_MAX_TABLE_COLUMNS        = 6;

        private const int MIN_MINUTES_RESULTS_CHECK     = 5;

        private const string FILE_PATH_TABLE            = "CurrentTableData.txt";


        private readonly HttpClientIHK webClient;
        private Worker worker;


        public MainWindow(HttpClientIHK webClient)
        {
            InitializeComponent();
            this.webClient = webClient;
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            Log("Successfully logged in!");

            UpdateExamsResults();
            TableData<string>.SerializeToFile(dashboard.TableData, FILE_PATH_TABLE);
        }

        private void UpdateExamsResults()
        {
            HtmlDocument doc = new HtmlDocument();
            //string content = File.ReadAllText(@"C:\1\test\ihk.html");
            string content = webClient.GetExamResultsDocument();
            doc.LoadHtml(content);

            HtmlNode tableNode = doc.DocumentNode
                .SelectNodes("//*[@id=\"outer\"]/div[2]/div[4]/div[4]")
                .First();

            ParseHTMLTableData(tableNode);
        }

        private void ParseHTMLTableData(HtmlNode tableNode)
        {
            for (int i = 1; i < HTML_MAX_TABLE_ROWS; i++)
            {
                HtmlNode rowNode = tableNode.ChildNodes[i];
                bool hasData = rowNode.ChildNodes.Count > HTML_FIRST_RESULT_COLUMN_IDX;
                if (!hasData) continue;

                for (int j = HTML_FIRST_RESULT_COLUMN_IDX - 1; j < HTML_MAX_TABLE_COLUMNS; j++)
                {
                    string rowValue = rowNode.ChildNodes[j].InnerHtml;
                    if (String.IsNullOrEmpty(rowValue)) continue;

                    dashboard.TableData[i - 1, j - 2] = rowValue;
                }
            }
        }

        private void btnStartStop_Click(object sender, EventArgs e)
        {
            if (btnStartStop.IsActivated)
            {
                tbxMinutes.Enabled = false;
                Log("Starting listening for new results...");
                Log($"Request to update results will be sent to the server every {tbxMinutes.Text} minutes.");
                worker = new Worker(StartListening).Start();
            }
            else
            {
                tbxMinutes.Enabled = true;
                Log("Stopping listening for results...");
                worker.Stop();
            }
        }

        private void StartListening()
        {
            TableData<string> oldTable = TableData<string>.DeserializeFromFile(FILE_PATH_TABLE);

            int.TryParse(tbxMinutes.Text, out int checkEveryXTime);
            ValidateLoopTime(ref checkEveryXTime);

            do
            {
                this.InvokeSafe(() => Log("Updating exams results."));
                this.InvokeSafe(UpdateExamsResults);

                if (!oldTable.SequenceEqual(dashboard.TableData))
                {
                    this.InvokeSafe(() => tbxLogs.ForeColor = Color.DarkGreen);
                    this.InvokeSafe(() => Log("Wohooo....New results available!!!!!"));
                    this.InvokeSafe(Activate);
                    SystemSounds.Beep.Play();
                }
                else
                {
                    this.InvokeSafe(() => tbxLogs.ForeColor = DefaultForeColor);
                    this.InvokeSafe(() => Log("Boring...nothing new."));
                }

#if DEBUG
                worker.ThreadToken.WaitOne(TimeSpan.FromSeconds(checkEveryXTime));
#else
                worker.ThreadToken.WaitOne(TimeSpan.FromMinutes(checkEveryXTime));
#endif
            } while (worker.IsWorking);


            TableData<string>.SerializeToFile(dashboard.TableData, FILE_PATH_TABLE);
        }

        private bool ValidateLoopTime(ref int minutes)
        {
            if (minutes < MIN_MINUTES_RESULTS_CHECK)
            {
                string msg =
                    $"Wow...Seriously? less than {MIN_MINUTES_RESULTS_CHECK} minutes? " +
                    $"Minimum is set to {MIN_MINUTES_RESULTS_CHECK} mins, sorry...";

                this.InvokeSafe(() => Log(msg));
                minutes = MIN_MINUTES_RESULTS_CHECK;

                return false;
            }

            return true;
        }

        private void btnClearLog_Click(object sender, EventArgs e) => tbxLogs.Clear();

        private void Log(string message)
        {
            tbxLogs.SelectionStart  = 0;
            tbxLogs.SelectionLength = 0;
            tbxLogs.SelectedText    = $"[{Utility.TimeStamp}] - {message}\n";
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (worker != null && worker.IsWorking)
            {
                Log("Cleaning up worker threads before application closes.");
                worker.Stop();
                worker.Dispose();
            }

            webClient?.Dispose();
            Owner?.Show();
        } 

    }
}
