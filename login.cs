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
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
            loadCompany();
        }
        string CS = ConfigurationManager.ConnectionStrings["CS"].ConnectionString;
        SqlDataReader dr;
        public static string empname = "";
        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            using (SqlConnection cn = new SqlConnection(CS))
            {
                try
                {
                    /*
                     Create proc SP_Login
                    @UserName varchar(100), @UserPW varchar(100),@Error varchar(max) OUTPUT
                        as
                    begin try
                    select * from EmployeeTBL where
                    empName=@empName AND empPassword=@empPassword
                    END try
                    begin catch
                    Select @Error='Wrong Username or password'
                    END CATCH
                    */
                    string query = "SELECT empName FROM EmployeeTBL WHERE empName ='" + txtName.Text + "' AND empPassword ='" + txtPassword.Text + "'";
                    SqlCommand cmd = new SqlCommand(query, cn);
                    cn.Open();
                    dr = cmd.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        empname = txtName.Text;
                        MessageBox.Show("WELCOME " + empname, "ACCESS GRANTED", MessageBoxButtons.OK, MessageBoxIcon.Information);
                       
                        this.Hide();
                        MainForm main = new MainForm();
                        main.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Invalid username or password", "ACCESS DENIED", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    cn.Close();
                    dr.Close();
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message); ;
                }

            }

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.facebook.com/A.Mohsen74/");

        }

        private void ShowPass_CheckedChanged(object sender, EventArgs e)
        {
            if (ShowPass.Checked)
                txtPassword.UseSystemPasswordChar = false;
            else
                txtPassword.UseSystemPasswordChar = true;
        }
        public void loadCompany()
        {
            using (SqlConnection cn = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM CompanyTBl", cn);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    lblCompany.Text = dr["name"].ToString();
                    lblAddress.Text = dr["address"].ToString();
                }
                dr.Close();
                cn.Close();
            }

        }

        private void forgotpass_Click(object sender, EventArgs e)
        {
            Forgetpass f = new Forgetpass(this);
            f.ShowDialog();
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
