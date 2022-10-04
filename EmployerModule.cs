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
    public partial class EmployerModule : Form
    {
        SqlCommand cm = new SqlCommand();
        dbConnect dbcon = new dbConnect();
        string title = "Car Wash Management System";
        bool check = false;
        Employer employer;
        string CS = ConfigurationManager.ConnectionStrings["CS"].ConnectionString;

        public EmployerModule(Employer emp)
        {
            InitializeComponent();
            employer = emp;
            cbRole.SelectedIndex = 3;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        //to insert employer data in the database
        private void btnSave_Click(object sender, EventArgs e)
        {//type try and then double press Tab key
            try
            {
                checkField();
                if (check)
                {
                    using (SqlConnection cn = new SqlConnection(CS))
                    {
                        try
                        {
                            checkField();
                            if (check)
                            {

                                string query = "InsertEmployeeData";
                                SqlCommand cmd = new SqlCommand(query, cn);
                                cmd.CommandType = CommandType.StoredProcedure;

                                SqlParameter[] p = new SqlParameter[8];


                                p[0] = new SqlParameter("@empName", SqlDbType.VarChar, 100);
                                p[0].Value = txtName.Text;

                                p[1] = new SqlParameter("@empDOB", SqlDbType.DateTime);
                                p[1].Value = dtDob.Text;

                                p[2] = new SqlParameter("@empPhone", SqlDbType.VarChar, 11);
                                p[2].Value = txtPhone.Text;

                                p[3] = new SqlParameter("@empAddress", SqlDbType.VarChar);
                                p[3].Value = txtAddress.Text;

                                p[4] = new SqlParameter("@empGenter", SqlDbType.VarChar);
                                p[4].Value = rdMale.Checked ? "Male" : "Female";

                                p[5] = new SqlParameter("@empRole", SqlDbType.VarChar, 50);
                                p[5].Value = cbRole.Text;

                                p[6] = new SqlParameter("@empSalery", SqlDbType.Decimal);
                                p[6].Value = txtSalary.Text;

                                p[7] = new SqlParameter("@empPassword", SqlDbType.VarChar, 100);
                                p[7].Value = txtPassword.Text;

                                cmd.Parameters.AddRange(p);
                                cn.Open();
                                cmd.ExecuteNonQuery();
                                cn.Close();
                                MessageBox.Show("Add Successfully", "Carwash", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                check = false;

                                Clear();

                                employer.loadEmployer();
                            }
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
            using (SqlConnection cn = new SqlConnection(CS))
            {
                try
                {
                    checkField();
                    if (check)
                    {
                        string query = "UpdateEmployeeData";
                        SqlCommand cmd = new SqlCommand(query, cn);
                        cmd.CommandType = CommandType.StoredProcedure;

                        SqlParameter[] p = new SqlParameter[9];

                        p[0] = new SqlParameter("@empID", SqlDbType.Int);
                        p[0].Value = lblEid.Text;

                        p[1] = new SqlParameter("@empName", SqlDbType.VarChar, 100);
                        p[1].Value = txtName.Text;

                        p[2] = new SqlParameter("@empDOB", SqlDbType.DateTime);
                        p[2].Value = dtDob.Text;

                        p[3] = new SqlParameter("@empPhone", SqlDbType.VarChar, 11);
                        p[3].Value = txtPhone.Text;

                        p[4] = new SqlParameter("@empAddress", SqlDbType.VarChar);
                        p[4].Value = txtAddress.Text;

                        p[5] = new SqlParameter("@empGenter", SqlDbType.VarChar);
                        p[5].Value = rdMale.Checked ? "Male" : "Female";

                        p[6] = new SqlParameter("@empRole", SqlDbType.VarChar, 50);
                        p[6].Value = cbRole.Text;

                        p[7] = new SqlParameter("@empSalery", SqlDbType.Decimal);
                        p[7].Value = txtSalary.Text;

                        p[8] = new SqlParameter("@empPassword", SqlDbType.VarChar, 100);
                        p[8].Value = txtPassword.Text;

                        cmd.Parameters.AddRange(p);

                        cn.Open();
                        cmd.ExecuteNonQuery();
                        cn.Close();
                        MessageBox.Show("Succsessfully updated!", "CarWash", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Clear();
                        this.Dispose();
                        employer.loadEmployer();
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
            Clear();
            btnUpdate.Enabled = false;
            btnSave.Enabled = true;
        }

        private void txtSalary_KeyPress(object sender, KeyPressEventArgs e)
        {
            // only allow digit number
            if(!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar !='.'))
            {
                e.Handled = true;
            }
            // only allow one decimal 
            if((e.KeyChar=='.') && ((sender as TextBox).Text.IndexOf('.')>-1))
            {
                e.Handled = true;
            }
        }

        private void cbRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cbRole.Text=="Supervisor" || cbRole.Text=="Worker")
            {
                this.Height = 453 - 26;
                txtPassword.Clear();
                lblPass.Visible = false;// to hide password label and textbox
                txtPassword.Visible = false;
            }
            else
            {
                lblPass.Visible = true;
                txtPassword.Visible = true;
                this.Height = 453;
            }
        }
        // to create a function for clear all field
        #region method
        public void Clear()
        {
            txtAddress.Clear();
            txtName.Clear();
            txtPassword.Clear();
            txtPhone.Clear();
            txtSalary.Clear();

            dtDob.Value = DateTime.Now;
            cbRole.SelectedIndex = 3;//default is worker
        }

        //to check data field and date of birth
        public void checkField()
        {
            if(txtAddress.Text==""||txtName.Text==""||txtPhone.Text==""||txtSalary.Text=="")
            {
                MessageBox.Show("Required data Field!", "Warning");
                return; // return to the data field and form
            }

            if(checkAge(dtDob.Value)<18)
            {
                MessageBox.Show("Employer is under 18!", "Warning");
                return;
            }
            check = true;
        }

        // to check the age and calculate for under 18
        private static int checkAge(DateTime dateofBirth)
        {
            int age = DateTime.Now.Year - dateofBirth.Year;
            if (DateTime.Now.DayOfYear < dateofBirth.DayOfYear)
                age = age - 1;
            return age;
        }

        #endregion method

       
    }
}
