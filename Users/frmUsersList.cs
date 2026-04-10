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
    public partial class frmUsersList : Form
    {
        DataTable dtUsers;
        private void _RefreshRecordsNumber()
        {
            lblRecord.Text = dgvUsers.Rows.Count.ToString();
        }

        private void _RefreshUsersList()
        {
            dtUsers = clsUser.GetAllUsers();
            dgvUsers.DataSource = dtUsers;
            _RefreshRecordsNumber();
        }

        public frmUsersList()
        {
            InitializeComponent();
        }

        private void frmUsersList_Load(object sender, EventArgs e)
        {
            _RefreshUsersList();

            cbFilter.SelectedIndex = 0;
            cbIsActive.SelectedIndex = 0;

            dgvUsers.Columns[0].HeaderText = "User ID";
            dgvUsers.Columns[0].Width = 110;

            dgvUsers.Columns[1].HeaderText = "Person ID";
            dgvUsers.Columns[1].Width = 120;

            dgvUsers.Columns[2].HeaderText = "Full Name";
            dgvUsers.Columns[2].Width = 350;

            dgvUsers.Columns[3].HeaderText = "UserName";
            dgvUsers.Columns[3].Width = 120;

            dgvUsers.Columns[4].HeaderText = "Is Active";
            dgvUsers.Columns[4].Width = 120;
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbIsActive.Visible = false;

            if (cbFilter.SelectedItem.ToString() == "None")
            {
                cbIsActive.Visible = false;
                cbIsActive.SelectedIndex = 0;
                txtFilter.Text = string.Empty;
                txtFilter.Visible = false;
                return;
            }

            if (cbFilter.SelectedItem.ToString() == "Is Active")
            {
                cbIsActive.Visible = true;
                txtFilter.Visible = false;
                txtFilter.Text = string.Empty;
                return;
            }

            txtFilter.Visible = true;
            txtFilter.Text = string.Empty;


        }

        private void cbIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbIsActive.SelectedItem)
            {
                case "All":
                    dtUsers.DefaultView.RowFilter = string.Empty;
                    break;
                case "Yes":
                    dtUsers.DefaultView.RowFilter = "IsActive = true";
                    break;
                case "No":
                    dtUsers.DefaultView.RowFilter = "IsActive = false";
                    break;
            }

            _RefreshRecordsNumber();
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            //this will allow only digits if person id is selected
            if (cbFilter.Text == "Person ID" || cbFilter.Text == "User ID")
            {
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
                return;
            }
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            string SelectedString;

            switch (cbFilter.SelectedItem.ToString())
            {
                case "User ID":
                    SelectedString = "UserID";
                    break;
                case "Person ID":
                    SelectedString = "PersonID";
                    break;
                case "Full Name":
                    SelectedString = "FullName";
                    break;
                case "UserName":
                    SelectedString = "UserName";
                    break;
                default:
                    SelectedString = "UserID";
                    break;

            }

            if (txtFilter.Text.ToString().Trim() == string.Empty)
            {
                dtUsers.DefaultView.RowFilter = string.Empty;
                _RefreshRecordsNumber();
                return;
            }

            if (cbFilter.SelectedItem.ToString() == "User ID" || cbFilter.SelectedItem.ToString() == "Person ID")
            {
                dtUsers.DefaultView.RowFilter = $"{SelectedString} = {txtFilter.Text.ToString()}";
                _RefreshRecordsNumber();
                return;
            }

            if (cbFilter.SelectedItem.ToString() == "UserName")
            {
                dtUsers.DefaultView.RowFilter = $"{SelectedString} LIKE '{txtFilter.Text.ToString().Trim()}%'";
                _RefreshRecordsNumber();
                return;
            }

            dtUsers.DefaultView.RowFilter = $"{SelectedString} LIKE '%{txtFilter.Text.ToString().Trim()}%'";

            _RefreshRecordsNumber();

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddNewUser_Click(object sender, EventArgs e)
        {
            frmAddUpdateUser frm = new frmAddUpdateUser();
            frm.ShowDialog();
            _RefreshUsersList();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This feature not emplented yet", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void AddNewUsertoolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdateUser frm = new frmAddUpdateUser();
            frm.ShowDialog();
            _RefreshUsersList();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdateUser frm = new frmAddUpdateUser((int)dgvUsers.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            _RefreshUsersList();
        }
    }
}
