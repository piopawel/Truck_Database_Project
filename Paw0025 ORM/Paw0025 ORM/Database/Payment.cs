using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paw0025_ORM.Database
{
    class Payment
    {
        public int payment_id { get; set; }

        public int order_id { get; set; }


        public int client_id { get; set; }


        public int price { get; set; }
        public string payment_status { get; set; }
        
    }
}
