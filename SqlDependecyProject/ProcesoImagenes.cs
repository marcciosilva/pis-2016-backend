namespace SqlDependecyProject
{
    using System;
    using TableDependency.Mappers;
    using TableDependency.SqlClient;
    using TableDependency.Enums;
    using Emsys.DataAccesLayer.Model;
    using Emsys.DataAccesLayer.Core;
    using DataTypeObject;
    using Emsys.LogicLayer;
    using System.Threading;

    public class ProcesoImagenes
    {
        private static bool llamo = true;

        private static string proceso = "ProcesoMonitoreoImagenes";

        private static SqlTableDependency<Imagen> _dependency;

        private static readonly string _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        /// <summary>
        /// Funcion que engloba el proceso de atender Imagenes de la BD para extensiones.
        /// </summary>
        public static void ProcesoMonitoreoImagenes()
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
                dbAL.AgregarLogError("vacio", "servidor", "Emsys.ProcesoMonitoreoImagenes", "Program", 0, "_dependency_OnChanged", "Error al intentar capturar un Imagenes en la bd. Excepcion: " + e.Message, CodigosLog.LogCapturarCambioEventoCod);
                throw e;
            }
        }

        /// <summary>
        /// Implentacion con sql table dependency para noticiar los cambios en la bd.
        /// </summary>
        public static void Listener()
        {
            var mapper = new ModelToTableMapper<Imagen>();
            mapper.AddMapping(model => model.Id, "Id");
            _dependency = new SqlTableDependency<Imagen>(_connectionString, "Imagenes", mapper);
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
        /// <param name="imagenEnBD">imagen generado desde la bd.</param>
        private static void _dependency_OnChanged(object sender, TableDependency.EventArgs.RecordChangedEventArgs<Imagen> imagenEnBD)
        {
            try
            {
                if (imagenEnBD.ChangeType != ChangeType.None)
                {
                    Utils.Notifications.INotifications GestorNotificaciones = Utils.Notifications.FactoryNotifications.GetInstance();
                    switch (imagenEnBD.ChangeType)
                    {
                        // El caso no es util por que si se crea un Imagenes no tiene asignados recursos probablemte.
                        case ChangeType.Delete:
                            Console.WriteLine("ProcesoMonitoreoImagenes - Accion: Borro, Pk del Imagenes: " + imagenEnBD.Entity.Id);
                            AtenderEvento(DataNotificacionesCodigos.ModificacionEvento, imagenEnBD, GestorNotificaciones);
                            break;
                        case ChangeType.Insert:
                            Console.WriteLine("ProcesoMonitoreoImagenes - Accion Insert, Pk del Imagenes: " + imagenEnBD.Entity.Id);
                            AtenderEvento(DataNotificacionesCodigos.ModificacionEvento, imagenEnBD, GestorNotificaciones);
                            break;
                        case ChangeType.Update:
                            Console.WriteLine("ProcesoMonitoreoImagenes - Accion update, Pk del Imagenes: " + imagenEnBD.Entity.Id);
                            AtenderEvento(DataNotificacionesCodigos.ModificacionEvento, imagenEnBD, GestorNotificaciones);
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                IMetodos dbAL = new Metodos();
                dbAL.AgregarLogError("vacio", "servidor", "Emsys._dependency_OnChangedVideos", "Program", 0, "_dependency_OnChanged", "Error al intentar capturar un evento en la bd. Excepcion: " + e.Message, CodigosLog.LogCapturarCambioEventoCod);
                throw e;
            }
        }

        /// <summary>
        /// Metodo que se utiliza para enviar una notificaion a un Imagenes.
        /// </summary>
        /// <param name="cod">Codigo que se desea notificar a la aplicacion dado el imagen.</param>
        /// <param name="imagen">Identificador del Imagenes que fue modificado/alta/baja.</param>
        /// <param name="GestorNotificaciones">Instancia de INotification.</param>
        private static void AtenderEvento(string cod, TableDependency.EventArgs.RecordChangedEventArgs<Imagen> imagen, Utils.Notifications.INotifications GestorNotificaciones)
        {
            using (EmsysContext db = new EmsysContext())
            {
                IMetodos dbAL = new Metodos();
                dbAL.AgregarLog("vacio", "servidor", "Emsys.ObserverDataBase", "Imagenes", imagen.Entity.Id, "_dependency_OnChanged", "Se captura una modificacion de la base de datos para la tabla video. Se inicia la secuencia de envio de notificaciones.", CodigosLog.LogCapturarCambioEventoCod);
                var videoDEBD = db.Imagenes.Find(imagen.Entity.Id);
                if (videoDEBD != null)
                {                    
                    // Para los recursos asociados a la extension genero una notificacion.
                    foreach (var item in videoDEBD.Evento.ExtensionesEvento)
                    {
                        foreach (var recurso in item.Recursos)
                        {
                            GestorNotificaciones.SendMessage(cod, item.Id.ToString(), "recurso-" + item.Id);
                        }

                        // Para la zona asociada a la extensen le envia una notificacion.
                        GestorNotificaciones.SendMessage(cod, item.Id.ToString(), "zona-" + item.Zona.Id);
                    }
                }
            }
        }
    }
}