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

    public class ProcesoImagenes
    {
        private static string _proceso = "ProcesoMonitoreoImagenes";

        private static SqlTableDependency<Imagen> _dependency;

        private static readonly string _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        /// <summary>
        /// Funcion que engloba el proceso de atender Imagenes de la BD para extensiones.
        /// </summary>
        public static void ProcesoMonitoreoImagenes()
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
                dbAL.AgregarLogError("vacio", "servidor", "Emsys.ProcesoMonitoreoImagenes", "Program", 0, "_dependency_OnChanged", "Error al intentar capturar un Imagenes en la bd. Excepcion: " + e.Message, MensajesParaFE.LogErrorObserverDataBaseImagenes);
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
            dbAL.AgregarLogError("vacio", "servidor", "Emsys.ProcesoMonitoreoImagenes", "ProcesoMonitoreoImagenes", 0, "_dependency_OnChanged", "Error al intentar capturar un Imagenes en la bd. Excepcion: " + e.Message, MensajesParaFE.LogErrorObserverDataBaseeImagenesDependencyOnError);
        }

        /// <summary>
        /// Implementacion del metodo encargado de realizar la operativa de las notificaciones cuando se obtiene un cambvio en la bd.
        /// </summary>
        /// <param name="sender">no se usa</param>
        /// <param name="imagenEnBD">imagen generado desde la bd.</param>
        private static void DependencyOnChanged(object sender, TableDependency.EventArgs.RecordChangedEventArgs<Imagen> imagenEnBD)
        {
            try
            {
                if (imagenEnBD.ChangeType != ChangeType.None)
                {
                    Utils.Notifications.INotifications GestorNotificaciones = Utils.Notifications.FactoryNotifications.GetInstance();
                    switch (imagenEnBD.ChangeType)
                    {
                        // El caso no es util por que si se crea un Imagenes no tiene asignados recursos probablemente.
                        case ChangeType.Delete:
                            Console.WriteLine("ProcesoMonitoreoImagenes - Accion: Borro, Pk del Imagenes: " + imagenEnBD.Entity.Id);
                            break;
                        case ChangeType.Insert:
                            Console.WriteLine("ProcesoMonitoreoImagenes - Accion Insert, Pk del Imagenes: " + imagenEnBD.Entity.Id);
                            AtenderEvento(DataNotificacionesCodigos.AltaImagen, imagenEnBD, GestorNotificaciones);
                            break;
                        case ChangeType.Update:
                            Console.WriteLine("ProcesoMonitoreoImagenes - Accion update, Pk del Imagenes: " + imagenEnBD.Entity.Id);
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                IMetodos dbAL = new Metodos();
                dbAL.AgregarLogError("vacio", "servidor", "Emsys._dependency_OnChangedImagenes", "Program", 0, "_dependency_OnChanged", "Error al intentar capturar un evento en la bd. Excepcion: " + e.Message, MensajesParaFE.LogCapturarCambioEventoCod);
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
                dbAL.AgregarLog("vacio", "servidor", "Emsys.ObserverDataBase", "Imagenes", imagen.Entity.Id, "_dependency_OnChanged", "Se captura una modificacion de la base de datos para la tabla video. Se inicia la secuencia de envio de notificaciones.", MensajesParaFE.LogCapturarCambioEventoCod);
                var imagenEnBD = db.imagenes.Find(imagen.Entity.Id);
                if (imagenEnBD != null)
                {
                    if (imagenEnBD.ExtensionEvento != null)
                    {
                        int idEvento = imagenEnBD.ExtensionEvento.Evento.Id;
                        int idExtension = imagenEnBD.ExtensionEvento.Id;
                        int idZona = imagenEnBD.ExtensionEvento.Zona.Id;
                        string nombreZona = imagenEnBD.ExtensionEvento.Zona.Nombre;
                        // Para cada extension del evento modificado.
                        foreach (var item in imagenEnBD.ExtensionEvento.Evento.ExtensionesEvento)
                        {
                            // Para cada recurso de la extension.
                            foreach (var asig in item.AsignacionesRecursos)
                            {
                                // Si hay un usuario conectado con ese recurso.
                                if ((asig.ActualmenteAsignado == true) && (asig.Recurso.Estado == EstadoRecurso.NoDisponible))
                                {
                                    GestorNotificaciones.SendMessage(cod, idEvento, idExtension, idZona, nombreZona, "recurso-" + asig.Recurso.Id);
                                }
                            }

                            // Para la zona asociada a la extensen le envia una notificacion.
                            GestorNotificaciones.SendMessage(cod, idEvento, idExtension, idZona, nombreZona, "zona-" + item.Zona.Id);
                        }
                    }
                }
            }
        }
    }
}