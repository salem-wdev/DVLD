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
    public partial class frmShowPersonInfo : Form
    {

        int _PersonID = -1;


        // Never Use New With Controls!!!!!

        public frmShowPersonInfo()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            // Never Use New With Controls!!!!!

        }

        public frmShowPersonInfo(int PersonID)
        {
            InitializeComponent();
            _PersonID = PersonID;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            // Never Use New With Controls!!!!!

        }

        public frmShowPersonInfo(clsPerson Person)
        {
            InitializeComponent();
            _PersonID = Person.PersonID;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            // Never Use New With Controls!!!!!

        }



        private void frmShowPersonInfo_Load(object sender, EventArgs e)
        {
            // Only try to reload if a person is set
           if (!ctrlShowPersonInfo1.LoadData(_PersonID))
           {
                MessageBox.Show("Person not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void ctrlShowPersonInfo1_Load(object sender, EventArgs e)
        {

        }
    }
}
