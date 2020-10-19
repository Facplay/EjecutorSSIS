using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Text;

namespace Libreria.EjecutarSSIS35
{
    /// <summary>
    /// Clase que se encarga de proveer utilidades o funcionalidades para el proyectos.
    /// </summary>
    public class Funciones
    {
        #region Métodos de la clase
        /// <summary>
        /// Generar un mensaje en el log. 
        /// </summary>
        /// <param name="Mensaje">Texto a guardar en el archivo</param>
        public static void GuardarLogError(string Mensaje, bool activarLog = true)
        {

            if (activarLog)
            {
                string StrNombre = DateTime.Now.ToString("yyyyMMdd") + ".txt";
                StreamWriter ObjStreamWriter = new StreamWriter(ConfigurationManager.AppSettings["LogError"].ToString() + StrNombre, true, Encoding.UTF8);
                ObjStreamWriter.WriteLine(DateTime.Now.ToString() + " ==> " + Mensaje + Environment.NewLine);
                ObjStreamWriter.Close();
                ObjStreamWriter = null;
            }
            else
            {
                Console.WriteLine(DateTime.Now.ToString() + " ==> " + Mensaje);
            }
        }

        /// <summary>
        /// Leer archivos XML y convertilos en DataSet
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static DataSet LeerXml(string path)
        {
            DataSet ds = new DataSet("Data");
            ds.ReadXml(path);
            return ds;
        }
        #endregion
    }
}
