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

    public class ProcesoGeoUbicacion
    {
        private static string _proceso = "ProcesoMonitoreoGeoUbicacion";

        private static SqlTableDependency<GeoUbicacion> _dependency;

        private static readonly string _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        /// <summary>
        /// Funcion que engloba el proceso de atender Videos de la BD para extensiones.
        /// </summary>
        public static void ProcesoMonitoreoGeoUbicaciones()
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
                dbAL.AgregarLogError("vacio", "servidor", "Emsys.ProcesoMonitoreoGeoUbicacion", "Program", 0, "_dependency_OnChanged", "Error al intentar capturar una GeoUbicacion en la bd. Excepcion: " + e.Message, MensajesParaFE.LogErrorObserverDataBaseGeoUbicaciones);
            }
        }

        /// <summary>
        /// Implentacion con sql table dependency para noticiar los cambios en la bd.
        /// </summary>
        public static void Listener()
        {
            var mapper = new ModelToTableMapper<GeoUbicacion>();
            mapper.AddMapping(model => model.Id, "Id");
            _dependency = new SqlTableDependency<GeoUbicacion>(_connectionString, "GeoUbicaciones", mapper);
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
            dbAL.AgregarLogError("vacio", "servidor", "Emsys.ProcesoMonitoreoGeoUbicacion", "Program", 0, "_dependency_OnChanged", "Error al intentar capturar una GeoUbicacion en la bd. Excepcion: " + e.Message, MensajesParaFE.LogErrorObserverDataBaseeGeoUbicacionesDependencyOnError);
        }

        /// <summary>
        /// Implementacion del metodo encargado de realizar la operativa de las notificaciones cuando se obtiene un cambvio en la bd.
        /// </summary>
        /// <param name="sender">no se usa</param>
        /// <param name="geoubicacionesEnBD">Evento Video generado desde la bd.</param>
        private static void DependencyOnChanged(object sender, TableDependency.EventArgs.RecordChangedEventArgs<GeoUbicacion> geoubicacionesEnBD)
        {
            try
            {
                if (geoubicacionesEnBD.ChangeType != ChangeType.None)
                {
                    Utils.Notifications.INotifications GestorNotificaciones = Utils.Notifications.FactoryNotifications.GetInstance();
                    switch (geoubicacionesEnBD.ChangeType)
                    {
                        // El caso no es util por que si se crea un evento no tiene asignados recursos probablemente.
                        case ChangeType.Delete:
                            Console.WriteLine("ProcesoMonitoreoVideos - Accion: Borro, Pk del evento: " + geoubicacionesEnBD.Entity.Id);
                            break;
                        case ChangeType.Insert:
                            Console.WriteLine("ProcesoMonitoreoVideos - Accion Insert, Pk del evento: " + geoubicacionesEnBD.Entity.Id);
                            AtenderEvento(DataNotificacionesCodigos.AltaGeoubicacion, geoubicacionesEnBD, GestorNotificaciones);
                            break;
                        case ChangeType.Update:
                            Console.WriteLine("ProcesoMonitoreoVideos - Accion update, Pk del evento: " + geoubicacionesEnBD.Entity.Id);
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                IMetodos dbAL = new Metodos();
                dbAL.AgregarLogError("vacio", "servidor", "Emsys._dependency_OnChangedGeoUbicacion", "Program", 0, "_dependency_OnChanged", "Error al intentar capturar un evento en la bd. Excepcion: " + e.Message, MensajesParaFE.LogCapturarCambioEventoCod);
            }
        }

        /// <summary>
        /// Metodo que se utiliza para enviar una notificaion a un Video.
        /// </summary>
        /// <param name="cod">Codigo que se desea notificar a la aplicacion dado el Video.</param>
        /// <param name="GeoUbicacionEvento">Identificador de GeoUbicacion que fue modificado/alta/baja.</param>
        /// <param name="GestorNotificaciones">Instancia de INotification.</param>
        private static void AtenderEvento(string cod, TableDependency.EventArgs.RecordChangedEventArgs<GeoUbicacion> GeoUbicacionEvento, Utils.Notifications.INotifications GestorNotificaciones)
        {
            using (EmsysContext db = new EmsysContext())
            {
                IMetodos dbAL = new Metodos();
                dbAL.AgregarLog("vacio", "servidor", "Emsys.ObserverDataBase", "GeoUbicacion", GeoUbicacionEvento.Entity.Id, "_dependency_OnChanged", "Se captura una modificacion de la base de datos para la tabla GeoUbicacion. Se inicia la secuencia de envio de notificaciones.", MensajesParaFE.LogCapturarCambioEventoCod);
                var GeoUbicacionDEBD = db.GeoUbicaciones.Find(GeoUbicacionEvento.Entity.Id);
                if (GeoUbicacionDEBD != null)
                {
                    if (GeoUbicacionDEBD.ExtensionEvento != null)
                    {
                        int idEvento = GeoUbicacionDEBD.ExtensionEvento.Evento.Id;
                        int idExtension = GeoUbicacionDEBD.ExtensionEvento.Id;
                        int idZona = GeoUbicacionDEBD.ExtensionEvento.Zona.Id;
                        string nombreZona = GeoUbicacionDEBD.ExtensionEvento.Zona.Nombre;
                        // Para cada extension del evento modificado.
                        foreach (var item in GeoUbicacionDEBD.ExtensionEvento.Evento.ExtensionesEvento)
                        {
                            // Para cada recurso de la extension.
                            foreach (var asig in item.AsignacionesRecursos)
                            {
                                if ((asig.ActualmenteAsignado == true) && (asig.Recurso.Estado == EstadoRecurso.NoDisponible))
                                {
                                    GestorNotificaciones.SendMessage(cod, idEvento, idExtension, idZona, nombreZona, "recurso-" + asig.Recurso.Id);
                                }
                            }
                            // Para la zona asociada a la extension le envia una notificacion.
                            GestorNotificaciones.SendMessage(cod, idEvento, idExtension, idZona, nombreZona, "zona-" + item.Zona.Id);
                        }
                    }
                }
            }
        }
    }
}