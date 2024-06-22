using LiteSql.DB;
using LiteSql.Models;
using LiteSql.Util;
using SQLite;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using static SQLite.SQLiteConnection;

namespace LiteSql
{
    public partial class MainWindow : Window
    {
        MessageBoxButton connectMsgBoxButtons;
        MessageBoxImage connectMsgBoxicon;
        MessageBoxImage connectMsgBoxErroricon;
        private SQLiteConnection? conn = null;

        public MainWindow()
        {
            InitializeComponent();
            UIInit();
        }

        private void UIInit() {
            connectMsgBoxButtons = MessageBoxButton.OK;
            connectMsgBoxicon = MessageBoxImage.Information;
            connectMsgBoxErroricon = MessageBoxImage.Error;
        }

        private void connectButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (conn != null && connectButton.Content.ToString() == Constant.DISCONNET)
                {
                    connectButton.Background = Brushes.Transparent;
                    connectButton.Content = Constant.CONNET;
                    conn.Close();
                    conn = null;
                }
                else if (conn == null)
                {

                    conn = new SQLiteCipherConnection(connectStringTextField.Text, "iAr4dTqA30LMZpSH1hlOspGcnAn77DUkmog1U9l5siA=+B^erH^Tsr@7$6szk+").getConnection(); ;
                    string allTablequery = $"SELECT name FROM sqlite_schema WHERE type ='table' AND name NOT LIKE 'sqlite_%'";
                    List<TableNameData> results = conn.Query<TableNameData>(allTablequery).OrderBy(r => r.Name).ToList(); ;

                    foreach (TableNameData data in results )
                    {
                        tableComboBox.Items.Add(data.Name);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Not connected : " + ex.Message, "DB Status", connectMsgBoxButtons, connectMsgBoxErroricon);
            }
            finally
            {
                if (conn != null && conn.Handle != null && connectButton.Content.ToString() == Constant.CONNET)
                {
                    //MessageBox.Show("Connected.", "DB Status", connectMsgBoxButtons, connectMsgBoxicon);
                    connectButton.Background = Brushes.Red;
                    connectButton.Content = Constant.DISCONNET;
                }
            }
        }

        private void tableComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var dataGridSource = new DataTable();
            List<ColumnInfo>  columnInfos= conn.GetTableInfo((string)tableComboBox.SelectedValue).OrderBy(c => c.Name).ToList();
            string query = $"SELECT * FROM activity_type";
            
            int columnCount = columnInfos.Count;
            foreach (ColumnInfo columnInfo in columnInfos)
            {
                AddColumn<double>(dataGridSource, columnInfo.Name);
            }
            
            for (int rowIndex = 0; rowIndex < columnCount; rowIndex++)
            {
                var columnValues = new List<object>();
                for (int columnIndex = 0; columnIndex < dataGridSource.Columns.Count; columnIndex++)
                {
                    columnValues.Add("1");
                }
                AddRow(dataGridSource, columnValues);
            }
            

            this.MainTable.ItemsSource = dataGridSource.DefaultView;
        }

        private void AddRow(DataTable dataTable, IList<object> columnValues)
        {
            DataRow rowModelWithCurrentColumns = dataTable.NewRow();
            dataTable.Rows.Add(rowModelWithCurrentColumns);

            for (int columnIndex = 0; columnIndex < dataTable.Columns.Count; columnIndex++)
            {
                rowModelWithCurrentColumns[columnIndex] = columnValues[columnIndex].ToString();
            }
        }

        private void AddColumn<TData>(DataTable dataTable, string columnName, int columnIndex = -1)
        {
            DataColumn newColumn = new DataColumn(columnName, typeof(TData));

            dataTable.Columns.Add(newColumn);
            if (columnIndex > -1)
            {
                newColumn.SetOrdinal(columnIndex);
            }

            int newColumnIndex = dataTable.Columns.IndexOf(newColumn);

            // Initialize existing rows with default value for the new column
            foreach (DataRow row in dataTable.Rows)
            {
                row[newColumnIndex] = default(TData);
            }
        }
    }
}