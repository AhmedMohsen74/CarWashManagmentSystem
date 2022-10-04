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
    public partial class Customer : Form
    {
        SqlCommand cm = new SqlCommand();
        dbConnect dbcon = new dbConnect();
        SqlDataReader dr;
        string CS = ConfigurationManager.ConnectionStrings["CS"].ConnectionString;

        string title = "Car Wash Management System";
        public Customer()
        {
            InitializeComponent();
            loadCustomer();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            CustomerModule module = new CustomerModule(this);
            module.btnUpdate.Enabled = false;
            module.ShowDialog();
        }

        private void dgvCustomer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvCustomer.Columns[e.ColumnIndex].Name;
            if (colName == "Edit")
            {
                //to sent employer data to the customer module 
                CustomerModule module = new CustomerModule(this);
                module.lblCid.Text = dgvCustomer.Rows[e.RowIndex].Cells[1].Value.ToString();
                module.txtName.Text = dgvCustomer.Rows[e.RowIndex].Cells[2].Value.ToString();
                module.txtPhone.Text = dgvCustomer.Rows[e.RowIndex].Cells[3].Value.ToString();
                module.txtCarNo.Text = dgvCustomer.Rows[e.RowIndex].Cells[4].Value.ToString();
                module.txtCarModel.Text = dgvCustomer.Rows[e.RowIndex].Cells[5].Value.ToString();
                module.vid = vehicleIdbyName(dgvCustomer.Rows[e.RowIndex].Cells[6].Value.ToString());
                module.txtAddress.Text = dgvCustomer.Rows[e.RowIndex].Cells[7].Value.ToString();
                module.udPoints.Text = dgvCustomer.Rows[e.RowIndex].Cells[8].Value.ToString();

                module.btnSave.Enabled = false;
                module.udPoints.Enabled = true;
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
                            string query = "DeleteCustomerData";
                            SqlCommand cmd = new SqlCommand(query, cn);
                            cmd.CommandType = CommandType.StoredProcedure;


                            SqlParameter param = new SqlParameter();
                            param = new SqlParameter("@id", SqlDbType.Int);
                            param.Value = dgvCustomer.Rows[e.RowIndex].Cells[1].Value.ToString();

                            cmd.Parameters.Add(param);

                            cn.Open();
                            cmd.ExecuteNonQuery();
                            cn.Close();
                            MessageBox.Show("Row deleted successfully", "CarWash", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            loadCustomer();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            loadCustomer();
        }

        #region method
        public void loadCustomer()
        {
            try
            {
                int i = 0; // to show number for customer list
                dgvCustomer.Rows.Clear();
                cm = new SqlCommand("SELECT C.id,C.name, phone, carno, Carmodel, V.vname, address, points FROM CustomerTbl AS C INNER JOIN VehicleTypeTBL AS V ON C.vid=V.id WHERE CONCAT (C.name,carno,Carmodel,address) LIKE '%" + txtSearch.Text + "%'", dbcon.connect());
                dbcon.open();
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    // to add data to the datagridview from the database
                    dgvCustomer.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString(), dr[7].ToString());
                }
                dbcon.close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title);
            }
        }

        public int vehicleIdbyName(string str)
        {
            int i = 0;
            cm = new SqlCommand("SELECT id FROM VehicleTypeTBL WHERE vname LIKE '" + str + "' ", dbcon.connect());
            dbcon.open();
            dr = cm.ExecuteReader();           
            dr.Read();
            if(dr.HasRows)
            {
                i = int.Parse(dr["id"].ToString());
            }
            dbcon.close();
            return i;
        }
        #endregion method
    }
}
