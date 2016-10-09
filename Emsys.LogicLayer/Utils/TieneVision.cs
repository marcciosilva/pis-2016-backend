using Emsys.DataAccesLayer.Core;
using Emsys.DataAccesLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Emsys.LogicLayer.Utils
{
    public class TieneVision
    {
        public static bool tieneVisionEvento(ApplicationUser user, Evento evento)
        {
            using (var context = new EmsysContext())
            {
                if (user != null)
                {
                    if (user.Recurso.Count() > 0)
                    {
                        foreach (Extension_Evento ext in user.Recurso.FirstOrDefault().Extensiones_Eventos)
                        {
                            if ((ext.Estado != EstadoExtension.Cerrado) && (ext.Evento == evento))
                            {
                                return true;
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
                                if ((ext.Estado != EstadoExtension.Cerrado) && (ext.Evento == evento))
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


        public static bool tieneVisionExtension(ApplicationUser user, Extension_Evento extension)
        {
            using (var context = new EmsysContext())
            {
                if (user != null)
                {
                    if (user.Recurso.Count() > 0)
                    {
                        foreach (Extension_Evento ext in user.Recurso.FirstOrDefault().Extensiones_Eventos)
                        {
                            if ((ext.Estado != EstadoExtension.Cerrado) && (ext == extension))
                            {
                                return true;
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
                                if ((ext.Estado != EstadoExtension.Cerrado) && (ext == extension))
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
    }
}
