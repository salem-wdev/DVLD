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
        clsPerson _Person;

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
            _Person = Person;


        }

        public frmAddUpdatePerson(int PersonID)
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            lblPersonID.Text = PersonID.ToString();
            _Person = clsPerson.Find(PersonID);


        }

        private void frmAddNewPerson_Load(object sender, EventArgs e)
        {
<<<<<<< HEAD
=======

>>>>>>> 709473bd007495376bf7c6289d5c4fceca48a82d
        }

        private void FillPersonIDLabel(object sender, int PersonID)
        {
            lblPersonID.Text = PersonID.ToString();
        }

        private void ctrlAddAndEditPersonInfo1_Load(object sender, EventArgs e)
        {
            ctrlAddAndEditPersonInfo1.SendDataBack += FillPersonIDLabel;
<<<<<<< HEAD
            if (_Person != null)
            {
                ctrlAddAndEditPersonInfo1.FillPersonWithData(_Person);
            }
            //{
            //    MessageBox.Show("Unable to load person data.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
=======
>>>>>>> 709473bd007495376bf7c6289d5c4fceca48a82d
        }
    }
}
