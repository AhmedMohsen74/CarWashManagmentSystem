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
    public partial class Cash : Form
    {
        SqlCommand cm = new SqlCommand();
        dbConnect dbcon = new dbConnect();
        SqlDataReader dr;
        string title = "Car Wash Management System";
        public int customerId = 0, vehicleTypeId = 0;
        public string carno, carmodel;
        MainForm main;
        string CS = ConfigurationManager.ConnectionStrings["CS"].ConnectionString;

        public Cash(MainForm mainForm)
        {
            InitializeComponent();
            getTransno();
            loadCash();
            main = mainForm;
        }

        private void btnAddCustomer_Click(object sender, EventArgs e)
        {
            openChildForm(new CashCustomer(this));
            btnAddService.Enabled = true;
        }

        private void btnAddService_Click(object sender, EventArgs e)
        {
            openChildForm(new CashService(this));
            btnAddCustomer.Enabled = false;
        }

        private void btnCash_Click(object sender, EventArgs e)
        {
            SettlePayment module = new SettlePayment(this);
            module.txtSale.Text = lblTotal.Text;
            module.ShowDialog();            
            main.loadGrossProfit();
        }
        private void dgvCash_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string columneName = dgvCash.Columns[e.ColumnIndex].Name;

            using (SqlConnection cn = new SqlConnection(CS))
            {
                try
                {
                    if (MessageBox.Show("Are you sure??", "CarWash", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        string query = "DeleteCashData";
                        SqlCommand cmd = new SqlCommand(query, cn);
                        cmd.CommandType = CommandType.StoredProcedure;


                        SqlParameter param = new SqlParameter();
                        param = new SqlParameter("@id", SqlDbType.Int);
                        param.Value = dgvCash.Rows[e.RowIndex].Cells[1].Value.ToString();

                        cmd.Parameters.Add(param);

                        cn.Open();
                        cmd.ExecuteNonQuery();
                        cn.Close();
                        MessageBox.Show("Row deleted successfully", "CarWash", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        loadCash();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        #region method
        // create a function any form to the panelChild on the mainform

        private Form activeForm = null;
        public void openChildForm(Form childForm)
        {
            if (activeForm != null)
                activeForm.Close();
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelCash.Height = 200;
            panelCash.Controls.Add(childForm);
            panelCash.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        // Create a function for transatoin generator depend on date
        private void Cash_Load(object sender, EventArgs e)
        {
        }

        public void getTransno()
        {
            using (SqlConnection cn = new SqlConnection(CS))
            {
                try
                {
                    string sdate = DateTime.Now.ToString("yyyyMMdd");
                    int count;
                    string transno;


                    cn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT TOP 1 TransNUMB FROM CashTBl WHERE TransNUMB LIKE '" + sdate + "%' ORDER BY id DESC", cn);
                    SqlDataReader dr = cmd.ExecuteReader();
                    dr.Read();

                    if (dr.HasRows)
                    {
                        transno = dr[0].ToString();
                        count = int.Parse(transno.Substring(8, 4));
                        lblTransno.Text = sdate + (count + 1);
                    }
                    else
                    {
                        transno = sdate + "1001";
                        lblTransno.Text = transno;
                    }

                    cn.Close();
                    dr.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    cn.Close();
                }
            }
        }
        public void loadCash()
        {
            using (SqlConnection cn = new SqlConnection(CS))
            {
                int i = 0;
                double total = 0;
                double price = 0;
                dgvCash.Rows.Clear();
                SqlCommand cmd = new SqlCommand("SELECT ca.id,ca.TransNUMB,cu.name,cu.carno,cu.Carmodel,v.vname,v.class,s.name,ca.price,ca.date FROM CashTBl AS Ca " +
                          "LEFT JOIN CustomerTbl AS Cu ON CA.cid = Cu.id LEFT JOIN ServiceTBL AS s ON CA.serid = s.id LEFT JOIN VehicleTypeTBL AS v ON Ca.vid = v.id WHERE status LIKE 'Pending' AND Ca.TransNUMB='" + lblTransno.Text + "'", cn);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    price = int.Parse(dr[6].ToString()) * double.Parse(dr[8].ToString());
                    dgvCash.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString(), dr[7].ToString(), price, dr[9].ToString());
                    total += price;
                    carno = dr[3].ToString();
                    carmodel = dr[4].ToString();
                }

                dr.Close();
                cn.Close();
                lblTotal.Text = total.ToString("#,##0.00");
            }
        }
        #endregion method
    }
}
