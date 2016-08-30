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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PortfolioManagementSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CheckConnection(object sender, RoutedEventArgs e)
        {
            TestConnection testConnection = new TestConnection();
            bool? result = testConnection.ShowDialog();
            
            if(result == true)
            {
                MessageBox.Show("Ho gya");
            }
        }

        private void OpenPortfolioManagementSystem(object sender, RoutedEventArgs e)
        {
            PorfolioManagementSystemWindow portfolioMananagementSystemWindow = new PorfolioManagementSystemWindow();
            //TextBox txtThank = new TextBox();
            //txtThank.Text = "Welcome To Portfolio Management System";
            //txtThank.TextAlignment = TextAlignment.Center;
            this.Hide();
            bool? result = portfolioMananagementSystemWindow.ShowDialog();
            this.Show();
        }
    }
}
