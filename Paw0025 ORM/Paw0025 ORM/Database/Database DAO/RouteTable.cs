using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace Paw0025_ORM.Database.Database_DAO
{
    class RouteTable
    {
        /*
            * 5.1 New Route                 JEST
            5.2 Route details               JEST
            5.3 List of client’s routes     JEST
         */
        
        public static String SQL_SELECT_ID = "SELECT * FROM Route WHERE route_id=@route_id";
        public static String SQL_INSERT = "INSERT INTO Route(start_place_id,end_place_id) VALUES (@start_place_id,@end_place_id)";

        public static String SQL_SELECT_PLACES = "select * from place where client_id=@client_id";
        public static String SQL_SELECT_ROUTES = "select * from route where start_place_id=@place_id OR end_place_id=@place_id";

        /// <summary>
        /// Insert the record.
        /// </summary>
        public static int Insert(Route route)
        {
            Database db = new Database();
            db.Connect();

            SqlCommand command = db.CreateCommand(SQL_INSERT);
            PrepareCommand(command, route);
            int ret = db.ExecuteNonQuery(command);
            
                db.Close();
            Console.WriteLine("Executed");

            return ret;
        }




        //Route details
        
        public static Route Select(int id)
        {
            Database db = new Database();
            db.Connect();

            SqlCommand command = db.CreateCommand(SQL_SELECT_ID);

            command.Parameters.AddWithValue("@route_id", id);
            SqlDataReader reader = db.Select(command);

            Collection<Route> Routes = Read(reader);
            Route Route = null;
            if (Routes.Count == 1)
            {
                Route = Routes[0];
            }
            reader.Close();

            db.Close();

            Console.WriteLine("Details of route " + id);
            Console.WriteLine(Route.route_id + " " + Route.start_place_id + " " + Route.end_place_id);
            return Route;
        }

        public static Collection<Route> list_of_clients_routes(int client_id)
        {
           
            Database db = new Database();
            db.Connect();

            SqlCommand command = db.CreateCommand(SQL_SELECT_PLACES);

            command.Parameters.AddWithValue("@client_id", client_id);
            SqlDataReader reader = db.Select(command);

            Collection<Place> Places =Database_DAO.PlaceTable.Read(reader);
            Collection<Route> Routes=null;

            Collection<Route> RouteList = new Collection<Route>();
            
            
            reader.Close();
            //Console.WriteLine("List of clients routes");
            foreach (Place p in Places)
            {
                //Console.WriteLine(p.place_id);
                SqlCommand command2 = db.CreateCommand(SQL_SELECT_ROUTES);
                command2.Parameters.AddWithValue("@place_id", p.place_id);
                SqlDataReader reader2 = db.Select(command2);

                Routes = Read(reader2);
                Route Route = null;

                Boolean Routelistcheck = true;

                // One place can be found in different routes so there may be more routes to be handled
                foreach(Route rr in Routes)
                {
                    foreach(Route r in RouteList)
                    {
                        //The cursor brings every route twice(client's ID in place of start end end) so the duplicate must be ommited
                        if (r.route_id == rr.route_id)
                        {
                            Routelistcheck = false;
                        }
                    }
                    if (Routelistcheck == true)
                    {
                        RouteList.Add(rr);
                        //Console.WriteLine(rr.route_id + " " + rr.start_place_id + " " + rr.end_place_id);
                    }
                }
                reader2.Close();
                
            }
            
            db.Close();
            return RouteList;
        
    }






        /// <summary>
        ///  Prepare a command.
        /// </summary>
        private static void PrepareCommand(SqlCommand command, Route Route)
        {
            command.Parameters.AddWithValue("@route_id", Route.route_id);
            command.Parameters.AddWithValue("@start_place_id", Route.start_place_id);
            command.Parameters.AddWithValue("@end_place_id", Route.end_place_id);
            


        }

        private static Collection<Route> Read(SqlDataReader reader)
        {
            Collection<Route> routes = new Collection<Route>();

            while (reader.Read())
            {
                int i = -1;
                Route route = new Route();
                route.route_id = reader.GetInt32(++i);
                route.start_place_id = reader.GetInt32(++i);
                route.end_place_id= reader.GetInt32(++i);


                


                routes.Add(route);
            }
            return routes;
        }
    }
}
