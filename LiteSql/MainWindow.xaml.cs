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
        private SQLiteConnection? conn;
        private Queue<IRemoteRow> rowsQueue = new Queue<IRemoteRow>();
        private SettingsData settingsData = new SettingsData();
        private bool isDbConnected = false;

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
                if (conn != null && isDbConnected)
                {
                    connectButton.Background = Brushes.Transparent;
                    connectButton.Content = Constants.CONNET;
                    conn.Close();
                    conn = null;
                    this.tableComboBox.Items.Clear();
                    this.mainTable.ItemsSource = null;  
                    this.isDbConnected = false;
                }
                else if (conn == null || !isDbConnected)
                {

                    conn = new SQLiteCipherConnection(connectStringTextField.Text, settingsData).getConnection();
                    string allTablequery = $"SELECT name FROM sqlite_schema WHERE type ='table' AND name NOT LIKE 'sqlite_%'";
                    List<TableNameData> results = conn.Query<TableNameData>(allTablequery).OrderBy(r => r.Name).ToList(); ;

                    foreach (TableNameData data in results )
                    {
                        tableComboBox.Items.Add(data.Name);
                    }
                    this.isDbConnected = true;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Not connected : " + ex.Message, "DB Status", connectMsgBoxButtons, connectMsgBoxErroricon);
                this.isDbConnected = false;

            }
            finally
            {
                if (conn != null && conn.Handle != null && isDbConnected)
                {
                    connectButton.Background = Brushes.Red;
                    connectButton.Content = Constants.DISCONNET;
                }
            }
        }

        private void tableComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (tableComboBox.SelectedValue != null)
            {
                string query = $"SELECT * FROM " + tableComboBox.SelectedValue;
                List<ColumnInfo> columnInfos = conn.GetTableInfo((string)tableComboBox.SelectedValue).ToList(); ;
                var ResultList = DBUtil.getResultList(conn, query);
                this.mainTable.ItemsSource = ViewUtil.populateTable(ResultList, columnInfos.Count).DefaultView;
                Constants.OBJKEY_INDEX = CommonUtil.getObjKeyIndex(ResultList[0]);
                makeReadOnlyColumns();
            }
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

        private void settingsButton_Click(object sender, RoutedEventArgs e)
        {
            var preferenceWindow = new Preference();
            preferenceWindow.ShowDialog();
            settingsData = preferenceWindow.settingsData;
        }

        private void commitButton_Click(object sender, RoutedEventArgs e)
        {
            while (rowsQueue.Count > 0)
            {
                rowsQueue.Dequeue().executeSQL(conn);
            }
            
        }
        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            string header = e.Column.Header.ToString();
            e.Column.Header = header.Replace("_", "__");
        }
    }
}