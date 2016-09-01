using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManagementSystem
{
    [DataContract]
    public class Transaction
    {
        public enum TransactionType
        {
            Sell, Buy
        }
        [DataMember]
        public TransactionType buySell { get; set; }
        [DataMember]
        public string ticker { get; set; }
        public DateTime TransactionDate { get; set; }
        [DataMember]
        public double date { get; set; }
        [DataMember]
        public string dateTime { get; set; }
        [DataMember]
        public double price { get; set; }
        [DataMember]
        public int units { get; set; }
        public string Date { get; set; }

        // Constructor
        public Transaction(string ticker, TransactionType transactionType, string dateTime, double stockPrice, int units)
        {
            this.ticker = ticker;
            this.buySell = transactionType;
            this.dateTime = dateTime;
            this.price = stockPrice;
            this.units = units;

        }

    }
}
