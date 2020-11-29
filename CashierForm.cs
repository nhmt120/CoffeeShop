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
    public partial class CashierForm : Form
    {
        private SqlConnection connection;
        List<float> listPrice = new List<float>();

        int listNo = 0;
        float total;

        ArrayList listName = new ArrayList();
        List<int> listQuantity = new List<int>();
        List<float> listSum = new List<float>();

        public CashierForm()
        {
            InitializeComponent();
            Connect();
        }

        private void CashierForm_Load(object sender, EventArgs e)
        {
            gridOrder.ColumnCount = 4;
            gridOrder.Columns[0].Name = "No.";
            gridOrder.Columns[1].Name = "Name";
            gridOrder.Columns[2].Name = "Quantity";
            gridOrder.Columns[3].Name = "Sum";
            Connect();
            LoadDrinks();
        }

        private void btnCheckout_Click(object sender, EventArgs e)
        {
            connection.Open();
            String sql = "INSERT INTO Orders(total, date) VALUES('" + total + "', (SELECT GETDATE()))";
            SqlCommand cmd = new SqlCommand(sql, connection);
            cmd.ExecuteNonQuery();

            int order_id = 1;

            sql = "SELECT order_id FROM Orders t1 WHERE date = (SELECT max([date]) FROM Orders t2)";
            cmd = new SqlCommand(sql, connection);
            cmd.ExecuteNonQuery();
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read()) {
                    order_id = int.Parse(reader["order_id"].ToString());
                }
                reader.Close();
            }

            for (int i = 0; i < listNo; i++)
            {
                sql = "INSERT INTO Order_Detail (order_id, drink_name, quantity) " +
                    "VALUES (" + order_id + ", '" + listName[i] + "', " + listQuantity[i] + ")";
                cmd = new SqlCommand(sql, connection);
                cmd.ExecuteNonQuery();
            }
            


            clearOrder();
            LoadOrder();
            connection.Close();
        }

        private void clearOrder()
        {
            listNo = 0;
            listName.Clear();
            listQuantity.Clear();
            listSum.Clear();
            total = 0;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clearOrder();
            LoadOrder();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

            String value = (String) gridMenu.CurrentCell.Value;
            int itemIndex = gridMenu.CurrentCell.RowIndex;
            int index;
            
            //MessageBox.Show(listPrice[index].ToString());
            foreach (string i in listName)
            {
                
                if (i == value) {
                    index = listName.IndexOf(i);
                    listQuantity[index] += 1;
                    listSum[index] += listPrice[itemIndex];
                    total = 0;
                    foreach (float j in listSum)
                    {
                        total += j;
                    }
                    LoadOrder();
                    return;
                }
            }
            listNo += 1;
            index = listNo;
            listQuantity.Add(1);
            listName.Add(value);
            listSum.Add(listPrice[itemIndex]);

            total = 0;
            foreach (float i in listSum)
            {
                total += i;
            }

            LoadOrder();
             
            //MessageBox.Show("yooo " + listQuantity + " ss " + listQuantity[index]);
        }

        public void LoadOrder() {
            

            gridOrder.Rows.Clear();
            gridOrder.Refresh();

            for (int i = 0; i < listNo; i++)
            {
                DataGridViewRow newRow = new DataGridViewRow();

                newRow.CreateCells(gridOrder);
                newRow.Cells[0].Value = i + 1;
                newRow.Cells[1].Value = listName[i];
                newRow.Cells[2].Value = listQuantity[i];
                newRow.Cells[3].Value = listSum[i];
                gridOrder.Rows.Add(newRow);

            }

            lbTotal.Text = total.ToString();
            

        }

        public void LoadDrinks()
        {
            ArrayList listNamed = new ArrayList();
            ArrayList listStock = new ArrayList();

            String sql = "SELECT name, stock, price FROM Drinks";
            SqlCommand cmd = new SqlCommand(sql, connection);
            SqlDataReader reader = cmd.ExecuteReader();

            gridMenu.ColumnCount = 2;
            gridMenu.Columns[0].Name = "Name";
            gridMenu.Columns[1].Name = "Stock";
            
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    listNamed.Add(reader["name"].ToString());
                    listStock.Add(reader["stock"].ToString());
                    listPrice.Add(float.Parse(reader["price"].ToString()));
                }
                reader.Close();

                //MessageBox.Show(listStock.ToArray().ToString());

                

                for (int i = 0; i < listNamed.Count; i++)
                {
                    DataGridViewRow newRow = new DataGridViewRow();

                    newRow.CreateCells(gridMenu);
                    newRow.Cells[0].Value = listNamed[i];
                    newRow.Cells[1].Value = listStock[i];
                    gridMenu.Rows.Add(newRow);
                }
            }
            connection.Close();
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

        

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
        }

        private void CashierForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to exit?", "Coffee Shop Management System", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void CashierForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
