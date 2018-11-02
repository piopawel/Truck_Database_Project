using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paw0025_ORM.Database
{
    class Place
    {
        public int place_id { get; set; }

        public Client client { get; set; }
        public int client_id { get; set; }

        public String city { get; set; }
        public String street { get; set; }
        public String numer { get; set; }
        public String ZIP { get; set; }
    }
}
