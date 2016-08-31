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

        static StockDetail stockDetail;

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

            LoadAnalyseTab();
        }

        // Compare Portfolio when Compare Button is Pressed
        private void OpenComparePortfolioTab(object sender, RoutedEventArgs e)
        {
            tabCtrlPorfolioManagementSystem.SelectedIndex = 3;
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

        private void ShowVolumeGraph(object sender, RoutedEventArgs e)
        {
            List<KeyValuePair<DateTime, long>> graphPoints = new List<KeyValuePair<DateTime, long>>();
            foreach (MarketStat marketStat in stockDetail.marketList)
            {
                graphPoints.Add(new KeyValuePair<DateTime, long>(marketStat.Date, marketStat.volumeTraded));
            }
            lineChartClose.ItemsSource = graphPoints;
            lineChartClose.Title = "Volume Traded";
        }

        private void ShowLowGraph(object sender, RoutedEventArgs e)
        {
            List<KeyValuePair<DateTime, double>> graphPoints = new List<KeyValuePair<DateTime, double>>();
            foreach (MarketStat marketStat in stockDetail.marketList)
            {
                graphPoints.Add(new KeyValuePair<DateTime, double>(marketStat.Date, marketStat.low));
            }
            lineChartClose.ItemsSource = graphPoints;
            lineChartClose.Title = "Low Price";
        }

        private void ShowHighGraph(object sender, RoutedEventArgs e)
        {
            List<KeyValuePair<DateTime, double>> graphPoints = new List<KeyValuePair<DateTime, double>>();
            foreach (MarketStat marketStat in stockDetail.marketList)
            {
                graphPoints.Add(new KeyValuePair<DateTime, double>(marketStat.Date, marketStat.high));
            }
            lineChartClose.ItemsSource = graphPoints;
            lineChartClose.Title = "High Price";
        }

        private void ShowCloseGraph(object sender, RoutedEventArgs e)
        {
            List<KeyValuePair<DateTime, double>> graphPoints = new List<KeyValuePair<DateTime, double>>();
            foreach (MarketStat marketStat in stockDetail.marketList)
            {
                graphPoints.Add(new KeyValuePair<DateTime, double>(marketStat.Date, marketStat.close));
            }
            lineChartClose.ItemsSource = graphPoints;
            lineChartClose.Title = "Close Price";

        }

        private void ShowOpenGraph(object sender, RoutedEventArgs e)
        {
            List<KeyValuePair<DateTime, double>> graphPoints = new List<KeyValuePair<DateTime, double>>();
            foreach (MarketStat marketStat in stockDetail.marketList)
            {
                graphPoints.Add(new KeyValuePair<DateTime, double>(marketStat.Date, marketStat.open));
            }
            lineChartClose.ItemsSource = graphPoints;
            lineChartClose.Title = "Open Price";
        }

        private void ShowTransactionsBetweenDates(object sender, RoutedEventArgs e)
        {
            DateTime referenceDate = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            DateTime fromDate = (DateTime)dateFromDate.SelectedDate;
            DateTime toDate = (DateTime)dateToDate.SelectedDate;
            MessageBox.Show((fromDate - referenceDate).TotalMilliseconds + "");
            transactions = LoadGrid<Transaction>
                (baseAddress + "transactions/dates?start=" +(fromDate-referenceDate).TotalMilliseconds 
                +"&end="+(toDate-referenceDate).TotalMilliseconds);
            dataGridTransaction.Columns.Remove(clmDate);
            dataGridTransaction.ItemsSource = transactions;
        }
    }

    public partial class PorfolioManagementSystemWindow
    {
        public void LoadAnalyseTab()
        {
            string ticker = ((Investment)dataGridInvestments.SelectedItem).ticker;
            //MessageBox.Show(ticker);
            string jsonString = helper.DownloadJsonString(baseAddress + "stocks/analyze/" + ticker);
            Stream jsonStream = helper.GenerateStreamFromJsonString(jsonString);

            

            stockDetail = helper.UnserializeObjectFromJsonStream<StockDetail>(jsonStream);
            foreach(MarketStat marketStat in stockDetail.marketList)
            {
                marketStat.Date = helper.DateTimeResolve(marketStat.date);
            }

            txtAveChange.Text = stockDetail.avgChange.ToString();
            txtLiquidity.Text = stockDetail.liquidity.ToString();
            txtVolatility.Text = stockDetail.volatility.ToString();
            radioClose.IsChecked = true;
            dataGridMarketStats.ItemsSource = stockDetail.marketList;
        }
        
        private void LoadTransactionGrid()
        {
            transactions = LoadGrid<Transaction>
                (baseAddress + "transactions/all");
            foreach(Transaction tr in transactions)
            {
                tr.TransactionDate = helper.DateTimeResolve(tr.date);
            }
            dataGridTransaction.ItemsSource = transactions;
        }

        private void LoadInvestmentGrid(string param)
        {
            investments = LoadGrid<Investment>
                (baseAddress + "investments" + param);
            dataGridInvestments.ItemsSource = investments;
        }

        public void AddItemsToComboBox()
        {
            comboBoxPortfolio.Items.Add("All");
            comboBoxPortfolio.Items.Add(Investment.Portfolio.Finance);
            comboBoxPortfolio.Items.Add(Investment.Portfolio.Automobiles);
            comboBoxPortfolio.Items.Add("Information Technology");
            comboBoxPortfolio.SelectedIndex = 0;

            
        }

        private List<T> LoadGrid<T>(string uri)
        {

            List<T> obj;
            string jsonString = helper.DownloadJsonString(uri);
            MessageBox.Show(jsonString);
            Stream jsonStream = helper.GenerateStreamFromJsonString(jsonString);
            obj = helper.UnserializeListObjectFromJsonStream<T>(jsonStream);
            return obj;
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
            DateTime transactionDate = (DateTime)dateTransactionDate.SelectedDate;

            Transaction transaction = new Transaction(txtTicker.Text, 
                transactionType, transactionDate.ToString("yyyy-MM-dd HH:mm:ss"), 
                int.Parse(txtStockPrice.Text), int.Parse(txtNoOfUnits.Text));

            

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

