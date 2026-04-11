using DVLD.People.Controls;
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

            if (txtNewPassword.Text == string.Empty)
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
            if (!_IsPasswordReadyToSave())
            {
                MessageBox.Show("Please fill in all fields.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if(txtCurrentPassword.Text != ctrlUserCard1.User.Password)
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
            TextBox text = sender as TextBox;

            if(text.Text == string.Empty)
            {
                errorProvider1.SetError(text, $"{text.Tag} is required.");
            }
        }
    }
}
