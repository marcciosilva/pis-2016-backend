using Emsys.DataAccesLayer.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using Servicios.Identity;
using DataTypeObject;
using Emsys.DataAccesLayer.Model;
using Utils.Login;
using DataTypeObjetc;
using Emsys.Logs;
namespace Servicios.Controllers
{
    public class LoginController : ApiController
    {
        [HttpPost]
        [Route("users/authenticate")]
        public async Task<GenericResponse> Login(string usuario, string contraseña)
        {

            try
            {
                using (EmsysContext db = new EmsysContext())
                {
                    var user = db.Users.Where(x => x.NombreUsuario == usuario).FirstOrDefault();
                    if (user != null)
                    {
                        if (user.Contraseña == Contraseñas.GetSHA1(contraseña))
                        {
                            if (user.Token != null)
                            {
                                //ya estaba logueadok, no genero nuevo token
                                return new GenericResponse(Mensajes.GetCodMenssage(Mensajes.UsuarioLogueado), new Authenticate(null, Mensajes.UsuarioLogueado));
                            }
                            //son correcto los datos entonces genero token
                            user.Token = TokenGenerator.ObetenerToken();
                            user.FechaInicioSesion = DateTime.Now;
                            db.SaveChanges();
                            return new GenericResponse(0, new Authenticate(user.Token, null));
                        }
                    }
                    //retoronar que los datos son invalidos.
                    return new GenericResponse(Mensajes.GetCodMenssage(Mensajes.UsuarioContraseñaInvalidos), new Authenticate(null, Mensajes.UsuarioContraseñaInvalidos));
                }
            }
            catch (Exception e)
            {
                Emsys.Logs.Log.AgregarLogError(usuario,"", "Emsys.ServiceLayer", "LoginController", 0, "Login","Hubo un error al intentar iniciar sesion, se adjunta excepcion: " + e.Message, Emsys.Logs.Constantes.ErrorIniciarSesion);
                return new GenericResponse(Mensajes.GetCodMenssage(Mensajes.UsuarioContraseñaInvalidos), new Authenticate(null, Mensajes.UsuarioContraseñaInvalidos));
            }
        }
    }
}