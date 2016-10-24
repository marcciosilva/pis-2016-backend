using System;
using System.Linq;
using Emsys.DataAccesLayer.Core;
using NUnit.Framework;
using System.IO;
using Emsys.LogicLayer;
using DataTypeObject;
using Servicios.Controllers;
using System.Net.Http;
using System.Threading;
using Newtonsoft.Json;
using System.Web.Services.Protocols;
using System.Collections.Generic;

namespace Test.UnitTesting
{
    [TestFixture]
    public class ServiceLayerUnitTest
    {
        /// <summary>
        /// Test login.
        /// </summary>
        [Test]
        public void LoginTest()
        {
            AppDomain.CurrentDomain.SetData(
            "DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));
            EmsysContext db = new EmsysContext();
            db.Users.FirstOrDefault(u => u.NombreLogin == "A").Token = null;
            db.SaveChanges();

            var controller = new LoginController();
            DtoUser u1 = new DtoUser() { username = "invalido", password = "invalido" };

            var resp1 = controller.Login(u1);
            Assert.IsTrue(resp1.cod == 1);

            DtoUser u2 = new DtoUser() { username = "A", password = "A" };

            var resp2 = controller.Login(u2);
            Assert.IsTrue(resp2.cod == 0);

            var resp3 = controller.Login(u2);
            Assert.IsTrue(resp3.cod == 1);
        }

        /// <summary>
        /// Test get roles.
        /// </summary>
        [Test]
        public void GetRolesTest()
        {
            AppDomain.CurrentDomain.SetData(
            "DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));
            EmsysContext db = new EmsysContext();
            db.Users.FirstOrDefault(u => u.NombreLogin == "A").Token = null;
            db.SaveChanges();

            var controller = new LoginController();

            var resp1 = controller.GetRoles();
            Assert.IsTrue(resp1.cod == 2);

            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", "tokenInvalido");

            var resp2 = controller.GetRoles();
            Assert.IsTrue(resp2.cod == 2);
        }

        /// <summary>
        /// Test elegir roles.
        /// </summary>
        [Test]
        public void ElegirRolesTest()
        {
            AppDomain.CurrentDomain.SetData(
            "DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));
            EmsysContext db = new EmsysContext();
            db.Users.FirstOrDefault(us => us.NombreLogin == "A").Token = null;
            db.SaveChanges();

            var controller = new LoginController();

            DtoRol rol = new DtoRol() { zonas = new List<DtoZona>(), recursos = new List<DtoRecurso>() };

            var resp1 = controller.ElegirRoles(rol);
            Assert.IsTrue(resp1.cod == 2);

            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", "tokenInvalido");

            var resp2 = controller.ElegirRoles(rol);
            Assert.IsTrue(resp2.cod == 2);

            DtoUser u = new DtoUser() { username = "A", password = "A" };
            var resp3 = controller.Login(u);
            string token = ((DtoAutenticacion)resp3.response).access_token;

            DtoZona z = new DtoZona();
            DtoRecurso r = new DtoRecurso() { id = 1 };
            rol.zonas.Add(z);
            rol.recursos.Add(r);
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp4 = controller.ElegirRoles(rol);
            Assert.IsTrue(resp4.cod == 3);

            db.Recursos.FirstOrDefault().Estado = Emsys.DataAccesLayer.Model.EstadoRecurso.NoDisponible;
            db.SaveChanges();

            rol.zonas.Clear();
            var resp5 = controller.ElegirRoles(rol);
            Assert.IsTrue(resp5.cod == 4);

            db.Recursos.FirstOrDefault().Estado = Emsys.DataAccesLayer.Model.EstadoRecurso.Disponible;
            db.SaveChanges();
        }


        /// <summary>
        /// Test cerrar sesion.
        /// </summary>
        [Test]
        public void CerrarSesionTest()
        {
            AppDomain.CurrentDomain.SetData(
            "DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));
            EmsysContext db = new EmsysContext();
            db.Users.FirstOrDefault(u => u.NombreLogin == "A").Token = null;
            db.SaveChanges();

            var controller = new LoginController();

            var resp1 = controller.CerrarSesion();
            Assert.IsTrue(resp1.cod == 2);

            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", "tokenInvalido");

            var resp2 = controller.CerrarSesion();
            Assert.IsTrue(resp2.cod == 2);
        }


        /// <summary>
        /// Test keep me alive.
        /// </summary>
        [Test]
        public void KeepMeAlive()
        {
            AppDomain.CurrentDomain.SetData(
            "DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));
            EmsysContext db = new EmsysContext();
            db.Users.FirstOrDefault(us => us.NombreLogin == "A").Token = null;
            db.SaveChanges();

            var controller = new LoginController();

            DtoRol rol = new DtoRol() { zonas = new List<DtoZona>(), recursos = new List<DtoRecurso>() };

            var resp1 = controller.KeepMeAlive();
            Assert.IsTrue(resp1.cod == 2);

            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", "tokenInvalido");

            var resp2 = controller.KeepMeAlive();
            Assert.IsTrue(resp2.cod == 2);

            DtoUser u = new DtoUser() { username = "A", password = "A" };
            var resp3 = controller.Login(u);
            string token = ((DtoAutenticacion)resp3.response).access_token;

            
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp4 = controller.KeepMeAlive();
            Assert.IsTrue(resp4.cod == 0);
        }


        /// <summary>
        /// Test listar eventos.
        /// </summary>
        [Test]
        public void ListarEventosTest()
        {
            AppDomain.CurrentDomain.SetData(
            "DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));
            EmsysContext db = new EmsysContext();
            db.Users.FirstOrDefault(us => us.NombreLogin == "A").Token = null;
            db.SaveChanges();

            var controller = new EventosController();

            DtoRol rol = new DtoRol() { zonas = new List<DtoZona>(), recursos = new List<DtoRecurso>() };

            var resp1 = controller.ListarEventos();
            Assert.IsTrue(resp1.cod == 2);

            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", "tokenInvalido");

            var resp2 = controller.ListarEventos();
            Assert.IsTrue(resp2.cod == 2);
        }


        /// <summary>
        /// Test get evento.
        /// </summary>
        [Test]
        public void GetEventoTest()
        {
            AppDomain.CurrentDomain.SetData(
            "DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));
            EmsysContext db = new EmsysContext();
            db.Users.FirstOrDefault(us => us.NombreLogin == "A").Token = null;
            db.Recursos.FirstOrDefault().Estado = Emsys.DataAccesLayer.Model.EstadoRecurso.Disponible;
            db.SaveChanges();

            var controller = new EventosController();

            var resp1 = controller.getEvento(1);
            Assert.IsTrue(resp1.cod == 2);

            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", "tokenInvalido");

            var resp2 = controller.getEvento(1);
            Assert.IsTrue(resp2.cod == 2);

            DtoUser u = new DtoUser() { username = "A", password = "A" };
            var controller2 = new LoginController();
            var resp3 = controller2.Login(u);
            string token = ((DtoAutenticacion)resp3.response).access_token;
            
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp4 = controller.getEvento(-1);
            Assert.IsTrue(resp4.cod == 9);

            DtoRol rol = new DtoRol() { zonas = new List<DtoZona>(), recursos = new List<DtoRecurso>() };
            DtoRecurso r = new DtoRecurso() { id = 1 };
            rol.recursos.Add(r);
            controller2.Request = new HttpRequestMessage();
            controller2.Request.Headers.Add("auth", token);
            var ok = controller2.ElegirRoles(rol);

            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp5 = controller.getEvento(1);
            Assert.IsTrue(resp5.cod == 500);
        }


        ///// <summary>
        ///// Test get evento.
        ///// </summary>
        //[Test]
        //public void AdjuntarImagenTest()
        //{
        //    AppDomain.CurrentDomain.SetData(
        //    "DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));
        //    EmsysContext db = new EmsysContext();
        //    db.Users.FirstOrDefault(us => us.NombreLogin == "A").Token = null;
        //    db.Recursos.FirstOrDefault().Estado = Emsys.DataAccesLayer.Model.EstadoRecurso.Disponible;
        //    db.SaveChanges();

        //    var controller = new AdjuntosController();
        //    var controller2 = new LoginController();
           
        //    //var resp1 = controller.PostImageFile();
        //    //Assert.IsTrue(resp1.cod == 2);

        //    //controller.Request = new HttpRequestMessage();
        //    //controller.Request.Headers.Add("auth", "tokenInvalido");
        //    //controller.Request.Content = new MultipartFormDataContent();
        //    //var resp2 = controller.PostImageFile();
        //    //Assert.IsTrue(resp2.cod == 12);

        //    DtoUser u = new DtoUser() { username = "A", password = "A" };
        //    var resp3 = controller2.Login(u);
        //    string token = ((DtoAutenticacion)resp3.response).access_token;

        //    DtoRol rol = new DtoRol() { zonas = new List<DtoZona>(), recursos = new List<DtoRecurso>() };
        //    DtoRecurso r = new DtoRecurso() { id = 1 };
        //    rol.recursos.Add(r);
        //    controller2.Request = new HttpRequestMessage();
        //    controller2.Request.Headers.Add("auth", token);
        //    var ok = controller2.ElegirRoles(rol);
        //}


        ///// <summary>
        ///// Prueba servicio externo.
        ///// </summary>
        //[Test]
        //public void ServicioExternoTesto()
        //{
        //    ServidorExterno.Servicios s = new ServidorExterno.Servicios();
        //    var controller = new ServicioExternoController();
        //    var resp = controller.ConsumirServicioExterno(new DtoConsultaExterna() { param1 = "uno", param2 = "dos", param3 = "tres" });
        //    var lista = (List<DtoRespuestaExterna>) resp.response;
        //    Assert.IsTrue(lista.Count == 2);
        //    Assert.AreEqual(lista[0].field1, "uno");
        //    Assert.AreEqual(lista[0].field2, "dos");
        //    Assert.AreEqual(lista[0].field3, "tres");
        //    Assert.AreEqual(lista[0].field4, "DDD");
        //    Assert.AreEqual(lista[0].field5, "EEE");
        //    Assert.AreEqual(lista[0].field6, "FFF");
        //    Assert.AreEqual(lista[0].field7, "GGG");
        //    Assert.AreEqual(lista[0].field8, "HHH");
        //    Assert.AreEqual(lista[0].field9, "III");
        //    Assert.AreEqual(lista[0].field10, "JJJ");

        //    //try
        //    //{
        //    //    controller.ConsumirServicioExterno(new DtoConsultaExterna() { param1 = "uno", param2 = "dos", param3 = "tres" });
        //    //}
        //    //catch (SoapException e)
        //    //{
        //    //    Assert.IsTrue(true);
        //    //}
        //}
    }
}
