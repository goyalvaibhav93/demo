using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
namespace PortfolioManagementSystem
{
    [DataContract]
    class StockSummery
    {
        [DataMember]
        public string StockId { get; set; }
        [DataMember]
        public string CompanyName { get; set; }
        [DataMember]
        public double Price { get; set; }
        [DataMember]
        public double PL { get; set; }
        [DataMember]
        public double ConfidanceValue { get; set; }

        public StockSummery(string stockID, string companyName, double price, double pl, double confidanceValue) 
        {
            StockId = stockID;
            CompanyName = companyName;
            Price = price;
            PL = pl;
            ConfidanceValue = confidanceValue;
        }
    }

}
