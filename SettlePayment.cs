using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace CarWashManagementSystem
{
    public partial class SettlePayment : Form
    {
        SqlCommand cm = new SqlCommand();
        dbConnect dbcon = new dbConnect();        
        string title = "Car Wash Management System";
        Cash cash;
        public string CS = ConfigurationManager.ConnectionStrings["CS"].ConnectionString;

        public SettlePayment(Cash cashform)
        {
            InitializeComponent();
            cash = cashform;
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btn0_Click(object sender, EventArgs e)
        {
        }

        private void btnPoint_Click(object sender, EventArgs e)
        {

        }

        private void btn1_Click(object sender, EventArgs e)
        {
        }

        private void btn2_Click(object sender, EventArgs e)
        {
        }

        private void btn3_Click(object sender, EventArgs e)
        {
        }

        private void btn4_Click(object sender, EventArgs e)
        {
        }

        private void btn5_Click(object sender, EventArgs e)
        {
        }

        private void btn6_Click(object sender, EventArgs e)
        {
        }

        private void btn7_Click(object sender, EventArgs e)
        {
        }

        private void btn8_Click(object sender, EventArgs e)
        {
        }

        private void btn9_Click(object sender, EventArgs e)
        {
        }

        private void btn00_Click(object sender, EventArgs e)
        {
        }

        private void btnClean_Click(object sender, EventArgs e)
        {
            txtCash.Clear();
            txtCash.Focus();// cusor focus int textbox of cash
        }

        private void btnEnter_Click(object sender, EventArgs e)
        {
            using (SqlConnection cn = new SqlConnection(CS))
            {
                try
                {
                    if (double.Parse(txtChange.Text) < 0 || txtCash.Text.Equals(""))
                    {
                        MessageBox.Show("Insufficient amount, Please enter the corret amount!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else
                    {
                        for (int i = 0; i < cash.dgvCash.Rows.Count; i++)
                        {
                            string query = "UPDATE CashTBl SET status='Sold',price='" + cash.dgvCash.Rows[i].Cells[9].Value.ToString() + "'WHERE id='" + cash.dgvCash.Rows[i].Cells[1].Value.ToString() + "'";
                            SqlCommand cmd = new SqlCommand(query, cn);
                            cn.Open();
                            cmd.ExecuteNonQuery();
                            cn.Close();

                            cn.Open();     //if i need each order have one point leave this iteration out of the loop
                            string query2 = "UPDATE CustomerTbl set points+=" + 1 + " WHERE id='" + cash.customerId + "'";
                            SqlCommand cmddd2 = new SqlCommand(query2, cn);
                            cmddd2.ExecuteNonQuery();
                            cn.Close();
                        }

                        receipt module = new receipt(cash);
                        module.loadReceipt(txtCash.Text, txtChange.Text);
                        module.ShowDialog();


                        MessageBox.Show("Payment successfully saved!", "Payment", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cash.loadCash();
                        this.Dispose();
                        cash.btnAddCustomer.Enabled = true;
                        cash.btnAddService.Enabled = false;
                        cash.getTransno();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, title);
                }
            }
        }

        private void txtCash_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double charge = double.Parse(txtCash.Text) - double.Parse(txtSale.Text);
                txtChange.Text = charge.ToString("#,##0.00");
            }
            catch (Exception)
            {
                txtChange.Text = "0.00";
            }
        }

        private void SettlePayment_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                btnEnter.PerformClick();// action click enter button
            }
            else if(e.KeyCode==Keys.Escape)
            {
                this.Dispose();
            }
        }
    }
}
