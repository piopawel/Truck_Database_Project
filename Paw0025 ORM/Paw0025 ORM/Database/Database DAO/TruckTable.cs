using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace Paw0025_ORM.Database.Database_DAO
{
    /*
     *  6.1 New Truck               JEST
        6.2 Update Truck            
            a) Update driver_id     JEST
            b) Update available     JEST
        6.3 List of Trucks          JEST
     * */
    class TruckTable
    {
        //TODO update the available
        public static String SQL_SELECT = "SELECT * FROM Truck";
        public static String SQL_INSERT = "INSERT INTO Truck VALUES (@plate, @driver_id,@model, @brand, @width, @height, @available)";
        public static String SQL_UPDATE = "UPDATE Truck SET driver_id=@driver_id, model=@model, brand=@brand, width=@width, height=@height, available=@available where  plate=@plate";


        public static String SQL_SPEC_ORDER = "SELECT * FROM orders WHERE truck_plate=@truck_plate AND start_date>@current_date";
        public static String SQL_UPDATE_ORDER_TRUCK = "UPDATE orders SET truck_plate = null WHERE order_id = @order_id";
        public static String SQL_UPDATE_TRUCK_AVAILABLE = "UPDATE truck SET available=0 WHERE plate=@plate";
        /// <summary>
        /// Insert the record.
        /// </summary>
        public static int Insert(Truck truck)
        {
            Database db = new Database();
            db.Connect();

            SqlCommand command = db.CreateCommand(SQL_INSERT);
            PrepareCommand(command, truck);
            int ret = db.ExecuteNonQuery(command);
            
            db.Close();

            Console.WriteLine("Executed");
            return ret;
        }

    
        public static int Update(Truck truck)
        {
            Database db = new Database();
            db.Connect();

            SqlCommand command = db.CreateCommand(SQL_UPDATE);
            PrepareCommand(command, truck);
            int ret = db.ExecuteNonQuery(command);

            db.Close();

            Console.WriteLine("Executed");
            return ret;
        }

        
        public static Collection<Truck> Select()
        {
            Database db = new Database();
            db.Connect();

            SqlCommand command = db.CreateCommand(SQL_SELECT);
            SqlDataReader reader = db.Select(command);

            Collection<Truck> trucks = Read(reader);
            reader.Close();

            db.Close();

            Console.WriteLine("List of trucks:");
            foreach(Truck t in trucks)
            {
                Console.WriteLine(t.plate + " " + t.driver_id + " " + t.brand + " " + t.model + " " + t.width +" "+ t.height +" "+ t.available);
            }
            return trucks;
        }

        public static void UpdateAvailable(string plate)
        {
            Database db = new Database();
            db.Connect();

            db.BeginTransaction();
            try
            {
                SqlCommand command = db.CreateCommand(SQL_SPEC_ORDER);
                command.Parameters.AddWithValue("@truck_plate", plate);
                //date is fixed for testing
                command.Parameters.AddWithValue("@current_date", "2016-05-03");
                SqlDataReader reader = db.Select(command);



                Collection<Orders> orders = OrdersTable.Read(reader);
                reader.Close();
                Console.WriteLine("Changed orders: (order_id, start_date)");
                foreach (Orders o in orders)
                {
                    Console.WriteLine(o.order_id + " " + o.start_date);
                    SqlCommand command2 = db.CreateCommand(SQL_UPDATE_ORDER_TRUCK);
                    command2.Parameters.AddWithValue("@order_id", o.order_id);
                    db.ExecuteNonQuery(command2);
                }
                SqlCommand command3 = db.CreateCommand(SQL_UPDATE_TRUCK_AVAILABLE);
                command3.Parameters.AddWithValue("@plate", plate);
                db.ExecuteNonQuery(command3);
                Console.WriteLine("Executed");
                db.EndTransaction();

            }catch(Exception e)
            {
                Console.WriteLine("Sth wrong");
                db.Rollback();
            }
            
            db.Close();
            
        }


        /// <summary>
        ///  Prepare a command.
        /// </summary>
        private static void PrepareCommand(SqlCommand command, Truck Truck)
        {
            command.Parameters.AddWithValue("@plate", Truck.plate);
            command.Parameters.AddWithValue("@driver_id", Truck.driver_id);
            command.Parameters.AddWithValue("@model", Truck.model);
            command.Parameters.AddWithValue("@brand", Truck.brand);
            command.Parameters.AddWithValue("@width", Truck.width);
            command.Parameters.AddWithValue("@height", Truck.height);
            command.Parameters.AddWithValue("@available", Truck.available);

        }

        private static Collection<Truck> Read(SqlDataReader reader)
        {
            Collection<Truck> trucks = new Collection<Truck>();

            while (reader.Read())
            {
                int i = -1;
                Truck truck = new Truck();
                truck.plate = reader.GetString(++i);
                
                truck.driver_id = reader.GetInt32(++i);
                
                truck.model = reader.GetString(++i);
                
                truck.brand = reader.GetString(++i);
                
                truck.width = reader.GetInt64(++i);
                
                truck.height = reader.GetInt64(++i);
                truck.available = reader.GetBoolean(++i);
             
                trucks.Add(truck);
            }
            return trucks;
        }
    }
}
