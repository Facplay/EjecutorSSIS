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

namespace Libreria.EjecutarSSIS35
{
    public class EventListener : DefaultEvents
    {
        /// <summary>
        /// Captura errores al ejecutar el PCK SSIS
        /// </summary>
        /// <param name="source"></param>
        /// <param name="errorCode"></param>
        /// <param name="subComponent"></param>
        /// <param name="description"></param>
        /// <param name="helpFile"></param>
        /// <param name="helpContext"></param>
        /// <param name="idofInterfaceWithError"></param>
        /// <returns></returns>
        public override bool OnError(DtsObject source, int errorCode, string subComponent,
          string description, string helpFile, int helpContext, string idofInterfaceWithError)
        {
            // Add application-specific diagnostics here.  
            Funciones.GuardarLogError($"Error {errorCode} in {source}/{subComponent} : {description} >> {idofInterfaceWithError}", ProcesarPCK.activarLog);
            return false;
        }
    }
}