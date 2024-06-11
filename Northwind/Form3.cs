using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace Northwind
{
    public partial class Form3 : Form
    {
        string connectionString = @"Data Source=DESKTOP-FT33RAV;Initial Catalog=Northwind;Integrated Security=True;TrustServerCertificate=true;";
        private SqlConnection connection;
        private SqlDataAdapter adapter;
        private DataTable DataTable;

        public Form3()
        {
            InitializeComponent();
            //Inicializar la conexion y el adaptador
            connection = new SqlConnection(connectionString);
            adapter = new SqlDataAdapter("SELECT CategoryID FROM Categories", connection);
            DataTable = new DataTable();
            adapter.Fill(DataTable);
            // Asignar los datos al Combobox
            comboBox1.DataSource = DataTable;
            comboBox1.DisplayMember = "CategoryID";

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form2 Form2 = new Form2();
            Form2.Show();
            this.Hide();
        }
        public static Bitmap ByteToImage(byte[] blob)
        {

            try
            {
                TypeConverter tc = TypeDescriptor.GetConverter(typeof(Bitmap));
                Bitmap bmp = (Bitmap)tc.ConvertFrom(blob);
                return bmp;

            }
            catch (Exception ex)
            {
                // Manejar la excepción (puedes mostrar un mensaje o registrar el error)
                MessageBox.Show("Error al convertir bytes a imagen: " + ex.Message);
                return null; // Otra opción es devolver null en caso de error
            }
        }


        private void Form3_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedItem = null;
            LimpiarCampos();
         
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            {
                // Obtener el CategoryID seleccionado
                if (comboBox1.SelectedItem is DataRowView selectedRowView)
                {


                    int CategoryID = Convert.ToInt32(selectedRowView.Row["CategoryID"]);
                    // Obtener los datos de la categoría seleccionada
                    string query = "SELECT CategoryName, Description, Picture FROM Categories WHERE CategoryID = @CategoryID";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@CategoryID", CategoryID);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        textBox1.Text = reader["CategoryName"].ToString();
                        textBox2.Text = reader["Description"].ToString();
                        byte[] imageBytes = (byte[])reader["Picture"];
                        // Convertir los bytes a un objeto Bitmap (Image)
                        Bitmap categoryBitmap = ByteToImage(imageBytes);
                        if (categoryBitmap != null)
                        {
                            // Mostrar la imagen en el PictureBox
                            pictureBox1.Image = categoryBitmap;
                        }
                        else
                        {
                            // Manejar el caso de error (por ejemplo, mostrar un mensaje)
                            MessageBox.Show("Error al cargar la imagen.");
                        }



                        reader.Close();
                        connection.Close();
                    }
                }


            }
        }
        private byte[] ImageToByteArray(Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string categoryName = textBox1.Text;
                string description = textBox2.Text;

                // obtener la imagen actual del pictureBox

                Image categoryImage = pictureBox1.Image;
                byte[] imageBytes = ImageToByteArray(categoryImage);
                //Abir la conexion
                connection.Open();
                //Consulta SQL para insertar
                string insertQuery = "INSERT INTO Categories(CategoryName, Description, Picture) VALUES (@CategoryName, @Description, @Picture)";
                SqlCommand insertCommand = new SqlCommand(insertQuery, connection);

                insertCommand.Parameters.AddWithValue("@CategoryName", categoryName);
                insertCommand.Parameters.AddWithValue("@Description", description);
                insertCommand.Parameters.AddWithValue("@Picture", imageBytes);

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
        private void LimpiarCampos()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            pictureBox1.Image = null;



        }

        private void button5_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Archivos de imagen|*.jpg;*.jpeg;*.png;*.gif|Todos los archivos|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string imagePath = openFileDialog.FileName;
                        pictureBox1.Image = Image.FromFile(imagePath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al cargar la imagen: " + ex.Message);
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Categories WHERE CategoryID = @CategoryID", connection);
                cmd.Parameters.AddWithValue("CategoryID", int.Parse(comboBox1.Text));
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

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                int categoryID = int.Parse(comboBox1.Text);

                string categoryName = textBox1.Text;
                string description = textBox2.Text;
                byte[] picture = ImageToByteArray(pictureBox1.Image);
                connection.Open();

                string updateQuery = "UPDATE Categories SET CategoryName = @CategoryName, Description = @Description, Picture = @Picture WHERE CategoryID = @CategoryID";
                SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
                updateCommand.Parameters.AddWithValue("@CategoryName", categoryName);
                updateCommand.Parameters.AddWithValue("@Description", description);
                updateCommand.Parameters.AddWithValue("@Picture", picture);
                updateCommand.Parameters.AddWithValue("@CategoryID", categoryID);

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
        }
    }

