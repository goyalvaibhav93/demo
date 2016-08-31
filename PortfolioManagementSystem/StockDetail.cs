using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManagementSystem
{
    [DataContract]
    public class StockDetail
    {
        [DataMember]
        public double avgChange { get; set; }
        [DataMember]
        public string volatility { get; set; }
        [DataMember]
        public string liquidity{ get; set; }
        [DataMember]
        public List<MarketStat> marketList { get; set; }

        public StockDetail()
        {

        }
    }
}
