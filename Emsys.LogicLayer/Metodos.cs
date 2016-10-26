using DataTypeObject;
using Emsys.DataAccesLayer.Core;
using Emsys.DataAccesLayer.Model;
using Emsys.LogicLayer.ApplicationExceptions;
using Emsys.LogicLayer.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;

namespace Emsys.LogicLayer
{
    public class Metodos : IMetodos
    {
        public DtoAutenticacion autenticarUsuario(string userName, string password)
        {
            using (var context = new EmsysContext())
            {
                var user = context.Users.FirstOrDefault(u => u.NombreLogin == userName);
                // Si el usuario o contraseña son incorrectos.
                if ((user == null) || ((user.Contraseña != Passwords.GetSHA1(password))))
                {
                    throw new InvalidCredentialsException();
                }
                // Si el usuario ya tiene una sesion activa.
                if (user.Token != null)
                {
                    throw new SesionActivaException();
                }
               
                // Quita posibles logins previos.
                foreach (Recurso r in user.Recurso)
                {
                    r.Estado = EstadoRecurso.Disponible;
                }
                user.Zonas.Clear();
                user.Recurso.Clear();
                context.SaveChanges();
                
                // Retorna un token y registra el inicia de sesion.
                string token = TokenGenerator.ObtenerToken();
                user.Token = token;
                user.FechaInicioSesion = DateTime.Now;
                user.UltimoSignal = DateTime.Now;
                context.SaveChanges();                    
                return new DtoAutenticacion(token, MensajesParaFE.Correcto);
            }
        }

        // Metodo legado.
        public DtoRol getRolUsuario(string token)
        {
            using (var context = new EmsysContext())
            {
                if (token == null)
                {
                    throw new InvalidTokenException();
                }

                var user = context.Users.FirstOrDefault(u => u.Token == token);
                // Si no encuentra el token.
                if (user == null)
                {
                    throw new InvalidTokenException();
                }
                // Agrega las zonas disponibles para el usuario mediante sus unidades ejecutoras.
                ICollection<DtoZona> zonas = new List<DtoZona>();
                foreach (Unidad_Ejecutora ue in user.Unidades_Ejecutoras)
                {
                    foreach (Zona z in ue.Zonas)
                    {
                        zonas.Add(DtoGetters.getDtoZona(z));
                    }
                }

                // Agrega los recursos disponibles para el usuario mediante sus grupos_recursos.
                ICollection<DtoRecurso> recursos = new List<DtoRecurso>();
                List<int> recursosAgregados = new List<int>();
                foreach (Grupo_Recurso gr in user.Grupos_Recursos)
                {
                    foreach (Recurso r in gr.Recursos)
                    {
                        if ((r.Estado == EstadoRecurso.Disponible) && (!recursosAgregados.Contains(r.Id)))
                        {
                            recursos.Add(DtoGetters.getDtoRecurso(r));
                            recursosAgregados.Add(r.Id);
                        }       
                    }
                }

                DtoRol rol = new DtoRol() { zonas = zonas, recursos = recursos };
                return rol;            
            }
        }

        public bool autorizarUsuario(string token, string[] etiquetas)
        {
            using (var context = new EmsysContext())
            {
                if (token == null)
                {
                    throw new InvalidTokenException();
                }

                var user = context.Users.FirstOrDefault(u => u.Token == token);
                if (user == null)
                {
                    return false;
                }
                // Si el token ya expiro.
                if ((user.FechaInicioSesion.Value.Year < DateTime.Now.Year) ||
                    (user.FechaInicioSesion.Value.Month < DateTime.Now.Month) ||
                    (user.FechaInicioSesion.Value.Day < DateTime.Now.Day) ||
                    (user.FechaInicioSesion.Value.Hour < DateTime.Now.Hour - 8))
                {
                    // Libero recursos y expiro el token.
                    user.Recurso.ToList().ForEach(x => x.Estado = EstadoRecurso.Disponible);
                    user.Recurso.Clear();
                    user.Zonas.Clear();
                    user.Token = null;
                    user.FechaInicioSesion = null;
                    context.SaveChanges();
                    return false;
                }
                // Si no hay etiquetas.
                if (!etiquetas.Any())
                {
                    return true;
                }
                // Revisa que existe permiso para alguna etiqueta.
                foreach (var item in etiquetas)
                {
                    foreach (Rol ar in user.ApplicationRoles)
                    {
                        foreach (Permiso p in ar.Permisos)
                        {
                            if (item == p.Clave)
                            {
                                return true;
                            }
                        }
                    }
                }
                // Si no se encontre permiso para ninguna etiqueta.
                return false;
            }
        }

        public bool loguearUsuario(string token, DtoRol rol)
        {
            using (var context = new EmsysContext())
            {
                if (token == null)
                {
                    throw new InvalidTokenException();
                }
                var user = context.Users.FirstOrDefault(u => u.Token == token);
                if (user == null)
                {
                    throw new InvalidTokenException();
                }

                // Si el usuario se loguea por recurso.
                if ((rol.recursos.Count() == 1) && (rol.zonas.Count() == 0))
                {
                    Recurso recurso = null;

                    // Verifica que el recurso seleccionado sea seleccionable por el usuario.
                    foreach (Grupo_Recurso gr in user.Grupos_Recursos)
                    {
                        recurso = gr.Recursos.FirstOrDefault(r => r.Id == rol.recursos.FirstOrDefault().id);
                        if (recurso != null)
                        {
                            break;
                        }  
                    }
                    // Si es seleccionable y esta libre se lo asigna y lo marca como no disponible.
                    if ((recurso != null) && (recurso.Estado == EstadoRecurso.Disponible))
                    {
                        user.Recurso.Add(context.Recursos.Find(rol.recursos.FirstOrDefault().id));
                        context.Recursos.Find(rol.recursos.FirstOrDefault().id).Estado = EstadoRecurso.NoDisponible;
                        context.SaveChanges();
                        return true;
                    }

                    // Si el recurso no se encuentra disponible se lanza una excepcion.
                    else if ((recurso != null) && (recurso.Estado == EstadoRecurso.NoDisponible))
                    {
                        throw new RecursoNoDisponibleException();
                    }

                    // Si el recurso no existe o el usuario no tiene acceso a este se retorna false.
                    else
                    {
                        return false;
                    }
                }

                // Si el usuario se loguea por zonas.
                else if ((rol.recursos.Count() == 0) && (rol.zonas.Count() > 0))
                {
                    foreach (DtoZona z in rol.zonas)
                    {
                        // Verifica que el usuario pertenezca a la unidad ejecutora de cada zona.
                        Zona zona = context.Zonas.Find(z.id);
                        if ((zona != null) && (user.Unidades_Ejecutoras.Contains(zona.UnidadEjecutora)))
                        {
                            user.Zonas.Add(zona);
                        }

                        // Si existe una zona que no le corresponda no agrega ninguna zona.
                        else
                        {
                            return false;
                        }
                    }

                    context.SaveChanges();
                    return true;
                }

                // Si el usuario se loguea como visitante.
                else if ((rol.recursos.Count() == 0) && (rol.zonas.Count() == 0))
                {
                    return true;
                }

                // Si se recibe una combinacion de zonas y recursos o mas de un recurso retorna false.
                else
                {
                    return false;
                }

            }
        }
        
        public ICollection<DtoEvento> listarEventos(string token)
        {
            using (var context = new EmsysContext())
            {
                if (token == null)
                {
                    throw new InvalidTokenException();
                }

                var user = context.Users.FirstOrDefault(u => u.Token == token);
                if (user == null)
                {
                    throw new InvalidTokenException();
                }
                List<DtoEvento> eventos = new List<DtoEvento>();
                List<int> eventosAgregados = new List<int>();

                // Si el usuario esta conectado como recurso.
                if (user.Recurso.Count() > 0)
                {
                    foreach (Extension_Evento ext in user.Recurso.FirstOrDefault().Extensiones_Eventos)
                    {
                        if ((ext.Estado != EstadoExtension.Cerrado) && (!eventosAgregados.Contains(ext.Evento.Id)))
                        {
                            eventos.Add(DtoGetters.getDtoEvento(ext.Evento));
                            eventosAgregados.Add(ext.Evento.Id);
                        }
                    }
                }
                // Si el usuario esta conectado por zonas.
                else if (user.Zonas.Count() > 0)
                {
                    foreach (Zona z in user.Zonas)
                    {
                        foreach (Extension_Evento ext in z.Extensiones_Evento)
                        {
                            if ((ext.Estado != EstadoExtension.Cerrado) && (!eventosAgregados.Contains(ext.Evento.Id)))
                            {
                                eventos.Add(DtoGetters.getDtoEvento(ext.Evento));
                                eventosAgregados.Add(ext.Evento.Id);
                            }
                        }
                    }
                }
                return eventos;
            }
        }

        public bool cerrarSesion(string token)
        {
            using (var context = new EmsysContext())
            {
                if (token == null)
                {
                    throw new InvalidTokenException();
                }
                var user = context.Users.FirstOrDefault(u => u.Token == token);
                if (user == null)
                {
                    throw new InvalidTokenException();
                }
                // Retira las zonas y recursos asociadas al usuario.
                user.Zonas.Clear();
                foreach (Recurso r in user.Recurso)
                {
                    r.Estado = EstadoRecurso.Disponible;
                }
                user.Recurso.Clear();
                user.Token = null;
                user.FechaInicioSesion = null;
                user.UltimoSignal = null;
                context.SaveChanges();
                return true;

            }
        }

        public void AgregarLog(string token, string terminal, string modulo, string Entidad, int idEntidad, string accion, string detalles, int codigo)
        {
            try
            {
                using (EmsysContext context = new EmsysContext())
                {
                    string IdUsuario = string.Empty;
                    if (token != null)
                    {
                        var user = context.Users.FirstOrDefault(u => u.Token == token);
                        if (user != null)
                        {
                            IdUsuario = user.NombreLogin;
                        }    
                    }

                    Log log = new Log();
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
                    context.Logs.Add(log);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "Errores"))
                {
                    Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "Errores");
                }

                //string ruta = string.Format("{0}Errores\\{1}", AppDomain.CurrentDomain.BaseDirectory, DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss").Replace(" ", "").Replace(":", "_") + ".txt");

                //StreamWriter fs = File.CreateText(ruta);
                //fs.Write("Mensaje: " + " error al registrar un log " + e.Message + "\n" +
                //        "HelpLink: " + e.HelpLink + "\n" +
                //        "Hresult: " + e.HResult + "\n" +
                //        "Innerexception: " + e.InnerException + "\n" +
                //        "Source: " + e.Source + "\n" +
                //        "StackTrace: " + e.StackTrace + "\n" +
                //        "TargetSite: " + e.TargetSite + "\n");
                //fs.Close();
            }
        }

        public void AgregarLogError(string token, string terminal, string modulo, string Entidad, int idEntidad, string accion, string detalles, int codigo)
        {
            try
            {
                using (EmsysContext context = new EmsysContext())
                {
                    string IdUsuario = null;
                    if (token != null)
                    {
                        var user = context.Users.FirstOrDefault(u => u.Token == token);
                        if (user != null)
                        {
                            IdUsuario = user.NombreLogin;
                        }    
                    }

                    Log log = new Log();
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
                    context.Logs.Add(log);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.Message);
                if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "Errores"))
                {
                    Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "Errores");
                }

                //string ruta = string.Format("{0}Errores\\{1}", AppDomain.CurrentDomain.BaseDirectory, DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss").Replace(" ", "").Replace(":", "_") + ".txt");

                //StreamWriter fs = File.CreateText(ruta);
                //fs.Write("Mensaje: " + " error al registrar un log " + e.Message + "\n" +
                //        "HelpLink: " + e.HelpLink + "\n" +
                //        "Hresult: " + e.HResult + "\n" +
                //        "Innerexception: " + e.InnerException + "\n" +
                //        "Source: " + e.Source + "\n" +
                //        "StackTrace: " + e.StackTrace + "\n" +
                //        "TargetSite: " + e.TargetSite + "\n");
                //fs.Close();
            }
        }

        public DtoEvento verInfoEvento(string token, int idEvento)
        {
            using (var context = new EmsysContext())
            {
                if (token == null)
                {
                    throw new InvalidTokenException();
                }
                var user = context.Users.FirstOrDefault(u => u.Token == token);
                if (user == null)
                {
                    throw new InvalidTokenException();
                }
                Evento evento = context.Evento.FirstOrDefault(e => e.Id == idEvento);
                if (!TieneAcceso.tieneVisionEvento(user, evento))
                {
                    throw new EventoInvalidoException();
                }
                return DtoGetters.getDtoEvento(evento);                
            }
        }

        public bool adjuntarGeoUbicacion(string token, DtoGeoUbicacion ubicacion)
        {
            using (var context = new EmsysContext())
            {
                if (token == null)
                {
                    throw new InvalidTokenException();
                }
                var user = context.Users.FirstOrDefault(u => u.Token == token);
                if (user == null)
                {
                    throw new InvalidTokenException();
                }
                Extension_Evento ext = context.Extensiones_Evento.FirstOrDefault(e => e.Id == ubicacion.idExtension);
                if (!TieneAcceso.tieneAccesoExtension(user, ext))
                {                    
                    return false;
                }
                GeoUbicacion geoU = new GeoUbicacion() { Usuario = user, FechaEnvio = DateTime.Now, Latitud = ubicacion.latitud, Longitud = ubicacion.longitud };
                ext.GeoUbicaciones.Add(geoU);
                ext.TimeStamp = DateTime.Now;
                ext.Evento.TimeStamp = DateTime.Now;
                context.SaveChanges();
                return true;
            }
        }

        public DtoApplicationFile getImageData(string token, int idAdjunto)
        {
            using (var context = new EmsysContext())
            {
                if (token == null)
                {
                    throw new InvalidTokenException();
                }

                var user = context.Users.FirstOrDefault(u => u.Token == token);
                if (user == null)
                {
                    throw new InvalidTokenException();
                }
                Imagen img = context.Imagenes.FirstOrDefault(i => i.Id == idAdjunto);
                if (img == null)
                {
                    throw new ImagenInvalidaException();                    
                }
                // Si es la imagen de una extension.
                if (img.ExtensionEvento != null)
                {
                    Extension_Evento ext = context.Extensiones_Evento.FirstOrDefault(e => e.Id == img.ExtensionEvento.Id);
                    if (ext != null)
                    {
                        if (TieneAcceso.tieneVisionExtension(user, ext))
                        {
                            return DtoGetters.GetDtoApplicationfile(img.ImagenData);
                        }

                        throw new UsuarioNoAutorizadoException();
                    }
                }

                // Si es la imagen de un evento.
                else if (img.Evento != null)
                {
                    Evento ev = context.Evento.FirstOrDefault(e => e.Id == img.Evento.Id);
                    if (ev != null)
                    {
                        if (TieneAcceso.tieneVisionEvento(user, ev))
                        {
                            return DtoGetters.GetDtoApplicationfile(img.ImagenData);
                        }

                        throw new UsuarioNoAutorizadoException();
                    }
                }
                return null;
            }
        }

        public DtoApplicationFile getVideoData(string token, int idAdjunto)
        {
            using (var context = new EmsysContext())
            {
                if (token == null)
                {
                    throw new InvalidTokenException();
                }

                var user = context.Users.FirstOrDefault(u => u.Token == token);
                if (user == null)
                {
                    throw new InvalidTokenException();                    
                }
                Video vid = context.Videos.FirstOrDefault(v => v.Id == idAdjunto);
                if (vid == null)
                {
                    throw new VideoInvalidoException();                    
                }
                
                // Si el video es de una extension.
                if (vid.ExtensionEvento != null)
                {
                    Extension_Evento ext = context.Extensiones_Evento.FirstOrDefault(e => e.Id == vid.ExtensionEvento.Id);
                    if (ext != null)
                    {
                        if (TieneAcceso.tieneVisionExtension(user, ext))
                        {
                            return DtoGetters.GetDtoApplicationfile(vid.VideoData);
                        }
                        throw new UsuarioNoAutorizadoException();
                    }
                }

                // Si el video es de un evento.
                else if (vid.Evento != null)
                {
                    Evento ev = context.Evento.FirstOrDefault(e => e.Id == vid.Evento.Id);
                    if (ev != null)
                    {
                        if (TieneAcceso.tieneVisionEvento(user, ev))
                        {
                            return DtoGetters.GetDtoApplicationfile(vid.VideoData);
                        }
                        throw new UsuarioNoAutorizadoException();
                    }
                }
                return null;
            }
        }

        public DtoApplicationFile getAudioData(string token, int idAdjunto)
        {
            using (var context = new EmsysContext())
            {
                if (token == null)
                {
                    throw new InvalidTokenException();
                }
                var user = context.Users.FirstOrDefault(u => u.Token == token);
                if (user == null)
                {
                    throw new InvalidTokenException();                    
                }
                Audio aud = context.Audios.FirstOrDefault(a => a.Id == idAdjunto);
                if (aud == null)
                {
                    throw new AudioInvalidoException();                    
                }
                
                // Si el video es de una extension.
                if (aud.ExtensionEvento != null)
                {
                    Extension_Evento ext = context.Extensiones_Evento.FirstOrDefault(e => e.Id == aud.ExtensionEvento.Id);
                    if (ext != null)
                    {
                        if (TieneAcceso.tieneVisionExtension(user, ext))
                        {
                            return DtoGetters.GetDtoApplicationfile(aud.AudioData);
                        }
                        throw new UsuarioNoAutorizadoException();
                    }
                }

                // Si el video es de un evento.
                else if (aud.Evento != null)
                {
                    Evento ev = context.Evento.FirstOrDefault(e => e.Id == aud.Evento.Id);
                    if (ev != null)
                    {
                        if (TieneAcceso.tieneVisionEvento(user, ev))
                        {
                            return DtoGetters.GetDtoApplicationfile(aud.AudioData);
                        }
                        throw new UsuarioNoAutorizadoException();
                    }
                }
                return null;
            }
        }


        public bool adjuntarImagen(string token, byte[] imagenData, string extArchivo, int idExtension)
        {
            using (var context = new EmsysContext())
            {
                if (token == null)
                {
                    throw new InvalidTokenException();
                }
                var user = context.Users.FirstOrDefault(u => u.Token == token);
                if (user == null)
                {
                    throw new InvalidTokenException();                   
                }
                string nombre;
                Extension_Evento ext = context.Extensiones_Evento.FirstOrDefault(e => e.Id == idExtension);
                if (!TieneAcceso.tieneAccesoExtension(user, ext))
                {
                    return false;
                }

                // Si es el primer archivo.
                if (context.ApplicationFiles.Count() == 0)
                {
                    nombre = "1" + extArchivo;
                }
                else
                {
                    nombre = (context.ApplicationFiles.Max(u => u.Id) + 1).ToString() + extArchivo;
                }
                var file = new ApplicationFile() { Nombre = nombre, FileData = imagenData };                
                Imagen img = new Imagen() { Usuario = user, FechaEnvio = DateTime.Now, ImagenData = file };
                ext.Imagenes.Add(img);
                ext.TimeStamp = DateTime.Now;
                ext.Evento.TimeStamp = DateTime.Now;
                context.SaveChanges();
                return true;
            }
        }


        public bool adjuntarVideo(string token, byte[] videoData, string extArchivo, int idExtension)
        {
            using (var context = new EmsysContext())
            {
                if (token == null)
                {
                    throw new InvalidTokenException();
                }
                var user = context.Users.FirstOrDefault(u => u.Token == token);
                if (user == null)
                {
                    throw new InvalidTokenException();
                }
                string nombre;
                Extension_Evento ext = context.Extensiones_Evento.FirstOrDefault(e => e.Id == idExtension);
                if (!TieneAcceso.tieneAccesoExtension(user, ext))
                {
                    return false;
                }

                // Si es el primer archivo.
                if (context.ApplicationFiles.Count() == 0)
                {
                    nombre = "1" + extArchivo;
                }
                else
                {
                    nombre = (context.ApplicationFiles.Max(u => u.Id) + 1).ToString() + extArchivo;
                }
                var file = new ApplicationFile() { Nombre = nombre, FileData = videoData };
                Video vid = new Video() { Usuario = user, FechaEnvio = DateTime.Now, VideoData = file };
                ext.Videos.Add(vid);
                ext.TimeStamp = DateTime.Now;
                ext.Evento.TimeStamp = DateTime.Now;
                context.SaveChanges();
                return true;
            }
        }

        
        public bool adjuntarAudio(string token, byte[] audioData, string extArchivo, int idExtension)
        {
            using (var context = new EmsysContext())
            {
                if (token == null)
                {
                    throw new InvalidTokenException();
                }
                var user = context.Users.FirstOrDefault(u => u.Token == token);
                if (user == null)
                {
                    throw new InvalidTokenException();
                }
                string nombre;
                Extension_Evento ext = context.Extensiones_Evento.FirstOrDefault(e => e.Id == idExtension);
                if (!TieneAcceso.tieneAccesoExtension(user, ext))
                {
                    return false;
                }

                // Si es el primer archivo.
                if (context.ApplicationFiles.Count() == 0)
                {
                    nombre = "1" + extArchivo;
                }
                else
                {
                    nombre = (context.ApplicationFiles.Max(u => u.Id) + 1).ToString() + extArchivo;
                }
                var file = new ApplicationFile() { Nombre = nombre, FileData = audioData };
                Audio aud = new Audio() { Usuario = user, FechaEnvio = DateTime.Now, AudioData = file };
                ext.Audios.Add(aud);
                ext.TimeStamp = DateTime.Now;
                ext.Evento.TimeStamp = DateTime.Now;
                context.SaveChanges();
                return true;
            }
        }


        public bool ActualizarDescripcionRecurso(DtoActualizarDescripcionParametro descParam, string token)
        {
            using (var context = new EmsysContext())
            {
                if (token == null)
                {
                    throw new InvalidTokenException();
                }

                var user = context.Users.FirstOrDefault(u => u.Token == token);
                if ((user == null) || (user.Recurso.Count() != 1))
                {
                    throw new InvalidTokenException();                   
                }
                Extension_Evento ext = context.Extensiones_Evento.FirstOrDefault(e => e.Id == descParam.idExtension);
                Recurso rec = user.Recurso.FirstOrDefault();
                if ((ext == null) || (rec == null))
                {
                    return false;                    
                }
                if (!TieneAcceso.tieneAccesoExtension(user, ext))
                {
                    throw new UsuarioNoAutorizadoException();                    
                }
                foreach (var item in ext.AsignacionesRecursos)
                {
                    if (item.Recurso == rec)
                    {
                        item.AsignacionRecursoDescripcion.Add(new AsignacionRecursoDescripcion(descParam.descripcion, DateTime.Now));
                        ext.TimeStamp = DateTime.Now;
                        ext.Evento.TimeStamp = DateTime.Now;
                        context.SaveChanges();
                        return true;
                    }
                }
                return false;
            }
        }
        
        public bool keepMeAlive(string token)
        {
            using (var context = new EmsysContext())
            {
                if (token == null)
                {
                    throw new InvalidTokenException();
                }
                var user = context.Users.FirstOrDefault(u => u.Token == token);
                if (user == null)
                {
                    throw new InvalidTokenException();                    
                }
                user.UltimoSignal = DateTime.Now;
                context.SaveChanges();
                return true;
            }
        }
        
        public void desconectarAusentes(int maxTime)
        {
            using (var context = new EmsysContext())
            {
                DateTime ahora = DateTime.Now;
                foreach (Usuario user in context.Users)
                {
                    try
                    {
                        // Si el usuario cuenta con una sesion activa.
                        if ((user.Token != null) && (user.UltimoSignal != null))
                        {
                            // Si el usuario esta inactivo.
                            if ((ahora.Subtract(user.UltimoSignal.Value)).TotalMinutes > maxTime)
                            {
                                cerrarSesion(user.Token);
                                Console.WriteLine("Se desconecto al usuario <" + user.NombreLogin + ">");
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Ocurrio un error: " + e.ToString());
                    }
                }
            }
        }

        public bool reportarHoraArribo(string token, int idExtension)
        {
            using (var context = new EmsysContext())
            {
                if (token == null)
                {
                    throw new InvalidTokenException();
                }

                var user = context.Users.FirstOrDefault(u => u.Token == token);
                if ((user == null) || (user.Recurso.Count() != 1))
                {
                    throw new InvalidTokenException();                    
                }
                Extension_Evento ext = context.Extensiones_Evento.FirstOrDefault(e => e.Id == idExtension);
                Recurso rec = user.Recurso.FirstOrDefault();
                if ((ext == null) || (rec == null))
                {
                    return false;                    
                }
                foreach (var item in ext.AsignacionesRecursos)
                {
                    if (item.Recurso == rec)
                    {
                        item.HoraArribo = DateTime.Now;
                        context.SaveChanges();
                        return true;
                    }
                }
                return false;
            }
        }



    }
}
