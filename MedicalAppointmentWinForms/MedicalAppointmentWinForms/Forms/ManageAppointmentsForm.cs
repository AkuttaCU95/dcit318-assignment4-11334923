using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using MedicalAppointmentWinForms.Utils;

namespace MedicalAppointmentWinForms.Forms
{
    public partial class ManageAppointmentsForm : Form
    {
        private DataSet ds = new DataSet();

        public ManageAppointmentsForm()
        {
            InitializeComponent();
        }

        private void ManageAppointmentsForm_Load(object sender, EventArgs e)
        {
            LoadAppointments();
        }

        private void LoadAppointments()
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = @"SELECT A.AppointmentID, D.FullName AS Doctor, P.FullName AS Patient, 
                                        A.AppointmentDate, A.Notes, A.DoctorID, A.PatientID
                                 FROM Appointments A
                                 INNER JOIN Doctors D ON A.DoctorID = D.DoctorID
                                 INNER JOIN Patients P ON A.PatientID = P.PatientID
                                 ORDER BY A.AppointmentDate DESC";

                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                ds = new DataSet();
                try
                {
                    adapter.Fill(ds, "Appointments");
                    dgvAppointments.DataSource = ds.Tables["Appointments"];
                    dgvAppointments.Columns["DoctorID"].Visible = false;
                    dgvAppointments.Columns["PatientID"].Visible = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to load appointments: " + ex.Message);
                }
            }
        }

        private int? SelectedAppointmentId()
        {
            if (dgvAppointments.CurrentRow == null) return null;
            var row = dgvAppointments.CurrentRow;
            return Convert.ToInt32(row.Cells["AppointmentID"].Value);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadAppointments();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int? id = SelectedAppointmentId();
            if (id == null)
            {
                MessageBox.Show("Select an appointment first.");
                return;
            }

            DateTime newDate = dtpNewDate.Value;

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand("UPDATE Appointments SET AppointmentDate=@d WHERE AppointmentID=@id", conn))
            {
                cmd.Parameters.Add("@d", SqlDbType.DateTime).Value = newDate;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id.Value;

                try
                {
                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        MessageBox.Show("Appointment updated.");
                        LoadAppointments();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Update failed: " + ex.Message);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int? id = SelectedAppointmentId();
            if (id == null)
            {
                MessageBox.Show("Select an appointment to delete.");
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete this appointment?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand("DELETE FROM Appointments WHERE AppointmentID=@id", conn))
            {
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id.Value;
                try
                {
                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        MessageBox.Show("Appointment deleted.");
                        LoadAppointments();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Delete failed: " + ex.Message);
                }
            }
        }
    }
}