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
    public partial class ManageVehicleType : Form
    {
        SqlCommand cm = new SqlCommand();
        dbConnect dbcon = new dbConnect();
        string title = "Car Wash Management System";
        Setting setting;
        string CS = ConfigurationManager.ConnectionStrings["CS"].ConnectionString;

        public ManageVehicleType(Setting sett)
        {
            InitializeComponent();
            setting = sett;
            cbClass.SelectedIndex = 0;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {    if(txtName.Text=="")
                {
                    MessageBox.Show("Required vehicle type name!", "Warning");
                    return; // return to the data field and form
                }
                 if (MessageBox.Show("Are you sure you want to register this vehicle type?", "Vehicle Type Registration", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                 {
                    using (SqlConnection cn = new SqlConnection(CS))
                    {
                        try
                        {

                            string query = "InsertVehicleData";
                            SqlCommand cmd = new SqlCommand(query, cn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            SqlParameter[] param = new SqlParameter[2];

                            param[0] = new SqlParameter("@vname", SqlDbType.NVarChar);
                            param[0].Value = txtName.Text;

                            param[1] = new SqlParameter("@class", SqlDbType.NVarChar);
                            param[1].Value = cbClass.Text;

                            cmd.Parameters.AddRange(param);
                            cn.Open();
                            cmd.ExecuteNonQuery();
                            cn.Close();
                            MessageBox.Show("Vehicle Added Successfully", "CarWash", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Clear();
                            setting.loadVehicleType();

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
                if (txtName.Text == "")
                {
                    MessageBox.Show("Required vehicle type name!", "Warning");
                    return; // return to the data field and form
                }
                if (MessageBox.Show("Are you sure you want to edit this vehicle type?", "Vehicle Type Editing", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    using (SqlConnection cn = new SqlConnection(CS))
                    {
                        try
                        {
                            
                                string query = "UpdateVehicleData";
                                SqlCommand cmd = new SqlCommand(query, cn);
                                cmd.CommandType = CommandType.StoredProcedure;
                                SqlParameter[] param = new SqlParameter[3];

                                param[0] = new SqlParameter("id", SqlDbType.Int);
                                param[0].Value = lblVid.Text;

                                param[1] = new SqlParameter("vname", SqlDbType.NVarChar);
                                param[1].Value = txtName.Text;

                                param[2] = new SqlParameter("class", SqlDbType.NVarChar);
                                param[2].Value = cbClass.Text;

                                cmd.Parameters.AddRange(param);
                                cn.Open();
                                cmd.ExecuteNonQuery();
                                cn.Close();
                                MessageBox.Show("Succsessfully updated!", "CarWash", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                setting.loadVehicleType();
                            
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
            cbClass.SelectedIndex = 0;

            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
        }
        #endregion method
    }
}
