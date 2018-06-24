using System;
using System.Windows.Forms;

using IHK.ResultsNotifier.Utils;


namespace IHK.ResultsNotifier.Controls
{

    public partial class Dashboard : UserControl
    {
        private const int ROWS    = 6;
        private const int COLUMNS = 4;

        public TableData<string> TableData { get; }

        public Dashboard()
        {
            InitializeComponent();

            TableData = new TableData<string>(ROWS, COLUMNS);
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            InitDashboard();
            RegisterHandlers();
        }

        private void InitDashboard()
        {
            for (int i = 1; i < 8; i++)
            {
                CustomLabel lbl = new CustomLabel();
                lbl.Text = $"{i}.";
                panelMain.Controls.Add(lbl, 0, i);
            }


            // Result Notes placeholders:
            // Columns : 2 - 5
            // Rows    : 1 - 7
            for (int i = 2; i < 6; i++)
                for (int j = 1; j < 8; j++)
                    panelMain.Controls.Add(new CustomLabel(), i, j);


            for (int i = 0; i < TableData.GetLength(0); i++)
                for (int j = 0; j < TableData.GetLength(1); j++)
                    TableData[i, j] = "-";

        }

        private void RegisterHandlers()
        {
            TableData.DataChanged += TableDataChanged;
        }

        private void TableDataChanged(object sender, DataChangedEventArgs e)
        {
            Control lbl = panelMain.GetControlFromPosition(e.ColumnIndex + 2, e.RowIndex + 1);
            lbl.Text(TableData[e.RowIndex, e.ColumnIndex]);
        }

    }
}
