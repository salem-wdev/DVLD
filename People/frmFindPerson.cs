using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.People
{
    public partial class frmFindPerson : Form
    {

        public delegate void DataBackEventHandler(object sender, int PersonID);

        public event DataBackEventHandler DataBack;

        public frmFindPerson()
        {
            InitializeComponent();
        }

         
       

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void frmFindPerson_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ctrlPersonCardWithFilter1.PersonID > 0)
            {
                // Trigger the event to send data back to the caller form.
                DataBack?.Invoke(this, ctrlPersonCardWithFilter1.PersonID);
            }
            else
            {
                // Trigger the event to send data back to the caller form with -1 to indicate no selection.
                DataBack?.Invoke(this, -1);
            }
        }
    }
}
