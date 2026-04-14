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

namespace DVLD.Users
{
    public partial class frmAddUpdateUser : Form
    {
        enum enMode { AddNew, Update }

        enMode _Mode;

        clsUser _User;

        public frmAddUpdateUser()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
            _User = new clsUser();
            tpLoginInfo.Enabled = false;
        }

        public frmAddUpdateUser(int UserID)
        {
            InitializeComponent();
            _Mode = enMode.Update;
            if ((_User = clsUser.Find(UserID)) != null)
            { 
                ctrlPersonCardWithFilter1.LoadPersonInfo(_User.PersonID);
                txtUserName.Text = _User.UserName;
                chkIsActive.Checked = _User.IsActive;
            }

            tcInfo.SelectedIndex = 1;
            tpPesronInfo.Enabled = true;
            ctrlPersonCardWithFilter1.gbFilters.Enabled = false;
            btnNext.Enabled = false;
            txtUserName.Enabled = false;

        }

        private void _ResetValues()
        {
            txtConfirmPassword.Text = string.Empty;
            txtPassword.Text = string.Empty;
        }

        private void _FillUserWithData()
        {
            _User.UserName = txtUserName.Text;
            _User.Password = txtPassword.Text;
            _User.IsActive = chkIsActive.Checked;

            _User.PersonInfo = clsPerson.Find(ctrlPersonCardWithFilter1.PersonID);
        }

        private void _Save()
        {

            _FillUserWithData();

            if(_User.Save())
            {
                _Mode = enMode.Update;
                MessageBox.Show("Saved sauccessfuly", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _ResetValues();
            }
            else
            {
                MessageBox.Show("Save faild!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
                   
            
        }

        private bool _IsPersonUser()
        {
            int PersonID = ctrlPersonCardWithFilter1.PersonID;
            if (clsUser.IsUserExistsForPersonID(PersonID))
            {
                return true;
            }

            return false;
        }

        private bool _IsbtnSaveReadyToEnable()
        {
            if(ctrlPersonCardWithFilter1.SelectedPerson==null)
            {
                return false;
            }

            if(txtUserName.Text == string.Empty)
            {
                return false;
            }

            if(txtPassword.Text == string.Empty)
            {
                return false;
            }

            if(txtConfirmPassword.Text == string.Empty)
            {
                return false;
            }

            return true;
        }

        private void frmAddUpdateUser_Load(object sender, EventArgs e)
        {
            ctrlPersonCardWithFilter1.OnPersonSelected += CtrlPersonCardWithFilter1_OnPersonSelected;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void CtrlPersonCardWithFilter1_OnPersonSelected(int PersonID)
        {

            if (_Mode == enMode.AddNew && _IsPersonUser())
            {
                MessageBox.Show("Person already is a User", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnNext.Enabled = false;
                btnSave.Enabled = false;
                return;
            }
            else
            {
                btnNext.Enabled = true;
            }

            btnNext.Enabled = ctrlPersonCardWithFilter1.PersonID > 0;

            if (!_IsPersonUser())
            {
                tpLoginInfo.Enabled = true;
            }

            if(_IsbtnSaveReadyToEnable())
            {
                btnSave.Enabled = true;
            }
            else
            {
                btnSave.Enabled = false;
            }

            _User.PersonInfo = clsPerson.Find(PersonID);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            // check is person is a user if mode is add new
            if (_Mode == enMode.AddNew && _IsPersonUser())
            {
                MessageBox.Show("Person already is a User", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnNext.Enabled = false;
                return;
            }


            tcInfo.SelectedIndex = tcInfo.SelectedIndex + 1;
        }

        private void User_Validating(object sender, CancelEventArgs e)
        {
            TextBox text = (TextBox)sender;

            if (text.Text== string.Empty)
            {
                errorProvider1.SetError(text, $"{text.Tag.ToString()} is Requered");
            }

            

        }

        private void User_TextChanged(object sender, EventArgs e)
        {
            if (_IsbtnSaveReadyToEnable())
            {
                btnSave.Enabled = true;
            }
            else
            {
                btnSave.Enabled = false;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //if (_IsPersonUser())
            //{
            //    MessageBox.Show("Person already is a User", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return;
            //}

            // check if password and confirm password match
            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show("Password and Confirm Password must match.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
                return;
            }

            // check is person is a user if mode is add new
            if (_Mode == enMode.AddNew && _IsPersonUser())
            {
                MessageBox.Show("Person already is a User", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnSave.Enabled = false;
                return;
            }

            _Save();

            txtPassword.Focus();

        }
    }
}
