using DVLD.People.Forms;
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
    public partial class ctrlShowPersonInfo : UserControl
    {
        public delegate void BackPersonInfoEventHandler(object sender, clsPerson person);

        public event BackPersonInfoEventHandler BackPersonInfo;

        public ctrlShowPersonInfo()
        {
            InitializeComponent();
        }

        clsPerson _Person;

        //public ctrlShowPersonInfo(int PersonID)
        //{
        //    InitializeComponent();

        //    _Person = clsPerson.Find(PersonID);

            
        //}
        //public ctrlShowPersonInfo(clsPerson Person)
        //{
        //    InitializeComponent();

        //    _Person = Person;

        //    // Use the same method to populate UI
        //}

        public bool LoadData(int PersonID)
        {
            _Person = clsPerson.Find(PersonID);

            if (_Person != null)
            {
                this.lblPersonID.Text = _Person.PersonID.ToString();
                this.lblName.Text = _Person.FullName;
                this.lblNationalNo.Text = _Person.NationalNo;
                if (_Person.Gender == 0)
                {
                    this.lblGender.Text = "Male";
                }
                else
                {
                    this.lblGender.Text = "Female";
                }
                this.lblEmail.Text = _Person.Email;
                this.lblPhone.Text = _Person.Phone;
                this.lblAddress.Text = _Person.Address;
                this.lblDateOfBirth.Text = _Person.DateOfBirth.ToShortDateString();
                this.lblCountry.Text = clsCountry.GetCountryName(_Person.NationalityCountryID);
                this.pbPersonPhoto.ImageLocation = _Person.ImagePath;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void LoadData(clsPerson Person)
        {
            _Person = Person;

            this.lblPersonID.Text = _Person.PersonID.ToString();
            this.lblName.Text = _Person.FullName;
            this.lblNationalNo.Text = _Person.NationalNo;
            if (_Person.Gender == 0)
            {
                this.lblGender.Text = "Male";
            }
            else
            {
                this.lblGender.Text = "Female";
            }
            this.lblEmail.Text = _Person.Email;
            this.lblPhone.Text = _Person.Phone;
            this.lblAddress.Text = _Person.Address;
            this.lblDateOfBirth.Text = _Person.DateOfBirth.ToShortDateString();
            this.lblCountry.Text = clsCountry.GetCountryName(_Person.NationalityCountryID);
            this.pbPersonPhoto.ImageLocation = _Person.ImagePath;
        }

        private void BackData(object sender, clsPerson person)
        {
            _Person = person;
            LoadData(_Person);
            BackPersonInfo?.Invoke(this, _Person);
        }

        private void llEditPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson(_Person);
            frm.PersonDataReceivedToForm += BackData;
            frm.ShowDialog();
            frm.Dispose();
        }

        private void gbPersonInfo_Enter(object sender, EventArgs e)
        {
           
        }
    }
}
