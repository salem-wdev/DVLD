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
        /// <Logic>
        enum enMode { AddNew, EditExisting }

        private enMode _Mode;

        clsPerson _Person;

        public delegate void DataBackEventHandler(object sender,int PersonID);

        // Define The Delegete
        public event DataBackEventHandler SendDataBack;
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

        public ctrlAddAndEditPersonInfo(int PersonID)
        {
            InitializeComponent();
            _Mode = enMode.EditExisting;
            _Person = clsPerson.Find(PersonID);
        }

        private int SavePerson()
        {
            FillPersonWithData();
           _Person.Save();
            return _Person.PersonID;
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

        public bool FillPersonWithData(int PersonID)
        {
            _Person = clsPerson.Find(PersonID);

            if (_Person != null)
            {
                this.txtFirstName.Text = _Person.FirstName;
                this.txtSecondName.Text = _Person.SecondName;
                this.txtThirdName.Text = _Person.ThirdName;
                this.txtLastName.Text = _Person.LastName;
                this.txtNationalNo.Text = _Person.NationalNo;
                if (_Person.Gender == 0)
                {
                    this.rbMale.Checked = true;
                }
                else
                {
                    this.rbFemale.Checked = true;
                }
                this.txtEmail.Text = _Person.Email;
                this.txtPhone.Text = _Person.Phone;
                this.txtAddress.Text = _Person.Address;
                this.dtpDateOfBirth.Value = _Person.DateOfBirth;
                this.cmbNationality.SelectedValue = _Person.NationalityCountryID;
                this.pbPersonPhoto.ImageLocation = _Person.ImagePath;
                
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool FillPersonWithData(clsPerson Person)
        {
            _Person = Person;
            if (_Person != null)
            {
                this.txtFirstName.Text = _Person.FirstName;
                this.txtSecondName.Text = _Person.SecondName;
                this.txtThirdName.Text = _Person.ThirdName;
                this.txtLastName.Text = _Person.LastName;
                this.txtNationalNo.Text = _Person.NationalNo;
                if (_Person.Gender == 0)
                {
                    this.rbMale.Checked = true;
                }
                else
                {
                    this.rbFemale.Checked = true;
                }
                this.txtEmail.Text = _Person.Email;
                this.txtPhone.Text = _Person.Phone;
                this.txtAddress.Text = _Person.Address;
                this.dtpDateOfBirth.Value = _Person.DateOfBirth;
                this.cmbNationality.SelectedValue = _Person.NationalityCountryID;
                this.pbPersonPhoto.ImageLocation = _Person.ImagePath;

                return true;
            }
            else
            {
                return false;
            }
        }
        /// <Logic>

        private void ctrlAddAndEditPersonInfo_Load(object sender, EventArgs e)
        {
            clsCountry country = new clsCountry();
            cmbNationality.DataSource = clsCountry.GetAllCountries();
            cmbNationality.DisplayMember = "CountryName";
            cmbNationality.ValueMember = "CountryID";
            cmbNationality.SelectedIndex = 190;   // Default to Yemen
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SavePerson();
            SendDataBack?.Invoke(this, _Person.PersonID);
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

        private void llSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.Title = "Select Person Photo";
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pbPersonPhoto.ImageLocation = openFileDialog1.FileName;
                llRemoveImage.Visible = true;
            }
        }

        private void rbMale_CheckedChanged(object sender, EventArgs e)
        {
            pbPersonPhoto.Image = Properties.Resources.man_avatar_icon_flat_vector1;
        }

        private void llRemoveImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pbPersonPhoto.ImageLocation = null;
            pbPersonPhoto.Image = null;
            llRemoveImage.Visible = false;
        }

        private void rbFemale_CheckedChanged(object sender, EventArgs e)
        {
            pbPersonPhoto.Image = Properties.Resources.OIP1;
        }
    }
}
