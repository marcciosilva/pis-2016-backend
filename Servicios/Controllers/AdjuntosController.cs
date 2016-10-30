using DataTypeObject;
using Emsys.DataAccesLayer.Core;
using Emsys.DataAccesLayer.Model;
using Emsys.LogicLayer;
using Emsys.LogicLayer.ApplicationExceptions;
using Newtonsoft.Json;
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
        /// Permite enviar una geo ubicacion al servidor e indicar a que evento o extension le pertenece.
        /// </summary>
        /// <param name="ubicacion">Ubicación a adjuntar</param>
        /// <returns>Un DtoRespuesta que indica si se agrego correctamente</returns>
        [CustomAuthorizeAttribute("adjuntarGeoUbicacion")]
        [LogFilter]
        [Route("adjuntos/postgeoubicacion")]
        [HttpPost]
        public DtoRespuesta AdjuntarGeoUbicacion([FromBody] DtoGeoUbicacion ubicacion)
        {
            IMetodos dbAL = new Metodos();
            string token = ObtenerToken.GetToken(Request);
            try
            {
                if (token == null)
                {
                    return new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado));
                }

                if (dbAL.adjuntarGeoUbicacion(token, ubicacion))
                {
                    return new DtoRespuesta(MensajesParaFE.CorrectoCod, null);
                }

                return new DtoRespuesta(MensajesParaFE.ExtensionInvalidaCod, new Mensaje(MensajesParaFE.ExtensionInvalida));
            }
            catch (InvalidTokenException)
            {
                return new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado));
            }
            catch (Exception e)
            {
                dbAL.AgregarLogError(token, "", "Emsys.ServiceLayer", "AdjuntosController", 0, "AdjuntarGeoUbicacion", "Hubo un error al intentar adjuntar una geo ubicacion, se adjunta excepcion: " + e.Message, MensajesParaFE.ErrorAdjuntarGeoUbicacionCod);
                return new DtoRespuesta(MensajesParaFE.ErrorCod, new Mensaje(MensajesParaFE.ErrorAdjuntarGeoUbicacion));
            }
        }


        /// <summary>
        /// Agrega un archivo de imagen a una extension del servidor, recibe la imagen y el id de la extension como multipart form data.
        /// </summary>
        /// <returns></returns>
        [CustomAuthorizeAttribute("adjuntarMultimedia")]
        [LogFilter]
        [HttpPost]
        [Route("adjuntos/adjuntarimagen")]
        public DtoRespuesta AdjuntarImagen([FromBody] DtoApplicationFile img)
        {
            IMetodos dbAL = new Metodos();
            string token = ObtenerToken.GetToken(Request);
            try
            {
                if (token == null)
                {
                    return new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado));
                }
                if (img == null)
                {
                    return new DtoRespuesta(MensajesParaFE.ErrorEnviarArchivoCod, new Mensaje(MensajesParaFE.ErrorEnviarArchivo));
                }
                if (dbAL.adjuntarImagen(token, img))
                {
                    return new DtoRespuesta(MensajesParaFE.CorrectoCod, new Mensaje(MensajesParaFE.Correcto));
                }
                return new DtoRespuesta(MensajesParaFE.ExtensionInvalidaCod, new Mensaje(MensajesParaFE.ExtensionInvalida));
                //var request = HttpContext.Current.Request;
                //if (request.Files.Count == 0)
                //{
                //    return new DtoRespuesta(MensajesParaFE.ErrorEnviarArchivoCod, new Mensaje(MensajesParaFE.ErrorEnviarArchivo));
                //}
                //var postedFile = request.Files[0];
                //string ext = Path.GetExtension(postedFile.FileName);
                //if ((ext != ".jpg") && (ext != ".png"))
                //{
                //    return new DtoRespuesta(MensajesParaFE.FormatoNoSoportadoCod, new Mensaje(MensajesParaFE.FormatoNoSoportado));
                //}
                //string[] idExtension = request.Params.GetValues("idExtension");
                //if (idExtension.Count() == 0)
                //{
                //    return new DtoRespuesta(MensajesParaFE.ExtensionInvalidaCod, new Mensaje(MensajesParaFE.ExtensionInvalida));
                //}
                //byte[] bytesImagen;
                //using (MemoryStream ms = new MemoryStream())
                //{
                //    postedFile.InputStream.CopyTo(ms);
                //    bytesImagen = ms.ToArray();
                //}
                //if (dbAL.adjuntarImagen(token, bytesImagen, ext, Int32.Parse(idExtension[0])))
                //{
                //    return new DtoRespuesta(MensajesParaFE.CorrectoCod, new Mensaje(MensajesParaFE.Correcto));
                //}
                //return new DtoRespuesta(MensajesParaFE.ExtensionInvalidaCod, new Mensaje(MensajesParaFE.ExtensionInvalida));
            }
            catch (FormatoInvalidoException e)
            {
                return new DtoRespuesta(MensajesParaFE.FormatoNoSoportadoCod, new Mensaje(MensajesParaFE.FormatoNoSoportado));
            }
            catch (Exception e)
            {
                dbAL.AgregarLogError(token, "", "Emsys.ServiceLayer", "AdjuntosController", 0, "AdjuntarImagen", "Hubo un error al intentar adjuntar una imagen, se adjunta excepcion: " + e.Message, MensajesParaFE.ErrorAdjuntarImagenCod);
                return new DtoRespuesta(MensajesParaFE.ErrorCod, new Mensaje(MensajesParaFE.ErrorAdjuntarImagen));
            }
        }


        /// <summary>
        /// Agrega un archivo de audio a una extension del servidor, recibe el audio y el id de la extension como multipart form data.
        /// </summary>
        /// <returns></returns>
        [CustomAuthorizeAttribute("adjuntarMultimedia")]
        [LogFilter]
        [HttpPost]
        [Route("adjuntos/adjuntaraudio")]
        public DtoRespuesta AdjuntarAudio([FromBody] DtoApplicationFile aud)
        {
            IMetodos dbAL = new Metodos();
            string token = ObtenerToken.GetToken(Request);
            try
            {
                if (token == null)
                {
                    return new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado));
                }
                if (aud == null)
                {
                    return new DtoRespuesta(MensajesParaFE.ErrorEnviarArchivoCod, new Mensaje(MensajesParaFE.ErrorEnviarArchivo));
                }
                if (dbAL.adjuntarAudio(token, aud))
                {
                    return new DtoRespuesta(MensajesParaFE.CorrectoCod, new Mensaje(MensajesParaFE.Correcto));
                }
                return new DtoRespuesta(MensajesParaFE.ExtensionInvalidaCod, new Mensaje(MensajesParaFE.ExtensionInvalida));
                //var request = HttpContext.Current.Request;
                //if (request.Files.Count == 0)
                //{
                //    return new DtoRespuesta(MensajesParaFE.ErrorEnviarArchivoCod, new Mensaje(MensajesParaFE.ErrorEnviarArchivo));
                //}
                //var postedFile = request.Files[0];
                //string ext = Path.GetExtension(postedFile.FileName);
                //if ((ext != ".mp3") && (ext != ".wav"))
                //{
                //    return new DtoRespuesta(MensajesParaFE.FormatoNoSoportadoCod, new Mensaje(MensajesParaFE.FormatoNoSoportado));
                //}
                //string[] idExtension = request.Params.GetValues("idExtension");
                //if (idExtension.Count() == 0)
                //{
                //    return new DtoRespuesta(MensajesParaFE.ExtensionInvalidaCod, new Mensaje(MensajesParaFE.ExtensionInvalida));
                //}
                //byte[] bytesAudio;
                //using (MemoryStream ms = new MemoryStream())
                //{
                //    postedFile.InputStream.CopyTo(ms);
                //    bytesAudio = ms.ToArray();
                //}
                //if (dbAL.adjuntarAudio(token, bytesAudio, ext, Int32.Parse(idExtension[0])))
                //{
                //    return new DtoRespuesta(MensajesParaFE.CorrectoCod, new Mensaje(MensajesParaFE.Correcto));
                //}
                //return new DtoRespuesta(MensajesParaFE.ExtensionInvalidaCod, new Mensaje(MensajesParaFE.ExtensionInvalida));
            }
            catch (FormatoInvalidoException e)
            {
                return new DtoRespuesta(MensajesParaFE.FormatoNoSoportadoCod, new Mensaje(MensajesParaFE.FormatoNoSoportado));
            }
            catch (Exception e)
            {
                dbAL.AgregarLogError(token, "", "Emsys.ServiceLayer", "AdjuntosController", 0, "AdjuntarAudio", "Hubo un error al intentar adjuntar un audio, se adjunta excepcion: " + e.Message, MensajesParaFE.ErrorAdjuntarAudioCod);
                return new DtoRespuesta(MensajesParaFE.ErrorCod, new Mensaje(MensajesParaFE.ErrorAdjuntarAudio));
            }
        }


        /// <summary>
        /// Agrega un archivo de video a una extension del servidor, recibe el video y el id de la extension como multipart form data.
        /// </summary>
        /// <returns></returns>
        [CustomAuthorizeAttribute("adjuntarMultimedia")]
        [LogFilter]
        [HttpPost]
        [Route("adjuntos/adjuntarvideo")]
        public DtoRespuesta AdjuntarVideo([FromBody] DtoApplicationFile vid)
        {
            IMetodos dbAL = new Metodos();
            string token = ObtenerToken.GetToken(Request);
            try
            {
                if (token == null)
                {
                    return new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado));
                }
                if (vid == null)
                {
                    return new DtoRespuesta(MensajesParaFE.ErrorEnviarArchivoCod, new Mensaje(MensajesParaFE.ErrorEnviarArchivo));
                }
                if (dbAL.adjuntarVideo(token, vid))
                {
                    return new DtoRespuesta(MensajesParaFE.CorrectoCod, new Mensaje(MensajesParaFE.Correcto));
                }
                return new DtoRespuesta(MensajesParaFE.ExtensionInvalidaCod, new Mensaje(MensajesParaFE.ExtensionInvalida));
                //var request = HttpContext.Current.Request;
                //if (request.Files.Count == 0)
                //{
                //    return new DtoRespuesta(MensajesParaFE.ErrorEnviarArchivoCod, new Mensaje(MensajesParaFE.ErrorEnviarArchivo));
                //}
                //var postedFile = request.Files[0];
                //string ext = Path.GetExtension(postedFile.FileName);
                //if ((ext != ".mp4") && (ext != ".avi"))
                //{
                //    return new DtoRespuesta(MensajesParaFE.FormatoNoSoportadoCod, new Mensaje(MensajesParaFE.FormatoNoSoportado));
                //}
                //string[] idExtension = request.Params.GetValues("idExtension");
                //if (idExtension.Count() == 0)
                //{
                //    return new DtoRespuesta(MensajesParaFE.ExtensionInvalidaCod, new Mensaje(MensajesParaFE.ExtensionInvalida));
                //}
                //byte[] bytesVideo;
                //using (MemoryStream ms = new MemoryStream())
                //{
                //    postedFile.InputStream.CopyTo(ms);
                //    bytesVideo = ms.ToArray();
                //}
                //if (dbAL.adjuntarVideo(token, bytesVideo, ext, Int32.Parse(idExtension[0])))
                //{
                //    return new DtoRespuesta(MensajesParaFE.CorrectoCod, new Mensaje(MensajesParaFE.Correcto));
                //}
                //return new DtoRespuesta(MensajesParaFE.ExtensionInvalidaCod, new Mensaje(MensajesParaFE.ExtensionInvalida));
            }
            catch (FormatoInvalidoException e)
            {
                return new DtoRespuesta(MensajesParaFE.FormatoNoSoportadoCod, new Mensaje(MensajesParaFE.FormatoNoSoportado));
            }
            catch (Exception e)
            {
                dbAL.AgregarLogError(token, "", "Emsys.ServiceLayer", "AdjuntosController", 0, "AdjuntarVideo", "Hubo un error al intentar adjuntar un video, se adjunta excepcion: " + e.Message, MensajesParaFE.ErrorAdjuntarVideoCod);
                return new DtoRespuesta(MensajesParaFE.ErrorCod, new Mensaje(MensajesParaFE.ErrorAdjuntarVideo));
            }
        }


        /// <summary>
        /// Servicio para obtener un archivo de imagen.
        /// </summary>
        /// <param name="idImagen">id de la imagen solicitada</param>
        /// <returns>Dto con el nombre y los bytes del archivo</returns>
        [CustomAuthorizeAttribute("verMultimedia")]
        [LogFilter]
        [Route("adjuntos/getimagedata")]
        [HttpGet]
        public DtoRespuesta GetImageData(int idImagen)
        {
            IMetodos dbAL = new Metodos();
            string token = ObtenerToken.GetToken(Request);
            HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                if (token == null)
                {
                    return new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado));
                }

                var img = dbAL.getImageData(token, idImagen);
                if (img == null)
                {    
                    return new DtoRespuesta(MensajesParaFE.ImagenInvalidaCod, new Mensaje(MensajesParaFE.ImagenInvalida));
                }
                //var stream = new MemoryStream(img.file_data);
                //responseMessage.Content = new StreamContent(stream);
                //responseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                //responseMessage.Content.Headers.ContentDisposition.FileName = img.nombre;
                //responseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                //responseMessage.Content.Headers.ContentLength = stream.Length;
                //return responseMessage;
                return new DtoRespuesta(MensajesParaFE.CorrectoCod, img);
            }
            catch (ImagenInvalidaException)
            {
                return new DtoRespuesta(MensajesParaFE.ImagenInvalidaCod, new Mensaje(MensajesParaFE.ImagenInvalida));
            }
            catch (UsuarioNoAutorizadoException)
            {
                return new DtoRespuesta(MensajesParaFE.UsuarioNoAutorizadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutorizado));
            }
            catch (InvalidTokenException)
            {
                return new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado));
            }
            catch (Exception e)
            {
                dbAL.AgregarLogError(token, "", "Emsys.ServiceLayer", "AdjuntosController", 0, "GetImageData", "Hubo un error al intentar obtener los datos de una imagen, se adjunta excepcion: " + e.Message, MensajesParaFE.ErrorDescargarArchivoCod);
                return new DtoRespuesta(MensajesParaFE.ErrorCod, new Mensaje(MensajesParaFE.ErrorDescargarArchivo));
            }
        }

        /// <summary>
        /// Servicio para obtener un archivo de video.
        /// </summary>
        /// <param name="idVideo">Id del video solicitado</param>
        /// <returns>Dto con el nombre y los bytes del archivo</returns>
        [CustomAuthorizeAttribute("verMultimedia")]
        [LogFilter]
        [Route("adjuntos/getvideodata")]
        [HttpGet]
        public DtoRespuesta GetVideoData(int idVideo)
        {
            IMetodos dbAL = new Metodos();
            string token = ObtenerToken.GetToken(Request);
            HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                if (token == null)
                {
                    return new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado));
                }

                var vid = dbAL.getVideoData(token, idVideo);
                if (vid == null)
                {
                    return new DtoRespuesta(MensajesParaFE.VideoInvalidoCod, new Mensaje(MensajesParaFE.VideoInvalido));
                }
                //var stream = new MemoryStream(vid.file_data);
                //responseMessage.Content = new StreamContent(stream);
                //responseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                //responseMessage.Content.Headers.ContentDisposition.FileName = vid.nombre;
                //responseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                //responseMessage.Content.Headers.ContentLength = stream.Length;
                return new DtoRespuesta(MensajesParaFE.CorrectoCod, vid);
            }
            catch (VideoInvalidoException)
            {
                return new DtoRespuesta(MensajesParaFE.VideoInvalidoCod, new Mensaje(MensajesParaFE.VideoInvalido));
            }
            catch (UsuarioNoAutorizadoException)
            {
                return new DtoRespuesta(MensajesParaFE.UsuarioNoAutorizadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutorizado));
            }
            catch (InvalidTokenException)
            {
                return new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado));
            }
            catch (Exception e)
            {
                dbAL.AgregarLogError(token, "", "Emsys.ServiceLayer", "AdjuntosController", 0, "GetVideoData", "Hubo un error al intentar obtener los datos de un video, se adjunta excepcion: " + e.Message, MensajesParaFE.ErrorDescargarArchivoCod);
                return new DtoRespuesta(MensajesParaFE.ErrorCod, new Mensaje(MensajesParaFE.ErrorDescargarArchivo));
            }
        }

        /// <summary>
        /// Servicio para obtener un archivo de audio.
        /// </summary>
        /// <param name="idAudio">Id del audio solicitado</param>
        /// <returns>Dto con el nombre y los bytes del archivo</returns>
        [CustomAuthorizeAttribute("verMultimedia")]
        [LogFilter]
        [Route("adjuntos/getaduiodata")]
        [HttpGet]
        public DtoRespuesta GetAudioData(int idAudio)
        {
            IMetodos dbAL = new Metodos();
            string token = ObtenerToken.GetToken(Request);
            HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                if (token == null)
                {
                    return new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado));
                }

                var aud = dbAL.getAudioData(token, idAudio);
                if (aud == null)
                {
                    return new DtoRespuesta(MensajesParaFE.AudioInvalidoCod, new Mensaje(MensajesParaFE.AudioInvalido));
                }
                //var stream = new MemoryStream(aud.file_data);
                //responseMessage.Content = new StreamContent(stream);
                //responseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                //responseMessage.Content.Headers.ContentDisposition.FileName = aud.nombre;
                //responseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                //responseMessage.Content.Headers.ContentLength = stream.Length;
                //return responseMessage;
                return new DtoRespuesta(MensajesParaFE.CorrectoCod, aud);
            }
            catch (AudioInvalidoException)
            {
                return new DtoRespuesta(MensajesParaFE.AudioInvalidoCod, new Mensaje(MensajesParaFE.AudioInvalido));
            }
            catch (UsuarioNoAutorizadoException)
            {
                return new DtoRespuesta(MensajesParaFE.UsuarioNoAutorizadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutorizado));
            }
            catch (InvalidTokenException)
            {
                return new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado));
            }
            catch (Exception e)
            {
                dbAL.AgregarLogError(token, "", "Emsys.ServiceLayer", "AdjuntosController", 0, "GetAudioData", "Hubo un error al intentar obtener los datos de un audio, se adjunta excepcion: " + e.Message, MensajesParaFE.ErrorDescargarArchivoCod);
                return new DtoRespuesta(MensajesParaFE.ErrorCod, new Mensaje(MensajesParaFE.ErrorDescargarArchivo));
            }
        }
    }
}
