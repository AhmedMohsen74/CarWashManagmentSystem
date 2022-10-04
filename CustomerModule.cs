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
    public partial class CustomerModule : Form
    {
        SqlCommand cm = new SqlCommand();
        dbConnect dbcon = new dbConnect();
        string title = "Car Wash Management System";
        bool check = false;
        public int vid=0;
        Customer customer;
        string CS = ConfigurationManager.ConnectionStrings["CS"].ConnectionString;

        public CustomerModule(Customer cust)
        {
            InitializeComponent();
            customer = cust;            
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
                        if (MessageBox.Show("Are you sure you want to register this Customer?", "Customer Registration", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            string query = "InsertCustomerData";
                            SqlCommand cmd = new SqlCommand(query, cn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            SqlParameter[] param = new SqlParameter[7];


                            param[0] = new SqlParameter("@vid", SqlDbType.Int);
                            param[0].Value = cbCarType.SelectedValue;

                            param[1] = new SqlParameter("@name", SqlDbType.NVarChar, 100);
                            param[1].Value = txtName.Text;

                            param[2] = new SqlParameter("@phone", SqlDbType.NVarChar, 11);
                            param[2].Value = txtPhone.Text;

                            param[3] = new SqlParameter("@carno", SqlDbType.NVarChar, 11);
                            param[3].Value = txtCarNo.Text;


                            param[4] = new SqlParameter("@Carmodel", SqlDbType.NVarChar, 100);
                            param[4].Value = txtCarModel.Text;


                            param[5] = new SqlParameter("@address", SqlDbType.Text);
                            param[5].Value = txtAddress.Text;

                            param[6] = new SqlParameter("@points", SqlDbType.Int);
                            param[6].Value = udPoints.Text;

                            cmd.Parameters.AddRange(param);
                            cn.Open();
                            cmd.ExecuteNonQuery();
                            cn.Close();
                            Clear();
                            MessageBox.Show("Add Successfully", "Carwash", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            customer.loadCustomer();
                            check = false;
                            Clear();//to clear data field, after data inserted into the database                        
                        }
                    }
                    customer.loadCustomer();
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
                        string query = "UpdateCustomerData";
                        SqlCommand cmd = new SqlCommand(query, cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter[] param = new SqlParameter[8];

                        param[0] = new SqlParameter("@id", SqlDbType.Int);
                        param[0].Value = lblCid.Text;

                        param[1] = new SqlParameter("@vid", SqlDbType.Int);
                        param[1].Value = cbCarType.SelectedValue;

                        param[2] = new SqlParameter("@name", SqlDbType.NVarChar, 100);
                        param[2].Value = txtName.Text;

                        param[3] = new SqlParameter("@phone", SqlDbType.NVarChar, 11);
                        param[3].Value = txtPhone.Text;

                        param[4] = new SqlParameter("@carno", SqlDbType.NVarChar, 50);
                        param[4].Value = txtCarNo.Text;

                        param[5] = new SqlParameter("@Carmodel", SqlDbType.NVarChar, 50);
                        param[5].Value = txtCarModel.Text;

                        param[6] = new SqlParameter("@address", SqlDbType.Text);
                        param[6].Value = txtAddress.Text;

                        param[7] = new SqlParameter("@points", SqlDbType.Int);
                        param[7].Value = udPoints.Text;

                        cmd.Parameters.AddRange(param);
                        cn.Open();
                        cmd.ExecuteNonQuery();
                        cn.Close();
                        MessageBox.Show("Succsessfully updated!", "CarWash", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Dispose();
                        customer.loadCustomer();
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

        // this is load to start form
        private void CustomerModule_Load(object sender, EventArgs e)
        {
            // to add vehicle list in the combobox
            cbCarType.DataSource = vehicleType();
            cbCarType.DisplayMember = "vname";
            cbCarType.ValueMember = "id";
            if (vid > 0)
                cbCarType.SelectedValue = vid;
        }

        #region method
        // to create a function vehicle type for return data table of vehicle type
        public DataTable vehicleType()
        {
            cm = new SqlCommand("SELECT * FROM VehicleTypeTBL", dbcon.connect());
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable dataTable = new DataTable();

            adapter.SelectCommand = cm;
            adapter.Fill(dataTable);

            return dataTable;
        }


        // to create a function for data field
        public void Clear()
        {
            txtAddress.Clear();
            txtCarModel.Clear();
            txtCarNo.Clear();
            txtName.Clear();
            txtPhone.Clear();

            cbCarType.SelectedIndex = 0;
            udPoints.Value = 0;

            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
        }

        public void checkField()
        {
            if (txtAddress.Text == "" || txtName.Text == "" || txtPhone.Text == "" || txtCarNo.Text == ""|| txtCarModel.Text=="")
            {
                MessageBox.Show("Required data Field!", "Warning");
                return; // return to the data field and form
            }
            
            check = true;
        }
        #endregion method
    }
}
