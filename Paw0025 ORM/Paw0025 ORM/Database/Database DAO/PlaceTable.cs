using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paw0025_ORM.Database.Database_DAO
{
    class PlaceTable
    {

        /*2.1 New place             JEST 
          2.2 Place details         JEST
          2.3 List of Places        JEST

         */
        public static String SQL_SELECT = "SELECT * FROM Place";
        public static String SQL_SELECT_ID = "SELECT * FROM Place WHERE place_id=@place_id";
        public static String SQL_INSERT = "INSERT INTO Place(client_id,city,street,numer,ZIP) VALUES ( @client_id,@city, @street, @numer, @ZIP)";
       

        /// <summary>
        /// Insert the record.
        /// </summary>
        public static int Insert(Place place)
        {
            Database db = new Database();
            db.Connect();

            SqlCommand command = db.CreateCommand(SQL_INSERT);
            PrepareCommand(command, place);
            int ret = db.ExecuteNonQuery(command);

           
                db.Close();

            Console.WriteLine("Executed");
            return ret;
        }

        /// <summary>
        /// Update the record.
        /// </summary>
        


        /// <summary>
        /// Select the records.
        /// </summary>
        public static Collection<Place> Select()
        {
            Database db = new Database();
            db.Connect();

            SqlCommand command = db.CreateCommand(SQL_SELECT);
            SqlDataReader reader = db.Select(command);

            Collection<Place> places = Read(reader);
            reader.Close();

                db.Close();
            Console.WriteLine("List of all the places");
            foreach(Place p in places)
            {
                Console.WriteLine(p.place_id+" "+ p.client_id + " " + p.city + " " + p.street + " " + p.numer + " " + p.ZIP);
            }

            return places;
        }

        /// <summary>
        /// Select the record.
        /// </summary>
        /// <param name="id">client_id</param>
        public static Place Select(int id)
        {
            Database db = new Database();
            db.Connect();

            SqlCommand command = db.CreateCommand(SQL_SELECT_ID);

            command.Parameters.AddWithValue("@place_id", id);
            SqlDataReader reader = db.Select(command);

            Collection<Place> Places = Read(reader);
            Place Place = null;
            if (Places.Count == 1)
            {
                Place = Places[0];
            }
            reader.Close();

            
                db.Close();
            Console.WriteLine("A specific place with id "+ id);
            Console.WriteLine(Place.place_id + " " + Place.client_id + " " + Place.city + " " + Place.street + " " + Place.numer + " " + Place.ZIP);

            return Place;
        }



        /// <summary>
        ///  Prepare a command.
        /// </summary>
        private static void PrepareCommand(SqlCommand command, Place Place)
        {
            command.Parameters.AddWithValue("@place_id", Place.place_id);
            command.Parameters.AddWithValue("@client_id", Place.client_id);
            command.Parameters.AddWithValue("@city", Place.city);
            command.Parameters.AddWithValue("@street", Place.street);
            command.Parameters.AddWithValue("@numer", Place.numer);
            command.Parameters.AddWithValue("@ZIP", Place.ZIP);

        }

        public static Collection<Place> Read(SqlDataReader reader)
        {
            Collection<Place> places = new Collection<Place>();

            while (reader.Read())
            {
                int i = -1;
                Place place = new Place();
                place.place_id = reader.GetInt32(++i);
                place.client_id = reader.GetInt32(++i);
                place.city = reader.GetString(++i);
                place.street = reader.GetString(++i);
                place.numer = reader.GetString(++i);
                place.ZIP = reader.GetString(++i);
                //?


                places.Add(place);
            }
            return places;
        }
    }
}
