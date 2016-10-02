using DataTypeObject;
using Emsys.DataAccesLayer.Core;
using Emsys.DataAccesLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaAcessoDatos
{
    public class Metodos : IMetodos
    {
        /// <summary>
        /// Indica si las credenciales ingresadas son validas para algun usuario.
        /// </summary>
        /// <param name="userName">Nombre de usuario</param>
        /// <param name="pass">Contraseña hasheada del usuario</param>
        /// <returns>Verdadero o falso si las credenciales son correctas</returns>
        public bool autenticarUsuario(string userName, string pass)
        {
            using (var context = new EmsysContext())
            {
                if (context.Users.FirstOrDefault(u => u.UserName == userName) != null && context.Users.FirstOrDefault(u => u.UserName == userName).Contraseña == pass)
                    return true;
                return false;
            }
        }

        public bool registrarInicioUsuario(string userName, string token, DateTime fecha)
        {
            using (var context = new EmsysContext())
            {
                var user = context.Users.FirstOrDefault(u => u.NombreUsuario == userName);
                if (user != null)
                {
                    user.Token = token;
                    user.FechaInicioSesion = fecha;
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Retorna una coleccion de eventos que son visibles al usuario en cuestion.
        /// </summary>
        /// <param name="userName">El nombre de usuario</param>
        /// <returns>Una coleccion de DtoEvento de los eventos visibles al usuario, con sus respectivos DtoExtension e informacion basica</returns>
        public ICollection<DtoEvento> listarEventos(string userName)
        {
            using (var context = new EmsysContext())
            {
                List<int> eventosAgregados = new List<int>();
                List<DtoEvento> eventos = new List<DtoEvento>();
                var user = context.Users.FirstOrDefault(u => u.NombreUsuario == userName);

                // Si el usuario esta conectado como recurso.
                if (user.Recurso.Count() > 0)
                {
                    foreach (Extension_Evento ext in user.Recurso.FirstOrDefault().Extensiones_Eventos)
                    {
                        if (ext.Estado != Emsys.DataAccesLayer.Model.EstadoExtension.Cerrado && !eventosAgregados.Contains(ext.Evento.Id))
                        {
                            eventos.Add(ext.Evento.getDto());
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
                                eventos.Add(ext.Evento.getDto());
                                eventosAgregados.Add(ext.Evento.Id);
                            }
                        }
                    }
                }
                return eventos;
            }            
        }
        
        /// <summary>
        /// Este metodo indica las zonas y recursos con los cuales el usuario en cuestion puede iniciar sesion.
        /// </summary>
        /// <param name="userName">Nombre de usuario</param>
        /// <returns>DtoRol que contiene una coleccion de DtoZona y otra de DtoRecurso correspondiente a las zonas y recursos disponibles al usuario</returns>
        public DtoRol getRolUsuario(string userName)
        {
            using (var context = new EmsysContext())
            {
                var user = context.Users.FirstOrDefault(u => u.NombreUsuario == userName);
                // Agrega las zonas disponibles para el usuario mediante sus unidades ejecutoras.
                ICollection<DtoZona> zonas = new List<DtoZona>();
                foreach (Unidad_Ejecutora ue in user.Unidades_Ejecutoras)
                {
                    foreach (Zona z in ue.Zonas)
                    {
                        zonas.Add(z.getDto());
                    }
                }
                // Agrega los recursos disponibles para el usuario mediante sus grupos_recursos.
                ICollection<DtoRecurso> recursos = new List<DtoRecurso>();
                foreach (Grupo_Recurso gr in user.Grupos_Recursos)
                {
                    foreach (Recurso r in gr.Recursos)
                    {
                        if(r.Estado == EstadoRecurso.Disponible)
                            recursos.Add(r.getDto());
                    }
                }
                DtoRol rol = new DtoRol() { zonas = zonas, recursos = recursos };
                return rol;
            }
        }

        /// <summary>
        /// Indica al sistema las zonas o recurso con los cuales el usuario desea iniciar sesion (determinando la informacion que sera visible al usuario).
        /// </summary>
        /// <param name="userName">Nombre de usuario</param>
        /// <param name="rol">DtoRol que contiene una coleccion con las zonas que desea ver el usuario, o una coleccion con un unico recurso que desea tomar el usuario </param>
        public void loguearUsuario(string userName, DtoRol rol)
        {
            using (var context = new EmsysContext())
            {
                var user = context.Users.FirstOrDefault(u => u.NombreUsuario == userName);

                // Quita posibles logins previos
                user.Zonas.Clear();
                user.Recurso.Clear();
                context.SaveChanges();

                // Si el usuario se loguea por recurso
                if (rol.recursos.Count() == 1 && rol.zonas.Count() == 0)
                {
                    bool okRecurso = false;
                    // Verifica que el recurso seleccionado sea seleccionable por el usuario.
                    foreach(Grupo_Recurso gr in user.Grupos_Recursos)
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
                        return;
                    }
                    else
                    {
                        throw new ArgumentException("Recurso invalido");
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
                            throw new ArgumentException("Argumentos invalidos");
                        }                        
                    }
                    context.SaveChanges();
                    return;
                }
                // Si el usuario se loguea como visitante.
                else if (rol.recursos.Count() == 0 && rol.zonas.Count() == 0)
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Indica al sistema que un usuario desea desconectarse, se le quitan las zonas y recursos asociados.
        /// </summary>
        /// <param name="userName">Nombre de usuario</param>
        public void cerrarSesion(string userName)
        {
            using (var context = new EmsysContext())
            {
                var user = context.Users.FirstOrDefault(u => u.NombreUsuario == userName);
                user.Zonas.Clear();
                foreach (Recurso r in user.Recurso)
                {
                    r.Estado = EstadoRecurso.Disponible;
                }
                user.Recurso.Clear();
                user.Token = null;
                user.FechaInicioSesion = null;
                context.SaveChanges();
            }
            return;
        }

        
    }
}
