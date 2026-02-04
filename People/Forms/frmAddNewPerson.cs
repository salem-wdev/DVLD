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

namespace DVLD.People.Forms
{
    public partial class frmAddNewPerson : Form
    {
        public frmAddNewPerson()
        {
            InitializeComponent();
            ctrlAddAndEditPersonInfo AddAndEditPersonInfo = new ctrlAddAndEditPersonInfo();
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

        }

        public frmAddNewPerson(clsPerson clsPerson)
        {
            InitializeComponent();
            ctrlAddAndEditPersonInfo AddAndEditPersonInfo = new ctrlAddAndEditPersonInfo(clsPerson);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

        }

        private void frmAddNewPerson_Load(object sender, EventArgs e)
        {

        }
    }
}
