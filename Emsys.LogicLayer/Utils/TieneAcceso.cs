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
                        ExtensionEvento ext = user.Recurso.FirstOrDefault().ExtensionesEventos.FirstOrDefault(e => e.Evento.Id == evento.Id);
                        if ((ext != null) && (ext.Estado != EstadoExtension.Cerrado))
                        {
                            return true;
                        }
                    }

                    // Si el usuario esta conectado por zonas.
                    else if (user.Zonas.Count() > 0)
                    {
                        foreach (ExtensionEvento ext in evento.ExtensionesEvento)
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

        public static bool tieneVisionExtension(Usuario user, ExtensionEvento extension)
        {
            using (var context = new EmsysContext())
            {
                if (user != null)
                {
                    if (user.Recurso.Count() > 0)
                    {
                        ExtensionEvento ext = user.Recurso.FirstOrDefault().ExtensionesEventos.FirstOrDefault(e => e.Evento.Id == extension.Id);
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

        public static bool estaAsignadoExtension(Usuario user, ExtensionEvento extension)
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
                ExtensionEvento ext = user.Recurso.FirstOrDefault().ExtensionesEventos.FirstOrDefault(e => e.Id == extension.Id);
                if ((ext != null) && (ext.Estado != EstadoExtension.Cerrado))
                {
                    return true;
                }
                return false;
            }
        }

        public static bool estaDespachandoExtension(Usuario user, ExtensionEvento extension)
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
