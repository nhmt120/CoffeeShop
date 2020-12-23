using System;
using System.Collections;
using System.Data.SqlClient;
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

                LoadUsers();
                LoadDrinks();
                LoadHistory();
                cbbRole.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can't connect to db\nError:\n" + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void LoadHistory()
        {
            // display all order histories in db
            ArrayList listId = new ArrayList();
            ArrayList listTotal = new ArrayList();
            ArrayList listDate = new ArrayList();

            String sql = "SELECT * FROM Orders";
            SqlCommand cmd = new SqlCommand(sql, connection);
            SqlDataReader reader = cmd.ExecuteReader();

            gridHistory.ColumnCount = 3;
            gridHistory.Columns[0].Name = "Order ID";
            gridHistory.Columns[1].Name = "Total";
            gridHistory.Columns[2].Name = "Time";

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    listId.Add(reader["order_id"].ToString());
                    listTotal.Add(reader["total"].ToString());
                    listDate.Add(reader["date"].ToString());
                }


                for (int i = 0; i < listId.Count; i++)
                {
                    DataGridViewRow newRow = new DataGridViewRow();

                    newRow.CreateCells(gridHistory);
                    newRow.Cells[0].Value = listId[i];
                    newRow.Cells[1].Value = listTotal[i];
                    newRow.Cells[2].Value = listDate[i];
                    gridHistory.Rows.Add(newRow);
                }
            }
            reader.Close();
        }

        public void LoadUsers()
        {
            // display all users in db
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

            gridUsers.Columns[0].ReadOnly = true;

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
                this.gridUsers.CurrentCell = this.gridUsers[0, listId.Count - 1];
            }
            reader.Close();
            txtUsername.Clear();
            txtPassword.Clear();
            txtName.Clear();
        }

        public void LoadDrinks()
        {
            // display all current drinks in db
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

            gridDrinks.Columns[0].ReadOnly = true;

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
            reader.Close();
            this.gridDrinks.CurrentCell = this.gridDrinks[0, listId.Count - 1];
            txtDrinkName.Clear();
            txtDrinkPrice.Clear();
            txtDrinkStock.Clear();
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

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            String username = txtUsername.Text.Trim();
            String password = txtPassword.Text.Trim();
            String name = txtName.Text.Trim();
            String role = cbbRole.SelectedItem.ToString().Trim();

            if (username == "")
            {
                txtUsername.Focus();
            }
            else if (password == "")
            {
                txtPassword.Focus();
            }
            else
            {
                if (name == "") name = "";
                String sql = "INSERT INTO Accounts(username, password, name, role)" +
                    " VALUES('" + username + "', '" + password + "', '" + name + "', '" + role + "')";
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

        private void AdminForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to exit?", "Coffee Shop Management System", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true;
            } 
        }

        private void AdminForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void btnCashier_Click(object sender, EventArgs e)
        {
            this.Hide();
            CashierForm cashier = new CashierForm();
            cashier.Show();
        }

        private void gridDrinks_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            String id = (String)gridDrinks.CurrentRow.Cells[0].Value.ToString();
            String name = (String)gridDrinks.CurrentRow.Cells[1].Value.ToString();
            String price = (String)gridDrinks.CurrentRow.Cells[2].Value.ToString();
            String stock = (String)gridDrinks.CurrentRow.Cells[3].Value.ToString();

            // update drink detail in db
            String sql = "UPDATE Drinks SET stock = " + stock + ", name = '" + name + "', price = " + price +
                " WHERE drink_id = " + id;
            SqlCommand cmd = new SqlCommand(sql, connection);
            cmd.ExecuteNonQuery();
        }

        private void gridUsers_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            String id = (String)gridUsers.CurrentRow.Cells[0].Value.ToString();
            String username = (String)gridUsers.CurrentRow.Cells[1].Value.ToString();
            String password = (String)gridUsers.CurrentRow.Cells[2].Value.ToString();
            String name = (String)gridUsers.CurrentRow.Cells[3].Value.ToString();
            String role = (String)gridUsers.CurrentRow.Cells[4].Value.ToString();

            // update drink detail in db
            String sql = "UPDATE Accounts SET username = '" + username + "', password = '" + password + "', name = '" + name + "', role = '" + role + "'" +
                " WHERE account_id = " + id;
            SqlCommand cmd = new SqlCommand(sql, connection);
            cmd.ExecuteNonQuery();
        }
    }
}
