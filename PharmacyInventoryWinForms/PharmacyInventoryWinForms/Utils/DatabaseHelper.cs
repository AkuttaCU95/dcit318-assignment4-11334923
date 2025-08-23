using System.Configuration;
using System.Data.SqlClient;

namespace PharmacyInventoryWinForms.Utils
{
    public static class DatabaseHelper
    {
        public static string ConnectionString =>
            ConfigurationManager.ConnectionStrings["PharmacyDBConn"].ConnectionString;

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}