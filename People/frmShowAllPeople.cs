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
        private static DataTable _dtAllPeople = clsPerson.GetAllPeople();

        //only select the columns that you want to show in the grid
        private DataTable _dtPeople = _dtAllPeople.DefaultView.ToTable(false, "PersonID", "NationalNo",
                                                         "FirstName", "SecondName", "ThirdName", "LastName",
                                                         "GendorCaption", "DateOfBirth", "CountryName",
                                                         "Phone", "Email");

        private static DataTable _dtCountries = clsCountry.GetAllCountries();

        private void _RefreshPeopleList()
        {
            _dtAllPeople = clsPerson.GetAllPeople();
            _dtPeople = _dtAllPeople.DefaultView.ToTable(false, "PersonID", "NationalNo",
                                                       "FirstName", "SecondName", "ThirdName", "LastName",
                                                       "GendorCaption", "DateOfBirth", "CountryName",
                                                       "Phone", "Email");

            dgvPeople.DataSource = _dtPeople;
            lblRecords.Text = dgvPeople.Rows.Count.ToString();
            fillcmbSortedBy();
            ChangeSorting();
        }

        private void fillcmbSortedBy()
        {
            foreach (var item in _dtPeople.Columns)
            {
                cmbSortedBy.Items.Add(item);
            }
            cmbSortedBy.DisplayMember = "ColumnName";

            cmbSortedBy.SelectedIndex = 0;
        }
        private void fillcmbSearchBy()
        {
            cmbSearchBy.Items.Add("PersonID");
            cmbSearchBy.Items.Add("NationalNo");

            cmbSearchBy.DisplayMember = "ColumnName";
            cmbSearchBy.SelectedIndex = 0;

        }
        private void ChangeSorting()
        {
            _dtPeople.DefaultView.Sort = cmbSortedBy.SelectedItem.ToString() + " ASC";
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
                        _dtCountries.DefaultView.RowFilter = $"{cmbSearchBy.SelectedItem} = {Convert.ToInt32(txtSearchBy.Text)}";
                    }
                }
                else
                {

                    _dtCountries.DefaultView.RowFilter = $"{cmbSearchBy.SelectedItem} LIKE '%{txtSearchBy.Text}%'";
                    
                }
            }
        }

        private void frmShowAllPeople_Load(object sender, EventArgs e)
        {
            _RefreshPeopleList();
            fillcmbSearchBy();



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
            frm.Close();
            _RefreshPeopleList();
        }

        private void editeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson(int.Parse(dgvPeople.CurrentRow.Cells[0].Value.ToString()));
            frm.ShowDialog();
            frm.Close();
            _RefreshPeopleList();
            
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonID = int.Parse(dgvPeople.CurrentRow.Cells[0].Value.ToString());
            if (MessageBox.Show("Are you sure you want to delete this person?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                if (clsPerson.Delete(PersonID))
                {
                    MessageBox.Show($"Person with ID {PersonID} deleted successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _RefreshPeopleList();
                }
                else
                {
                    MessageBox.Show($"Failed to delete person with ID {PersonID}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                lblRecords.Text = dgvPeople.RowCount.ToString();
            }
            else
            {
                MessageBox.Show("Deletion cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _RefreshPeopleList();
            lblRecords.Text = dgvPeople.RowCount.ToString();
            ChangeSorting();
        }


        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonID = int.Parse(dgvPeople.CurrentRow.Cells[0].Value.ToString());

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
            this.Close();
        }

        private void btnAddNewPerson_Click_1(object sender, EventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson();
            frm.ShowDialog();
            frm.Close();
            _RefreshPeopleList();


        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void txtSearchBy_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSearchBy.Text))
            {
                _dtCountries.DefaultView.RowFilter = string.Empty; // Clear the filter when the search box is empty
            }
            //else
            //{
            //    Search();
            //}
        }

        private void txtSearchBy_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Search();
                e.Handled = true; // Prevent the default behavior of the Enter key
            }
        }

    }
}
