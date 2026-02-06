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
    public partial class frmAddUpdatePerson : Form
    {
        public frmAddUpdatePerson()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            lblPersonID.Text = "N/A";

        }

        public frmAddUpdatePerson(clsPerson Person)
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            lblPersonID.Text = Person.PersonID.ToString();


        }

        public frmAddUpdatePerson(int PersonID)
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            lblPersonID.Text = PersonID.ToString();


        }

        private void frmAddNewPerson_Load(object sender, EventArgs e)
        {

        }

        private void FillPersonIDLabel(object sender, int PersonID)
        {
            lblPersonID.Text = PersonID.ToString();
        }

        private void ctrlAddAndEditPersonInfo1_Load(object sender, EventArgs e)
        {
            ctrlAddAndEditPersonInfo1.SendDataBack += FillPersonIDLabel;
        }
    }
}
