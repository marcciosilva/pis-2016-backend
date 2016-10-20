﻿
namespace SqlDependecyProject
{
    using System;
    using TableDependency.Mappers;
    using TableDependency.SqlClient;
    using TableDependency.Enums;
    using Emsys.DataAccesLayer.Model;
    using Emsys.DataAccesLayer.Core;
    using System.Linq;
    using DataTypeObject;
    using Emsys.LogicLayer;
    using System.Threading;

    public class ProcesoAsignacionRecursoDescripcion
    {

        private static string proceso = "ProcesoAsignacionRecursoDescripcion";

        /// <summary>
        /// Funcion que engloba el proceso de atender eventos de la BD para extensiones.
        /// </summary>
        public static void ProcesoMonitorearAsignacionRecursoDescripcion()
        {
            try
            {

                Console.WriteLine(proceso +"- Observo la BD:\n");
                Listener();

                while (true)
                {
                    Thread.Sleep(10000);
                }
            }
            catch (Exception e)
            {
                IMetodos dbAL = new Metodos();
                dbAL.AgregarLogError("vacio", "servidor", "Emsys.ProcesoAsignacionRecursoDescripcion", "Program", 0, "_dependency_OnChanged", "Error al intentar capturar un AsignacionRecursoDescripcion en la bd. Excepcion: " + e.Message, CodigosLog.LogCapturarCambioEventoCod);
                throw e;
            }
        }

        private static SqlTableDependency<AsignacionRecursoDescripcion> _dependency;
        private static readonly string _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;


        /// <summary>
        /// Implentacion con sql table dependency para noticiar los cambios en la bd.
        /// </summary>
        public static void Listener()
        {
            var mapper = new ModelToTableMapper<AsignacionRecursoDescripcion>();
            mapper.AddMapping(model => model.Id, "Id");
            _dependency = new SqlTableDependency<AsignacionRecursoDescripcion>(_connectionString, "AsignacionRecursoDescripcion", mapper);
            _dependency.OnChanged += _dependency_OnChanged;
            _dependency.OnError += _dependency_OnError;
            _dependency.Start();
        }

      



        /// <summary>
        /// Metodo que se dispara cuando ocurre un error al detectar los cambios en la base de datos.
        /// </summary>
        /// <param name="sender">No se utiliza.</param>
        /// <param name="e">Excepcion generada por el sistema de error.</param>
        private static void _dependency_OnError(object sender, TableDependency.EventArgs.ErrorEventArgs e)
        {
            throw e.Error;
        }

        /// <summary>
        /// Implementacion del metodo encargado de realizar la operativa de las notificaciones cuando se obtiene un cambvio en la bd.
        /// </summary>
        /// <param name="sender">no se usa</param>
        /// <param name="eventoEnBD">Evento generado desde la bd.</param>
        private static void _dependency_OnChanged(object sender, TableDependency.EventArgs.RecordChangedEventArgs<AsignacionRecursoDescripcion> AsignacionRecursoDescripcionEnDb)
        {
            try
            {
                if (AsignacionRecursoDescripcionEnDb.ChangeType != ChangeType.None)
                {
                    Utils.Notifications.INotifications GestorNotificaciones = Utils.Notifications.FactoryNotifications.GetInstance();
                    switch (AsignacionRecursoDescripcionEnDb.ChangeType)
                    {
                        // el caso no es util por que si se crea un evento no tiene asignados recursos probablemte
                        case ChangeType.Delete:
                            Console.WriteLine("ProcesoMonitoreoExtensiones - Accion: Borro, Pk del evento: " + AsignacionRecursoDescripcionEnDb.Entity.Id);
                            AtenderEvento(DataNotificacionesCodigos.CierreEvento, AsignacionRecursoDescripcionEnDb, GestorNotificaciones);
                            break;
                        case ChangeType.Insert:
                            Console.WriteLine("ProcesoMonitoreoExtensiones - Accion Insert, Pk del evento: " + AsignacionRecursoDescripcionEnDb.Entity.Id);
                            AtenderEvento(DataNotificacionesCodigos.AltaEvento, AsignacionRecursoDescripcionEnDb, GestorNotificaciones);
                            break;
                        case ChangeType.Update:
                            Console.WriteLine("ProcesoMonitoreoExtensiones - Accion update, Pk del evento: " + AsignacionRecursoDescripcionEnDb.Entity.Id);
                            AtenderEvento(DataNotificacionesCodigos.ModificacionEvento, AsignacionRecursoDescripcionEnDb, GestorNotificaciones);
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                IMetodos dbAL = new Metodos();
                dbAL.AgregarLogError("vacio", "servidor", "Emsys._dependency_OnChangedAsignacionRecursoDescripcion", "Program", 0, "_dependency_OnChanged", "Error al intentar capturar un AsignacionRecursoDescripcion en la bd. Excepcion: " + e.Message, CodigosLog.LogCapturarCambioEventoCod);
                throw e;
            }
        }
        /// <summary>
        /// Metodo que se utiliza para enviar una notificaion a un evento.
        /// </summary>
        /// <param name="cod">Codigo que se desea notificar a la aplicacion dado el evento.</param>
        /// <param name="extension">Identificador del evento que fue modificado/alta/baja.</param>
        /// <param name="GestorNotificaciones">Instancia de INotification.</param>
        private static void AtenderEvento(string cod, TableDependency.EventArgs.RecordChangedEventArgs<AsignacionRecursoDescripcion> asignacionRecursoDescripcion, Utils.Notifications.INotifications GestorNotificaciones)
        {
            using (EmsysContext db = new EmsysContext())
            {

                IMetodos dbAL = new Metodos();
                dbAL.AgregarLog("vacio", "servidor", "Emsys.ObserverDataBase", "Evento", asignacionRecursoDescripcion.Entity.Id, "_dependency_OnChanged", "Se captura una modificacion de la base de datos para la tabla video. Se inicia la secuencia de envio de notificaciones.", CodigosLog.LogCapturarCambioEventoCod);
                var asignacionRecursoDescripcionEnDB = db.AsignacionRecursoDescripcion.Find(asignacionRecursoDescripcion.Entity.Id);
                if (asignacionRecursoDescripcionEnDB != null)
                {                    
                    //para los recursos asociados a la extension genero una notificacion                   
                    GestorNotificaciones.SendMessage(cod, asignacionRecursoDescripcion.Entity.Id.ToString(), "recurso-" + asignacionRecursoDescripcionEnDB.AsignacionRecurso.Recurso.Id);
                       
                    //para la zona asociada a la extensen le envia una notificacion
                   // GestorNotificaciones.SendMessage(cod, asignacionRecursoDescripcion.Entity.Id.ToString(), "zona-" + asignacionRecursoDescripcionEnDB.AsignacionRecurso.Recurso);
                   
                }
            }
        }
    }
}

