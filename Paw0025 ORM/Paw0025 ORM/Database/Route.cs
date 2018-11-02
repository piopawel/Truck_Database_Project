using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paw0025_ORM.Database
{
    class Route
    {
        public int route_id { get; set; }

        public Place start_place { get; set; }
        public int start_place_id { get; set; }

        public Place end_place { get; set; }
        public int end_place_id { get; set; }
    }
}
