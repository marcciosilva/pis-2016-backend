namespace Emsys.LogicLayer.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using DataAccesLayer.Model;
    using DataTypeObject;

    public class DtoGetters
    {
        public static DtoApplicationFile GetDtoApplicationfile(ApplicationFile file)
        {
            return new DtoApplicationFile()
            {
                nombre = file.Nombre,
                fileData = file.FileData
            };
        }

        public static byte[] GenerateImageThumbnail(ApplicationFile file)
        {
            var ms = new MemoryStream(file.FileData);
            Image img = Image.FromStream(ms);
            Image thumb = img.GetThumbnailImage(120, 120, () => false, IntPtr.Zero);
            ms = new MemoryStream();
            thumb.Save(ms, img.RawFormat);
            return ms.ToArray();
        }

        public static DtoAsignacionRecurso getDtoAsignacionesRecursos(AsignacionRecurso asignacionRecurso)
        {
            DtoAsignacionRecurso res = new DtoAsignacionRecurso()
            {
                id = asignacionRecurso.Id,
                recurso = asignacionRecurso.Recurso.Codigo,
                fechaArribo = asignacionRecurso.HoraArribo,
                actualmenteAsignado = asignacionRecurso.ActualmenteAsignado
            };
            var desc = new List<DtoDescripcion>();
            foreach (var item in asignacionRecurso.AsignacionRecursoDescripcion)
            {
                desc.Add(new DtoDescripcion
                {
                    descripcion = item.Descripcion,
                    fecha = item.Fecha,
                    origen = OrigenDescripcion.Despachador,
                    agregadaOffline = item.agregadaOffline
                });
            }

            res.descripcion = desc;
            return res;
        }

        public static DtoImagen getDtoImagen(Imagen img)
        {
            return new DtoImagen()
            {
                id = img.Id,
                id_imagen = img.ImagenData.Id,
                usuario = img.Usuario.Nombre,
                fechaEnvio = img.FechaEnvio
            };
        }

        public static DtoOrigenEvento getDtoOrigenEvento(OrigenEvento oe)
        {
            return new DtoOrigenEvento()
            {
                id = oe.Id,
                idOrigen = oe.IdOrigen,
                tipoOrigen = oe.TipoOrigen
            };
        }


        public static DtoVideo getDtoVideo(Video vid)
        {
            return new DtoVideo()
            {
                id = vid.Id,
                idVideo = vid.VideoData.Id,
                usuario = vid.Usuario.Nombre,
                fechaEnvio = vid.FechaEnvio
            };
        }

        public static DtoAudio getDtoAudio(Audio aud)
        {
            return new DtoAudio()
            {
                id = aud.Id,
                idAudio = aud.AudioData.Id,
                usuario = aud.Usuario.Nombre,
                fechaEnvio = aud.FechaEnvio
            };
        }

        public static DtoGeoUbicacion getDtoGeoUbicacion(GeoUbicacion ubicacion)
        {
            return new DtoGeoUbicacion()
            {
                longitud = ubicacion.Longitud,
                latitud = ubicacion.Latitud,
                fechaEnvio = ubicacion.FechaEnvio                
            };
        }

        public static DtoDepartamento getDtoDepartamento(Departamento dep)
        {
            return new DtoDepartamento()
            {
                id = dep.Id,
                nombre = dep.Nombre
            };
        }

        public static DtoSector getDtoSector(Sector sector)
        {
            return new DtoSector()
            {
                id = sector.Id,
                nombre = sector.Nombre
            };
        }

        public static DtoZona getDtoZona(Zona zona)
        {
            return new DtoZona()
            {
                id = zona.Id,
                nombre = zona.Nombre,
                nombreUe = zona.UnidadEjecutora.Nombre
            };
        }

        public static DtoZona getDtoZonaCompleto(Zona zona)
        {
            ICollection<DtoSector> sects = new List<DtoSector>();
            foreach (Sector s in zona.Sectores)
            {
                sects.Add(getDtoSector(s));
            }

            return new DtoZona()
            {
                id = zona.Id,
                nombre = zona.Nombre,
                nombreUe = zona.UnidadEjecutora.Nombre,
                sectores = sects
            };
        }

        public static DtoRecurso getDtoRecurso(Recurso recurso)
        {
            string user = null;
            if ((recurso.Estado == EstadoRecurso.NoDisponible) && (recurso.Usuario != null))
            {
                user = recurso.Usuario.Nombre;
            }

            return new DtoRecurso()
            {
                id = recurso.Id,
                codigo = recurso.Codigo,
                estado = recurso.Estado.ToString().ToLower(),
                estadoAsignacion = recurso.EstadoAsignacion.ToString().ToLower(),
                usuario = user
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
        
        public static DtoExtension getDtoExtension(ExtensionEvento ext)
        {
            List<string> recursos = new List<string>();
            foreach (Recurso r in ext.Recursos)
            {
                recursos.Add(r.Codigo);
            }

            List<DtoAsignacionRecurso> asignaciones = new List<DtoAsignacionRecurso>();
            foreach (AsignacionRecurso a in ext.AsignacionesRecursos)
            {
                asignaciones.Add(getDtoAsignacionesRecursos(a));
            }  

            DtoCategoria cat = null;
            if (ext.SegundaCategoria != null)
            {
                cat = getDtoCategoria(ext.SegundaCategoria);
            }   

            string desp = null;
            if (ext.Despachador != null)
            {
                desp = ext.Despachador.Nombre;
            }

            List<DtoImagen> imgs = new List<DtoImagen>();
            foreach (Imagen i in ext.Imagenes)
            {
                imgs.Add(getDtoImagen(i));
            } 

            List<DtoVideo> vids = new List<DtoVideo>();
            foreach (Video v in ext.Videos)
            {
                vids.Add(getDtoVideo(v));
            }    

            List<DtoAudio> auds = new List<DtoAudio>();
            foreach (Audio a in ext.Audios)
            {
                auds.Add(getDtoAudio(a));
            }    

            List<DtoGeoUbicacion> geos = new List<DtoGeoUbicacion>();
            foreach (GeoUbicacion g in ext.GeoUbicaciones)
            {
                geos.Add(getDtoGeoUbicacion(g));
            }

            List<DtoDescripcion> descDespachadores;
            if (ext.DescripcionDespachador != null)
            {
                descDespachadores = parsearDesacripcion(ext.DescripcionDespachador, OrigenDescripcion.Despachador).ToList();
            }
            else
            {
                descDespachadores = new List<DtoDescripcion>();
            }

            return new DtoExtension()
            {
                id = ext.Id,
                zona = getDtoZona(ext.Zona),
                despachador = desp,
                descripcionSupervisor = ext.DescripcionSupervisor,
                asignacionesRecursos = asignaciones,
                estado = ext.Estado.ToString().ToLower(),
                timeStamp = ext.TimeStamp,
                segundaCategoria = cat,
                recursos = recursos,
                imagenes = imgs,
                videos = vids,
                audios = auds,
                geoUbicaciones = geos,
                descripcionDespachadores = descDespachadores,
            };
        }

        public static DtoEvento getDtoEvento(Evento evento)
        {
            List<DtoExtension> extensiones = new List<DtoExtension>();
            foreach (ExtensionEvento e in evento.ExtensionesEvento)
            {
                extensiones.Add(getDtoExtension(e));
            }

            string dep = string.Empty;
            if (evento.Departamento != null)
            {
                dep = evento.Departamento.Nombre;
            }  

            List<DtoImagen> imgs = new List<DtoImagen>();
            foreach (Imagen i in evento.Imagenes)
            {
                imgs.Add(getDtoImagen(i));
            }   

            List<DtoVideo> vids = new List<DtoVideo>();
            foreach (Video v in evento.Videos)
            {
                vids.Add(getDtoVideo(v));
            }  

            List<DtoAudio> auds = new List<DtoAudio>();
            foreach (Audio a in evento.Audios)
            {
                auds.Add(getDtoAudio(a));
            }

            string cread = null;
            if (evento.Usuario != null)
            {
                cread = evento.Usuario.Nombre;
            }

            DtoOrigenEvento oe = null;
            if (evento.OrigenEvento != null)
            {
                oe = getDtoOrigenEvento(evento.OrigenEvento);
            }  

            return new DtoEvento()
            {
                id = evento.Id,
                informante = evento.NombreInformante,                
                telefono = evento.TelefonoEvento,
                categoria = getDtoCategoria(evento.Categoria),
                estado = evento.Estado.ToString().ToLower(),
                timeStamp = evento.TimeStamp,
                creador = cread,
                fechaCreacion = evento.FechaCreacion,
                calle = evento.Calle,
                esquina = evento.Esquina,
                numero = evento.Numero,
                departamento = dep,
                sector = evento.Sector.Nombre,
                longitud = evento.Longitud,
                latitud = evento.Latitud,
                descripcion = evento.Descripcion,
                enProceso = evento.EnProceso,
                extensiones = extensiones,
                imagenes = imgs,
                videos = vids,
                audios = auds,
                origenEvento = oe
            };
        }

        /// <summary>
        /// Metodo auxiliar que convierte un string con formato hora1\\usuario1\\texto1\\hora2\\usuario2\\texto2....
        /// en una colección de DtoDescripcion.
        /// </summary>
        /// <param name="descripcion"></param>
        /// <param name="origen"></param>
        /// <returns>Colección de DtoDescripcion</returns>
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
                    descripcion = textoParseado[i + 2],
                    origen = origen
                };
                resultado.Add(d);
            }

            return resultado;
        }
    }
}
