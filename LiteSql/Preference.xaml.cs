using LiteSql.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LiteSql
{
    /// <summary>
    /// Interaction logic for Preference.xaml
    /// </summary>
    public partial class Preference : Window
    {
        public PreferenceData preferenceData { get; set; }

        public Preference()
        {
            InitializeComponent();
            preferenceData = new PreferenceData();
        }
        private void prefSaveButton_Click(object sender, RoutedEventArgs e)
        {
            preferenceData.decryptKey = decryptKeyText.Text;
            if (MessageBox.Show("Preference data is saved. Do you want to close this window?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }
    }
}
