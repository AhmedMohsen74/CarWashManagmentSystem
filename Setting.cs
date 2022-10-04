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
    public partial class Setting : Form
    {
        SqlCommand cm = new SqlCommand();
        dbConnect dbcon = new dbConnect();
        SqlDataReader dr;
        string title = "Car Wash Management System";
        bool hasdetail = false;
        public string CS = ConfigurationManager.ConnectionStrings["CS"].ConnectionString;

        public Setting()
        {
            InitializeComponent();
            loadVehicleType();
            loadCostofGood();
            loadCompany();
        }

        #region VehicleType

        private void txtSearchVT_TextChanged(object sender, EventArgs e)
        {
            loadVehicleType();
        }

        private void btnAddVT_Click(object sender, EventArgs e)
        {
            ManageVehicleType module = new ManageVehicleType(this);
            module.btnUpdate.Enabled = false;
            module.ShowDialog();
        }

        private void dgvVehicleType_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvVehicleType.Columns[e.ColumnIndex].Name;
            if (colName == "Edit")
            {
                //to sent vehicle data to the vehicle module 
                ManageVehicleType module = new ManageVehicleType(this);
                module.lblVid.Text = dgvVehicleType.Rows[e.RowIndex].Cells[1].Value.ToString();
                module.txtName.Text = dgvVehicleType.Rows[e.RowIndex].Cells[2].Value.ToString();
                module.cbClass.Text = dgvVehicleType.Rows[e.RowIndex].Cells[3].Value.ToString();
               

                module.btnSave.Enabled = false;
                module.ShowDialog();
            }
            else if (colName == "Delete") // if you want to delete the record to click the delete icon on the datagridview
            {
                using (SqlConnection cn = new SqlConnection(CS))
                {
                    try
                    {
                        if (MessageBox.Show("Are you sure??", "CarWash", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            string query = "DeleteVehicleData";
                            SqlCommand cmd = new SqlCommand(query, cn);
                            cmd.CommandType = CommandType.StoredProcedure;


                            SqlParameter param = new SqlParameter();
                            param = new SqlParameter("@id", SqlDbType.Int);
                            param.Value = dgvVehicleType.Rows[e.RowIndex].Cells[1].Value.ToString();

                            cmd.Parameters.Add(param);

                            cn.Open();
                            cmd.ExecuteNonQuery();
                            cn.Close();
                            MessageBox.Show("Row deleted successfully", "CarWash", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            loadVehicleType();
                            MainForm m = new MainForm();
                            m.loadGrossProfit();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
        public void loadVehicleType()
        {
            try
            {
                int i = 0; // to show number for vehicle list
                dgvVehicleType.Rows.Clear();
                cm = new SqlCommand("SELECT * FROM VehicleTypeTBL WHERE CONCAT (vname,class) LIKE '%" + txtSearchVT.Text + "%'", dbcon.connect());
                dbcon.open();
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    // to add data to the datagridview from the database
                    dgvVehicleType.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString());
                }
                dbcon.close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title);
            }
        }


        #endregion VehicleType

        #region CostofGoodSold
        private void btnAddCoG_Click(object sender, EventArgs e)
        {
            ManageCostofGoodSold module = new ManageCostofGoodSold(this);
            module.btnUpdate.Enabled = false;
            module.ShowDialog();
        }

        private void txtSearchCoG_TextChanged(object sender, EventArgs e)
        {
            loadCostofGood();
        }

        private void dgvCostofGoodSold_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvCostofGoodSold.Columns[e.ColumnIndex].Name;
            if (colName == "EditCoG")
            {
                //to sent cost of good sold data to the manage cost of good sold module 
                ManageCostofGoodSold module = new ManageCostofGoodSold(this);
                module.lblCid.Text = dgvCostofGoodSold.Rows[e.RowIndex].Cells[1].Value.ToString();
                module.txtCostName.Text = dgvCostofGoodSold.Rows[e.RowIndex].Cells[2].Value.ToString();
                module.txtCost.Text = dgvCostofGoodSold.Rows[e.RowIndex].Cells[3].Value.ToString();
                module.dtCoG.Text = dgvCostofGoodSold.Rows[e.RowIndex].Cells[4].Value.ToString();
                module.btnSave.Enabled = false;
                module.ShowDialog();
            }
            else if (colName == "DeleteCoG") // if you want to delete the record to click the delete icon on the datagridview
            {
                using (SqlConnection cn = new SqlConnection(CS))
                {
                    try
                    {
                        if (MessageBox.Show("Are you sure??", "CarWash", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            string query = "DeleteCOSTdata";
                            SqlCommand cmd = new SqlCommand(query, cn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            SqlParameter param = new SqlParameter();
                            param = new SqlParameter("@id", SqlDbType.Int);
                            param.Value = dgvCostofGoodSold.Rows[e.RowIndex].Cells[1].Value.ToString();
                            cmd.Parameters.Add(param);
                            cn.Open();
                            cmd.ExecuteNonQuery();
                            cn.Close();
                            MessageBox.Show("Row deleted successfully", "CarWash", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            loadCostofGood();// to reload the cost of good sold list after edit and update the record
        }

        public void loadCostofGood()
        {
            try
            {
                int i = 0; // to show number for cost of good sole
                dgvCostofGoodSold.Rows.Clear();
                cm = new SqlCommand("SELECT * FROM COSTOFGOOD WHERE CONCAT (id,costName,cost,date) LIKE '%" + txtSearchCoG.Text + "%'", dbcon.connect());
                dbcon.open();
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    // to add data to the datagridview from the database
                    dgvCostofGoodSold.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(),DateTime.Parse(dr[3].ToString()).ToShortDateString());
                }
                dbcon.close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title);
            }
        }
        #endregion CostofGoodSold

        #region Company Detail
        //first we need to load the data from the database
        public void loadCompany()
        {
            try
            {
                dbcon.open();
                cm = new SqlCommand("SELECT * FROM CompanyTBl", dbcon.connect());
                dr = cm.ExecuteReader();
                dr.Read();
                if(dr.HasRows)
                {
                    hasdetail = true;
                    txtComName.Text = dr["name"].ToString();
                    txtComAddress.Text= dr["address"].ToString();
                }
                else
                {
                    txtComName.Clear();
                    txtComAddress.Clear();
                }
                dr.Close();
                dbcon.close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title);
            }

        }
       

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (SqlConnection cn = new SqlConnection(CS))
            {
                try
                {
                    if (MessageBox.Show("Save company detail?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {// now we create a function for execute querry only one line 
                        if (hasdetail)
                        {
                            string query = "UpdateCompany";
                            SqlCommand cmd = new SqlCommand(query, cn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            SqlParameter[] param = new SqlParameter[2];
                            param[0] = new SqlParameter("@name", SqlDbType.NVarChar, 200);
                            param[0].Value = txtComName.Text;

                            param[1] = new SqlParameter("@address", SqlDbType.NVarChar, 500);
                            param[1].Value = txtComName.Text;
                            cmd.Parameters.AddRange(param);

                            cn.Open();
                            cmd.ExecuteNonQuery();
                            cn.Close();
                        }
                        else
                        {
                            string query2 = "insertCompany";
                            SqlCommand cmd2 = new SqlCommand(query2, cn);
                            cmd2.CommandType = CommandType.StoredProcedure;

                            SqlParameter[] param = new SqlParameter[2];
                            param[0] = new SqlParameter("@name", SqlDbType.NVarChar, 200);
                            param[0].Value = txtComName.Text;

                            param[1] = new SqlParameter("@address", SqlDbType.NVarChar, 500);
                            param[1].Value = txtComAddress.Text;

                            cmd2.Parameters.AddRange(param);
                            cn.Open();
                            cmd2.ExecuteNonQuery();
                            cn.Close();
                        }
                        MessageBox.Show("Company detail has been successfully saved!", "Save Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, title);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtComName.Clear();
            txtComAddress.Clear();
        }
        #endregion Company Detail
    }
}
