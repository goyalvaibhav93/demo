using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManagementSystem
{
    [DataContract]
    public class Investment
    {
        public string profitString { get; set; }
        public enum Portfolio { Finance, Automobiles, Information_Techonology}
        [DataMember]
        public string portfolio { get; set; }
        [DataMember]
        public string ticker { get; set; }
        [DataMember]
        public int investmentId { get; set;}
        [DataMember]
        public long buyDate { get; set; }
        [DataMember]
        public double investmentCost { get; set; }
        [DataMember]
        public int units { get; set; }
        [DataMember]
        public double profit { get; set; }

        public string color { get; set; }

        public Investment(int investmentId, string ticker, long buyDate, double buyPrice, int units)
        {
            this.ticker = ticker;
            this.investmentId = investmentId;
            this.buyDate = buyDate;
            this.investmentCost = buyPrice;
            this.units = units;
        }
    }
}
