using DVLD.Global_Classes;
using DVLD.People.Controls;
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
    public partial class frmChangePassword : Form
    {
        public frmChangePassword(int UserID)
        {
            InitializeComponent();
            ctrlUserCard1.LoadUserInfo(UserID);
        }

        private bool _IsPasswordReadyToSave()
        {
            if (ctrlUserCard1.User == null)
            {
                return false;
            }

            if (txtCurrentPassword.Text == string.Empty)
            {
                return false;
            }

            if (txtNewPassword.Text == string.Empty)
            {
                return false;
            }

            if (txtConfirmPassword.Text == string.Empty)
            {
                return false;
            }

            return true;
        }

        private void frmChangePassword_Load(object sender, EventArgs e)
        {

        }

        private void Password_TextChanged(object sender, EventArgs e)
        {
            if (_IsPasswordReadyToSave())
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
            if (ctrlUserCard1.User.Password != txtCurrentPassword.Text)
            {
                MessageBox.Show("Current password is incorrect.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!this.ValidateChildren())
            {
                MessageBox.Show("Please fill in all fields.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if(txtNewPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show("Password is not matching.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ctrlUserCard1.User.Password = txtNewPassword.Text;

            if (ctrlUserCard1.User.Save())
            {
                MessageBox.Show("Password changed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Failed to change password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtCurrentPassword_Validated(object sender, EventArgs e)
        {
            
        }

        private void txtCurrentPassword_Validating(object sender, CancelEventArgs e)
        {
            TextBox text = sender as TextBox;

            if (ctrlUserCard1.User.Password != txtCurrentPassword.Text)
            {
                errorProvider1.SetError(text, $"{text.Tag} is incorrect.");
                return;
            }

            if (text.Text == string.Empty)
            {
                errorProvider1.SetError(text, $"{text.Tag} is required.");
            }
            else
            {
                errorProvider1.SetError(text, string.Empty);
            }
        }

        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            TextBox text = sender as TextBox;

            if (text.Text == string.Empty)
            {
                errorProvider1.SetError(text, $"{text.Tag} is required.");
            }
            else
            {
                errorProvider1.SetError(text, string.Empty);
            }
        }
    }
}
