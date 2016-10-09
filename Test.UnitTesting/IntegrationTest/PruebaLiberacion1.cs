using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTypeObject;
using NUnit.Framework;
using Servicios.Controllers;
using System.Net.Http;
using Emsys.DataAccesLayer.Core;
using System.IO;
using Emsys.DataAccesLayer.Model;

namespace Test.UnitTesting
{
    [TestFixture]
    public class PruebaLiberacion1
    {
        [Test]
        public void positiveTest()
        {
            using (var context = new EmsysContext())
            {
                AppDomain.CurrentDomain.SetData(
                "DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));
                

                var nombreUsuario = "A";
                var contraseña = "A";
                // LOGIN.
                var controllerLogin = new LoginController();
                var response = controllerLogin.Login(new DtoUser() { username = nombreUsuario, password = contraseña });
                var respuesta = response.Result;
                Assert.IsTrue(respuesta.cod == 0);
                var respuestaAutenticate = (DtoAutenticacion)respuesta.response;
                var token = respuestaAutenticate.access_token;

                controllerLogin.Request = new HttpRequestMessage();
                controllerLogin.Request.Headers.Add("auth", token);
                var respuesta2 = controllerLogin.GetRoles();
                Assert.IsTrue(respuesta2.cod == 0);
                var recurso_o_zona = (DtoRol)respuesta2.response;

                controllerLogin.Request = new HttpRequestMessage();
                controllerLogin.Request.Headers.Add("auth", token);
                var rolElegidoZona = new DtoRol() { zonas = recurso_o_zona.zonas, recursos = new List<DtoRecurso>() };
                var respuesta3 = controllerLogin.ElegirRoles(rolElegidoZona);
                Assert.IsTrue(respuesta3.cod == 0);


                // LISTAR EVENTOS.
                var controllerListarEventos = new EventosController();
                controllerListarEventos.Request = new HttpRequestMessage();
                controllerListarEventos.Request.Headers.Add("auth", token);
                var respuesta4 = controllerListarEventos.ListarEventos();
                Assert.IsTrue(respuesta4.cod == 0);
                var eventosRespuesta = (ICollection<DtoEvento>)respuesta4.response;
                using (EmsysContext db = new EmsysContext())
                {
                    var user = db.Users.Where(x => x.NombreUsuario == nombreUsuario).FirstOrDefault();
                    if (user != null)
                    {
                        var zonas = user.Zonas;
                        foreach (var item in zonas)
                        {
                            foreach (var extensionEvento in item.Extensiones_Evento)
                            {
                                var esta = eventosRespuesta.Where(x => x.id == extensionEvento.Evento.Id).FirstOrDefault();
                                Assert.IsNotNull(esta);
                            }

                        }
                    }
                }

                // CERRAR SESION.
                controllerLogin.Request = new HttpRequestMessage();
                controllerLogin.Request.Headers.Add("auth", token);
                var respuesta5 = controllerLogin.CerrarSesion();
                Assert.IsTrue(respuesta5.cod == 0);
                Assert.IsTrue(context.Users.FirstOrDefault(u => u.NombreUsuario == "A").Zonas.Count() == 0);
            }
        }
    }
}
