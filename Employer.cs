using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace CarWashManagementSystem
{
    public partial class Employer : Form
    {
        SqlCommand cm = new SqlCommand();
        dbConnect dbcon = new dbConnect();
        SqlDataReader dr;
        string title = "Car Wash Management System";
        string CS = ConfigurationManager.ConnectionStrings["CS"].ConnectionString;

        public Employer()
        {
            InitializeComponent();
            loadEmployer();// to call this function , this form starting
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            EmployerModule module = new EmployerModule(this);
            module.btnUpdate.Enabled = false;// this is for save not for update
            module.ShowDialog();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            loadEmployer();
        }
        private void dgvEmployer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvEmployer.Columns[e.ColumnIndex].Name;
            if(colName=="Edit")
            {
                //to sent employer data to the employer module 
                EmployerModule module = new EmployerModule(this);
                module.lblEid.Text = dgvEmployer.Rows[e.RowIndex].Cells[1].Value.ToString();
                module.txtName.Text = dgvEmployer.Rows[e.RowIndex].Cells[2].Value.ToString();
                module.txtPhone.Text = dgvEmployer.Rows[e.RowIndex].Cells[3].Value.ToString();
                module.txtAddress.Text = dgvEmployer.Rows[e.RowIndex].Cells[4].Value.ToString();
                module.dtDob.Text = dgvEmployer.Rows[e.RowIndex].Cells[5].Value.ToString();
                module.rdMale.Checked = dgvEmployer.Rows[e.RowIndex].Cells[6].Value.ToString()=="Male"?true:false;//like if condition
                module.cbRole.Text = dgvEmployer.Rows[e.RowIndex].Cells[7].Value.ToString();
                module.txtSalary.Text = dgvEmployer.Rows[e.RowIndex].Cells[8].Value.ToString();
                module.txtPassword.Text = dgvEmployer.Rows[e.RowIndex].Cells[9].Value.ToString();

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
                            string query = "DeleteEmployeeData";
                            SqlCommand cmd = new SqlCommand(query, cn);
                            cmd.CommandType = CommandType.StoredProcedure;


                            SqlParameter param = new SqlParameter();
                            param = new SqlParameter("@empID", SqlDbType.Int);
                            param.Value = dgvEmployer.Rows[e.RowIndex].Cells[1].Value.ToString();

                            cmd.Parameters.Add(param);

                            cn.Open();
                            cmd.ExecuteNonQuery();
                            cn.Close();
                            MessageBox.Show("Row deleted successfully", "CarWash", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            loadEmployer();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

            }
        }
        #region method
        //query employer list data form the database to the datagridview 
        public void loadEmployer()
        {
            try
            {
                int i = 0; // to show number for employer list
                dgvEmployer.Rows.Clear();
                cm = new SqlCommand("SELECT * FROM EmployeeTBL WHERE CONCAT (empName,empAddress,empRole) LIKE '%" + txtSearch.Text+"%'",dbcon.connect());
                dbcon.open();
                dr = cm.ExecuteReader();
                while(dr.Read())
                {
                    i++;
                    // to add data to the datagridview from the database
                    dgvEmployer.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), DateTime.Parse(dr[4].ToString()).ToShortDateString(), dr[5].ToString(), dr[6].ToString(), dr[7].ToString(), dr[8].ToString());                    
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
