using Emsys.DataAccesLayer.Core;
using Emsys.DataAccesLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Notifications.Utils
{
    public class LogsManager
    {
        public static LogNotification AgregarLogNotification(string token, string terminal, string modulo, string Entidad, 
            int idEntidad, string accion, string detalles, int codigo,
            string topic, string codigoNotificacion, string pkEvento, string firebaseResponse, LogNotification logViejo = null)
        {
            try
            {
                using (EmsysContext context = new EmsysContext())
                {
                    string IdUsuario = string.Empty;
                    if (token != null)
                    {
                        var user = context.Usuarios.FirstOrDefault(u => u.Token == token);
                        if (user != null)
                        {
                            IdUsuario = user.NombreLogin;
                        }
                    }

                    LogNotification log = new LogNotification();
                    log.Usuario = IdUsuario;
                    log.TimeStamp = DateTime.Now;
                    log.Terminal = terminal;
                    log.Modulo = modulo;
                    log.Entidad = Entidad;
                    log.idEntidad = idEntidad;
                    log.Accion = accion;
                    log.Detalles = detalles;
                    log.Codigo = codigo;
                    log.EsError = false;
                    log.CodigoNotificacion = codigoNotificacion;
                    log.Topic = topic;
                    log.PKEventoAfectado = pkEvento;
                    log.responseFireBase = firebaseResponse;
                    log.LogNotificationPrevio = logViejo;
                    context.LogNotification.Add(log);
                    context.SaveChanges();
                    return log;
                }
            }
            catch (Exception e)
            {
                return null;
                Console.WriteLine(e.Message);
            }
        }

        public static LogNotification AgregarLogErrorNotification(string token, string terminal, string modulo,
            string Entidad, int idEntidad, string accion, string detalles, int codigo, string topicFinal,
            string codigoNotificacion, string pkEvento, string responseFirebase, LogNotification logViejo = null)
        {
            try
            {
                using (EmsysContext context = new EmsysContext())
                {
                    string IdUsuario = null;
                    if (token != null)
                    {
                        var user = context.Usuarios.FirstOrDefault(u => u.Token == token);
                        if (user != null)
                        {
                            IdUsuario = user.NombreLogin;
                        }
                    }

                    LogNotification log = new LogNotification();
                    log.Usuario = IdUsuario;
                    log.TimeStamp = DateTime.Now;
                    log.Terminal = terminal;
                    log.Modulo = modulo;
                    log.Entidad = Entidad;
                    log.idEntidad = idEntidad;
                    log.Accion = accion;
                    log.Detalles = detalles;
                    log.Codigo = codigo;
                    log.EsError = true;
                    log.CodigoNotificacion = codigoNotificacion;
                    log.Topic = topicFinal;
                    log.PKEventoAfectado = pkEvento;
                    log.responseFireBase = responseFirebase;
                    log.LogNotificationPrevio = logViejo;
                    context.LogNotification.Add(log);
                    context.SaveChanges();
                    return log;
                }
            }
            catch (Exception e)
            {
                return null;
                Console.WriteLine(e.Message);
            }
        }

        public static void AgregarLogNotificationDessuscripcionUsuario(string token, string terminal,
            string modulo, string Entidad, int idEntidad, string accion, string detalles, int codigo,
            string topicDessuscribir, string nombreUsuario, string firebaseResponse)
        {
            try
            {
                using (EmsysContext context = new EmsysContext())
                {
                    string IdUsuario = null;
                    if (token != null)
                    {
                        var user = context.Usuarios.FirstOrDefault(u => u.Token == token);
                        if (user != null)
                        {
                            IdUsuario = user.NombreLogin;
                        }
                    }

                    LogNotification log = new LogNotification();
                    log.Usuario = IdUsuario;
                    log.TimeStamp = DateTime.Now;
                    log.Terminal = terminal;
                    log.Modulo = modulo;
                    log.Entidad = Entidad;
                    log.idEntidad = idEntidad;
                    log.Accion = nombreUsuario;
                    log.Detalles = detalles;
                    log.Codigo = codigo;
                    log.EsError = false;
                    log.Topic = topicDessuscribir;
                    log.responseFireBase = firebaseResponse;
                    context.LogNotification.Add(log);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void AgregarLogNotificationDessuscripcionUsuarioError(string token, string terminal,
            string modulo, string Entidad, int idEntidad, string accion, string detalles, int codigo,
            string topicDessuscribir, string nombreUsuario, string firebaseResponse)
        {
            try
            {
                using (EmsysContext context = new EmsysContext())
                {
                    string IdUsuario = null;
                    if (token != null)
                    {
                        var user = context.Usuarios.FirstOrDefault(u => u.Token == token);
                        if (user != null)
                        {
                            IdUsuario = user.NombreLogin;
                        }
                    }
                    LogNotification log = new LogNotification();
                    log.Usuario = IdUsuario;
                    log.TimeStamp = DateTime.Now;
                    log.Terminal = terminal;
                    log.Modulo = modulo;
                    log.Entidad = Entidad;
                    log.idEntidad = idEntidad;
                    log.Accion = nombreUsuario;
                    log.Detalles = detalles;
                    log.Codigo = codigo;
                    log.EsError = false;
                    log.Topic = topicDessuscribir;
                    log.responseFireBase = firebaseResponse;
                    context.LogNotification.Add(log);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
