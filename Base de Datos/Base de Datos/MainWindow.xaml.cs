using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.OleDb;//Agregamos libreria OleDB
using System.Data; //Agregamos System.Data

namespace WPFDBParte2
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        OleDbConnection con; //Agregamos la conexión
        DataTable dt;   //Agregamos la tabla
        public MainWindow()
        {
            InitializeComponent();
            //Conectamos la base de datos a nuestro archivo Access
            con = new OleDbConnection();
            con.ConnectionString = "Provider=Microsoft.Jet.Oledb.4.0; Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "\\BDMusica.mdb";
            MostrarDatos();
        }
        //Mostramos los registros de la tabla
        private void MostrarDatos()
        {
            OleDbCommand cmd = new OleDbCommand();
            if (con.State != ConnectionState.Open)
                con.Open();
            cmd.Connection = con;
            cmd.CommandText = "select * from Musica";
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);
            gvDatos.ItemsSource = dt.AsDataView();

            if (dt.Rows.Count > 0)
            {
                lbContenido.Visibility = System.Windows.Visibility.Hidden;
                gvDatos.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                lbContenido.Visibility = System.Windows.Visibility.Visible;
                gvDatos.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        //Limpiamos todos los campos
        private void LimpiaTodo()
        {
            txtIdCancion.Text = "";
            txtNombre.Text = "";
            cbGenero.SelectedIndex = 0;
            txtAutor.Text = "";
            txtAño.Text = "";
            btnNuevo.Content = "Nuevo";
            txtIdCancion.IsEnabled = true;
        }
        private void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            OleDbCommand cmd = new OleDbCommand();
            if (con.State != ConnectionState.Open)
                con.Open();
            cmd.Connection = con;

            if (txtIdCancion.Text != "")
            {
                if (txtIdCancion.IsEnabled == true)
                {
                    if (cbGenero.Text != "Selecciona Genero")
                    {
                        cmd.CommandText = "insert into Progra(Id,Nombre,Genero,Telefono,Direccion) " +
                            "Values(" + txtIdCancion.Text + ",'" + txtNombre.Text + "','" + cbGenero.Text + "'," + txtAutor.Text + ",'" + txtAño.Text + "')";
                        cmd.ExecuteNonQuery();
                        MostrarDatos();
                        MessageBox.Show("Alumno agregado correctamente...");
                        LimpiaTodo();

                    }
                    else
                    {
                        MessageBox.Show("Favor de seleccionar el genero....");
                    }
                }
                else
                {
                    cmd.CommandText = "update Progra set Nombre='" + txtNombre.Text + "',Genero='" + cbGenero.Text + "',Telefono=" + txtAutor.Text
                        + ",Direccion='" + txtAño.Text + "' where Id=" + txtIdCancion.Text;
                    cmd.ExecuteNonQuery();
                    MostrarDatos();
                    MessageBox.Show("Datos del alumno Actualizados...");
                    LimpiaTodo();
                }
            }
            else
            {
                MessageBox.Show("Favor de poner el ID de un Alumno.......");
            }
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (gvDatos.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)gvDatos.SelectedItems[0];
                txtIdCancion.Text = row["Id"].ToString();
                txtNombre.Text = row["Nombre"].ToString();
                cbGenero.Text = row["Genero"].ToString();
                txtAutor.Text = row["Autor"].ToString();
                txtAño.Text = row["Año"].ToString();
                txtIdCancion.IsEnabled = false;
                btnNuevo.Content = "Actualizar";
            }
            else
            {
                MessageBox.Show("Favor de seleccionar un alumno....");
            }
        }

        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (gvDatos.SelectedItems.Count > 0)
            {
                if (con.State != ConnectionState.Open)
                    con.Open();
                cmd.Connection = con;
                cmd.CommandText = "delete from Musica where Id =" + row["IdCancion"].ToString();
                cmd.ExecuteNonQuery();
                MostrarDatos();
                MessageBox.Show("Alumno eliminado correctamente....");
                LimpiaTodo();
            }

        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            LimpiaTodo();
        }

        private void BtnSalir_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
