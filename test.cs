using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD_Business;

namespace DVLD
{
    public partial class test : Form
    {
        public test()
        {
            InitializeComponent();
        }

        private void FillData()
        {
            
        }

        private void test_Load(object sender, EventArgs e)
        {
            FillData();
            //MessageBox.Show(dataGridView1.Rows.Count.ToString());
        }

        private void ctrlPersonCardWithFilter1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Button clicked!");
        }

        private void test_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                // to unenable validation for all controls on the form 
                foreach (Control ctrl in this.Controls)
                {
                    ctrl.CausesValidation = false;
                }

                // allow the form to close without being blocked by validation
                e.Cancel = false;
            }
        }
    }
}
