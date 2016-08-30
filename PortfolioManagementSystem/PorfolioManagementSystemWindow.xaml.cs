using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
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

namespace PortfolioManagementSystem
{
    /// <summary>
    /// Interaction logic for PorfolioManagementSystemWindow.xaml
    /// </summary>
    public partial class PorfolioManagementSystemWindow : Window
    {
        // Object of helper class
        HelperClass helper = new HelperClass();
        
        
        // Object of Stocks
        static List<Stock> stocks = new List<Stock>();

        // Object of Investment
        public static List<Investment> investments = new List<Investment>();

        public PorfolioManagementSystemWindow()
        {
            InitializeComponent();
        }


        
        // Handler of Opening Add/Delete User Tab Button
        private void OpenAddUserTab(object sender, RoutedEventArgs e)
        {
            tabCtrlPorfolioManagementSystem.SelectedIndex = 2;
            //DialogResult = true;
        }


        // Handler of Add User Button
        private void AddUser(object sender, RoutedEventArgs e)
        {
            //Stock newStock = new Stock(txtTicker.Text, int.Parse(txtShareId.Text));
            //stocks.Add(newStock);

            //Investment investment = new Investment(123, "AAPL", 38384998, 78, 50);
            Investment investment =
                new Investment(123, txtTicker.Text, long.Parse(txtBuyDate.Text), int.Parse(txtBuyPrice.Text), int.Parse(txtNoOfUnits.Text));
            MemoryStream mStream = helper.SerializeObjectToJsonStream(investment);
            string jsonString = helper.GenerateJsonStringFromStream(mStream);
            string baseAddress =
                "http://10.87.207.27:8080/PortfolioManagementSystemWeb/rest/posttest";
            helper.PostJsonData(baseAddress, jsonString);
        }

        private void RefreshGrid(object sender, RoutedEventArgs e)
        {
           
            //string jsonString = helper.DownloadJsonString
            //    ("http://10.87.200.248:8080/PortfolioManagementSystemWeb/rest/performanceSummary");
            
            string jsonString = helper.DownloadJsonString
                ("http://10.87.207.27:8080/PortfolioManagementSystemWeb/rest/investments/current");

            //MessageBox.Show(jsonString1);
            Stream jsonStream = helper.GenerateStreamFromJsonString(jsonString);
            //MessageBox.Show(jsonStream.ToString());
            //stocks = helper.UnserializeObjectFromJsonStream<Stock>(jsonStream);
            investments = helper.UnserializeListObjectFromJsonStream<Investment>(jsonStream);
            //investments.Add(new Investment(stocks[0], 110, DateTime.Now, 0, 0, 0));
           
            dataGridMyStocks.ItemsSource = investments;
        }

        private void ClosingWindow(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult = true;
        }

        private void DisplayRowNumber(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void AddItemsToComboBox(ComboBox comboBox)
        {
            comboBox.Items.Add(Portfolio.PortfolioType.Automobile);
            comboBox.Items.Add(Portfolio.PortfolioType.Finance);
            comboBox.Items.Add(Portfolio.PortfolioType.IT);
        }

        private void AddItemsComboBox(object sender, RoutedEventArgs e)
        {
            AddItemsToComboBox(comboBoxShowPortfolioType);
            AddItemsToComboBox(comboBoxPortfolioType);
        }

        private void Modify(object sender, RoutedEventArgs e)
        {

        }

        private void EnableModify(object sender, SelectionChangedEventArgs e)
        {
            btnModify.IsEnabled = true;
            btnDelete.IsEnabled = true;
        }

        private void Enable(object sender, RoutedEventArgs e)
        {
            btnModify.IsEnabled = true;
            btnDelete.IsEnabled = true;
        }

        private void Delete(object sender, RoutedEventArgs e)
        {

        }
    }
}
