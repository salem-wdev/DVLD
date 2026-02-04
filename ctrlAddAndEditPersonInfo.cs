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

namespace DVLD
{
    public partial class ctrlAddAndEditPersonInfo : UserControl
    {
        enum enMode
        {
            AddNew,
            EditExisting
        }

        private enMode _Mode;

        clsPerson _Person;

        private void FillPerson()
        {
            _Person.FirstName = txtSecondName.Text;
        }

        public ctrlAddAndEditPersonInfo()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
            _Person = new clsPerson();
        }

        public ctrlAddAndEditPersonInfo(clsPerson Person)
        {
            InitializeComponent();
            _Mode = enMode.EditExisting;
            _Person = Person;
        }

        private void ctrlAddAndEditPersonInfo_Load(object sender, EventArgs e)
        {

        }
    }
}
