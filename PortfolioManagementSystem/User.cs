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
    class User
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public String name { get; set; }

        public User(int id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }
}
