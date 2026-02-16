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

        private void fillcmbSortedBy()
        {
            foreach (var item in dt.Columns)
            {
                cmbSortedBy.Items.Add(item);
            }
        }
        private void fillcmbSearchBy()
        {
            cmbSearchBy.Items.Add("PersonID");
            cmbSearchBy.Items.Add("NationalNo");
        }
        private void ChangeSorting()
        {
            dt.DefaultView.Sort = cmbSortedBy.SelectedItem.ToString() + " ASC";
        }
        private bool IsValidSearchInput()
        {
            if (string.IsNullOrEmpty(txtSearchBy.Text))
            {
                errorProvider1.SetError(txtSearchBy, "Please enter a value to search.");
                return false;
            }
            else
            {
                errorProvider1.SetError(txtSearchBy, string.Empty); // Clear previous errors
                return true;
            }
        }
        private void Search()
        {
            if(IsValidSearchInput())
            {
                if (cmbSearchBy.SelectedItem.ToString() == "PersonID")
                {
                    if (!int.TryParse(txtSearchBy.Text, out _))
                    {
                        errorProvider1.SetError(txtSearchBy, "Please enter a valid integer for PersonID.");
                        return;
                    }
                    else
                    {
                        dt.DefaultView.RowFilter = $"{cmbSearchBy.SelectedItem} = {Convert.ToInt32(txtSearchBy.Text)}";
                    }
                }
                else
                {

                    dt.DefaultView.RowFilter = $"{cmbSearchBy.SelectedItem} LIKE '%{txtSearchBy.Text}%'";
                    
                }
            }
        }
        private void DeleteRowFromDataGridView()
        {
            if (dgvShowAllPeople.CurrentRow != null)
            {
                DataRowView drv = (DataRowView)dgvShowAllPeople.CurrentRow.DataBoundItem;
                drv.Delete();        // Delete the row from the DataTable
                dt.AcceptChanges();  // Confirm changes in DataTable
            }
        }

        private void frmShowAllPeople_Load(object sender, EventArgs e)
        {
            FillDataGridView();
            lblRecords.Text = dgvShowAllPeople.RowCount.ToString();
            fillcmbSearchBy();
            fillcmbSortedBy();

            cmbSortedBy.DisplayMember = "ColumnName";

            cmbSortedBy.SelectedIndex = 0;

            cmbSearchBy.DisplayMember = "ColumnName";
            cmbSearchBy.SelectedIndex = 0;

        }

        private void btnAddNewPerson_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson();
            frm.ShowDialog();
            frm.Dispose();
            
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
            
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonID = int.Parse(dgvShowAllPeople.CurrentRow.Cells[0].Value.ToString());
            if (MessageBox.Show("Are you sure you want to delete this person?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                if (clsPerson.Delete(PersonID))
                {
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
            FillDataGridView();
            lblRecords.Text = dgvShowAllPeople.RowCount.ToString();
            ChangeSorting();
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
            

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

            
                Search();
            
        }

        private void txtSearchBy_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSearchBy.Text))
            {
                dt.DefaultView.RowFilter = string.Empty; // Clear the filter when the search box is empty
            }
            //else
            //{
            //    Search();
            //}
        }
    }
}
