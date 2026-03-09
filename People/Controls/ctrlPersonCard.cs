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
    public partial class ctrlPersonCard : UserControl
    {
        public delegate void BackPersonInfoEventHandler(object sender, clsPerson person);

        public event BackPersonInfoEventHandler BackPersonInfo;

        public ctrlPersonCard()
        {
            InitializeComponent();
        }

        clsPerson _Person;

        public bool Save()
        {
            return _Person.Save();
        }

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
                this.lblCountry.Text = clsCountry.Find(_Person.NationalityCountryID).CountryName;
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
            this.lblCountry.Text = clsCountry.Find(_Person.NationalityCountryID).CountryName;
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
            if (_Person == null)
            {
                MessageBox.Show("No person data to edit.","Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            frmAddUpdatePerson frm = new frmAddUpdatePerson(_Person);
            frm.SendDataBack += BackData;
            frm.ShowDialog();
            frm.Dispose();
        }

        private void gbPersonInfo_Enter(object sender, EventArgs e)
        {
           
        }
    }
}
