using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
namespace PortfolioManagementSystem
{
    [DataContract]
    public class Stock
    {
        public enum Combo { One, Two, Three}

        public Combo comboValue { get; set; }
        [DataMember]
        public string ticker { get; set; }
        [DataMember]
        public int shareId { get; set; }
        //[DataMember]
        public double currentPrice { get; set; }
        //[DataMember]
        public double confidanceValue { get; set; }
        //[DataMember]
        public double openPrice { get; set; }
        //[DataMember]
        public double closePrice { get; set; }
        //[DataMember]
        public int volume { get; set; }

        // Custructor

        public Stock(string ticker, int shareId) : this(ticker, shareId, 0, 0, 0, 0, 0)
        { }
        public Stock(string ticker, int shareId, double currentPrice, 
            double confidanceValue, double openPrice, double closePrice, int volume)
        {
            this.ticker = ticker;
            this.shareId = shareId;
            this.currentPrice = currentPrice;
            this.openPrice = openPrice;
            this.closePrice = closePrice;
            this.confidanceValue = confidanceValue;
            this.volume = volume;
        }
    }
}
