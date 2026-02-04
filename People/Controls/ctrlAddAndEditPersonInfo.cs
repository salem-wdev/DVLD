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

namespace DVLD
{
    public partial class ctrlAddAndEditPersonInfo : UserControl
    {
        enum enMode
        {
            AddNew,
            EditExisting
        }

        private enMode _Mode;

        clsPerson _Person;

        private void FillPerson()
        {
            _Person.FirstName = txtSecondName.Text;
        }

        public ctrlAddAndEditPersonInfo()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
            _Person = new clsPerson();
            dtpDateOfBirth.Value = DateTime.Now;
        }

        public ctrlAddAndEditPersonInfo(clsPerson Person)
        {
            InitializeComponent();
            _Mode = enMode.EditExisting;
            _Person = Person;
        }

        private void FillPersonWithData()
        {
            _Person.FirstName = txtFirstName.Text;
            _Person.SecondName = txtSecondName.Text;
            _Person.ThirdName = txtThirdName.Text;
            _Person.LastName = txtLastName.Text;
            _Person.NationalNo = txtNationalNo.Text;
            _Person.DateOfBirth = dtpDateOfBirth.Value;
            _Person.Gender = (short)(rbMale.Checked ? 0 : 1);
            _Person.Address = txtAddress.Text;
            _Person.Phone = txtPhone.Text;
            _Person.Email = txtEmail.Text;
            _Person.NationalityCountryID = (int)cmbNationality.SelectedValue;
            _Person.ImagePath = pbPersonPhoto.ImageLocation;

        }

        private void SavePerson()
        {
            FillPersonWithData();
           _Person.Save();
        }

        private void ctrlAddAndEditPersonInfo_Load(object sender, EventArgs e)
        {
            clsCountry country = new clsCountry();
            cmbNationality.DataSource = clsCountry.GetAllCountries();
            cmbNationality.DisplayMember = "CountryName";
            cmbNationality.ValueMember = "CountryID";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SavePerson();
            this.ParentForm.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.ParentForm.Close();
        }

        private void txtFirstName_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(txtFirstName.Text))
            {
                errorProvider1.SetError(txtFirstName, "First Name is required.");
                e.Cancel = true;
            }
            else
            {
                errorProvider1.SetError(txtFirstName, string.Empty);
            }
        }

        private void txtSecondName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSecondName.Text))
            {
                errorProvider1.SetError(txtSecondName, "Second Name is required.");
                e.Cancel = true;
            }
            else
            {
                errorProvider1.SetError(txtSecondName, string.Empty);
            }
        }

        private void txtThirdName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtThirdName.Text))
            {
                errorProvider1.SetError(txtThirdName, "Third Name is required.");
                e.Cancel = true;
            }
            else
            {
                errorProvider1.SetError(txtThirdName, string.Empty);
            }
        }

        private void txtLastName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLastName.Text))
            {
                errorProvider1.SetError(txtLastName, "Last Name is required.");
                e.Cancel = true;
            }
            else
            {
                errorProvider1.SetError(txtLastName, string.Empty);
            }
        }

        private void txtNationalNo_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNationalNo.Text))
            {
                errorProvider1.SetError(txtNationalNo, "National No is required.");
                e.Cancel = true;
            }
            else
            {
                errorProvider1.SetError(txtNationalNo, string.Empty);
            }
        }

        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            if(!txtEmail.Text.Contains("@"))
            {
                errorProvider1.SetError(txtEmail, "Invalid email format.");
                e.Cancel = true;
            }
            else
            {
                errorProvider1.SetError(txtEmail, string.Empty);
            }
        }

        private void txtPhone_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                errorProvider1.SetError(txtPhone, "Phone is required.");
                e.Cancel = true;
            }
            else
            {
                errorProvider1.SetError(txtPhone, string.Empty);
            }
        }

        private void txtAddress_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                errorProvider1.SetError(txtAddress, "Address is required.");
                e.Cancel = true;
            }
            else
            {
                errorProvider1.SetError(txtAddress, string.Empty);
            }
        }
    }
}
