using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using PharmacyInventoryWinForms.Utils;

namespace PharmacyInventoryWinForms.Forms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadAllMedicines();
        }

        private void LoadAllMedicines()
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand("GetAllMedicines", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    conn.Open();
                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(r);
                        dgvMedicines.DataSource = dt;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Load failed: " + ex.Message);
                }
            }
        }

        private int? SelectedMedicineId()
        {
            if (dgvMedicines.CurrentRow == null) return null;
            return Convert.ToInt32(dgvMedicines.CurrentRow.Cells["MedicineID"].Value);
        }

        private void btnViewAll_Click(object sender, EventArgs e)
        {
            LoadAllMedicines();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string category = txtCategory.Text.Trim();
            decimal price;
            int qty;

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(category) ||
                !decimal.TryParse(txtPrice.Text.Trim(), out price) || !int.TryParse(txtQuantity.Text.Trim(), out qty) ||
                price < 0 || qty < 0)
            {
                MessageBox.Show("Provide valid Name, Category, non-negative Price and Quantity.");
                return;
            }

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand("AddMedicine", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Category", category);
                cmd.Parameters.AddWithValue("@Price", price);
                cmd.Parameters.AddWithValue("@Quantity", qty);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Medicine added.");
                    LoadAllMedicines();
                    txtName.Clear(); txtCategory.Clear(); txtPrice.Clear(); txtQuantity.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Add failed: " + ex.Message);
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string term = txtSearch.Text.Trim();
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand("SearchMedicine", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SearchTerm", term);

                try
                {
                    conn.Open();
                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(r);
                        dgvMedicines.DataSource = dt;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Search failed: " + ex.Message);
                }
            }
        }

        private void btnUpdateStock_Click(object sender, EventArgs e)
        {
            int? id = SelectedMedicineId();
            if (id == null)
            {
                MessageBox.Show("Select a medicine from the list.");
                return;
            }

            int qty;
            if (!int.TryParse(txtQuantity.Text.Trim(), out qty) || qty < 0)
            {
                MessageBox.Show("Enter a non-negative Quantity to set as the new stock level.");
                return;
            }

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand("UpdateStock", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@MedicineID", SqlDbType.Int).Value = id.Value;
                cmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = qty;

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Stock updated.");
                    LoadAllMedicines();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Update failed: " + ex.Message);
                }
            }
        }

        private void btnRecordSale_Click(object sender, EventArgs e)
        {
            int? id = SelectedMedicineId();
            if (id == null)
            {
                MessageBox.Show("Select a medicine to record a sale.");
                return;
            }

            int qtySold;
            if (!int.TryParse(txtQuantity.Text.Trim(), out qtySold) || qtySold <= 0)
            {
                MessageBox.Show("Enter a positive Quantity (quantity sold).");
                return;
            }

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand("RecordSale", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@MedicineID", SqlDbType.Int).Value = id.Value;
                cmd.Parameters.Add("@QuantitySold", SqlDbType.Int).Value = qtySold;

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Sale recorded.");
                    LoadAllMedicines();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Sale failed: " + ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Sale failed: " + ex.Message);
                }
            }
        }
    }
}