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
                if (user.Recurso != null)
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
                else if (user.Zonas != null)
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
    }
}
