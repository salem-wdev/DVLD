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
using static DVLD.ctrlAddAndEditPersonInfo;

namespace DVLD.People.Forms
{
    public partial class frmAddUpdatePerson : Form
    {
        clsPerson _Person;

        public delegate void PersonDataReceivedEventHandlerInForm(object sender, clsPerson Person);

        public event PersonDataReceivedEventHandlerInForm PersonDataReceivedToForm;


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
        }

        private void FillPersonIDLabel()
        {
            lblPersonID.Text = _Person.PersonID.ToString();
        }

        private void ReceivePersonDataFromControl(object sender, clsPerson Person)
        {
            _Person = Person;
            FillPersonIDLabel();
            PersonDataReceivedToForm?.Invoke(this, _Person);
        }

        private void ctrlAddAndEditPersonInfo1_Load(object sender, EventArgs e)
        {
            ctrlAddAndEditPersonInfo1.PersonDataReceived += ReceivePersonDataFromControl;
            if (_Person != null)
            {
                ctrlAddAndEditPersonInfo1.FillPersonWithData(_Person);
            }

            //{
            //    MessageBox.Show("Unable to load person data.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }
    }
}
