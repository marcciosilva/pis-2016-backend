
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

    public class ProcesoAudios
    {
        private static bool llamo = true;

        private static string proceso= "ProcesoMonitoreoAudios";

        /// <summary>
        /// Funcion que engloba el proceso de atender Audios de la BD para extensiones.
        /// </summary>
        public static void ProcesoMonitoreoAudios()
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
                dbAL.AgregarLogError("vacio", "servidor", "Emsys.ProcesoMonitoreoAudios", "Program", 0, "_dependency_OnChanged", "Error al intentar capturar un Audios en la bd. Excepcion: " + e.Message, CodigosLog.LogCapturarCambioEventoCod);
                throw e;
            }
        }

        private static SqlTableDependency<Audio> _dependency;
        private static readonly string _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;


        /// <summary>
        /// Implentacion con sql table dependency para noticiar los cambios en la bd.
        /// </summary>
        public static void Listener()
        {
            var mapper = new ModelToTableMapper<Audio>();
            mapper.AddMapping(model => model.Id, "Id");
            _dependency = new SqlTableDependency<Audio>(_connectionString, "Audios", mapper);
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
        /// <param name="AudioEnBD">Audios generado desde la bd.</param>
        private static void _dependency_OnChanged(object sender, TableDependency.EventArgs.RecordChangedEventArgs<Audio> AudioEnBD)
        {
            try
            {
                if (AudioEnBD.ChangeType != ChangeType.None)
                {
                    Utils.Notifications.INotifications GestorNotificaciones = Utils.Notifications.FactoryNotifications.GetInstance();
                    switch (AudioEnBD.ChangeType)
                    {
                        // el caso no es util por que si se crea un Audios no tiene asignados recursos probablemte
                        case ChangeType.Delete:
                            Console.WriteLine("ProcesoMonitoreoExtensiones - Accion: Borro, Pk del Audios: " + AudioEnBD.Entity.Id);
                            AtenderEvento(DataNotificacionesCodigos.ModificacionEvento, AudioEnBD, GestorNotificaciones);
                            break;
                        case ChangeType.Insert:
                            Console.WriteLine("ProcesoMonitoreoExtensiones - Accion Insert, Pk del Audios: " + AudioEnBD.Entity.Id);
                            AtenderEvento(DataNotificacionesCodigos.ModificacionEvento, AudioEnBD, GestorNotificaciones);
                            break;
                        case ChangeType.Update:
                            Console.WriteLine("ProcesoMonitoreoExtensiones - Accion update, Pk del Audios: " + AudioEnBD.Entity.Id);
                            AtenderEvento(DataNotificacionesCodigos.ModificacionEvento, AudioEnBD, GestorNotificaciones);
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
        /// Metodo que se utiliza para enviar una notificaion a un Audios.
        /// </summary>
        /// <param name="cod">Codigo que se desea notificar a la aplicacion dado el evento.</param>
        /// <param name="extension">Identificador del Audios que fue modificado/alta/baja.</param>
        /// <param name="GestorNotificaciones">Instancia de INotification.</param>
        private static void AtenderEvento(string cod, TableDependency.EventArgs.RecordChangedEventArgs<Audio> audio, Utils.Notifications.INotifications GestorNotificaciones)
        {
            using (EmsysContext db = new EmsysContext())
            {

                IMetodos dbAL = new Metodos();
                dbAL.AgregarLog("vacio", "servidor", "Emsys.ObserverDataBaseAudio", "Audio", audio.Entity.Id, "_dependency_OnChanged", "Se captura una modificacion de la base de datos para la tabla Audio. Se inicia la secuencia de envio de notificaciones.", CodigosLog.LogCapturarCambioEventoCod);
                var videoDEBD = db.Audios.Find(audio.Entity.Id);
                if (videoDEBD != null)
                {                    
                    //para los recursos asociados a la extension genero una notificacion
                    foreach (var item in videoDEBD.Evento.ExtensionesEvento)
                    {
                        foreach (var recurso in item.Recursos)
                        {
                            GestorNotificaciones.SendMessage(cod, item.Id.ToString(), "recurso-" + item.Id);
                        }
                        //para la zona asociada a la extensen le envia una notificacion
                        GestorNotificaciones.SendMessage(cod, item.Id.ToString(), "zona-" + item.Zona.Id);
                    }
                }
            }
        }
    }
}

