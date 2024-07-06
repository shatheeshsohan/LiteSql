using LiteSql.DB;
using LiteSql.Models;
using LiteSql.Util;
using SQLite;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static SQLite.SQLite3;
using static SQLite.SQLiteConnection;

namespace LiteSql
{
    public partial class MainWindow : Window
    {
        MessageBoxButton connectMsgBoxButtons;
        MessageBoxImage connectMsgBoxicon;
        MessageBoxImage connectMsgBoxErroricon;
        private SQLiteConnection? conn = null;
        private Queue<IRemoteRow> rowsQueue = new Queue<IRemoteRow>();
    
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
                if (conn != null && connectButton.Content.ToString() == Constants.DISCONNET)
                {
                    connectButton.Background = Brushes.Transparent;
                    connectButton.Content = Constants.CONNET;
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
                if (conn != null && conn.Handle != null && connectButton.Content.ToString() == Constants.CONNET)
                {
                    connectButton.Background = Brushes.Red;
                    connectButton.Content = Constants.DISCONNET;
                }
            }
        }

        private void tableComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            string query = $"SELECT * FROM " + tableComboBox.SelectedValue;
            List<ColumnInfo>  columnInfos= conn.GetTableInfo((string)tableComboBox.SelectedValue).ToList();;
            var ResultList = DBUtil.getResultList(conn, query);
            this.mainTable.ItemsSource = ViewUtil.populateTable(ResultList, columnInfos.Count).DefaultView;
            Constants.OBJKEY_INDEX = CommonUtil.getObjKeyIndex(ResultList[0]);
            makeReadOnlyColumns();
        }
        private void mainTable_CellEditTriggered(object sender, DataGridCellEditEndingEventArgs e)
        {
            IEditableCollectionView itemsView = ((DataGrid)sender).Items;
            TextBox editedValue = e.EditingElement as TextBox;
            var objKeyRowInfo= new DataGridCellInfo(((DataGrid)sender).Items[e.Row.GetIndex()], ((DataGrid)sender).Columns[Constants.OBJKEY_INDEX]);
            string objKey = (string)((DataRowView)objKeyRowInfo.Item).Row.ItemArray[Constants.OBJKEY_INDEX];
            rowsQueue.Enqueue(new UpdateRow(tableComboBox.SelectedValue.ToString(), e.Column.Header.ToString(), editedValue.Text, objKey, Constants.ROW_UPDATE));

        }

        private void makeReadOnlyColumns ()
        {
            foreach (DataGridColumn col in this.mainTable.Columns)
            {
                if (col.Header.Equals("obj_id") || col.Header.Equals("row_id") || col.Header.Equals("obj_version") || col.Header.Equals("obj_key") || col.Header.Equals("objgrants")) 
                {
                    col.IsReadOnly = true;
                }
                
            }
        }

        private void ignoreButton_Click(object sender, RoutedEventArgs e)
        {
            rowsQueue.Clear();
        }

        private void refreshButton_Click(object sender, RoutedEventArgs e)
        {
            this.mainTable.ItemsSource = null;
            this.tableComboBox_SelectionChanged(sender, null);
            this.rowsQueue.Clear();
        }
    }
}