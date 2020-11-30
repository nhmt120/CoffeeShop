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
        List<int> stockTemp = new List<int>();

        int listNo = 0;
        float total;

        ArrayList listName = new ArrayList();
        List<int> listQuantity = new List<int>();
        List<float> listSum = new List<float>();
        List<int> listStock = new List<int>();

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
            LoadDrinks(-1);
        }

        private void btnCheckout_Click(object sender, EventArgs e)
        {

            List<int> listStockIndex = new List<int>();
            int order_id = 1;

            for (int i = 0; i < listStock.Count; i++)
            {
                if (stockTemp[i] != listStock[i])
                {
                    listStockIndex.Add(i);
                }
            }

            connection.Open();
            String sql = "INSERT INTO Orders(total, date) VALUES('" + total + "', (SELECT GETDATE()))";
            SqlCommand cmd = new SqlCommand(sql, connection);
            cmd.ExecuteNonQuery();

            //get id of latest order 
            sql = "SELECT order_id FROM Orders t1 WHERE date = (SELECT max([date]) FROM Orders t2)";
            cmd = new SqlCommand(sql, connection);
            cmd.ExecuteNonQuery();
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read()) {
                    order_id = int.Parse(reader["order_id"].ToString());
                }
                
            }
            reader.Close();

            for (int i = 0; i < listNo; i++)
            {
                // order details
                sql = "INSERT INTO Order_Detail (order_id, drink_name, quantity) " +
                    "VALUES (" + order_id + ", '" + listName[i] + "', " + listQuantity[i] + ")";
                cmd = new SqlCommand(sql, connection);
                cmd.ExecuteNonQuery();

                /*
                // update stock
                sql = "UPDATE Order_Detail SET quantity = " + listQuantity[i] +
                    " WHERE order_id = " + order_id + " and drink_name = '" + listName[i] + "'";
                cmd = new SqlCommand(sql, connection);
                cmd.ExecuteNonQuery();
                */

                Console.WriteLine(listStock[listStockIndex[i]] + ", i: " + i + ", listStockIndex[i]: " + listStockIndex[i]);
                sql = "UPDATE Drinks SET stock = " + (listStock[listStockIndex[i]] - listQuantity[i]) +
                    " WHERE drink_id = " + (listStockIndex[i] + 1);
                cmd = new SqlCommand(sql, connection);
                cmd.ExecuteNonQuery();
            }

            connection.Close();

            clearOrder();
            LoadOrder(-1);
            MessageBox.Show("Checkout successfully.", "Checkout", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //LoadDrinks(-1);
            connection.Close();
        }

        private void clearOrder()
        {
            listNo = 0;
            listStock.Clear();
            stockTemp.Clear();
            listName.Clear();
            listQuantity.Clear();
            listSum.Clear();
            total = 0;
            //LoadDrinks(-1);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            //stockTemp = listStock;
            clearOrder();
            LoadOrder(-1);
            //LoadDrinks(-1);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

            //List<int> listItemIndex = new List<int>();
            String value = (String) gridMenu.CurrentCell.Value;
            int itemIndex = gridMenu.CurrentCell.RowIndex;
            int index;
            
            foreach (string i in listName)
            {
                // update quantity of already added item in gridOrder
                if (i == value) {
                    index = listName.IndexOf(i);
                    listQuantity[index] += 1;
                    listSum[index] += listPrice[itemIndex];
                    total = 0;
                    foreach (float j in listSum)
                    {
                        total += j;
                    }
                    
                    LoadOrder(itemIndex);
                    return;
                }
            }

            // add new item in gridOrder
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

            LoadOrder(itemIndex);
             
            //MessageBox.Show("yooo " + listQuantity + " ss " + listQuantity[index]);
        }

        public void LoadOrder(int index) {

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

            LoadDrinks(index);

            lbTotal.Text = total.ToString();
            

        }

        public void LoadDrinks(int index)
        {
            connection.Open();
            gridMenu.Rows.Clear();
            gridMenu.Refresh();

            ArrayList listNamed = new ArrayList();
            List<int> stock = new List<int>();
            
            String sql = "SELECT name, stock, price FROM Drinks";
            SqlCommand cmd = new SqlCommand(sql, connection);
            SqlDataReader reader = cmd.ExecuteReader();

            gridMenu.ColumnCount = 2;
            gridMenu.Columns[0].Name = "Name";
            gridMenu.Columns[1].Name = "Stock";
            
            if (reader.HasRows)
            {
                listStock.Clear();
                while (reader.Read())
                {
                    listNamed.Add(reader["name"].ToString());
                    listStock.Add(int.Parse(reader["stock"].ToString()));
                    stockTemp.Add(int.Parse(reader["stock"].ToString()));
                    stock.Add(int.Parse(reader["stock"].ToString()));
                    listPrice.Add(float.Parse(reader["price"].ToString()));
                }
                

                //MessageBox.Show(listStock.ToArray().ToString());

                if (index != -1)
                {
                    stockTemp[index] -= 1;
                    for (int i = 0; i < listNamed.Count; i++)
                    {
                        DataGridViewRow newRow = new DataGridViewRow();

                        newRow.CreateCells(gridMenu);
                        newRow.Cells[0].Value = listNamed[i];
                        newRow.Cells[1].Value = stockTemp[i];
                        gridMenu.Rows.Add(newRow);
                    }
                } else
                {
                    for (int i = 0; i < listNamed.Count; i++)
                    {
                        DataGridViewRow newRow = new DataGridViewRow();

                        newRow.CreateCells(gridMenu);
                        newRow.Cells[0].Value = listNamed[i];
                        newRow.Cells[1].Value = stock[i];
                        gridMenu.Rows.Add(newRow);
                    }
                }
                

                
            }
            reader.Close();
            connection.Close();
        }

        private void Connect()
        {
            String connectionString = "Server=.\\SQLEXPRESS; Database=CFS;Integrated Security=SSPI";
            try
            {
                connection = new SqlConnection(connectionString);
                //connection.Open();
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
