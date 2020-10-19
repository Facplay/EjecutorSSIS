using Libreria.EjecutarSSIS35;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace WServiceEjecutarSSIS40
{
    public partial class ServiceEjecutarSSIS : ServiceBase
    {
        public ServiceEjecutarSSIS()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Funciones.GuardarLogError("Servicio iniciado");

            ProcesarPCK objProcessPCK = new ProcesarPCK();
        }

        protected override void OnStop()
        {
            Funciones.GuardarLogError("Servicio detenido");
        }
    }
}
