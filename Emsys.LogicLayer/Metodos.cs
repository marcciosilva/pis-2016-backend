using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DataTypeObject;
using Emsys.DataAccesLayer.Core;
using Emsys.DataAccesLayer.Model;
using Emsys.LogicLayer.ApplicationExceptions;
using Emsys.LogicLayer.Utils;
using Utils.Notifications;

namespace Emsys.LogicLayer
{
    public class Metodos : IMetodos
    {
        public DtoAutenticacion autenticarUsuario(string userName, string password, string token)
        {
            using (var context = new EmsysContext())
            {
                var user = context.Usuarios.FirstOrDefault(u => u.NombreLogin == userName);

                // Si el usuario o contraseña son incorrectos.
                if ((user == null) || ((user.Contraseña != Passwords.GetSHA1(password))))
                {
                    throw new CredencialesInvalidasException();
                }

                // Si el usuario ya tiene una sesion activa y no corresponde al usuario que intenta loguearse.
                if ((user.Token != null) && (user.Token != token))
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
                string tokenNuevo = TokenGenerator.ObtenerToken();
                user.Token = tokenNuevo;
                user.FechaInicioSesion = DateTime.Now;
                user.UltimoSignal = DateTime.Now;
                context.SaveChanges();
                return new DtoAutenticacion(tokenNuevo, MensajesParaFE.Correcto);
            }
        }

        // Metodo legado.
        public DtoRol getRolUsuario(string token)
        {
            using (var context = new EmsysContext())
            {
                if (token == null)
                {
                    throw new TokenInvalidoException();
                }

                var user = context.Usuarios.FirstOrDefault(u => u.Token == token);

                // Si no encuentra el token.
                if (user == null)
                {
                    throw new TokenInvalidoException();
                }

                // Agrega las zonas disponibles para el usuario mediante sus unidades ejecutoras.
                ICollection<DtoZona> zonas = new List<DtoZona>();
                foreach (UnidadEjecutora ue in user.UnidadesEjecutoras)
                {
                    foreach (Zona z in ue.Zonas)
                    {
                        zonas.Add(DtoGetters.getDtoZona(z));
                    }
                }

                // Agrega los recursos disponibles para el usuario mediante sus grupos_recursos.
                ICollection<DtoRecurso> recursos = new List<DtoRecurso>();
                List<int> recursosAgregados = new List<int>();
                foreach (GrupoRecurso gr in user.GruposRecursos)
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
                    throw new TokenInvalidoException();
                }

                var user = context.Usuarios.FirstOrDefault(u => u.Token == token);
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

                // Si no se encontro permiso para ninguna etiqueta.
                return false;
            }
        }

        public bool loguearUsuario(string token, DtoRol rol)
        {
            using (var context = new EmsysContext())
            {
                if (token == null)
                {
                    throw new TokenInvalidoException();
                }

                var user = context.Usuarios.FirstOrDefault(u => u.Token == token);
                if (user == null)
                {
                    throw new TokenInvalidoException();
                }

                // Si el usuario se loguea por recurso.
                if ((rol.recursos.Count() == 1) && (rol.zonas.Count() == 0))
                {
                    Recurso recurso = null;

                    // Verifica que el recurso seleccionado sea seleccionable por el usuario.
                    foreach (GrupoRecurso gr in user.GruposRecursos)
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
                        if ((zona != null) && (user.UnidadesEjecutoras.Contains(zona.UnidadEjecutora)))
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
                    throw new TokenInvalidoException();
                }

                var user = context.Usuarios.FirstOrDefault(u => u.Token == token);
                if (user == null)
                {
                    throw new TokenInvalidoException();
                }

                List<DtoEvento> eventos = new List<DtoEvento>();
                List<int> eventosAgregados = new List<int>();

                // Si el usuario esta conectado como recurso.
                if (user.Recurso.Count() > 0)
                {
                    foreach (ExtensionEvento ext in user.Recurso.FirstOrDefault().ExtensionesEventos)
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
                        foreach (ExtensionEvento ext in z.ExtensionesEvento)
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
                    throw new TokenInvalidoException();
                }

                var user = context.Usuarios.FirstOrDefault(u => u.Token == token);
                if (user == null)
                {
                    throw new TokenInvalidoException();
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
                        var user = context.Usuarios.FirstOrDefault(u => u.Token == token);
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
                        var user = context.Usuarios.FirstOrDefault(u => u.Token == token);
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
                    log.EsError = true;
                    context.Logs.Add(log);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
   
        public DtoEvento verInfoEvento(string token, int idEvento)
        {
            using (var context = new EmsysContext())
            {
                if (token == null)
                {
                    throw new TokenInvalidoException();
                }

                var user = context.Usuarios.FirstOrDefault(u => u.Token == token);
                if (user == null)
                {
                    throw new TokenInvalidoException();
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
                    throw new TokenInvalidoException();
                }

                var user = context.Usuarios.FirstOrDefault(u => u.Token == token);
                if (user == null)
                {
                    throw new TokenInvalidoException();
                }

                ExtensionEvento ext = context.ExtensionesEvento.FirstOrDefault(e => e.Id == ubicacion.idExtension);
                if (ext == null)
                {
                    throw new ExtensionInvalidaException();
                }

                if ((!TieneAcceso.estaAsignadoExtension(user, ext)) && (!TieneAcceso.estaDespachandoExtension(user, ext)))
                {
                    throw new UsuarioNoAutorizadoException();
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
                    throw new TokenInvalidoException();
                }

                var user = context.Usuarios.FirstOrDefault(u => u.Token == token);
                if (user == null)
                {
                    throw new TokenInvalidoException();
                }

                Imagen img = context.Imagenes.FirstOrDefault(i => i.Id == idAdjunto);
                if (img == null)
                {
                    throw new ImagenInvalidaException();
                }

                // Si es la imagen de una extension.
                if (img.ExtensionEvento != null)
                {
                    ExtensionEvento ext = context.ExtensionesEvento.FirstOrDefault(e => e.Id == img.ExtensionEvento.Id);
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

        public DtoApplicationFile getImageThumbnail(string token, int idAdjunto)
        {
            using (var context = new EmsysContext())
            {
                if (token == null)
                {
                    throw new TokenInvalidoException();
                }

                var user = context.Usuarios.FirstOrDefault(u => u.Token == token);
                if (user == null)
                {
                    throw new TokenInvalidoException();
                }

                Imagen img = context.Imagenes.FirstOrDefault(i => i.Id == idAdjunto);
                if (img == null)
                {
                    throw new ImagenInvalidaException();
                }

                // Si es la imagen de una extension.
                if (img.ExtensionEvento != null)
                {
                    ExtensionEvento ext = context.ExtensionesEvento.FirstOrDefault(e => e.Id == img.ExtensionEvento.Id);
                    if (ext != null)
                    {
                        if (TieneAcceso.tieneVisionExtension(user, ext))
                        {
                            return DtoGetters.GetImageThumbnail(img.ImagenData);
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
                            return DtoGetters.GetImageThumbnail(img.ImagenData);
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
                    throw new TokenInvalidoException();
                }

                var user = context.Usuarios.FirstOrDefault(u => u.Token == token);
                if (user == null)
                {
                    throw new TokenInvalidoException();
                }

                Video vid = context.Videos.FirstOrDefault(v => v.Id == idAdjunto);
                if (vid == null)
                {
                    throw new VideoInvalidoException();
                }

                // Si el video es de una extension.
                if (vid.ExtensionEvento != null)
                {
                    ExtensionEvento ext = context.ExtensionesEvento.FirstOrDefault(e => e.Id == vid.ExtensionEvento.Id);
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
                    throw new TokenInvalidoException();
                }

                var user = context.Usuarios.FirstOrDefault(u => u.Token == token);
                if (user == null)
                {
                    throw new TokenInvalidoException();
                }

                Audio aud = context.Audios.FirstOrDefault(a => a.Id == idAdjunto);
                if (aud == null)
                {
                    throw new AudioInvalidoException();
                }

                // Si el video es de una extension.
                if (aud.ExtensionEvento != null)
                {
                    ExtensionEvento ext = context.ExtensionesEvento.FirstOrDefault(e => e.Id == aud.ExtensionEvento.Id);
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

        public bool adjuntarImagen(string token, DtoApplicationFile imgN)
        {
            using (var context = new EmsysContext())
            {
                if (token == null)
                {
                    throw new TokenInvalidoException();
                }

                var user = context.Usuarios.FirstOrDefault(u => u.Token == token);
                if (user == null)
                {
                    throw new TokenInvalidoException();
                }

                if (imgN == null)
                {
                    throw new ImagenInvalidaException();
                }

                ExtensionEvento ext = context.ExtensionesEvento.FirstOrDefault(e => e.Id == imgN.idExtension);
                if (ext == null)
                {
                    throw new ExtensionInvalidaException();
                }

                if ((!TieneAcceso.estaAsignadoExtension(user, ext)) && (!TieneAcceso.estaDespachandoExtension(user, ext)))
                {
                    throw new UsuarioNoAutorizadoException();
                }

                string extArchivo = Path.GetExtension(imgN.nombre);
                if ((extArchivo != ".jpg") && (extArchivo != ".png"))
                {
                    throw new FormatoInvalidoException();
                }

                string nombre;

                // Si es el primer archivo.
                if (context.ApplicationFiles.Count() == 0)
                {
                    nombre = "1" + extArchivo;
                }
                else
                {
                    nombre = (context.ApplicationFiles.Max(u => u.Id) + 1).ToString() + extArchivo;
                }

                var file = new ApplicationFile() { Nombre = nombre, FileData = imgN.fileData };
                Imagen img = new Imagen() { Usuario = user, FechaEnvio = DateTime.Now, ImagenData = file };
                ext.Imagenes.Add(img);
                ext.TimeStamp = DateTime.Now;
                ext.Evento.TimeStamp = DateTime.Now;
                context.SaveChanges();
                return true;
            }
        }


        public bool adjuntarVideo(string token, DtoApplicationFile vidN)
        {
            using (var context = new EmsysContext())
            {
                if (token == null)
                {
                    throw new TokenInvalidoException();
                }

                var user = context.Usuarios.FirstOrDefault(u => u.Token == token);
                if (user == null)
                {
                    throw new TokenInvalidoException();
                }

                if (vidN == null)
                {
                    throw new VideoInvalidoException();
                }

                ExtensionEvento ext = context.ExtensionesEvento.FirstOrDefault(e => e.Id == vidN.idExtension);
                if (ext == null)
                {
                    throw new ExtensionInvalidaException();
                }

                if ((!TieneAcceso.estaAsignadoExtension(user, ext)) && (!TieneAcceso.estaDespachandoExtension(user, ext)))
                {
                    throw new UsuarioNoAutorizadoException();
                }

                string extArchivo = Path.GetExtension(vidN.nombre);
                if ((extArchivo != ".mp4") && (extArchivo != ".avi"))
                {
                    throw new FormatoInvalidoException();
                }

                string nombre;

                // Si es el primer archivo.
                if (context.ApplicationFiles.Count() == 0)
                {
                    nombre = "1" + extArchivo;
                }
                else
                {
                    nombre = (context.ApplicationFiles.Max(u => u.Id) + 1).ToString() + extArchivo;
                }

                var file = new ApplicationFile() { Nombre = nombre, FileData = vidN.fileData };
                Video vid = new Video() { Usuario = user, FechaEnvio = DateTime.Now, VideoData = file };
                ext.Videos.Add(vid);
                ext.TimeStamp = DateTime.Now;
                ext.Evento.TimeStamp = DateTime.Now;
                context.SaveChanges();
                return true;
            }
        }


        public bool adjuntarAudio(string token, DtoApplicationFile audN)
        {
            using (var context = new EmsysContext())
            {
                if (token == null)
                {
                    throw new TokenInvalidoException();
                }

                var user = context.Usuarios.FirstOrDefault(u => u.Token == token);
                if (user == null)
                {
                    throw new TokenInvalidoException();
                }

                if (audN == null)
                {
                    throw new AudioInvalidoException();
                }

                ExtensionEvento ext = context.ExtensionesEvento.FirstOrDefault(e => e.Id == audN.idExtension);
                if (ext == null)
                {
                    throw new ExtensionInvalidaException();
                }

                if ((!TieneAcceso.estaAsignadoExtension(user, ext)) && (!TieneAcceso.estaDespachandoExtension(user, ext)))
                {
                    throw new UsuarioNoAutorizadoException();
                }

                string extArchivo = Path.GetExtension(audN.nombre);
                if ((extArchivo != ".mp3") && (extArchivo != ".wav"))
                {
                    throw new FormatoInvalidoException();
                }

                string nombre;

                // Si es el primer archivo.
                if (context.ApplicationFiles.Count() == 0)
                {
                    nombre = "1" + extArchivo;
                }
                else
                {
                    nombre = (context.ApplicationFiles.Max(u => u.Id) + 1).ToString() + extArchivo;
                }

                var file = new ApplicationFile() { Nombre = nombre, FileData = audN.fileData };
                Audio aud = new Audio() { Usuario = user, FechaEnvio = DateTime.Now, AudioData = file };
                ext.Audios.Add(aud);
                ext.TimeStamp = DateTime.Now;
                ext.Evento.TimeStamp = DateTime.Now;
                context.SaveChanges();
                return true;
            }
        }


        public bool ActualizarDescripcionRecurso(DtoActualizarDescripcion descParam, string token)
        {
            using (var context = new EmsysContext())
            {
                if (token == null)
                {
                    throw new TokenInvalidoException();
                }

                var user = context.Usuarios.FirstOrDefault(u => u.Token == token);
                if (user == null)
                {
                    throw new TokenInvalidoException();
                }

                ExtensionEvento ext = context.ExtensionesEvento.FirstOrDefault(e => e.Id == descParam.idExtension);
                Recurso rec = user.Recurso.FirstOrDefault();
                if ((ext == null) || (rec == null))
                {
                    return false;
                }

                if (!TieneAcceso.estaAsignadoExtension(user, ext))
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

        public bool ActualizarDescripcionRecursoOffline(DtoActualizarDescripcionOffline descParam)
        {
            using (var context = new EmsysContext())
            {

                var user = context.Usuarios.FirstOrDefault(u => u.NombreLogin == descParam.userData.username);
                if ((user == null) || ((user.Contraseña != Passwords.GetSHA1(descParam.userData.password))))
                {
                    throw new CredencialesInvalidasException();
                }

                ExtensionEvento ext = context.ExtensionesEvento.FirstOrDefault(e => e.Id == descParam.idExtension);
                DtoRecurso dtoR = descParam.userData.roles.recursos.FirstOrDefault();
                Recurso rec = context.Recursos.FirstOrDefault(r => r.Id == dtoR.id);
                if (ext == null)
                {
                    throw new ExtensionInvalidaException();
                }
                if (rec == null)
                {
                    throw new RecursoInvalidoException();
                }
                AsignacionRecurso asig = ext.AsignacionesRecursos.FirstOrDefault(a => a.Recurso.Id == rec.Id);
                if (asig == null)
                {
                    throw new ExtensionInvalidaException();
                }

                asig.AsignacionRecursoDescripcion.Add(new AsignacionRecursoDescripcion(descParam.descripcion, DateTime.Now, true));
                ext.TimeStamp = DateTime.Now;
                ext.Evento.TimeStamp = DateTime.Now;
                context.SaveChanges();
                return true;
            }
        }
       

        public bool keepMeAlive(string token)
        {
            using (var context = new EmsysContext())
            {
                if (token == null)
                {
                    throw new TokenInvalidoException();
                }

                var user = context.Usuarios.FirstOrDefault(u => u.Token == token);
                if (user == null)
                {
                    throw new TokenInvalidoException();
                }

                user.UltimoSignal = DateTime.Now;
                context.SaveChanges();
                return true;
            }
        }

        public bool SetRegistrationToken(string token, string tokenFirebase)
        {
            using (var context = new EmsysContext())
            {
                if (token == null)
                {
                    throw new TokenInvalidoException();
                }

                var user = context.Usuarios.FirstOrDefault(u => u.Token == token);
                if (user == null)
                {
                    throw new TokenInvalidoException();
                }

                user.RegistrationTokenFirebase = tokenFirebase;
                context.SaveChanges();
                return true;
            }
        }
        public void desconectarAusentes(int maxTime)
        {
            using (var context = new EmsysContext())
            {
                DateTime ahora = DateTime.Now;
                foreach (Usuario user in context.Usuarios)
                {
                    try
                    {
                        // Si el usuario cuenta con una sesion activa.
                        if ((user.Token != null) && (user.UltimoSignal != null))
                        {
                            // Si el usuario esta inactivo.
                            if ((ahora.Subtract(user.UltimoSignal.Value)).TotalMinutes > maxTime)
                            {
                                //doy de baja del servidor de firebase al usuario
                                unsuscribeTopicsFromFirebase(context, user);
                                //ahora lo doy de baja en el sistema
                                cerrarSesion(user.Token);
                                Console.WriteLine("Se desconecto al usuario <" + user.NombreLogin + ">");
                                string hora = user.UltimoSignal.Value.ToString();
                                AgregarLog(user.NombreLogin, "Servidor", "Emsys.LogicLayer", "Usuarios", user.Id, "Se desconecta al usuario indicado.", "Ultimo signal a las " + hora, MensajesParaFE.LogDesconectarUsuarioCod);
                                
                                
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

        private static void unsuscribeTopicsFromFirebase(EmsysContext context, Usuario user)
        {
            INotifications GestorNotificaciones = FactoryNotifications.GetInstance();
            foreach (var item in user.Recurso)
            {
                GestorNotificaciones.RemoveUserFromTopic(user.RegistrationTokenFirebase, "recurso-" + item.Id, user.Nombre);
            }
            foreach (var item in user.Zonas)
            {
                GestorNotificaciones.RemoveUserFromTopic(user.RegistrationTokenFirebase, "zona-" + item.Id, user.Nombre);
            }
           
        }

        public bool reportarHoraArribo(string token, int idExtension)
        {
            using (var context = new EmsysContext())
            {
                if (token == null)
                {
                    throw new TokenInvalidoException();
                }

                var user = context.Usuarios.FirstOrDefault(u => u.Token == token);
                if (user == null)
                {
                    throw new TokenInvalidoException();
                }

                ExtensionEvento ext = context.ExtensionesEvento.FirstOrDefault(e => e.Id == idExtension);
                Recurso rec = user.Recurso.FirstOrDefault();
                if ((ext == null) || (rec == null))
                {
                    return false;
                }

                if (!TieneAcceso.estaAsignadoExtension(user, ext))
                {
                    throw new ExtensionInvalidaException();
                }

                AsignacionRecurso asig = ext.AsignacionesRecursos.FirstOrDefault(a => a.Recurso.Id == rec.Id);
                if (asig == null)
                {
                    return false;
                }

                if (asig.HoraArribo != null)
                {
                    throw new ArriboPrevioException();
                }

                asig.HoraArribo = DateTime.Now;
                context.SaveChanges();
                return true;
            }
        }

        public DtoInfoCreacionEvento getInfoCreacionEvento(string token)
        {
            using (var context = new EmsysContext())
            {
                if (token == null)
                {
                    throw new TokenInvalidoException();
                }

                var user = context.Usuarios.FirstOrDefault(u => u.Token == token);
                if (user == null)
                {
                    throw new TokenInvalidoException();
                }

                DtoInfoCreacionEvento resp = new DtoInfoCreacionEvento();
                List<DtoZona> zonas = new List<DtoZona>();
                List<DtoCategoria> cats = new List<DtoCategoria>();
                List<DtoDepartamento> deps = new List<DtoDepartamento>();

                // Obtiene zonas y sectores.
                foreach (Zona z in context.Zonas)
                {
                    zonas.Add(DtoGetters.getDtoZonaCompleto(z));
                }

                // Obtiene categorias.
                foreach (Categoria c in context.Categorias)
                {
                    cats.Add(DtoGetters.getDtoCategoria(c));
                }

                // Obtiene departamentos.
                foreach (Departamento d in context.Departamentos)
                {
                    deps.Add(DtoGetters.getDtoDepartamento(d));
                }

                resp.categorias = cats;
                resp.zonasSectores = zonas;
                resp.departamentos = deps;
                return resp;
            }
        }

        public bool crearEvento(string token, DtoEvento ev)
        {
            using (var context = new EmsysContext())
            {
                if (token == null)
                {
                    throw new TokenInvalidoException();
                }

                var user = context.Usuarios.FirstOrDefault(u => u.Token == token);
                if (user == null)
                {
                    throw new TokenInvalidoException();
                }

                if (ev == null)
                {
                    throw new ArgumentoInvalidoException();
                }

                // Si no se eligieron zonas.
                if (ev.idZonas.Count() <= 0)
                {
                    throw new SeleccionZonasInvalidaException();
                }

                // Si la categoria es invalida.
                var cat = context.Categorias.FirstOrDefault(c => c.Id == ev.categoria.id);
                if (cat == null)
                {
                    throw new ArgumentoInvalidoException();
                }

                // Si el departamento es invalido.
                Departamento dep = null;
                if (ev.idDepartamento != 0)
                {
                    dep = context.Departamentos.FirstOrDefault(d => d.Id == ev.idDepartamento);
                    if (dep == null)
                    {
                        throw new ArgumentoInvalidoException();
                    }
                }

                List<Zona> zonas = new List<Zona>();
                foreach (int z in ev.idZonas)
                {
                    Zona zona = context.Zonas.FirstOrDefault(zid => zid.Id == z);
                    if (zona == null)
                    {
                        throw new ArgumentoInvalidoException();
                    }
                    else
                    {
                        zonas.Add(zona);
                    }
                }

                // Si el sector es invalido.                
                Sector sect = context.Sectores.FirstOrDefault(s => s.Id == ev.idSector);
                if (sect == null)
                {
                    throw new ArgumentoInvalidoException();
                }

                // Si el sector no se corresponde a alguna de las zonas.
                bool sectorCorrecto = false;
                foreach (Zona zAgregada in zonas)
                {
                    foreach (Sector sZona in zAgregada.Sectores)
                    {
                        if (sect.Id == sZona.Id)
                        {
                            sectorCorrecto = true;
                            break;
                        }
                    }
                }

                if (!sectorCorrecto)
                {
                    throw new ArgumentoInvalidoException();
                }

                EstadoEvento est;
                if (ev.estado == "creado")
                {
                    est = EstadoEvento.Creado;
                }
                else
                {
                    est = EstadoEvento.Enviado;
                }

                // Crea el evento.
                Evento nuevoEvento = new Evento()
                {
                    NombreInformante = ev.informante,
                    TelefonoEvento = ev.telefono,
                    Categoria = cat,
                    Estado = est,
                    TimeStamp = DateTime.Now,
                    Usuario = user,
                    FechaCreacion = DateTime.Now,
                    Calle = ev.calle,
                    Esquina = ev.esquina,
                    Numero = ev.numero,
                    Departamento = dep,
                    Sector = sect,
                    Latitud = ev.latitud,
                    Longitud = ev.longitud,
                    Descripcion = ev.descripcion,
                    EnProceso = ev.enProceso,
                    ExtensionesEvento = new List<ExtensionEvento>()
                };

                // Agrego extensiones.
                foreach (Zona zEveto in zonas)
                {
                    nuevoEvento.ExtensionesEvento.Add(new ExtensionEvento()
                    {
                        Zona = zEveto,
                        Evento = nuevoEvento,
                        Estado = EstadoExtension.FaltaDespachar,
                        TimeStamp = DateTime.Now
                    });
                }

                context.Evento.Add(nuevoEvento);
                context.SaveChanges();
                return true;
            }
        }

        public bool tomarExtension(string token, int idExtension)
        {
            using (var context = new EmsysContext())
            {
                if (token == null)
                {
                    throw new TokenInvalidoException();
                }

                var user = context.Usuarios.FirstOrDefault(u => u.Token == token);
                if (user == null)
                {
                    throw new TokenInvalidoException();
                }

                ExtensionEvento ext = context.ExtensionesEvento.FirstOrDefault(e => e.Id == idExtension);
                if ((ext == null) || (ext.Estado != EstadoExtension.FaltaDespachar))
                {
                    throw new ExtensionInvalidaException();
                }

                // Si esta logueado como recurso o no tiene vision sobre la extension.
                if ((user.Recurso.Count() > 0) || (!TieneAcceso.tieneVisionExtension(user, ext)))
                {
                    throw new UsuarioNoAutorizadoException();
                }

                ext.Estado = EstadoExtension.Despachado;
                ext.Despachador = user;
                user.Despachando.Add(ext);
                context.SaveChanges();
                return true;
            }
        }

        public bool liberarExtension(string token, int idExtension)
        {
            using (var context = new EmsysContext())
            {
                if (token == null)
                {
                    throw new TokenInvalidoException();
                }

                var user = context.Usuarios.FirstOrDefault(u => u.Token == token);
                if (user == null)
                {
                    throw new TokenInvalidoException();
                }

                ExtensionEvento ext = user.Despachando.FirstOrDefault(e => e.Id == idExtension);
                if (ext == null)
                {
                    throw new ExtensionInvalidaException();
                }

                if ((ext.Estado == EstadoExtension.Despachado) && (ext.Despachador.Id == user.Id))
                {
                    ext.Estado = EstadoExtension.FaltaDespachar;
                    ext.Despachador = null;
                    user.Despachando.Remove(ext);
                    context.SaveChanges();
                    return true;
                }

                return false;
            }
        }

        public DtoRecursosExtension getRecursosExtension(string token, int idExtension)
        {
            using (var context = new EmsysContext())
            {
                if (token == null)
                {
                    throw new TokenInvalidoException();
                }

                var user = context.Usuarios.FirstOrDefault(u => u.Token == token);
                if (user == null)
                {
                    throw new TokenInvalidoException();
                }

                ExtensionEvento ext = context.ExtensionesEvento.FirstOrDefault(e => e.Id == idExtension);
                if ((ext == null) || (!TieneAcceso.estaDespachandoExtension(user, ext)))
                {
                    throw new ExtensionInvalidaException();
                }

                ICollection<DtoRecurso> asignados = new List<DtoRecurso>();
                ICollection<DtoRecurso> noAsignados = new List<DtoRecurso>();

                foreach (Recurso r in context.Recursos)
                {
                    if (r.ExtensionesEventos.Contains(ext))
                    {
                        asignados.Add(DtoGetters.getDtoRecurso(r));
                    }
                    else if (r.EstadoAsignacion == EstadoAsignacionRecurso.Libre)
                    {
                        noAsignados.Add(DtoGetters.getDtoRecurso(r));
                    }
                }

                DtoRecursosExtension resp = new DtoRecursosExtension()
                {
                    idExtension = idExtension,
                    recursosAsignados = asignados,
                    recursosNoAsignados = noAsignados
                };
                return resp;
            }
        }

        public bool gestionarRecursos(string token, DtoRecursosExtension recursos)
        {
            using (var context = new EmsysContext())
            {
                if (token == null)
                {
                    throw new TokenInvalidoException();
                }

                var user = context.Usuarios.FirstOrDefault(u => u.Token == token);
                if (user == null)
                {
                    throw new TokenInvalidoException();
                }

                if (recursos == null)
                {
                    throw new ArgumentoInvalidoException();
                }

                ExtensionEvento ext = context.ExtensionesEvento.FirstOrDefault(e => e.Id == recursos.idExtension);
                if ((ext == null) || (!TieneAcceso.estaDespachandoExtension(user, ext)))
                {
                    throw new ExtensionInvalidaException();
                }

                // Agrego los recursos asignados.
                foreach (DtoRecurso r in recursos.recursosAsignados)
                {
                    Recurso rec = context.Recursos.FirstOrDefault(rb => rb.Id == r.id);
                    if ((rec == null) || (rec.EstadoAsignacion == EstadoAsignacionRecurso.Operativo) || (ext.Recursos.Contains(rec)))
                    {
                        throw new ArgumentoInvalidoException();
                    }

                    ext.Recursos.Add(rec);
                    AsignacionRecurso ar = ext.AsignacionesRecursos.FirstOrDefault(a => a.Recurso.Id == rec.Id);

                    // Si el recurso no habia estado asignado a la extension crea una nueva AsignacionRecurso.
                    if (ar == null)
                    {
                        ext.AsignacionesRecursos.Add(new AsignacionRecurso() { Recurso = rec, Extension = ext, ActualmenteAsignado = true });
                    }

                    // Si ya estuvo asignado.
                    else
                    {
                        ar.ActualmenteAsignado = true;
                        ar.HoraArribo = null;
                    }
                }

                // Quito los recursos a retirar.
                foreach (DtoRecurso r in recursos.recursosNoAsignados)
                {
                    Recurso rec = context.Recursos.FirstOrDefault(rb => rb.Id == r.id);
                    if ((rec == null) || (!ext.Recursos.Contains(rec)))
                    {
                        throw new ArgumentoInvalidoException();
                    }

                    ext.Recursos.Remove(rec);
                    AsignacionRecurso ar = ext.AsignacionesRecursos.FirstOrDefault(a => a.Recurso.Id == rec.Id);
                    if (ar != null)
                    {
                        ar.ActualmenteAsignado = false;
                        ar.HoraArribo = null;
                    }
                }

                ext.TimeStamp = DateTime.Now;
                ext.Evento.TimeStamp = DateTime.Now;
                context.SaveChanges();
                return true;
            }
        }

        public bool actualizarSegundaCategoria(string token, int idExtension, int idCategoria)
        {
            using (var context = new EmsysContext())
            {
                if (token == null)
                {
                    throw new TokenInvalidoException();
                }

                var user = context.Usuarios.FirstOrDefault(u => u.Token == token);
                if (user == null)
                {
                    throw new TokenInvalidoException();
                }

                var cat = context.Categorias.FirstOrDefault(c => c.Id == idCategoria);

                // En caso de usar id -1 se asume que se desea eliminar la segunda categoria.
                if ((cat == null) && (idCategoria != -1))
                {
                    throw new CategoriaInvalidaException();
                }

                ExtensionEvento ext = context.ExtensionesEvento.FirstOrDefault(e => e.Id == idExtension);
                if ((ext == null) || (!TieneAcceso.estaDespachandoExtension(user, ext)))
                {
                    throw new ExtensionInvalidaException();
                }

                if (ext.SegundaCategoria != null)
                {
                    ext.SegundaCategoria.ExtensionesEvento.Remove(ext);
                }

                ext.SegundaCategoria = cat;
                ext.TimeStamp = DateTime.Now;
                ext.Evento.TimeStamp = DateTime.Now;
                context.SaveChanges();
                return true;
            }
        }

        public ICollection<DtoZona> getZonasLibresEvento(string token, int idExtension)
        {
            using (var context = new EmsysContext())
            {
                if (token == null)
                {
                    throw new TokenInvalidoException();
                }

                var user = context.Usuarios.FirstOrDefault(u => u.Token == token);
                if (user == null)
                {
                    throw new TokenInvalidoException();
                }

                ExtensionEvento ext = context.ExtensionesEvento.FirstOrDefault(e => e.Id == idExtension);
                if ((ext == null) || (!TieneAcceso.estaDespachandoExtension(user, ext)))
                {
                    throw new ExtensionInvalidaException();
                }

                List<DtoZona> zonas = new List<DtoZona>();
                List<int> zonasNoDisponibles = new List<int>();
                foreach (ExtensionEvento e in ext.Evento.ExtensionesEvento)
                {
                    zonasNoDisponibles.Add(e.Id);
                }

                foreach (Zona z in context.Zonas)
                {
                    if (!zonasNoDisponibles.Contains(z.Id))
                    {
                        zonas.Add(DtoGetters.getDtoZona(z));
                    }
                }

                return zonas;
            }
        }

        public bool abrirExtension(string token, int idExtension, int idZona)
        {
            using (var context = new EmsysContext())
            {
                if (token == null)
                {
                    throw new TokenInvalidoException();
                }

                var user = context.Usuarios.FirstOrDefault(u => u.Token == token);
                if (user == null)
                {
                    throw new TokenInvalidoException();
                }

                ExtensionEvento ext = context.ExtensionesEvento.FirstOrDefault(e => e.Id == idExtension);
                if ((ext == null) || (!TieneAcceso.estaDespachandoExtension(user, ext)))
                {
                    throw new ExtensionInvalidaException();
                }

                Zona zExt = context.Zonas.FirstOrDefault(z => z.Id == idZona);

                // Si la zona es invalida o evento cuenta con una extension para esa zona.
                if ((zExt == null) || (zExt.ExtensionesEvento.Contains(ext)))
                {
                    throw new ZonaInvalidaException();
                }

                ext.Evento.ExtensionesEvento.Add(new ExtensionEvento()
                {
                    Evento = ext.Evento,
                    Zona = zExt,
                    Estado = EstadoExtension.FaltaDespachar,
                    TimeStamp = DateTime.Now
                });
                ext.Evento.TimeStamp = DateTime.Now;
                context.SaveChanges();
                return true;
            }
        }

        public bool cerrarExtension(string token, int idExtension)
        {
            using (var context = new EmsysContext())
            {
                if (token == null)
                {
                    throw new TokenInvalidoException();
                }

                var user = context.Usuarios.FirstOrDefault(u => u.Token == token);
                if (user == null)
                {
                    throw new TokenInvalidoException();
                }

                ExtensionEvento ext = context.ExtensionesEvento.FirstOrDefault(e => e.Id == idExtension);
                if ((ext == null) || (!TieneAcceso.estaDespachandoExtension(user, ext)))
                {
                    throw new ExtensionInvalidaException();
                }

                if (ext.Evento.Estado == EstadoEvento.Creado)
                {
                    throw new EventoNoEnviadoException();
                }

                // Libero recursos de extension.
                foreach (Recurso r in ext.Recursos)
                {
                    r.ExtensionesEventos.Remove(ext);
                    r.EstadoAsignacion = EstadoAsignacionRecurso.Libre;
                }

                // Libero al despachador.
                user.Despachando.Remove(ext);

                ext.Estado = EstadoExtension.Cerrado;
                ext.TimeStamp = DateTime.Now;
                ext.Evento.TimeStamp = DateTime.Now;
                context.SaveChanges();
                return true;
            }
        }

        public bool actualizarDescripcionDespachador(string token, DtoActualizarDescripcion descr)
        {
            using (var context = new EmsysContext())
            {
                if (token == null)
                {
                    throw new TokenInvalidoException();
                }

                var user = context.Usuarios.FirstOrDefault(u => u.Token == token);
                if (user == null)
                {
                    throw new TokenInvalidoException();
                }

                ExtensionEvento ext = context.ExtensionesEvento.FirstOrDefault(e => e.Id == descr.idExtension);
                if ((ext == null) || (!TieneAcceso.estaDespachandoExtension(user, ext)))
                {
                    throw new ExtensionInvalidaException();
                }

                string formato = "yyyy-MM-dd'T'hh:mm:ss.FFF";
                string descrFormateada = "\\" + DateTime.Now.ToString(formato) + "\\" + user.Nombre + "\\" + descr.descripcion;
                ext.DescripcionDespachador += descrFormateada;
                ext.TimeStamp = DateTime.Now;
                ext.Evento.TimeStamp = DateTime.Now;
                context.SaveChanges();
                return true;
            }
        }
    }
}
