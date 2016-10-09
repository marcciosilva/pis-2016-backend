using System;
using TableDependency.Mappers;
using TableDependency.SqlClient;
using TableDependency.Enums;
using Emsys.DataAccesLayer.Model;
using Emsys.DataAccesLayer.Core;
using System.Linq;
using DataTypeObject;

namespace SqlDependecyProject
{
    public class Program
    {
        private static bool llamo = true;
        public static void Main(string[] args)
        {
            try
            {
                //para iniciar la bd si no esta creada
                EmsysContext db = new EmsysContext();
                var eventos = db.Evento.FirstOrDefault();

                //me quedo loopeando
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
                throw;
            }
        }

        public static void Listener()
        {
            var mapper = new ModelToTableMapper<Evento>();
            mapper.AddMapping(model => model.NombreInformante, "NombreInformante");
            _dependency = new SqlTableDependency<Evento>(_connectionString, "Evento", mapper);
            _dependency.OnChanged += _dependency_OnChanged;
            _dependency.OnError += _dependency_OnError;
            _dependency.Start();
        }

        // private static IList<Model.Evento> _stocks;
        private static readonly string _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        // "data source=DESKTOP-T27K22L\\SQLExpressLocal;initial catalog=Prototipo1;integrated security=True";
        private static SqlTableDependency<Evento> _dependency;

        private static void _dependency_OnError(object sender, TableDependency.EventArgs.ErrorEventArgs e)
        {
            throw e.Error;
        }
        private static void _dependency_OnChanged(object sender, TableDependency.EventArgs.RecordChangedEventArgs<Evento> evento)
        {
            try
            {
                if (evento.ChangeType != ChangeType.None)
                {
                    Utils.Notifications.INotifications GestorNotificaciones = Utils.Notifications.FactoryNotifications.GetInstance();
                    switch (evento.ChangeType)
                    {
                        case ChangeType.Delete:
                            Console.WriteLine("Accion: Borro, Pk del evento: " + evento.Entity.NombreInformante);
                            AtenderEvento(DataNotificacionesCodigos.CierreEvento, evento, GestorNotificaciones);
                            break;
                        case ChangeType.Insert:
                            Console.WriteLine("Accion Insert, Pk del evento: " + evento.Entity.NombreInformante);
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
                Emsys.Logs.Log.AgregarLogError("vacio", "servidor", "Emsys.ObserverDataBase", "Program", 0, "_dependency_OnChanged", "Error al intentar capturar un evento en la bd. ", Mensajes.LogCapturarCambioEventoCod);

            }
        }

        private static void AtenderEvento(string cod, TableDependency.EventArgs.RecordChangedEventArgs<Evento> evento, Utils.Notifications.INotifications GestorNotificaciones)
        {
            using (EmsysContext db = new EmsysContext())
            {

                Emsys.Logs.Log.AgregarLog("vacio", "servidor", "Emsys.ObserverDataBase", "Evento", evento.Entity.Id, "_dependency_OnChanged", "Se captura una modificacion de la base de datos para la tabla Eventos. Se inicia la secuencia de envio de notificaciones.", Mensajes.LogCapturarCambioEventoCod);
                var eventoBD = db.Evento.Find(evento.Entity.Id);
                if (eventoBD!=null) {
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
