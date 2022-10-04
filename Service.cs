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
    public partial class Service : Form
    {
        SqlCommand cm = new SqlCommand();
        dbConnect dbcon = new dbConnect();
        SqlDataReader dr;
        string title = "Car Wash Management System";
        public Service()
        {
            InitializeComponent();
            loadService();
        }

        string CS = ConfigurationManager.ConnectionStrings["CS"].ConnectionString;

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ServiceModule module = new ServiceModule(this);
            module.btnUpdate.Enabled = true;
            module.ShowDialog();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            loadService();
        }

        private void dgvService_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvService.Columns[e.ColumnIndex].Name;
            if (colName == "Edit")
            {
                //to sent vehicle data to the vehicle module 
                ServiceModule module = new ServiceModule(this);
                module.lblSid.Text = dgvService.Rows[e.RowIndex].Cells[1].Value.ToString();
                module.txtName.Text = dgvService.Rows[e.RowIndex].Cells[2].Value.ToString();
                module.txtPrice.Text = dgvService.Rows[e.RowIndex].Cells[3].Value.ToString();


                module.btnSave.Enabled = false;
                module.ShowDialog();
            }
            else if (colName == "Delete")
            {
                using (SqlConnection cn = new SqlConnection(CS))
                {
                    try
                    {
                        if (MessageBox.Show("Are you sure??", "CarWash", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            string query = "DeleteServiceData";
                            SqlCommand cmd = new SqlCommand(query, cn);
                            cmd.CommandType = CommandType.StoredProcedure;


                            SqlParameter param = new SqlParameter();
                            param = new SqlParameter("@id", SqlDbType.Int);
                            param.Value = dgvService.Rows[e.RowIndex].Cells[1].Value.ToString();

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
            loadService();// to reload the service list after update the record
        }
        #region method
        public void loadService()
        {
            try
            {
                int i = 0; // to show number for service list
                dgvService.Rows.Clear();
                cm = new SqlCommand("SELECT * FROM ServiceTBL WHERE name LIKE '%" + txtSearch.Text + "%'", dbcon.connect());
                dbcon.open();
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    // to add data to the datagridview from the database
                    dgvService.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString());
                }
                dbcon.close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title);
            }
        }
        #endregion method

        
    }
}
