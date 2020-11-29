using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoffeeShop
{
    public partial class AdminForm : Form
    {
        private SqlConnection connection;
        public AdminForm()
        {
            InitializeComponent();
        }

        private void AdminForm_Load(object sender, EventArgs e)
        {
            String connectionString = "Server=.\\SQLEXPRESS; Database=CFS;Integrated Security=SSPI";
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                //MessageBox.Show("Connect to db.", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadUsers();
                LoadDrinks();
                cbbRole.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can't connect to db\nError:\n" + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        public void LoadUsers()
        {
            ArrayList listId = new ArrayList();
            ArrayList listUsername = new ArrayList();
            ArrayList listPassword = new ArrayList();
            ArrayList listName = new ArrayList();
            ArrayList listRole = new ArrayList();

            String sql = "SELECT * FROM Accounts";
            SqlCommand cmd = new SqlCommand(sql, connection);
            SqlDataReader reader = cmd.ExecuteReader();

            gridUsers.ColumnCount = 5;
            gridUsers.Columns[0].Name = "Account ID";
            gridUsers.Columns[1].Name = "Username";
            gridUsers.Columns[2].Name = "Password";
            gridUsers.Columns[3].Name = "Name";
            gridUsers.Columns[4].Name = "Role";

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    listId.Add(reader["account_id"].ToString());
                    listUsername.Add(reader["username"].ToString());
                    listPassword.Add(reader["password"].ToString());
                    listName.Add(reader["name"].ToString());
                    listRole.Add(reader["role"].ToString());
                }
                reader.Close();

                for (int i = 0; i < listId.Count; i++)
                {
                    DataGridViewRow newRow = new DataGridViewRow();

                    newRow.CreateCells(gridUsers);
                    newRow.Cells[0].Value = listId[i];
                    newRow.Cells[1].Value = listUsername[i];
                    newRow.Cells[2].Value = listPassword[i];
                    newRow.Cells[3].Value = listName[i];
                    newRow.Cells[4].Value = listRole[i];
                    gridUsers.Rows.Add(newRow);
                }
            }
        }

        public void LoadDrinks()
        {
            ArrayList listId = new ArrayList();
            ArrayList listName = new ArrayList();
            ArrayList listPrice = new ArrayList();
            ArrayList listStock = new ArrayList();
            ArrayList listImage = new ArrayList();

            String sql = "SELECT * FROM Drinks";
            SqlCommand cmd = new SqlCommand(sql, connection);
            SqlDataReader reader = cmd.ExecuteReader();

            gridDrinks.ColumnCount = 5;
            gridDrinks.Columns[0].Name = "Drink ID";
            gridDrinks.Columns[1].Name = "Name";
            gridDrinks.Columns[2].Name = "Price";
            gridDrinks.Columns[3].Name = "Stock";
            gridDrinks.Columns[4].Name = "Image";

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    listId.Add(reader["drink_id"].ToString());
                    listName.Add(reader["name"].ToString());
                    listPrice.Add(reader["price"].ToString());
                    listStock.Add(reader["stock"].ToString());
                    listImage.Add(reader["image"].ToString());
                }
                reader.Close();

                for (int i = 0; i < listId.Count; i++)
                {
                    DataGridViewRow newRow = new DataGridViewRow();

                    newRow.CreateCells(gridDrinks);
                    newRow.Cells[0].Value = listId[i];
                    newRow.Cells[1].Value = listName[i];
                    newRow.Cells[2].Value = listPrice[i];
                    newRow.Cells[3].Value = listStock[i];
                    newRow.Cells[4].Value = listImage[i];
                    gridDrinks.Rows.Add(newRow);
                }
            }
        }

        private void AdminForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to exit?", "Coffee Shop Management System", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                Application.Exit();
            }
        }

        private void btnAddDrinks_Click(object sender, EventArgs e)
        {
            String name = txtDrinkName.Text.Trim();
            String price = txtDrinkPrice.Text.Trim();
            String stock = txtDrinkStock.Text.Trim();

            if (name == "")
            {
                txtDrinkName.Focus();
            }
            else if (price == "")
            {
                txtDrinkPrice.Focus();
            }
            else
            {
                if (stock == "") stock = "default";
                String sql = "INSERT INTO Drinks(name, price, stock)" +
                    "VALUES('" + name + "', ' " + price + " ', " + stock + ")";

                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.ExecuteNonQuery();
                
                
                gridDrinks.Rows.Clear();
                LoadDrinks();
                gridDrinks.Refresh();
                MessageBox.Show(name + " added successfully.");
            }
        }

        private void btnAddUser_Click(object sender, EventArgs e) {
            String username = txtUsername.Text.Trim();
            String password = txtPassword.Text.Trim();
            String name = txtName.Text.Trim();
            String role = cbbRole.SelectedItem.ToString().Trim();

            if (username == "")
            {
                txtUsername.Focus();
            }
            else if (password == "") {
                txtPassword.Focus();
            }
            else {
                if (name == "") name = "";
                String sql = "INSERT INTO Accounts(username, password, name, role)" +
                    "VALUES('" + username + "', ' " + password + " ', '" + name + "', '" + role + "')";
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.ExecuteNonQuery();
                gridUsers.Rows.Clear();
                LoadUsers();
                gridUsers.Refresh();
                MessageBox.Show("New " + role.ToLower() + " added successfully.");
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
        }
    }
}
