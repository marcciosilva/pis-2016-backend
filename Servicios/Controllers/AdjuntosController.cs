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
        /// Permite enviar en archivo de imagen al servidor.
        /// </summary>
        /// <returns>Un DtoRespuesta que contiene un int, necesario para asociar el archivo enviado a un evento o extension</returns>
        [CustomAuthorizeAttribute("adjuntarMultimedia")]
        [LogFilter]
        [HttpPost]
        [Route("adjuntos/postimagefile")]
        public DtoRespuesta PostImageFile()
        {
            IMetodos dbAL = new Metodos();
            string token = ObtenerToken.GetToken(Request);
            try
            {
                if (token == null)
                {
                    return new DtoRespuesta(2, new Mensaje(Mensajes.UsuarioNoAutenticado));
                }
                var request = HttpContext.Current.Request;
                if (request.Files.Count > 0)
                {
                    var postedFile = request.Files[0];
                    string ext = Path.GetExtension(postedFile.FileName);
                    if ((ext == ".jpg") || (ext == ".png"))
                    {
                        byte[] bytesImagen;
                        using (MemoryStream ms = new MemoryStream())
                        {
                            postedFile.InputStream.CopyTo(ms);
                            bytesImagen = ms.ToArray();
                        }
                        int cod = dbAL.agregarFileData(bytesImagen, ext);
                        return new DtoRespuesta(0, cod);
                    }        
                    return new DtoRespuesta(13, new Mensaje(Mensajes.FormatoNoSoportado));
                }
                return new DtoRespuesta(12, new Mensaje(Mensajes.ErrorEnviarArchivo));
            }
            catch (Exception e)
            {
                dbAL.AgregarLogError(token, "", "Emsys.ServiceLayer", "AdjuntosController", 0, "PostFile", "Hubo un error al intentar enviar un archivo, se adjunta excepcion: " + e.Message, Mensajes.ErrorIniciarSesionCod);
                return new DtoRespuesta(500, new Mensaje(Mensajes.ErrorEnviarArchivo));
            }
        }

        /// <summary>
        /// Permite enviar en archivo de video al servidor.
        /// </summary>
        /// <returns>Un DtoRespuesta que contiene un int, necesario para asociar el archivo enviado a un evento o extension</returns>
        [CustomAuthorizeAttribute("adjuntarMultimedia")]
        [LogFilter]
        [HttpPost]
        [Route("adjuntos/postvideofile")]
        public DtoRespuesta PostVideoFile()
        {
            IMetodos dbAL = new Metodos();
            string token = ObtenerToken.GetToken(Request);
            try
            {
                if (token == null)
                {
                    return new DtoRespuesta(2, new Mensaje(Mensajes.UsuarioNoAutenticado));
                }

                var request = HttpContext.Current.Request;
                if (request.Files.Count > 0)
                {
                    var postedFile = request.Files[0];
                    string ext = Path.GetExtension(postedFile.FileName);
                    if ((ext == ".avi") || (ext == ".mp4"))
                    {
                        byte[] bytesImagen;
                        using (MemoryStream ms = new MemoryStream())
                        {
                            postedFile.InputStream.CopyTo(ms);
                            bytesImagen = ms.ToArray();
                        }
                        int cod = dbAL.agregarFileData(bytesImagen, ext);
                        return new DtoRespuesta(0, cod);
                    }
                    return new DtoRespuesta(13, new Mensaje(Mensajes.FormatoNoSoportado));
                }
                return new DtoRespuesta(12, new Mensaje(Mensajes.ErrorEnviarArchivo));
            }
            catch (Exception e)
            {
                dbAL.AgregarLogError(token, "", "Emsys.ServiceLayer", "AdjuntosController", 0, "PostFile", "Hubo un error al intentar enviar un archivo, se adjunta excepcion: " + e.Message, Mensajes.ErrorIniciarSesionCod);
                return new DtoRespuesta(500, new Mensaje(Mensajes.ErrorEnviarArchivo));
            }
        }

        /// <summary>
        /// Permite enviar en archivo de audio al servidor.
        /// </summary>
        /// <returns>Un DtoRespuesta que contiene un int, necesario para asociar el archivo enviado a un evento o extension</returns>
        [CustomAuthorizeAttribute("adjuntarMultimedia")]
        [LogFilter]
        [HttpPost]
        [Route("adjuntos/postaudiofile")]
        public DtoRespuesta PostAudioFile()
        {
            IMetodos dbAL = new Metodos();
            string token = ObtenerToken.GetToken(Request);
            try
            {
                if (token == null)
                {
                    return new DtoRespuesta(2, new Mensaje(Mensajes.UsuarioNoAutenticado));
                }

                var request = HttpContext.Current.Request;
                if (request.Files.Count > 0)
                {
                    var postedFile = request.Files[0];
                    string ext = Path.GetExtension(postedFile.FileName);
                    if ((ext == ".mp3") || (ext == ".wav"))
                    {
                        byte[] bytesImagen;
                        using (MemoryStream ms = new MemoryStream())
                        {
                            postedFile.InputStream.CopyTo(ms);
                            bytesImagen = ms.ToArray();
                        }
                        int cod = dbAL.agregarFileData(bytesImagen, ext);
                        return new DtoRespuesta(0, cod);
                    }
                    return new DtoRespuesta(13, new Mensaje(Mensajes.FormatoNoSoportado));
                }
                return new DtoRespuesta(12, new Mensaje(Mensajes.ErrorEnviarArchivo));
            }
            catch (Exception e)
            {
                dbAL.AgregarLogError(token, "", "Emsys.ServiceLayer", "AdjuntosController", 0, "PostFile", "Hubo un error al intentar enviar un archivo, se adjunta excepcion: " + e.Message, Mensajes.ErrorIniciarSesionCod);
                return new DtoRespuesta(500, new Mensaje(Mensajes.ErrorEnviarArchivo));
            }
        }

        /// <summary>
        /// Permite enviar una geo ubicacion al servidor e indicar a que evento o extension le pertenece.
        /// </summary>
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
                dbAL.AgregarLogError(token, "", "Emsys.ServiceLayer", "AdjuntosController", 0, "AdjuntarGeoUbicacion", "Hubo un error al intentar adjuntar una geo ubicacion, se adjunta excepcion: " + e.Message, Mensajes.ErrorIniciarSesionCod);
                return new DtoRespuesta(2, new Mensaje(Mensajes.UsuarioNoAutenticado));
            }
        }

        /// <summary>
        /// Agrega un vdieo a un evento o extension.
        /// </summary>
        /// <param name="imagen">DtoImagen que indica a que evento o extension agregar la imagen, y el id de imagen, agregado previamente con "PostImageFile"</param>
        /// <returns>Un DtoRespuesta que indica si se agrego correctamente</returns>
        [CustomAuthorizeAttribute("adjuntarMultimedia")]
        [LogFilter]
        [Route("adjuntos/adjuntarImagen")]
        [HttpPost]
        public DtoRespuesta AdjuntarImagen([FromBody] DtoImagen imagen)
        {
            IMetodos dbAL = new Metodos();
            string token = ObtenerToken.GetToken(Request);
            try
            {
                if (token == null)
                {
                    return new DtoRespuesta(2, new Mensaje(Mensajes.UsuarioNoAutenticado));
                }
                if (dbAL.adjuntarImagen(token, imagen))
                {
                    return new DtoRespuesta(0, null);
                }
                return new DtoRespuesta(10, new Mensaje(Mensajes.ExtensionInvalida));
            }
            catch (InvalidTokenException e)
            {
                return new DtoRespuesta(2, new Mensaje(Mensajes.TokenInvalido));
            }
            catch (Exception e)
            {
                dbAL.AgregarLogError(token, "", "Emsys.ServiceLayer", "AdjuntosController", 0, "AdjuntarImagen", "Hubo un error al intentar adjuntar una imagen, se adjunta excepcion: " + e.Message, Mensajes.ErrorIniciarSesionCod);
                return new DtoRespuesta(500, new Mensaje(Mensajes.UsuarioNoAutenticado));
            }
        }


        /// <summary>
        /// Agrega un vdieo a un evento o extension.
        /// </summary>
        /// <param name="video">DtoVideo que indica a que evento o extension agregar el video, y el id del archivo de video, agregado previamente con "PostVideoFile"</param>
        /// <returns>Un DtoRespuesta que indica si se agrego correctamente</returns>
        [CustomAuthorizeAttribute("adjuntarMultimedia")]
        [LogFilter]
        [Route("adjuntos/adjuntarVideo")]
        [HttpPost]
        public DtoRespuesta AdjuntarVideo([FromBody] DtoVideo video)
        {
            IMetodos dbAL = new Metodos();
            string token = ObtenerToken.GetToken(Request);
            try
            {
                if (token == null)
                {
                    return new DtoRespuesta(2, new Mensaje(Mensajes.UsuarioNoAutenticado));
                }
                if (dbAL.adjuntarVideo(token, video))
                {
                    return new DtoRespuesta(0, null);
                }
                return new DtoRespuesta(10, new Mensaje(Mensajes.ExtensionInvalida));
            }
            catch (InvalidTokenException e)
            {
                return new DtoRespuesta(2, new Mensaje(Mensajes.TokenInvalido));
            }
            catch (Exception e)
            {
                dbAL.AgregarLogError(token, "", "Emsys.ServiceLayer", "AdjuntosController", 0, "AdjuntarImagen", "Hubo un error al intentar adjuntar una imagen, se adjunta excepcion: " + e.Message, Mensajes.ErrorIniciarSesionCod);
                return new DtoRespuesta(500, new Mensaje(Mensajes.UsuarioNoAutenticado));
            }
        }

        /// <summary>
        /// Agrega un audio a un evento o extension.
        /// </summary>
        /// <param name="audio">DtoAudio que indica a que evento o extension agregar el audio, y el id del archivo de audio, agregado previamente con "PostVideoFile"</param>
        /// <returns>Un DtoRespuesta que indica si se agrego correctamente</returns>
        [CustomAuthorizeAttribute("adjuntarMultimedia")]
        [LogFilter]
        [Route("adjuntos/adjuntarAudio")]
        [HttpPost]
        public DtoRespuesta AdjuntarAudio([FromBody] DtoAudio audio)
        {
            IMetodos dbAL = new Metodos();
            string token = ObtenerToken.GetToken(Request);
            try
            {
                if (token == null)
                {
                    return new DtoRespuesta(2, new Mensaje(Mensajes.UsuarioNoAutenticado));
                }
                if (dbAL.adjuntarAudio(token, audio))
                {
                    return new DtoRespuesta(0, null);
                }
                return new DtoRespuesta(10, new Mensaje(Mensajes.ExtensionInvalida));
            }
            catch (InvalidTokenException e)
            {
                return new DtoRespuesta(2, new Mensaje(Mensajes.TokenInvalido));
            }
            catch (Exception e)
            {
                dbAL.AgregarLogError(token, "", "Emsys.ServiceLayer", "AdjuntosController", 0, "AdjuntarImagen", "Hubo un error al intentar adjuntar una imagen, se adjunta excepcion: " + e.Message, Mensajes.ErrorIniciarSesionCod);
                return new DtoRespuesta(500, new Mensaje(Mensajes.UsuarioNoAutenticado));
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
        public HttpResponseMessage GetImageData(int idImagen)
        {
            IMetodos dbAL = new Metodos();
            string token = ObtenerToken.GetToken(Request);
            HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                if (token == null)
                {
                    responseMessage.Content = new StringContent(JsonConvert.SerializeObject(new DtoRespuesta(2, new Mensaje(Mensajes.UsuarioNoAutenticado))));
                    return responseMessage;
                }
                var img = dbAL.getImageData(token, idImagen);
                if(img == null)
                {
                    responseMessage.Content = new StringContent(JsonConvert.SerializeObject(new DtoRespuesta(15, new Mensaje(Mensajes.ImagenInvalida))));
                    return responseMessage;                   
                }
                var stream = new MemoryStream(img.file_data);
                responseMessage.Content = new StreamContent(stream);
                responseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                responseMessage.Content.Headers.ContentDisposition.FileName = img.nombre;
                responseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                responseMessage.Content.Headers.ContentLength = stream.Length;
                return responseMessage;

            }
            catch (ImagenInvalidaException e)
            {
                responseMessage.Content = new StringContent(JsonConvert.SerializeObject(new DtoRespuesta(15, new Mensaje(Mensajes.ImagenInvalida))));
                return responseMessage;
            }
            catch (UsuarioNoAutorizadoException e)
            {
                responseMessage.Content = new StringContent(JsonConvert.SerializeObject(new DtoRespuesta(14, new Mensaje(Mensajes.UsuarioNoAutorizado))));
                return responseMessage;
            }
            catch (InvalidTokenException e)
            {
                responseMessage.Content = new StringContent(JsonConvert.SerializeObject(new DtoRespuesta(500, new Mensaje(Mensajes.UsuarioNoAutenticado))));
                return responseMessage;
            }
            catch (Exception e)
            {
                dbAL.AgregarLogError(token, "", "Emsys.ServiceLayer", "AdjuntosController", 0, "AdjuntarImagen", "Hubo un error al intentar adjuntar una imagen, se adjunta excepcion: " + e.Message, Mensajes.ErrorIniciarSesionCod);
                responseMessage.Content = new StringContent(JsonConvert.SerializeObject(new DtoRespuesta(500, new Mensaje(Mensajes.UsuarioNoAutenticado))));
                return responseMessage;
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
                    responseMessage.Content = new StringContent(JsonConvert.SerializeObject(new DtoRespuesta(2, new Mensaje(Mensajes.UsuarioNoAutenticado))));
                    return responseMessage;
                }
                var vid = dbAL.getVideoData(token, idVideo);
                if (vid == null)
                {
                    responseMessage.Content = new StringContent(JsonConvert.SerializeObject(new DtoRespuesta(15, new Mensaje(Mensajes.VideoInvalido))));
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
            catch (ImagenInvalidaException e)
            {
                responseMessage.Content = new StringContent(JsonConvert.SerializeObject(new DtoRespuesta(15, new Mensaje(Mensajes.VideoInvalido))));
                return responseMessage;
            }
            catch (UsuarioNoAutorizadoException e)
            {
                responseMessage.Content = new StringContent(JsonConvert.SerializeObject(new DtoRespuesta(14, new Mensaje(Mensajes.UsuarioNoAutorizado))));
                return responseMessage;
            }
            catch (InvalidTokenException e)
            {
                responseMessage.Content = new StringContent(JsonConvert.SerializeObject(new DtoRespuesta(500, new Mensaje(Mensajes.UsuarioNoAutenticado))));
                return responseMessage;
            }
            catch (Exception e)
            {
                dbAL.AgregarLogError(token, "", "Emsys.ServiceLayer", "AdjuntosController", 0, "AdjuntarImagen", "Hubo un error al intentar adjuntar una imagen, se adjunta excepcion: " + e.Message, Mensajes.ErrorIniciarSesionCod);
                responseMessage.Content = new StringContent(JsonConvert.SerializeObject(new DtoRespuesta(500, new Mensaje(Mensajes.UsuarioNoAutenticado))));
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
                    responseMessage.Content = new StringContent(JsonConvert.SerializeObject(new DtoRespuesta(2, new Mensaje(Mensajes.UsuarioNoAutenticado))));
                    return responseMessage;
                }
                var aud = dbAL.getAudioData(token, idAudio);
                if (aud == null)
                {
                    responseMessage.Content = new StringContent(JsonConvert.SerializeObject(new DtoRespuesta(15, new Mensaje(Mensajes.AudioInvalido))));
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
            catch (ImagenInvalidaException e)
            {
                responseMessage.Content = new StringContent(JsonConvert.SerializeObject(new DtoRespuesta(15, new Mensaje(Mensajes.AudioInvalido))));
                return responseMessage;
            }
            catch (UsuarioNoAutorizadoException e)
            {
                responseMessage.Content = new StringContent(JsonConvert.SerializeObject(new DtoRespuesta(14, new Mensaje(Mensajes.UsuarioNoAutorizado))));
                return responseMessage;
            }
            catch (InvalidTokenException e)
            {
                responseMessage.Content = new StringContent(JsonConvert.SerializeObject(new DtoRespuesta(500, new Mensaje(Mensajes.UsuarioNoAutenticado))));
                return responseMessage;
            }
            catch (Exception e)
            {
                dbAL.AgregarLogError(token, "", "Emsys.ServiceLayer", "AdjuntosController", 0, "AdjuntarImagen", "Hubo un error al intentar adjuntar una imagen, se adjunta excepcion: " + e.Message, Mensajes.ErrorIniciarSesionCod);
                responseMessage.Content = new StringContent(JsonConvert.SerializeObject(new DtoRespuesta(500, new Mensaje(Mensajes.UsuarioNoAutenticado))));
                return responseMessage;
            }
        }
    }
}
