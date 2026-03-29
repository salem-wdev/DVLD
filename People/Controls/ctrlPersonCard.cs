using DVLD.People.Forms;
using DVLD.Properties;
using DVLD_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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

        private int _PersonID = -1;

        private clsPerson _Person;

        public int PersonID
        {
            get
            {
               return _PersonID;
            }
        }

        public clsPerson SelectedPerson
        {
            get
            {
                return _Person;
            }
        }

        public bool Save()
        {
            if (_Person == null)
            {
                return false;
            }

            bool IsSaved = _Person.Save();
            _PersonID = _Person.PersonID;
            return IsSaved;
        }

        public bool LoadData(int PersonID)
        {
            _Person = clsPerson.Find(PersonID);
            return LoadData(_Person);
        }

        public bool LoadData(string NationalNo)
        {
            _Person = clsPerson.Find(NationalNo);
            return LoadData(_Person);
        }

        public bool LoadData(clsPerson Person)
        {
            _Person = Person;
            if (_Person != null)
            {
                _PersonID = Person.PersonID;
                _FillPersonInfo();
                return true;
            }
            else
            {
                MessageBox.Show("Could not load this person.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ResetPersonInfo();
                return false;
            }
        }

        private void _LoadPersonImage()
        {
            if (_Person.Gender == 0)
                pbPersonPhoto.Image = Resources.Male_512;
            else
                pbPersonPhoto.Image = Resources.Female_512;

            string ImagePath = _Person.ImagePath;
            if (ImagePath != "")
                if (File.Exists(ImagePath))
                    pbPersonPhoto.ImageLocation = ImagePath;
                else
                    MessageBox.Show("Could not find this image: = " + ImagePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void _FillPersonInfo()
        {
            llEditPersonInfo.Enabled = true;
            llEditPersonInfo.TabIndex = 0;
            this.TabIndex = 0;
            _PersonID = _Person.PersonID;
            lblPersonID.Text = _Person.PersonID.ToString();
            lblNationalNo.Text = _Person.NationalNo;
            lblFullName.Text = _Person.FullName;
            lblGender.Text = _Person.Gender == 0 ? "Male" : "Female";
            lblEmail.Text = _Person.Email;
            lblPhone.Text = _Person.Phone;
            lblDateOfBirth.Text = _Person.DateOfBirth.ToShortDateString();
            lblCountry.Text = clsCountry.Find(_Person.NationalityCountryID).CountryName;
            lblAddress.Text = _Person.Address;
            _LoadPersonImage();




        }

        public void ResetPersonInfo()
        {
            _PersonID = -1;
            lblPersonID.Text = "[????]";
            lblNationalNo.Text = "[????]";
            lblFullName.Text = "[????]";
            pbGender.Image = Resources.Man_32;
            lblGender.Text = "[????]";
            lblEmail.Text = "[????]";
            lblPhone.Text = "[????]";
            lblDateOfBirth.Text = "[????]";
            lblCountry.Text = "[????]";
            lblAddress.Text = "[????]";
            pbPersonPhoto.Image = Resources.Male_512;

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
            frm.SendDataBack += BackData;
            frm.ShowDialog();
            frm.Close();
        }

        private void ctrlPersonCard_Load(object sender, EventArgs e)
        {
            llEditPersonInfo.Enabled = false;
        }

        private void gbPersonInfo_Enter(object sender, EventArgs e)
        {

        }
    }
}
