using DVLD_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.People.Forms
{
    public partial class frmShowAllPeople : Form
    {
        public frmShowAllPeople()
        {
            InitializeComponent();
        }

        private void FillDataGridView()
        {
            dataGridView1.DataSource = clsPerson.GetAllPeople();
        }


        private void frmShowAllPeople_Load(object sender, EventArgs e)
        {
            FillDataGridView();
        }

        private void btnAddNewPerson_Click(object sender, EventArgs e)
        {
            frmAddNewPerson frm = new frmAddNewPerson();
            frm.ShowDialog();
            frm.Dispose();
            dataGridView1.Refresh();
        }
    }
}
