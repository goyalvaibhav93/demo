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

namespace PortfolioManagementSystem
{
    /// <summary>
    /// Interaction logic for ConfirmAddTransaction.xaml
    /// </summary>
    public partial class ConfirmAddTransaction : Window
    {
        public ConfirmAddTransaction()
        {
            InitializeComponent();
        }
        public ConfirmAddTransaction(string ticker, Transaction.TransactionType tType, double stockPrice, int units, DateTime date)
        {
            InitializeComponent();
            txtContent.Text = "Ticker " + ticker + "\n" + "Transaction Type " + tType.ToString()+"\n" + "Stock Price " +
                stockPrice + "\n" + "Units " + units + "\n" + "Transaction Date " + date.ToShortDateString();
            
        }

        private void btnYes_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }

        private void btnNo_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

    }
}
