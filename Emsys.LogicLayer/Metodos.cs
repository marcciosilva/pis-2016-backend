using DataTypeObject;
using Emsys.DataAccesLayer.Core;
using Emsys.DataAccesLayer.Model;
using Emsys.LogicLayer.ApplicationExceptions;
using Emsys.LogicLayer.Utils;
using System;
using System.Collections.Generic;
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
                if ((user != null) && (user.Contraseña == Passwords.GetSHA1(password)))
                {
                    // Si el usuario ya tiene una sesion activa.
                    if (user.Token != null)
                    {
                        throw new SesionActivaException();
                    }

                    // Quita posibles logins previos.
                    user.Recurso.ToList().ForEach(x => x.Estado = EstadoRecurso.Disponible);
                    user.Zonas.Clear();
                    user.Recurso.Clear();
                    context.SaveChanges();

                    //// Comentado por compatibilidad con front end.
                    //// Agrega las zonas disponibles para el usuario mediante sus unidades ejecutoras.
                    //ICollection<DtoZona> zonas = new List<DtoZona>();
                    //foreach (Unidad_Ejecutora ue in user.Unidades_Ejecutoras)
                    //{
                    //    foreach (Zona z in ue.Zonas)
                    //    {
                    //        zonas.Add(z.getDto());
                    //    }
                    //}
                    //// Agrega los recursos disponibles para el usuario mediante sus grupos_recursos.
                    //ICollection<DtoRecurso> recursos = new List<DtoRecurso>();
                    //foreach (Grupo_Recurso gr in user.Grupos_Recursos)
                    //{
                    //    foreach (Recurso r in gr.Recursos)
                    //    {
                    //        if (r.Estado == EstadoRecurso.Disponible)
                    //            recursos.Add(r.getDto());
                    //    }
                    //}
                    //DtoRol rol = new DtoRol() { zonas = zonas, recursos = recursos };
                    string token = TokenGenerator.ObtenerToken();
                    user.Token = token;
                    user.FechaInicioSesion = DateTime.Now;
                    user.UltimoSignal = DateTime.Now;
                    context.SaveChanges();

                    //return new DtoAutenticacion(token, Mensajes.Correcto, rol);
                    return new DtoAutenticacion(token, Mensajes.Correcto);
                }

                throw new InvalidCredentialsException();
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
                if (user != null)
                {
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
                    foreach (Grupo_Recurso gr in user.Grupos_Recursos)
                    {
                        foreach (Recurso r in gr.Recursos)
                        {
                            if (r.Estado == EstadoRecurso.Disponible)
                            {
                                recursos.Add(DtoGetters.getDtoRecurso(r));
                            }       
                        }
                    }

                    DtoRol rol = new DtoRol() { zonas = zonas, recursos = recursos };
                    return rol;
                }

                throw new InvalidTokenException();
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
                if (user != null)
                {
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

                    if (!etiquetas.Any())
                    {
                        return true;
                    }

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
                }

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
                if (user != null)
                {
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

                throw new InvalidTokenException();
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
                if (user != null)
                {
                    List<DtoEvento> eventos = new List<DtoEvento>();
                    List<int> eventosAgregados = new List<int>();

                    // Si el usuario esta conectado como recurso.
                    if (user.Recurso.Count() > 0)
                    {
                        foreach (Extension_Evento ext in user.Recurso.FirstOrDefault().Extensiones_Eventos)
                        {
                            if (ext.Estado != EstadoExtension.Cerrado && !eventosAgregados.Contains(ext.Evento.Id))
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
                                if (ext.Estado != EstadoExtension.Cerrado && !eventosAgregados.Contains(ext.Evento.Id))
                                {
                                    eventos.Add(DtoGetters.getDtoEvento(ext.Evento));
                                    eventosAgregados.Add(ext.Evento.Id);
                                }
                            }
                        }
                    }

                    return eventos;
                }

                throw new InvalidTokenException();
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
                if (user != null)
                {
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

                throw new InvalidTokenException();
            }
        }

        public string getNombreUsuario(string token)
        {
            using (var context = new EmsysContext())
            {
                if (token == null)
                {
                    return string.Empty;
                }

                var user = context.Users.FirstOrDefault(u => u.Token == token);
                if (user != null)
                {
                    return user.NombreLogin;
                }

                return string.Empty;
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

                string ruta = string.Format("{0}Errores\\{1}", AppDomain.CurrentDomain.BaseDirectory, DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss").Replace(" ", "").Replace(":", "_") + ".txt");

                StreamWriter fs = File.CreateText(ruta);
                fs.Write("Mensaje: " + " error al registrar un log " + e.Message + "\n" +
                        "HelpLink: " + e.HelpLink + "\n" +
                        "Hresult: " + e.HResult + "\n" +
                        "Innerexception: " + e.InnerException + "\n" +
                        "Source: " + e.Source + "\n" +
                        "StackTrace: " + e.StackTrace + "\n" +
                        "TargetSite: " + e.TargetSite + "\n");
                fs.Close();
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

                string ruta = string.Format("{0}Errores\\{1}", AppDomain.CurrentDomain.BaseDirectory, DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss").Replace(" ", "").Replace(":", "_") + ".txt");

                StreamWriter fs = File.CreateText(ruta);
                fs.Write("Mensaje: " + " error al registrar un log " + e.Message + "\n" +
                        "HelpLink: " + e.HelpLink + "\n" +
                        "Hresult: " + e.HResult + "\n" +
                        "Innerexception: " + e.InnerException + "\n" +
                        "Source: " + e.Source + "\n" +
                        "StackTrace: " + e.StackTrace + "\n" +
                        "TargetSite: " + e.TargetSite + "\n");
                fs.Close();
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
                if (user != null)
                {
                    Evento evento = context.Evento.FirstOrDefault(e => e.Id == idEvento);
                    if (TieneAcceso.tieneVisionEvento(user, evento))
                    {
                        return DtoGetters.getDtoEvento(evento);
                    }

                    throw new EventoInvalidoException();
                }

                throw new InvalidTokenException();
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
                if (user != null)
                {
                    Extension_Evento ext = context.Extensiones_Evento.FirstOrDefault(e => e.Id == ubicacion.idExtension);
                    if (TieneAcceso.tieneAccesoExtension(user, ext))
                    {
                        GeoUbicacion geoU = new GeoUbicacion() { Usuario = user, FechaEnvio = DateTime.Now, Latitud = ubicacion.latitud, Longitud = ubicacion.longitud };
                        context.GeoUbicaciones.Add(geoU);
                        ext.GeoUbicaciones.Add(geoU);
                        context.SaveChanges();
                        return true;
                    }

                    return false;
                }

                throw new InvalidTokenException();
            }
        }

        public int agregarFileData(byte[] data, string extension)
        {
            using (var context = new EmsysContext())
            {
                string nombre;
                if (context.ApplicationFiles.Count() != 0)
                {
                    nombre = (context.ApplicationFiles.Max(u => u.Id) + 1).ToString() + extension;
                }
                else
                {
                    nombre = "1" + extension;
                }
                    
                var file = new ApplicationFile() { Nombre = nombre, FileData = data };
                context.ApplicationFiles.Add(file);
                context.SaveChanges();
                return file.Id;
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
                if (user != null)
                {
                    Imagen img = context.Imagenes.FirstOrDefault(i => i.Id == idAdjunto);
                    if (img != null)
                    {
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
                            Evento ev = context.Evento.FirstOrDefault(e => e == img.Evento);
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

                    throw new ImagenInvalidaException();
                }

                throw new InvalidTokenException();
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
                if (user != null)
                {
                    Video vid = context.Videos.FirstOrDefault(v => v.Id == idAdjunto);
                    if (vid != null)
                    {
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
                            Evento ev = context.Evento.FirstOrDefault(e => e == vid.Evento);
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

                    throw new VideoInvalidoException();
                }

                throw new InvalidTokenException();
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
                if (user != null)
                {
                    Audio aud = context.Audios.FirstOrDefault(a => a.Id == idAdjunto);
                    if (aud != null)
                    {
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
                            Evento ev = context.Evento.FirstOrDefault(e => e == aud.Evento);
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

                    throw new AudioInvalidoException();
                }

                throw new InvalidTokenException();
            }
        }

        public bool adjuntarImagen(string token, DtoImagen imagen)
        {
            using (var context = new EmsysContext())
            {
                if (token == null)
                {
                    throw new InvalidTokenException();
                }

                var user = context.Users.FirstOrDefault(u => u.Token == token);
                if (user != null)
                {
                    ApplicationFile file = context.ApplicationFiles.FirstOrDefault(f => f.Id == imagen.id_imagen);
                    if (file != null)
                    {
                        Extension_Evento ext = context.Extensiones_Evento.FirstOrDefault(e => e.Id == imagen.idExtension);
                        if (TieneAcceso.tieneAccesoExtension(user, ext))
                        {
                            Imagen img = new Imagen() { Usuario = user, FechaEnvio = DateTime.Now, ImagenData = file };
                            context.Imagenes.Add(img);
                            ext.Imagenes.Add(img);
                            context.SaveChanges();
                            return true;
                        }
                        else
                        {
                            context.ApplicationFiles.Remove(file);
                            context.SaveChanges();
                            return false;
                        }
                    }

                    return false;
                }

                throw new InvalidTokenException();
            }
        }

        public bool adjuntarVideo(string token, DtoVideo video)
        {
            using (var context = new EmsysContext())
            {
                if (token == null)
                {
                    throw new InvalidTokenException();
                }

                var user = context.Users.FirstOrDefault(u => u.Token == token);
                if (user != null)
                {
                    ApplicationFile file = context.ApplicationFiles.FirstOrDefault(f => f.Id == video.id_video);
                    if (file != null)
                    {
                        Extension_Evento ext = context.Extensiones_Evento.FirstOrDefault(e => e.Id == video.idExtension);
                        if (TieneAcceso.tieneAccesoExtension(user, ext))
                        {
                            Video vid = new Video() { Usuario = user, FechaEnvio = DateTime.Now, VideoData = file };
                            context.Videos.Add(vid);
                            ext.Videos.Add(vid);
                            context.SaveChanges();
                            return true;
                        }
                        else
                        {
                            context.ApplicationFiles.Remove(file);
                            context.SaveChanges();
                            return false;
                        }
                    }

                    return false;
                }

                throw new InvalidTokenException();
            }
        }

        public bool adjuntarAudio(string token, DtoAudio audio)
        {
            using (var context = new EmsysContext())
            {
                if (token == null)
                {
                    throw new InvalidTokenException();
                }

                var user = context.Users.FirstOrDefault(u => u.Token == token);
                if (user != null)
                {
                    ApplicationFile file = context.ApplicationFiles.FirstOrDefault(f => f.Id == audio.id_audio);
                    if (file != null)
                    {
                        Extension_Evento ext = context.Extensiones_Evento.FirstOrDefault(e => e.Id == audio.idExtension);
                        if (TieneAcceso.tieneAccesoExtension(user, ext))
                        {
                            Audio aud = new Audio() { Usuario = user, FechaEnvio = DateTime.Now, AudioData = file };
                            context.Audios.Add(aud);
                            ext.Audios.Add(aud);
                            context.SaveChanges();
                            return true;
                        }
                        else
                        {
                            context.ApplicationFiles.Remove(file);
                            context.SaveChanges();
                            return false;
                        }
                    }

                    return false;
                }

                throw new InvalidTokenException();
            }
        }

        /// <summary>
        /// Implementacion de ActualizarDescripcionRecurso, actualiza la descripcion segun los parametros.
        /// </summary>
        /// <param name="descParam">Data Type Object con la descripcion a agregar y la fecha.</param>
        /// <param name="token">Identificador unico del usuario.</param>
        /// <returns>Mensaje de exito.</returns>
        public Mensaje ActualizarDescripcionRecurso(DtoActualizarDescripcionParametro descParam, string token)
        {
            if (token == null)
            {
                throw new InvalidTokenException();
            }

            using (var context = new EmsysContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Token == token);
                if (user != null)
                {
                    var extension = context.Extensiones_Evento.Find(descParam.idExtension);
                    if (extension != null)
                    {
                        // Verifico que la extension sea del usuario.
                        bool extensionAsociadaUsuario = ExtensionAsociadaUsuario(descParam.idExtension, user);
                        if (!extensionAsociadaUsuario)
                        {
                            throw new InvalidExtensionForUserException();
                        }

                        foreach (var item in extension.AccionesRecursos)
                        {
                            item.AsignacionRecursoDescripcion.Add(new AsignacionRecursoDescripcion(descParam.dtoDescripcion.descripcion, descParam.dtoDescripcion.fecha));
                            context.SaveChanges();
                        }

                        return new Mensaje("Exito.");
                    }
                    else
                    {
                        throw new InvalidExtensionException();
                    }
                }

                throw new InvalidTokenException();
            }
        }

        /// <summary>
        /// Funcion interna que verifica si para algun recurso del usuario esta asociada a la extension.
        /// </summary>
        /// <param name="extensionId">Identificacion de la extension.</param>
        /// <param name="user">Usuario por el que se consulta.</param>
        /// <returns>Si la extension esta asociada al usuario o no.</returns>
        private static bool ExtensionAsociadaUsuario(int extensionId, Usuario user)
        {
            bool extensionAsociadaUsuario = false;
            foreach (var recursoUsuario in user.Grupos_Recursos)
            {
                var recursos = recursoUsuario.Recursos; 
                foreach (var item in recursos)
                {
                    var extensionUsuario = item.Extensiones_Eventos.FirstOrDefault(x => x.Id == extensionId);
                    if (extensionUsuario != null)
                    {
                        extensionAsociadaUsuario = true;
                    }
                }  
            }

            return extensionAsociadaUsuario;
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
                if (user != null)
                {
                    user.UltimoSignal = DateTime.Now;
                    context.SaveChanges();
                    return true;
                }

                throw new InvalidTokenException();
            }
        }
    }
}
