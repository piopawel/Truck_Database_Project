using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paw0025_ORM.Database
{
    class Orders
    {
        public int order_id { get; set; }

        public int route_id { get; set; }


        public int client_id { get; set; }


        public string Truck_plate { get; set; }


        public string order_status { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public double width { get; set; }
        public double height { get; set; }
        public string load_description { get; set; }
    }
}
