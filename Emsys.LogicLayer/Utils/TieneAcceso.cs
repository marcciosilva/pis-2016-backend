using Emsys.DataAccesLayer.Core;
using Emsys.DataAccesLayer.Model;
using System.Linq;

namespace Emsys.LogicLayer.Utils
{
    public class TieneAcceso
    {
        public static bool tieneVisionEvento(Usuario user, Evento evento)
        {
            if (evento == null)
            {
                return false;
            }

            using (var context = new EmsysContext())
            {
                if (user != null)
                {
                    if (user.Recurso.Count() > 0)
                    {
                        Extension_Evento ext = user.Recurso.FirstOrDefault().Extensiones_Eventos.FirstOrDefault(e => e.Evento.Id == evento.Id);
                        if ((ext != null) && (ext.Estado != EstadoExtension.Cerrado))
                        {
                            return true;
                        }
                    }

                    // Si el usuario esta conectado por zonas.
                    else if (user.Zonas.Count() > 0)
                    {
                        foreach (Extension_Evento ext in evento.ExtensionesEvento)
                        {
                            if ((ext.Estado != EstadoExtension.Cerrado) && (user.Zonas.Contains(ext.Zona)))
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
        }

        public static bool tieneVisionExtension(Usuario user, Extension_Evento extension)
        {
            using (var context = new EmsysContext())
            {
                if (user != null)
                {
                    if (user.Recurso.Count() > 0)
                    {
                        Extension_Evento ext = user.Recurso.FirstOrDefault().Extensiones_Eventos.FirstOrDefault(e => e.Evento.Id == extension.Id);
                        if ((ext != null) && (ext.Estado != EstadoExtension.Cerrado))
                        {
                            return true;
                        }
                    }

                    // Si el usuario esta conectado por zonas.
                    else if (user.Zonas.Count() > 0)
                    {
                        if (tieneVisionEvento(user, extension.Evento))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        public static bool estaAsignadoExtension(Usuario user, Extension_Evento extension)
        {
            using (var context = new EmsysContext())
            {
                if (user == null)
                {
                    return false;
                }
                if (user.Recurso.Count() == 0)
                {
                    return false;
                }
                Extension_Evento ext = user.Recurso.FirstOrDefault().Extensiones_Eventos.FirstOrDefault(e => e.Id == extension.Id);
                if ((ext != null) && (ext.Estado != EstadoExtension.Cerrado))
                {
                    return true;
                }
                return false;
            }
        }

        public static bool estaDespachandoExtension(Usuario user, Extension_Evento extension)
        {
            using (var context = new EmsysContext())
            {
                if (user == null)
                {
                    return false;
                }
                if (user.Zonas.Count() == 0)
                {
                    return false;
                }

                if ((user.Despachando.FirstOrDefault(e => e.Id == extension.Id) != null) && (extension.Despachador.Id == user.Id))
                {
                    return true;
                }
                return false;
            }
        }
    }
}
