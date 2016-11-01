using System;
using System.Collections.Generic;
using System.Linq;
using DataTypeObject;
using NUnit.Framework;
using Servicios.Controllers;
using System.Net.Http;
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
            var context = new EmsysContext();
            AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));

            var nombreUsuario = "B";
            var contraseña = "B";

            // LOGIN.
            var controllerLogin = new LoginController();
            var us = context.Evento.FirstOrDefault();

            var respuesta = controllerLogin.Login(new DtoUsuario() { username = nombreUsuario, password = contraseña });
            Assert.IsTrue(respuesta.cod == 0);
            var respuestaAutenticate = (DtoAutenticacion)respuesta.response;
            var token = respuestaAutenticate.accessToken;

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
            var eventosRespuesta = (ICollection<DataTypeObject.DtoEvento>)respuesta4.response;
            using (EmsysContext db = new EmsysContext())
            {
                var user = db.Usuarios.Where(x => x.NombreLogin == nombreUsuario).FirstOrDefault();
                if (user != null)
                {
                    var zonas = user.Zonas;
                    foreach (var item in zonas)
                    {
                        foreach (var extensionEvento in item.ExtensionesEvento)
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
            context = new EmsysContext();
            int cant = context.Usuarios.FirstOrDefault(u => u.NombreLogin == "B").Zonas.Count();
            Assert.IsTrue(cant == 0);
        }
        
    }
}
