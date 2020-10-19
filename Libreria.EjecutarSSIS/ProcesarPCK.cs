#region Derechos reservados
/*
 * Autor: Fabián Cataño Sánchez
 * Empresa: MVM Ingenieria de Software
 * Fecha de creacion: 20/09/2020 
 * Modificado por: Fabián Cataño Sánchez
 * Fecha de modificación: 20/09/2020   
 */
#endregion
using Microsoft.SqlServer.Dts.Runtime;
using System;
using System.Configuration;
using System.Data;
using System.Timers;

namespace Libreria.EjecutarSSIS35
{
    public class ProcesarPCK
    {
        private Timer ObjTemporizadorParametros = null;
        private Timer ObjTemporizadorEjecucionSSIS = null;
        private string pathPCK = string.Empty;
        private string periodoEjecucionActual = string.Empty;
        private string periodoEjecucion = string.Empty;
        public static bool activarLog = true;

        public ProcesarPCK()
        {
            ObjTemporizadorParametros = new Timer();
            ObjTemporizadorParametros.Elapsed += new ElapsedEventHandler(ChequeoParametros_Tick);
            ObjTemporizadorParametros.Interval = 10000;
            ObjTemporizadorParametros.Start();

            ObjTemporizadorEjecucionSSIS = new Timer();
            ObjTemporizadorEjecucionSSIS.Elapsed += new ElapsedEventHandler(ChequeoEjecucionSSIS_Tick);
            ConfiguracionParametros();

        }


        /// <summary>
        /// Validacion periodica de configuracion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChequeoParametros_Tick(object sender, EventArgs e)
        {
            ObjTemporizadorParametros.Stop();
            ConfiguracionParametros();
            ObjTemporizadorParametros.Start();

        }
        /// <summary>
        /// Ejecución periodica del PCK SSIS
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChequeoEjecucionSSIS_Tick(object sender, EventArgs e)
        {
            ObjTemporizadorEjecucionSSIS.Stop();

            var pkgResults = Execute_Package();
            if (pkgResults != DTSExecResult.Success)
            {
                Funciones.GuardarLogError("Package failed",activarLog);
            }

            ObjTemporizadorEjecucionSSIS.Start();
        }

        /// <summary>
        /// Ejecucion
        /// </summary>
        /// <returns></returns>
        public DTSExecResult Execute_Package()
        {
            try
            {
                string pkgLocation = this.pathPCK;
                EventListener eventListener = new EventListener();
                Package pkg;
                Application app;
                DTSExecResult pkgResults;
                Variables vars;

                app = new Application();
                pkg = app.LoadPackage(pkgLocation, eventListener);

                //vars = pkg.Variables;
                //vars["A_Variable"].Value = "Some value";
                pkgResults = pkg.Execute(null, null, eventListener, null, null);

                return pkgResults;
            }
            catch (Exception ex)
            {
                Funciones.GuardarLogError("Error >> " + ex.ToString(),activarLog);
                return DTSExecResult.Canceled;
            }
        }

        /// <summary>
        /// Validar parametros de configuracion
        /// </summary>
        private void ConfiguracionParametros()
        {
            try
            {
                string pathXML = ConfigurationManager.AppSettings["pathXML"].ToString();

                var parametros = Funciones.LeerXml(pathXML).Tables[0].Rows[0];        
                    
                this.pathPCK = parametros["RutaPCK"].ToString();
                this.periodoEjecucion = parametros["PeriodoEjecucion"].ToString();
                activarLog = bool.Parse(parametros["ActiVaLog"].ToString());

                if (periodoEjecucionActual != periodoEjecucion)
                {
                    ObjTemporizadorEjecucionSSIS.Stop();
                    ObjTemporizadorEjecucionSSIS.Interval = Convert.ToDouble(periodoEjecucion) * 60000;
                    ObjTemporizadorEjecucionSSIS.Start();
                }

                periodoEjecucionActual = periodoEjecucion;
            }
            catch (Exception ex)
            {
                Funciones.GuardarLogError("Error >> " + ex.ToString());
            }
        }
    }

}
