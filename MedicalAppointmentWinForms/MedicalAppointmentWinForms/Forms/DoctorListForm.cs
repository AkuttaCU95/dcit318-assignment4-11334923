using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using MedicalAppointmentWinForms.Utils;

namespace MedicalAppointmentWinForms.Forms
{
    public partial class DoctorListForm : Form
    {
        public DoctorListForm()
        {
            InitializeComponent();
        }

        private void DoctorListForm_Load(object sender, EventArgs e)
        {
            LoadDoctors();
        }

        private void LoadDoctors(string search = "")
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = "SELECT DoctorID, FullName, Specialty, Availability FROM Doctors";
                if (!string.IsNullOrWhiteSpace(search))
                {
                    query += " WHERE FullName LIKE @s OR Specialty LIKE @s";
                }

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (!string.IsNullOrWhiteSpace(search))
                        cmd.Parameters.AddWithValue("@s", "%" + search + "%");

                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            DataTable dt = new DataTable();
                            dt.Load(reader);
                            dgvDoctors.DataSource = dt;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Failed to load doctors: " + ex.Message);
                    }
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadDoctors(txtSearch.Text.Trim());
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            LoadDoctors();
        }
    }
}