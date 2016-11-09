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
using System.Net;
using System.Web.Http.Controllers;

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
            db.Usuarios.FirstOrDefault(u => u.NombreLogin == "A").Token = null;
            db.SaveChanges();

            var controller = new LoginController();
            DtoUsuario u1 = new DtoUsuario() { username = "invalido", password = "invalido" };

            var resp1 = controller.Login(u1);
            Assert.IsTrue(resp1.cod == 1);

            DtoUsuario u2 = new DtoUsuario() { username = "A", password = "A" };

            var resp2 = controller.Login(u2);
            Assert.IsTrue(resp2.cod == 0);

            var resp3 = controller.Login(u2);
            Assert.IsTrue(resp3.cod == 6);
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
            db.Usuarios.FirstOrDefault(u => u.NombreLogin == "A").Token = null;
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
            db.Usuarios.FirstOrDefault(us => us.NombreLogin == "A").Token = null;
            db.SaveChanges();

            var controller = new LoginController();

            DtoRol rol = new DtoRol() { zonas = new List<DtoZona>(), recursos = new List<DtoRecurso>() };

            var resp1 = controller.ElegirRoles(rol);
            Assert.IsTrue(resp1.cod == 2);

            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", "tokenInvalido");

            var resp2 = controller.ElegirRoles(rol);
            Assert.IsTrue(resp2.cod == 2);

            DtoUsuario u = new DtoUsuario() { username = "A", password = "A" };
            var resp3 = controller.Login(u);
            string token = ((DtoAutenticacion)resp3.response).accessToken;

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
            db.Usuarios.FirstOrDefault(u => u.NombreLogin == "A").Token = null;
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
            db.Usuarios.FirstOrDefault(us => us.NombreLogin == "A").Token = null;
            db.SaveChanges();

            var controller = new LoginController();

            DtoRol rol = new DtoRol() { zonas = new List<DtoZona>(), recursos = new List<DtoRecurso>() };

            var resp1 = controller.KeepMeAlive();
            Assert.IsTrue(resp1.cod == 2);

            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", "tokenInvalido");

            var resp2 = controller.KeepMeAlive();
            Assert.IsTrue(resp2.cod == 2);

            DtoUsuario u = new DtoUsuario() { username = "A", password = "A" };
            var resp3 = controller.Login(u);
            string token = ((DtoAutenticacion)resp3.response).accessToken;


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
            db.Usuarios.FirstOrDefault(us => us.NombreLogin == "A").Token = null;
            db.SaveChanges();

            var controller = new EventosController();

            // No autenticado.
            var resp1 = controller.ListarEventos();
            Assert.IsTrue(resp1.cod == 2);

            // Token invalido.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", "tokenInvalido");
            var resp2 = controller.ListarEventos();
            Assert.IsTrue(resp2.cod == 2);

            // Autenticar.
            DtoUsuario u = new DtoUsuario() { username = "A", password = "A" };
            var controller2 = new LoginController();
            var resp3 = controller2.Login(u);
            string token = ((DtoAutenticacion)resp3.response).accessToken;

            // Loguear.
            DtoRol rol = new DtoRol() { zonas = new List<DtoZona>(), recursos = new List<DtoRecurso>() };
            DtoRecurso r = new DtoRecurso() { id = 1 };
            rol.recursos.Add(r);
            controller2.Request = new HttpRequestMessage();
            controller2.Request.Headers.Add("auth", token);
            var ok = controller2.ElegirRoles(rol);

            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp4 = controller.ListarEventos();
            Assert.IsTrue(resp4.cod == 0);

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
            db.Usuarios.FirstOrDefault(us => us.NombreLogin == "A").Token = null;
            db.Recursos.FirstOrDefault().Estado = Emsys.DataAccesLayer.Model.EstadoRecurso.Disponible;
            db.SaveChanges();

            var controller = new EventosController();

            var resp1 = controller.GetEvento(1);
            Assert.IsTrue(resp1.cod == 2);

            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", "tokenInvalido");

            var resp2 = controller.GetEvento(1);
            Assert.IsTrue(resp2.cod == 2);

            DtoUsuario u = new DtoUsuario() { username = "A", password = "A" };
            var controller2 = new LoginController();
            var resp3 = controller2.Login(u);
            string token = ((DtoAutenticacion)resp3.response).accessToken;

            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp4 = controller.GetEvento(-1);
            Assert.IsTrue(resp4.cod == 9);

            DtoRol rol = new DtoRol() { zonas = new List<DtoZona>(), recursos = new List<DtoRecurso>() };
            DtoRecurso r = new DtoRecurso() { id = 1 };
            rol.recursos.Add(r);
            controller2.Request = new HttpRequestMessage();
            controller2.Request.Headers.Add("auth", token);
            var ok = controller2.ElegirRoles(rol);

            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp5 = controller.GetEvento(1);
            Assert.IsTrue(resp5.cod == 0);
        }

        /// <summary>
        /// Test actualizar descripcion recurso.
        /// </summary>
        [Test]
        public void ActualizarDescripcionRecursoTest()
        {
            AppDomain.CurrentDomain.SetData(
            "DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));
            EmsysContext db = new EmsysContext();
            db.Usuarios.FirstOrDefault(us => us.NombreLogin == "A").Token = null;
            db.Recursos.FirstOrDefault().Estado = Emsys.DataAccesLayer.Model.EstadoRecurso.Disponible;
            db.SaveChanges();

            var controller = new EventosController();

            DtoActualizarDescripcion descr = new DtoActualizarDescripcion() { descripcion = "descripcion", idExtension = 1 };

            // No autenticado.
            var resp1 = controller.ActualizarDescripcionRecurso(descr);
            Assert.IsTrue(resp1.cod == 2);

            // Token invalido.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", "tokenInvalido");
            var resp2 = controller.ActualizarDescripcionRecurso(descr);
            Assert.IsTrue(resp2.cod == 2);

            // Autenticar.
            DtoUsuario u = new DtoUsuario() { username = "A", password = "A" };
            var controller2 = new LoginController();
            var resp3 = controller2.Login(u);
            string token = ((DtoAutenticacion)resp3.response).accessToken;


            // Loguear.
            DtoRol rol = new DtoRol() { zonas = new List<DtoZona>(), recursos = new List<DtoRecurso>() };
            DtoRecurso r = new DtoRecurso() { id = 1 };
            rol.recursos.Add(r);
            controller2.Request = new HttpRequestMessage();
            controller2.Request.Headers.Add("auth", token);
            var ok = controller2.ElegirRoles(rol);

            // Sin tener vision.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp6 = controller.ActualizarDescripcionRecurso(new DtoActualizarDescripcion() { idExtension = 2, descripcion = "hola" });
            Assert.IsTrue(resp6.cod == 15);

            // Extension invalida.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp7 = controller.ActualizarDescripcionRecurso(new DtoActualizarDescripcion() { idExtension = -1, descripcion = "hola" });
            Assert.IsTrue(resp7.cod == 12);

            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp5 = controller.ActualizarDescripcionRecurso(descr);
            Assert.IsTrue(resp5.cod == 0);
        }


        /// <summary>
        /// Test reportar hora de arribo.
        /// </summary>
        [Test]
        public void ReportarHoraArriboTest()
        {
            AppDomain.CurrentDomain.SetData(
            "DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));
            EmsysContext db = new EmsysContext();
            db.Usuarios.FirstOrDefault(us => us.NombreLogin == "A").Token = null;
            db.Recursos.FirstOrDefault().Estado = Emsys.DataAccesLayer.Model.EstadoRecurso.Disponible;
            db.AsignacionesRecursos.FirstOrDefault().HoraArribo = null;
            db.SaveChanges();

            var controller = new EventosController();

            DtoActualizarDescripcion descr = new DtoActualizarDescripcion() { descripcion = "descripcion", idExtension = 1 };

            // No autenticado.
            var resp1 = controller.ReportarHoraArribo(1);
            Assert.IsTrue(resp1.cod == 2);

            // Token invalido.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", "tokenInvalido");
            var resp2 = controller.ReportarHoraArribo(1);
            Assert.IsTrue(resp2.cod == 2);

            // Autenticar.
            DtoUsuario u = new DtoUsuario() { username = "A", password = "A" };
            var controller2 = new LoginController();
            var resp3 = controller2.Login(u);
            string token = ((DtoAutenticacion)resp3.response).accessToken;


            // Loguear.
            DtoRol rol = new DtoRol() { zonas = new List<DtoZona>(), recursos = new List<DtoRecurso>() };
            DtoRecurso r = new DtoRecurso() { id = 1 };
            rol.recursos.Add(r);
            controller2.Request = new HttpRequestMessage();
            controller2.Request.Headers.Add("auth", token);
            var ok = controller2.ElegirRoles(rol);

            // Sin tener vision.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp6 = controller.ReportarHoraArribo(2);
            Assert.IsTrue(resp6.cod == 12);

            // Extension invalida.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp7 = controller.ReportarHoraArribo(-1);
            Assert.IsTrue(resp7.cod == 12);

            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp5 = controller.ReportarHoraArribo(1);
            Assert.IsTrue(resp5.cod == 0);
        }

        /// <summary>
        /// Test info creacion evento.
        /// </summary>
        [Test]
        public void InfoCreacionEventoTest()
        {
            AppDomain.CurrentDomain.SetData(
            "DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));
            EmsysContext db = new EmsysContext();
            db.Usuarios.FirstOrDefault(us => us.NombreLogin == "A").Token = null;
            db.Recursos.FirstOrDefault().Estado = Emsys.DataAccesLayer.Model.EstadoRecurso.Disponible;
            db.SaveChanges();

            var controller = new EventosController();

            DtoActualizarDescripcion descr = new DtoActualizarDescripcion() { descripcion = "descripcion", idExtension = 1 };

            // No autenticado.
            var resp1 = controller.GetInfoCreacionEvento();
            Assert.IsTrue(resp1.cod == 2);

            // Token invalido.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", "tokenInvalido");
            var resp2 = controller.GetInfoCreacionEvento();
            Assert.IsTrue(resp2.cod == 2);

            // Autenticar.
            DtoUsuario u = new DtoUsuario() { username = "A", password = "A" };
            var controller2 = new LoginController();
            var resp3 = controller2.Login(u);
            string token = ((DtoAutenticacion)resp3.response).accessToken;

            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp5 = controller.GetInfoCreacionEvento();
            Assert.IsTrue(resp5.cod == 0);
        }


        /// <summary>
        /// Test crear evento.
        /// </summary>
        [Test]
        public void CrearEventoTest()
        {
            AppDomain.CurrentDomain.SetData(
            "DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));
            EmsysContext db = new EmsysContext();
            db.Usuarios.FirstOrDefault(us => us.NombreLogin == "A").Token = null;
            db.Recursos.FirstOrDefault().Estado = Emsys.DataAccesLayer.Model.EstadoRecurso.Disponible;
            db.SaveChanges();

            var controller = new EventosController();

            DtoActualizarDescripcion descr = new DtoActualizarDescripcion() { descripcion = "descripcion", idExtension = 1 };

            // No autenticado.
            var resp1 = controller.CrearEvento(new DtoEvento());
            Assert.IsTrue(resp1.cod == 2);

            // Token invalido.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", "tokenInvalido");
            var resp2 = controller.CrearEvento(new DtoEvento());
            Assert.IsTrue(resp2.cod == 2);

            // Autenticar.
            DtoUsuario u = new DtoUsuario() { username = "A", password = "A" };
            var controller2 = new LoginController();
            var resp3 = controller2.Login(u);
            string token = ((DtoAutenticacion)resp3.response).accessToken;

            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            DtoInfoCreacionEvento info = (DtoInfoCreacionEvento)((DtoRespuesta)controller.GetInfoCreacionEvento()).response;
            List<int> idZonas = new List<int>();
            idZonas.Add(info.zonasSectores.FirstOrDefault().id);
            DtoEvento ev = new DtoEvento()
            {
                informante = "Informante",
                telefono = "0800-6969-6969",
                categoria = info.categorias.FirstOrDefault(),
                estado = "enviado",
                calle = "calle evento",
                esquina = "esquina evento",
                numero = "110",
                idDepartamento = info.departamentos.FirstOrDefault().id,
                idSector = info.zonasSectores.FirstOrDefault().sectores.FirstOrDefault().id,
                longitud = 19.95,
                latitud = 666.6,
                descripcion = "Este es un evento de prueba",
                enProceso = false,
                idZonas = new List<int>()
            };

            // Sin zonas.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp5 = controller.CrearEvento(ev);
            Assert.IsTrue(resp5.cod == 19);

            ev.idZonas = idZonas;

            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp6 = controller.CrearEvento(ev);
            Assert.IsTrue(resp6.cod == 0);

            // Argumento invalido.
            ev.idSector = -1;
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp7 = controller.CrearEvento(ev);
            Assert.IsTrue(resp7.cod == 14);
            controller2.SetRegistrationToken(new DtoResgistrationToken { registrationTokens = "A" });
            controller2.Request = new HttpRequestMessage();
            controller2.Request.Headers.Add("auth", token);
            controller2.CerrarSesion();
        }


        /// <summary>
        /// Test tomar y liberar extension.
        /// </summary>
        [Test]
        public void TomarLiberarExtensionTest()
        {
            AppDomain.CurrentDomain.SetData(
            "DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));
            EmsysContext db = new EmsysContext();
            db.Usuarios.FirstOrDefault(us => us.NombreLogin == "A").Token = null;
            db.Recursos.FirstOrDefault().Estado = Emsys.DataAccesLayer.Model.EstadoRecurso.Disponible;
            db.SaveChanges();

            var controller = new EventosController();

            DtoActualizarDescripcion descr = new DtoActualizarDescripcion() { descripcion = "descripcion", idExtension = 1 };

            // No autenticado.
            var resp1 = controller.TomarExtension(1);
            Assert.IsTrue(resp1.cod == 2);

            // Token invalido.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", "tokenInvalido");
            var resp2 = controller.TomarExtension(1);
            Assert.IsTrue(resp2.cod == 2);

            // Autenticar.
            DtoUsuario u = new DtoUsuario() { username = "A", password = "A" };
            var controller2 = new LoginController();
            var resp3 = controller2.Login(u);
            string token = ((DtoAutenticacion)resp3.response).accessToken;

            // Loguear.
            DtoRol rol = new DtoRol() { zonas = new List<DtoZona>(), recursos = new List<DtoRecurso>() };
            DtoZona z = new DtoZona() { id = 1 };
            rol.zonas.Add(z);
            controller2.Request = new HttpRequestMessage();
            controller2.Request.Headers.Add("auth", token);
            var ok = controller2.ElegirRoles(rol);

            // Extension invalida.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp5 = controller.TomarExtension(-1);
            Assert.IsTrue(resp5.cod == 12);

            // No autorizado.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp6 = controller.TomarExtension(2);
            Assert.IsTrue(resp6.cod == 15);


            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp7 = controller.TomarExtension(1);
            Assert.IsTrue(resp7.cod == 0);

            // No autenticado.
            controller.Request = new HttpRequestMessage();
            var resp8 = controller.LiberarExtension(1);
            Assert.IsTrue(resp8.cod == 2);

            // Token invalido.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", "tokenInvalido");
            var resp9 = controller.LiberarExtension(1);
            Assert.IsTrue(resp9.cod == 2);

            // Extension invalido.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp10 = controller.LiberarExtension(-1);
            Assert.IsTrue(resp10.cod == 12);

            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp11 = controller.LiberarExtension(1);
            Assert.IsTrue(resp11.cod == 0);
            controller2.Request = new HttpRequestMessage();
            controller2.Request.Headers.Add("auth", token);
            controller2.CerrarSesion();
        }


        /// <summary>
        /// Test get recursos y gestionar recursos.
        /// </summary>
        [Test]
        public void GetGestionarRecursosTest()
        {
            AppDomain.CurrentDomain.SetData(
            "DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));
            EmsysContext db = new EmsysContext();
            db.Usuarios.FirstOrDefault(us => us.NombreLogin == "A").Token = null;
            db.Recursos.FirstOrDefault().Estado = Emsys.DataAccesLayer.Model.EstadoRecurso.Disponible;
            db.SaveChanges();

            var controller = new EventosController();

            DtoActualizarDescripcion descr = new DtoActualizarDescripcion() { descripcion = "descripcion", idExtension = 1 };

            // No autenticado.
            var resp1 = controller.GetRecursosExtension(1);
            Assert.IsTrue(resp1.cod == 2);

            // Token invalido.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", "tokenInvalido");
            var resp2 = controller.GetRecursosExtension(1);
            Assert.IsTrue(resp2.cod == 2);

            // Autenticar.
            DtoUsuario u = new DtoUsuario() { username = "A", password = "A" };
            var controller2 = new LoginController();
            var resp3 = controller2.Login(u);
            string token = ((DtoAutenticacion)resp3.response).accessToken;

            // Loguear.
            DtoRol rol = new DtoRol() { zonas = new List<DtoZona>(), recursos = new List<DtoRecurso>() };
            DtoZona z = new DtoZona() { id = 1 };
            rol.zonas.Add(z);
            controller2.Request = new HttpRequestMessage();
            controller2.Request.Headers.Add("auth", token);
            var ok = controller2.ElegirRoles(rol);

            // Tomar extension.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp7 = controller.TomarExtension(1);

            // Extension invalida.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp5 = controller.GetRecursosExtension(-1);
            Assert.IsTrue(resp5.cod == 12);

            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            controller.GetRecursosExtension(1);
            Assert.IsTrue(resp7.cod == 0);


            // No autenticado.
            controller.Request = new HttpRequestMessage();
            var resp8 = controller.GestionarRecursos(new DtoRecursosExtension() { idExtension = 1, recursosAsignados = new List<DtoRecurso>(), recursosNoAsignados = new List<DtoRecurso>() });
            Assert.IsTrue(resp8.cod == 2);

            // Token invalido.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", "tokenInvalido");
            var resp9 = controller.GestionarRecursos(new DtoRecursosExtension() { idExtension = 1, recursosAsignados = new List<DtoRecurso>(), recursosNoAsignados = new List<DtoRecurso>() });
            Assert.IsTrue(resp9.cod == 2);

            // Extension invalido.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp10 = controller.GestionarRecursos(new DtoRecursosExtension() { idExtension = -1, recursosAsignados = new List<DtoRecurso>(), recursosNoAsignados = new List<DtoRecurso>() });
            Assert.IsTrue(resp10.cod == 12);

            // Argumento invalido.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp11 = controller.GestionarRecursos(null);
            Assert.IsTrue(resp11.cod == 14);

            // Extension invalido.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp12 = controller.GestionarRecursos(new DtoRecursosExtension() { idExtension = 1, recursosAsignados = new List<DtoRecurso>(), recursosNoAsignados = new List<DtoRecurso>() });
            Assert.IsTrue(resp12.cod == 0);

            // Liberar extension
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            controller.LiberarExtension(1);
            controller2.Request = new HttpRequestMessage();
            controller2.Request.Headers.Add("auth", token);
            controller2.CerrarSesion();
        }


        /// <summary>
        /// Test actualizar segunda categoria.
        /// </summary>
        [Test]
        public void ActualizarSegundaCategoriaTest()
        {
            AppDomain.CurrentDomain.SetData(
            "DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));
            EmsysContext db = new EmsysContext();
            db.Usuarios.FirstOrDefault(us => us.NombreLogin == "A").Token = null;
            db.Recursos.FirstOrDefault().Estado = Emsys.DataAccesLayer.Model.EstadoRecurso.Disponible;
            db.SaveChanges();

            var controller = new EventosController();

            DtoActualizarDescripcion descr = new DtoActualizarDescripcion() { descripcion = "descripcion", idExtension = 1 };
                       
            // No autenticado.
            controller.Request = new HttpRequestMessage();
            var resp1 = controller.ActualizarSegundaCategoria(1, 1);
            Assert.IsTrue(resp1.cod == 2);

            // Token invalido.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", "tokenInvalido");
            var resp2 = controller.ActualizarSegundaCategoria(1, 1);
            Assert.IsTrue(resp2.cod == 2);

            // Autenticar.
            DtoUsuario u = new DtoUsuario() { username = "A", password = "A" };
            var controller2 = new LoginController();
            var resp3 = controller2.Login(u);
            string token = ((DtoAutenticacion)resp3.response).accessToken;

            // No autenticado.
            controller.Request = new HttpRequestMessage();
            var respC1 = controller.GetCategorias();
            Assert.IsTrue(respC1.cod == 2);

            // Correcto.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var respC2 = controller.GetCategorias();
            Assert.IsTrue(respC2.cod == 0);

            // Loguear.
            DtoRol rol = new DtoRol() { zonas = new List<DtoZona>(), recursos = new List<DtoRecurso>() };
            DtoZona z = new DtoZona() { id = 1 };
            rol.zonas.Add(z);
            controller2.Request = new HttpRequestMessage();
            controller2.Request.Headers.Add("auth", token);
            var ok = controller2.ElegirRoles(rol);

            // Tomar extension.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            controller.TomarExtension(1);

            // Extension invalida.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp5 = controller.ActualizarSegundaCategoria(-1, 1);
            Assert.IsTrue(resp5.cod == 12);

            // Categoria invalida.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp6 = controller.ActualizarSegundaCategoria(1, -2);
            Assert.IsTrue(resp6.cod == 18);

            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp7 = controller.ActualizarSegundaCategoria(1, 2);
            Assert.IsTrue(resp7.cod == 0);

            // Dejo como estaba.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            controller.ActualizarSegundaCategoria(1, 1);

            // Liberar extension
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            controller.LiberarExtension(1);
            controller2.Request = new HttpRequestMessage();
            controller2.Request.Headers.Add("auth", token);
            controller2.CerrarSesion();
        }

        /// <summary>
        /// Test get zonas libres, abrir y cerrar extension.
        /// </summary>
        [Test]
        public void AbrirCerrarExtension()
        {
            AppDomain.CurrentDomain.SetData(
            "DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));
            EmsysContext db = new EmsysContext();
            db.Usuarios.FirstOrDefault(us => us.NombreLogin == "A").Token = null;
            db.Recursos.FirstOrDefault().Estado = Emsys.DataAccesLayer.Model.EstadoRecurso.Disponible;
            db.SaveChanges();

            var controller = new EventosController();

            DtoActualizarDescripcion descr = new DtoActualizarDescripcion() { descripcion = "descripcion", idExtension = 1 };

            // No autenticado.
            var resp1 = controller.GetZonasLibresEvento(1);
            Assert.IsTrue(resp1.cod == 2);

            // Token invalido.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", "tokenInvalido");
            var resp2 = controller.GetZonasLibresEvento(1);
            Assert.IsTrue(resp2.cod == 2);

            // Autenticar.
            DtoUsuario u = new DtoUsuario() { username = "A", password = "A" };
            var controller2 = new LoginController();
            var resp3 = controller2.Login(u);
            string token = ((DtoAutenticacion)resp3.response).accessToken;

            // Loguear.
            DtoRol rol = new DtoRol() { zonas = new List<DtoZona>(), recursos = new List<DtoRecurso>() };
            DtoZona z = new DtoZona() { id = 1 };
            rol.zonas.Add(z);
            controller2.Request = new HttpRequestMessage();
            controller2.Request.Headers.Add("auth", token);
            var ok = controller2.ElegirRoles(rol);

            // Tomar extension.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            controller.TomarExtension(1);

            // Extension invalida.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp5 = controller.GetZonasLibresEvento(-1);
            Assert.IsTrue(resp5.cod == 12);

            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp7 = controller.GetZonasLibresEvento(1);
            Assert.IsTrue(resp7.cod == 0);

            // No autenticado.
            controller.Request = new HttpRequestMessage();
            var resp8 = controller.AbrirExtension(1, 2);
            Assert.IsTrue(resp8.cod == 2);

            // Token invalido.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", "tokenInvalido");
            var resp9 = controller.AbrirExtension(1, 2);
            Assert.IsTrue(resp9.cod == 2);

            // Extension invalida.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp10 = controller.AbrirExtension(-1, 2);
            Assert.IsTrue(resp10.cod == 12);

            // Zona invalida.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp11 = controller.AbrirExtension(1, -1);
            Assert.IsTrue(resp11.cod == 16);

            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp12 = controller.AbrirExtension(1, 2);
            Assert.IsTrue(resp12.cod == 0);

            db = new EmsysContext();
            int nuevaExt = db.ExtensionesEvento.Max(e => e.Id);

            // Tomar extension.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            controller.TomarExtension(nuevaExt);

            // No autenticado.
            controller.Request = new HttpRequestMessage();
            var resp13 = controller.CerrarExtension(nuevaExt);
            Assert.IsTrue(resp13.cod == 2);

            // Token invalido.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", "tokenInvalido");
            var resp14 = controller.CerrarExtension(nuevaExt);
            Assert.IsTrue(resp14.cod == 2);

            // Extension invalida.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp15 = controller.CerrarExtension(-1);
            Assert.IsTrue(resp15.cod == 12);

            db.ExtensionesEvento.FirstOrDefault(ex => ex.Id == nuevaExt).Evento.Estado = Emsys.DataAccesLayer.Model.EstadoEvento.Creado;
            db.SaveChanges();

            // Evento no enviado.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp16 = controller.CerrarExtension(nuevaExt);
            Assert.IsTrue(resp16.cod == 17);

            db.ExtensionesEvento.FirstOrDefault(ex => ex.Id == nuevaExt).Evento.Estado = Emsys.DataAccesLayer.Model.EstadoEvento.Enviado;
            db.SaveChanges();

            db.ExtensionesEvento.FirstOrDefault(ex => ex.Id == nuevaExt).Recursos.Add(db.Recursos.FirstOrDefault());
            db.SaveChanges();

            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp17 = controller.CerrarExtension(nuevaExt);
            Assert.IsTrue(resp17.cod == 0);

            // Liberar extension
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            controller.LiberarExtension(1);
            controller2.Request = new HttpRequestMessage();
            controller2.Request.Headers.Add("auth", token);
            controller2.CerrarSesion();
        }

        /// <summary>
        /// Test actualizar descripcion despachador.
        /// </summary>
        [Test]
        public void ActualizarDescripcionDespachadorTest()
        {
            AppDomain.CurrentDomain.SetData(
            "DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));
            EmsysContext db = new EmsysContext();
            db.Usuarios.FirstOrDefault(us => us.NombreLogin == "A").Token = null;
            db.Recursos.FirstOrDefault().Estado = Emsys.DataAccesLayer.Model.EstadoRecurso.Disponible;
            db.SaveChanges();

            var controller = new EventosController();

            DtoActualizarDescripcion descr = new DtoActualizarDescripcion() { descripcion = "descripcion", idExtension = 1 };

            // No autenticado.
            var resp1 = controller.ActualizarDescripcionDespachador(new DtoActualizarDescripcion() { idExtension = 1, descripcion = "hola" });
            Assert.IsTrue(resp1.cod == 2);

            // Token invalido.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", "tokenInvalido");
            var resp2 = controller.ActualizarDescripcionDespachador(new DtoActualizarDescripcion() { idExtension = 1, descripcion = "hola" });
            Assert.IsTrue(resp2.cod == 2);

            // Autenticar.
            DtoUsuario u = new DtoUsuario() { username = "A", password = "A" };
            var controller2 = new LoginController();
            var resp3 = controller2.Login(u);
            string token = ((DtoAutenticacion)resp3.response).accessToken;

            // Loguear.
            DtoRol rol = new DtoRol() { zonas = new List<DtoZona>(), recursos = new List<DtoRecurso>() };
            DtoZona z = new DtoZona() { id = 1 };
            rol.zonas.Add(z);
            controller2.Request = new HttpRequestMessage();
            controller2.Request.Headers.Add("auth", token);
            var ok = controller2.ElegirRoles(rol);

            // Tomar extension.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            controller.TomarExtension(1);

            // Extension invalida.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp5 = controller.ActualizarDescripcionDespachador(new DtoActualizarDescripcion() { idExtension = -1, descripcion = "hola" });
            Assert.IsTrue(resp5.cod == 12);

            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp7 = controller.ActualizarDescripcionDespachador(new DtoActualizarDescripcion() { idExtension = 1, descripcion = "hola" });
            Assert.IsTrue(resp7.cod == 0);


            // Liberar extension
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            controller.LiberarExtension(1);
            controller2.Request = new HttpRequestMessage();
            controller2.Request.Headers.Add("auth", token);
            controller2.CerrarSesion();
        }

        /// <summary>
        /// Test adjuntar geoubicacion.
        /// </summary>
        [Test]
        public void AdjuntarGeoUbicacionTest()
        {
            AppDomain.CurrentDomain.SetData(
            "DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));
            EmsysContext db = new EmsysContext();
            db.Usuarios.FirstOrDefault(us => us.NombreLogin == "A").Token = null;
            db.Recursos.FirstOrDefault().Estado = Emsys.DataAccesLayer.Model.EstadoRecurso.Disponible;
            db.SaveChanges();

            var controller = new AdjuntosController();

            // No autenticado.
            var resp1 = controller.AdjuntarGeoUbicacion(new DtoGeoUbicacion() { latitud = 10, longitud = 10, idExtension = 1 });
            Assert.IsTrue(resp1.cod == 2);

            // Token invalido.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", "tokenInvalido");
            var resp2 = controller.AdjuntarGeoUbicacion(new DtoGeoUbicacion() { latitud = 10, longitud = 10, idExtension = 1 });
            Assert.IsTrue(resp2.cod == 2);

            // Autenticar.
            DtoUsuario u = new DtoUsuario() { username = "A", password = "A" };
            var controller2 = new LoginController();
            var resp3 = controller2.Login(u);
            string token = ((DtoAutenticacion)resp3.response).accessToken;

            // Loguear.
            DtoRol rol = new DtoRol() { zonas = new List<DtoZona>(), recursos = new List<DtoRecurso>() };
            DtoRecurso r = new DtoRecurso() { id = 1 };
            rol.recursos.Add(r);
            controller2.Request = new HttpRequestMessage();
            controller2.Request.Headers.Add("auth", token);
            var ok = controller2.ElegirRoles(rol);

            // Extension invalida.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp6 = controller.AdjuntarGeoUbicacion(new DtoGeoUbicacion() { latitud = 10, longitud = 10, idExtension = -1 });
            Assert.IsTrue(resp6.cod == 12);

            // Usuario no autorizado.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp7 = controller.AdjuntarGeoUbicacion(new DtoGeoUbicacion() { latitud = 10, longitud = 10, idExtension = 2 });
            Assert.IsTrue(resp7.cod == 15);

            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp8 = controller.AdjuntarGeoUbicacion(new DtoGeoUbicacion() { latitud = 10, longitud = 10, idExtension = 1 });
            Assert.IsTrue(resp8.cod == 0);

            controller2.Request = new HttpRequestMessage();
            controller2.Request.Headers.Add("auth", token);
            controller2.CerrarSesion();
        }

        /// <summary>
        /// Test adjuntar imagen.
        /// </summary>
        [Test]
        public void AdjuntarImagenTest()
        {
            AppDomain.CurrentDomain.SetData(
            "DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));
            EmsysContext db = new EmsysContext();
            db.Usuarios.FirstOrDefault(us => us.NombreLogin == "A").Token = null;
            db.Recursos.FirstOrDefault().Estado = Emsys.DataAccesLayer.Model.EstadoRecurso.Disponible;
            db.SaveChanges();

            var controller = new AdjuntosController();

            // No autenticado.
            var resp1 = controller.AdjuntarImagen(new DtoApplicationFile() { idExtension = 1, nombre = "algo.jpg", fileData = new byte[0] });
            Assert.IsTrue(resp1.cod == 2);

            // Token invalido.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", "tokenInvalido");
            var resp2 = controller.AdjuntarImagen(new DtoApplicationFile() { idExtension = 1, nombre = "algo.jpg", fileData = new byte[0] });
            Assert.IsTrue(resp2.cod == 2);

            // Autenticar.
            DtoUsuario u = new DtoUsuario() { username = "A", password = "A" };
            var controller2 = new LoginController();
            var resp3 = controller2.Login(u);
            string token = ((DtoAutenticacion)resp3.response).accessToken;

            // Loguear.
            DtoRol rol = new DtoRol() { zonas = new List<DtoZona>(), recursos = new List<DtoRecurso>() };
            DtoRecurso r = new DtoRecurso() { id = 1 };
            rol.recursos.Add(r);
            controller2.Request = new HttpRequestMessage();
            controller2.Request.Headers.Add("auth", token);
            var ok = controller2.ElegirRoles(rol);

            // Extension invalida.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp6 = controller.AdjuntarImagen(new DtoApplicationFile() { idExtension = -1, nombre = "algo.jpg", fileData = new byte[0] });
            Assert.IsTrue(resp6.cod == 12);

            // Usuario no autorizado.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp7 = controller.AdjuntarImagen(new DtoApplicationFile() { idExtension = 2, nombre = "algo.jpg", fileData = new byte[0] });
            Assert.IsTrue(resp7.cod == 15);

            // Formato invalido.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp8 = controller.AdjuntarImagen(new DtoApplicationFile() { idExtension = 1, nombre = "algo.algo", fileData = new byte[0] });
            Assert.IsTrue(resp8.cod == 13);

            // Imagen invalida.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp9 = controller.AdjuntarImagen(null);
            Assert.IsTrue(resp9.cod == 2002);

            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp10 = controller.AdjuntarImagen(new DtoApplicationFile() { idExtension = 1, nombre = "algo.jpg", fileData = new byte[0] });
            Assert.IsTrue(resp10.cod == 0);

            // No autenticado.
            controller.Request = new HttpRequestMessage();
            var resp11 = controller.GetImageData(1);
            Assert.IsTrue(resp11.StatusCode == HttpStatusCode.Unauthorized);
            var resp11T = controller.GetImageThumbnail(1);
            Assert.IsTrue(resp11T.StatusCode == HttpStatusCode.Unauthorized);

            // Token invalido.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", "tokenInvalido");
            var resp12 = controller.GetImageData(1);
            Assert.IsTrue(resp12.StatusCode == HttpStatusCode.Unauthorized);
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", "tokenInvalido");
            var resp12T = controller.GetImageThumbnail(1);
            Assert.IsTrue(resp12T.StatusCode == HttpStatusCode.Unauthorized);

            // Imagen invalida.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp14 = controller.GetImageData(-1);
            Assert.IsTrue(resp14.StatusCode == HttpStatusCode.NotFound);
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp14T = controller.GetImageThumbnail(-1);
            Assert.IsTrue(resp14T.StatusCode == HttpStatusCode.NotFound);

            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp16 = controller.GetImageData(1);
            Assert.IsTrue(resp16.StatusCode == HttpStatusCode.OK);
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp16T = controller.GetImageThumbnail(1);
            Assert.IsTrue(resp16T.StatusCode == HttpStatusCode.OK);

            controller2.Request = new HttpRequestMessage();
            controller2.Request.Headers.Add("auth", token);
            controller2.CerrarSesion();

            // Autenticar.
            var resp15 = controller2.Login(u);
            token = ((DtoAutenticacion)resp15.response).accessToken;

            // Usuario no autorizado.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp13 = controller.GetImageData(1);
            Assert.IsTrue(resp13.StatusCode == HttpStatusCode.Unauthorized);
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp13T = controller.GetImageThumbnail(1);
            Assert.IsTrue(resp13T.StatusCode == HttpStatusCode.Unauthorized);
        }


        /// <summary>
        /// Test adjuntar video.
        /// </summary>
        [Test]
        public void AdjuntarVideoTest()
        {
            AppDomain.CurrentDomain.SetData(
            "DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));
            EmsysContext db = new EmsysContext();
            db.Usuarios.FirstOrDefault(us => us.NombreLogin == "A").Token = null;
            db.Recursos.FirstOrDefault().Estado = Emsys.DataAccesLayer.Model.EstadoRecurso.Disponible;
            db.SaveChanges();

            var controller = new AdjuntosController();

            // No autenticado.
            var resp1 = controller.AdjuntarVideo(new DtoApplicationFile() { idExtension = 1, nombre = "algo.mp4", fileData = new byte[0] });
            Assert.IsTrue(resp1.cod == 2);

            // Token invalido.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", "tokenInvalido");
            var resp2 = controller.AdjuntarVideo(new DtoApplicationFile() { idExtension = 1, nombre = "algo.mp4", fileData = new byte[0] });
            Assert.IsTrue(resp2.cod == 2);

            // Autenticar.
            DtoUsuario u = new DtoUsuario() { username = "A", password = "A" };
            var controller2 = new LoginController();
            var resp3 = controller2.Login(u);
            string token = ((DtoAutenticacion)resp3.response).accessToken;

            // Loguear.
            DtoRol rol = new DtoRol() { zonas = new List<DtoZona>(), recursos = new List<DtoRecurso>() };
            DtoRecurso r = new DtoRecurso() { id = 1 };
            rol.recursos.Add(r);
            controller2.Request = new HttpRequestMessage();
            controller2.Request.Headers.Add("auth", token);
            var ok = controller2.ElegirRoles(rol);

            // Extension invalida.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp6 = controller.AdjuntarVideo(new DtoApplicationFile() { idExtension = -1, nombre = "algo.mp4", fileData = new byte[0] });
            Assert.IsTrue(resp6.cod == 12);

            // Usuario no autorizado.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp7 = controller.AdjuntarVideo(new DtoApplicationFile() { idExtension = 2, nombre = "algo.mp4", fileData = new byte[0] });
            Assert.IsTrue(resp7.cod == 15);

            // Formato invalido.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp8 = controller.AdjuntarVideo(new DtoApplicationFile() { idExtension = 1, nombre = "algo.algo", fileData = new byte[0] });
            Assert.IsTrue(resp8.cod == 13);

            // Imagen invalida.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp9 = controller.AdjuntarVideo(null);
            Assert.IsTrue(resp9.cod == 2003);

            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp10 = controller.AdjuntarVideo(new DtoApplicationFile() { idExtension = 1, nombre = "algo.mp4", fileData = new byte[0] });
            Assert.IsTrue(resp10.cod == 0);

            // No autenticado.
            controller.Request = new HttpRequestMessage();
            var resp11 = controller.GetVideoData(1);
            Assert.IsTrue(resp11.cod == 2);

            // Token invalido.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", "tokenInvalido");
            var resp12 = controller.GetVideoData(1);
            Assert.IsTrue(resp12.cod == 2);

            // Imagen invalida.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp14 = controller.GetVideoData(-1);
            Assert.IsTrue(resp14.cod == 102);

            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp16 = controller.GetVideoData(1);
            Assert.IsTrue(resp16.cod == 0);

            controller2.Request = new HttpRequestMessage();
            controller2.Request.Headers.Add("auth", token);
            controller2.CerrarSesion();

            // Autenticar.
            var resp15 = controller2.Login(u);
            token = ((DtoAutenticacion)resp15.response).accessToken;

            // Usuario no autorizado.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp13 = controller.GetVideoData(1);
            Assert.IsTrue(resp13.cod == 15);


        }


        /// <summary>
        /// Test adjuntar audio.
        /// </summary>
        [Test]
        public void PruebaConfiguraciones()
        {
            var Servicio1ResponseBody = new Servicios.ServicioExterno.Servicio1ResponseBody();
            Servicio1ResponseBody = new Servicios.ServicioExterno.Servicio1ResponseBody(new Servicios.ServicioExterno.ArrayOfString[1]);
            var servicio1Response = new Servicios.ServicioExterno.Servicio1Response();
            servicio1Response = new Servicios.ServicioExterno.Servicio1Response(new Servicios.ServicioExterno.Servicio1ResponseBody());
            var servicio1RequestBody = new Servicios.ServicioExterno.Servicio1RequestBody();
            servicio1RequestBody = new Servicios.ServicioExterno.Servicio1RequestBody("", "", "");
            var Servicio1Request = new Servicios.ServicioExterno.Servicio1Request();
            Servicio1Request = new Servicios.ServicioExterno.Servicio1Request(new Servicios.ServicioExterno.Servicio1RequestBody());
            // var serviciosSoapClient = new Servicios.ServicioExterno.ServiciosSoapClient();
            // var serviciosSoapClient = new Servicios.ServicioExterno.ServiciosSoapClient("");
            // serviciosSoapClient = new Servicios.ServicioExterno.ServiciosSoapClient("", "");
            //  serviciosSoapClient = new Servicios.ServicioExterno.ServiciosSoapClient("", new System.ServiceModel.EndpointAddress(""));
            // serviciosSoapClient = new Servicios.ServicioExterno.ServiciosSoapClient();
            //    var dos = serviciosSoapClient.Servicio1("", "", "");
            // var uno = serviciosSoapClient.Servicio1Async("", "", "");
            // var filtros = new Servicios.Filtros.LogFilter();
            //filtros.OnActionExecuting(new HttpActionContext());
            var CustomAuthorizeAttribute = new Servicios.Filtros.CustomAuthorizeAttribute("hola");
            CustomAuthorizeAttribute.OnAuthorization(new HttpActionContext());
            var RequireHttpsAttribute = new Emsys.ServiceLayer.Filtros.RequireHttpsAttribute();
            //RequireHttpsAttribute.OnAuthorization(new HttpActionContext());
            var delegateHandler = new Emsys.ServiceLayer.Filtros.DelegateHandler();
            //  Emsys.ServiceLayer.SwaggerConfig.Register();
            Emsys.ServiceLayer.WebApiConfig.Register(new System.Web.Http.HttpConfiguration());
            Emsys.ServiceLayer.SwaggerConfig.Register();
            var servicioExternoController= new Servicios.Controllers.ServicioExternoController();
            servicioExternoController.ConsumirServicioExterno(new DtoConsultaExterna { param1="", param2="",param3=""});
            Utils.Notifications.Utils.LogsManager.AgregarLogNotificationDessuscripcionUsuario("vacio", "servidor", "Utils.Notitications",
                        "NotificacionesFirebase", 0, "sendNotification",
                        "Se genero una notificacion exitosamente.",
                        901,
                        "", "", "");
            Assert.IsTrue(true);
        }
        /// <summary>
        /// Test adjuntar audio.
        /// </summary>
        [Test]
        public void AdjuntarAudioTest()
        {
            AppDomain.CurrentDomain.SetData(
            "DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));
            EmsysContext db = new EmsysContext();
            db.Usuarios.FirstOrDefault(us => us.NombreLogin == "A").Token = null;
            db.Recursos.FirstOrDefault().Estado = Emsys.DataAccesLayer.Model.EstadoRecurso.Disponible;
            db.SaveChanges();

            var controller = new AdjuntosController();

            // No autenticado.
            var resp1 = controller.AdjuntarAudio(new DtoApplicationFile() { idExtension = 1, nombre = "algo.mp3", fileData = new byte[0] });
            Assert.IsTrue(resp1.cod == 2);

            // Token invalido.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", "tokenInvalido");
            var resp2 = controller.AdjuntarAudio(new DtoApplicationFile() { idExtension = 1, nombre = "algo.mp3", fileData = new byte[0] });
            Assert.IsTrue(resp2.cod == 2);

            // Autenticar.
            DtoUsuario u = new DtoUsuario() { username = "A", password = "A" };
            var controller2 = new LoginController();
            var resp3 = controller2.Login(u);
            string token = ((DtoAutenticacion)resp3.response).accessToken;

            // Loguear.
            DtoRol rol = new DtoRol() { zonas = new List<DtoZona>(), recursos = new List<DtoRecurso>() };
            DtoRecurso r = new DtoRecurso() { id = 1 };
            rol.recursos.Add(r);
            controller2.Request = new HttpRequestMessage();
            controller2.Request.Headers.Add("auth", token);
            var ok = controller2.ElegirRoles(rol);

            // Extension invalida.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp6 = controller.AdjuntarAudio(new DtoApplicationFile() { idExtension = -1, nombre = "algo.mp3", fileData = new byte[0] });
            Assert.IsTrue(resp6.cod == 12);

            // Usuario no autorizado.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp7 = controller.AdjuntarAudio(new DtoApplicationFile() { idExtension = 2, nombre = "algo.mp3", fileData = new byte[0] });
            Assert.IsTrue(resp7.cod == 15);

            // Formato invalido.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp8 = controller.AdjuntarAudio(new DtoApplicationFile() { idExtension = 1, nombre = "algo.algo", fileData = new byte[0] });
            Assert.IsTrue(resp8.cod == 13);

            // Imagen invalida.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp9 = controller.AdjuntarAudio(null);
            Assert.IsTrue(resp9.cod == 2004);

            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp10 = controller.AdjuntarAudio(new DtoApplicationFile() { idExtension = 1, nombre = "algo.mp3", fileData = new byte[0] });
            Assert.IsTrue(resp10.cod == 0);

            // No autenticado.
            controller.Request = new HttpRequestMessage();
            var resp11 = controller.GetAudioData(1);
            Assert.IsTrue(resp11.cod == 2);

            // Token invalido.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", "tokenInvalido");
            var resp12 = controller.GetAudioData(1);
            Assert.IsTrue(resp12.cod == 2);

            // Imagen invalida.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp14 = controller.GetAudioData(-1);
            Assert.IsTrue(resp14.cod == 103);

            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp16 = controller.GetAudioData(1);
            Assert.IsTrue(resp16.cod == 0);

            controller2.Request = new HttpRequestMessage();
            controller2.Request.Headers.Add("auth", token);
            controller2.CerrarSesion();

            // Autenticar.
            var resp15 = controller2.Login(u);
            token = ((DtoAutenticacion)resp15.response).accessToken;

            // Usuario no autorizado.
            controller.Request = new HttpRequestMessage();
            controller.Request.Headers.Add("auth", token);
            var resp13 = controller.GetAudioData(1);
            Assert.IsTrue(resp13.cod == 15);
        }


        ///// <summary>
        ///// Prueba servicio externo.
        ///// </summary>
        //[Test]
        //public void ServicioExternoTesto()
        //{
        //    ServidorExterno.Servicios s = new ServidorExterno.Servicios();
        //    var controller = new ServicioExternoController();
        //    var resp = controller.ConsumirServicioExterno(new DtoConsultaExterna() { param1 = "uno", param2 = "dos", param3 = "tres" });
        //    var lista = (List<DtoRespuestaExterna>)resp.response;
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
