using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTypeObject;
using NUnit.Framework;
using Servicios.Controllers;
using System.Net.Http;
using System.Web.Http;
using DataTypeObjetc;
using Newtonsoft.Json;
using Emsys.DataAccesLayer.Core;
using System.IO;

namespace Test.UnitTesting
{
    [TestFixture]
    public class PruebaLiberacion1
    {
        [Test]
        public void positiveTest()
        {

            AppDomain.CurrentDomain.SetData(
           "DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));

            var nombreUsuario = "A";
            var contraseña = "A";
            //LOGIN
            var controllerAutenticate = new LoginController();
            var response= controllerAutenticate.Login(nombreUsuario, contraseña);
            var respuesta=  response.Result;
            Assert.IsTrue(respuesta.cod==0);
            var respuestaAutenticate =(Authenticate) respuesta.response;
            var token = respuestaAutenticate.access_token;

            var controllerGetRoles = new GetRolUsuarioController();
            controllerGetRoles.Request = new HttpRequestMessage();
            controllerGetRoles.Request.Headers.Add("auth", "1");
            var respuesta2 = controllerGetRoles.GetRoles();
            Assert.IsTrue(respuesta2.cod==0);
            var recurso_o_zona = (DtoRol) respuesta2.response;

            var controllerLoguearUsuario = new LoguearUsuarioController();
            controllerLoguearUsuario.Request = new HttpRequestMessage();
            controllerLoguearUsuario.Request.Headers.Add("auth", "1");
            string rolElegidoZona = JsonConvert.SerializeObject( new DtoRol() { zonas = recurso_o_zona.zonas, recursos = new List<DtoRecurso>() });
            var respuesta3 = controllerLoguearUsuario.ElegirRole(rolElegidoZona);
            Assert.IsTrue(respuesta3.cod==0);


            //LISTAR EVENTOS
            var controllerListarEventos = new ListarEventosController();
            controllerListarEventos.Request = new HttpRequestMessage();
            controllerListarEventos.Request.Headers.Add("auth", "1");
            var respuesta4= controllerListarEventos.ListarEventos();
            Assert.IsTrue(respuesta4.cod==0);
            var eventosRespuesta = (ICollection<DtoEvento>)respuesta4.response;
            using (EmsysContext db = new EmsysContext()) {
                var user = db.Users.Where(x => x.NombreUsuario == nombreUsuario).FirstOrDefault();
                if (user!=null) {
                    var zonas = user.Zonas;
                    foreach (var item in zonas)
                    {
                        foreach (var extensionEvento in item.Extensiones_Evento)
                        {
                            var esta=eventosRespuesta.Where(x => x.id == extensionEvento.Evento.Id).FirstOrDefault();
                            Assert.IsNotNull(esta);
                        }
                        
                    }
                }
            }
            //Assert.IsTrue(response.TryGetContentValue<Product>(out product));
            //controller.Request = new HttpRequestMessage();
            //controller.Request.Headers.Add("auth", "A");
            //controller.Configuration = new HttpConfiguration();
        }
    }
}
