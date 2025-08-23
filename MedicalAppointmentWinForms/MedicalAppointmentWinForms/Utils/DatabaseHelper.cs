using System.Configuration;
using System.Data.SqlClient;

namespace MedicalAppointmentWinForms.Utils
{
    public static class DatabaseHelper
    {
        public static string ConnectionString =>
            ConfigurationManager.ConnectionStrings["MedicalDBConn"].ConnectionString;

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}