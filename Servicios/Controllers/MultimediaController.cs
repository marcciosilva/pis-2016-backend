using DataTypeObject;
using Emsys.DataAccesLayer.Core;
using Emsys.DataAccesLayer.Model;
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
    public class MultimediaController : ApiController
    {
        /// <summary>
        /// Los archivos esperados son del formato MIME
        /// </summary>
        /// <returns></returns>
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
                            img.imagen = ms.ToArray();
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

        /// <summary>
        /// Utiliza el formato octet-stream
        /// </summary>
        /// <param name="nombreArchivo"></param>
        /// <returns></returns>
        [Route("getFile")]
        [HttpGet]
        public HttpResponseMessage Get()
        {
            using (var context = new EmsysContext())
            {
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                var img = context.Imagenes.FirstOrDefault();
                var stream = new MemoryStream(img.imagen);
                result.Content = new StreamContent(stream);
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                result.Content.Headers.ContentDisposition.FileName = img.Nombre;
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                result.Content.Headers.ContentLength = stream.Length;
                return result;
            }
        }

    }
}
