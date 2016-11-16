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
    using System.Collections.Generic;
    using System.Web.Configuration;

    public class ProcesoVideos
    {
        private static string _proceso = "ProcesoMonitoreoVideos";

        private static SqlTableDependency<Video> _dependency;

        private static readonly string _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        /// <summary>
        /// Funcion que engloba el proceso de atender Videos de la BD para extensiones.
        /// </summary>
        public static void ProcesoMonitoreoVideos()
        {
            try
            {
                Console.WriteLine(_proceso + "- Observo la BD:\n");
                Listener();

                while (true)
                {
                    //esta logica lo que hacer es reinciar la conexion a la base de datos.
                    int _milisegundosDuermo = Convert.ToInt32(WebConfigurationManager.AppSettings["TiempoEsperaReiniciarConexionBdObservers"]);
                    Thread.Sleep(_milisegundosDuermo);
                    _dependency.Stop();
                    Listener();
                }
            }
            catch (Exception e)
            {
                IMetodos dbAL = new Metodos();
                dbAL.AgregarLogError("vacio", "servidor", "Emsys.ProcesoMonitoreoVideos", "ProcesoMonitoreoVideos", 0, "_dependency_OnChanged", "Error al intentar capturar un Video en la bd. Excepcion: " + e.Message, MensajesParaFE.LogErrorObserverDataBaseVideo);
                _dependency.Stop();
                ProcesoMonitoreoVideos();
            }
        }

        /// <summary>
        /// Implentacion con sql table dependency para noticiar los cambios en la bd.
        /// </summary>
        public static void Listener()
        {
            var mapper = new ModelToTableMapper<Video>();
            mapper.AddMapping(model => model.Id, "Id");
            _dependency = new SqlTableDependency<Video>(_connectionString, "Videos", mapper);
            _dependency.OnChanged += DependencyOnChanged;
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
            IMetodos dbAL = new Metodos();
            dbAL.AgregarLogError("vacio", "servidor", "Emsys.ProcesoMonitoreoVideos", "ProcesoMonitoreoVideos", 0, "_dependency_OnChanged", "Error al intentar capturar un Video en la bd. Excepcion: " + e.Message, MensajesParaFE.LogErrorObserverDataBaseeVideoDependencyOnError);
        }

        /// <summary>
        /// Implementacion del metodo encargado de realizar la operativa de las notificaciones cuando se obtiene un cambvio en la bd.
        /// </summary>
        /// <param name="sender">no se usa</param>
        /// <param name="videoEnBD">Evento Video generado desde la bd.</param>
        private static void DependencyOnChanged(object sender, TableDependency.EventArgs.RecordChangedEventArgs<Video> videoEnBD)
        {
            try
            {
                if (videoEnBD.ChangeType != ChangeType.None)
                {
                    Utils.Notifications.INotifications GestorNotificaciones = Utils.Notifications.FactoryNotifications.GetInstance();
                    switch (videoEnBD.ChangeType)
                    {
                        // El caso no es util por que si se crea un evento no tiene asignados recursos probablemente.
                        case ChangeType.Delete:
                            Console.WriteLine("ProcesoMonitoreoVideos - Accion: Borro, Pk del evento: " + videoEnBD.Entity.Id);
                            break;
                        case ChangeType.Insert:
                            Console.WriteLine("ProcesoMonitoreoVideos - Accion Insert, Pk del evento: " + videoEnBD.Entity.Id);
                            AtenderEvento(DataNotificacionesCodigos.AltaVideo, videoEnBD, GestorNotificaciones);
                            break;
                        case ChangeType.Update:
                            Console.WriteLine("ProcesoMonitoreoVideos - Accion update, Pk del evento: " + videoEnBD.Entity.Id);
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                IMetodos dbAL = new Metodos();
                dbAL.AgregarLogError("vacio", "servidor", "Emsys._dependency_OnChangedVideos", "Program", 0, "_dependency_OnChanged", "Error al intentar capturar un evento en la bd. Excepcion: " + e.Message, MensajesParaFE.LogCapturarCambioEventoCod);
            }
        }

        /// <summary>
        /// Metodo que se utiliza para enviar una notificaion a un Video.
        /// </summary>
        /// <param name="cod">Codigo que se desea notificar a la aplicacion dado el Video.</param>
        /// <param name="video">Identificador del Video que fue modificado/alta/baja.</param>
        /// <param name="GestorNotificaciones">Instancia de INotification.</param>
        private static void AtenderEvento(string cod, TableDependency.EventArgs.RecordChangedEventArgs<Video> video, Utils.Notifications.INotifications GestorNotificaciones)
        {
            using (EmsysContext db = new EmsysContext())
            {
                IMetodos dbAL = new Metodos();
                dbAL.AgregarLog("vacio", "servidor", "Emsys.ObserverDataBase", "Video", video.Entity.Id, "_dependency_OnChanged", "Se captura una modificacion de la base de datos para la tabla video. Se inicia la secuencia de envio de notificaciones.", MensajesParaFE.LogCapturarCambioEventoCod);
                var videoDEBD = db.Videos.Find(video.Entity.Id);
                if (videoDEBD != null) 
                {
                    if (videoDEBD.ExtensionEvento != null)
                    {
                        List<int> recursosNotificados = new List<int>();
                        int idEvento = videoDEBD.ExtensionEvento.Evento.Id;
                        int idExtension = videoDEBD.ExtensionEvento.Id;
                        int idZona = videoDEBD.ExtensionEvento.Zona.Id;
                        string nombreZona = videoDEBD.ExtensionEvento.Zona.Nombre;
                        // Para cada extension del evento modificado.
                        foreach (var item in videoDEBD.ExtensionEvento.Evento.ExtensionesEvento)
                        {
                            // Para cada recurso de la extension.
                            foreach (var asig in item.AsignacionesRecursos)
                            {
                                // Si hay un usuario conectado con ese recurso.
                                if ((asig.ActualmenteAsignado == true) && (asig.Recurso.Estado == EstadoRecurso.NoDisponible) && (!recursosNotificados.Contains(asig.Recurso.Id)) && (asig.Recurso.Usuario.Id != videoDEBD.Usuario.Id))
                                {
                                    GestorNotificaciones.SendMessage(cod, idEvento, idExtension, idZona, nombreZona, "recurso-" + asig.Recurso.Id);
                                    recursosNotificados.Add(asig.Recurso.Id);
                                }
                            }
                            if (item.Zona.Usuarios.Count != 0)
                            {
                                // Para la zona asociada a la extensen le envia una notificacion.
                                GestorNotificaciones.SendMessage(cod, idEvento, idExtension, idZona, nombreZona, "zona-" + item.Zona.Id);
                            }
                        }
                    }
                }
            }
        }
    }
}