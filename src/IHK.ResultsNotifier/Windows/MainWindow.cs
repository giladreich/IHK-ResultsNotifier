using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Custom;
using HtmlAgilityPack;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace IHK.ResultsNotifier.Windows
{
    public partial class MainWindow : CustomForm
    {
        public string TimeStamp => DateTime.Now.ToString("T");

        private const int RESULT_COLUMN_INDEX = 3;
        private readonly HttpClientIHK webClient;

        public MainWindow(HttpClientIHK webClient)
        {
            InitializeComponent();
            this.webClient = webClient;
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            lbxLogs.Items.Add($"[{TimeStamp}] - Successfully logged in!");

            LoadExamsResults();
        }

        private void LoadExamsResults()
        {
            HtmlDocument doc = new HtmlDocument();
            string content = webClient.GetExamResultsDocument();
            doc.LoadHtml(content);

            HtmlNode tableNode = doc.DocumentNode
                .SelectNodes("//*[@id=\"outer\"]/div[2]/div[4]/div[4]")
                .First();

            for (int i = 1; i < 8; i++)
            {
                HtmlNode rowNode = tableNode.ChildNodes[i];
                bool hasData = rowNode.ChildNodes.Count > RESULT_COLUMN_INDEX;
                if (!hasData) continue;

                for (int j = 2; j < 6; j++)
                {
                    string rowValue = rowNode.ChildNodes[j].InnerHtml;
                    if (string.IsNullOrEmpty(rowValue)) continue;

                    dashboard.TableData[i - 1, j - 2] = rowValue;
                }
            }
        }

        private void btnStartStop_Click(object sender, EventArgs e)
        {
            // TODO: Toogle button and spawn a new thread to run in a while loop to update data.

        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            Owner.Show();
        }

    }
}
