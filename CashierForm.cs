using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoffeeShop
{
    public partial class CashierForm : Form
    {
        public CashierForm()
        {
            InitializeComponent();
        }

        private void CashierForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to exit?", "Coffee Shop Management System", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.No)
            {
                e.Cancel = true;
            } else {
                Application.Exit();
            }
        }
    }
}
