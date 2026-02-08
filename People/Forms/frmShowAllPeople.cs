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
        DataTable dt;
        private void FillDataGridView()
        {
            dt = clsPerson.GetAllPeople();
            dgvShowAllPeople.DataSource = dt;
        }

        private void fillcbSortedBy()
        {
            foreach (var item in dt.Columns)
            {
                cmbSortedBy.Items.Add(item);
            }
        }
        private void ChangeSorting()
        {
            dt.DefaultView.Sort = cmbSortedBy.SelectedItem.ToString() + " ASC";
        }
        private void DeleteRowFromDataGridView()
        {
            dt.Rows.RemoveAt(dgvShowAllPeople.CurrentRow.Index);
        }

        private void frmShowAllPeople_Load(object sender, EventArgs e)
        {
            FillDataGridView();
            lblRecords.Text = dgvShowAllPeople.RowCount.ToString();
            fillcbSortedBy();
            cmbSortedBy.DisplayMember = "ColumnName";

            cmbSortedBy.SelectedIndex = 0;
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
                if (clsPerson.Delete(PersonID))
                {
                    dgvShowAllPeople.Update();

                    DeleteRowFromDataGridView();
                    MessageBox.Show($"Person with ID {PersonID} deleted successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"Failed to delete person with ID {PersonID}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                lblRecords.Text = dgvShowAllPeople.RowCount.ToString();
            }
            else
            {
                MessageBox.Show("Deletion cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            ChangeSorting();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnAddNewPerson_Click_1(object sender, EventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson();
            frm.ShowDialog();
            frm.Dispose();
            dgvShowAllPeople.Update();

        }
    }
}
