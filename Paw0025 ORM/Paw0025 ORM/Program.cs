using System;
using System.Windows.Forms;


namespace Paw0025_ORM
{
    class Program
    {
        static void clients()
        {
            Database.Client client = new Database.Client();

            client.fname = "Jon";
            client.lname = "Sneg";
            client.email = "knowsnothing@abc.com";
            client.password = "pass";
            client.login = "somebod";

            //Database.Database_DAO.ClientTable.Insert(client);

            client.lname = "Snow";
            //Database.Database_DAO.ClientTable.Update(client,1);       // The ID must be the same as in the DB!
            Database.Database_DAO.ClientTable.Select();
            Database.Database_DAO.ClientTable.Select(2);
        }

        static void places()
        {
            Database.Place place1 = new Database.Place();

            place1.client_id = 1;
            place1.city = "Brno";
            place1.street = "Dukelska";
            place1.numer = "111";
            place1.ZIP = "11-111";

            Database.Place place2 = new Database.Place();

            place2.client_id = 1;
            place2.city = "OstravaSvi";
            place2.street = "Studentskaaaa";
            place2.numer = "1121";
            place2.ZIP = "121-111";

            Database.Database_DAO.PlaceTable.Insert(place1);
            //Database.Database_DAO.PlaceTable.Insert(place2);

            Database.Database_DAO.PlaceTable.Select();
            Database.Database_DAO.PlaceTable.Select(5);

        }

        static void drivers()
        {
            Database.Driver driver1 = new Database.Driver();

            driver1.fname = "Ben";
            driver1.lname = "Kowalski";

            Database.Driver driver2 = new Database.Driver();

            driver2.fname = "Marcin";
            driver2.lname = "Polak";

            //Database.Database_DAO.DriverTable.Insert(driver1);
            //Database.Database_DAO.DriverTable.Insert(driver2);

            driver2.fname = "Krzysztof";
            //Database.Database_DAO.DriverTable.Update(driver2, 3);

            Database.Database_DAO.DriverTable.Select();
            
            Database.Database_DAO.DriverTable.Specific_places(1);

            
        }

        static void payments()
        {
            
            
            Database.Payment payment1 = new Database.Payment();
            payment1.client_id = 2;
            payment1.price = 1900;
            payment1.payment_status = "unpaid";
            payment1.order_id = 5;

           // Database.Database_DAO.PaymentTable.Insert(payment1);

            payment1.price = 2500;
            payment1.payment_status = "paid_but_deleted";

            //Database.Database_DAO.PaymentTable.Update(payment1, 14);

            Database.Database_DAO.PaymentTable.Select(1);

            Console.WriteLine(Database.Database_DAO.PaymentTable.unpaid_sum(1));

            //Database.Database_DAO.PaymentTable.delete_as_admin(16);

        }

        static void routes()
        {
            Database.Route route1 = new Database.Route();
            route1.end_place_id = 16;
            route1.start_place_id = 15;

            //Database.Database_DAO.RouteTable.Insert(route1);
            Database.Database_DAO.RouteTable.Select(1);
            Database.Database_DAO.RouteTable.list_of_clients_routes(1);
        }

        static void trucks()
        {
            Database.Truck truck1 = new Database.Truck();
            truck1.plate = "ccccccc";
            truck1.available = true;
            truck1.brand = "Trucks";
            truck1.driver_id = 8;
            truck1.height = 4;
            truck1.model = "NiceModel";
            truck1.width = 3;

            //Database.Database_DAO.TruckTable.Insert(truck1);

            //truck1.driver_id = 7;
            //Database.Database_DAO.TruckTable.Update(truck1);


            //Database.Database_DAO.TruckTable.UpdateAvailable("aaa1111");
            /*Command returning 
                update orders set truck_plate='aaa1111' where order_id=4;
                update orders set truck_plate='aaa1111' where order_id=3;
                update truck set available = 1 where plate='aaa1111'
                */
            Database.Database_DAO.TruckTable.Select();

            
        }

        static void orders()
        {
            //Database.Database_DAO.OrdersTable.Select(1);
            //Database.Database_DAO.OrdersTable.Select();

            Database.Orders order1 = new Database.Orders();
            order1.route_id = 1;
            order1.client_id = 1;
            order1.start_date = Convert.ToDateTime("2016-05-25");
            order1.width = 2;
            order1.height = 2;
            order1.load_description = "Nice things";

            //Database.Database_DAO.OrdersTable.new_order(order1);

            order1.Truck_plate = "aaa1114";
            //order1.order_status = "executed";

            //Database.Database_DAO.OrdersTable.Update(order1, 17);
            //Database.Database_DAO.OrdersTable.Delete(1);
            //Database.Database_DAO.OrdersTable.Order_details(2);
        }

        static void mainMenu()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainMenu());
        }

        static void newOrder()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new NewOrder());
        }

        static void Main(string[] args)
        {
            //ORM

            //clients();
            //places();
            //drivers();
            //payments();
            //routes();
            //trucks();
            //orders();
            //Database.Database_DAO.NotificationTable.notifications(Convert.ToDateTime("2016-05-10"));
            //Console.ReadLine();

            // INTERFACE

            
            mainMenu();
            //newOrder();

        }
    }
}
