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
    public partial class ManageCostofGoodSold : Form
    {
        SqlCommand cm = new SqlCommand();
        dbConnect dbcon = new dbConnect();
        string title = "Car Wash Management System";
        Setting setting;
        public string CS = ConfigurationManager.ConnectionStrings["CS"].ConnectionString;

        bool check = false;
        public ManageCostofGoodSold(Setting sett)
        {
            InitializeComponent();
            setting = sett;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (SqlConnection cn = new SqlConnection(CS))
            {
                try
                {
                    checkField();
                    if (check)
                    {
                        try
                        {
                            string query = "InsertCOSTData";
                            SqlCommand cmd = new SqlCommand(query, cn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            SqlParameter[] param = new SqlParameter[3];

                            param[0] = new SqlParameter("@costName", SqlDbType.NVarChar, 100);
                            param[0].Value = txtCostName.Text;

                            param[1] = new SqlParameter("@cost", SqlDbType.NVarChar, 100);
                            param[1].Value = txtCost.Text;

                            param[2] = new SqlParameter("@date", SqlDbType.DateTime);
                            param[2].Value = dtCoG.Text;

                            cmd.Parameters.AddRange(param);
                            cn.Open();
                            cmd.ExecuteNonQuery();
                            cn.Close();
                            MessageBox.Show("Add Successfully", "Carwash", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Clear();
                            setting.loadCostofGood();
                            MainForm m = new MainForm();
                            m.loadGrossProfit();
                            this.Dispose();
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
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, title);
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                checkField();
                if (check)
                {
                    using (SqlConnection cn = new SqlConnection(CS))
                    {
                        try
                        {
                            string query = "UpdateCOSTData";
                            SqlCommand cmd = new SqlCommand(query, cn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            SqlParameter[] param = new SqlParameter[4];

                            param[0] = new SqlParameter("id", SqlDbType.Int);
                            param[0].Value = lblCid.Text;

                            param[1] = new SqlParameter("costName", SqlDbType.NVarChar, 100);
                            param[1].Value = txtCostName.Text;

                            param[2] = new SqlParameter("cost", SqlDbType.NVarChar, 100);
                            param[2].Value = txtCost.Text;

                            param[3] = new SqlParameter("date", SqlDbType.DateTime);
                            param[3].Value = dtCoG.Text;

                            cmd.Parameters.AddRange(param);
                            cn.Open();
                            cmd.ExecuteNonQuery();
                            cn.Close();
                            MessageBox.Show("Succsessfully updated!", "CarWash", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            setting.loadCostofGood();
                            this.Dispose();
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void txtCost_KeyPress(object sender, KeyPressEventArgs e)
        {
            // only allow digit number
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
            // only allow one decimal 
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }
        #region method

        public void checkField()
        {
            if (txtCostName.Text == ""|| txtCost.Text=="")
            {
                MessageBox.Show("Required data field!", "Warning");
                return; // return to the data field and form
            }
            check = true;
        }

        public void Clear()
        {
            txtCost.Clear();
            txtCostName.Clear();
            dtCoG.Value = DateTime.Now;

            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
        }
        #endregion method
    }
}
