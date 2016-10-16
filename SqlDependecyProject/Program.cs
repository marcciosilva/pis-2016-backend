using System;
using TableDependency.Mappers;
using TableDependency.SqlClient;
using TableDependency.Enums;
using Emsys.DataAccesLayer.Model;
using Emsys.DataAccesLayer.Core;
using System.Linq;
using DataTypeObject;
using Emsys.LogicLayer;

namespace SqlDependecyProject
{
    public class Program
    {
        private static bool llamo = true;

        /// <summary>
        /// Metodo principal para el proyecto ObserverDatabase encargado de manejar las notificaciones de los cambios de la bd.
        /// </summary>
        /// <param name="args">No utilizado.</param>
        public static void Main(string[] args)
        {
            try
            {
                // Para iniciar la bd si no esta creada.
                EmsysContext db = new EmsysContext();
                db.Evento.ToList();

                // Me quedo loopeando.
                while (true)
                {
                    if (llamo)
                    {
                        Console.WriteLine("Observo la BD:\n");
                        Listener();
                        llamo = false;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Ocurrio un error: ");
                Console.WriteLine(e.Message);
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
                        //// el caso no es util por que si se crea un evento no tiene asignados recursos probablemte
                        ////case ChangeType.Delete:
                        ////    Console.WriteLine("Accion: Borro, Pk del evento: " + evento.Entity.NombreInformante);
                        ////    AtenderEvento(DataNotificacionesCodigos.CierreEvento, evento, GestorNotificaciones);
                        ////    break;
                        case ChangeType.Insert:
                            Console.WriteLine("Accion Insert, Pk del evento: " + evento.Entity.Id);
                            AtenderEvento(DataNotificacionesCodigos.AltaEvento, evento, GestorNotificaciones);
                            break;
                        case ChangeType.Update:
                            Console.WriteLine("Accion update, Pk del evento: " + evento.Entity.Id);
                            AtenderEvento(DataNotificacionesCodigos.ModificacionEvento, evento, GestorNotificaciones);
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                IMetodos dbAL = new Metodos();
                dbAL.AgregarLogError("vacio", "servidor", "Emsys.ObserverDataBase", "Program", 0, "_dependency_OnChanged", "Error al intentar capturar un evento en la bd. Excepcion: " + e.Message, CodigosLog.LogCapturarCambioEventoCod);
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
                        foreach (var recurso in extension.Recursos)
                        {
                            GestorNotificaciones.SendMessage(cod, evento.Entity.Id.ToString(), recurso.Id.ToString());
                        }
                    }
                }
            }
        }
    }
}
