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

    public class ProcesoAsignacionRecurso
    {
        public partial class AsignacionRecursoMapeado
        {
            public int Id { get; set; }

            public bool ActualmenteAsignado { get; set; }            
        }


        private static string _proceso = "ProcesoMonitoreoAsignacionRecurso";

        private static SqlTableDependency<AsignacionRecursoMapeado> _dependency;

        private static readonly string _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        /// <summary>
        /// Funcion que engloba el proceso de atender AsignacionRecurso de la BD para extensiones.
        /// </summary>
        public static void ProcesoAsignacionRecursoMonitoreo()
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
                dbAL.AgregarLogError("vacio", "servidor", "Emsys.ProcesoAsignacionRecurso", "ProcesoAsignacionRecursoMonitoreo", 0, "_dependency_OnChanged", "Error al intentar capturar un Video en la bd. Excepcion: " + e.Message, MensajesParaFE.LogErrorObserverDataBaseAsignacionRecurso);
                _dependency.Stop();
                ProcesoAsignacionRecursoMonitoreo();
            }
        }

        /// <summary>
        /// Implentacion con sql table dependency para noticiar los cambios en la bd.
        /// </summary>
        public static void Listener()
        {
            var mapper = new ModelToTableMapper<AsignacionRecursoMapeado>();
            mapper.AddMapping(model => model.Id, "Id");
            mapper.AddMapping(model => model.ActualmenteAsignado, "ActualmenteAsignado");
            _dependency = new SqlTableDependency<AsignacionRecursoMapeado>(_connectionString, "AsignacionesRecursos", mapper);
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
            dbAL.AgregarLogError("vacio", "servidor", "Emsys.ProcesoAsignacionRecurso", "ProcesoAsignacionRecursoMonitoreo", 0, "_dependency_OnChanged", "Error al intentar capturar un Video en la bd. Excepcion: " + e.Message, MensajesParaFE.LogErrorObserverDataBaseAsignacionRecursoDependencyOnError);
        }

        /// <summary>
        /// Implementacion del metodo encargado de realizar la operativa de las notificaciones cuando se obtiene un cambvio en la bd.
        /// </summary>
        /// <param name="sender">no se usa</param>
        /// <param name="AsignacionRecursoDB"></param>
        private static void DependencyOnChanged(object sender, TableDependency.EventArgs.RecordChangedEventArgs<AsignacionRecursoMapeado> AsignacionRecursoDB)
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
                            break;
                        case ChangeType.Insert:
                            Console.WriteLine("ProcesoAsignacionRecursoMonitoreo - Accion Insert, Pk del evento: " + AsignacionRecursoDB.Entity.Id);
                            AtenderEvento(DataNotificacionesCodigos.AsignacionEvento, AsignacionRecursoDB, GestorNotificaciones);
                            break;
                        case ChangeType.Update:
                            Console.WriteLine("ProcesoAsignacionRecursoMonitoreo - Accion update, Pk del evento: " + AsignacionRecursoDB.Entity.Id);
                            AtenderEvento("cambio", AsignacionRecursoDB, GestorNotificaciones);
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
        /// <param name="asinacionRecurso">Identificador del Video que fue modificado/alta/baja.</param>
        /// <param name="GestorNotificaciones">Instancia de INotification.</param>
        private static void AtenderEvento(string cod, TableDependency.EventArgs.RecordChangedEventArgs<AsignacionRecursoMapeado> asinacionRecurso, Utils.Notifications.INotifications GestorNotificaciones)
        {
            using (EmsysContext db = new EmsysContext())
            {
                IMetodos dbAL = new Metodos();
                dbAL.AgregarLog("vacio", "servidor", "Emsys.ObserverDataBase", "Video", asinacionRecurso.Entity.Id, "_dependency_OnChanged", "Se captura una modificacion de la base de datos para la tabla video. Se inicia la secuencia de envio de notificaciones.", MensajesParaFE.LogCapturarCambioEventoCod);
                var asgnacionRecursoEnDB = db.AsignacionesRecursos.Find(asinacionRecurso.Entity.Id);
                if (asgnacionRecursoEnDB != null)
                {
                    int idEvento = asgnacionRecursoEnDB.Extension.Evento.Id;
                    int idExtension = asgnacionRecursoEnDB.Extension.Id;
                    int idZona = asgnacionRecursoEnDB.Extension.Zona.Id;
                    string nombreZona = asgnacionRecursoEnDB.Extension.Zona.Nombre;
                    if (cod == DataNotificacionesCodigos.AsignacionEvento)
                    {
                        // Notifico al usuario que ha sido asignado.
                        if ((asgnacionRecursoEnDB.ActualmenteAsignado == true) && (asgnacionRecursoEnDB.Recurso.Estado == EstadoRecurso.NoDisponible))
                        {
                            GestorNotificaciones.SendMessage(cod, idEvento, idExtension, idZona, nombreZona, "recurso-" + asgnacionRecursoEnDB.Recurso.Id);
                        }
                    }
                    // Si el evento fue de modificacion, se fija si el recurso fue asignado o retirado.
                    else if (cod == "cambio")
                    {
                        if ((asgnacionRecursoEnDB.ActualmenteAsignado == false) && (asgnacionRecursoEnDB.Recurso.Estado == EstadoRecurso.NoDisponible))
                        {
                            GestorNotificaciones.SendMessage(DataNotificacionesCodigos.RetiradoEvento, idEvento, idExtension, idZona, nombreZona, "recurso-" + asgnacionRecursoEnDB.Recurso.Id);
                        }
                        else if ((asgnacionRecursoEnDB.ActualmenteAsignado == true) && (asgnacionRecursoEnDB.Recurso.Estado == EstadoRecurso.NoDisponible))
                        {
                            GestorNotificaciones.SendMessage(DataNotificacionesCodigos.AsignacionEvento, idEvento, idExtension, idZona, nombreZona, "recurso-" + asgnacionRecursoEnDB.Recurso.Id);
                        }
                    }
                }
            }
        }
    }
}