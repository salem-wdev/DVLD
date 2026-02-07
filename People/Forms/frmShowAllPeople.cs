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
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
        }

        private void FillDataGridView()
        {
            dgvShowAllPeople.DataSource = clsPerson.GetAllPeople();
        }


        private void frmShowAllPeople_Load(object sender, EventArgs e)
        {
            FillDataGridView();
            lblRecords.Text = dgvShowAllPeople.RowCount.ToString();
        }

        private void btnAddNewPerson_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson();
            frm.ShowDialog();
            frm.Dispose();
            dgvShowAllPeople.Update();
        }

        private void addNewPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson();
            frm.ShowDialog();
            frm.Dispose();
        }

        private void editeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson(int.Parse(dgvShowAllPeople.CurrentRow.Cells[0].Value.ToString()));
            frm.ShowDialog();
            frm.Dispose();
            dgvShowAllPeople.Update();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonID = int.Parse(dgvShowAllPeople.CurrentRow.Cells[0].Value.ToString());
            if (MessageBox.Show("Are you sure you want to delete this person?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                clsPerson.Delete(PersonID);
                dgvShowAllPeople.Update();
                dgvShowAllPeople.DataSource = clsPerson.GetAllPeople();
                lblRecords.Text = dgvShowAllPeople.RowCount.ToString();
            }
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dgvShowAllPeople.DataSource = clsPerson.GetAllPeople();
            lblRecords.Text = dgvShowAllPeople.RowCount.ToString();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson();
            frm.ShowDialog();
            frm.Dispose();
            dgvShowAllPeople.Update();
            dgvShowAllPeople.DataSource = clsPerson.GetAllPeople();

        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonID = int.Parse(dgvShowAllPeople.CurrentRow.Cells[0].Value.ToString());

            frmShowPersonInfo frm = new frmShowPersonInfo(PersonID);
            frm.ShowDialog();
            frm.Dispose();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
    }
}
