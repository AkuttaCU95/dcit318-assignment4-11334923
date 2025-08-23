using System;
using System.Windows.Forms;

namespace MedicalAppointmentWinForms.Forms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnDoctors_Click(object sender, EventArgs e)
        {
            using (var f = new DoctorListForm())
            {
                f.ShowDialog(this);
            }
        }

        private void btnBook_Click(object sender, EventArgs e)
        {
            using (var f = new AppointmentForm())
            {
                f.ShowDialog(this);
            }
        }

        private void btnManage_Click(object sender, EventArgs e)
        {
            using (var f = new ManageAppointmentsForm())
            {
                f.ShowDialog(this);
            }
        }
    }
}