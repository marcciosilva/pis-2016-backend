using DataTypeObject;
using Emsys.DataAccesLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emsys.LogicLayer.Utils
{
    class DtoGetters
    {
        public static DtoGeoUbicacion getDtoGeoUbicacion(GeoUbicacion ubicacion)
        {
            return new DtoGeoUbicacion()
            {
                longitud = ubicacion.Longitud,
                latitud = ubicacion.Latitud
            };
        }

        public static DtoZona getDtoZona(Zona zona)
        {
            return new DtoZona()
            {
                id = zona.Id,
                nombre = zona.Nombre,
                nombre_ue = zona.Unidad_Ejecutora.Nombre
            };
        }

        public static DtoRecurso getDtoRecurso(Recurso recurso)
        {
            return new DtoRecurso()
            {
                id = recurso.Id,
                codigo = recurso.Codigo
            };
        }

        public static DtoCategoria getDtoCategoria(Categoria categoria)
        {
            return new DtoCategoria()
            {
                id = categoria.Id,
                codigo = categoria.Codigo,
                clave = categoria.Clave,
                prioridad = categoria.Prioridad.ToString().ToLower(),
                activo = categoria.Activo
            };
        }

        public static DtoExtension getDtoExtension(Extension_Evento ext)
        {
            List<string> recursos = new List<string>();
            foreach (Recurso r in ext.Recursos)
            {
                recursos.Add(r.Codigo);
            }
            DtoCategoria cat = null;
            if (ext.SegundaCategoria != null)
            {
                cat = getDtoCategoria(ext.SegundaCategoria);
            }

            return new DtoExtension()
            {
                zona = getDtoZona(ext.Zona),
                descripcion = ext.DescripcionDespachador,
                estado = ext.Estado.ToString().ToLower(),
                time_stamp = ext.TimeStamp,
                categoria = cat,
                recursos = recursos
            };
        }


        public static DtoEvento getDtoEvento(Evento evento)
        {
            List<DtoExtension> extensiones = new List<DtoExtension>();
            foreach (Extension_Evento e in evento.ExtensionesEvento)
            {
                extensiones.Add(getDtoExtension(e));
            }
            string dep = null;
            string sec = null;
            if (evento.Departamento != null)
                dep = evento.Departamento.Nombre;
            if (evento.Sector != null)
                sec = evento.Sector.Nombre;   
                     
            return new DtoEvento()
            {
                id = evento.Id,
                informante = evento.NombreInformante,
                telefono = evento.TelefonoEvento,
                categoria = getDtoCategoria(evento.Categoria),
                estado = evento.Estado.ToString().ToLower(),
                time_stamp = evento.TimeStamp,
                fecha_creacion = evento.FechaCreacion,
                departamento = dep,
                calle = evento.Calle,
                esquina = evento.Esquina,
                numero = evento.Numero,
                sector = sec,
                latitud = evento.Latitud,
                longitud = evento.Longitud,
                descripcion = evento.Descripcion,
                en_proceso = evento.EnProceso,
                extensiones = extensiones
            };
        }
    }
}
