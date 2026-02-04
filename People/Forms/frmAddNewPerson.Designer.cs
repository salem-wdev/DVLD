namespace DVLD.People.Forms
{
    partial class frmAddNewPerson
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ctrlAddAndEditPersonInfo1 = new DVLD.ctrlAddAndEditPersonInfo();
            this.SuspendLayout();
            // 
            // ctrlAddAndEditPersonInfo1
            // 
            this.ctrlAddAndEditPersonInfo1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ctrlAddAndEditPersonInfo1.Location = new System.Drawing.Point(13, 69);
            this.ctrlAddAndEditPersonInfo1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ctrlAddAndEditPersonInfo1.Name = "ctrlAddAndEditPersonInfo1";
            this.ctrlAddAndEditPersonInfo1.Size = new System.Drawing.Size(774, 387);
            this.ctrlAddAndEditPersonInfo1.TabIndex = 0;
            // 
            // frmAddNewPerson
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 470);
            this.Controls.Add(this.ctrlAddAndEditPersonInfo1);
            this.Name = "frmAddNewPerson";
            this.Text = "frmAddNewPerson";
            this.Load += new System.EventHandler(this.frmAddNewPerson_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private ctrlAddAndEditPersonInfo ctrlAddAndEditPersonInfo1;
    }
}