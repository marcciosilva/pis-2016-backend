﻿using DataTypeObject;
using Emsys.DataAccesLayer.Core;
using Emsys.DataAccesLayer.Model;
using Emsys.LogicLayer;
using Emsys.LogicLayer.ApplicationExceptions;
using Servicios.Filtros;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Servicios.Controllers
{
    public class AdjuntosController : ApiController
    {
        /// <summary>
        /// Los archivos esperados son del formato MIME.
        /// </summary>
        /// <returns> Retorna una respuesta con el resultado de cargar el objeto.</returns>
        [Route("subirFile")]
        public DtoRespuesta Post()
        {
            using (var context = new EmsysContext())
            {
                var request = HttpContext.Current.Request;
                if (request.Files.Count > 0)
                {
                    foreach (string file in request.Files)
                    {
                        var postedFile = request.Files[file];
                        Imagen img = new Imagen() { Nombre = postedFile.FileName, FechaEnvio = DateTime.Now, Usuario = context.Users.FirstOrDefault() };

                        using (MemoryStream ms = new MemoryStream())
                        {
                            postedFile.InputStream.CopyTo(ms);
                            //img.DatosImagen = ms.ToArray();
                        }
                        context.Imagenes.Add(img);
                        context.SaveChanges();
                        //var filePath = HttpContext.Current.Server.MapPath(string.Format("~/Multimedia/{0}", postedFile.FileName));
                        //postedFile.SaveAs(filePath);
                    }
                    return new DtoRespuesta(0, null);
                }
                else
                    return new DtoRespuesta(1, null);
            }
            // este codgio de aabajo tambien funciona pero me parecio mejor el de arriba.

            //if (!Request.Content.IsMimeMultipartContent())
            //    throw new Exception();

            //var provider = new MultipartMemoryStreamProvider();
            //await Request.Content.ReadAsMultipartAsync(provider);

            //var file = provider.Contents.First();
            //var filename = file.Headers.ContentDisposition.FileName.Trim('\"');
            //var buffer = await file.ReadAsByteArrayAsync();
            //var stream = new MemoryStream(buffer);

            //string path = AppDomain.CurrentDomain.BaseDirectory + "Multimedia\\" + filename;
            //using (var fileStream = System.IO.File.Create(path))
            //{
            //    stream.CopyTo(fileStream);
            //}
            //return Ok(true);
        }

        
        [Route("getFile")]
        [HttpGet]
        /// <summary>
        /// Utiliza el formato octet-stream.
        /// </summary>
        /// <returns>Retprma ima respuesta HTTP.</returns>
        public HttpResponseMessage Get()
        {
            using (var context = new EmsysContext())
            {
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                var img = context.Imagenes.FirstOrDefault();
                //var stream = new MemoryStream(img.DatosImagen);
                //result.Content = new StreamContent(stream);
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                result.Content.Headers.ContentDisposition.FileName = img.Nombre;
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                //result.Content.Headers.ContentLength = stream.Length;
                return result;
            }
        }

       /// <summary>
       /// Servicio para poder recibir la geoubicacion.
       /// </summary>
       /// <param name="ubicacion">DataType con los datos necesarios para guardar la ubicacion.</param>
       /// <returns>Retorna respuesta segun formato del documento de interfaz.</returns>
        [CustomAuthorizeAttribute()]
        [LogFilter]
        [Route("adjuntos/adjuntarGeoUbicacion")]
        [HttpPost]
        public DtoRespuesta AdjuntarGeoUbicacion([FromBody] DtoGeoUbicacion ubicacion)
        {
            IMetodos dbAL = new Metodos();
            string token = ObtenerToken.GetToken(Request);
            try
            {
                if (token == null)
                {
                    return new DtoRespuesta(2, new Mensaje(Mensajes.UsuarioNoAutenticado));
                }
                if (dbAL.adjuntarGeoUbicacion(token, ubicacion))
                {
                    return new DtoRespuesta(0, null);
                }
                return new DtoRespuesta(10, new Mensaje(Mensajes.ExtensionInvalida));
            }
            catch (InvalidTokenException)
            {
                return new DtoRespuesta(2, new Mensaje(Mensajes.TokenInvalido));
            }
            catch (Exception e)
            {
                dbAL.AgregarLogError(token, "", "Emsys.ServiceLayer", "ListarEventosController", 0, "ListarEventos", "Hubo un error al intentar listar eventos de un usuario, se adjunta excepcion: " + e.Message, Mensajes.ErrorIniciarSesionCod);
                return new DtoRespuesta(2, new Mensaje(Mensajes.UsuarioNoAutenticado));
            }
        }

    }
}
