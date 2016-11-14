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

    public class ProcesoAsignacionRecursoDescripcion
    {
        private static string _proceso = "ProcesoAsignacionRecursoDescripcion";

        private static SqlTableDependency<AsignacionRecursoDescripcion> _dependency;

        private static readonly string _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        /// <summary>
        /// Funcion que engloba el proceso de atender evento de la BD para AsignacionRecursoDescripcion.
        /// </summary>
        public static void ProcesoMonitorearAsignacionRecursoDescripcion()
        {
            try
            {
                Console.WriteLine(_proceso + "- Observo la BD:\n");
                Listener();
                while (true)
                {
                    Thread.Sleep(10000);
                }
            }
            catch (Exception e)
            {
                IMetodos dbAL = new Metodos();
                dbAL.AgregarLogError("vacio", "servidor", "Emsys.ProcesoAsignacionRecursoDescripcion", "ProcesoAsignacionRecursoDescripcion", 0, "_dependency_OnChanged", "Error al intentar capturar un AsignacionRecursoDescripcion en la bd. Excepcion: " + e.Message, MensajesParaFE.LogErrorObserverDataBaseAsignacionRecursoDescripcion);
            }
        }

        /// <summary>
        /// Implentacion con sql table dependency para noticiar los cambios en la bd.
        /// </summary>
        public static void Listener()
        {
            var mapper = new ModelToTableMapper<AsignacionRecursoDescripcion>();
            mapper.AddMapping(model => model.Id, "Id");
            _dependency = new SqlTableDependency<AsignacionRecursoDescripcion>(_connectionString, "AsignacionRecursoDescripcion", mapper);
            _dependency.OnChanged += DependencyOnChanged;
            _dependency.OnError += DependencyOnError;
            _dependency.Start();
        }

        /// <summary>
        /// Metodo que se dispara cuando ocurre un error al detectar los cambios en la base de datos.
        /// </summary>
        /// <param name="sender">No se utiliza.</param>
        /// <param name="e">Excepcion generada por el sistema de error.</param>
        private static void DependencyOnError(object sender, TableDependency.EventArgs.ErrorEventArgs e)
        {
            IMetodos dbAL = new Metodos();
            dbAL.AgregarLogError("vacio", "servidor", "Emsys.ProcesoAsignacionRecursoDescripcion", "ProcesoAsignacionRecursoDescripcion", 0, "_dependency_OnChanged", "Error al intentar capturar un AsignacionRecursoDescripcion en la bd. Excepcion: " + e.Message, MensajesParaFE.LogErrorObserverDataBaseAsignacionRecursoDescripcionDependencyOnError);
        }

        /// <summary>
        /// Implementacion del metodo encargado de realizar la operativa de las notificaciones cuando se obtiene un cambvio en la bd.
        /// </summary>
        /// <param name="sender">no se usa</param>
        /// <param name="AsignacionRecursoDescripcionEnDb">evento en AsignacionRecursoDescripcion generado desde la bd.</param>
        private static void DependencyOnChanged(object sender, TableDependency.EventArgs.RecordChangedEventArgs<AsignacionRecursoDescripcion> AsignacionRecursoDescripcionEnDb)
        {
            try
            {
                if (AsignacionRecursoDescripcionEnDb.ChangeType != ChangeType.None)
                {
                    Utils.Notifications.INotifications GestorNotificaciones = Utils.Notifications.FactoryNotifications.GetInstance();
                    switch (AsignacionRecursoDescripcionEnDb.ChangeType)
                    {
                        // El caso no es util por que si se crea un evento no tiene asignados recursos probablemte.
                        case ChangeType.Delete:
                            Console.WriteLine("ProcesoMonitorearAsignacionRecursoDescripcion - Accion: Borro, Pk del evento: " + AsignacionRecursoDescripcionEnDb.Entity.Id);
                            break;
                        case ChangeType.Insert:
                            Console.WriteLine("ProcesoMonitorearAsignacionRecursoDescripcion - Accion Insert, Pk del evento: " + AsignacionRecursoDescripcionEnDb.Entity.Id);
                            AtenderEvento(DataNotificacionesCodigos.ModificacionEvento, AsignacionRecursoDescripcionEnDb, GestorNotificaciones);
                            break;
                        case ChangeType.Update:
                            Console.WriteLine("ProcesoMonitorearAsignacionRecursoDescripcion - Accion update, Pk del evento: " + AsignacionRecursoDescripcionEnDb.Entity.Id);
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                IMetodos dbAL = new Metodos();
                dbAL.AgregarLogError("vacio", "servidor", "Emsys._dependency_OnChangedAsignacionRecursoDescripcion", "Program", 0, "_dependency_OnChanged", "Error al intentar capturar un AsignacionRecursoDescripcion en la bd. Excepcion: " + e.Message, MensajesParaFE.LogCapturarCambioEventoCod);
            }
        }

        /// <summary>
        /// Metodo que se utiliza para enviar una notificaion a un AsignacionRecursoDescripcion.
        /// </summary>
        /// <param name="cod">Codigo que se desea notificar a la aplicacion dado el AsignacionRecursoDescripcion.</param>
        /// <param name="asignacionRecursoDescripcion">Instancia de INotification.</param>
        /// <param name="GestorNotificaciones">Instancia de GestorNotificaciones.</param>
        private static void AtenderEvento(string cod, TableDependency.EventArgs.RecordChangedEventArgs<AsignacionRecursoDescripcion> asignacionRecursoDescripcion, Utils.Notifications.INotifications GestorNotificaciones)
        {
            using (EmsysContext db = new EmsysContext())
            {
                IMetodos dbAL = new Metodos();
                dbAL.AgregarLog("vacio", "servidor", "Emsys.ObserverDataBase", "AsignacionRecursoDescripcion", asignacionRecursoDescripcion.Entity.Id, "_dependency_OnChanged", "Se captura una modificacion de la base de datos para la tabla video. Se inicia la secuencia de envio de notificaciones.", MensajesParaFE.LogCapturarCambioEventoCod);
                var asignacionRecursoDescripcionEnDB = db.AsignacionRecursoDescripcion.Find(asignacionRecursoDescripcion.Entity.Id);
                if (asignacionRecursoDescripcionEnDB != null)
                {
                    int idEvento = asignacionRecursoDescripcionEnDB.AsignacionRecurso.Extension.Evento.Id;
                    int idExtension = asignacionRecursoDescripcionEnDB.AsignacionRecurso.Extension.Id;
                    int idZona = asignacionRecursoDescripcionEnDB.AsignacionRecurso.Extension.Zona.Id;
                    string nombreZona = asignacionRecursoDescripcionEnDB.AsignacionRecurso.Extension.Zona.Nombre;
                    // Para cada extension del evento modificado.
                    foreach (var item in asignacionRecursoDescripcionEnDB.AsignacionRecurso.Extension.Evento.ExtensionesEvento)
                    {
                        // Para cada recurso de la extension.
                        foreach (var asig in item.AsignacionesRecursos)
                        {
                            if ((asig.ActualmenteAsignado == true) && (asig.Recurso.Estado == EstadoRecurso.NoDisponible))
                            {
                                GestorNotificaciones.SendMessage(cod, idEvento, idExtension, idZona, nombreZona, "recurso-" + asig.Recurso.Id);
                            }
                        }
                        // Para la zona asociada a la extensen le envia una notificacion.
                        GestorNotificaciones.SendMessage(cod, idEvento, idExtension, idZona, nombreZona, "zona-" + item.Zona.Id);
                    }
                }
            }
        }
    }
}