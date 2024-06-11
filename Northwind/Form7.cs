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
    public partial class Form7 : Form
    {
        string connectionString = @"Data Source=DESKTOP-FT33RAV;Initial Catalog=Northwind;Integrated Security=True;TrustServerCertificate=true;";
        private SqlConnection connection;
        private SqlDataAdapter adapter;
        private DataTable DataTable;
        public Form7()
        {
            InitializeComponent();
            connection = new SqlConnection(connectionString);
            adapter = new SqlDataAdapter("SELECT * FROM Suppliers", connection);
            DataTable = new DataTable();
            adapter.Fill(DataTable);
            comboBox1.DataSource = DataTable;
            comboBox1.DisplayMember = "SupplierID";
            dataGridView1.DataSource = DataTable;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
            this.Hide();
        }

        private void Form7_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedItem = null;
            LimpiarCampos();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem is DataRowView selectedRowView)
            {
                int SupplierID = Convert.ToInt32(selectedRowView.Row["SupplierID"]);

                string query = "SELECT CompanyName, ContactName, ContactTitle, Address, City, Region, PostalCode, Country, Phone, Fax, HomePage FROM Suppliers WHERE SupplierID = @SupplierID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@SupplierID", SupplierID);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    textBox1.Text = reader["CompanyName"].ToString();
                    textBox2.Text = reader["ContactName"].ToString();
                    textBox3.Text = reader["ContactTitle"].ToString();
                    textBox4.Text = reader["Address"].ToString();
                    textBox5.Text = reader["City"].ToString();
                    textBox6.Text = reader["Region"].ToString();
                    textBox7.Text = reader["PostalCode"].ToString();
                    textBox8.Text = reader["Country"].ToString();
                    textBox9.Text = reader["Phone"].ToString();
                    textBox10.Text = reader["Fax"].ToString();
                    textBox11.Text = reader["HomePage"].ToString();

                }
                reader.Close();
                connection.Close();

            }
        }
        private void LimpiarCampos()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            textBox9.Text = "";
            textBox10.Text = "";
            textBox11.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string CompanyName = textBox1.Text;
                string ContactName = textBox2.Text;
                string ContactTitle = textBox3.Text;
                string Address = textBox4.Text;
                string City = textBox5.Text;
                string Region = textBox6.Text;
                string PostalCode = textBox7.Text;
                string Country = textBox8.Text;
                string Phone = textBox9.Text;
                string Fax = textBox10.Text;
                string HomePage = textBox11.Text;

                connection.Open();
                string insertQuery = "INSERT INTO Suppliers(CompanyName, ContactName, ContactTitle, Address, City, Region, PostalCode, Country, Phone, Fax, HomePage) VALUES (@CompanyName, @ContactName, @ContactTitle, @Address, @City, @Region, @PostalCode, @Country, @Phone, @Fax, @HomePage)";
                SqlCommand insertCommand = new SqlCommand(insertQuery, connection);

                insertCommand.Parameters.AddWithValue("@CompanyName", CompanyName);
                insertCommand.Parameters.AddWithValue("@ContactName", ContactName);
                insertCommand.Parameters.AddWithValue("@ContactTitle", ContactTitle);
                insertCommand.Parameters.AddWithValue("@Address", Address);
                insertCommand.Parameters.AddWithValue("@City", City);
                insertCommand.Parameters.AddWithValue("@Region", Region);
                insertCommand.Parameters.AddWithValue("@PostalCode", PostalCode);
                insertCommand.Parameters.AddWithValue("@Country", Country);
                insertCommand.Parameters.AddWithValue("@Phone", Phone);
                insertCommand.Parameters.AddWithValue("@Fax", Fax);
                insertCommand.Parameters.AddWithValue("@HomePage", HomePage);

                //Ejecutar la consulta

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

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                int SupplierID = int.Parse(comboBox1.Text);

                string CompanyName = textBox1.Text;
                string ContactName = textBox2.Text;
                string ContactTitle = textBox3.Text;
                string Address = textBox4.Text;
                string City = textBox5.Text;
                string Region = textBox6.Text;
                string PostalCode = textBox7.Text;
                string Country = textBox8.Text;
                string Phone = textBox9.Text;
                string Fax = textBox10.Text;
                string HomePage = textBox11.Text;
                connection.Open();
                string updateQuery = "UPDATE Suppliers SET CompanyName = @CompanyName, ContactName = @ContactName, ContactTitle = @ContactTitle, Address = @Address, City = @City, Region = @Region, PostalCode = @PostalCode, Country = @Country, Phone = @Phone, Fax = @Fax, HomePage = @HomePage WHERE SupplierID = @SupplierID";
                SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
                updateCommand.Parameters.AddWithValue("@CompanyName", CompanyName);
                updateCommand.Parameters.AddWithValue("@ContactName", ContactName);
                updateCommand.Parameters.AddWithValue("@ContactTitle", ContactTitle);
                updateCommand.Parameters.AddWithValue("@Address", Address);
                updateCommand.Parameters.AddWithValue("@City", City);
                updateCommand.Parameters.AddWithValue("@Region", Region);
                updateCommand.Parameters.AddWithValue("@PostalCode", PostalCode);
                updateCommand.Parameters.AddWithValue("@Country", Country);
                updateCommand.Parameters.AddWithValue("@Phone", Phone);
                updateCommand.Parameters.AddWithValue("@Fax", Fax);
                updateCommand.Parameters.AddWithValue("@HomePage", HomePage);
                updateCommand.Parameters.AddWithValue("@SupplierID", SupplierID);
                int rowsAffected = updateCommand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Categoría actualizada correctamente");
                    LimpiarCampos();
                }
                else
                {
                    MessageBox.Show("Error al actualizar la categoría");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                // Cerrar la conexión
                connection.Close();
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Suppliers WHERE SupplierID = @SupplierID", connection);
                cmd.Parameters.AddWithValue("SupplierID", int.Parse(comboBox1.Text));
                cmd.ExecuteNonQuery();
                connection.Close();
                MessageBox.Show("Registro eliminado correctamente");
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se elimino el registro", ex.Message);
            }
        }
    }
}


