using System;


namespace IHK.ResultsNotifier.Utils
{
    /// <summary>
    /// Event args for the TableData object.
    /// </summary>
    public class DataChangedEventArgs : EventArgs
    {
        public int RowIndex { get; set; }
        public int ColumnIndex { get; set; }

        public DataChangedEventArgs(int row = 0, int column = 0)
        {
            RowIndex = row;
            ColumnIndex = column;
        }
    }
}