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
        [CustomAuthorizeAttribute("adjuntarMultimedia")]
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
        public DtoRespuesta AdjuntarImagen()
        {
            IMetodos dbAL = new Metodos();
            string token = ObtenerToken.GetToken(Request);
            try
            {
                if (token == null)
                {
                    return new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado));
                }
                var request = HttpContext.Current.Request;
                if (request.Files.Count == 0)
                {
                    return new DtoRespuesta(MensajesParaFE.ErrorEnviarArchivoCod, new Mensaje(MensajesParaFE.ErrorEnviarArchivo));
                }
                var postedFile = request.Files[0];
                string ext = Path.GetExtension(postedFile.FileName);
                if ((ext != ".jpg") && (ext != ".png"))
                {
                    return new DtoRespuesta(MensajesParaFE.FormatoNoSoportadoCod, new Mensaje(MensajesParaFE.FormatoNoSoportado));
                }
                string[] idExtension = request.Params.GetValues("idExtension");
                if (idExtension.Count() == 0)
                {
                    return new DtoRespuesta(MensajesParaFE.ExtensionInvalidaCod, new Mensaje(MensajesParaFE.ExtensionInvalida));
                }
                byte[] bytesImagen;
                using (MemoryStream ms = new MemoryStream())
                {
                    postedFile.InputStream.CopyTo(ms);
                    bytesImagen = ms.ToArray();
                }
                if (dbAL.adjuntarImagen(token, bytesImagen, ext, Int32.Parse(idExtension[0])))
                {
                    return new DtoRespuesta(MensajesParaFE.CorrectoCod, new Mensaje(MensajesParaFE.Correcto));
                }
                return new DtoRespuesta(MensajesParaFE.ExtensionInvalidaCod, new Mensaje(MensajesParaFE.ExtensionInvalida));
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
        public DtoRespuesta AdjuntarAudio()
        {
            IMetodos dbAL = new Metodos();
            string token = ObtenerToken.GetToken(Request);
            try
            {
                if (token == null)
                {
                    return new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado));
                }
                var request = HttpContext.Current.Request;
                if (request.Files.Count == 0)
                {
                    return new DtoRespuesta(MensajesParaFE.ErrorEnviarArchivoCod, new Mensaje(MensajesParaFE.ErrorEnviarArchivo));
                }
                var postedFile = request.Files[0];
                string ext = Path.GetExtension(postedFile.FileName);
                if ((ext != ".mp3") && (ext != ".wav"))
                {
                    return new DtoRespuesta(MensajesParaFE.FormatoNoSoportadoCod, new Mensaje(MensajesParaFE.FormatoNoSoportado));
                }
                string[] idExtension = request.Params.GetValues("idExtension");
                if (idExtension.Count() == 0)
                {
                    return new DtoRespuesta(MensajesParaFE.ExtensionInvalidaCod, new Mensaje(MensajesParaFE.ExtensionInvalida));
                }
                byte[] bytesAudio;
                using (MemoryStream ms = new MemoryStream())
                {
                    postedFile.InputStream.CopyTo(ms);
                    bytesAudio = ms.ToArray();
                }
                if (dbAL.adjuntarAudio(token, bytesAudio, ext, Int32.Parse(idExtension[0])))
                {
                    return new DtoRespuesta(MensajesParaFE.CorrectoCod, new Mensaje(MensajesParaFE.Correcto));
                }
                return new DtoRespuesta(MensajesParaFE.ExtensionInvalidaCod, new Mensaje(MensajesParaFE.ExtensionInvalida));
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
        public DtoRespuesta AdjuntarVideo()
        {
            IMetodos dbAL = new Metodos();
            string token = ObtenerToken.GetToken(Request);
            try
            {
                if (token == null)
                {
                    return new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado));
                }
                var request = HttpContext.Current.Request;
                if (request.Files.Count == 0)
                {
                    return new DtoRespuesta(MensajesParaFE.ErrorEnviarArchivoCod, new Mensaje(MensajesParaFE.ErrorEnviarArchivo));
                }
                var postedFile = request.Files[0];
                string ext = Path.GetExtension(postedFile.FileName);
                if ((ext != ".mp4") && (ext != ".avi"))
                {
                    return new DtoRespuesta(MensajesParaFE.FormatoNoSoportadoCod, new Mensaje(MensajesParaFE.FormatoNoSoportado));
                }
                string[] idExtension = request.Params.GetValues("idExtension");
                if (idExtension.Count() == 0)
                {
                    return new DtoRespuesta(MensajesParaFE.ExtensionInvalidaCod, new Mensaje(MensajesParaFE.ExtensionInvalida));
                }
                byte[] bytesVideo;
                using (MemoryStream ms = new MemoryStream())
                {
                    postedFile.InputStream.CopyTo(ms);
                    bytesVideo = ms.ToArray();
                }
                if (dbAL.adjuntarVideo(token, bytesVideo, ext, Int32.Parse(idExtension[0])))
                {
                    return new DtoRespuesta(MensajesParaFE.CorrectoCod, new Mensaje(MensajesParaFE.Correcto));
                }
                return new DtoRespuesta(MensajesParaFE.ExtensionInvalidaCod, new Mensaje(MensajesParaFE.ExtensionInvalida));
            }
            catch (Exception e)
            {
                dbAL.AgregarLogError(token, "", "Emsys.ServiceLayer", "AdjuntosController", 0, "AdjuntarVideo", "Hubo un error al intentar adjuntar un video, se adjunta excepcion: " + e.Message, MensajesParaFE.ErrorAdjuntarVideoCod);
                return new DtoRespuesta(MensajesParaFE.ErrorCod, new Mensaje(MensajesParaFE.ErrorAdjuntarVideo));
            }
        }


        /// <summary>
        /// Retorna el archivo de imagen indicada.
        /// </summary>
        /// <param name="idImagen">Id de la imagen seleccionada</param>
        /// <returns>Un stream con la imagen selecionada</returns>
        [CustomAuthorizeAttribute("adjuntarMultimedia")]
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
                    //responseMessage.Content = new StringContent(JsonConvert.SerializeObject(new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado))));
                    //return responseMessage;
                    return new DtoRespuesta(1, "1");
                }

                var img = dbAL.getImageData(token, idImagen);
                if (img == null)
                {
                    //responseMessage.Content = new StringContent(JsonConvert.SerializeObject(new DtoRespuesta(MensajesParaFE.ImagenInvalidaCod, new Mensaje(MensajesParaFE.ImagenInvalida))));
                    //return responseMessage;      
                    return new DtoRespuesta(2, "2");
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
                //responseMessage.Content = new StringContent(JsonConvert.SerializeObject(new DtoRespuesta(MensajesParaFE.ImagenInvalidaCod, new Mensaje(MensajesParaFE.ImagenInvalida))));
                //return responseMessage;
                return new DtoRespuesta(3, "3");

            }
            catch (UsuarioNoAutorizadoException)
            {
                //responseMessage.Content = new StringContent(JsonConvert.SerializeObject(new DtoRespuesta(MensajesParaFE.UsuarioNoAutorizadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutorizado))));
                //return responseMessage;
                return new DtoRespuesta(4, "4");
            }
            catch (InvalidTokenException)
            {
                //responseMessage.Content = new StringContent(JsonConvert.SerializeObject(new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado))));
                //return responseMessage;
                return new DtoRespuesta(5, "5");
            }
            catch (Exception e)
            {
                //dbAL.AgregarLogError(token, "", "Emsys.ServiceLayer", "AdjuntosController", 0, "GetImageData", "Hubo un error al intentar obtener los datos de una imagen, se adjunta excepcion: " + e.Message, MensajesParaFE.ErrorDescargarArchivoCod);
                //responseMessage.Content = new StringContent(JsonConvert.SerializeObject(new DtoRespuesta(MensajesParaFE.ErrorCod, new Mensaje(MensajesParaFE.ErrorDescargarArchivo))));
                //return responseMessage;
                return new DtoRespuesta(6, "6");
            }
        }

        /// <summary>
        /// Retorna el archivo del video indicado.
        /// </summary>
        /// <param name="idVideo">id del video seleccinado</param>
        /// <returns>Un stream con el video seleccionado</returns>
        [CustomAuthorizeAttribute("adjuntarMultimedia")]
        [LogFilter]
        [Route("adjuntos/getvideodata")]
        [HttpGet]
        public HttpResponseMessage GetVideoData(int idVideo)
        {
            IMetodos dbAL = new Metodos();
            string token = ObtenerToken.GetToken(Request);
            HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                if (token == null)
                {
                    responseMessage.Content = new StringContent(JsonConvert.SerializeObject(new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado))));
                    return responseMessage;
                }

                var vid = dbAL.getVideoData(token, idVideo);
                if (vid == null)
                {
                    responseMessage.Content = new StringContent(JsonConvert.SerializeObject(new DtoRespuesta(MensajesParaFE.VideoInvalidoCod, new Mensaje(MensajesParaFE.VideoInvalido))));
                    return responseMessage;
                }

                var stream = new MemoryStream(vid.file_data);
                responseMessage.Content = new StreamContent(stream);
                responseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                responseMessage.Content.Headers.ContentDisposition.FileName = vid.nombre;
                responseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                responseMessage.Content.Headers.ContentLength = stream.Length;
                return responseMessage;
            }
            catch (VideoInvalidoException)
            {
                responseMessage.Content = new StringContent(JsonConvert.SerializeObject(new DtoRespuesta(MensajesParaFE.VideoInvalidoCod, new Mensaje(MensajesParaFE.VideoInvalido))));
                return responseMessage;
            }
            catch (UsuarioNoAutorizadoException)
            {
                responseMessage.Content = new StringContent(JsonConvert.SerializeObject(new DtoRespuesta(MensajesParaFE.UsuarioNoAutorizadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutorizado))));
                return responseMessage;
            }
            catch (InvalidTokenException)
            {
                responseMessage.Content = new StringContent(JsonConvert.SerializeObject(new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado))));
                return responseMessage;
            }
            catch (Exception e)
            {
                dbAL.AgregarLogError(token, "", "Emsys.ServiceLayer", "AdjuntosController", 0, "GetVideoData", "Hubo un error al intentar obtener los datos de un video, se adjunta excepcion: " + e.Message, MensajesParaFE.ErrorDescargarArchivoCod);
                responseMessage.Content = new StringContent(JsonConvert.SerializeObject(new DtoRespuesta(MensajesParaFE.ErrorCod, new Mensaje(MensajesParaFE.ErrorDescargarArchivo))));
                return responseMessage;
            }
        }

        /// <summary>
        /// Retorna el archivo de audio indicado.
        /// </summary>
        /// <param name="idAudio">Id del audio seleccionado</param>
        /// <returns>Un stream con el audio seleccionado</returns>
        [CustomAuthorizeAttribute("adjuntarMultimedia")]
        [LogFilter]
        [Route("adjuntos/getaduiodata")]
        [HttpGet]
        public HttpResponseMessage GetAudioData(int idAudio)
        {
            IMetodos dbAL = new Metodos();
            string token = ObtenerToken.GetToken(Request);
            HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                if (token == null)
                {
                    responseMessage.Content = new StringContent(JsonConvert.SerializeObject(new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado))));
                    return responseMessage;
                }

                var aud = dbAL.getAudioData(token, idAudio);
                if (aud == null)
                {
                    responseMessage.Content = new StringContent(JsonConvert.SerializeObject(new DtoRespuesta(MensajesParaFE.AudioInvalidoCod, new Mensaje(MensajesParaFE.AudioInvalido))));
                    return responseMessage;
                }

                var stream = new MemoryStream(aud.file_data);
                responseMessage.Content = new StreamContent(stream);
                responseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                responseMessage.Content.Headers.ContentDisposition.FileName = aud.nombre;
                responseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                responseMessage.Content.Headers.ContentLength = stream.Length;
                return responseMessage;
            }
            catch (AudioInvalidoException)
            {
                responseMessage.Content = new StringContent(JsonConvert.SerializeObject(new DtoRespuesta(MensajesParaFE.AudioInvalidoCod, new Mensaje(MensajesParaFE.AudioInvalido))));
                return responseMessage;
            }
            catch (UsuarioNoAutorizadoException)
            {
                responseMessage.Content = new StringContent(JsonConvert.SerializeObject(new DtoRespuesta(MensajesParaFE.UsuarioNoAutorizadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutorizado))));
                return responseMessage;
            }
            catch (InvalidTokenException)
            {
                responseMessage.Content = new StringContent(JsonConvert.SerializeObject(new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado))));
                return responseMessage;
            }
            catch (Exception e)
            {
                dbAL.AgregarLogError(token, "", "Emsys.ServiceLayer", "AdjuntosController", 0, "GetAudioData", "Hubo un error al intentar obtener los datos de un audio, se adjunta excepcion: " + e.Message, MensajesParaFE.ErrorDescargarArchivoCod);
                responseMessage.Content = new StringContent(JsonConvert.SerializeObject(new DtoRespuesta(MensajesParaFE.ErrorCod, new Mensaje(MensajesParaFE.ErrorDescargarArchivo))));
                return responseMessage;
            }
        }
    }
}
