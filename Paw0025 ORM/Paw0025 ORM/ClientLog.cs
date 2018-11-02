using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data;

namespace Paw0025_ORM
{
    class ClientLog
    {
        public string login;
        public string password;
        public static String SQL_SELECT_LOGIN = "SELECT * FROM Client WHERE login=@login";

        public bool checkLogin(string login, string password)
        {
            Database.Database_DAO.Database db = new Database.Database_DAO.Database();
            db.Connect();

            SqlCommand command = db.CreateCommand(SQL_SELECT_LOGIN);
            command.Parameters.AddWithValue("@login", login);

            SqlDataReader reader = db.Select(command);

            Collection<Database.Client> Clients = Database.Database_DAO.ClientTable.Read(reader);
            Database.Client Client = null;
            if (Clients.Count == 1)
            {
                Client = Clients[0];
            }
            else
            {

                return false;
            }
            reader.Close();
            db.Close();

            if (Client.password == password)
                return true;
            else
            {
               
                return false;
            }
        }
    }

    


}
