using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data;

namespace Paw0025_ORM.Database.Database_DAO
{
    class NotificationTable
    {
        /*8.1 Notify users who have unpaid payments – this function is executed once a day. 
         * It finds payments that have an order to be finished soon and that are unpaid. 
         * Such payment is inserted into table notification (in a real one an e-mail would be sent)
         * */
         //since there is no need to create objects of Notification there is no such class
        //current date could be extracted inside of DBMS but for the purpose of the tests it is passed as a paramter
        public static void notifications(DateTime date)
        {
            Database db = new Database();
            db.Connect();

            SqlCommand command = db.CreateCommand("notifications");

            command.CommandType = CommandType.StoredProcedure;

            SqlParameter datee = new SqlParameter();
            datee.ParameterName = "@current_date";
            datee.DbType = DbType.DateTime;
            datee.Value = date;
            datee.Direction = ParameterDirection.Input;
            command.Parameters.Add(datee);

           

            // 4. execute procedure

            db.ExecuteNonQuery(command);
            //db.ExecuteNonQuery(command);
            Console.WriteLine("Executed");
            // 5. get values of the output parameters
            //string result = command.Parameters["@result"].Value.ToString();


            db.Close();
            Console.WriteLine("Executed");

        }
    }
}
