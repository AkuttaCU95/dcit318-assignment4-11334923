namespace MedicalAppointmentWinForms.Forms
{
    partial class MainForm
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
            this.btnDoctors = new System.Windows.Forms.Button();
            this.btnBook = new System.Windows.Forms.Button();
            this.btnManage = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnDoctors
            // 
            this.btnDoctors.Location = new System.Drawing.Point(38, 86);
            this.btnDoctors.Name = "btnDoctors";
            this.btnDoctors.Size = new System.Drawing.Size(240, 45);
            this.btnDoctors.TabIndex = 0;
            this.btnDoctors.Text = "View Available Doctors";
            this.btnDoctors.UseVisualStyleBackColor = true;
            this.btnDoctors.Click += new System.EventHandler(this.btnDoctors_Click);
            // 
            // btnBook
            // 
            this.btnBook.Location = new System.Drawing.Point(38, 147);
            this.btnBook.Name = "btnBook";
            this.btnBook.Size = new System.Drawing.Size(240, 45);
            this.btnBook.TabIndex = 1;
            this.btnBook.Text = "Book Appointment";
            this.btnBook.UseVisualStyleBackColor = true;
            this.btnBook.Click += new System.EventHandler(this.btnBook_Click);
            // 
            // btnManage
            // 
            this.btnManage.Location = new System.Drawing.Point(38, 208);
            this.btnManage.Name = "btnManage";
            this.btnManage.Size = new System.Drawing.Size(240, 45);
            this.btnManage.TabIndex = 2;
            this.btnManage.Text = "Manage Appointments";
            this.btnManage.UseVisualStyleBackColor = true;
            this.btnManage.Click += new System.EventHandler(this.btnManage_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(34, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(375, 25);
            this.label1.TabIndex = 3;
            this.label1.Text = "Medical Appointment Booking System";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(406, 292);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnManage);
            this.Controls.Add(this.btnBook);
            this.Controls.Add(this.btnDoctors);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Main";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button btnDoctors;
        private System.Windows.Forms.Button btnBook;
        private System.Windows.Forms.Button btnManage;
        private System.Windows.Forms.Label label1;
    }
}