using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManagementSystem
{
    [DataContract]
    public class MarketStat
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public double date { get; set; }
        [DataMember]
        public double open { get; set; }
        [DataMember]
        public double low { get; set; }
        [DataMember]
        public double high { get; set; }
        [DataMember]
        public double close { get; set; }
        [DataMember]
        public long volumeTraded { get; set; }

        public DateTime Date { get; set; }
        public string dateString { get; set; }


        public MarketStat()
        {

        }
    }
}
