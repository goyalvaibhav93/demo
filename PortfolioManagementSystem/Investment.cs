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
        public enum Portfolio { Finance, Automobiles, Information_Techonology}
        public Portfolio portfolio { get; set; }
        [DataMember]
        public string ticker { get; set; }
        [DataMember]
        public int investmentId { get; set;}
        [DataMember]
        public long buyDate { get; set; }
        [DataMember]
        public double buyPrice { get; set; }
        [DataMember]
        public int units { get; set; }
        //[DataMember]
        public double pAndL { get; set; }

        public Investment(int investmentId, string ticker, long buyDate, double buyPrice, int units)
        {
            this.ticker = ticker;
            this.investmentId = investmentId;
            this.buyDate = buyDate;
            this.buyPrice = buyPrice;
            this.units = units;
        }
    }
}
