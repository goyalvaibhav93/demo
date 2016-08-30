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

        // Object of Transactions
        List<Transaction> transactions = new List<Transaction>();
        
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


        // Handler of Add Transaction Button
        private void AddTransaction(object sender, RoutedEventArgs e)
        {
            AddTransaction();
        }

        private void ClosingWindow(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult = true;
        }

        private void DisplayRowNumber(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        
        private void AddItemsComboBox(object sender, RoutedEventArgs e)
        {
            AddItemsToComboBox(comboBoxTransactionType);
        }

        // Enable Compare button when multiple stocks are selected
        private void EnableCompare(object sender, SelectionChangedEventArgs e)
        {
            if(dataGridPortfolio.SelectedItems.Count >= 2)
            {
                btnCompare.IsEnabled = true;
            }
        }

        private void Delete(object sender, RoutedEventArgs e)
        {

        }

        // Compare Portfolio when Compare Button is Pressed
        private void ComparePortfolio(object sender, RoutedEventArgs e)
        {
            tabCompare.Visibility = Visibility.Visible;
            tabCtrlPorfolioManagementSystem.SelectedIndex = 1;
        }

        // Load Refreshed Grid when Show Portfolio tab is Selected
        private void RefreshGridPortFolioTabSelection(object sender, SelectionChangedEventArgs e)
        {
            if (tabCtrlPorfolioManagementSystem.SelectedIndex == 0)
            {
                //RefreshGrid();
            }
            if(tabCtrlPorfolioManagementSystem.SelectedIndex != 1)
            {
                tabCompare.Visibility = Visibility.Collapsed;
            }
        }

        // Load Grid when opening First Time
        private void LoadGrids(object sender, RoutedEventArgs e)
        {
            LoadGrid<Investment>
                ("http://10.87.198.148:8080/PortfolioManagementSystemWeb/rest/investments/current", 
                investments, dataGridPortfolio);
            LoadGrid<Transaction>("http://10.87.198.148:8080/PortfolioManagementSystemWeb/rest/sun32/transactions/all",
                transactions, dataGridTransaction);
        }
    }

    public partial class PorfolioManagementSystemWindow
    {
        private void LoadGrid<T>(string uri, List<T> obj, DataGrid dataGrid)
        {
            string jsonString = helper.DownloadJsonString(uri);
            
            Stream jsonStream = helper.GenerateStreamFromJsonString(jsonString);
            obj = helper.UnserializeListObjectFromJsonStream<T>(jsonStream);

            dataGrid.ItemsSource = obj;
        }

        private void AddTransaction()
        {
            Transaction transaction = new Transaction(txtTicker.Text, 
                (Transaction.TransactionType)comboBoxTransactionType.SelectedItem, "", 
                int.Parse(txtStockPrice.Text), int.Parse(txtNoOfUnits.Text));

            MemoryStream mStream = helper.SerializeObjectToJsonStream(transaction);
            string jsonString = helper.GenerateJsonStringFromStream(mStream);
            string baseAddress =
                "http://10.87.198.148:8080/PortfolioManagementSystemWeb/rest/sun32/transactions/new";
            helper.PostJsonData(baseAddress, jsonString);
            ResetAddTransactionFrom();
            LoadGrid<Transaction>("http://10.87.198.148:8080/PortfolioManagementSystemWeb/rest/sun32/transactions/all", transactions, dataGridTransaction);
        }

        private void AddItemsToComboBox(ComboBox comboBox)
        {
            comboBox.Items.Add(Transaction.TransactionType.Buy);
            comboBox.Items.Add(Transaction.TransactionType.Sell);
        }

        private void ResetAddTransactionFrom()
        {
            txtNoOfUnits.Text = "";
            txtStockPrice.Text = "";
            txtTicker.Text = "";
            comboBoxTransactionType.Text = "";
        }
    }
}

