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

        List<MarketStat> marketStats = new List<MarketStat>();

        public string baseAddress = "http://10.87.200.63:8080/PortfolioManagementSystemWeb/rest/";

        public PorfolioManagementSystemWindow()
        {
            InitializeComponent();
        }

        // Handler of Add Transaction Button
        private void AddTransaction(object sender, RoutedEventArgs e)
        {
            AddTransaction();
            tabCtrlPorfolioManagementSystem.SelectedIndex = 0;
            tabNewTransaction.Visibility = Visibility.Collapsed;
        }

        private void ClosingWindow(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult = true;
        }

        private void DisplayRowNumber(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        // Enable Compare button when multiple stocks are selected
        private void EnableButtons(object sender, SelectionChangedEventArgs e)
        {
            if(dataGridInvestments.SelectedItems.Count == 0)
            {
                btnAnalyse.IsEnabled = false;
                btnCompare.IsEnabled = false;
            }
            if(dataGridInvestments.SelectedItems.Count == 1)
            {
                btnAnalyse.IsEnabled = true;
                btnCompare.IsEnabled = false;

            }
            if(dataGridInvestments.SelectedItems.Count >= 2)
            {
                btnAnalyse.IsEnabled = false;
                btnCompare.IsEnabled = true;
            }
        }
        
        // Load Grid when opening First Time
        private void Load(object sender, RoutedEventArgs e)
        {
            //LoadInvestmentGrid("");
            LoadTransactionGrid();
            AddItemsToComboBox();
        }


        // Load Refreshed Grid when Show Portfolio tab is Selected
        private void TabSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tabCtrlPorfolioManagementSystem.SelectedIndex == 0)
            {
                //RefreshGrid();
            }
            if (tabCtrlPorfolioManagementSystem.SelectedIndex != 1 
                || tabCtrlPorfolioManagementSystem.SelectedIndex != 2
                || tabCtrlPorfolioManagementSystem.SelectedIndex != 4)
            {
                tabCompare.Visibility = Visibility.Collapsed;
                tabAnalyseStock.Visibility = Visibility.Collapsed;
                tabNewTransaction.Visibility = Visibility.Collapsed;
            }

        }

        private void OpenNewTransactionTab(object sender, RoutedEventArgs e)
        {
            
            tabCtrlPorfolioManagementSystem.SelectedIndex = 2;
            tabNewTransaction.Visibility = Visibility.Visible;

        }

        private void OpenAnalyseTab(object sender, RoutedEventArgs e)
        {
            tabCtrlPorfolioManagementSystem.SelectedIndex = 4;
            tabAnalyseStock.Visibility = Visibility.Visible;

            //DateTime dtp = new DateTime(1970, 1, 1, 0, 0, 0);
            //dtp.AddMilliseconds()

            LoadAnalyseTab();
        }

        // Compare Portfolio when Compare Button is Pressed
        private void OpenComparePortfolioTab(object sender, RoutedEventArgs e)
        {
            tabCtrlPorfolioManagementSystem.SelectedIndex = 4;
            tabCompare.Visibility = Visibility.Visible;


        }

        private void ChangePortfolio(object sender, SelectionChangedEventArgs e)
        {
            switch (comboBoxPortfolio.SelectedIndex)
            {
                case 0:
                    LoadInvestmentGrid("");
                    dataGridInvestments.Columns.Remove(clmPsector);
                    break;
                case 1:
                    LoadInvestmentGrid("/Finance");
                    break;
                case 2:
                    LoadInvestmentGrid("/Automobiles");
                    break;
                case 3:
                    LoadInvestmentGrid("/Information Technology");
                    break;
                default:
                    MessageBox.Show("Please Select Right choice");
                    break;
                    
            }
            switch (comboBoxPortfolio.SelectedIndex)
            {
                case 0:
                    dataGridInvestments.Columns.Insert(1, clmPsector);
                    break;
                default:
                    dataGridInvestments.Columns.Remove(clmPsector);
                    break;
            }
        }
    }

    public partial class PorfolioManagementSystemWindow
    {
        public void LoadAnalyseTab()
        {
            string ticker = ((Investment)dataGridInvestments.SelectedItem).ticker;
            MessageBox.Show(ticker);
            string jsonString = helper.DownloadJsonString(baseAddress + "stocks/analyze/" + ticker);
            Stream jsonStream = helper.GenerateStreamFromJsonString(jsonString);


            StockDetail stockDetail = helper.UnserializeObjectFromJsonStream<StockDetail>(jsonStream);
            foreach(MarketStat marketStat in stockDetail.marketList)
            {
                marketStat.Date = helper.DateTimeResolve(marketStat.date);
            }
            txtAveChange.Text = stockDetail.avgChange.ToString();
            txtLiquidity.Text = stockDetail.liquidity.ToString();
            txtVolatility.Text = stockDetail.volatility.ToString();

            dataGridMarketStats.ItemsSource = stockDetail.marketList;
        }
        
        private void LoadTransactionGrid()
        {
            LoadGrid<Transaction>
                (baseAddress + "transactions/all",
                transactions, dataGridTransaction);
        }

        private void LoadInvestmentGrid(string param)
        {
            LoadGrid<Investment>
                (baseAddress + "investments" + param,
                investments, dataGridInvestments);
        }

        public void AddItemsToComboBox()
        {
            comboBoxPortfolio.Items.Add("All");
            comboBoxPortfolio.Items.Add(Investment.Portfolio.Finance);
            comboBoxPortfolio.Items.Add(Investment.Portfolio.Automobiles);
            comboBoxPortfolio.Items.Add("Information Technology");
            comboBoxPortfolio.SelectedIndex = 0;

            
        }

        private void LoadGrid<T>(string uri, List<T> obj, DataGrid dataGrid)
        {
            string jsonString = helper.DownloadJsonString(uri);
            
            Stream jsonStream = helper.GenerateStreamFromJsonString(jsonString);
            obj = helper.UnserializeListObjectFromJsonStream<T>(jsonStream);

            dataGrid.ItemsSource = obj;
        }

        private void AddTransaction()
        {
            radioButtonBuy.IsChecked = true;
            Transaction.TransactionType transactionType;
            if (radioButtonBuy.IsChecked == true)
            {
                transactionType = Transaction.TransactionType.Buy;
            }
            else
            {
                transactionType = Transaction.TransactionType.Sell;
            }
            Transaction transaction = new Transaction(txtTicker.Text, 
                transactionType, "", 
                int.Parse(txtStockPrice.Text), int.Parse(txtNoOfUnits.Text));

            DateTime transactionDate = (DateTime)dateTransactionDate.SelectedDate;
            double date = double.Parse(transactionDate.ToString("yyyy-MM-dd HH:mm:ss"));

            MemoryStream mStream = helper.SerializeObjectToJsonStream(transaction);
            string jsonString = helper.GenerateJsonStringFromStream(mStream);
            string posttAddress = baseAddress + "transactions/new";
            helper.PostJsonData(posttAddress, jsonString);
            ResetAddTransactionForm();
            LoadInvestmentGrid("");
            LoadTransactionGrid();
            comboBoxPortfolio.SelectedIndex = 0;
        }

        private void ResetAddTransactionForm()
        {
            txtNoOfUnits.Text = "";
            txtStockPrice.Text = "";
            txtTicker.Text = "";
        }
    }
}

