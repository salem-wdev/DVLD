using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD.People.Forms;
using DVLD_Business;

namespace DVLD.People.Controls
{
    public partial class ctrlPersonCardWithFilter : UserControl
    {
        public ctrlPersonCardWithFilter()
        {
            InitializeComponent();
        }

        private void _FillcbFilterBy()
        {
            cbFilterBy.Items.Clear();
            cbFilterBy.Items.Add("ID");
            cbFilterBy.Items.Add("NationalNo");

            cbFilterBy.SelectedIndex = 0;
        }

        private bool _IsValidInput()
        {
            if (string.IsNullOrWhiteSpace(txtFilterValue.Text))
            {
                MessageBox.Show("Please enter a value to filter by.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (cbFilterBy.SelectedItem.ToString() == "ID" && !int.TryParse(txtFilterValue.Text, out _))
            {
                MessageBox.Show("Please enter a valid integer for ID.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (cbFilterBy.SelectedItem.ToString() == "NationalNo" && txtFilterValue.Text.Length > 10)
            {
                MessageBox.Show("Please enter a valid 10-digit National Number.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void _FindPerson()
        {
            if (cbFilterBy.SelectedItem.ToString() == "ID")
            {
                clsPerson person = clsPerson.Find(int.Parse(txtFilterValue.Text));
                if (person != null)
                {
                    ctrlPersonCard1.LoadData(person);
                    return;
                }
            }
            else if (cbFilterBy.SelectedItem.ToString() == "NationalNo")
            {
                clsPerson person = clsPerson.Find(txtFilterValue.Text);
                if (person != null)
                {
                    ctrlPersonCard1.LoadData(person);
                    return;
                }
            }

            MessageBox.Show($"No person found with the given {cbFilterBy.SelectedItem.ToString()}.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void FillPersonCard(object sender,clsPerson person)
        {
            if (person != null)
            {
                txtFilterValue.Text = person.PersonID.ToString();
                ctrlPersonCard1.LoadData(person);
            }
            else
            {
                MessageBox.Show("No person found with the given criteria.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (_IsValidInput())
            {
                _FindPerson();
            }
        }

        private void ctrlPersonCardWithFilter_Load(object sender, EventArgs e)
        {
            _FillcbFilterBy();
        }

        private void btnAddNewPerson_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson();
            frm.SendDataBack += FillPersonCard;
            frm.ShowDialog();
        }

        private void txtFilterValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (_IsValidInput())
                {
                    _FindPerson();
                }
            }
        }
    }
}
