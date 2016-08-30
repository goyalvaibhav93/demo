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
    class Product
    {
        [DataMember]
        int productId { get; set; }
        [DataMember]
        String productName { get; set; }
        [DataMember]
        double unitPrice { get; set; }
        [DataMember]
        int unitsInStock { get; set; }
        [DataMember]
        int reorderLevel { get; set; }

        public Product(int productId, String productName, double unitPrice, int unitsInStock, int reOrderLevel)
        {
            this.productId = productId;
            this.productName = productName;
            this.unitPrice = unitPrice;
            this.unitsInStock = unitsInStock;
            this.reorderLevel = reOrderLevel;
        }

        public override string ToString()
        {
            return productName;
        }
    }
}
