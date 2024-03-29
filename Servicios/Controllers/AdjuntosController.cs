﻿using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using DataTypeObject;
using Emsys.LogicLayer;
using Emsys.LogicLayer.ApplicationExceptions;
using Servicios.Filtros;

namespace Servicios.Controllers
{
    public class AdjuntosController : ApiController
    {
        private const string Modulo = "Emsys.ServiceLayer";

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

                dbAL.adjuntarGeoUbicacion(token, ubicacion);
                return new DtoRespuesta(MensajesParaFE.CorrectoCod, null);
            }
            catch (TokenInvalidoException)
            {
                return new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado));
            }
            catch (ExtensionInvalidaException)
            {
                return new DtoRespuesta(MensajesParaFE.ExtensionInvalidaCod, new Mensaje(MensajesParaFE.ExtensionInvalida));
            }
            catch (UsuarioNoAutorizadoException)
            {
                return new DtoRespuesta(MensajesParaFE.UsuarioNoAutorizadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutorizado));
            }
            catch (Exception e)
            {
                dbAL.AgregarLogError(token, string.Empty, Modulo, "AdjuntosController", 0, "AdjuntarGeoUbicacion", "Hubo un error al intentar adjuntar una geo ubicacion, se adjunta excepcion: " + e.Message, MensajesParaFE.ErrorAdjuntarGeoUbicacionCod);
                return new DtoRespuesta(MensajesParaFE.ErrorCod, new Mensaje(MensajesParaFE.ErrorAdjuntarGeoUbicacion));
            }
        }

        /// <summary>
        /// Agrega un archivo de imagen a una extension del servidor, recibe la imagen y el id de la extension como multipart form data.
        /// </summary>
        /// <returns></returns>
        [CustomAuthorizeAttribute("adjuntarImagen")]
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

                dbAL.adjuntarImagen(token, img);              
                return new DtoRespuesta(MensajesParaFE.CorrectoCod, new Mensaje(MensajesParaFE.Correcto));
            }
            catch (TokenInvalidoException)
            {
                return new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado));
            }
            catch (ExtensionInvalidaException)
            {
                return new DtoRespuesta(MensajesParaFE.ExtensionInvalidaCod, new Mensaje(MensajesParaFE.ExtensionInvalida));
            }
            catch (FormatoInvalidoException)
            {
                return new DtoRespuesta(MensajesParaFE.FormatoNoSoportadoCod, new Mensaje(MensajesParaFE.FormatoNoSoportado));
            }
            catch (ImagenInvalidaException)
            {
                return new DtoRespuesta(MensajesParaFE.ErrorEnviarArchivoCod, new Mensaje(MensajesParaFE.ErrorEnviarArchivo));
            }
            catch (UsuarioNoAutorizadoException)
            {
                return new DtoRespuesta(MensajesParaFE.UsuarioNoAutorizadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutorizado));
            }
            catch (Exception e)
            {
                dbAL.AgregarLogError(token, string.Empty, Modulo, "AdjuntosController", 0, "AdjuntarImagen", "Hubo un error al intentar adjuntar una imagen, se adjunta excepcion: " + e.Message, MensajesParaFE.ErrorAdjuntarImagenCod);
                return new DtoRespuesta(MensajesParaFE.ErrorCod, new Mensaje(MensajesParaFE.ErrorAdjuntarImagen));
            }
        }

        /// <summary>
        /// Agrega un archivo de audio a una extension del servidor, recibe el audio y el id de la extension como multipart form data.
        /// </summary>
        /// <param name="aud"></param>
        /// <returns></returns>
        [CustomAuthorizeAttribute("adjuntarAudio")]
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

                dbAL.adjuntarAudio(token, aud);
                return new DtoRespuesta(MensajesParaFE.CorrectoCod, new Mensaje(MensajesParaFE.Correcto));
            }
            catch (TokenInvalidoException)
            {
                return new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado));
            }
            catch (ExtensionInvalidaException)
            {
                return new DtoRespuesta(MensajesParaFE.ExtensionInvalidaCod, new Mensaje(MensajesParaFE.ExtensionInvalida));
            }
            catch (FormatoInvalidoException)
            {
                return new DtoRespuesta(MensajesParaFE.FormatoNoSoportadoCod, new Mensaje(MensajesParaFE.FormatoNoSoportado));
            }
            catch (AudioInvalidoException)
            {
                return new DtoRespuesta(MensajesParaFE.ErrorEnviarArchivoCod, new Mensaje(MensajesParaFE.ErrorEnviarArchivo));
            }
            catch (UsuarioNoAutorizadoException)
            {
                return new DtoRespuesta(MensajesParaFE.UsuarioNoAutorizadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutorizado));
            }
            catch (Exception e)
            {
                dbAL.AgregarLogError(token, string.Empty, Modulo, "AdjuntosController", 0, "AdjuntarAudio", "Hubo un error al intentar adjuntar un audio, se adjunta excepcion: " + e.Message, MensajesParaFE.ErrorAdjuntarAudioCod);
                return new DtoRespuesta(MensajesParaFE.ErrorCod, new Mensaje(MensajesParaFE.ErrorAdjuntarAudio));
            }
        }

        /// <summary>
        /// Agrega un archivo de video a una extension del servidor, recibe el video y el id de la extension como multipart form data.
        /// </summary>
        /// <returns></returns>
        [CustomAuthorizeAttribute("adjuntarVideo")]
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

                dbAL.adjuntarVideo(token, vid);
                return new DtoRespuesta(MensajesParaFE.CorrectoCod, new Mensaje(MensajesParaFE.Correcto));                
            }
            catch (TokenInvalidoException)
            {
                return new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado));
            }
            catch (ExtensionInvalidaException)
            {
                return new DtoRespuesta(MensajesParaFE.ExtensionInvalidaCod, new Mensaje(MensajesParaFE.ExtensionInvalida));
            }
            catch (FormatoInvalidoException)
            {
                return new DtoRespuesta(MensajesParaFE.FormatoNoSoportadoCod, new Mensaje(MensajesParaFE.FormatoNoSoportado));
            }
            catch (VideoInvalidoException)
            {
                return new DtoRespuesta(MensajesParaFE.ErrorEnviarArchivoCod, new Mensaje(MensajesParaFE.ErrorEnviarArchivo));
            }
            catch (UsuarioNoAutorizadoException)
            {
                return new DtoRespuesta(MensajesParaFE.UsuarioNoAutorizadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutorizado));
            }
            catch (Exception e)
            {
                dbAL.AgregarLogError(token, string.Empty, Modulo, "AdjuntosController", 0, "AdjuntarVideo", "Hubo un error al intentar adjuntar un video, se adjunta excepcion: " + e.Message, MensajesParaFE.ErrorAdjuntarVideoCod);
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
        public HttpResponseMessage GetImageData(int idImagen)
        {
            IMetodos dbAL = new Metodos();
            string token = ObtenerToken.GetToken(Request);
            try
            {
                if (token == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.Unauthorized);
                }

                var img = dbAL.getImageData(token, idImagen);
                if (img == null)
                {                    
                    return new HttpResponseMessage(HttpStatusCode.NotFound);
                }

                HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.OK);
                var stream = new MemoryStream(img.fileData);
                responseMessage.Content = new StreamContent(stream);
                responseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                responseMessage.Content.Headers.ContentDisposition.FileName = img.nombre;
                responseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                responseMessage.Content.Headers.ContentLength = stream.Length;
                return responseMessage;
            }
            catch (ImagenInvalidaException)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
            catch (UsuarioNoAutorizadoException)
            {
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }
            catch (TokenInvalidoException)
            {
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }
            catch (Exception e)
            {
                dbAL.AgregarLogError(token, string.Empty, Modulo, "AdjuntosController", 0, "GetImageData", "Hubo un error al intentar obtener los datos de una imagen, se adjunta excepcion: " + e.Message, MensajesParaFE.ErrorDescargarArchivoCod);
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

    /// <summary>
    /// Servicio para obtener el thumbnail de un archivo de imagen.
    /// </summary>
    /// <param name="idImagen">id de la imagen solicitada</param>
    /// <returns>Dto con el nombre y los bytes del archivo</returns>
    [CustomAuthorizeAttribute("verMultimedia")]
        [LogFilter]
        [Route("adjuntos/getimagethumbnail")]
        [HttpGet]
        public HttpResponseMessage GetImageThumbnail(int idImagen)
        {
            IMetodos dbAL = new Metodos();
            string token = ObtenerToken.GetToken(Request);
            try
            {
                if (token == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.Unauthorized);
                }

                var img = dbAL.getImageThumbnail(token, idImagen);
                if (img == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.NotFound);
                }

                HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.OK);
                var stream = new MemoryStream(img.fileData);
                responseMessage.Content = new StreamContent(stream);
                responseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                responseMessage.Content.Headers.ContentDisposition.FileName = img.nombre;
                responseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                responseMessage.Content.Headers.ContentLength = stream.Length;
                return responseMessage;
            }
            catch (ImagenInvalidaException)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
            catch (UsuarioNoAutorizadoException)
            {
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }
            catch (TokenInvalidoException)
            {
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }
            catch (Exception e)
            {
                dbAL.AgregarLogError(token, string.Empty, Modulo, "AdjuntosController", 0, "GetImageData", "Hubo un error al intentar obtener los datos de una imagen, se adjunta excepcion: " + e.Message, MensajesParaFE.ErrorDescargarArchivoCod);
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
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
            catch (TokenInvalidoException)
            {
                return new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado));
            }
            catch (Exception e)
            {
                dbAL.AgregarLogError(token, string.Empty, Modulo, "AdjuntosController", 0, "GetVideoData", "Hubo un error al intentar obtener los datos de un video, se adjunta excepcion: " + e.Message, MensajesParaFE.ErrorDescargarArchivoCod);
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
            catch (TokenInvalidoException)
            {
                return new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado));
            }
            catch (Exception e)
            {
                dbAL.AgregarLogError(token, string.Empty, Modulo, "AdjuntosController", 0, "GetAudioData", "Hubo un error al intentar obtener los datos de un audio, se adjunta excepcion: " + e.Message, MensajesParaFE.ErrorDescargarArchivoCod);
                return new DtoRespuesta(MensajesParaFE.ErrorCod, new Mensaje(MensajesParaFE.ErrorDescargarArchivo));
            }
        }
    }
}
