using System;
using System.Collections.Generic;
using System.IO;
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

namespace PortfolioManagementSystem
{
    /// <summary>
    /// Interaction logic for AddTransactionWindow.xaml
    /// </summary>
    /// 
    
    public partial class AddTransactionWindow : Window
    {
        HelperClass helper = new HelperClass();
        string baseAddress;
        public AddTransactionWindow(string baseAddress)
        {
            InitializeComponent();
            this.baseAddress = baseAddress;
        }

        private void AddTransaction(object sender, RoutedEventArgs e)
        {
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
            int n;
            double m;
            int result = DateTime.Compare(transactionDate, DateTime.Now.AddDays(1));
            if(result <= 0 && int.TryParse(txtNoOfUnits.Text, out n) && n>0 && double.TryParse(txtStockPrice.Text, out m) &&
                (radioButtonBuy.IsChecked == true || radioButtonSell.IsChecked== true))
            {
                Transaction transaction = new Transaction(txtTicker.Text,
                transactionType, transactionDate.ToString("yyyy-MM-dd HH:mm:ss"),
                double.Parse(txtStockPrice.Text), int.Parse(txtNoOfUnits.Text));

                ConfirmAddTransaction confirm = new ConfirmAddTransaction(txtTicker.Text,
                    transactionType, int.Parse(txtStockPrice.Text), int.Parse(txtNoOfUnits.Text), transactionDate);
                bool? result1 = confirm.ShowDialog();
                if (result1 == true)
                {

                    MemoryStream mStream = helper.SerializeObjectToJsonStream(transaction);
                    string jsonString = helper.GenerateJsonStringFromStream(mStream);
                    string posttAddress = baseAddress + "transactions/new";
                    helper.PostJsonData(posttAddress, jsonString);
                    ResetAddTransactionForm();
                    DialogResult = true;
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Enter Correct Values!");
                ResetAddTransactionForm();

            }
        }
        private void ResetAddTransactionForm()
        {
            radioButtonBuy.IsChecked = false;
            radioButtonSell.IsChecked = false;
            txtNoOfUnits.Text = "";
            txtStockPrice.Text = "";
            txtTicker.Text = "";
            dateTransactionDate.SelectedDate = null;
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }
    }
}
