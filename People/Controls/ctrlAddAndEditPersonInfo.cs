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

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SavePerson();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
