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

        public bool loguearUsuario(string userName, ICollection<DtoRol> rol)
        {
            using (var context = new EmsysContext())
            {
                var user = context.Users.FirstOrDefault(u => u.UserName == userName);

                // Quita posibles logins previos
                user.Zonas.Clear();
                user.Recurso.Clear();
                
                // Asigna el usario a las zonas o al recurso correspondiente.
                foreach (DtoRol dtr in rol)
                {
                    // Si el usuario se loguea por recurso
                    if (dtr.GetType().Equals(typeof(DtoRecurso)))
                    {
                        DtoRecurso rec = (DtoRecurso)dtr;
                        user.Recurso.Add(context.Recursos.Find(rec.IdRecurso));
                        return true;
                    }
                    else if (dtr.GetType().Equals(typeof(DtoZona)))
                    {
                        DtoZona zona = (DtoZona)dtr;
                        user.Zonas.Add(context.Zonas.Find(zona.IdZona));
                    }
                }
            }
            return true;
        }
    }
}
