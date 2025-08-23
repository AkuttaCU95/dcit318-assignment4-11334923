using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using MedicalAppointmentWinForms.Utils;

namespace MedicalAppointmentWinForms.Forms
{
    public partial class AppointmentForm : Form
    {
        public AppointmentForm()
        {
            InitializeComponent();
        }

        private void AppointmentForm_Load(object sender, EventArgs e)
        {
            LoadDoctors();
            LoadPatients();
        }

        private void LoadDoctors()
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand("SELECT DoctorID, FullName, Specialty, Availability FROM Doctors", conn))
            {
                try
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        cboDoctor.DisplayMember = "FullName";
                        cboDoctor.ValueMember = "DoctorID";
                        cboDoctor.DataSource = dt;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to load doctors: " + ex.Message);
                }
            }
        }

        private void LoadPatients()
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand("SELECT PatientID, FullName FROM Patients", conn))
            {
                try
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        cboPatient.DisplayMember = "FullName";
                        cboPatient.ValueMember = "PatientID";
                        cboPatient.DataSource = dt;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to load patients: " + ex.Message);
                }
            }
        }

        private bool DoctorIsAvailable(int doctorId)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand("SELECT Availability FROM Doctors WHERE DoctorID=@id", conn))
            {
                cmd.Parameters.AddWithValue("@id", doctorId);
                try
                {
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    if (result == null || result == DBNull.Value) return false;
                    return Convert.ToBoolean(result);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Availability check failed: " + ex.Message);
                    return false;
                }
            }
        }

        private bool SlotIsTaken(int doctorId, DateTime dt)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Appointments WHERE DoctorID=@d AND AppointmentDate=@t", conn))
            {
                cmd.Parameters.AddWithValue("@d", doctorId);
                cmd.Parameters.AddWithValue("@t", dt);
                try
                {
                    conn.Open();
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Conflict check failed: " + ex.Message);
                    return true;
                }
            }
        }

        private void btnBook_Click(object sender, EventArgs e)
        {
            if (cboDoctor.SelectedValue == null || cboPatient.SelectedValue == null)
            {
                MessageBox.Show("Please select both doctor and patient.");
                return;
            }

            int doctorId = Convert.ToInt32(cboDoctor.SelectedValue);
            int patientId = Convert.ToInt32(cboPatient.SelectedValue);
            DateTime apptDate = dtpDate.Value;
            string notes = txtNotes.Text.Trim();

            if (!DoctorIsAvailable(doctorId))
            {
                MessageBox.Show("Selected doctor is currently unavailable.");
                return;
            }

            if (SlotIsTaken(doctorId, apptDate))
            {
                MessageBox.Show("This time slot is already booked. Choose another time.");
                return;
            }

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                "INSERT INTO Appointments (DoctorID, PatientID, AppointmentDate, Notes) VALUES (@DoctorID, @PatientID, @Date, @Notes)", conn))
            {
                cmd.Parameters.Add("@DoctorID", SqlDbType.Int).Value = doctorId;
                cmd.Parameters.Add("@PatientID", SqlDbType.Int).Value = patientId;
                cmd.Parameters.Add("@Date", SqlDbType.DateTime).Value = apptDate;
                cmd.Parameters.Add("@Notes", SqlDbType.VarChar, 200).Value = notes;

                try
                {
                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        MessageBox.Show("Appointment booked successfully!");
                        txtNotes.Clear();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Booking failed: " + ex.Message);
                }
            }
        }
    }
}