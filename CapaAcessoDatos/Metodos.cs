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
        public ICollection<DtoEvento> listarEventos(string userName)
        {
            using (var context = new EmsysContext())
            {
                List<int> eventosAgregados = new List<int>();
                List<DtoEvento> eventos = new List<DtoEvento>();
                var user = context.Users.FirstOrDefault(u => u.UserName == userName);

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

        public bool loguearUsuario(string userName, DtoRol rol)
        {
            using (var context = new EmsysContext())
            {
                var user = context.Users.FirstOrDefault(u => u.UserName == userName);

                // Quita posibles logins previos
                user.Zonas.Clear();
                user.Recurso.Clear();

                // Si el usuario se loguea por recurso
                if (rol.Recursos.Count() == 1 && rol.Zonas.Count() == 0)
                {
                    bool okRecurso = false;
                    // Verifica que el recurso seleccionado esta disponible para el usuario.
                    foreach(Grupo_Recurso gr in user.Grupos_Recursos)
                    {
                        if (gr.Recursos.FirstOrDefault(r => r.Id == rol.Recursos.FirstOrDefault().IdRecurso) != null)
                        {
                            okRecurso = true;
                            break;
                        }
                    }
                    // Si esta disponible, lo asigna.
                    if (okRecurso)
                    {
                        user.Recurso.Add(context.Recursos.Find(rol.Recursos.FirstOrDefault().IdRecurso));
                        context.SaveChanges();
                        return true;
                    }
                }
                // Si el usuario se loguea por zonas.
                else if (rol.Recursos.Count() == 0 && rol.Zonas.Count() > 0)
                {
                    bool okZonas;
                    foreach (DtoZona z in rol.Zonas)
                    {
                        okZonas = false;
                        // Para cada zona verifico que este disponible al usuario.
                        foreach (Unidad_Ejecutora ue in user.Unidades_Ejecutoras)
                        {
                            if (ue.Zonas.FirstOrDefault(zo => zo.Id == zo.Id) != null)
                            {
                                okZonas = true;
                                break;
                            }
                        }
                        // Si la zona esta disponible al usuario se la asigna.
                        if (okZonas)
                        {
                            user.Zonas.Add(context.Zonas.Find(z.IdZona));
                        }
                        // Si existe una zona que no le corresponda, retorna false y no agrega ninguna zona.
                        else
                        {
                            return false;
                        }                        
                    }
                    context.SaveChanges();
                    return true;
                }
                // Si el usuario se loguea como visitante.
                else if (rol.Recursos.Count() == 0 && rol.Zonas.Count() == 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
