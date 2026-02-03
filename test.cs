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
            dataGridView1.DataSource = (DataTable)clsPerson.GetAllPeople();
            dataGridView1.Update();
        }

        private void test_Load(object sender, EventArgs e)
        {
            FillData();
            //MessageBox.Show(dataGridView1.Rows.Count.ToString());
        }
    }
}
