using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarWashManagementSystem
{
    public partial class Forgetpass : Form
    {
        login l;

        public Forgetpass(login log)
        {
            InitializeComponent();
            l = log;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (Security.Text == "213.239.207.78:33036")
            {
                MessageBox.Show("Welcome", "Carwash", MessageBoxButtons.OK, MessageBoxIcon.Information);
                l.Hide();

                MainForm s = new MainForm();
                this.Hide();
                s.Show();
            }
            else
            {
                MessageBox.Show("Wrong Security key your are not admin", "Carrwash", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
