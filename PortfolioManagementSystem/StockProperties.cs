using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManagementSystem
{
    public class StockProperties
    {
        public string Name { get; set; }
        public double AveChange { get; set; }
        public string Volatility { get; set; }
        public string Liquidity { get; set; }

        public StockProperties(string name, double aveChange, string volatility, string liquidity)
        {
            Name = name;
            AveChange = aveChange;
            Volatility = volatility;
            Liquidity = liquidity;
        }
    }
}
