using LiteSql.DB;
using System;
using System.Data;
using System.Data.SQLite;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LiteSql
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MessageBoxButton connectMsgBoxButtons;
        MessageBoxImage connectMsgBoxicon;
        public MainWindow()
        {
            InitializeComponent();
        }

        public void UIInit() {
            connectMsgBoxButtons = MessageBoxButton.OK;
            connectMsgBoxicon = MessageBoxImage.Information;
        }

        private void connectButton_Click(object sender, RoutedEventArgs e)
        {
            //SQLiteConnection conn = new SQLLiteDBConnection(connectString.Text).getConnection();
            SQLiteConnection sqlite = new SQLiteConnection("Data Source=C:\\Users\\satrlk\\AppData\\Local\\Packages\\IFS.IFSAurenaNative10IBS_4zvhwksmf3w4m\\LocalState\\database_1.db");
            sqlite.SetPassword("iAr4dTqA30LMZpSH1hlOspGcnAn77DUkmog1U9l5siA=+B^erH^Tsr@7$6szk+");
            SQLiteDataAdapter ad;
            DataTable dt = new DataTable();

            try
            {
                SQLiteCommand cmd;
                sqlite.Open();  //Initiate connection to the db
                cmd = sqlite.CreateCommand();
                cmd.CommandText = "SELECT name FROM sqlite_schema WHERE type ='table' AND name NOT LIKE 'sqlite_%'";  //set the passed query
                ad = new SQLiteDataAdapter(cmd);
                ad.Fill(dt); //fill the datasource

                SQLiteDataReader sqlite_datareader;
  

                sqlite_datareader = cmd.ExecuteReader();
                while (sqlite_datareader.Read())
                {
                    string myreader = sqlite_datareader.GetString(0);
                    System.Diagnostics.Debug.WriteLine(myreader);
                }
            }
            catch (SQLiteException ex)
            {
                //Add your exception code here.
            }
            sqlite.Close();
            MessageBox.Show("Connected.", "DB Status", connectMsgBoxButtons, connectMsgBoxicon);
        }
    }
}