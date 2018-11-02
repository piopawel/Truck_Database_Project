using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Paw0025_ORM
{
    public partial class MainMenu : Form
    {
        Database.Client newClient;
        public MainMenu()
        {
            InitializeComponent();
        }

        private void MainMenu_Load(object sender, EventArgs e)
        {
            submitClient.Enabled = false;
            submitNewAcc.Enabled = false;
            passwordClient.Enabled = false;

            
            password.Enabled = false;
            email.Enabled = false;
            fName.Enabled = false;
            lName.Enabled = false;
            passwordRepeat.Enabled = false;
            submitNewAcc.Enabled = false;
            newClient = new Database.Client();
        }

        private void logClient_TextChanged(object sender, EventArgs e)
        {
            passwordClient.Enabled = true;
        }

        private void passwordClient_TextChanged(object sender, EventArgs e)
        {
            submitClient.Enabled = true;
        }

        private void submitClient_Click(object sender, EventArgs e)
        {
            
                ClientLog client = new ClientLog();
                client.login = logClient.Text;
                client.password = passwordClient.Text;
                if (client.checkLogin(client.login, client.password) == true)
                {
                    MessageBox.Show("Login succesful. Welcome " + client.login +".");
                }
                else
                {
                    MessageBox.Show("Incorrect login or password. Please write your login and password again.");
                }
            
        }

        private void login_TextChanged(object sender, EventArgs e)
        {
            password.Enabled = true;
        }

        private void password_TextChanged(object sender, EventArgs e)
        {
            passwordRepeat.Enabled = true;
        }

        private void passwordRepeat_TextChanged(object sender, EventArgs e)
        {
            email.Enabled = true;
        }

        private void email_TextChanged(object sender, EventArgs e)
        {
            fName.Enabled = true;
        }

        private void fName_TextChanged(object sender, EventArgs e)
        {
            lName.Enabled = true;
        }

        private void lName_TextChanged(object sender, EventArgs e)
        {
            submitNewAcc.Enabled = true;
        }

        private void submitNewAcc_Click(object sender, EventArgs e)
        {
            if (password.Text == passwordRepeat.Text)
            {
                
                newClient.fname = fName.Text;
                newClient.lname = lName.Text;
                newClient.login = login.Text;
                newClient.password = password.Text;
                newClient.email = email.Text;

                try
                {
                    Database.Database_DAO.ClientTable.Insert(newClient);
                    MessageBox.Show("New account created!");
                    fName.Text = "";
                    lName.Text = "";
                    login.Text = "";
                    password.Text = "";
                    email.Text = "";
                    passwordRepeat.Text = "";
                }catch(System.Data.SqlClient.SqlException ex)
                {
                    MessageBox.Show(Convert.ToString(ex));
                    MessageBox.Show("The login or email is already in use!");
                }
            }
            else
            {
                MessageBox.Show("Passwords do not match, please try again.");
            }
        }
    }
}
