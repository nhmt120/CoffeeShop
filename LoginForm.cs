using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace CoffeeShop
{
    public partial class LoginForm : Form
    {
        private SqlConnection connection;
        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            String username = txtUsername.Text;
            String password = txtPassword.Text;


            //MessageBox.Show(username + " " + password);

            if (username == "") {
                txtUsername.Focus();
            } else if (password == "") {
                txtPassword.Focus();
            } else {
                String role = null;
                String sql = "SELECT * FROM Accounts where username = '" + username + "' and password = '" + password + "'";
                SqlCommand cmd = new SqlCommand(sql, connection);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read()) {
                    role = reader[4].ToString();
                }
                reader.Close();
                if (role == "Admin") {
                    AdminForm admin = new AdminForm();
                    admin.Show();
                    connection.Close();
                    this.Hide();
                } else if (role == "Cashier") {
                    CashierForm cashier = new CashierForm();
                    cashier.Show();
                    connection.Close();
                    this.Hide();
                } else {
                    MessageBox.Show("Authentication failed.", "Login CFS", MessageBoxButtons.OK,  MessageBoxIcon.Error);
                    txtUsername.Text = "";
                    txtPassword.Text = "";
                    txtUsername.Focus();
                }
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            Connect();
        }

        private void Connect()
        {
            String connectionString = "Server=.\\SQLEXPRESS; Database=CFS;Integrated Security=SSPI";
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                //MessageBox.Show("Connect to db.", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can't connect to db\nError:\n" + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
