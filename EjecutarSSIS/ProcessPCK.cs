using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Timers;
using Microsoft.SqlServer.Dts.Runtime;

namespace EjecutarSSIS
{
    public class ProcessPCK
    {
        private Timer ObjTemporizador = null;

        public ProcessPCK()
        {
            ObjTemporizador = new Timer();
            ObjTemporizador.Elapsed += new ElapsedEventHandler(Chequeo_Tick);
            ObjTemporizador.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["PeriodoEjecucion"]) * 60000;
            ObjTemporizador.Start();
        }

        /// <summary>
        /// Ejecución periodica del PCK SSIS
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Chequeo_Tick(object sender, EventArgs e)
        {
            ObjTemporizador.Stop();

            var pkgResults = Execute_Package();
            if (pkgResults == DTSExecResult.Success)
                Console.WriteLine(DateTime.Now + ": Package ran successfully");
            else
                Console.WriteLine(DateTime.Now + ": Package failed");

            ObjTemporizador.Start();
        }

        /// <summary>
        /// Ejecucion
        /// </summary>
        /// <returns></returns>
        public DTSExecResult Execute_Package()
        {         
            try
            {
                string pkgLocation = ConfigurationManager.AppSettings["PCKRuta"].ToString();
                MyEventListener eventListener = new MyEventListener();
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
                Console.WriteLine(DateTime.Now + ": "+ex.ToString());
                return DTSExecResult.Canceled;
            }
        }
    }

    public class MyEventListener : DefaultEvents
    {
        public override bool OnError(DtsObject source, int errorCode, string subComponent,
          string description, string helpFile, int helpContext, string idofInterfaceWithError)
        {
            // Add application-specific diagnostics here.  
            Console.WriteLine(DateTime.Now+": Error in {0}/{1} : {2}", source, subComponent, description);
            return false;
        }
    }
}
