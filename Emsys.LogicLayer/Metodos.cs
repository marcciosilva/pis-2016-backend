using DataTypeObject;
using DataTypeObjetc;
using Emsys.DataAccesLayer.Core;
using Emsys.DataAccesLayer.Model;
using Emsys.LogicLayer.ApplicationExceptions;
using Emsys.LogicLayer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Emsys.LogicLayer
{
    public class Metodos : IMetodos
    {
        public DtoAutenticacion autenticarUsuario(string userName, string password)
        {
            using (var context = new EmsysContext())
            {
                var user = context.Users.FirstOrDefault(u => u.NombreUsuario == userName);
                if (user != null && user.Contraseña == Passwords.GetSHA1(password))
                {
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
                    context.SaveChanges();

                    //return new DtoAutenticacion(token, Mensajes.Correcto, rol);
                    return new DtoAutenticacion(token, Mensajes.Correcto);
                }
                throw new InvalidCredentialsException(Mensajes.UsuarioContraseñaInvalidos);
            }
        }
        
        // Metodo legado.
        public DtoRol getRolUsuario(string token)
        {
            using (var context = new EmsysContext())
            {
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
                                recursos.Add(DtoGetters.getDtoRecurso(r));
                        }
                    }
                    DtoRol rol = new DtoRol() { zonas = zonas, recursos = recursos };
                    return rol;
                }
                throw new InvalidTokenException(Mensajes.TokenInvalido);
            }
        }

        public bool autorizarUsuario(string token, string[] etiquetas)
        {
            using (var context = new EmsysContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Token == token);
                if (user != null)
                {
                    if (!etiquetas.Any())
                    {
                        return true;
                    }
                    foreach (var item in etiquetas)
                    {
                        foreach (ApplicationRole ar in user.ApplicationRoles)
                        {
                            foreach (Permiso p in ar.Permisos)
                            {
                                if (item == p.Clave)
                                {
                                    if (user.FechaInicioSesion.Value.Year < DateTime.Now.Year ||
                                        user.FechaInicioSesion.Value.Month < DateTime.Now.Month ||
                                        user.FechaInicioSesion.Value.Day < DateTime.Now.Day ||
                                        user.FechaInicioSesion.Value.Hour < DateTime.Now.Hour - 8
                                        )
                                    {
                                        //libero recursos y expiro el token
                                        user.Recurso.ToList().ForEach(x => x.Estado = 0);
                                        user.Token = null;
                                        user.FechaInicioSesion = null;
                                        context.SaveChanges();
                                        return false;
                                    }
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
                var user = context.Users.FirstOrDefault(u => u.Token == token);
                if (user != null)
                {
                    // Quita posibles logins previos
                    user.Zonas.Clear();
                    user.Recurso.Clear();
                    context.SaveChanges();

                    // Si el usuario se loguea por recurso
                    if (rol.recursos.Count() == 1 && rol.zonas.Count() == 0)
                    {
                        bool okRecurso = false;
                        // Verifica que el recurso seleccionado sea seleccionable por el usuario.
                        foreach (Grupo_Recurso gr in user.Grupos_Recursos)
                        {
                            if (gr.Recursos.FirstOrDefault(r => r.Id == rol.recursos.FirstOrDefault().id) != null)
                            {
                                okRecurso = true;
                                break;
                            }
                        }
                        // Si es seleccionable y esta libre se lo asigna y lo marca como no disponible.
                        if (okRecurso && (context.Recursos.Find(rol.recursos.FirstOrDefault().id).Estado == EstadoRecurso.Disponible))
                        {
                            user.Recurso.Add(context.Recursos.Find(rol.recursos.FirstOrDefault().id));
                            context.Recursos.Find(rol.recursos.FirstOrDefault().id).Estado = EstadoRecurso.NoDisponible;
                            context.SaveChanges();
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    // Si el usuario se loguea por zonas.
                    else if (rol.recursos.Count() == 0 && rol.zonas.Count() > 0)
                    {
                        foreach (DtoZona z in rol.zonas)
                        {
                            // Verifica que el usuario pertenezca a la unidad ejecutora de cada zona.
                            Zona zona = context.Zonas.Find(z.id);
                            if (user.Unidades_Ejecutoras.Contains(zona.Unidad_Ejecutora))
                            {
                                user.Zonas.Add(context.Zonas.Find(z.id));
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
                    else if (rol.recursos.Count() == 0 && rol.zonas.Count() == 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                throw new InvalidTokenException(Mensajes.TokenInvalido);
            }
        }

        public ICollection<DtoEvento> listarEventos(string token)
        {
            using (var context = new EmsysContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Token == token);
                if (user != null)
                {
                    List<int> eventosAgregados = new List<int>();
                    List<DtoEvento> eventos = new List<DtoEvento>();


                    // Si el usuario esta conectado como recurso.
                    if (user.Recurso.Count() > 0)
                    {
                        foreach (Extension_Evento ext in user.Recurso.FirstOrDefault().Extensiones_Eventos)
                        {
                            if (ext.Estado != Emsys.DataAccesLayer.Model.EstadoExtension.Cerrado && !eventosAgregados.Contains(ext.Evento.Id))
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
                                if (ext.Estado != Emsys.DataAccesLayer.Model.EstadoExtension.Cerrado && !eventosAgregados.Contains(ext.Evento.Id))
                                {
                                    eventos.Add(DtoGetters.getDtoEvento(ext.Evento));
                                    eventosAgregados.Add(ext.Evento.Id);
                                }
                            }
                        }
                    }
                    return eventos;
                }
                throw new InvalidTokenException(Mensajes.TokenInvalido);
            }
        }


        public bool cerrarSesion(string token)
        {
            using (var context = new EmsysContext())
            {
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
                    context.SaveChanges();
                    return true;
                }
                throw new InvalidTokenException(Mensajes.TokenInvalido);
            }            
        }


        public string getNombreUsuario(string token)
        {

            using (var context = new EmsysContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Token == token);
                if (user != null)
                {
                    return user.NombreUsuario;
                }
                return "";
            }
        }
    }
}
