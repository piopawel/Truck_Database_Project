using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paw0025_ORM.Database
{
    class Truck
    {
        public string plate { get; set; }
        
        public int driver_id { get; set; }


        public string model { get; set; }
        public string brand { get; set; }
        public double width { get; set; }
        public double height { get; set; }
        public bool available { get; set; }
    }
}
