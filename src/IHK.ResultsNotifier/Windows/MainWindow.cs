using System;
using System.Windows.Forms;
using System.Windows.Forms.Custom;

namespace IHK.ResultsNotifier.Windows
{
    public partial class MainWindow : CustomForm
    {
        public string TimeStamp => DateTime.Now.ToString("T");


        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            lbxLogs.Items.Add($"[{TimeStamp}] - Successfully logged in!");


            //dashboard.TableData[3, 2] = "test";

        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            Owner.Show();
        }

        private void btnStartStop_Click(object sender, EventArgs e)
        {
            // TODO: Toogle button and spawn a new thread to run in a while loop to update data.

        }
    }
}
