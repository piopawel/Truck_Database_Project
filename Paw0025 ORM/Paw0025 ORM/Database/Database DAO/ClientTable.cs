using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paw0025_ORM.Database.Database_DAO
{
    class ClientTable
    {

        /*  1.1 Client Insert JEST

            1.2 Client Update   JEST   

            1.3 List of clients (with a definition of a filter to search users)  JEST
                Filter to ID  

            1.4 Client detail   JEST

         * */

        public static String SQL_SELECT = "SELECT * FROM Client";
        public static String SQL_SELECT_ID = "SELECT * FROM Client WHERE client_id=@client_id";
        public static String SQL_INSERT = "INSERT INTO Client(fname,lname,email,password, login) VALUES (@fname, @lname, @email,@password, @login)";
        public static String SQL_UPDATE = "UPDATE Client SET fname=@fname, lname=@lname, email=@email, password=@password WHERE client_id=@client_id";

       //public static String SQL_USERNAME = "SELECT login FROM Client WHERE client_id=@client_id";
        /// <summary>
        /// Insert the record.
        /// </summary>
        public static int Insert(Client client)
        {
            Database db = new Database();
            db.Connect();


            SqlCommand command = db.CreateCommand(SQL_INSERT);
            PrepareCommand(command, client);
            int ret = db.ExecuteNonQuery(command);
            Console.WriteLine("Executed");
          
            db.Close();


            return ret;
        }
        
        public static int Update(Client client, int client_id)
        {
            Database db = new Database();
            db.Connect();

            SqlCommand command = db.CreateCommand(SQL_UPDATE);
            //PrepareCommand(command, client);
            command.Parameters.AddWithValue("@client_id", client_id);
            command.Parameters.AddWithValue("@fname", client.fname);
            command.Parameters.AddWithValue("@lname", client.lname);
            command.Parameters.AddWithValue("@email", client.email);
            command.Parameters.AddWithValue("@password", client.password);

            int ret = db.ExecuteNonQuery(command);

               db.Close();

            Console.WriteLine("Executed");
            return ret;
        }


        public static Collection<Client> Select()
        {
            Database db = new Database();
            db.Connect();

            SqlCommand command = db.CreateCommand(SQL_SELECT);
            SqlDataReader reader = db.Select(command);

            Collection<Client> clients = Read(reader);
            reader.Close();

            
                db.Close();
            Console.WriteLine("All clients:");
            foreach(Client c in clients)
            {
                Console.WriteLine(c.client_id +" "+ c.fname + " " + c.lname + " " + c.email);
            }

            return clients;
        }

        /// <summary>
        /// Select the record.
        /// </summary>
        /// <param name="id">client_id</param>
        public static Client Select(int id)
        {
            Database db = new Database();
            db.Connect();

            SqlCommand command = db.CreateCommand(SQL_SELECT_ID);

            command.Parameters.AddWithValue("@client_id", id);
            
            SqlDataReader reader = db.Select(command);

            Collection<Client> Clients = Read(reader);
            Client Client = null;
            if (Clients.Count == 1)
            {
                Client = Clients[0];
            }
            reader.Close();

            
                db.Close();
            Console.WriteLine("Details of the client with id: " + id);
            Console.WriteLine(Client.client_id + " " + Client.fname + " " + Client.lname + " " + Client.email);

            return Client;
        }

       

        /// <summary>
        ///  Prepare a command.
        /// </summary>
        private static void PrepareCommand(SqlCommand command, Client Client)
        {
            //command.Parameters.AddWithValue("@client_id", Client.client_id); //Client.client_id == null? DBNull.Value : (object)Client.client_id);
            command.Parameters.AddWithValue("@fname", Client.fname);
            command.Parameters.AddWithValue("@lname", Client.lname);
            command.Parameters.AddWithValue("@email", Client.email);
            command.Parameters.AddWithValue("@password", Client.password);
            command.Parameters.AddWithValue("@login", Client.login);
               
        }

        public static Collection<Client> Read(SqlDataReader reader)
        {
            Collection<Client> clients = new Collection<Client>();

            while (reader.Read())
            {
                int i = -1;
                Client client = new Client();
                client.client_id = reader.GetInt32(++i);
                client.fname = reader.GetString(++i);
                client.lname = reader.GetString(++i);
                client.email = reader.GetString(++i);
                client.password = reader.GetString(++i);
                client.login = reader.GetString(++i);
                //?


                clients.Add(client);
            }
            return clients;
        }

        
    }
}
