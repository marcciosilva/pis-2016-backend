using System;
using System.Web.Http;
using DataTypeObject;
using Emsys.LogicLayer;
using Emsys.LogicLayer.ApplicationExceptions;
using Servicios.Filtros;

namespace Servicios.Controllers
{
    public class LoginController : ApiController
    {
        /// <summary>
        /// Servicio de login.
        /// </summary>
        /// <param name="user">Datos del usuario.</param>
        /// <returns>Respuesta definida en el documento de interfaz.</returns>
        [HttpPost]
        [Route("users/authenticate")]
        [LogFilter]
        public DtoRespuesta Login([FromBody] DtoUsuario user)
        {
            IMetodos dbAL = new Metodos();
            string token = ObtenerToken.GetToken(Request);
            try
            {
                DtoAutenticacion autenticacion = dbAL.autenticarUsuario(user.username, user.password, token);
                return new DtoRespuesta(MensajesParaFE.CorrectoCod, autenticacion);
            }
            catch (SesionActivaException)
            {
                return new DtoRespuesta(MensajesParaFE.SesionActivaCod, new Mensaje(MensajesParaFE.SesionActiva));
            }
            catch (CredencialesInvalidasException)
            {
                return new DtoRespuesta(MensajesParaFE.UsuarioContraseñaInvalidosCod, new Mensaje(MensajesParaFE.UsuarioContraseñaInvalidos));
            }
            catch (Exception e)
            {
                dbAL.AgregarLogError(user.username, "", "Emsys.ServiceLayer", "LoginController", 0, "Login", "Hubo un error al intentar iniciar sesion, se adjunta excepcion: " + e.Message, MensajesParaFE.ErrorIniciarSesionCod);
                return new DtoRespuesta(MensajesParaFE.ErrorCod, new Mensaje(MensajesParaFE.ErrorIniciarSesion));
            }          
        }

        /// <summary>
        /// Servicio obtener roles.
        /// </summary>
        /// <returns>Devuelve los roles asociados a un usuario. En el header del request se recibe el token del usuario.</returns>
        [CustomAuthorizeAttribute("login")]
        [HttpPost]
        [LogFilter]
        [Route("users/getroles")]
        public DtoRespuesta GetRoles()
        {
            IMetodos dbAL = new Metodos();
            string token = ObtenerToken.GetToken(Request);
            try
            {
                if (token == null)
                {
                    return new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado));
                }

                DtoRol rol = dbAL.getRolUsuario(token);
                return new DtoRespuesta(MensajesParaFE.CorrectoCod, rol);
            }
            catch (TokenInvalidoException)
            {
                return new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado));
            }
            catch (Exception e)
            {
                dbAL.AgregarLogError(token, "", "Emsys.ServiceLayer", "LoginController", 0, "GetRoles", "Hubo un error al intentar obtener roles de un usuario, se adjunta excepcion: " + e.Message, MensajesParaFE.ErrorGetRolesCod);
                return new DtoRespuesta(MensajesParaFE.ErrorCod, new Mensaje(MensajesParaFE.ErrorGetRoles));
            }
        }

        /// <summary>
        /// Servicio para elegir con que rol se loguea un usuario.
        /// </summary>
        /// <param name="rol">Roles a los que sea desea loguear.</param>
        /// <returns>Devuelve la respuesta definida en el documento de interfaz.</returns>
        [CustomAuthorizeAttribute("login")]
        [HttpPost]
        [LogFilter]
        [Route("users/login")]
        public DtoRespuesta ElegirRoles([FromBody] DtoRol rol)
        {
            IMetodos dbAL = new Metodos();
            string token = ObtenerToken.GetToken(Request);
            try
            {
                if (token == null)
                {
                    return new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado));
                }

                if (dbAL.loguearUsuario(token, rol))
                {
                    return new DtoRespuesta(MensajesParaFE.CorrectoCod, new Mensaje(MensajesParaFE.Correcto));
                }
                else
                {
                    return new DtoRespuesta(MensajesParaFE.SeleccionZonasRecursosInvalidaCod, new Mensaje(MensajesParaFE.SeleccionZonasRecursosInvalida));
                }
            }
            catch (RecursoNoDisponibleException)
            {
                return new DtoRespuesta(MensajesParaFE.RecursoNoDisponibleCod, new Mensaje(MensajesParaFE.RecursoNoDisponible));
            }
            catch (TokenInvalidoException)
            {
                return new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado));
            }
            catch (Exception e)
            {
                dbAL.AgregarLogError(token, "", "Emsys.ServiceLayer", "LoginController", 0, "ElegirRoles", "Hubo un error al intentar elegir los roles de un usuario, se adjunta excepcion: " + e.Message, MensajesParaFE.ErrorElegirRolesCod);
                return new DtoRespuesta(MensajesParaFE.ErrorCod, new Mensaje(MensajesParaFE.ErrorElegirRoles));
            }
        }

        /// <summary>
        /// Servicio para cerrar session de usuario. Borrando el token y la fecha de inicio de sesion.
        /// </summary>
        /// <returns>Retorna la respuesta definida en el documento de interfaz.</returns>
        [CustomAuthorizeAttribute("login")]
        [HttpPost]
        [LogFilter]
        [Route("users/logout")]
        public DtoRespuesta CerrarSesion()
        {
            IMetodos dbAL = new Metodos();
            string token = ObtenerToken.GetToken(Request);
            try
            {
                if (token == null)
                {
                    return new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado));
                }

                // TODO esto no se puede hacer hasta tener operaciones.
                var usuarioOperacionesNoFinalizadas = false;
                if (usuarioOperacionesNoFinalizadas)
                {
                    return new DtoRespuesta(MensajesParaFE.UsuarioTieneOperacionesNoFinalizadasCod, new Mensaje(MensajesParaFE.UsuarioTieneOperacionesNoFinalizadas));
                }

                dbAL.cerrarSesion(token);
                return new DtoRespuesta(MensajesParaFE.CorrectoCod, new Mensaje(MensajesParaFE.Correcto));
            }
            catch (TokenInvalidoException)
            {
                return new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado));
            }
            catch (Exception e)
            {
                dbAL.AgregarLogError(token, "", "Emsys.ServiceLayer", "LoginController", 0, "CerrarSesion", "Hubo un error al intentar cerrar sesion, se adjunta excepcion: " + e.Message, MensajesParaFE.ErrorCerrarSesionCod);
                return new DtoRespuesta(MensajesParaFE.ErrorCod, new Mensaje(MensajesParaFE.ErrorCerraSesion));
            }
        }

        [CustomAuthorizeAttribute("login")]
        [HttpPost]
        [LogFilter]
        [Route("users/keepmealive")]
        public DtoRespuesta KeepMeAlive()
        {
            IMetodos dbAL = new Metodos();
            string token = ObtenerToken.GetToken(Request);
            try
            {
                if (token == null)
                {
                    return new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado));
                }

                dbAL.keepMeAlive(token);
                return new DtoRespuesta(MensajesParaFE.CorrectoCod, new Mensaje(MensajesParaFE.Correcto));
            }
            catch (TokenInvalidoException)
            {
                return new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado));
            }
            catch (Exception e)
            {
                dbAL.AgregarLogError(token, "", "Emsys.ServiceLayer", "LoginController", 0, "KeepMeAlive", "Hubo un error al intentar llaar al keepAlive: " + e.Message, MensajesParaFE.ErrorKeepMeAliveCod);
                return new DtoRespuesta(MensajesParaFE.ErrorCod, new Mensaje(MensajesParaFE.ErrorKeepMeAlive));
            }
        }


        
        [HttpPost]
        [LogFilter]
        [Route("users/SetRegistrationToken")]
        public DtoRespuesta SetRegistrationToken(DtoResgistrationToken dtoToken)
        {
            IMetodos dbAL = new Metodos();
            string token = ObtenerToken.GetToken(Request);
            try
            {
                if (token == null)
                {
                    return new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado));
                }

                dbAL.SetRegistrationToken(token, dtoToken.registrationTokens);
                return new DtoRespuesta(MensajesParaFE.CorrectoCod, new Mensaje(MensajesParaFE.Correcto));
            }
            catch (TokenInvalidoException)
            {
                return new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado));
            }
            catch (Exception e)
            {
                dbAL.AgregarLogError(token, "", "Emsys.ServiceLayer", "LoginController", 0, "KeepMeAlive", "Hubo un error al intentar llaar al keepAlive: " + e.Message, MensajesParaFE.ErrorKeepMeAliveCod);
                return new DtoRespuesta(MensajesParaFE.ErrorCod, new Mensaje(MensajesParaFE.ErrorKeepMeAlive));
            }
        }
    }
}