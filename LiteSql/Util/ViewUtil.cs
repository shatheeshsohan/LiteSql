using System.Data;

namespace LiteSql.DB
{
    public class ViewUtil
    {
        public static DataTable populateTable(List<List<string>> tableData, int columnCount)
        {
            var dataGridSource = new DataTable();
            int rowCount = tableData.Count - 1;
            foreach (string columnInfo in tableData[0])
            {
                 AddColumn<string>(dataGridSource, columnInfo);
            }

            for (int rowIndex = 1; rowIndex <= rowCount; rowIndex++)
            {
                var columnValues = new List<string>();
                
                foreach (string rowData in tableData[rowIndex])
                {
                    columnValues.Add(rowData);
                }
                AddRow(dataGridSource, columnValues);
            }
            return dataGridSource;
        }

        private static void AddRow(DataTable dataTable, IList<string> columnValues)
        {
            DataRow rowModelWithCurrentColumns = dataTable.NewRow();
            dataTable.Rows.Add(rowModelWithCurrentColumns);

            for (int columnIndex = 0; columnIndex < dataTable.Columns.Count; columnIndex++)
            {
                rowModelWithCurrentColumns[columnIndex] = Convert.ToString(columnValues[columnIndex]);
            }
        }

        private static void AddColumn<TData>(DataTable dataTable, string columnName, int columnIndex = -1)
        {
            DataColumn newColumn = new DataColumn(columnName, typeof(TData));
            dataTable.Columns.Add(newColumn);
            if (columnIndex > -1)
            {
                newColumn.SetOrdinal(columnIndex);
            }

            int newColumnIndex = dataTable.Columns.IndexOf(newColumn);

            foreach (DataRow row in dataTable.Rows)
            {
                row[newColumnIndex] = default(TData);
            }
        }
    }
}
