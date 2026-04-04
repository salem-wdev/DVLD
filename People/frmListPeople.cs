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
    public partial class frmListPeople : Form
    {
        public frmListPeople()
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

        private void _RefreshdgvPeople()
        {
            dgvPeople.DataSource = _dtPeople;
            lblRecords.Text = dgvPeople.Rows.Count.ToString();
        }

        private void _RefreshPeopleList()
        {
            _dtAllPeople = clsPerson.GetAllPeople();
            _dtPeople = _dtAllPeople.DefaultView.ToTable(false, "PersonID", "NationalNo",
                                                       "FirstName", "SecondName", "ThirdName", "LastName",
                                                       "GendorCaption", "DateOfBirth", "CountryName",
                                                       "Phone", "Email");
            _RefreshdgvPeople();
            _RefreshDefaultVeiw();
        }

        private void _RefreshDefaultVeiw()
        {
            _dtPeople.DefaultView.Sort = "PersonID";
        }

        private void _FillcbFilterBy()
        {
            cbFilterBy.Items.Add("None");

            foreach (var item in _dtPeople.Columns)
            {
                cbFilterBy.Items.Add(item);
            }

            cbFilterBy.DisplayMember = "ColumnName";
            cbFilterBy.SelectedIndex = 0;

        }
        private bool _IsValidFilteringInput()
        {
            if (string.IsNullOrEmpty(txtFilterBy.Text))
            {
                errorProvider1.SetError(txtFilterBy, "Please enter a value to search.");
                return false;
            }
            else
            {
                errorProvider1.SetError(txtFilterBy, string.Empty); // Clear previous errors
                return true;
            }
        }
        private void _Filtering()
        {
            if(_IsValidFilteringInput())
            {
                if (cbFilterBy.SelectedItem.ToString() == "PersonID")
                {
                    if (!int.TryParse(txtFilterBy.Text, out _))
                    {
                        errorProvider1.SetError(txtFilterBy, "Please enter a valid integer for PersonID.");
                        return;
                    }
                    else
                    {
                        _dtPeople.DefaultView.RowFilter = $"{cbFilterBy.SelectedItem} = {Convert.ToInt32(txtFilterBy.Text)}";
                    }
                    return;
                }
                else if (cbFilterBy.SelectedItem.ToString() == "DateOfBirth")
                {
                    string input = txtFilterBy.Text.Trim();

                    _dtPeople.DefaultView.RowFilter = string.Format("Convert([DateOfBirth], 'System.String') LIKE '%{0}%'", input);
                    return;
                }

                _dtPeople.DefaultView.RowFilter = $"{cbFilterBy.SelectedItem} LIKE '%{txtFilterBy.Text}%'";
            }
        }

        private void frmShowAllPeople_Load(object sender, EventArgs e)
        {
            _RefreshPeopleList();
            _FillcbFilterBy();

            if (dgvPeople.Rows.Count > 0)
            {

                dgvPeople.Columns[0].HeaderText = "Person ID";
                dgvPeople.Columns[0].Width = 110;

                dgvPeople.Columns[1].HeaderText = "National No.";
                dgvPeople.Columns[1].Width = 120;


                dgvPeople.Columns[2].HeaderText = "First Name";
                dgvPeople.Columns[2].Width = 120;

                dgvPeople.Columns[3].HeaderText = "Second Name";
                dgvPeople.Columns[3].Width = 140;


                dgvPeople.Columns[4].HeaderText = "Third Name";
                dgvPeople.Columns[4].Width = 120;

                dgvPeople.Columns[5].HeaderText = "Last Name";
                dgvPeople.Columns[5].Width = 120;

                dgvPeople.Columns[6].HeaderText = "Gendor";
                dgvPeople.Columns[6].Width = 120;

                dgvPeople.Columns[7].HeaderText = "Date Of Birth";
                dgvPeople.Columns[7].Width = 140;

                dgvPeople.Columns[8].HeaderText = "Nationality";
                dgvPeople.Columns[8].Width = 120;


                dgvPeople.Columns[9].HeaderText = "Phone";
                dgvPeople.Columns[9].Width = 120;


                dgvPeople.Columns[10].HeaderText = "Email";
                dgvPeople.Columns[10].Width = 170;
            }


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

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonID = int.Parse(dgvPeople.CurrentRow.Cells[0].Value.ToString());

            frmShowPersonInfo frm = new frmShowPersonInfo(PersonID);
            frm.ShowDialog();
            frm.Dispose();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddNewPerson_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson();
            frm.ShowDialog();
            frm.Close();
            _RefreshPeopleList();


        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            _Filtering();
        }

        private void txtSearchBy_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFilterBy.Text))
            {
                _dtPeople.DefaultView.RowFilter = string.Empty; // Clear the filter when the search box is empty
            }
            else
            {
                _Filtering();
            }
        }

        private void txtSearchBy_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                _Filtering();
                e.Handled = true; // Prevent the default behavior of the Enter key
            }
        }

        private void dgvPeople_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left && e.RowIndex >= 0)
            {
                dgvPeople.ClearSelection();
                dgvPeople.Rows[e.RowIndex].Selected = true;
                dgvPeople.CurrentCell = dgvPeople.Rows[e.RowIndex].Cells[0];
            }

            if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                dgvPeople.ClearSelection();
                dgvPeople.Rows[e.RowIndex].Selected = true;
                dgvPeople.CurrentCell = dgvPeople.Rows[e.RowIndex].Cells[0];
            }
        }

        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This feature is not implemented yet.", "Not Implemented", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void phoneCallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This feature is not implemented yet.", "Not Implemented", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilterBy.SelectedItem.ToString() == "None" || cbFilterBy.SelectedItem == null)
            {
                txtFilterBy.Text = string.Empty;
                txtFilterBy.Visible = false;

                _RefreshDefaultVeiw();

                return;
            }

            txtFilterBy.Text = string.Empty;
            txtFilterBy.Visible = true;

        }
    }
}
