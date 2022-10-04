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
    public partial class ServiceModule : Form
    {
        SqlCommand cm = new SqlCommand();
        dbConnect dbcon = new dbConnect();
        string title = "Car Wash Management System";
        Service service;
        public ServiceModule(Service ser)
        {
            InitializeComponent();
            service = ser;
        }
        public string CS = ConfigurationManager.ConnectionStrings["CS"].ConnectionString;

        private void txtPrice_KeyPress(object sender, KeyPressEventArgs e)
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtName.Text == "" || txtPrice.Text=="")
                {
                    MessageBox.Show("Required data field!", "Warning");
                    return; // return to the data field and form
                }
                if (MessageBox.Show("Are you sure you want to register this service?", "Service Registration", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    using (SqlConnection cn = new SqlConnection(CS))
                    {
                        try
                        {
                            string query = "AddServiceData";
                            SqlCommand cmd = new SqlCommand(query, cn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            SqlParameter[] param = new SqlParameter[2];

                            param[0] = new SqlParameter("@name", SqlDbType.NVarChar, 100);
                            param[0].Value = txtName.Text;

                            param[1] = new SqlParameter("@price", SqlDbType.Decimal);
                            param[1].Value = txtPrice.Text;

                            cmd.Parameters.AddRange(param);
                            cn.Open();
                            cmd.ExecuteNonQuery();
                            cn.Close();
                            MessageBox.Show("Added Successfully", "Carwash", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Clear();
                            service.loadService();

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

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtName.Text == "" || txtPrice.Text == "")
                {
                    MessageBox.Show("Required data field!", "Warning");
                    return; // return to the data field and form
                }
                if (MessageBox.Show("Are you sure you want to edit this service?", "Service Editing", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    using (SqlConnection cn = new SqlConnection(CS))
                    {
                        try
                        {
                            string query = "UpdateServiceData";
                            SqlCommand cmd = new SqlCommand(query, cn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            SqlParameter[] param = new SqlParameter[3];

                            param[0] = new SqlParameter("@id", SqlDbType.Int);
                            param[0].Value = lblSid.Text;

                            param[1] = new SqlParameter("@name", SqlDbType.NVarChar, 100);
                            param[1].Value = txtName.Text;

                            param[2] = new SqlParameter("@price", SqlDbType.Decimal);
                            param[2].Value = txtPrice.Text;

                            cmd.Parameters.AddRange(param);
                            cn.Open();
                            cmd.ExecuteNonQuery();
                            cn.Close();
                            MessageBox.Show("Updated Successfully", "Carwash", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Dispose();
                            service.loadService();
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

        #region method
        public void Clear()
        {
            txtName.Clear();
            txtPrice.Clear();

            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
                
        }
        #endregion method
    }
}
