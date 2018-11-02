using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.ObjectModel;

namespace Paw0025_ORM
{
    public partial class NewOrder : Form
    {
        private double width;
        private double height;
        private int route;
        private string date;
        public int clientID;

        


        public NewOrder()
        {
            InitializeComponent();
            
            widthChoice.Enabled = false;
            heightChoice.Enabled = false;
            description.Enabled = false;
            dateChoice.Enabled = false;
            submitOrder.Enabled = false;

            clientID = 1;


            Collection<Database.Route> routes = new Collection<Database.Route>();
            routes = Database.Database_DAO.RouteTable.list_of_clients_routes(clientID);
            foreach (Database.Route r in routes)
            {
                //Console.WriteLine(r.route_id);
                //routeChoice.Items.Add(5);
                routeChoice.Items.Add(r.route_id);
               
            }

            Database.Client client = new Database.Client();
            label1.Text = client.login;
          
        }

        private void widthChoice_TextChanged(object sender, EventArgs e)
        {
            try
            {
                width = Convert.ToDouble(widthChoice.Text);
                heightChoice.Enabled = true;
            }catch(Exception f)
            {
                MessageBox.Show("Not a number, try again");
                widthChoice.Text = "0";
            }
        }

        private void routeChoice_SelectedIndexChanged(object sender, EventArgs e)
        {
            route = Convert.ToInt32(routeChoice.SelectedItem);
            
            widthChoice.Enabled = true;
        }

        private void heightChoice_TextChanged(object sender, EventArgs e)
        {
            height = Convert.ToDouble(heightChoice.Text);
            dateChoice.Enabled = true;
        }

        private void dateChoice_ValueChanged(object sender, EventArgs e)
        {
            
            description.Enabled = true;
        }

        private void description_TextChanged(object sender, EventArgs e)
        {
            submitOrder.Enabled = true;
        }

        private void submitOrder_Click(object sender, EventArgs e)
        {
            Database.Orders newOrder = new Database.Orders();
            newOrder.client_id = clientID;
            newOrder.route_id = route;
            newOrder.width = width;
            newOrder.height = height;
            newOrder.start_date = dateChoice.Value.Date;
            newOrder.load_description = description.Text;
           

            Database.Database_DAO.OrdersTable.new_order(newOrder);
        }
    }
}
