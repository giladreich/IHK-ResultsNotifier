using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace IHK.ResultsNotifier.Utils
{
    public class TableData<T> : IEquatable<TableData<T>>, IComparable<TableData<T>>
    {
        private readonly int ROWS, COLUMNS;

        private readonly T[,] _tableData;

        public event Action<object, DataChangedEventArgs> DataChanged;

        public TableData(int rows, int columns)
        {
            ROWS = rows;
            COLUMNS = columns;

            _tableData = new T[ROWS, COLUMNS];
        }


        public T this[int row, int column]
        {
            get => _tableData[row, column];

            set
            {
                _tableData[row, column] = value;
                OnDataChanged(row, column);
            }
        }

        public static void SerializeToFile(TableData<T> tableData, string filePath)
        {
            File.WriteAllText(filePath, tableData.ToString());
        }

        public static TableData<T> DeserializeFromFile(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException();

            string[] rowsDirty = File.ReadAllLines(filePath);
            string[] arrayDimension = rowsDirty[0].Split('#');

            int.TryParse(arrayDimension[0], out int rows);
            int.TryParse(arrayDimension[1], out int columns);

            TableData<T> newTableData = new TableData<T>(rows, columns);

            for (int i = 1; i < newTableData.GetLength(0) + 1; i++)
            {
                string[] rowData = rowsDirty[i].Split('#');

                for (int j = 0; j < newTableData.GetLength(1); j++)
                {
                    newTableData[i - 1, j] = (T)(object)rowData[j];
                }
            }

            return newTableData;
        }

        public static bool SequenceEqual(TableData<T> table1, TableData<T> table2)
        {

            if (table1 == null || table2 == null) return false;

            if (table1.ROWS    != table2.ROWS &&
                table1.COLUMNS != table2.COLUMNS)
                return false;

            for (int i = 0; i < table1.GetLength(0); i++)
                for (int j = 0; j < table1.GetLength(1); j++)
                    if (!table1[i, j].Equals(table2[i, j]))
                        return false;

            return true;
        }

        protected virtual void OnDataChanged(int row, int column)
        {
            DataChanged?.Invoke(this, new DataChangedEventArgs(row, column));
        }


        public int GetLength(int dimension)
        {
            return _tableData.GetLength(dimension);
        }

        public TableData<T> Duplicate()
        {
            TableData<T> newTable = new TableData<T>(ROWS, COLUMNS);
            for (int i = 0; i < GetLength(0); i++)
                for (int j = 0; j < GetLength(1); j++)                 
                    newTable[i, j] = this[i, j];

            return newTable;
        }

        public void Swap(TableData<T> other)
        {
            for (int i = 0; i < GetLength(0); i++)
                for (int j = 0; j < GetLength(1); j++)
                    this[i, j] = other[i, j];
        }

        public bool SequenceEqual(TableData<T> other)
        {
            if (!Equals(other))
                return false;

            for (int i = 0; i < GetLength(0); i++)
                for (int j = 0; j < GetLength(1); j++)
                    if (!this[i, j].Equals(other[i, j]))
                        return false;

            return true;
        }

        /// <summary>
        /// Compares table layout and data equality.
        /// </summary>
        public int CompareTo(TableData<T> other)
        {
            if (ReferenceEquals(this, other)) return 0; // if same memory address.
            if (ReferenceEquals(null, other)) return 1;

            return SequenceEqual(other) ? 0 : 1;
        }

        /// <summary>
        /// Checks table layout equality.
        /// Does not check for data equality.
        /// </summary>
        public override bool Equals(object other)
        {
            return Equals(other as TableData<T>);
        }

        /// <summary>
        /// Checks table layout equality.
        /// Does not check for data equality.
        /// </summary>
        public bool Equals(TableData<T> other)
        {
            return other   != null          &&
                   ROWS    == other.ROWS    &&
                   COLUMNS == other.COLUMNS;
        }


        public override int GetHashCode()
        {
            int hashCode = 1850346236;

            hashCode = hashCode * -1521134295 + ROWS.GetHashCode();
            hashCode = hashCode * -1521134295 + COLUMNS.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<T[,]>.Default.GetHashCode(_tableData);

            return hashCode;
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine($"{ROWS}#{COLUMNS}");

            for (int i = 0; i < GetLength(0); i++)
            {
                for (int j = 0; j < GetLength(1); j++)
                {
                    result.Append($"{this[i, j]}#");
                }

                result.Remove(result.Length - 1, 1);
                result.AppendLine();
            }

            return result.ToString();
        }

    }
}
