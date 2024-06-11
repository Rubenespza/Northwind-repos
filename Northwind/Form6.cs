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
    public partial class Form6 : Form
    {
        string connectionString = @"Data Source=DESKTOP-FT33RAV;Initial Catalog=Northwind;Integrated Security=True;TrustServerCertificate=true;";
        private SqlConnection connection;
        private SqlDataAdapter adapter;
        private DataTable DataTable;
        public Form6()
        {
            InitializeComponent();
            connection = new SqlConnection(connectionString);
            adapter = new SqlDataAdapter("SELECT ShipperID FROM Shippers", connection);
            DataTable = new DataTable();
            adapter.Fill(DataTable);
            comboBox1.DataSource = DataTable;
            comboBox1.DisplayMember = "ShipperID";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form2 Form2 = new Form2();
            Form2.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Shippers WHERE ShipperID = @ShipperID", connection);
                cmd.Parameters.AddWithValue("ShipperID", int.Parse(comboBox1.Text));
                cmd.ExecuteNonQuery();
                connection.Close();
                MessageBox.Show("Registro eliminado correctamente");
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se elmino el registro" + ex.Message);
            }
           

        }
        private void LimpiarCampos()
        {
            textBox1.Text = "";
            textBox2.Text = "";
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string CompanyName = textBox1.Text;
                string Phone = textBox2.Text;

                connection.Open();

                string insertQuery = "INSERT INTO Shippers(CompanyName, Phone) VALUES (@CompanyName, @Phone)";
                SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                insertCommand.Parameters.AddWithValue("@CompanyName", CompanyName);
                insertCommand.Parameters.AddWithValue("@Phone", Phone);

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
                int ShipperID = Convert.ToInt32(selectedRowView.Row["ShipperID"]);

                string query = "SELECT CompanyName, Phone FROM Shippers WHERE ShipperID = @ShipperID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ShipperID", ShipperID);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    textBox1.Text = reader["CompanyName"].ToString();
                    textBox2.Text = reader["Phone"].ToString();
                    reader.Close();
                    connection.Close();
                }
            }
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedItem = null;
            LimpiarCampos();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                int ShipperID = int.Parse(comboBox1.Text);
                string CompanyName = textBox1.Text;
                string Phone = textBox2.Text;

                connection.Open();
                string updateQuery = "UPDATE Shippers SET CompanyName = @CompanyName, Phone = @Phone WHERE ShipperID = @ShipperID";
                SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
                updateCommand.Parameters.AddWithValue("@CompanyName", CompanyName);
                updateCommand.Parameters.AddWithValue("@Phone", Phone);
                updateCommand.Parameters.AddWithValue("ShipperID", ShipperID);

                int rowsAffected = updateCommand.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Dato actualizado correctamente");
                }
                else
                {
                    MessageBox.Show("Error al actualizar el dato");
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
    }
}
