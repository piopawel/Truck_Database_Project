using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data;

namespace Paw0025_ORM.Database.Database_DAO
{
    class PaymentTable
    {

        /*4.1 New Payment           JEST
          4.2 Update Payment        JEST
          4.3 Delete Payment
            Client – occurs alongside deleting an order, only if it payment’s status is ‘unpaid’;       JEST (funkcja wywolywana przy delete order)
            Admin – admin can delete a payment if the payment status is ‘paid_but_deleted’              JEST
          4.4 List of client’s payments           JEST
          4.5 Sum of client’s unpaid payments     JEST

         */
        public static String SQL_SELECT_PAYMENT_ID = "SELECT * FROM Payment WHERE payment_id=@payment_id";
        public static String SQL_SELECT_ID = "SELECT * FROM Payment WHERE client_id=@client_id";
        public static String SQL_INSERT = "INSERT INTO Payment(client_id,order_id,price,payment_status) VALUES (@client_id,@order_id,@price, @payment_status)";
        public static String SQL_UPDATE = "UPDATE Payment SET client_id=@client_id,order_id=@order_id, price=@price, payment_status=@payment_status WHERE payment_id=@payment_id";
        public static String SQL_DELETE_ID = "DELETE FROM Payment WHERE payment_id=@payment_id";

        /// <summary>
        /// Insert the record.
        /// </summary>
        public static int Insert(Payment payment)
        {
            Database db = new Database();
            db.Connect();

            SqlCommand command = db.CreateCommand(SQL_INSERT);
            PrepareCommand(command, payment);
            int ret = db.ExecuteNonQuery(command);

            
                db.Close();
            Console.WriteLine("Executed");

            return ret;
        }

        /// <summary>
        /// Update the record.
        /// </summary>
        public static int Update(Payment payment, int payment_id)
        {
            
            Database db = new Database();
            db.Connect();

            SqlCommand command = db.CreateCommand(SQL_UPDATE);
            command.Parameters.AddWithValue("@payment_id", payment_id);
            command.Parameters.AddWithValue("@client_id", payment.client_id);
            command.Parameters.AddWithValue("@price", payment.price);
            command.Parameters.AddWithValue("@payment_status", payment.payment_status);
            command.Parameters.AddWithValue("@order_id", payment.order_id);

           
        
            int ret = db.ExecuteNonQuery(command);

           
                db.Close();

            Console.WriteLine("Executed");
            return ret;
        }

        //Select clients payments

        public static Collection<Payment> Select(int client_id)
        {
            Database db = new Database();
            db.Connect();

            SqlCommand command = db.CreateCommand(SQL_SELECT_ID);
            command.Parameters.AddWithValue("@client_id", client_id);
            SqlDataReader reader = db.Select(command);

            Collection<Payment> payments = Read(reader);
            reader.Close();


            db.Close();
            Console.WriteLine("Clients payments:");
            foreach (Payment c in payments)
            {
                Console.WriteLine(c.payment_id +" "+c.client_id + " " +c.order_id + " " + c.price + " " + c.payment_status);
            }

            return payments;
        }

        public static int unpaid_sum(int client_id)
        {
            Database db = new Database();
            db.Connect();

           
            SqlCommand command = db.CreateCommand("unpaid_sum");
            
            command.CommandType = CommandType.StoredProcedure;
            
            SqlParameter input = new SqlParameter();
            input.ParameterName = "@p_client_id";
            input.DbType = DbType.Int32;
            input.Value = client_id;
            input.Direction = ParameterDirection.Input;
            command.Parameters.Add(input);

            
            SqlParameter output = new SqlParameter();
            output.ParameterName = "@p_sum";
            output.DbType = DbType.Int32;
            output.Direction = ParameterDirection.Output;
            command.Parameters.Add(output);

            // 4. execute procedure

            //Executes a collection of batches in the context of the database where there are no results returned
            //but there is some result
            int ret = db.ExecuteNonQuery(command);

            // 5. get values of the output parameters
            int result = Convert.ToInt32(command.Parameters["@p_sum"].Value);

            
                db.Close();
            Console.WriteLine("Client has to pay:");
            
            return result;
        }

        //occurs alongside deleting an order, not 
        
        public static int Delete(int payment_id,Database db)
        {

            SqlCommand command = db.CreateCommand(SQL_DELETE_ID);

            command.Parameters.AddWithValue("@payment_id", payment_id);
            int ret = db.ExecuteNonQuery(command);
            Console.WriteLine("Executed");
            return ret;
            
        }
        
        public static int delete_as_admin(int payment_id)
        {
            Database db = new Database();
            db.Connect();
            SqlCommand command = db.CreateCommand(SQL_SELECT_PAYMENT_ID);

            command.Parameters.AddWithValue("@payment_id", payment_id);

            SqlDataReader reader = db.Select(command);
            Collection<Payment> Payments = Read(reader);
            Payment Payment = null;
            if (Payments.Count == 1)
            {
                Payment = Payments[0];
            }
            reader.Close();

            if (Payment.payment_status == "paid_but_deleted")
            {

                int ret = Delete(payment_id, db);
                db.Close();
                //Console.WriteLine("Executed");
                return ret;
            }
            else
            {
                Console.WriteLine("Sorry, this payment status is: " + Payment.payment_status + ". It must be paid_but_deleted");
                db.Close();
                return 0;
            }
           
            
        }

        


        /// <summary>
        ///  Prepare a command.
        /// </summary>
        private static void PrepareCommand(SqlCommand command, Payment Payment)
        {
            command.Parameters.AddWithValue("@payment_id", Payment.payment_id);
            command.Parameters.AddWithValue("@client_id", Payment.client_id);
            command.Parameters.AddWithValue("@order_id", Payment.order_id);
            command.Parameters.AddWithValue("@price", Payment.price);
            command.Parameters.AddWithValue("@payment_status", Payment.payment_status);


        }

        private static Collection<Payment> Read(SqlDataReader reader)
        {
            Collection<Payment> payments = new Collection<Payment>();

            while (reader.Read())
            {
                int i = -1;
                Payment payment = new Payment();
                payment.payment_id = reader.GetInt32(++i);
                if (!reader.IsDBNull(++i))
                {
                    payment.order_id = reader.GetInt32(i);
                }
                payment.client_id = reader.GetInt32(++i);
                payment.price = reader.GetInt32(++i);
                payment.payment_status = reader.GetString(++i);

                payments.Add(payment);
            }
            return payments;
        }
    }
}
