using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Northwind
{
    public partial class Form8 : Form
    {
        string connectionString = @"Data Source=DESKTOP-FT33RAV;Initial Catalog=Northwind;Integrated Security=True;TrustServerCertificate=true;";
        private SqlConnection connection;
        private SqlDataAdapter adapter;
        private DataTable DataTable;
        public Form8()
        {
            InitializeComponent();
            connection = new SqlConnection(connectionString);
            adapter = new SqlDataAdapter("SELECT * FROM Territories", connection);
            DataTable = new DataTable();
            adapter.Fill(DataTable);
            comboBox1.DataSource = DataTable;
            comboBox1.DisplayMember = "TerritoryID";
            comboBox2.DisplayMember = "RegionID";
            comboBox2.DataSource = DataTable;
           
        }
        private void LimpiarCampos()
        {
            textBox1.Text = "";
            comboBox2.SelectedIndex = -1;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string TerritoryDescription = textBox1.Text;

                // Obtén el RegionID seleccionado del ComboBox (asumiendo que tienes un ComboBox llamado comboBoxRegion)
                int RegionID = Convert.ToInt32(comboBox2.SelectedValue);

                connection.Open();
                string insertQuery = "INSERT INTO Territories (TerritoryDescription, RegionID) VALUES (@TerritoryDescription, @RegionID)";
                SqlCommand insertCommand = new SqlCommand(insertQuery, connection);

                insertCommand.Parameters.AddWithValue("@TerritoryDescription", TerritoryDescription);
                insertCommand.Parameters.AddWithValue("@RegionID", RegionID);

                int rowsAffected = insertCommand.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Dato Insertado Correctamente");
                    LimpiarCampos();
                }
                else
                {
                    MessageBox.Show("Error al insertar el dato");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem is DataRowView selectedRowView)
            {
                int TerritoryID = Convert.ToInt32(selectedRowView.Row["TerritoryID"]);
                

                string query = "SELECT TerritoryDescription, RegionID FROM Territories WHERE TerritoryID = @TerritoryID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@TerritoryID", TerritoryID);


                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    textBox1.Text = reader["TerritoryDescription"].ToString();
                    comboBox2.Text = reader["RegionID"].ToString();
                    reader.Close();
                }
                connection.Close();
            }
        }

        private void Form8_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedItem = null;
            comboBox2.SelectedItem = null;
            LimpiarCampos();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }
    }
}

