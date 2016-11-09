namespace SqlDependecyProject
{
    using System;
    using System.Threading;
    using DataTypeObject;
    using Emsys.DataAccesLayer.Core;
    using Emsys.DataAccesLayer.Model;
    using Emsys.LogicLayer;
    using TableDependency.Enums;
    using TableDependency.Mappers;
    using TableDependency.SqlClient;

    public class ProcesoAsignacionRecurso
    {
        private static bool llamo = true;

        private static string proceso = "ProcesoMonitoreoAsignacionRecurso";

        private static SqlTableDependency<AsignacionRecurso> _dependency;

        private static readonly string _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        /// <summary>
        /// Funcion que engloba el proceso de atender AsignacionRecurso de la BD para extensiones.
        /// </summary>
        public static void ProcesoAsignacionRecursoMonitoreo()
        {
            try
            {
                Console.WriteLine(proceso + "- Observo la BD:\n");
                Listener();

                while (true)
                {
                    Thread.Sleep(10000);
                }
            }
            catch (Exception e)
            {
                IMetodos dbAL = new Metodos();
                dbAL.AgregarLogError("vacio", "servidor", "Emsys.ProcesoAsignacionRecurso", "Program", 0, "_dependency_OnChanged", "Error al intentar capturar un Video en la bd. Excepcion: " + e.Message, MensajesParaFE.LogCapturarCambioEventoCod);
                throw e;
            }
        }

        /// <summary>
        /// Implentacion con sql table dependency para noticiar los cambios en la bd.
        /// </summary>
        public static void Listener()
        {
            var mapper = new ModelToTableMapper<AsignacionRecurso>();
            mapper.AddMapping(model => model.Id, "Id");
            _dependency = new SqlTableDependency<AsignacionRecurso>(_connectionString, "AsignacionesRecursos", mapper);
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
        /// <param name="AsignacionRecursoDB"></param>
        private static void _dependency_OnChanged(object sender, TableDependency.EventArgs.RecordChangedEventArgs<AsignacionRecurso> AsignacionRecursoDB)
        {
            try
            {
                if (AsignacionRecursoDB.ChangeType != ChangeType.None)
                {
                    Utils.Notifications.INotifications GestorNotificaciones = Utils.Notifications.FactoryNotifications.GetInstance();
                    switch (AsignacionRecursoDB.ChangeType)
                    {
                        // El caso no es util por que si se crea un evento no tiene asignados recursos probablemte.
                        case ChangeType.Delete:
                            Console.WriteLine("ProcesoAsignacionRecursoMonitoreo - Accion: Borro, Pk del evento: " + AsignacionRecursoDB.Entity.Id);
                            AtenderEvento(DataNotificacionesCodigos.AsignacionEvento, AsignacionRecursoDB, GestorNotificaciones);
                            break;
                        case ChangeType.Insert:
                            Console.WriteLine("ProcesoAsignacionRecursoMonitoreo - Accion Insert, Pk del evento: " + AsignacionRecursoDB.Entity.Id);
                            AtenderEvento(DataNotificacionesCodigos.AsignacionEvento, AsignacionRecursoDB, GestorNotificaciones);
                            break;
                        case ChangeType.Update:
                            Console.WriteLine("ProcesoAsignacionRecursoMonitoreo - Accion update, Pk del evento: " + AsignacionRecursoDB.Entity.Id);
                            AtenderEvento(DataNotificacionesCodigos.AsignacionEvento, AsignacionRecursoDB, GestorNotificaciones);
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                IMetodos dbAL = new Metodos();
                dbAL.AgregarLogError("vacio", "servidor", "Emsys._dependency_OnChangedVideos", "Program", 0, "_dependency_OnChanged", "Error al intentar capturar un evento en la bd. Excepcion: " + e.Message, MensajesParaFE.LogCapturarCambioEventoCod);
                throw e;
            }
        }

        /// <summary>
        /// Metodo que se utiliza para enviar una notificaion a un Video.
        /// </summary>
        /// <param name="cod">Codigo que se desea notificar a la aplicacion dado el Video.</param>
        /// <param name="asinacionRecurso">Identificador del Video que fue modificado/alta/baja.</param>
        /// <param name="GestorNotificaciones">Instancia de INotification.</param>
        private static void AtenderEvento(string cod, TableDependency.EventArgs.RecordChangedEventArgs<AsignacionRecurso> asinacionRecurso, Utils.Notifications.INotifications GestorNotificaciones)
        {
            using (EmsysContext db = new EmsysContext())
            {
                IMetodos dbAL = new Metodos();
                dbAL.AgregarLog("vacio", "servidor", "Emsys.ObserverDataBase", "Video", asinacionRecurso.Entity.Id, "_dependency_OnChanged", "Se captura una modificacion de la base de datos para la tabla video. Se inicia la secuencia de envio de notificaciones.", MensajesParaFE.LogCapturarCambioEventoCod);
                var asgnacionRecursoEnDB = db.AsignacionesRecursos.Find(asinacionRecurso.Entity.Id);
                if (asgnacionRecursoEnDB != null)
                {
                    // Para cada extension del evento modificado.
                    foreach (var item in asgnacionRecursoEnDB.Extension.Evento.ExtensionesEvento)
                    {
                        // Para cada recurso de la extension.
                        foreach (var recurso in asgnacionRecursoEnDB.Extension.Recursos)
                        {
                            if (recurso.Estado == EstadoRecurso.NoDisponible)
                            {
                                GestorNotificaciones.SendMessage(cod, asgnacionRecursoEnDB.Extension.Id.ToString(), "recurso-" + recurso.Id);
                            }
                        }
                        // Para la zona asociada a la extensen le envia una notificacion.
                        GestorNotificaciones.SendMessage(cod, asgnacionRecursoEnDB.Extension.Id.ToString(), "zona-" + item.Zona.Id);
                    }
                }
            }
        }
    }
}