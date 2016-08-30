using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Json;

namespace PortfolioManagementSystem
{
    /// <summary>
    /// Interaction logic for TestConnection.xaml
    /// </summary>
    public partial class TestConnection : Window
    {
        public TestConnection()
        {
            InitializeComponent();
        }

        private void CheckConnection(object sender, RoutedEventArgs e)
        {
            WebClient client = new WebClient();
            string jsonString = client.DownloadString("http://10.87.201.85:8080/PortfolioManagementSystemWeb/rest/performanceSummary");

            txtShowData.Text = jsonString;

            List<Product> products;
            Stream jsonStream = GenerateStreamFromString(jsonString);

            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<Product>));

            products = (List<Product>)serializer.ReadObject(jsonStream);

            string showContect = "";
            foreach (Product product in products)
            {
                showContect += product.ToString() + "\n";

            }
            txtShowData.Text = showContect;
            //DialogResult = true;

        }

        public Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
