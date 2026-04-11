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

namespace DVLD.Users.Controls
{
    public partial class ctrlUserCard : UserControl
    {
        public ctrlUserCard()
        {
            InitializeComponent();
        }

        public clsUser User;

        public void LoadUserInfo(int UserID)
        {
            if ((User = clsUser.Find(UserID)) != null)
            {
                ctrlPersonCard1.LoadData(User.PersonID);
                lblUserID.Text = User.UserID.ToString();
                lblUserName.Text = User.UserName;
                lblIsActive.Text = (User.IsActive) ? "Active" : "Inactive";
            }
        }

        private void ctrlUserCard_Load(object sender, EventArgs e)
        {

        }

        private void gbUserInfo_Enter(object sender, EventArgs e)
        {

        }
    }
}
