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
        public static DtoApplicationFile GetDtoApplicationfile(ApplicationFile file)
        {
            return new DtoApplicationFile()
            {
                nombre = file.Nombre,
                file_data = file.FileData
            };
        }

        public static DtoAccionesRecursoExtension getDtoAccionesRecursoExtension(AsignacionRecurso acciones)
        {           
            var desc = "";
            acciones.AsignacionRecursoDescripcion.ForEach(x => desc=desc +"/"+ x.Descripcion);
            return new DtoAccionesRecursoExtension()
            {
                id = acciones.Id,
                recurso = acciones.Recurso.Codigo,
                descripcion = desc,
                fecha_arribo = acciones.FechaArribo,
                descripcion = acciones.Descripcion,
                actualmente_asignado = acciones.ActualmenteAsignado
            };
        }

        public static DtoImagen getDtoImagen(Imagen img)
        {
            return new DtoImagen()
            {
                id = img.Id,
                id_imagen = img.ImagenData.Id,
                usuario = img.Usuario.Nombre,
                fecha_envio = img.FechaEnvio
            };
        }

        public static DtoVideo getDtoVideo(Video vid)
        {
            return new DtoVideo()
            {
                id = vid.Id,
                id_video = vid.VideoData.Id,
                usuario = vid.Usuario.Nombre,
                fecha_envio = vid.FechaEnvio
            };
        }

        public static DtoAudio getDtoAudio(Audio aud)
        {
            return new DtoAudio()
            {
                id = aud.Id,
                id_audio = aud.AudioData.Id,
                usuario = aud.Usuario.Nombre,
                fecha_envio = aud.FechaEnvio
            };
        }

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
                nombre_ue = zona.UnidadEjecutora.Nombre
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


        public static DtoItemListar getDtoItemListar(Extension_Evento ext)
        {
            DtoCategoria cat = null;
            if (ext.SegundaCategoria != null)
            {
                cat = getDtoCategoria(ext.SegundaCategoria);
            }
            else
            {
                cat = getDtoCategoria(ext.Evento.Categoria);
            }

            string desp = null;
            if (ext.Estado == EstadoExtension.Despachado)
                desp = ext.Despachador.Nombre;

            DtoGeoUbicacion geoU = null;
            if ((ext.Evento.Latitud != 0) && (ext.Evento.Longitud != 0))
            {
                geoU = new DtoGeoUbicacion() { latitud = ext.Evento.Latitud, longitud = ext.Evento.Longitud };
            }

            return new DtoItemListar()
            {
                id_evento = ext.Evento.Id,
                zona = getDtoZona(ext.Zona),
                descripcion = ext.Evento.Descripcion,
                despachador = desp,
                estado = ext.Estado.ToString().ToLower(),
                fecha_creacion = ext.Evento.FechaCreacion,
                categoria = cat,
                geoubicacion = geoU
            };
        }


        public static DtoExtension getDtoExtension(Extension_Evento ext)
        {
            List<string> recursos = new List<string>();
            foreach (Recurso r in ext.Recursos)
                recursos.Add(r.Codigo);

            List<DtoAccionesRecursoExtension> acciones = new List<DtoAccionesRecursoExtension>();
            foreach (AsignacionRecurso a in ext.AccionesRecursos)
                acciones.Add(getDtoAccionesRecursoExtension(a));

            DtoCategoria cat = null;
            if (ext.SegundaCategoria != null)
                cat = getDtoCategoria(ext.SegundaCategoria);

            string desp = null;
            if (ext.Despachador != null)
                desp = ext.Despachador.Nombre;

            List<DtoImagen> imgs = new List<DtoImagen>();
            foreach (Imagen i in ext.Imagenes)
                imgs.Add(getDtoImagen(i));

            List<DtoVideo> vids = new List<DtoVideo>();
            foreach (Video v in ext.Videos)
                vids.Add(getDtoVideo(v));

            List<DtoAudio> auds = new List<DtoAudio>();
            foreach (Audio a in ext.Audios)
                auds.Add(getDtoAudio(a));

            List<DtoGeoUbicacion> geos = new List<DtoGeoUbicacion>();
            foreach (GeoUbicacion g in ext.GeoUbicaciones)
                geos.Add(getDtoGeoUbicacion(g));

            return new DtoExtension()
            {
                id = ext.Id,
                zona = getDtoZona(ext.Zona),
                despachador = desp,
                descripcion_supervisor = ext.DescripcionSupervisor,
                asignaciones_recursos = asignaciones,
                estado = ext.Estado.ToString().ToLower(),
                time_stamp = ext.TimeStamp,
                segunda_categoria = cat,
                recursos = recursos,
                imagenes = imgs,
                videos = vids,
                audios = auds,
                geo_ubicaciones = geos
            };
            //if (ext.DescripcionDespachador != null)
            //{
            //    res.descripcion_despachadores = parsearDesacripcion(ext.DescripcionDespachador, OrigenDescripcion.Despachador).ToList();
            //}
            //else
            //{
            //    res.descripcion_despachadores = new List<DtoDescripcion>();
            //}
            
            //return res;
        }


        public static DtoEvento getDtoEvento(Evento evento)
        {
            List<DtoExtension> extensiones = new List<DtoExtension>();
            foreach (Extension_Evento e in evento.ExtensionesEvento)
            {
                extensiones.Add(getDtoExtension(e));
            }

            DtoGeoUbicacion ubicacion = null;
            if ((evento.Longitud != 0) && (evento.Latitud != 0))
            {
                ubicacion = new DtoGeoUbicacion()
                {
                    longitud = evento.Longitud,
                    latitud = evento.Latitud
                };
            }

            string dep = "";
            if (evento.Departamento != null)
                dep = evento.Departamento.Nombre;

            List<DtoImagen> imgs = new List<DtoImagen>();
            foreach (Imagen i in evento.Imagenes)
                imgs.Add(getDtoImagen(i));

            List<DtoVideo> vids = new List<DtoVideo>();
            foreach (Video v in evento.Videos)
                vids.Add(getDtoVideo(v));

            List<DtoAudio> auds = new List<DtoAudio>();
            foreach (Audio a in evento.Audios)
                auds.Add(getDtoAudio(a));

            string cread = null;
            if (evento.Usuario != null)
                cread = evento.Usuario.Nombre;

            return new DtoEvento()
            {
                id = evento.Id,
                informante = evento.NombreInformante,
                telefono = evento.TelefonoEvento,
                categoria = getDtoCategoria(evento.Categoria),
                estado = evento.Estado.ToString().ToLower(),
                time_stamp = evento.TimeStamp,
                creador = cread,
                fecha_creacion = evento.FechaCreacion,                
                calle = evento.Calle,
                esquina = evento.Esquina,
                numero = evento.Numero,
                departamento = dep,
                sector = evento.Sector.Nombre,
                descripcion = evento.Descripcion,
                en_proceso = evento.EnProceso,
                extensiones = extensiones,
                imagenes = imgs,
                videos = vids,
                audios = auds
            };
            //if (ubicacion != null)
            //{
            //    res.geo_ubicacion = ubicacion;
            //}
            //if (evento.Usuario != null)
            //{
            //    res.creador = evento.Usuario.Nombre;
            //}
            //return res;
        }

        /// <summary>
        /// Metodo auxiliar que convierte un string con formato hora1\\usuario1\\texto1\\hora2\\usuario2\\texto2....
        /// en una colección de DtoDescripcion
        /// </summary>
        /// <param name="descripcion"></param>
        /// <returns></returns>
        private static IEnumerable<DtoDescripcion> parsearDesacripcion(string descripcion, OrigenDescripcion origen)
        {
            string[] separadores = { "\\" };
            string[] textoParseado = descripcion.Split(separadores, int.MaxValue, StringSplitOptions.None);

            List<DtoDescripcion> resultado = new List<DtoDescripcion>();

            DtoDescripcion d;
            for (int i = 0; i < textoParseado.Count(); i = i + 3)
            {
                d = new DtoDescripcion()
                {
                    fecha = DateTime.Parse(textoParseado[i]),
                    usuario = textoParseado[i + 1],
                    texto = textoParseado[i + 2],
                    origen = origen
                };
                resultado.Add(d);
            }

            return resultado;
        }
    }
}
