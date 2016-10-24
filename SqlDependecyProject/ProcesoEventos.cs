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

    public class ProcesoEventos
    {
        private enum TablaMonitorar
        {
            Eventos,
            Extensiones
        }

        private static bool llamo = true;

        /// <summary>
        /// Funcion que engloba el proceso de atender eventos de la BD para Eventos.
        /// </summary>
        public static void ProcesoMonitoreoEventos()
        {
            try
            {
                Console.WriteLine("ProcesoMonitoreoEventos - Observo la BD:\n");
                Listener();
                while (true)
                {
                    Thread.Sleep(10000);
                }
            }
            catch (Exception e)
            {
                IMetodos dbAL = new Metodos();
                dbAL.AgregarLogError("vacio", "servidor", "Emsys.ObserverDataBase.ProcesoMonitoreoEventos", "Program", 0, "_dependency_OnChanged", "Error al intentar capturar un evento en la bd. Excepcion: " + e.Message, CodigosLog.LogCapturarCambioEventoCod);
                throw e;
            }
        }

        /// <summary>
        /// Implentacion con sql table dependency para noticiar los cambios en la bd.
        /// </summary>
        public static void Listener()
        {
            var mapper = new ModelToTableMapper<Evento>();
            mapper.AddMapping(model => model.NombreInformante, "NombreInformante");
            _dependency = new SqlTableDependency<Evento>(_connectionString, "Eventos", mapper);
            _dependency.OnChanged += _dependency_OnChanged;
            _dependency.OnError += _dependency_OnError;
            _dependency.Start();
        }

        // private static IList<Model.Evento> _stocks;
        private static readonly string _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        // "data source=DESKTOP-T27K22L\\SQLExpressLocal;initial catalog=Prototipo1;integrated security=True";
        private static SqlTableDependency<Evento> _dependency;

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
        /// <param name="evento">Evento generado desde la bd.</param>
        private static void _dependency_OnChanged(object sender, TableDependency.EventArgs.RecordChangedEventArgs<Evento> evento)
        {
            try
            {
                if (evento.ChangeType != ChangeType.None)
                {
                    Utils.Notifications.INotifications GestorNotificaciones = Utils.Notifications.FactoryNotifications.GetInstance();
                    switch (evento.ChangeType)
                    {
                        // el caso no es util por que si se crea un evento no tiene asignados recursos probablemente
                        case ChangeType.Delete:
                            Console.WriteLine("ProcesoMonitoreoEventos - Accion: Borro, Pk del evento: " + evento.Entity.NombreInformante);
                            AtenderEvento(DataNotificacionesCodigos.CierreEvento, evento, GestorNotificaciones);
                            break;
                        case ChangeType.Insert:
                            Console.WriteLine("ProcesoMonitoreoEventos - Accion Insert, Pk del evento: " + evento.Entity.Id);
                            AtenderEvento(DataNotificacionesCodigos.AltaEvento, evento, GestorNotificaciones);
                            break;
                        case ChangeType.Update:
                            Console.WriteLine("ProcesoMonitoreoEventos - Accion update, Pk del evento: " + evento.Entity.Id);
                            AtenderEvento(DataNotificacionesCodigos.ModificacionEvento, evento, GestorNotificaciones);
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                IMetodos dbAL = new Metodos();
                dbAL.AgregarLogError("vacio", "servidor", "Emsys.ObserverDataBase.ProcesoMonitoreoEventos", "Program", 0, "_dependency_OnChanged", "Error al intentar capturar un evento en la bd. Excepcion: " + e.Message, CodigosLog.LogCapturarCambioEventoCod);
                throw e;
            }
        }

        /// <summary>
        /// Metodo que se utiliza para enviar una notificaion a un evento.
        /// </summary>
        /// <param name="cod">Codigo que se desea notificar a la aplicacion dado el evento.</param>
        /// <param name="evento">Identificador del evento que fue modificado/alta/baja.</param>
        /// <param name="GestorNotificaciones">Instancia de INotification.</param>
        private static void AtenderEvento(string cod, TableDependency.EventArgs.RecordChangedEventArgs<Evento> evento, Utils.Notifications.INotifications GestorNotificaciones)
        {
            using (EmsysContext db = new EmsysContext())
            {
                IMetodos dbAL = new Metodos();
                dbAL.AgregarLog("vacio", "servidor", "Emsys.ObserverDataBase", "Evento", evento.Entity.Id, "_dependency_OnChanged", "Se captura una modificacion de la base de datos para la tabla Eventos. Se inicia la secuencia de envio de notificaciones.", CodigosLog.LogCapturarCambioEventoCod);
                var eventoBD = db.Evento.Find(evento.Entity.Id);
                if (eventoBD != null)
                {
                    foreach (var extension in eventoBD.ExtensionesEvento)
                    {
                        // Para cada recurso de las extensiones del evento genero una notificacion.
                        foreach (var recurso in extension.Recursos)
                        {
                            GestorNotificaciones.SendMessage(cod, evento.Entity.Id.ToString(), "recurso-" + recurso.Id.ToString());
                        }

                        // Para las zonas de las extensiones envio una notificacion.
                        GestorNotificaciones.SendMessage(cod, evento.Entity.Id.ToString(), "zona-" + extension.Zona.Id);
                    }
                }
            }
        }
    }
}