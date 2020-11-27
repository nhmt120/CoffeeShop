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

        public void LoadUsers()
        {
            //List<String> users = new List<String>();

            ArrayList listId = new ArrayList();
            ArrayList listUsername = new ArrayList();
            ArrayList listPassword = new ArrayList();
            ArrayList listName = new ArrayList();
            ArrayList listRole = new ArrayList();

            String sql = "SELECT * FROM Accounts";
            SqlCommand cmd = new SqlCommand(sql, connection);
            SqlDataReader reader = cmd.ExecuteReader();

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

        private void AdminForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to exit?", "Coffee Shop Management System", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.No)
            {
                e.Cancel = true;
            } else {
                Application.Exit();
            }
        }

        private void AdminForm_Load(object sender, EventArgs e)
        {
            String connectionString = "Server=.\\SQLEXPRESS; Database=CFS;Integrated Security=SSPI";
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                //MessageBox.Show("Connect to db.", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                gridUsers.ColumnCount = 5;
                gridUsers.Columns[0].Name = "Account ID";
                gridUsers.Columns[1].Name = "Username";
                gridUsers.Columns[2].Name = "Password";
                gridUsers.Columns[3].Name = "Name";
                gridUsers.Columns[4].Name = "Rolw";
                LoadUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can't connect to db\nError:\n" + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
