using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data;

namespace Paw0025_ORM.Database.Database_DAO
{
    class OrdersTable
    {
        /*
         7.1 New Order                          JEST   
         7.2 Update Order                       JEST
         7.3 Delete Order                       JEST
         7.4 List of Orders (clients)           JEST Filter is client id
         7.5 Specific order details with additional information about payment       JEST

             */
        public static String SQL_SELECT = "SELECT * FROM ORDERS";
        public static String SQL_SELECT_ID = "SELECT * FROM Orders WHERE client_id=@client_id";
       
        public static String SQL_UPDATE = "UPDATE orders SET  route_id=@route_id, truck_plate=@truck_plate, client_id=@client_id, "
            + "order_status=@order_status, start_date=@start_date, end_date=@end_date, width=@width, height=@height, load_description=@load_description"
            + " WHERE order_id=@order_id";

        public static string SQL_future_orders = "SELECT * FROM orders WHERE client_id =@client_id AND start_date>@current_date";
        public static string SQL_DELETE = "DELETE from orders where order_id=@order_id";
        public static string SQL_FIND_PAYMENT = "Select * from payment where order_id=order_id";

        public static string SQL_ORDER_DETAILS = "SELECT o.order_id, o.route_id, o.truck_plate, o.client_id, o.order_status, o.start_date, o.end_date, " +
            "o.height, o.width, o.load_description, p.price, p.payment_status FROM orders o " +
            "JOIN payment p on p.order_id=o.order_id WHERE o.order_id=@order_id";

        public static int new_order(Orders order)
        {
            Database db = new Database();
            db.Connect();

            SqlCommand command = db.CreateCommand("new_order");

            command.CommandType = CommandType.StoredProcedure;

            SqlParameter client = new SqlParameter();
            client.ParameterName = "@client_id";
            client.DbType = DbType.Int32;
            client.Value = order.client_id;
            client.Direction = ParameterDirection.Input;
            command.Parameters.Add(client);
            /*
             The analisys contains a mistake - there should not be a date of performing an action but
             start date of the order
             SELECT * FROM orders
             WHERE truck_plate=$available_truck.plate
             AND start_date-$current_date < 3
             AND start_date-$current_date >(-3)
             */


            SqlParameter start_date = new SqlParameter();
            start_date.ParameterName = "@start_date";
            start_date.DbType = DbType.Date;
            start_date.Value = order.start_date;
            start_date.Direction = ParameterDirection.Input;
            command.Parameters.Add(start_date);

            SqlParameter route_id = new SqlParameter();
            route_id.ParameterName = "@route_id";
            route_id.DbType = DbType.Int32;
            route_id.Value = order.route_id;
            route_id.Direction = ParameterDirection.Input;
            command.Parameters.Add(route_id);

            SqlParameter width = new SqlParameter();
            width.ParameterName = "@width";
            width.DbType = DbType.Int32;
            width.Value = order.width;
            width.Direction = ParameterDirection.Input;
            command.Parameters.Add(width);

            SqlParameter height = new SqlParameter();
            height.ParameterName = "@height";
            height.DbType = DbType.Int32;
            height.Value = order.height;
            height.Direction = ParameterDirection.Input;
            command.Parameters.Add(height);

            SqlParameter description = new SqlParameter();
            description.ParameterName = "@description";
            description.DbType = DbType.String;
            description.Value = order.load_description;
            description.Direction = ParameterDirection.Input;
            command.Parameters.Add(description);



            // 4. execute procedure

            int ret = db.ExecuteNonQuery(command);
            //db.ExecuteNonQuery(command);
            Console.WriteLine("Executed");
            // 5. get values of the output parameters
            //string result = command.Parameters["@result"].Value.ToString();


            db.Close();
            //Console.WriteLine("Executed");

            return ret;
        }


        public static int Update(Orders order, int order_id)
        {
            Database db = new Database();
            db.Connect();

            SqlCommand command = db.CreateCommand(SQL_UPDATE);
            PrepareCommand(command, order);
            command.Parameters.AddWithValue("@order_id", order_id);
            int ret = db.ExecuteNonQuery(command);
            
                db.Close();

            Console.WriteLine("Executed");
            return ret;
        }

       
        public static Collection<Orders> Select(int id)
        {
            Database db = new Database();
            db.Connect();

            SqlCommand command = db.CreateCommand(SQL_SELECT_ID);

            command.Parameters.AddWithValue("@client_id", id);
            SqlDataReader reader = db.Select(command);

            Collection<Orders> Orders = Read(reader);
            
            
            reader.Close();

            db.Close();

            Console.WriteLine("Orders of the client: " + id);
            foreach(Orders o in Orders)
            {
                Console.WriteLine("client_id "+ o.client_id + " order_id " + o.order_id);
            }

            return Orders;
        }

        public static Collection<Orders> Select()
        {
            Database db = new Database();
            db.Connect();

            SqlCommand command = db.CreateCommand(SQL_SELECT);
            SqlDataReader reader = db.Select(command);

            Collection<Orders> orders = Read(reader);
            reader.Close();

            db.Close();

            Console.WriteLine("List of orders:");
            foreach (Orders t in orders)
            {
                Console.WriteLine(t.order_id+" "+t.order_status);
            }
            return orders;
        }

        private static int id()
        {
            //There is no checking if the id is correct, this is just to follow the Analisys
            Console.WriteLine("Choose the order to delete(choose id from above!):");
            int order_id=0;
            try
            {
                order_id = Convert.ToInt32(Console.ReadLine());
            }
            catch (Exception e)
            {
                Console.WriteLine("Not a number");
                id();
            }
            return order_id;
        }
       
        public static void Delete(int client_id)
        {
            Database db = new Database();
            db.Connect();

            SqlCommand command = db.CreateCommand(SQL_future_orders);
            command.Parameters.AddWithValue("@client_id", client_id);
            command.Parameters.AddWithValue("@current_date", Convert.ToDateTime("2016-04-01"));
            SqlDataReader reader = db.Select(command);

            Collection<Orders> orders = Read(reader);
            reader.Close();

            foreach (Orders o in orders)
            {
                Console.WriteLine("order_id: "+ o.order_id);
            }
            int selected_order_id = id();
            SqlCommand command2 = db.CreateCommand("delete_order");

            command2.CommandType = CommandType.StoredProcedure;

            SqlParameter selected_order = new SqlParameter();
            selected_order.ParameterName = "@order_id";
            selected_order.DbType = DbType.Int32;
            selected_order.Value = selected_order_id;
            selected_order.Direction = ParameterDirection.Input;
            command2.Parameters.Add(selected_order);

            int ret = db.ExecuteNonQuery(command2);
            
            Console.WriteLine("Deleted the " + selected_order_id + " order");

            db.Close();
        }

        /// <summary>
        ///  Prepare a command.
        /// </summary>
        private static void PrepareCommand(SqlCommand command, Orders Order)
        {
            string defaultStatus = "not_started";
            //command.Parameters.AddWithValue("@order_id", Order.order_id);
            command.Parameters.AddWithValue("@route_id", Order.route_id);
            command.Parameters.AddWithValue("@truck_plate", Order.Truck_plate==null?DBNull.Value:(object)Order.Truck_plate);
            command.Parameters.AddWithValue("@client_id", Order.client_id);
            command.Parameters.AddWithValue("@order_status", Order.order_status==null?defaultStatus:(object)Order.order_status);
           
            command.Parameters.AddWithValue("@start_date", Order.start_date);
            // Null value in DateTime is considered as 0001-01-01, that's why this is the condition
            command.Parameters.AddWithValue("@end_date", Order.end_date== Convert.ToDateTime("0001-01-01")?DBNull.Value:(object)Order.end_date);
            command.Parameters.AddWithValue("@width", Order.width);
            command.Parameters.AddWithValue("@height", Order.height);
            command.Parameters.AddWithValue("@load_description", Order.load_description);


        }

        public static Collection<Orders> Read(SqlDataReader reader)
        {
            Collection<Orders> orders = new Collection<Orders>();

            while (reader.Read())
            {
                int i = -1;
                Orders order = new Orders();
                order.order_id = reader.GetInt32(++i);
                order.route_id = reader.GetInt32(++i);
                if (!reader.IsDBNull(++i))
                {
                    order.Truck_plate = reader.GetString(i);
                }
                order.client_id = reader.GetInt32(++i);
                order.order_status = reader.GetString(++i);
                order.start_date = reader.GetDateTime(++i);
                if (!reader.IsDBNull(++i))
                {
                    order.end_date = reader.GetDateTime(i);
                }
                order.width = reader.GetInt64(++i);
                order.height = reader.GetInt64(++i);
                order.load_description = reader.GetString(++i);
                


                orders.Add(order);
            }
            return orders;
        }

        public static OrderDetails Order_details(int order_id)
        {
            Database db = new Database();
            db.Connect();

            SqlCommand command = db.CreateCommand(SQL_ORDER_DETAILS);

            command.Parameters.AddWithValue("@order_id", order_id);
            SqlDataReader reader = db.Select(command);

            Collection<OrderDetails> Orders = OrderDetails.Read(reader);
            OrderDetails Order = null;
            if (Orders.Count == 1)
            {
                Order = Orders[0];
            }
            reader.Close();


            db.Close();
            Console.WriteLine("Details of the order :" +order_id) ;
            Console.WriteLine(Order.order_id +" "+ Order.route_id + " " +Order.Truck_plate + " " +Order.client_id + " " + Order.order_status + " " +Order.start_date + " " +Order.end_date + " " +Order.width + " " +Order.height + " " +Order.load_description + " " +Order.price + " " +Order.payment_status );

            return Order;

        }

        public class OrderDetails
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
            public int price { get; set; }
            public string payment_status { get; set; }

            public static Collection<OrderDetails> Read(SqlDataReader reader)
            {
                Collection<OrderDetails> orders = new Collection<OrderDetails>();

                while (reader.Read())
                {
                    int i = -1;
                    OrderDetails order = new OrderDetails();
                    order.order_id = reader.GetInt32(++i);
                    order.route_id = reader.GetInt32(++i);
                    if (!reader.IsDBNull(++i))
                    {
                        order.Truck_plate = reader.GetString(i);
                    }
                    order.client_id = reader.GetInt32(++i);
                    order.order_status = reader.GetString(++i);
                    order.start_date = reader.GetDateTime(++i);
                    if (!reader.IsDBNull(++i))
                    {
                        order.end_date = reader.GetDateTime(i);
                    }
                    order.width = reader.GetInt64(++i);
                    order.height = reader.GetInt64(++i);
                    order.load_description = reader.GetString(++i);
                    if (!reader.IsDBNull(++i))
                    {
                        order.price = reader.GetInt32(i);
                    }
                    if (!reader.IsDBNull(++i))
                    {
                        order.payment_status = reader.GetString(i);
                    }

                    orders.Add(order);
                }
                return orders;
            }
        }
    }
}
