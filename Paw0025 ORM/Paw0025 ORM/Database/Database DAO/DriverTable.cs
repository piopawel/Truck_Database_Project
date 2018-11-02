using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data;

namespace Paw0025_ORM.Database.Database_DAO
{
    class DriverTable
    {

        /*
         *  3.1 New Driver              DZIALA
            3.2 Update Driver           DZIALA
            3.3 List of Drivers         DZIALA
            3.4 List of routes a Driver has completed in last 30 days       DZIALA
         * */
        

        public static String SQL_SELECT = "SELECT * FROM Driver";
        //Zostawic - moze sie przydac do 3.4
        //public static String SQL_SELECT_ID = "SELECT * FROM Driver WHERE driver_id=@driver_id";
        public static String SQL_INSERT = "INSERT INTO Driver(fname,lname) VALUES (@fname, @lname)";
        public static String SQL_UPDATE = "UPDATE driver SET fname=@fname, lname=@lname WHERE driver_id=@driver_id";

        public static string SQL_THE_ORDERS = "SELECT * FROM orders WHERE TRUCK_PLATE ="
                                              + "(SELECT plate FROM truck WHERE driver_id = @driver_id AND "
                                              + "DATEDIFF ( dy , orders.start_date , @current_date )<30 "
                                              + "AND DATEDIFF(dy , orders.start_date , @current_date )>0 "
                                              + "AND orders.order_status='executed')";
        /// <summary>
        /// Insert the record.
        /// </summary>
        public static int Insert(Driver driver)
        {
            Database db = new Database();
            db.Connect();

            SqlCommand command = db.CreateCommand(SQL_INSERT);
            PrepareCommand(command, driver);
            int ret = db.ExecuteNonQuery(command);

            
                db.Close();
            Console.WriteLine("Executed");

            return ret;
        }

        /// <summary>
        /// Update the record.
        /// </summary>
        public static int Update(Driver driver, int driver_id)
        {
            Database db = new Database();
            db.Connect();

            SqlCommand command = db.CreateCommand(SQL_UPDATE);
            //PrepareCommand(command, driver);
            command.Parameters.AddWithValue("@driver_id", driver_id);
            command.Parameters.AddWithValue("@fname", driver.fname);
            command.Parameters.AddWithValue("@lname", driver.lname);
            int ret = db.ExecuteNonQuery(command);

           
                db.Close();

            Console.WriteLine("Executed");
            return ret;
        }


        /// <summary>
        /// Select the records.
        /// </summary>
        public static Collection<Driver> Select()
        {
            Database db = new Database();
            db.Connect();

            SqlCommand command = db.CreateCommand(SQL_SELECT);
            SqlDataReader reader = db.Select(command);

            Collection<Driver> drivers = Read(reader);
            reader.Close();
            
                db.Close();
            Console.WriteLine("All drivers: ");
            foreach (Driver c in drivers)
            {
                Console.WriteLine(c.driver_id + " " + c.fname + " " + c.lname);
            }

            return drivers;
        }

        public static void Specific_places(int id)
        {

            Database db = new Database();
            db.Connect();

            SqlCommand command = db.CreateCommand(SQL_THE_ORDERS);
            command.Parameters.AddWithValue("@driver_id", id);
            command.Parameters.AddWithValue("@current_date", Convert.ToDateTime("2016-05-20"));   
            //Current date is fixed because of the testing
            SqlDataReader reader = db.Select(command);

            Collection<Orders> orders = OrdersTable.Read(reader);
            reader.Close();

            Console.WriteLine("List of drivers places:");
            foreach (Orders o in orders)
            {
                SqlCommand command2 = db.CreateCommand("place_details");
                command2.CommandType = CommandType.StoredProcedure;
                SqlParameter route = new SqlParameter();
                route.ParameterName = "@route_id";
                route.DbType = DbType.Int32;
                route.Value = o.route_id;
                route.Direction = ParameterDirection.Input;
                command2.Parameters.Add(route);
            
                
                SqlParameter p1city = new SqlParameter();
                p1city.ParameterName = "@p1_city";
                //p1city.SqlDbType = SqlDbType.VarChar;
                
                p1city.DbType = DbType.String;
                p1city.Size = 50;
                p1city.Direction = ParameterDirection.Output;
                command2.Parameters.Add(p1city);

                SqlParameter p1street = new SqlParameter();
                p1street.ParameterName = "@p1_street";
                p1street.DbType = DbType.String;
                p1street.Size = 50;
                p1street.Direction = ParameterDirection.Output;
                command2.Parameters.Add(p1street);

                SqlParameter p1numer = new SqlParameter();
                p1numer.ParameterName = "@p1_numer";
                p1numer.DbType = DbType.String;
                p1numer.Size = 50;
                p1numer.Direction = ParameterDirection.Output;
                command2.Parameters.Add(p1numer);

                SqlParameter p2city = new SqlParameter();
                p2city.ParameterName = "@p2_city";
                p2city.DbType = DbType.String;
                p2city.Size = 50;
                p2city.Direction = ParameterDirection.Output;
                command2.Parameters.Add(p2city);

                SqlParameter p2street = new SqlParameter();
                p2street.ParameterName = "@p2_street";
                p2street.DbType = DbType.String;
                p2street.Size = 50;
                p2street.Direction = ParameterDirection.Output;
                command2.Parameters.Add(p2street);

                SqlParameter p2numer = new SqlParameter();
                p2numer.ParameterName = "@p2_numer";
                p2numer.DbType = DbType.String;
                p2numer.Size = 50;
                p2numer.Direction = ParameterDirection.Output;
                command2.Parameters.Add(p2numer);
                
                
                db.ExecuteNonQuery(command2);
                string result1 = command2.Parameters["@p1_city"].Value.ToString();
                string result2 = command2.Parameters["@p1_street"].Value.ToString();
                string result3 = command2.Parameters["@p1_numer"].Value.ToString();
                string result4 = command2.Parameters["@p2_city"].Value.ToString();
                string result5 = command2.Parameters["@p2_street"].Value.ToString();
                string result6 = command2.Parameters["@p2_numer"].Value.ToString();

                Console.WriteLine(result1 + " " + result2 + " " + result3 + " - " + result4 + " " + result5 + " " + result6);
            }
            db.Close();

        }
        
        /*
        public static string list_of_drivers_places(int driver_id)
        {
            Database db = new Database();
            db.Connect();
            
            SqlCommand command = db.CreateCommand("list_of_drivers_routes");
            
            command.CommandType = CommandType.StoredProcedure;
            
            SqlParameter driver = new SqlParameter();
            driver.ParameterName = "@p_id";
            driver.DbType = DbType.Int32;
            driver.Value = driver_id;driver.Direction = ParameterDirection.Input;
            command.Parameters.Add(driver);

            SqlParameter date = new SqlParameter();
            date.ParameterName = "@p_date";
            date.DbType = DbType.Date;
            date.Value = "2016-05-10";      //Date is fixed because of the fixed dates in DB
            date.Direction = ParameterDirection.Input;
            command.Parameters.Add(date);



            // 4. execute procedure
            
            int ret = db.ExecuteNonQuery(command);
            //db.ExecuteNonQuery(command);
            Console.WriteLine("Executed");
            // 5. get values of the output parameters
            //string result = command.Parameters["@result"].Value.ToString();

            
                db.Close();
            
            return "";
        }

        /// <summary>
        ///  Prepare a command.
        /// </summary>
        */
        private static void PrepareCommand(SqlCommand command, Driver Driver)
        {
            command.Parameters.AddWithValue("@driver_id", Driver.driver_id);
            command.Parameters.AddWithValue("@fname", Driver.fname);
            command.Parameters.AddWithValue("@lname", Driver.lname);
            

        }

        private static Collection<Driver> Read(SqlDataReader reader)
        {
            Collection<Driver> drivers = new Collection<Driver>();

            while (reader.Read())
            {
                int i = -1;
                Driver driver = new Driver();
                driver.driver_id = reader.GetInt32(++i);
                driver.fname = reader.GetString(++i);
                driver.lname = reader.GetString(++i);
               
                //?


                drivers.Add(driver);
            }
            return drivers;
        }
    }
}
