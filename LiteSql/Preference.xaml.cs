using LiteSql.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public SettingsData settingsData { get; set; }

        public Preference()
        {
            InitializeComponent();
            settingsData = new SettingsData();
        }
        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            try
            {
                settingsData.decryptKey = decryptKeyText.Text;
                settingsData.cipherCompatibility = Int32.Parse(cipherCompatibilityText.Text);
                settingsData.cipherDefaultKdfIter = Int32.Parse(cipherDefaultKdfIterText.Text);
                settingsData.cipherDdefaultPageSize = Int32.Parse(cipherDefaultPageSizeText.Text);
                settingsData.cipherDdefaultPageSize = Int32.Parse(cipherDefaultPageSizeText.Text);
                settingsData.cipherDefaultHmacAlgorithm = cipherDefaultHmacAlgorithmText.Text;
                settingsData.cipherDefaultKdfAlgorithm = cipherDefaultKdfAlgorithmText.Text;
                MessageBox.Show("Preferences are saved", "Information", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please check format of relevent setting.", "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                settingsData.setDefault();
            }
        }
    }
}
