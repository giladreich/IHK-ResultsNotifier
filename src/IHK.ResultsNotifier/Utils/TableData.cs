using System;

namespace IHK.ResultsNotifier.Utils
{
    public class TableData<T>
    {
        private readonly int ROWS;
        private readonly int COLUMNS;

        private T[,] tableData;

        public event Action<DataChangedArgs> DataChanged;

        public TableData(int rows, int columns)
        {
            ROWS = rows;
            COLUMNS = columns;

            tableData = new T[ROWS, COLUMNS];
        }

        public T this[int row, int column]
        {
            get => tableData[row, column];

            set
            {
                tableData[row, column] = value;
                OnDataChanged(row, column);
            }
        }

        public int GetLength(int dimension)
        {
            return tableData.GetLength(dimension);
        }

        protected virtual void OnDataChanged(int row, int column)
        {
            DataChanged?.Invoke(new DataChangedArgs(row, column));
        }
    }

    public class DataChangedArgs : EventArgs
    {
        public int RowIndex { get; set; }
        public int ColumnIndex { get; set; }

        public DataChangedArgs(int row = 0, int column = 0)
        {
            RowIndex = row;
            ColumnIndex = column;
        }
    }

}
