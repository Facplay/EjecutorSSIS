using Libreria.EjecutarSSIS35;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
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
using System.Xml;

namespace InterfazEjecutarSSIS
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                string pathXML = ConfigurationManager.AppSettings["pathXML"].ToString();

                var parametros = Funciones.LeerXml(pathXML).Tables[0].Rows[0];

                this.txtRutaPCK.Text = parametros["RutaPCK"].ToString();
                this.txtPeriodo.Text = parametros["PeriodoEjecucion"].ToString();
                this.chkActivarLog.IsChecked = bool.Parse(parametros["ActiVaLog"].ToString());
            }
            catch (Exception ex)
            {
                Funciones.GuardarLogError("Error >> " + ex.ToString());
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                    txtRutaPCK.Text = openFileDialog.FileName;
                }
            }
            catch (Exception ex)
            {
                Funciones.GuardarLogError("Error >> " + ex.ToString());
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                lblMensaje.Content = "";
                string pathXML = ConfigurationManager.AppSettings["pathXML"].ToString();
                crearXML(pathXML);

                lblMensaje.Content = "Aplicado correctamente!";
            }
            catch (Exception ex)
            {
                lblMensaje.Content = "Error ocurrido!";

                Funciones.GuardarLogError("Error >> " + ex.ToString());
            }
        }

        private void TxtPeriodo_TextChanged(object sender, TextChangedEventArgs e)
        {

            int valor = 0;
            if (!Int32.TryParse(txtPeriodo.Text, out valor))
            {
                txtPeriodo.Text = txtPeriodo.Text.Length > 0 ? txtPeriodo.Text.Remove(txtPeriodo.Text.Length-1) : string.Empty;
            }
        }

        private void crearXML(String file_path)
        {
            XmlTextWriter writer;
            writer = new XmlTextWriter(file_path, Encoding.UTF8);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartDocument();
            writer.WriteStartElement("Configuracion");
            writer.WriteElementString("RutaPCK", txtRutaPCK.Text);
            writer.WriteElementString("PeriodoEjecucion", txtPeriodo.Text);
            writer.WriteElementString("ActiVaLog", (chkActivarLog.IsChecked).ToString());
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();
        }

        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            lblMensaje.Content = "";
        }
    }
}
