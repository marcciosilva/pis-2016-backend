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
        public IHttpActionResult Post()
        {
            var request = HttpContext.Current.Request;
            if (request.Files.Count > 0)
            {
                foreach (string file in request.Files)
                {
                    var postedFile = request.Files[file];

                    var filePath = HttpContext.Current.Server.MapPath(string.Format("~/Multimedia/{0}", postedFile.FileName));
                    postedFile.SaveAs(filePath);
                }
                return Ok(true);
            }
            else
                return BadRequest();
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
        [HttpGet]
        public HttpResponseMessage Get(string nombreArchivo)
        {
            var path = System.Web.HttpContext.Current.Server.MapPath("~/Multimedia/+nombreArchivo"); ;
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new FileStream(path, FileMode.Open);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            result.Content.Headers.ContentDisposition.FileName = Path.GetFileName(path);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentLength = stream.Length;
            return result;
        }

    }
}
