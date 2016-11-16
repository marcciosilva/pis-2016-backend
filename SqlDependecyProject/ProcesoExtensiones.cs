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

    public class ProcesoExtensiones
    {
        private static SqlTableDependency<ExtensionEventoMapeado> _dependency;

        private static readonly string _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        /// <summary>
        /// Funcion que engloba el proceso de atender eventos de la BD para extensiones.
        /// </summary>
        public static void ProcesoMonitoreoExtensiones()
        {
            try
            {
                Console.WriteLine("ProcesoMonitoreoExtensiones- Observo la BD:\n");
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
                dbAL.AgregarLogError("vacio", "servidor", "Emsys.ProcesoMonitoreoExtensiones", "ProcesoMonitoreoExtensiones", 0, "_dependency_OnChanged", "Error al intentar capturar un evento en la bd. Excepcion: " + e.Message, MensajesParaFE.LogErrorObserverDataBaseExtensiones);
                _dependency.Stop();
                ProcesoMonitoreoExtensiones();
            }
        }

        public partial class ExtensionEventoMapeado
        {
            public int Id { get; set; }
            
            public string DescripcionDespachador { get; set; }

            public string DescripcionSupervisor { get; set; }

            public EstadoExtension Estado { get; set; }

            public int SegundaCategoria { get; set; }
        }

        /// <summary>
        /// Implentacion con sql table dependency para noticiar los cambios en la bd.
        /// </summary>
        public static void Listener()
        {
            var mapper = new ModelToTableMapper<ExtensionEventoMapeado>();
            mapper.AddMapping(model => model.Id, "Id");
            mapper.AddMapping(model => model.DescripcionDespachador, "DescripcionDespachador");
            mapper.AddMapping(model => model.DescripcionSupervisor, "DescripcionSupervisor");
            mapper.AddMapping(model => model.Estado, "Estado");
            mapper.AddMapping(model => model.SegundaCategoria, "SegundaCategoria_Id");

            _dependency = new SqlTableDependency<ExtensionEventoMapeado>(_connectionString, "Extensiones_Evento", mapper);
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
            dbAL.AgregarLogError("vacio", "servidor", "Emsys.ProcesoMonitoreoExtensiones", "ProcesoMonitoreoExtensiones", 0, "_dependency_OnChanged", "Error al intentar capturar un evento en la bd. Excepcion: " + e.Message, MensajesParaFE.LogErrorObserverDataBaseExtensionesDependencyOnError);
        }

        /// <summary>
        /// Implementacion del metodo encargado de realizar la operativa de las notificaciones cuando se obtiene un cambvio en la bd.
        /// </summary>
        /// <param name="sender">no se usa</param>
        /// <param name="eventoEnBD">Evento generado desde la bd.</param>
        private static void DependencyOnChanged(object sender, TableDependency.EventArgs.RecordChangedEventArgs<ExtensionEventoMapeado> eventoEnBD)
        {
            try
            {
                if (eventoEnBD.ChangeType != ChangeType.None)
                {
                    Utils.Notifications.INotifications GestorNotificaciones = Utils.Notifications.FactoryNotifications.GetInstance();
                    switch (eventoEnBD.ChangeType)
                    {
                        // El caso no es util por que si se crea un evento no tiene asignados recursos probablemte.
                        case ChangeType.Delete:
                            Console.WriteLine("ProcesoMonitoreoExtensiones - Accion: Borro, Pk del evento: " + eventoEnBD.Entity.Id);
                            break;
                        case ChangeType.Insert:
                            Console.WriteLine("ProcesoMonitoreoExtensiones - Accion Insert, Pk del evento: " + eventoEnBD.Entity.Id);
                            AtenderEvento(DataNotificacionesCodigos.AltaEvento, eventoEnBD, GestorNotificaciones);
                            break;
                        case ChangeType.Update:
                            Console.WriteLine("ProcesoMonitoreoExtensiones - Accion update, Pk del evento: " + eventoEnBD.Entity.Id);
                            AtenderEvento(DataNotificacionesCodigos.ModificacionEvento, eventoEnBD, GestorNotificaciones);
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                IMetodos dbAL = new Metodos();
                dbAL.AgregarLogError("vacio", "servidor", "Emsys.ProcesoMonitoreoExtensiones.ObserverDataBase", "Program", 0, "_dependency_OnChanged", "Error al intentar capturar un evento en la bd. Excepcion: " + e.Message, MensajesParaFE.LogCapturarCambioEventoCod);
            }
        }
        

        /// <summary>
        /// Metodo que se utiliza para enviar una notificaion a un evento.
        /// </summary>
        /// <param name="cod">Codigo que se desea notificar a la aplicacion dado el evento.</param>
        /// <param name="extension">Identificador del evento que fue modificado/alta/baja.</param>
        /// <param name="GestorNotificaciones">Instancia de INotification.</param>
        private static void AtenderEvento(string cod, TableDependency.EventArgs.RecordChangedEventArgs<ExtensionEventoMapeado> extension, Utils.Notifications.INotifications GestorNotificaciones)
        {
            using (EmsysContext db = new EmsysContext())
            {
                IMetodos dbAL = new Metodos();
                dbAL.AgregarLog("vacio", "servidor", "Emsys.ObserverDataBase", "Evento", extension.Entity.Id, "_dependency_OnChanged", "Se captura una modificacion de la base de datos para la tabla Eventos. Se inicia la secuencia de envio de notificaciones.", MensajesParaFE.LogCapturarCambioEventoCod);
                var extensionEnBD = db.ExtensionesEvento.Find(extension.Entity.Id);
                if (extensionEnBD != null)
                {
                    List<int> recursosNotificados = new List<int>();
                    int idEvento = extensionEnBD.Evento.Id;
                    int idExtension = extensionEnBD.Id;
                    int idZona = extensionEnBD.Zona.Id;
                    string nombreZona = extensionEnBD.Zona.Nombre;
                    // Si es un alta notifica solamente a los usuarios de la zona de la nueva extension y recursos asociados.
                    if (cod == DataNotificacionesCodigos.AltaEvento)
                    {                        
                        // Para cada recurso de la extension.
                        foreach (var asig in extensionEnBD.AsignacionesRecursos)
                        {
                            if ((asig.ActualmenteAsignado == true) && (asig.Recurso.Estado == EstadoRecurso.NoDisponible))
                            {
                                GestorNotificaciones.SendMessage(cod, idEvento, idExtension, idZona, nombreZona, "recurso-" + asig.Recurso.Id);
                            }
                        }
                        // Para la zona asociada a la extensen le envia una notificacion.
                        GestorNotificaciones.SendMessage(cod, idEvento, idExtension, idZona, nombreZona, "zona-" + extensionEnBD.Zona.Id);                        
                    }                    
                    else if (cod == DataNotificacionesCodigos.ModificacionEvento)
                    {
                        // Si hubo una modificacion, y el estado de la extension es "Cerrado" asume que la modificacion fue el cierre de la extension (no deberian ocurrir cambios en una extension cerrada).
                        if (extensionEnBD.Estado == EstadoExtension.Cerrado)
                        {
                            cod = DataNotificacionesCodigos.CierreEvento;
                        }
                        // Para cada extension del evento modificado.
                        foreach (var item in extensionEnBD.Evento.ExtensionesEvento)
                        {
                            // Para cada recurso de la extension.
                            foreach (var asig in item.AsignacionesRecursos)
                            {
                                if ((asig.ActualmenteAsignado == true) && (asig.Recurso.Estado == EstadoRecurso.NoDisponible) && (!recursosNotificados.Contains(asig.Recurso.Id)))
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