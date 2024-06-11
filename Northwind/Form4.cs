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
    public partial class Form4 : Form
    {
        string connectionString = @"Data Source=DESKTOP-FT33RAV;Initial Catalog=Northwind;Integrated Security=True;TrustServerCertificate=true;";
        private SqlConnection connection;
        private SqlDataAdapter adapter;
        private DataTable DataTable;

        public Form4()
        {
            InitializeComponent();
            connection = new SqlConnection(connectionString);
            adapter = new SqlDataAdapter("SELECT RegionID FROM Region", connection);
            DataTable = new DataTable();
            adapter.Fill(DataTable);
            comboBox1.DataSource = DataTable;
            comboBox1.DisplayMember = "RegionID";

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
            this.Hide();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            // Verificar la cantidad de registros existentes en la tabla 'Region'
            int totalRegiones = ObtenerCantidadRegiones(); // Implementa esta función según tu lógica

            if (totalRegiones >= 4)
            {
                MessageBox.Show(
                    "Los registros de esta entidad están completos. No es necesario añadir mas registros.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

            }
            comboBox1.SelectedItem = null;
            LimpiarCampos();
        }
        private void LimpiarCampos()
        {
            textBox1.Text = "";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem is DataRowView selectedRowView)
            {
                int RegionID = Convert.ToInt32(selectedRowView.Row["RegionID"]);

                string query = "SELECT RegionDescription FROM Region WHERE RegionID = @RegionID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@RegionID", RegionID);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    textBox1.Text = reader["RegionDescription"].ToString();
                    reader.Close();
                    connection.Close();
                }

            }
        }
        private int ObtenerCantidadRegiones()
        {
            int cantidadRegiones = 0;
            try
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM Region";
                SqlCommand command = new SqlCommand(query, connection);
                cantidadRegiones = (int)command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener la cantidad de regiones: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return cantidadRegiones;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string RegionDescription = textBox1.Text;


                connection.Open();

                // Consulta para obtener el último valor de RegionID
                string maxRegionIdQuery = "SELECT MAX(RegionID) FROM Region";
                SqlCommand maxRegionIdCommand = new SqlCommand(maxRegionIdQuery, connection);
                int lastRegionId = Convert.ToInt32(maxRegionIdCommand.ExecuteScalar());

                // Incrementa el valor para el nuevo registro
                int newRegionId = lastRegionId + 1;

                // Crea la consulta SQL para insertar en la tabla 'Region'
                string insertQuery = "INSERT INTO Region (RegionID, RegionDescription) VALUES (@RegionID, @RegionDescription)";
                SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                insertCommand.Parameters.AddWithValue("@RegionID", newRegionId);
                insertCommand.Parameters.AddWithValue("@RegionDescription", RegionDescription);

                // Ejecuta la consulta
                int filasAfectadas = insertCommand.ExecuteNonQuery();

                if (filasAfectadas > 0)
                {
                    MessageBox.Show("Dato insertado correctamente");
                    LimpiarCampos(); // Implementa esta función para limpiar los campos después de la inserción
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
                // Cierra la conexión
                connection.Close();
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Region WHERE RegionID = @RegionID", connection);
                cmd.Parameters.AddWithValue("RegionID", int.Parse(comboBox1.Text));
                cmd.ExecuteNonQuery();
                MessageBox.Show("Registro eliminado correctamente");
                connection.Close();
                LimpiarCampos();


            }
            catch (Exception ex)
            {
                MessageBox.Show("No se elimino el registro", ex.Message);
            }
            connection.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                int RegionID = int.Parse(comboBox1.Text);

                string RegionDescription = textBox1.Text;
                connection.Open();

                string updateQuery = "UPDATE Region SET RegionDescription = @RegionDescription WHERE RegionID = @RegionID";
                SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
                updateCommand.Parameters.AddWithValue("@RegionDescription", RegionDescription);
                updateCommand.Parameters.AddWithValue("@RegionID", RegionID);

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



