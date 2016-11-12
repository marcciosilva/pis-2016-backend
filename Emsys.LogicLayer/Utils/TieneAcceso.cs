﻿using System.Linq;
using Emsys.DataAccesLayer.Core;
using Emsys.DataAccesLayer.Model;

namespace Emsys.LogicLayer.Utils
{
    public class TieneAcceso
    {
        /// <summary>
        /// Indica si un usuario tiene vision sobre un evento (atraves de zonas o recurso)
        /// </summary>
        /// <param name="user">Usuario en cuestion</param>
        /// <param name="evento">Evento a verificar</param>
        /// <returns>Si el usuario tiene vision o no</returns>
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
                    // Si el usuario esta conectado como recurso.
                    if (user.Recurso.Count() > 0)
                    {
                        AsignacionRecurso asig = user.Recurso.FirstOrDefault().AsignacionesRecurso.FirstOrDefault(a=>a.Extension.Evento.Id == evento.Id);
                        if ((asig != null) && (asig.ActualmenteAsignado == true) && (asig.Extension.Estado != EstadoExtension.Cerrado))
                        {
                            return true;
                        }
                    }
                    else if (user.Zonas.Count() > 0)
                    {
                        //// Si el usuario esta conectado por zonas.
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

        /// <summary>
        /// Indica si un usuario tiene vision sobre una extension.
        /// </summary>
        /// <param name="user">Usuario</param>
        /// <param name="extension">Extension</param>
        /// <returns>Si tiene vision o no</returns>
        public static bool tieneVisionExtension(Usuario user, ExtensionEvento extension)
        {
            using (var context = new EmsysContext())
            {
                if (user != null)
                {
                    if (user.Recurso.Count() > 0)
                    {
                        AsignacionRecurso asig = user.Recurso.FirstOrDefault().AsignacionesRecurso.FirstOrDefault(a => a.Extension.Id == extension.Id);
                        if ((asig != null) && (asig.ActualmenteAsignado == true) && (asig.Extension.Estado != EstadoExtension.Cerrado))
                        {
                            return true;
                        }
                    }                   
                    else if (user.Zonas.Count() > 0)
                    {
                        //// Si el usuario esta conectado por zonas.
                        if (tieneVisionEvento(user, extension.Evento))
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Indica si un usuario esta logueado como recurso y ese recurso esta asignado a determinada la extension indicada o no.
        /// </summary>
        /// <param name="user">Usuario</param>
        /// <param name="extension">Extension</param>
        /// <returns>Devuelve verdadero si el usuario esta logueado como recurso y asignado a la extension, falso en caso contrario</returns>
        public static bool estaAsignadoExtension(Recurso rec, ExtensionEvento extension)
        {
            using (var context = new EmsysContext())
            {
                if (rec == null)
                {
                    return false;
                }

                AsignacionRecurso asig = rec.AsignacionesRecurso.FirstOrDefault(a => a.Extension.Id == extension.Id);
                if ((asig != null) && (asig.ActualmenteAsignado == true) && (asig.Extension.Estado != EstadoExtension.Cerrado))
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Indica si un usuario esta despachando una extension.
        /// </summary>
        /// <param name="user">Usuario</param>
        /// <param name="extension">Extension</param>
        /// <returns>Si esta despachando la extension o no</returns>
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
