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
    public partial class Form5 : Form
    {
        string connectionString = @"Data Source=DESKTOP-FT33RAV;Initial Catalog=Northwind;Integrated Security=True;TrustServerCertificate=true;";
        private SqlConnection connection;
        private SqlDataAdapter adapter;
        private DataTable DataTable;
        public Form5()
        {
            InitializeComponent();
            connection = new SqlConnection(connectionString);
            adapter = new SqlDataAdapter("SELECT * FROM Customers", connection);
            DataTable = new DataTable();
            adapter.Fill(DataTable);
            comboBox1.DataSource = DataTable;
            comboBox1.DisplayMember = "CustomerID";
            dataGridView1.DataSource = DataTable;
        }

        private void label11_Click(object sender, EventArgs e)
        {

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
                string CustomerID = textBox1.Text;
                string CompanyName = textBox2.Text;
                string ContactName = textBox3.Text;
                string ContactTitle = textBox4.Text;
                string Address = textBox5.Text;
                string City = textBox6.Text;
                string Region = textBox7.Text;
                string PostalCode = textBox8.Text;
                string Country = textBox9.Text;
                string Phone = textBox10.Text;
                string Fax = textBox11.Text;

                connection.Open();
                string insertQuery = "INSERT INTO Customers(CustomerID, CompanyName, ContactName, ContactTitle, Address, City, Region, PostalCode, Country, Phone, Fax) VALUES (@CustomerID, @CompanyName, @ContactName, @ContactTitle, @Address, @City, @Region, @PostalCode, @Country, @Phone, @Fax)";
                SqlCommand insertCommand = new SqlCommand(insertQuery, connection);

                insertCommand.Parameters.AddWithValue("@CustomerID", CustomerID);
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
                string CustomerID = Convert.ToString(selectedRowView.Row["CustomerID"]);

                string query = "SELECT CustomerID, CompanyName, ContactName, ContactTitle, Address, City, Region, PostalCode, Country, Phone, Fax FROM Customers WHERE CustomerID = @CustomerID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@CustomerID", CustomerID);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    textBox1.Text = reader["CustomerID"].ToString();
                    textBox2.Text = reader["CompanyName"].ToString();
                    textBox3.Text = reader["ContactName"].ToString();
                    textBox4.Text = reader["ContactTitle"].ToString();
                    textBox5.Text = reader["Address"].ToString();
                    textBox6.Text = reader["City"].ToString();
                    textBox7.Text = reader["Region"].ToString();
                    textBox8.Text = reader["PostalCode"].ToString();
                    textBox9.Text = reader["Country"].ToString();
                    textBox10.Text = reader["Phone"].ToString();
                    textBox11.Text = reader["Fax"].ToString();
                }
                reader.Close();
                connection.Close();
            }
        }

        private void Form5_Load(object sender, EventArgs e)
        {
           
            comboBox1.SelectedItem = null;
            LimpiarCampos();

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
                if (comboBox1.SelectedItem is DataRowView selectedRowView)
                {
                    string selectedCustomerID = selectedRowView["CustomerID"].ToString();

                    connection.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM Customers WHERE CustomerID = @CustomerID", connection);
                    cmd.Parameters.AddWithValue("@CustomerID", selectedCustomerID);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Registro eliminado correctamente");
                        LimpiarCampos();
                    }
                    else
                    {
                        MessageBox.Show("No se encontró ningún registro con el CustomerID especificado.");
                    }
                }
                else
                {
                    MessageBox.Show("Selecciona un CustomerID antes de eliminar.");
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

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.SelectedItem is DataRowView selectedRowView)
                {
                    string CustomerID = Convert.ToString(selectedRowView.Row["CustomerID"]);

                    string CompanyName = textBox2.Text;
                    string ContactName = textBox3.Text;
                    string ContactTitle = textBox4.Text;
                    string Address = textBox5.Text;
                    string City = textBox6.Text;
                    string Region = textBox7.Text;
                    string PostalCode = textBox8.Text;
                    string Country = textBox9.Text;
                    string Phone = textBox10.Text;
                    string Fax = textBox11.Text;

                    connection.Open();
                    string updateQuery = "UPDATE Customers SET CompanyName = @CompanyName, ContactName = @ContactName, ContactTitle = @ContactTitle, Address = @Address, City = @City, Region = @Region, PostalCode = @PostalCode, Country = @Country, Phone = @Phone, Fax = @Fax WHERE CustomerID = @CustomerID";
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
                    updateCommand.Parameters.AddWithValue("@CustomerID", CustomerID);

                    int rowsAffected = updateCommand.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Dato Actualizado Correctamente");

                    }
                    else
                    {
                        MessageBox.Show("Error al actualizar el dato");
                    }





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

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {


        }
    }
}
