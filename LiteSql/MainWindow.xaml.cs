using LiteSql.DB;
using LiteSql.Models;
using LiteSql.Util;
using SQLite;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

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
                    List<TableNameData> results = conn.Query<TableNameData>(allTablequery); ;

                    List<string> sortedTableNames = results.OrderBy(r => r.Name)
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
    }
}