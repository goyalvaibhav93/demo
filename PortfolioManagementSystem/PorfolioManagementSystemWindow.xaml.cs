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
using System.Windows.Controls.DataVisualization.Charting;
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

        string ticker;

        // Object of Transactions
        List<Transaction> transactions = new List<Transaction>();
        
        // Object of Stocks
        List<Stock> stocks = new List<Stock>();

        // Object of Investment
        public static List<Investment> investments = new List<Investment>();

        List<MarketStat> marketStats = new List<MarketStat>();

        public string baseAddress = "http://10.87.231.175:8080/PortfolioManagementSystemWeb/rest/";

        StockDetail stockDetailAnalyse;
        List<StockDetail> stocksDetailCompare;
        List<string> tickers;

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
                btnShowTransactionTicker.IsEnabled = false;
            }
            if(dataGridInvestments.SelectedItems.Count == 1)
            {
                btnShowTransactionTicker.IsEnabled = true;
               btnAnalyse.IsEnabled = true;
                btnCompare.IsEnabled = false;

            }
            if(dataGridInvestments.SelectedItems.Count >= 2)
            {
                btnShowTransactionTicker.IsEnabled = false;
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
                tabComparePortfolio.Visibility = Visibility.Collapsed;
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
        private void OpenCompareStockTab(object sender, RoutedEventArgs e)
        {
            tabCtrlPorfolioManagementSystem.SelectedIndex = 3;
            tabCompare.Visibility = Visibility.Visible;
            LoadCompareTab();
            
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
            foreach (MarketStat marketStat in stockDetailAnalyse.marketList)
            {
                graphPoints.Add(new KeyValuePair<DateTime, long>(marketStat.Date, marketStat.volumeTraded));
            }
            lineChartShowAnalyse.ItemsSource = graphPoints;
            lineChartShowAnalyse.Title = "Volume Traded";
        }

        private void ShowLowGraph(object sender, RoutedEventArgs e)
        {
            List<KeyValuePair<DateTime, double>> graphPoints = new List<KeyValuePair<DateTime, double>>();
            foreach (MarketStat marketStat in stockDetailAnalyse.marketList)
            {
                graphPoints.Add(new KeyValuePair<DateTime, double>(marketStat.Date, marketStat.low));
            }
            lineChartShowAnalyse.ItemsSource = graphPoints;
            lineChartShowAnalyse.Title = "Low Price";
        }

        private void ShowHighGraph(object sender, RoutedEventArgs e)
        {
            List<KeyValuePair<DateTime, double>> graphPoints = new List<KeyValuePair<DateTime, double>>();
            foreach (MarketStat marketStat in stockDetailAnalyse.marketList)
            {
                graphPoints.Add(new KeyValuePair<DateTime, double>(marketStat.Date, marketStat.high));
            }
            lineChartShowAnalyse.ItemsSource = graphPoints;
            lineChartShowAnalyse.Title = "High Price";
        }

        private void ShowCloseGraph(object sender, RoutedEventArgs e)
        {
            List<KeyValuePair<DateTime, double>> graphPoints = new List<KeyValuePair<DateTime, double>>();
            foreach (MarketStat marketStat in stockDetailAnalyse.marketList)
            {
                graphPoints.Add(new KeyValuePair<DateTime, double>(marketStat.Date, marketStat.close));
            }
            lineChartShowAnalyse.ItemsSource = graphPoints;
            lineChartShowAnalyse.Title = "Close Price";
            lineChartCompare.Title = ticker;
        }

        private void ShowOpenGraph(object sender, RoutedEventArgs e)
        {
            List<KeyValuePair<DateTime, double>> graphPoints = new List<KeyValuePair<DateTime, double>>();
            foreach (MarketStat marketStat in stockDetailAnalyse.marketList)
            {
                graphPoints.Add(new KeyValuePair<DateTime, double>(marketStat.Date, marketStat.open));
            }
            lineChartShowAnalyse.ItemsSource = graphPoints;
            lineChartShowAnalyse.Title = "Open Price";
        }

        private void ShowTransactionsBetweenDates(object sender, RoutedEventArgs e)
        {
            DateTime referenceDate = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            DateTime fromDate = (DateTime)dateFromDate.SelectedDate;
            DateTime toDate = (DateTime)dateToDate.SelectedDate;
            //MessageBox.Show((fromDate - referenceDate).TotalMilliseconds + "");
            transactions = LoadGrid<Transaction>
                (baseAddress + "transactions/dates?start=" +(fromDate-referenceDate).TotalMilliseconds 
                +"&end="+(toDate-referenceDate).TotalMilliseconds);
            foreach (Transaction tr in transactions)
            {
                
                tr.TransactionDate = helper.DateTimeResolve(tr.date);
                tr.Date = tr.TransactionDate.ToShortDateString();
            }
            dataGridTransaction.ItemsSource = transactions;
        }

        private void ShowCompareCloseGraph(object sender, RoutedEventArgs e)
        {
            double min = 5000, max = 5;
            List<KeyValuePair<DateTime, double>> graphPoints = new List<KeyValuePair<DateTime, double>>();
            foreach (MarketStat marketStat in stocksDetailCompare[1].marketList)
            {
                if (max < marketStat.close)
                {
                    max = marketStat.close;
                }
                if (min > marketStat.close)
                {
                    min = marketStat.close;
                }
                marketStat.Date = helper.DateTimeResolve(marketStat.date);
                graphPoints.Add(new KeyValuePair<DateTime, double>(marketStat.Date, marketStat.close));
            }
            lineChart2.ItemsSource = graphPoints;
            lineChart2.Title = tickers[1];
            graphPoints = new List<KeyValuePair<DateTime, double>>();
            foreach (MarketStat marketStat in stocksDetailCompare[0].marketList)
            {

                marketStat.Date = helper.DateTimeResolve(marketStat.date);
                if (max < marketStat.close)
                {
                    max = marketStat.close;
                }
                if (min > marketStat.close)
                {
                    min = marketStat.close;
                }
                graphPoints.Add(new KeyValuePair<DateTime, double>(marketStat.Date, marketStat.close));
            }
            lineChartCompareY.Minimum = (min-5 > 0)? min - 5:0;
            lineChartCompareY.Maximum = max + 5;
            lineChart1.ItemsSource = graphPoints;
            lineChart1.Title = tickers[0];

        }

        private void ShowCompareOpenGraph(object sender, RoutedEventArgs e)
        {

            double min = 5000, max = 5;
            List<KeyValuePair<DateTime, double>> graphPoints = new List<KeyValuePair<DateTime, double>>();
            foreach (MarketStat marketStat in stocksDetailCompare[1].marketList)
            {
                marketStat.Date = helper.DateTimeResolve(marketStat.date);
                if (max < marketStat.open)
                {
                    max = marketStat.open;
                }
                if (min > marketStat.open)
                {
                    min = marketStat.open;
                }
                graphPoints.Add(new KeyValuePair<DateTime, double>(marketStat.Date, marketStat.open));
            }
            lineChart2.ItemsSource = graphPoints;
            lineChart2.Title = tickers[1];

            graphPoints = new List<KeyValuePair<DateTime, double>>();
            foreach (MarketStat marketStat in stocksDetailCompare[0].marketList)
            {

                marketStat.Date = helper.DateTimeResolve(marketStat.date);
                if (max < marketStat.open)
                {
                    max = marketStat.open;
                }
                if (min > marketStat.open)
                {
                    min = marketStat.open;
                }
                graphPoints.Add(new KeyValuePair<DateTime, double>(marketStat.Date, marketStat.open));
            }
            lineChartCompareY.Minimum = (min-5 > 0)? min - 5:0;
            lineChartCompareY.Maximum = max + 5;
            lineChart1.ItemsSource = graphPoints;
            lineChart1.Title = tickers[0];
        }

        private void ShowCompareHighGraph(object sender, RoutedEventArgs e)
        {
            double min = 5000, max = 5;
            //MessageBox.Show(tickers[0] + stocksDetailCompare[0].volatility);
            List<KeyValuePair<DateTime, double>> graphPoints = new List<KeyValuePair<DateTime, double>>();
            foreach (MarketStat marketStat in stocksDetailCompare[1].marketList)
            {
                marketStat.Date = helper.DateTimeResolve(marketStat.date);
                if (max < marketStat.high)
                {
                    max = marketStat.high;
                }
                if (min > marketStat.high)
                {
                    min = marketStat.high;
                }
                graphPoints.Add(new KeyValuePair<DateTime, double>(marketStat.Date, marketStat.high));
            }
            lineChart2.ItemsSource = graphPoints;
            lineChart2.Title = tickers[1];
            graphPoints = new List<KeyValuePair<DateTime, double>>();
            foreach (MarketStat marketStat in stocksDetailCompare[0].marketList)
            {

                marketStat.Date = helper.DateTimeResolve(marketStat.date);
                marketStat.Date = helper.DateTimeResolve(marketStat.date);
                if (max < marketStat.high)
                {
                    max = marketStat.high;
                }
                if (min > marketStat.high)
                {
                    min = marketStat.high;
                }
                graphPoints.Add(new KeyValuePair<DateTime, double>(marketStat.Date, marketStat.high));
            }
            lineChartCompareY.Minimum = (min-5 > 0)? min - 5:0;
            lineChartCompareY.Maximum = max + 5;
            lineChart1.ItemsSource = graphPoints;
            lineChart1.Title = tickers[0];
        }

        private void ShowCompareLowGraph(object sender, RoutedEventArgs e)
        {
            double min = 5000, max = 5;
            //MessageBox.Show(tickers[0] + stocksDetailCompare[0].volatility);
            List<KeyValuePair<DateTime, double>> graphPoints = new List<KeyValuePair<DateTime, double>>();
            foreach (MarketStat marketStat in stocksDetailCompare[1].marketList)
            {
                marketStat.Date = helper.DateTimeResolve(marketStat.date);
                if(max < marketStat.low)
                {
                    max = marketStat.low;
                }
                if(min > marketStat.low)
                {
                    min = marketStat.low;
                }
                graphPoints.Add(new KeyValuePair<DateTime, double>(marketStat.Date, marketStat.low));
            }
            lineChart2.ItemsSource = graphPoints;
            lineChart2.Title = tickers[1];
            graphPoints = new List<KeyValuePair<DateTime, double>>();
            foreach (MarketStat marketStat in stocksDetailCompare[0].marketList)
            {

                marketStat.Date = helper.DateTimeResolve(marketStat.date);

                marketStat.Date = helper.DateTimeResolve(marketStat.date);
                if (max < marketStat.low)
                {
                    max = marketStat.low;
                }
                if (min > marketStat.low)
                {
                    min = marketStat.low;
                }

                graphPoints.Add(new KeyValuePair<DateTime, double>(marketStat.Date, marketStat.low));
            }
            lineChartCompareY.Minimum = (min-5 > 0)? min - 5:0;
            lineChartCompareY.Maximum = max+5;
            lineChart1.ItemsSource = graphPoints;
            lineChart1.Title = tickers[0];
        }

        private void ShowCompareVolumeGraph(object sender, RoutedEventArgs e)
        {
            long max = 500000;
            //MessageBox.Show(tickers[0] + stocksDetailCompare[0].volatility);
            List<KeyValuePair<DateTime, double>> graphPoints = new List<KeyValuePair<DateTime, double>>();
            foreach (MarketStat marketStat in stocksDetailCompare[1].marketList)
            {
                marketStat.Date = helper.DateTimeResolve(marketStat.date);
                //marketStat.Date = helper.DateTimeResolve(marketStat.date);
                //MessageBox.Show(marketStat.volumeTraded + "" + marketStat.Date);
                if(marketStat.volumeTraded > max)
                {
                    max = marketStat.volumeTraded;
                }
                graphPoints.Add(new KeyValuePair<DateTime, double>(marketStat.Date, marketStat.volumeTraded));
            }
            lineChart2.ItemsSource = graphPoints;
            lineChart2.Title = tickers[1];
            graphPoints = new List<KeyValuePair<DateTime, double>>();
            foreach (MarketStat marketStat in stocksDetailCompare[0].marketList)
            {

                marketStat.Date = helper.DateTimeResolve(marketStat.date);
                //MessageBox.Show(marketStat.volumeTraded + "" + marketStat.Date);
                if (marketStat.volumeTraded > max)
                {
                    max = marketStat.volumeTraded;
                }
                graphPoints.Add(new KeyValuePair<DateTime, double>(marketStat.Date, marketStat.volumeTraded));
            }
            lineChartCompareY.Minimum = 0;
            lineChartCompareY.Maximum = max+1000;
            //MessageBox.Show(min + " " + max);
            lineChart1.ItemsSource = graphPoints;
            lineChart1.Title = tickers[0];
        }

        private void ShowTransactionTicker(object sender, RoutedEventArgs e)
        {
            Investment investment = (Investment)dataGridInvestments.SelectedItem;
            txtTickerName.Text = investment.ticker;
            transactions = LoadGrid<Transaction>
               (baseAddress + "transactions/"+investment.ticker);
            foreach (Transaction tr in transactions)
            {
                tr.TransactionDate = helper.DateTimeResolve(tr.date);
                tr.Date = tr.TransactionDate.ToShortDateString();
            }
            
            dataGridTransaction.Columns.Remove(clmTicker);
            dataGridTransaction.ItemsSource = transactions;
            btnShowAllTransaction.IsEnabled = true;
            tabCtrlPorfolioManagementSystem.SelectedIndex = 1;
        }

        
        private void ShowAllTransaction(object sender, RoutedEventArgs e)
        {
            btnShowAllTransaction.IsEnabled = false;
            LoadTransactionGrid();
            dataGridTransaction.Columns.Insert(0, clmTicker);
        }

        private void OpenComparePortfolioTab(object sender, RoutedEventArgs e)
        {
            tabCtrlPorfolioManagementSystem.SelectedIndex = 5;
            tabComparePortfolio.Visibility = Visibility.Visible;
            LoadComparePortfolioTab();

        }

        private void CloseAnalyseTab(object sender, RoutedEventArgs e)
        {
            tabCtrlPorfolioManagementSystem.SelectedIndex = 0;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ResetAddTransactionForm();
            tabCtrlPorfolioManagementSystem.SelectedIndex = 0;
        }
    }


    public partial class PorfolioManagementSystemWindow
    {
        public void LoadComparePortfolioTab()
        {
            List<double> list = new List<double>();
            string jsonString = helper.DownloadJsonString(baseAddress + "portfolios/compare");
            Stream stream = helper.GenerateStreamFromJsonString(jsonString);
            list = helper.UnserializeListObjectFromJsonStream<double>(stream);

            list[1] *= 100;
            list[3] *= 100;
            list[5] *= 100;
            List<KeyValuePair<string, double>> graphPoints = new List<KeyValuePair<string, double>>();
            graphPoints.Add(new KeyValuePair<string, double>("Automobiles", list[3]));
            graphPoints.Add(new KeyValuePair<string, double>("Finance", list[1]));
            graphPoints.Add(new KeyValuePair<string, double>("Information Technology", list[5]));

            double min = -5, max = 50;

            min = (min < Minimum(list[1], list[3], list[5]))? min:  Minimum(list[1], list[3], list[5]) -5;
            max = (max > Maximum(list[1], list[3], list[5]))? max:  Maximum(list[1], list[3], list[5]) +5;

            barChartPortfolio.ItemsSource = graphPoints;
            barChartPortfolioY.Minimum = min;
            barChartPortfolioY.Maximum = max;
        }

        public void LoadCompareTab()
        {
            stocksDetailCompare = new List<StockDetail>();
            tickers = new List<string>();
            foreach (Investment investment in dataGridInvestments.SelectedItems)
            {
                tickers.Add(investment.ticker);
            }
            string jsonString = helper.DownloadJsonString(baseAddress + "stocks/compare?ticker1=" + tickers[0] + "&ticker2=" + tickers[1]);
            //MessageBox.Show(jsonString);
            Stream jsonStream = helper.GenerateStreamFromJsonString(jsonString);
            stocksDetailCompare = helper.UnserializeListObjectFromJsonStream<StockDetail>(jsonStream);

            List<StockProperties> stockProperties = new List<StockProperties>();
            stockProperties.Add(new StockProperties(tickers[0], stocksDetailCompare[0].avgChange, 
                stocksDetailCompare[0].volatility, stocksDetailCompare[0].liquidity));
            stockProperties.Add(new StockProperties(tickers[1], stocksDetailCompare[1].avgChange,
                stocksDetailCompare[1].volatility, stocksDetailCompare[1].liquidity));
            dataGridCompare.ItemsSource = stockProperties;
            radioCloseCompare.IsChecked = true;
            //MessageBox.Show(stocksDetailCompare[0].volatility);
            //MessageBox.Show(tickers[0] + " " + tickers[1]);
        }

        public void LoadAnalyseTab()
        {
            ticker = ((Investment)dataGridInvestments.SelectedItem).ticker;
            //MessageBox.Show(ticker);
            lineChartAnalyse.Title = ticker;
            string jsonString = helper.DownloadJsonString(baseAddress + "stocks/analyze/" + ticker);
            Stream jsonStream = helper.GenerateStreamFromJsonString(jsonString);

            stockDetailAnalyse = helper.UnserializeObjectFromJsonStream<StockDetail>(jsonStream);
            int i = 0;
            foreach(MarketStat marketStat in stockDetailAnalyse.marketList)
            {
                marketStat.Date = helper.DateTimeResolve(marketStat.date);
                marketStat.dateString = marketStat.Date.ToShortDateString();
            }

            txtAveChangeAnalyse.Text = stockDetailAnalyse.avgChange.ToString();
            txtLiquidityAnalyse.Text = stockDetailAnalyse.liquidity.ToString();
            txtVolatilityAnalyse.Text = stockDetailAnalyse.volatility.ToString();
            radioCloseAnalyse.IsChecked = true;
            
            dataGridMarketStatsAnalyse.ItemsSource = stockDetailAnalyse.marketList;
        }
        
        private void LoadTransactionGrid()
        {
            txtTickerName.Text = "All";
            transactions = LoadGrid<Transaction>
                (baseAddress + "transactions/all");
            foreach(Transaction tr in transactions)
            {
                tr.TransactionDate = helper.DateTimeResolve(tr.date);
                tr.Date = tr.TransactionDate.ToShortDateString();
            }
            dataGridTransaction.ItemsSource = transactions;
        }

        private void LoadInvestmentGrid(string param)
        {
            investments = LoadGrid<Investment>
                (baseAddress + "investments" + param);
            foreach(Investment investment in investments)
            {
                if(investment.profit < 0)
                {
                    investment.color = "LOSS";
                }
                else
                {
                    investment.color = "PROFIT";
                }
            }
            //MessageBox.Show(investments[0].color+"");
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
            //MessageBox.Show(jsonString);
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

        public double Minimum(double a, double b, double c)
        {
            return ((a < b) ? a : b) < c ? ((a < b) ? a : b) : c;
        }

        public double Maximum(double a, double b, double c)
        {
            return ((a > b) ? a : b) > c ? ((a > b) ? a : b) : c;
        }
    }
}

