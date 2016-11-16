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
    using System.Web.Configuration;

    public class ProcesoEventos
    {
        private static readonly string _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private static SqlTableDependency<Evento> _dependency;

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
                dbAL.AgregarLogError("vacio", "servidor", "Emsys.ObserverDataBase.ProcesoMonitoreoEventos", "ProcesoMonitoreoEventos", 0, "_dependency_OnChanged", "Error al intentar capturar un evento en la bd. Excepcion: " + e.Message, MensajesParaFE.LogErrorObserverDataBaseEvento);
                _dependency.Stop();
                Thread UserAdminThread = new Thread(new ThreadStart(ProcesoMonitoreoEventos));
                UserAdminThread.IsBackground = true;
                UserAdminThread.Start();
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
            dbAL.AgregarLogError("vacio", "servidor", "Emsys.ObserverDataBase.ProcesoMonitoreoEventos", "ProcesoMonitoreoEventos", 0, "_dependency_OnChanged", "Error al intentar capturar un evento en la bd. Excepcion: " + e.Message, MensajesParaFE.LogErrorObserverDataBaseEventoDependencyOnError);
        }

        /// <summary>
        /// Implementacion del metodo encargado de realizar la operativa de las notificaciones cuando se obtiene un cambvio en la bd.
        /// </summary>
        /// <param name="sender">no se usa</param>
        /// <param name="evento">Evento generado desde la bd.</param>
        private static void DependencyOnChanged(object sender, TableDependency.EventArgs.RecordChangedEventArgs<Evento> evento)
        {
            try
            {
                if (evento.ChangeType != ChangeType.None)
                {
                    Utils.Notifications.INotifications GestorNotificaciones = Utils.Notifications.FactoryNotifications.GetInstance();
                    switch (evento.ChangeType)
                    {
                        case ChangeType.Delete:
                            Console.WriteLine("ProcesoMonitoreoEventos - Accion: Borro, Pk del evento: " + evento.Entity.NombreInformante);
                            break;
                        case ChangeType.Insert:
                            Console.WriteLine("ProcesoMonitoreoEventos - Accion Insert, Pk del evento: " + evento.Entity.Id);
                            AtenderEvento(DataNotificacionesCodigos.AltaEvento, evento, GestorNotificaciones);
                            break;
                        case ChangeType.Update:
                            Console.WriteLine("ProcesoMonitoreoEventos - Accion update, Pk del evento: " + evento.Entity.Id);
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                IMetodos dbAL = new Metodos();
                dbAL.AgregarLogError("vacio", "servidor", "Emsys.ObserverDataBase.ProcesoMonitoreoEventos", "Program", 0, "_dependency_OnChanged", "Error al intentar capturar un evento en la bd. Excepcion: " + e.Message, MensajesParaFE.LogCapturarCambioEventoCod);
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
                dbAL.AgregarLog("vacio", "servidor", "Emsys.ObserverDataBase", "Evento", evento.Entity.Id, "_dependency_OnChanged", "Se captura una modificacion de la base de datos para la tabla Eventos. Se inicia la secuencia de envio de notificaciones.", MensajesParaFE.LogCapturarCambioEventoCod);
                var eventoBD = db.Evento.Find(evento.Entity.Id);
                if (eventoBD != null)
                {
                    int idEvento = eventoBD.Id;                    
                    // Para cada extension del evento modificado.
                    foreach (var extension in eventoBD.ExtensionesEvento)
                    {
                        int idExtension = extension.Id;
                        int idZona = extension.Zona.Id;
                        string nombreZona = extension.Zona.Nombre;
                        // Para cada recurso de la extension.
                        foreach (var asig in extension.AsignacionesRecursos)
                        {
                            if ((asig.ActualmenteAsignado == true) && (asig.Recurso.Estado == EstadoRecurso.NoDisponible))
                            {
                                GestorNotificaciones.SendMessage(cod, idEvento, idExtension, idZona, nombreZona, "recurso-" + asig.Recurso.Id.ToString());
                            }
                        }
                        if (extension.Zona.Usuarios.Count != 0)
                        {
                            // Para las zonas de las extensiones envio una notificacion.
                            GestorNotificaciones.SendMessage(cod, idEvento, idExtension, idZona, nombreZona, "zona-" + extension.Zona.Id);
                        }
                    }
                }
            }
        }
    }
}