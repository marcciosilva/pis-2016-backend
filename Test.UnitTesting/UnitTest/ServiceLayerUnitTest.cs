using System;
using System.Linq;
using Emsys.DataAccesLayer.Core;
using NUnit.Framework;
using System.IO;
using Emsys.LogicLayer;
using DataTypeObject;
using Servicios.Controllers;
using System.Net.Http;

namespace Test.UnitTesting
{
    [TestFixture]
    public class ServiceLayerUnitTest
    {
        /// <summary>
        /// Este metodo es una una prueba positiva
        /// </summary>
        [Test]
        public void PruebaAsignacionRecursoDescripcion()
        {
            AppDomain.CurrentDomain.SetData(
            "DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));
            EmsysContext db = new EmsysContext();

            DtoActualizarDescripcionParametro param = new DtoActualizarDescripcionParametro
            {
                idExtension = 1,
                dtoDescripcion = new DtoDescripcion
                {
                    descripcion = "Agrego una descripcion",
                    fecha = DateTime.Now
                }
            };

            var nombreUsuario = "A";
            var contraseña = "A";

            // LOGIN.
            var controllerLogin = new LoginController();
            var respuesta = controllerLogin.Login(new DtoUser() { username = nombreUsuario, password = contraseña });
            Assert.IsTrue(respuesta.cod == 0);
            var respuestaAutenticate = (DtoAutenticacion)respuesta.response;
            var token = respuestaAutenticate.access_token;

            ActualizarDescripcionRecursoController controller = new ActualizarDescripcionRecursoController();
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var respuestaAgregarDescripcion = controller.ActualizarDescripcionRecurso(param);

            // Pruebo que el codigo de respuesta sea 0.
            Assert.IsTrue(respuestaAgregarDescripcion.cod == 0);

            var extension = db.Extensiones_Evento.Find(param.idExtension);
            Assert.NotNull(extension);
            Assert.IsTrue(extension.AccionesRecursos.Where(x => x.AsignacionRecursoDescripcion.Count() > 0).Count() > 0);
        }
    }
}
