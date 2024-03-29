﻿using System;
using System.Linq;
using Emsys.DataAccesLayer.Core;
using NUnit.Framework;
using System.IO;
using Emsys.LogicLayer;
using Emsys.DataAccesLayer.Model;
using System.Collections.Generic;
using Emsys.LogicLayer.Utils;
using System.Data.Entity.Validation;
using DataTypeObject;
using Emsys.LogicLayer.ApplicationExceptions;
using System.Threading;
using Utils.Notifications.Utils;

namespace Test.UnitTesting
{
    [TestFixture]
    public class LogicLayerUnitTest
    {
        /// <summary>
        /// Agrega un usuario y luego ejecuta el metodo de autenticacion
        /// con credenciales validas y credenciales invalidas.
        /// </summary>
        [Test]
        public void AutenticarUsuarioTest()
        {
            using (var context = new EmsysContext())
            {
                AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));
                string pass = Passwords.GetSHA1("usuarioPruebaAutenticar");
                Usuario usuarioPrueba = new Usuario { NombreLogin = "usuarioPruebaAutenticar", Contraseña = pass };
                context.Usuarios.Add(usuarioPrueba);
                context.SaveChanges();
                IMetodos logica = new Metodos();
                try
                {
                    logica.autenticarUsuario("usuarioPruebaAutenticar", "incorrecto", null);
                    Assert.Fail();
                }
                catch (CredencialesInvalidasException)
                {
                    Assert.IsTrue(true);
                }

                try
                {
                    var result = logica.autenticarUsuario("usuarioPruebaAutenticar", "usuarioPruebaAutenticar", null);
                    Assert.IsNotNull(result);
                }
                catch (CredencialesInvalidasException)
                {
                    Assert.Fail();
                }

                try
                {
                    var result = logica.autenticarUsuario("usuarioPruebaAutenticar", "usuarioPruebaAutenticar", null);
                }
                catch (SesionActivaException)
                {
                    Assert.IsTrue(true);
                }
            }
        }

        /// <summary>
        /// Agrega un usuario y luego ejecuta el metodo de autenticacion
        /// con credenciales validas y credenciales invalidas.
        /// </summary>
        [Test]
        public void AutorizarUsuarioTest()
        {
            using (var context = new EmsysContext())
            {
                AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));

                var user = new Usuario() { NombreLogin = "usuario", Nombre = "pepe", Contraseña = Emsys.LogicLayer.Utils.Passwords.GetSHA1("pruebapass") };
                context.Usuarios.Add(user);

                var permiso = new Permiso() { Clave = "pruebaPermiso", Roles = new List<Rol>() };
                context.Permisos.Add(permiso);

                var rol = new Rol() { Nombre = "pruebaRol", Permisos = new List<Permiso>(), Usuarios = new List<Usuario>() };
                context.ApplicationRoles.Add(rol);

                rol.Permisos.Add(permiso);
                rol.Usuarios.Add(user);

                IMetodos logica = new Metodos();

                context.SaveChanges();

                var autent = logica.autenticarUsuario("usuario", "pruebapass", null);
                string token = autent.accessToken;

                string[] etiqueta1 = { "permisoFalso" };
                Assert.IsFalse(logica.autorizarUsuario(token, etiqueta1));

                string[] etiqueta2 = { "pruebaPermiso" };
                Assert.IsTrue(logica.autorizarUsuario(token, etiqueta2));

                context.Usuarios.FirstOrDefault(u => u.NombreLogin == "usuario").FechaInicioSesion = DateTime.Parse("2015/07/23 21:30:00");
                context.SaveChanges();

                logica.autorizarUsuario(token, new string[0]);
                Assert.IsTrue(context.Usuarios.FirstOrDefault(u => u.NombreLogin == "usuario").Token == null);
            }
        }

        /// <summary>
        /// Test que prueba el logueo del usuario mediante un recurso teniendo en cuenta las posibilidades
        /// si el recurso es accesible o no para el usuario y si este se encuentra o no disponible.
        /// </summary>
        [Test]
        public void LoguearUsuarioRecursoTest()
        {
            using (var context = new EmsysContext())
            {
                AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Empty));

                // Se crea un usuario con un recurso asociado en la BD.
                var user = new Usuario() { NombreLogin = "usuarioPruebaRecurso", Nombre = "usuarioPruebaRecurso", Contraseña = Passwords.GetSHA1("usuarioPruebaRecurso"), GruposRecursos = new List<GrupoRecurso>() };
                var user2 = new Usuario() { NombreLogin = "usuarioPruebaRecursoNoDisponible", Nombre = "usuarioPruebaRecursoNoDisponible", Contraseña = Passwords.GetSHA1("usuarioPruebaRecursoNoDisponible"), GruposRecursos = new List<GrupoRecurso>() };
                var recursoDisponible = new Recurso() { Codigo = "recursoPruebaDisponible", Estado = EstadoRecurso.Disponible, EstadoAsignacion = EstadoAsignacionRecurso.Libre };
                var recursoNoSeleccionable = new Recurso() { Codigo = "recursoNoSeleccionable", Estado = EstadoRecurso.Disponible, EstadoAsignacion = EstadoAsignacionRecurso.Libre };
                var gr = new GrupoRecurso() { Nombre = "grPrueba", Recursos = new List<Recurso>() };
                gr.Recursos.Add(recursoDisponible);
                user.GruposRecursos.Add(gr);
                user2.GruposRecursos.Add(gr);
                context.Recursos.Add(recursoDisponible);
                context.Recursos.Add(recursoNoSeleccionable);
                context.GruposRecursos.Add(gr);
                context.Usuarios.Add(user);
                context.Usuarios.Add(user2);
                context.SaveChanges();

                IMetodos logica = new Metodos();

                // Obtengo token de usuario. 
                var autent = logica.autenticarUsuario("usuarioPruebaRecurso", "usuarioPruebaRecurso", null);
                string token = autent.accessToken;

                // Recurso seleccionable por el usuario y disponible.
                List<DtoRecurso> listaRecurso = new List<DtoRecurso>();
                DtoRecurso dtoRecurso = new DtoRecurso() { id = recursoDisponible.Id, codigo = "recursoPruebaDisponible" };
                listaRecurso.Add(dtoRecurso);
                DtoRol rol = new DtoRol() { recursos = listaRecurso, zonas = new List<DtoZona>() };

                try
                {
                    logica.loguearUsuario(null, rol);
                }
                catch (TokenInvalidoException)
                {
                    Assert.IsTrue(true);
                }

                try
                {
                    logica.loguearUsuario("tokenIncorrecto", rol);
                }
                catch (TokenInvalidoException)
                {
                    Assert.IsTrue(true);
                }

                try
                {
                    Assert.IsTrue(logica.loguearUsuario(token, rol));
                }
                catch (RecursoNoDisponibleException)
                {
                    Assert.Fail();
                }

                //// Pruebo loguearme con otro usuario que tenga el mismo recurso asignado
                var autent2 = logica.autenticarUsuario("usuarioPruebaRecursoNoDisponible", "usuarioPruebaRecursoNoDisponible", null);
                string token2 = autent2.accessToken;
                try
                {
                    logica.loguearUsuario(token2, rol);
                    Assert.Fail();
                }
                catch (RecursoNoDisponibleException)
                {
                }

                // Recurso no seleccionable por el usuario.
                List<DtoRecurso> lRecurso2 = new List<DtoRecurso>();
                DtoRecurso dtoRecurso2 = new DtoRecurso() { id = recursoNoSeleccionable.Id, codigo = "recursoNoSeleccionable" };
                lRecurso2.Add(dtoRecurso2);
                DtoRol rol2 = new DtoRol() { recursos = lRecurso2, zonas = new List<DtoZona>() };
                try
                {
                    Assert.IsFalse(logica.loguearUsuario(token2, rol2));
                }
                catch (RecursoNoDisponibleException)
                {
                    Assert.Fail();
                }
            }
        }

        /// <summary>
        /// Test que prueba el logueo del usuario mediante zonas considerando las posibilidades
        /// si el usuario tiene acceso o no a las unidades ejecutoras de las zonas.
        /// </summary>
        [Test]
        public void loguearUsuarioZonaTest()
        {
            using (var context = new EmsysContext())
            {
                AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));

                // Se crea un usuario con zonas asociadas en la BD.
                var user = new Usuario() { NombreLogin = "usuarioPruebaZonas", Nombre = "usuarioPruebaZonas", Contraseña = Passwords.GetSHA1("usuarioPruebaZonas"), GruposRecursos = new List<GrupoRecurso>(), UnidadesEjecutoras = new List<UnidadEjecutora>() };
                var zona1 = new Zona() { Nombre = "zona1" };
                var zona2 = new Zona() { Nombre = "zona2" };
                var zona3 = new Zona() { Nombre = "zona3" };
                var zona4 = new Zona() { Nombre = "zona4" };
                var unidadEjecutora1 = new UnidadEjecutora() { Nombre = "uePrueba", Zonas = new List<Zona>() };
                var unidadEjecutora2 = new UnidadEjecutora() { Nombre = "uePrueba2", Zonas = new List<Zona>() };
                var unidadEjecutora3 = new UnidadEjecutora() { Nombre = "uePrueba3", Zonas = new List<Zona>() };
                unidadEjecutora1.Zonas.Add(zona1);
                unidadEjecutora1.Zonas.Add(zona2);
                unidadEjecutora2.Zonas.Add(zona3);
                unidadEjecutora3.Zonas.Add(zona4);
                zona1.UnidadEjecutora = unidadEjecutora1;
                zona2.UnidadEjecutora = unidadEjecutora1;
                zona3.UnidadEjecutora = unidadEjecutora2;
                zona4.UnidadEjecutora = unidadEjecutora3;
                user.UnidadesEjecutoras.Add(unidadEjecutora1);
                user.UnidadesEjecutoras.Add(unidadEjecutora2);
                context.Zonas.Add(zona1);
                context.Zonas.Add(zona2);
                context.Zonas.Add(zona3);
                context.Zonas.Add(zona4);
                context.UnidadesEjecutoras.Add(unidadEjecutora1);
                context.UnidadesEjecutoras.Add(unidadEjecutora2);
                context.UnidadesEjecutoras.Add(unidadEjecutora3);
                context.Usuarios.Add(user);
                try
                {
                    context.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    throw e;
                }

                IMetodos logica = new Metodos();

                //// Obtengo token de usuario. 
                var autent = logica.autenticarUsuario("usuarioPruebaZonas", "usuarioPruebaZonas", null);
                string token = autent.accessToken;

                //// Usuario pertenece a todas las unidades ejecutoras de las zonas.
                List<DtoZona> listaZonas = new List<DtoZona>();
                DtoZona dtoZona1 = new DtoZona() { id = zona1.Id, nombre = "zona1" };
                DtoZona dtoZona2 = new DtoZona() { id = zona2.Id, nombre = "zona2" };
                DtoZona dtoZona3 = new DtoZona() { id = zona3.Id, nombre = "zona3" };
                DtoZona dtoZona4 = new DtoZona() { id = zona4.Id, nombre = "zona4" };
                listaZonas.Add(dtoZona1);
                listaZonas.Add(dtoZona2);
                listaZonas.Add(dtoZona3);
                DtoRol rol = new DtoRol() { recursos = new List<DtoRecurso>(), zonas = listaZonas };

                try
                {
                    Assert.IsTrue(logica.loguearUsuario(token, rol));
                }
                catch (RecursoNoDisponibleException)
                {
                    Assert.Fail();
                }

                // Usuario se quiere loguear con una zona que no pertenece a ninguna de sus unidades ejecutoras.
                listaZonas.Add(dtoZona4);

                DtoRol rol2 = new DtoRol() { recursos = new List<DtoRecurso>(), zonas = listaZonas };

                try
                {
                    Assert.IsFalse(logica.loguearUsuario(token, rol2));
                }
                catch (RecursoNoDisponibleException)
                {
                    Assert.Fail();
                }
            }
        }

        /// <summary>
        /// Test que prueba el metodo de cerrar sesión para usuarios que se hayan logueado como recurso.
        /// </summary>
        [Test]
        public void CerrarSesionRecursoTest()
        {
            using (var context = new EmsysContext())
            {
                // Se crea un usuario con un recurso asociado en la BD.
                var user = new Usuario() { NombreLogin = "usuarioPruebaCerrarSesion", Nombre = "usuarioPruebaCerrarSesion", Contraseña = Passwords.GetSHA1("usuarioPruebaCerrarSesion"), GruposRecursos = new List<GrupoRecurso>() };
                var recursoDisponible = new Recurso() { Codigo = "recursoPruebaCerrarSesion", Estado = EstadoRecurso.Disponible, EstadoAsignacion = EstadoAsignacionRecurso.Libre };
                var gr = new GrupoRecurso() { Nombre = "grPruebaCerrarSesion", Recursos = new List<Recurso>() };
                gr.Recursos.Add(recursoDisponible);
                user.GruposRecursos.Add(gr);
                context.Recursos.Add(recursoDisponible);
                context.GruposRecursos.Add(gr);
                context.Usuarios.Add(user);
                context.SaveChanges();

                IMetodos logica = new Metodos();

                // Obtengo token de usuario. 
                var autent = logica.autenticarUsuario("usuarioPruebaCerrarSesion", "usuarioPruebaCerrarSesion", null);
                string token = autent.accessToken;

                try
                {
                    logica.cerrarSesion(null);
                }
                catch (TokenInvalidoException)
                {
                    Assert.IsTrue(true);
                }

                try
                {
                    logica.cerrarSesion("tokenIncorrecto");
                }
                catch (TokenInvalidoException)
                {
                    Assert.IsTrue(true);
                }

                // Logueo al usuario.
                List<DtoRecurso> lRecurso = new List<DtoRecurso>();
                DtoRecurso dtoRecurso = new DtoRecurso() { id = recursoDisponible.Id, codigo = "recursoPruebaCerrarSesion" };
                lRecurso.Add(dtoRecurso);
                DtoRol rol = new DtoRol() { recursos = lRecurso, zonas = new List<DtoZona>() };
                if (logica.loguearUsuario(token, rol))
                {
                    logica.cerrarSesion(token);

                    // Compruebo que se haya liberado el recurso asignado.
                    var tieneRecurso = context.Usuarios.Where(x => x.Id == user.Id).Select(x => x.Recurso.FirstOrDefault());
                    Assert.IsNull(tieneRecurso.FirstOrDefault());

                    // Compruebo que el recurso haya quedado disponible.
                    EstadoRecurso estaDisponible = context.Recursos.Where(x => x.Id == recursoDisponible.Id).Select(x => x.Estado).FirstOrDefault();
                    Assert.AreEqual(EstadoRecurso.Disponible, estaDisponible);
                }
            }
        }

        /// <summary>
        /// Test que prueba el metodo de cerrar sesión para usuarios que se hayan logueado como zona.
        /// </summary>
        [Test]
        public void CerrarSesionZonasTest()
        {
            using (var context = new EmsysContext())
            {
                AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Empty));

                // Se crea un usuario con zonas asociadas en la BD.
                var user = new Usuario() { NombreLogin = "usuarioPruebaZonasCerrarSesion", Nombre = "usuarioPruebaZonasCerrarSesion", Contraseña = Passwords.GetSHA1("usuarioPruebaZonasCerrarSesion"), GruposRecursos = new List<GrupoRecurso>(), UnidadesEjecutoras = new List<UnidadEjecutora>() };
                var zona1 = new Zona() { Nombre = "zona1CerrarSesion" };
                var zona2 = new Zona() { Nombre = "zona2CerrarSesion" };
                var zona3 = new Zona() { Nombre = "zona3CerrarSesion" };
                var unidadEjecutora1 = new UnidadEjecutora() { Nombre = "uePruebaCerrarSesion", Zonas = new List<Zona>() };
                var unidadEjecutora2 = new UnidadEjecutora() { Nombre = "uePrueba2CerrarSesion", Zonas = new List<Zona>() };
                var unidadEjecutora3 = new UnidadEjecutora() { Nombre = "uePrueba3CerrarSesion", Zonas = new List<Zona>() };
                unidadEjecutora1.Zonas.Add(zona1);
                unidadEjecutora1.Zonas.Add(zona2);
                unidadEjecutora2.Zonas.Add(zona3);
                zona1.UnidadEjecutora = unidadEjecutora1;
                zona2.UnidadEjecutora = unidadEjecutora1;
                zona3.UnidadEjecutora = unidadEjecutora2;
                user.UnidadesEjecutoras.Add(unidadEjecutora1);
                user.UnidadesEjecutoras.Add(unidadEjecutora2);
                context.Zonas.Add(zona1);
                context.Zonas.Add(zona2);
                context.Zonas.Add(zona3);
                context.UnidadesEjecutoras.Add(unidadEjecutora1);
                context.UnidadesEjecutoras.Add(unidadEjecutora2);
                context.UnidadesEjecutoras.Add(unidadEjecutora3);
                context.Usuarios.Add(user);
                context.SaveChanges();

                IMetodos logica = new Metodos();

                // Obtengo token de usuario. 
                var autent = logica.autenticarUsuario("usuarioPruebaZonasCerrarSesion", "usuarioPruebaZonasCerrarSesion", null);
                string token = autent.accessToken;

                // Usuario pertenece a todas las unidades ejecutoras de las zonas.
                List<DtoZona> listaZonas = new List<DtoZona>();
                DtoZona dtoZona1 = new DtoZona() { id = zona1.Id, nombre = "zona1CerrarSesion" };
                DtoZona dtoZona2 = new DtoZona() { id = zona2.Id, nombre = "zona2CerrarSesion" };
                DtoZona dtoZona3 = new DtoZona() { id = zona3.Id, nombre = "zona3CerrarSesion" };
                listaZonas.Add(dtoZona1);
                listaZonas.Add(dtoZona2);
                listaZonas.Add(dtoZona3);
                DtoRol rol = new DtoRol() { recursos = new List<DtoRecurso>(), zonas = listaZonas };

                try
                {
                    if (logica.loguearUsuario(token, rol))
                    {
                        logica.cerrarSesion(token);

                        // Compruebo que se hayan liberado las zonas asignadas.
                        var tieneZonasAsignadas = context.Usuarios.Where(x => x.Id == user.Id).Select(x => x.Zonas.FirstOrDefault());
                        Assert.IsNull(tieneZonasAsignadas.FirstOrDefault());
                    }
                }
                catch (RecursoNoDisponibleException)
                {
                    Assert.Fail();
                }
            }
        }

        /// <summary>
        /// Crea un evento con una extension que tenga descripcion despachador
        /// y se verifica que se devuelvan los DtoDescripcion correctamente armados
        /// luego de llamar al getEvento
        /// </summary>
        [Test]
        public void getDescripcionEventoTest()
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));

            using (var context = new EmsysContext())
            {
                // Se crea un usuario con un recurso asociado en la BD.
                var user = new Usuario() { NombreLogin = "usuarioDE", Nombre = "usuarioDE", Contraseña = Passwords.GetSHA1("usuarioDE"), GruposRecursos = new List<GrupoRecurso>(), UnidadesEjecutoras = new List<UnidadEjecutora>() };
                var recursoDisponible = new Recurso() { Codigo = "recursoDE", Estado = EstadoRecurso.Disponible, EstadoAsignacion = EstadoAsignacionRecurso.Libre, AsignacionesRecurso = new List<AsignacionRecurso>() };
                var gr = new GrupoRecurso() { Nombre = "grDEPrueba", Recursos = new List<Recurso>() };
                var zona1 = new Zona() { Nombre = "zonaDE1" };
                var unidadEjecutora1 = new UnidadEjecutora() { Nombre = "ueDEPrueba", Zonas = new List<Zona>() };

                unidadEjecutora1.Zonas.Add(zona1);
                zona1.UnidadEjecutora = unidadEjecutora1;
                user.UnidadesEjecutoras.Add(unidadEjecutora1);

                gr.Recursos.Add(recursoDisponible);
                user.GruposRecursos.Add(gr);
                context.Zonas.Add(zona1);

                context.UnidadesEjecutoras.Add(unidadEjecutora1);
                context.Recursos.Add(recursoDisponible);
                context.GruposRecursos.Add(gr);
                context.Usuarios.Add(user);
                context.SaveChanges();

                // Evento y extensiones
                var sector = new Sector() { Nombre = "sectorDEPrueba", Zona = zona1 };
                var catEvento = new Categoria() { Clave = "catPruebaDE", Activo = true, Codigo = "catDE", Prioridad = NombrePrioridad.Media };
                var evento = new Evento()
                {
                    NombreInformante = "PruebaDE",
                    TelefonoEvento = "PruebaDE",
                    Estado = EstadoEvento.Enviado,
                    Categoria = catEvento,
                    TimeStamp = DateTime.Now,
                    FechaCreacion = DateTime.Now,
                    Sector = sector,
                    EnProceso = true,
                    Numero = "PruebaDE",
                    Audios = new List<Audio>(),
                    Calle = "PruebaDE",
                    Esquina = "PruebaDE",
                    Imagenes = new List<Imagen>(),
                    Latitud = 0,
                    Longitud = 0,
                    Videos = new List<Video>(),
                    Descripcion = "PruebaDE"
                };

                var ext1 = new ExtensionEvento()
                {
                    Evento = evento,
                    Zona = zona1,
                    Estado = EstadoExtension.Despachado,
                    TimeStamp = DateTime.Now,
                    Despachador = user,
                    DescripcionDespachador = "2016/07/23 21:30:00\\UsuarioDespachador\\descripcion de evento\\2016/07/23 21:37:00\\UsuarioDespachador2\\otra descripcion de evento\\2016/07/24 10:37:00\\UsuarioDespachador2\\otra mas"
                };

                IMetodos logica = new Metodos();
                var u = context.Usuarios.Where(x => x.NombreLogin == "usuarioDE").FirstOrDefault();
                if (u != null && u.Token != null)
                {
                    u.Token = null;
                }
                
                recursoDisponible.AsignacionesRecurso.Add(new AsignacionRecurso() { ActualmenteAsignado = true, Extension = ext1, Recurso = recursoDisponible });
                try
                {
                    context.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    throw e;
                }

                // Obtengo token de usuario. 
                var autent = logica.autenticarUsuario("usuarioDE", "usuarioDE", null);
                string token = autent.accessToken;

                List<DtoRecurso> listaRecurso = new List<DtoRecurso>();
                DtoRecurso dtoRecurso = new DtoRecurso() { id = recursoDisponible.Id, codigo = "recursoListarEvento" };
                listaRecurso.Add(dtoRecurso);
                DtoRol rol = new DtoRol() { recursos = listaRecurso, zonas = new List<DtoZona>() };

                try
                {
                    logica.verInfoEvento(null, 1);
                }
                catch (TokenInvalidoException)
                {
                    Assert.IsTrue(true);
                }

                try
                {
                    logica.verInfoEvento("tokenIncorrecto", 1);
                }
                catch (TokenInvalidoException)
                {
                    Assert.IsTrue(true);
                }

                if (logica.loguearUsuario(token, rol))
                {
                    DtoEvento dtoEvento = logica.verInfoEvento(token, evento.Id);
                    DtoExtension dtoExt = dtoEvento.extensiones.FirstOrDefault();

                    // Compruebo que existan los 3 dtoDescripcion
                    Assert.AreEqual(dtoExt.descripcionDespachadores.Count, 3);

                    // Comrpuebo que los dtos tengan el texto correspondiente
                    DateTime fecha1 = DateTime.Parse("2016/07/23 21:30:00");
                    DateTime fecha2 = DateTime.Parse("2016/07/23 21:37:00");
                    DateTime fecha3 = DateTime.Parse("2016/07/24 10:37:00");
                    for (int i = 0; i < 3; i++)
                    {
                        DtoDescripcion dtoDesc = dtoExt.descripcionDespachadores.ElementAt(i);
                        switch (i)
                        {
                            case 0:
                                Assert.AreEqual(dtoDesc.fecha, fecha1);
                                Assert.AreEqual(dtoDesc.usuario, "UsuarioDespachador");
                                break;
                            case 1:
                                Assert.AreEqual(dtoDesc.fecha, fecha2);
                                Assert.AreEqual(dtoDesc.usuario, "UsuarioDespachador2");
                                break;
                            case 2:
                                Assert.AreEqual(dtoDesc.fecha, fecha3);
                                Assert.AreEqual(dtoDesc.usuario, "UsuarioDespachador2");
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Se prueba que al realizar listar eventos se traigan todos los eventos 
        /// asociados al recurso con el que se logueo el usuario.
        /// </summary>
        [Test]
        public void ListarEventosTest()
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Empty));
            var context = new EmsysContext();

            // Se crea un usuario con un recurso asociado en la BD.
            var user = new Usuario() { NombreLogin = "usuarioListarEventoRecurso", Nombre = "usuarioListarEventoRecurso", Contraseña = Passwords.GetSHA1("usuarioListarEventoRecurso"), GruposRecursos = new List<GrupoRecurso>(), UnidadesEjecutoras = new List<UnidadEjecutora>() };
            var recursoDisponible = new Recurso() { Codigo = "recursoListarEvento", Estado = EstadoRecurso.Disponible, EstadoAsignacion = EstadoAsignacionRecurso.Libre, AsignacionesRecurso = new List<AsignacionRecurso>() };
            var gr = new GrupoRecurso() { Nombre = "grPrueba", Recursos = new List<Recurso>() };
            var zona1 = new Zona() { Nombre = "zona1" };
            var zona2 = new Zona() { Nombre = "zona2" };
            var zona3 = new Zona() { Nombre = "zona3" };
            var zona4 = new Zona() { Nombre = "zona4" };
            var unidadEjecutora1 = new UnidadEjecutora() { Nombre = "uePrueba", Zonas = new List<Zona>() };
            var unidadEjecutora2 = new UnidadEjecutora() { Nombre = "uePrueba2", Zonas = new List<Zona>() };
            var unidadEjecutora3 = new UnidadEjecutora() { Nombre = "uePrueba3", Zonas = new List<Zona>() };
            var departamento1 = new Departamento() { Nombre = "dep1" };
            unidadEjecutora1.Zonas.Add(zona1);
            unidadEjecutora1.Zonas.Add(zona2);
            unidadEjecutora2.Zonas.Add(zona3);
            unidadEjecutora3.Zonas.Add(zona4);
            zona1.UnidadEjecutora = unidadEjecutora1;
            zona2.UnidadEjecutora = unidadEjecutora1;
            zona3.UnidadEjecutora = unidadEjecutora2;
            zona4.UnidadEjecutora = unidadEjecutora3;
            user.UnidadesEjecutoras.Add(unidadEjecutora1);
            user.UnidadesEjecutoras.Add(unidadEjecutora2);
            gr.Recursos.Add(recursoDisponible);
            user.GruposRecursos.Add(gr);
            context.Zonas.Add(zona1);
            context.Zonas.Add(zona2);
            context.Zonas.Add(zona3);
            context.Zonas.Add(zona4);
            context.UnidadesEjecutoras.Add(unidadEjecutora1);
            context.UnidadesEjecutoras.Add(unidadEjecutora2);
            context.UnidadesEjecutoras.Add(unidadEjecutora3);
            context.Recursos.Add(recursoDisponible);
            context.GruposRecursos.Add(gr);
            context.Usuarios.Add(user);
            context.Departamentos.Add(departamento1);
            context.SaveChanges();

            // Evento y extensiones
            var sector = new Sector() { Nombre = "sectorPruebaLE", Zona = zona1 };
            var catEvento = new Categoria() { Clave = "catPruebaListarEvento", Activo = true, Codigo = "catPrueba", Prioridad = NombrePrioridad.Media };
            List<Imagen> imgs = new List<Imagen>();
            imgs.Add(new Imagen() { FechaEnvio = DateTime.Now, ImagenData = new ApplicationFile() { FileData = new byte[0], Nombre = "img.jpg" }, Usuario = user });
            List<Audio> auds = new List<Audio>();
            auds.Add(new Audio() { FechaEnvio = DateTime.Now, AudioData = new ApplicationFile() { FileData = new byte[0], Nombre = "aud.mp3" }, Usuario = user });
            List<Video> vids = new List<Video>();
            vids.Add(new Video() { FechaEnvio = DateTime.Now, VideoData = new ApplicationFile() { FileData = new byte[0], Nombre = "vid.mp4" }, Usuario = user });
            var evento = new Evento()
            {
                Estado = EstadoEvento.Enviado,
                Categoria = catEvento,
                TimeStamp = DateTime.Now,
                FechaCreacion = DateTime.Now,
                Sector = sector,
                EnProceso = true,
                Imagenes = imgs,
                Videos = vids,
                Audios = auds,
                Usuario = user,
                Departamento = departamento1
            };
            var ext1 = new ExtensionEvento()
            {
                Evento = evento,
                Zona = zona1,
                Estado = EstadoExtension.Despachado,
                TimeStamp = DateTime.Now,
                Despachador = user
            };
            var ext2 = new ExtensionEvento()
            {
                Evento = evento,
                Zona = zona2,
                Estado = EstadoExtension.Despachado,
                TimeStamp = DateTime.Now,
                Despachador = user
            };
            var ext3 = new ExtensionEvento()
            {
                Evento = evento,
                Zona = zona3,
                Estado = EstadoExtension.Despachado,
                TimeStamp = DateTime.Now,
                Despachador = user
            };
            IMetodos logica = new Metodos();
            var u = context.Usuarios.Where(x => x.NombreLogin == "usuarioListarEventoRecurso").FirstOrDefault();
            if (u != null && u.Token != null)
            {
                u.Token = null;
            }

            // Obtengo token de usuario. 
            var autent = logica.autenticarUsuario("usuarioListarEventoRecurso", "usuarioListarEventoRecurso", null);
            string token = autent.accessToken;

            // Se prueba que se listen las extensiones asociadas a un recurso
            recursoDisponible.AsignacionesRecurso.Add(new AsignacionRecurso() { ActualmenteAsignado = true, Extension = ext1, Recurso = recursoDisponible });
            recursoDisponible.AsignacionesRecurso.Add(new AsignacionRecurso() { ActualmenteAsignado = true, Extension = ext2, Recurso = recursoDisponible });
            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                throw e;
            }

            List<DtoRecurso> listaRecurso = new List<DtoRecurso>();
            DtoRecurso dtoRecurso = new DtoRecurso() { id = recursoDisponible.Id, codigo = "recursoListarEvento" };
            listaRecurso.Add(dtoRecurso);
            DtoRol rol = new DtoRol() { recursos = listaRecurso, zonas = new List<DtoZona>() };

            try
            {
                logica.listarEventos(null);
            }
            catch (TokenInvalidoException)
            {
                Assert.IsTrue(true);
            }

            try
            {
                logica.listarEventos("tokenIncorrecto");
            }
            catch (TokenInvalidoException)
            {
                Assert.IsTrue(true);
            }

            if (logica.loguearUsuario(token, rol))
            {
                var listaEventos = logica.listarEventos(token);
                int cantExt = listaEventos.FirstOrDefault().extensiones.Count();
                Assert.IsTrue(cantExt == 2);
            }
            logica.cerrarSesion(token);
            context = new EmsysContext();
            autent = logica.autenticarUsuario("usuarioListarEventoRecurso", "usuarioListarEventoRecurso", null);
            token = autent.accessToken;
            List<DtoZona> _zonas = new List<DtoZona>();
            var userZ = context.Usuarios.FirstOrDefault(uz => uz.NombreLogin == "usuarioListarEventoRecurso");
            userZ.Zonas.Add(context.Zonas.FirstOrDefault());
            context.SaveChanges();
            var listaEventos2 = logica.listarEventos(token);
            int cantExt2 = listaEventos2.FirstOrDefault().extensiones.Count();
            Assert.IsTrue(cantExt2 > 0);
        }

        /// <summary>
        /// Se prueba registrar un log.
        /// </summary>
        [Test]
        public void AgregarLogPositive()
        {
            AppDomain.CurrentDomain.SetData(
            "DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));
            EmsysContext db = new EmsysContext();

            var cantidadLogsPrevia = db.Logs.Count() + 1;
            int PruebaConstante = 12345678;
            string nombre = "A";
            IMetodos dbAL = new Metodos();
            dbAL.AgregarLog(nombre, "1:1:1:1", "PruebaUnitaria", "LogUnitTest", 1, "agregar log", "esto es una prueba", PruebaConstante);
            var cantidadLogsDespues = db.Logs.Count();
            Assert.True(cantidadLogsPrevia == cantidadLogsDespues);
            db.SaveChanges();
            var log = db.Logs.FirstOrDefault(x => x.Terminal == "1:1:1:1");
            Assert.NotNull(log);
        }

        /// <summary>
        /// Se prueba agregar un log de error.
        /// </summary>
        [Test]
        public void AgregarLogErrorPositive()
        {
            AppDomain.CurrentDomain.SetData(
            "DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));
            EmsysContext db = new EmsysContext();

            var cantidadLogsPrevia = db.Logs.Count() + 1;
            int PruebaConstante = 12345678;
            string nombre = Guid.NewGuid().ToString();
            IMetodos dbAL = new Metodos();
            dbAL.AgregarLogError(nombre, "2:2:2:2", "PruebaUnitaria", "LogUnitTest", 1, "agregar log", "esto es una prueba", PruebaConstante);
            var cantidadLogsDespues = db.Logs.Count();
            Assert.True(cantidadLogsPrevia == cantidadLogsDespues);
            var log = db.Logs.FirstOrDefault(x => x.Terminal == "2:2:2:2");
            Assert.NotNull(log);
        }

        /// <summary>
        /// Se prueba agregar una geoubicacion a una extension.
        /// </summary>
        [Test]
        public void AdjuntarGeoUbicacion()
        {
            AppDomain.CurrentDomain.SetData(
            "DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Empty));
            EmsysContext db = new EmsysContext();
            db.Usuarios.FirstOrDefault(us => us.NombreLogin == "A").Token = null;
            db.SaveChanges();

            int cant = db.ExtensionesEvento.FirstOrDefault().GeoUbicaciones.Count();
            IMetodos logica = new Metodos();

            // Autenticar.
            var result = logica.autenticarUsuario("A", "A", null);
            string token = result.accessToken;

            // Elegir roles.
            List<DtoRecurso> listaRecursos = new List<DtoRecurso>();
            DtoRecurso dtoRecurso1 = new DtoRecurso() { id = 1, codigo = "recurso1" };
            listaRecursos.Add(dtoRecurso1);
            DtoRol rol = new DtoRol() { zonas = new List<DtoZona>(), recursos = listaRecursos };

            // Loguear.
            var log = logica.loguearUsuario(token, rol);

            // Sin token.
            try
            {
                logica.adjuntarGeoUbicacion(null, new DtoGeoUbicacion() { idExtension = 1, latitud = 12, longitud = 120 });
            }
            catch (TokenInvalidoException)
            {
                Assert.IsTrue(true);
            }

            // Token invalido.
            try
            {
                logica.adjuntarGeoUbicacion("estoesuntokeninvalido", new DtoGeoUbicacion() { idExtension = 1, latitud = 12, longitud = 120 });
            }
            catch (TokenInvalidoException)
            {
                Assert.IsTrue(true);
            }

            // Adjuntar geo ubicacion valida.
            var ok = logica.adjuntarGeoUbicacion(token, new DtoGeoUbicacion() { idExtension = 1, latitud = 12, longitud = 120 });
            Assert.IsTrue(ok);

            db = new EmsysContext();
            int cant2 = db.ExtensionesEvento.FirstOrDefault().GeoUbicaciones.Count();
            Assert.IsTrue(cant2 == cant + 1);

            var geo2 = db.ExtensionesEvento.FirstOrDefault().GeoUbicaciones.FirstOrDefault(g => g.Id == cant2);
            Assert.IsTrue((geo2.Longitud == 120) && (geo2.Latitud == 12));

            logica.cerrarSesion(token);
        }

        /// <summary>
        /// Se prueba agregar un archivo de imagen y agregar la imagen a una extension.
        /// </summary>
        [Test]
        public void AdjuntarImagenTest()
        {
            AppDomain.CurrentDomain.SetData(
            "DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Empty));
            EmsysContext db = new EmsysContext();
            db.Usuarios.FirstOrDefault(u => u.NombreLogin == "A").Token = null;
            db.SaveChanges();

            int cantAdjImagen = db.ExtensionesEvento.FirstOrDefault().Imagenes.Count();
            int cantFiles = db.ApplicationFiles.Count();
            IMetodos logica = new Metodos();

            // Autenticar.
            var result = logica.autenticarUsuario("A", "A", null);
            string token = result.accessToken;

            // Elegir roles.
            List<DtoRecurso> lRecursos = new List<DtoRecurso>();
            DtoRecurso dtoRecurso1 = new DtoRecurso() { id = 1, codigo = "recurso1" };
            lRecursos.Add(dtoRecurso1);
            DtoRol rol = new DtoRol() { zonas = new List<DtoZona>(), recursos = lRecursos };

            // Sin autorizacion.            
            try
            {
                logica.adjuntarImagen(token, new DtoApplicationFile() { fileData = new byte[0], nombre = "algo.jpg", idExtension = 1 });
            }
            catch (UsuarioNoAutorizadoException)
            {
                Assert.IsTrue(true);
            }
            //// Loguear.
            var log = logica.loguearUsuario(token, rol);

            // Sin token.
            try
            {
                logica.adjuntarImagen(null, new DtoApplicationFile() { fileData = new byte[0], nombre = "algo.jpg", idExtension = 1 });
            }
            catch (TokenInvalidoException)
            {
                Assert.IsTrue(true);
            }

            // Token invalido.
            try
            {
                logica.adjuntarImagen("tokenIncorrecto", new DtoApplicationFile() { fileData = new byte[0], nombre = "algo.jpg", idExtension = 1 });
            }
            catch (TokenInvalidoException)
            {
                Assert.IsTrue(true);
            }

            // Adjuntar imagen ubicacion valida.
            var ok = logica.adjuntarImagen(token, new DtoApplicationFile() { fileData = new byte[0], nombre = "algo.jpg", idExtension = 1 });

            var c = db.imagenes.Count();
            var c2 = db.ApplicationFiles.Count();
            var adj = db.imagenes.FirstOrDefault();
            var file = db.ApplicationFiles.Count();

            Assert.IsTrue(ok);
            Assert.IsTrue(db.ExtensionesEvento.FirstOrDefault().Imagenes.Count() == cantAdjImagen + 1);
            Assert.IsTrue(db.ApplicationFiles.Count() == cantFiles + 2);

            // Obtener data de la imagen.
            try
            {
                logica.getImageData(null, 1);
            }
            catch (TokenInvalidoException)
            {
                Assert.IsTrue(true);
            }

            try
            {
                logica.getImageThumbnail(null, 1);
            }
            catch (TokenInvalidoException)
            {
                Assert.IsTrue(true);
            }

            // Token invalido.
            try
            {
                logica.getImageData("tokenIncorrecto", 1);
            }
            catch (TokenInvalidoException)
            {
                Assert.IsTrue(true);
            }

            try
            {
                logica.getImageThumbnail("tokenIncorrecto", 1);
            }
            catch (TokenInvalidoException)
            {
                Assert.IsTrue(true);
            }

            // Adjunto invalido.
            try
            {
                logica.getImageData(token, -1);
            }
            catch (ImagenInvalidaException)
            {
                Assert.IsTrue(true);
            }
            try
            {
                logica.getImageThumbnail(token, -1);
            }
            catch (ImagenInvalidaException)
            {
                Assert.IsTrue(true);
            }

            DtoApplicationFile f = logica.getImageData(token, 1);
            DtoApplicationFile fT = logica.getImageThumbnail(token, 1);
            Assert.IsNotNull(f);
            Assert.IsNotNull(fT);
            Assert.IsTrue(f.nombre == db.imagenes.FirstOrDefault().ImagenData.Id.ToString() + ".jpg");

            // Imagen en evento.
            db.Evento.FirstOrDefault().Imagenes.Add(new Imagen() { Evento = db.Evento.FirstOrDefault(), FechaEnvio = DateTime.Now, ImagenThumbnail = db.ExtensionesEvento.FirstOrDefault().Imagenes.FirstOrDefault().ImagenThumbnail, ImagenData = db.ExtensionesEvento.FirstOrDefault().Imagenes.FirstOrDefault().ImagenData, Usuario = db.Usuarios.FirstOrDefault() });
            db.SaveChanges();
            DtoApplicationFile f2 = logica.getImageData(token, 2);
            Assert.IsNotNull(f2);
            DtoApplicationFile fT2 = logica.getImageThumbnail(token, 2);
            Assert.IsNotNull(fT2);
            Assert.IsTrue(f2.nombre == db.imagenes.FirstOrDefault().ImagenData.Id.ToString() + ".jpg");
        }

        /// <summary>
        /// Se prueba agregar un archivo de audio y agregar el audio a una extension.
        /// </summary>
        [Test]
        public void AdjuntarAudioTest()
        {
            AppDomain.CurrentDomain.SetData(
            "DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));
            EmsysContext db = new EmsysContext();
            db.Usuarios.FirstOrDefault(u => u.NombreLogin == "A").Token = null;
            db.SaveChanges();

            int cantAdjAudio = db.ExtensionesEvento.FirstOrDefault().Audios.Count();
            int cantFiles = db.ApplicationFiles.Count();
            IMetodos logica = new Metodos();

            // Autenticar.
            var result = logica.autenticarUsuario("A", "A", null);
            string token = result.accessToken;

            // Sin autorizacion.
            try
            {
                logica.adjuntarAudio(token, new DtoApplicationFile() { fileData = new byte[0], nombre = "algo.mp3", idExtension = 1 });
            }
            catch (UsuarioNoAutorizadoException)
            {
                Assert.IsTrue(true);
            }

            // Elegir roles.
            List<DtoRecurso> lRecursos = new List<DtoRecurso>();
            DtoRecurso dtoRecurso1 = new DtoRecurso() { id = 1, codigo = "recurso1" };
            lRecursos.Add(dtoRecurso1);
            DtoRol rol = new DtoRol() { zonas = new List<DtoZona>(), recursos = lRecursos };

            // Loguear.
            var log = logica.loguearUsuario(token, rol);

            // Sin token.
            try
            {
                logica.adjuntarAudio(null, new DtoApplicationFile() { fileData = new byte[0], nombre = "algo.mp3", idExtension = 1 });
            }
            catch (TokenInvalidoException)
            {
                Assert.IsTrue(true);
            }

            // Token invalido.
            try
            {
                logica.adjuntarAudio("tokenIncorrecto", new DtoApplicationFile() { fileData = new byte[0], nombre = "algo.mp3", idExtension = 1 });
            }
            catch (TokenInvalidoException)
            {
                Assert.IsTrue(true);
            }

            // Adjuntar geo ubicacion valida.
            var ok = logica.adjuntarAudio(token, new DtoApplicationFile() { fileData = new byte[0], nombre = "algo.mp3", idExtension = 1 });

            var c = db.Audios.Count();
            var c2 = db.ApplicationFiles.Count();
            var adj = db.Audios.FirstOrDefault();
            var file = db.ApplicationFiles.Count();

            Assert.IsTrue(ok);
            Assert.IsTrue(db.ExtensionesEvento.FirstOrDefault().Audios.Count() == cantAdjAudio + 1);
            Assert.IsTrue(db.ApplicationFiles.Count() == cantFiles + 1);

            // Obtener data del audio.
            try
            {
                logica.getAudioData(null, 1);
            }
            catch (TokenInvalidoException)
            {
                Assert.IsTrue(true);
            }

            // Token invalido.
            try
            {
                logica.getAudioData("tokenIncorrecto", 1);
            }
            catch (TokenInvalidoException)
            {
                Assert.IsTrue(true);
            }

            // Adjunto invalido.
            try
            {
                logica.getAudioData(token, -1);
            }
            catch (AudioInvalidoException)
            {
                Assert.IsTrue(true);
            }

            DtoApplicationFile f = logica.getAudioData(token, 1);
            Assert.IsNotNull(f);
            Assert.IsTrue(f.nombre == db.Audios.FirstOrDefault().AudioData.Id.ToString() + ".mp3");

            // Audio en evento.
            db.Evento.FirstOrDefault().Audios.Add(new Audio() { Evento = db.Evento.FirstOrDefault(), FechaEnvio = DateTime.Now, AudioData = db.ExtensionesEvento.FirstOrDefault().Audios.FirstOrDefault().AudioData, Usuario = db.Usuarios.FirstOrDefault() });
            db.SaveChanges();
            DtoApplicationFile f2 = logica.getAudioData(token, 2);
            Assert.IsNotNull(f2);
            Assert.IsTrue(f2.nombre == db.Audios.FirstOrDefault().AudioData.Id.ToString() + ".mp3");

            // logica.cerrarSesion(token);
        }

        /// <summary>
        /// Se prueba agregar un archivo de video y agregar el video a una extension.
        /// </summary>
        [Test]
        public void AdjuntarVideoTest()
        {
            AppDomain.CurrentDomain.SetData(
            "DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Empty));
            EmsysContext db = new EmsysContext();
            db.Usuarios.FirstOrDefault(u => u.NombreLogin == "A").Token = null;
            db.SaveChanges();

            int cantAdjVideo = db.ExtensionesEvento.FirstOrDefault().Videos.Count();
            int cantFiles = db.ApplicationFiles.Count();
            IMetodos logica = new Metodos();

            // Autenticar.
            var result = logica.autenticarUsuario("A", "A", null);
            string token = result.accessToken;

            // No tengo autorizacion.
            try
            {
                logica.adjuntarVideo(token, new DtoApplicationFile() { fileData = new byte[0], nombre = "algo.mp4", idExtension = 1 });
            }
            catch (UsuarioNoAutorizadoException)
            {
                Assert.IsTrue(true);
            }
            //// Elegir roles.
            List<DtoRecurso> listaRecursos = new List<DtoRecurso>();
            DtoRecurso dtoRecurso1 = new DtoRecurso() { id = 1, codigo = "recurso1" };
            listaRecursos.Add(dtoRecurso1);
            DtoRol rol = new DtoRol() { zonas = new List<DtoZona>(), recursos = listaRecursos };

            //// Loguear.
            var log = logica.loguearUsuario(token, rol);

            //// Sin token.
            try
            {
                logica.adjuntarVideo(null, new DtoApplicationFile() { fileData = new byte[0], nombre = "algo.mp4", idExtension = 1 });
            }
            catch (TokenInvalidoException)
            {
                Assert.IsTrue(true);
            }

            //// Token invalido.
            try
            {
                logica.adjuntarVideo("tokenIncorrecto", new DtoApplicationFile() { fileData = new byte[0], nombre = "algo.mp4", idExtension = 1 });
            }
            catch (TokenInvalidoException)
            {
                Assert.IsTrue(true);
            }
            //// Formato invalido.
            try
            {
                logica.adjuntarVideo(token, new DtoApplicationFile() { fileData = new byte[0], nombre = "algo.mp3", idExtension = 1 });
            }
            catch (FormatoInvalidoException)
            {
                Assert.IsTrue(true);
            }

            var ok = logica.adjuntarVideo(token, new DtoApplicationFile() { fileData = new byte[0], nombre = "algo.mp4", idExtension = 1 });

            var c = db.Videos.Count();
            var c2 = db.ApplicationFiles.Count();
            var adj = db.Videos.FirstOrDefault();
            var file = db.ApplicationFiles.Count();

            Assert.IsTrue(ok);
            Assert.IsTrue(db.ExtensionesEvento.FirstOrDefault().Videos.Count() == cantAdjVideo + 1);
            Assert.IsTrue(db.ApplicationFiles.Count() == cantFiles + 1);

            //// Obtener data del video.
            try
            {
                logica.getVideoData(null, 1);
            }
            catch (TokenInvalidoException)
            {
                Assert.IsTrue(true);
            }

            //// Token invalido.
            try
            {
                logica.getVideoData("tokenIncorrecto", 1);
            }
            catch (TokenInvalidoException)
            {
                Assert.IsTrue(true);
            }

            //// Adjunto invalido.
            try
            {
                logica.getVideoData(token, -1);
            }
            catch (VideoInvalidoException)
            {
                Assert.IsTrue(true);
            }

            DtoApplicationFile f = logica.getVideoData(token, 1);
            Assert.IsNotNull(f);
            Assert.IsTrue(f.nombre == db.Videos.FirstOrDefault().VideoData.Id.ToString() + ".mp4");

            //// Video en evento.
            db.Evento.FirstOrDefault().Videos.Add(new Video() { Evento = db.Evento.FirstOrDefault(), FechaEnvio = DateTime.Now, VideoData = db.ExtensionesEvento.FirstOrDefault().Videos.FirstOrDefault().VideoData, Usuario = db.Usuarios.FirstOrDefault() });
            db.SaveChanges();
            DtoApplicationFile f2 = logica.getVideoData(token, 2);
            Assert.IsNotNull(f2);
            Assert.IsTrue(f2.nombre == db.Videos.FirstOrDefault().VideoData.Id.ToString() + ".mp4");
        }


        /// <summary>
        /// Se prueba el metodo keepMeAlive.
        /// </summary>
        [Test]
        public void KeepMeAliveTest()
        {
            AppDomain.CurrentDomain.SetData(
            "DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));
            EmsysContext db = new EmsysContext();
            db.Usuarios.FirstOrDefault(u => u.NombreLogin == "A").Token = null;
            db.SaveChanges();

            IMetodos dbAL = new Metodos();
            var result = dbAL.autenticarUsuario("A", "A", null);

            try
            {
                dbAL.keepMeAlive(null);
            }
            catch (TokenInvalidoException)
            {
                Assert.IsTrue(true);
            }

            try
            {
                dbAL.keepMeAlive("tokenIncorrecto");
            }
            catch (TokenInvalidoException)
            {
                Assert.IsTrue(true);
            }

            bool ok = dbAL.keepMeAlive(result.accessToken);
            db = new EmsysContext();
            var time1 = db.Usuarios.FirstOrDefault(u => u.NombreLogin == "A").UltimoSignal.Value;

            Thread.Sleep(1000);

            ok = dbAL.keepMeAlive(result.accessToken);
            db = new EmsysContext();
            var time2 = db.Usuarios.FirstOrDefault(u => u.NombreLogin == "A").UltimoSignal.Value;

            Assert.IsTrue((time1 != null) && (time2 != null) && (DateTime.Compare(time1, time2) < 0));
        }

        /// <summary>
        /// Se prueba el metodo desconectar inactivos.
        /// </summary>
        [Test]
        public void DesconectarInactivosTest()
        {
            AppDomain.CurrentDomain.SetData(
            "DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));
            EmsysContext db = new EmsysContext();

            db.Usuarios.FirstOrDefault(u => u.NombreLogin == "A").Token = "simuloEstarConectado";
            db.Usuarios.FirstOrDefault(u => u.NombreLogin == "A").UltimoSignal = DateTime.Parse("2015/07/23 21:30:00");
            db.SaveChanges();

            IMetodos dbAL = new Metodos();
            try
            {
                var result = dbAL.autenticarUsuario("A", "A", null);
            }
            catch (SesionActivaException)
            {
                Assert.IsTrue(true);
            }

            dbAL.desconectarAusentes(10, 8);
            var result2 = dbAL.autenticarUsuario("A", "A", null);
            Assert.IsTrue(result2.accessToken != null);

            db = new EmsysContext();

            db.Usuarios.FirstOrDefault(u => u.NombreLogin == "A").Token = "simuloEstarConectado";
            db.Usuarios.FirstOrDefault(u => u.NombreLogin == "A").UltimoSignal = DateTime.Parse("2015/07/23 21:30:00");
            db.SaveChanges();

            Thread workerThread = new Thread(new ThreadStart(Emsys.LogicLayer.Program.Main));
            workerThread.Start();
            Thread.Sleep(1000);
            var result3 = dbAL.autenticarUsuario("A", "A", null);
            Assert.IsTrue(result3.accessToken != null);
        }


        /// <summary>
        /// Probar tiene acceso.
        /// </summary>
        [Test]
        public void TieneAccesoTest()
        {
            AppDomain.CurrentDomain.SetData(
            "DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));
            EmsysContext db = new EmsysContext();
            db.Usuarios.FirstOrDefault(us => us.NombreLogin == "A").Token = null;
            db.SaveChanges();


            int cant = db.ExtensionesEvento.FirstOrDefault().GeoUbicaciones.Count();
            IMetodos logica = new Metodos();

            // Autenticar.
            var result = logica.autenticarUsuario("A", "A", null);
            string token = result.accessToken;

            // Elegir roles.
            List<DtoZona> lZonas = new List<DtoZona>();
            DtoZona zona = new DtoZona() { id = 1 };
            lZonas.Add(zona);
            DtoRol rol = new DtoRol() { zonas = lZonas, recursos = new List<DtoRecurso>() };

            // Loguear.
            var log = logica.loguearUsuario(token, rol);

            Assert.IsTrue(TieneAcceso.tieneVisionEvento(db.Usuarios.FirstOrDefault(u => u.NombreLogin == "A"), db.Evento.FirstOrDefault()));
            Assert.IsTrue(TieneAcceso.tieneVisionExtension(db.Usuarios.FirstOrDefault(u => u.NombreLogin == "A"), db.ExtensionesEvento.FirstOrDefault()));
            Assert.IsFalse(TieneAcceso.tieneVisionExtension(null, db.ExtensionesEvento.FirstOrDefault()));
            Assert.IsFalse(TieneAcceso.tieneVisionEvento(null, db.Evento.FirstOrDefault()));
            Assert.IsFalse(TieneAcceso.estaAsignadoExtension(null, db.ExtensionesEvento.FirstOrDefault()));
            Assert.IsFalse(TieneAcceso.estaDespachandoExtension(null, db.ExtensionesEvento.FirstOrDefault()));

            var user = db.Usuarios.FirstOrDefault(u => u.NombreLogin == "A");
            user.Recurso.Clear();
            user.Despachando.Add(db.ExtensionesEvento.FirstOrDefault());
            db.SaveChanges();
            Assert.IsTrue(TieneAcceso.estaDespachandoExtension(user, db.ExtensionesEvento.FirstOrDefault()));
        }


        /// <summary>
        /// Se pureba el get dto imagen.
        /// </summary>
        [Test]
        public void GetDtoImagenTest()
        {
            AppDomain.CurrentDomain.SetData(
            "DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));
            EmsysContext db = new EmsysContext();

            db.ApplicationFiles.Add(new ApplicationFile() { FileData = new byte[0], Nombre = "algo.jpg" });
            db.SaveChanges();
            db = new EmsysContext();
            Imagen img = new Imagen() { Evento = db.Evento.FirstOrDefault(), ExtensionEvento = db.ExtensionesEvento.FirstOrDefault(), FechaEnvio = DateTime.Now, ImagenData = db.ApplicationFiles.FirstOrDefault(), Usuario = db.Usuarios.FirstOrDefault() };
            db.imagenes.Add(img);
            db.SaveChanges();

            db = new EmsysContext();
            DtoImagen dto = DtoGetters.getDtoImagen(db.imagenes.FirstOrDefault());
            Assert.AreEqual(dto.id, 1);
            Assert.AreNotEqual(dto.idImagen, 0);
            Assert.AreEqual(dto.usuario, db.Usuarios.FirstOrDefault().Nombre);
            Assert.AreNotEqual(dto.fechaEnvio, null);
        }

        /// <summary>
        /// Se pureba el get dto audio.
        /// </summary>
        [Test]
        public void GetDtoAudioTest()
        {
            AppDomain.CurrentDomain.SetData(
            "DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));
            EmsysContext db = new EmsysContext();

            db.ApplicationFiles.Add(new ApplicationFile() { FileData = new byte[0], Nombre = "algo.mp3" });
            db.SaveChanges();
            db = new EmsysContext();
            Audio aud = new Audio() { Evento = db.Evento.FirstOrDefault(), ExtensionEvento = db.ExtensionesEvento.FirstOrDefault(), FechaEnvio = DateTime.Now, AudioData = db.ApplicationFiles.FirstOrDefault(), Usuario = db.Usuarios.FirstOrDefault() };
            db.Audios.Add(aud);
            db.SaveChanges();

            db = new EmsysContext();
            DtoAudio dto = DtoGetters.getDtoAudio(db.Audios.FirstOrDefault());
            Assert.AreEqual(dto.id, 1);
            Assert.AreNotEqual(dto.idAudio, 0);
            Assert.AreEqual(dto.usuario, db.Usuarios.FirstOrDefault().Nombre);
            Assert.AreNotEqual(dto.fechaEnvio, null);
        }


        /// <summary>
        /// Se pureba el get dto video.
        /// </summary>
        [Test]
        public void GetDtoVideoTest()
        {
            AppDomain.CurrentDomain.SetData(
            "DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));
            EmsysContext db = new EmsysContext();

            db.ApplicationFiles.Add(new ApplicationFile() { FileData = new byte[0], Nombre = "algo.mp4" });
            db.SaveChanges();
            db = new EmsysContext();
            Video vid = new Video() { Evento = db.Evento.FirstOrDefault(), ExtensionEvento = db.ExtensionesEvento.FirstOrDefault(), FechaEnvio = DateTime.Now, VideoData = db.ApplicationFiles.FirstOrDefault(), Usuario = db.Usuarios.FirstOrDefault() };
            db.Videos.Add(vid);
            db.SaveChanges();

            db = new EmsysContext();
            DtoVideo dto = DtoGetters.getDtoVideo(db.Videos.FirstOrDefault());
            Assert.AreEqual(dto.id, 1);
            Assert.AreNotEqual(dto.idVideo, 0);
            Assert.AreEqual(dto.usuario, db.Usuarios.FirstOrDefault().Nombre);
            Assert.AreNotEqual(dto.fechaEnvio, null);
        }


        /// <summary>
        /// Se prueba actualizar una descripcion como recurso.
        /// </summary>
        [Test]
        public void ActualizarDescripcionRecursoTest()
        {
            AppDomain.CurrentDomain.SetData(
            "DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));
            EmsysContext db = new EmsysContext();
            db.Usuarios.FirstOrDefault(u => u.NombreLogin == "A").Token = null;
            db.SaveChanges();

            int cantDescripciones = db.ExtensionesEvento.FirstOrDefault().AsignacionesRecursos.FirstOrDefault().AsignacionRecursoDescripcion.Count();
            IMetodos logica = new Metodos();

            // Autenticar.
            var result = logica.autenticarUsuario("A", "A", null);
            string token = result.accessToken;

            // La actualizacion.
            var dto = new DtoActualizarDescripcion() { descripcion = "hola", idExtension = 1 };

            // Elegir roles.
            List<DtoRecurso> lRecursos = new List<DtoRecurso>();
            DtoRecurso dtoRecurso1 = new DtoRecurso() { id = 1, codigo = "recurso1" };
            lRecursos.Add(dtoRecurso1);
            DtoRol rol = new DtoRol() { zonas = new List<DtoZona>(), recursos = lRecursos };

            // Loguear.
            var log = logica.loguearUsuario(token, rol);

            // Sin token.
            try
            {
                logica.ActualizarDescripcionRecurso(dto, null);
            }
            catch (TokenInvalidoException)
            {
                Assert.IsTrue(true);
            }

            // Token invalido.
            try
            {
                logica.ActualizarDescripcionRecurso(dto, "tokenIncorrecto");
            }
            catch (TokenInvalidoException)
            {
                Assert.IsTrue(true);
            }

            // Adjuntar geo ubicacion valida.
            var ok = logica.ActualizarDescripcionRecurso(dto, token);

            db = new EmsysContext();
            Assert.IsTrue(ok);
            Assert.IsTrue(db.ExtensionesEvento.FirstOrDefault().AsignacionesRecursos.FirstOrDefault().AsignacionRecursoDescripcion.Count() == cantDescripciones + 1);
            Assert.AreEqual(db.ExtensionesEvento.FirstOrDefault().AsignacionesRecursos.FirstOrDefault().AsignacionRecursoDescripcion.ToArray()[cantDescripciones].Descripcion, "hola");

            logica.cerrarSesion(token);
        }

        /// <summary>
        /// Se prueba actualizar una descripcion como recurso.
        /// </summary>
        [Test]
        public void ActualizarDescripcionRecursoOfflineTest()
        {
            AppDomain.CurrentDomain.SetData(
            "DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));
            EmsysContext db = new EmsysContext();

            int cantDescripciones = db.ExtensionesEvento.FirstOrDefault().AsignacionesRecursos.FirstOrDefault().AsignacionRecursoDescripcion.Count();
            IMetodos logica = new Metodos();

            DtoRol rol = new DtoRol() { zonas = new List<DtoZona>(), recursos = new List<DtoRecurso>() };

            // Credenciales invalidas.
            try
            {
                logica.ActualizarDescripcionRecursoOffline(new DtoActualizarDescripcionOffline()
                {
                    descripcion = "offline",
                    idExtension = 1,
                    timeStamp = DateTime.Parse(" 2016-11-07T19:54:45.123"),
                    userData = new DtoUsuario() { username = "A", password = "incorrecta", roles = rol }
                });
            }
            catch (CredencialesInvalidasException)
            {
                Assert.IsTrue(true);
            }
            // Extension invalida.
            try
            {
                logica.ActualizarDescripcionRecursoOffline(new DtoActualizarDescripcionOffline()
                {
                    descripcion = "offline",
                    idExtension = -1,
                    timeStamp = DateTime.Parse(" 2016-11-07T19:54:45.123"),
                    userData = new DtoUsuario() { username = "A", password = "A", roles = rol }
                });
            }
            catch (ExtensionInvalidaException)
            {
                Assert.IsTrue(true);
            }
            // Sin recurso.
            try
            {
                logica.ActualizarDescripcionRecursoOffline(new DtoActualizarDescripcionOffline()
                {
                    descripcion = "offline",
                    idExtension = 1,
                    timeStamp = DateTime.Parse(" 2016-11-07T19:54:45.123"),
                    userData = new DtoUsuario() { username = "A", password = "A", roles = rol }
                });
            }
            catch (RecursoInvalidoException)
            {
                Assert.IsTrue(true);
            }

            rol.recursos.Add(new DtoRecurso() { id = 1 });

            // Valido.
            var ok = logica.ActualizarDescripcionRecursoOffline(new DtoActualizarDescripcionOffline()
            {
                descripcion = "offline",
                idExtension = 1,
                timeStamp = DateTime.Parse(" 2016-11-07T19:54:45.123"),
                userData = new DtoUsuario() { username = "A", password = "A", roles = rol }
            });

            db = new EmsysContext();
            Assert.IsTrue(ok);
            Assert.IsTrue(db.ExtensionesEvento.FirstOrDefault().AsignacionesRecursos.FirstOrDefault().AsignacionRecursoDescripcion.Count() == cantDescripciones + 1);
            Assert.IsTrue(db.AsignacionRecursoDescripcion.FirstOrDefault(a => a.Descripcion == "offline").agregadaOffline == true);

        }


        /// <summary>
        /// Se prueba reportar la hora de arribo.
        /// </summary>
        [Test]
        public void ReportarHoraArriboTest()
        {
            AppDomain.CurrentDomain.SetData(
            "DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));
            EmsysContext db = new EmsysContext();
            db.Usuarios.FirstOrDefault(u => u.NombreLogin == "A").Token = null;
            db.SaveChanges();

            int cantDescripciones = db.ExtensionesEvento.FirstOrDefault().AsignacionesRecursos.FirstOrDefault().AsignacionRecursoDescripcion.Count();
            IMetodos logica = new Metodos();

            // Autenticar.
            var result = logica.autenticarUsuario("A", "A", null);
            string token = result.accessToken;

            // Elegir roles.
            List<DtoRecurso> lRecursos = new List<DtoRecurso>();
            DtoRecurso dtoRecurso1 = new DtoRecurso() { id = 1, codigo = "recurso1" };
            lRecursos.Add(dtoRecurso1);
            DtoRol rol = new DtoRol() { zonas = new List<DtoZona>(), recursos = lRecursos };


            // Loguear.
            var log = logica.loguearUsuario(token, rol);

            // Sin token.
            try
            {
                logica.reportarHoraArribo(null, 1);
            }
            catch (TokenInvalidoException)
            {
                Assert.IsTrue(true);
            }

            // Token invalido.
            try
            {
                logica.reportarHoraArribo("tokenIncorrecto", 1);
            }
            catch (TokenInvalidoException)
            {
                Assert.IsTrue(true);
            }

            // Reporto validamente.
            var ok = logica.reportarHoraArribo(token, 1);
            DateTime ahora = DateTime.Now;

            db = new EmsysContext();
            Assert.IsTrue(ok);
            Assert.IsTrue(db.ExtensionesEvento.FirstOrDefault().AsignacionesRecursos.FirstOrDefault().HoraArribo != null);
            Assert.IsTrue(ahora.Subtract(db.ExtensionesEvento.FirstOrDefault().AsignacionesRecursos.FirstOrDefault().HoraArribo.Value).TotalSeconds < 5);

            logica.cerrarSesion(token);
        }


        /// <summary>
        /// Se prueba agregar log de error notificacion.
        /// </summary>
        [Test]
        public void AgregarLogErrorNotificationTest()
        {
            AppDomain.CurrentDomain.SetData(
            "DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));
            EmsysContext db = new EmsysContext();
            int cantLogs = db.LogNotification.Count();
            IMetodos logic = new Metodos();
            db.Usuarios.FirstOrDefault().Token = "hola";
            db.SaveChanges();
            LogsManager.AgregarLogErrorNotification("hola", "hola", "hola", "hola", 1, "hola", "hola", 1, "hola", "hola", "hola", "hola");
            db = new EmsysContext();
            int cant2 = db.LogNotification.Count();
            Assert.IsTrue(cant2 >= cantLogs + 1);
            db.Usuarios.FirstOrDefault().Token = null;
        }


        /// <summary>
        /// Se prueba crear un evento.
        /// </summary>
        [Test]
        public void CrearEventoTest()
        {
            AppDomain.CurrentDomain.SetData(
            "DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));
            EmsysContext db = new EmsysContext();
            db.Usuarios.FirstOrDefault(u => u.NombreLogin == "A").Token = null;
            db.SaveChanges();

            int cantEventos = db.Evento.Count();
            int cantExtensiones = db.ExtensionesEvento.Count();
            IMetodos logica = new Metodos();

            // Autenticar.
            var result = logica.autenticarUsuario("A", "A", null);
            string token = result.accessToken;


            DtoInfoCreacionEvento info = logica.getInfoCreacionEvento(token);
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
                idZonas = idZonas
            };

            // Sin token.
            try
            {
                logica.crearEvento(null, ev);
            }
            catch (TokenInvalidoException)
            {
                Assert.IsTrue(true);
            }

            // Token invalido.
            try
            {
                logica.crearEvento("tokenIncorrecto", ev);
            }
            catch (TokenInvalidoException)
            {
                Assert.IsTrue(true);
            }
            // Sin evento.
            try
            {
                logica.crearEvento(token, null);
            }
            catch (ArgumentoInvalidoException)
            {
                Assert.IsTrue(true);
            }
            // Sin zonas.
            try
            {
                ev.idZonas = new List<int>();
                logica.crearEvento(token, ev);
            }
            catch (SeleccionZonasInvalidaException)
            {
                Assert.IsTrue(true);
            }

            ev.idZonas = idZonas;

            // Crea evento valido.
            var ok = logica.crearEvento(token, ev);

            db = new EmsysContext();
            Assert.IsTrue(ok);
            Assert.IsTrue(db.Evento.Count() == cantEventos + 1);
            Assert.IsTrue(db.ExtensionesEvento.Count() == cantExtensiones + 1);

            logica.cerrarSesion(token);
        }


        /// <summary>
        /// Se prueba tomar y liberar una extension.
        /// </summary>
        [Test]
        public void TomatLiberarExtensionTest()
        {
            AppDomain.CurrentDomain.SetData(
            "DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));
            EmsysContext db = new EmsysContext();
            IMetodos logic = new Metodos();
            db.Usuarios.FirstOrDefault(u => u.NombreLogin == "A").Token = null;
            db.SaveChanges();

            // Autenticar.
            var result = logic.autenticarUsuario("A", "A", null);
            string token = result.accessToken;

            // Elegir roles.
            List<DtoZona> lZonas = new List<DtoZona>();
            DtoZona dtoZona1 = new DtoZona() { id = 1 };
            lZonas.Add(dtoZona1);
            DtoRol rol = new DtoRol() { zonas = lZonas, recursos = new List<DtoRecurso>() };


            // Loguear.
            var log = logic.loguearUsuario(token, rol);

            // Sin token.
            try
            {
                logic.tomarExtension(null, 1);
            }
            catch (TokenInvalidoException)
            {
                Assert.IsTrue(true);
            }

            // Token invalido.
            try
            {
                logic.tomarExtension("tokenIncorrecto", 1);
            }
            catch (TokenInvalidoException)
            {
                Assert.IsTrue(true);
            }

            //// Extension invalida.
            try
            {
                logic.tomarExtension(token, -1);
            }
            catch (ExtensionInvalidaException)
            {
                Assert.IsTrue(true);
            }

            var ok = logic.tomarExtension(token, 1);
            Assert.IsTrue(ok);
            db = new EmsysContext();
            Assert.IsTrue(db.ExtensionesEvento.FirstOrDefault().Estado == EstadoExtension.Despachado);
            Assert.IsTrue(db.Usuarios.FirstOrDefault(u => u.NombreLogin == "A").Despachando.Count() == 1);

            //// Sin token.
            try
            {
                logic.liberarExtension(null, 1);
            }
            catch (TokenInvalidoException)
            {
                Assert.IsTrue(true);
            }

            // Token invalido.
            try
            {
                logic.liberarExtension("tokenIncorrecto", 1);
            }
            catch (TokenInvalidoException)
            {
                Assert.IsTrue(true);
            }

            // Extension invalida.
            try
            {
                logic.liberarExtension(token, -1);
            }
            catch (ExtensionInvalidaException)
            {
                Assert.IsTrue(true);
            }

            var ok2 = logic.liberarExtension(token, 1);
            Assert.IsTrue(ok2);
            db = new EmsysContext();
            Assert.IsTrue(db.ExtensionesEvento.FirstOrDefault().Estado == EstadoExtension.FaltaDespachar);
            Assert.IsTrue(db.Usuarios.FirstOrDefault(u => u.NombreLogin == "A").Despachando.Count() == 0);
            logic.cerrarSesion(token);
        }

        /// <summary>
        /// Se prueba gestionar los recursos de una extension.
        /// </summary>
        [Test]
        public void getRecursosExtensionGestionarRecursosTest()
        {
            AppDomain.CurrentDomain.SetData(
            "DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));
            EmsysContext db = new EmsysContext();
            IMetodos logic = new Metodos();
            db.Usuarios.FirstOrDefault(u => u.NombreLogin == "A").Token = null;
            db.SaveChanges();

            // Autenticar.
            var result = logic.autenticarUsuario("A", "A", null);
            string token = result.accessToken;

            // Elegir roles.
            List<DtoZona> lZonas = new List<DtoZona>();
            DtoZona dtoZona1 = new DtoZona() { id = 1 };
            lZonas.Add(dtoZona1);
            DtoRol rol = new DtoRol() { zonas = lZonas, recursos = new List<DtoRecurso>() };

            // Loguear.
            var log = logic.loguearUsuario(token, rol);

            var ok = logic.tomarExtension(token, 1);

            // Sin token.
            try
            {
                logic.getRecursosExtension(null, 1);
            }
            catch (TokenInvalidoException)
            {
                Assert.IsTrue(true);
            }

            // Token invalido.
            try
            {
                logic.getRecursosExtension("tokenIncorrecto", 1);
            }
            catch (TokenInvalidoException)
            {
                Assert.IsTrue(true);
            }

            // Extension invalida.
            try
            {
                logic.getRecursosExtension(token, -1);
            }
            catch (ExtensionInvalidaException)
            {
                Assert.IsTrue(true);
            }

            Assert.AreEqual(db.ExtensionesEvento.FirstOrDefault().AsignacionesRecursos.Count(), 1);
            var result2 = logic.getRecursosExtension(token, 1);
            Assert.IsNotNull(result2);
            Assert.AreEqual(result2.idExtension, 1);
            Assert.IsTrue(result2.recursosAsignados.Count() == 1);
            Assert.AreEqual(result2.recursosAsignados.FirstOrDefault().id, 1);

            var r1 = result2.recursosAsignados.FirstOrDefault();
            var r2 = result2.recursosNoAsignados.FirstOrDefault();

            // Asigna r2 a la extension.
            result2.recursosAsignados.Remove(r1);
            result2.recursosAsignados.Add(r2);

            // Quita r1 de la extension.
            result2.recursosNoAsignados.Clear();
            result2.recursosNoAsignados.Add(r1);

            // Sin token.
            try
            {
                logic.gestionarRecursos(null, result2);
            }
            catch (TokenInvalidoException)
            {
                Assert.IsTrue(true);
            }

            // Token invalido.
            try
            {
                logic.gestionarRecursos("tokenIncorrecto", result2);
            }
            catch (TokenInvalidoException)
            {
                Assert.IsTrue(true);
            }

            // Extension invalida.
            try
            {
                logic.gestionarRecursos(token, null);
            }
            catch (ArgumentoInvalidoException)
            {
                Assert.IsTrue(true);
            }

            // Extension invalida.
            try
            {
                logic.gestionarRecursos(token, new DtoRecursosExtension() { idExtension = -1, recursosAsignados = new List<DtoRecurso>(), recursosNoAsignados = new List<DtoRecurso>() });
            }
            catch (ExtensionInvalidaException)
            {
                Assert.IsTrue(true);
            }

            var result3 = logic.gestionarRecursos(token, result2);
            Assert.IsTrue(result3);

            var result4 = logic.getRecursosExtension(token, 1);
            Assert.IsNotNull(result4);
            Assert.AreEqual(result4.idExtension, 1);
            Assert.IsTrue(result4.recursosAsignados.Count() == 1);
            Assert.AreEqual(result4.recursosAsignados.FirstOrDefault().id, 2);
            db = new EmsysContext();
            Assert.AreEqual(db.ExtensionesEvento.FirstOrDefault().AsignacionesRecursos.Count(), 2);

            // Vuelvo al estado inicial.
            List<DtoRecurso> l1 = new List<DtoRecurso>();
            l1.Add(r1);
            List<DtoRecurso> l2 = new List<DtoRecurso>();
            l2.Add(r2);
            var fine = logic.gestionarRecursos(token, new DtoRecursosExtension() { idExtension = 1, recursosAsignados = l1, recursosNoAsignados = l2 });
            db = new EmsysContext();
            Assert.AreEqual(db.ExtensionesEvento.FirstOrDefault().AsignacionesRecursos.Count(), 2);

            var ok2 = logic.liberarExtension(token, 1);
            logic.cerrarSesion(token);
        }

        /// <summary>
        /// Se prueba actualizar la segudna categoria de una extension.
        /// </summary>
        [Test]
        public void actualizarSegundaCategoriaTest()
        {
            AppDomain.CurrentDomain.SetData(
            "DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));
            EmsysContext db = new EmsysContext();
            IMetodos logic = new Metodos();
            db.Usuarios.FirstOrDefault(u => u.NombreLogin == "A").Token = null;
            db.SaveChanges();

            // Autenticar.
            var result = logic.autenticarUsuario("A", "A", null);
            string token = result.accessToken;

            // Elegir roles.
            List<DtoZona> lZonas = new List<DtoZona>();
            DtoZona dtoZona1 = new DtoZona() { id = 1 };
            lZonas.Add(dtoZona1);
            DtoRol rol = new DtoRol() { zonas = lZonas, recursos = new List<DtoRecurso>() };


            // Loguear.
            var log = logic.loguearUsuario(token, rol);

            var ok = logic.tomarExtension(token, 1);

            var ok2 = logic.getCategorias();
            Assert.IsTrue(ok2 != null);

            // Sin token.
            try
            {
                logic.actualizarSegundaCategoria(null, 1, 1);
            }
            catch (TokenInvalidoException)
            {
                Assert.IsTrue(true);
            }
            // Token invalido.
            try
            {
                logic.actualizarSegundaCategoria("tokenIncorrecto", 1, 1);
            }
            catch (TokenInvalidoException)
            {
                Assert.IsTrue(true);
            }
            // Extension invalida.
            try
            {
                logic.actualizarSegundaCategoria(token, -1, 1);
            }
            catch (ExtensionInvalidaException)
            {
                Assert.IsTrue(true);
            }
            // Categoria invalida.
            try
            {
                logic.actualizarSegundaCategoria(token, 1, 0);
            }
            catch (CategoriaInvalidaException)
            {
                Assert.IsTrue(true);
            }

            Categoria cat1 = db.ExtensionesEvento.FirstOrDefault().SegundaCategoria;
            Assert.IsTrue(cat1 == null);

            var result2 = logic.actualizarSegundaCategoria(token, 1, 1);
            Assert.IsTrue(result2);

            db = new EmsysContext();
            Categoria cat2 = db.ExtensionesEvento.FirstOrDefault().SegundaCategoria;
            Assert.IsNotNull(cat2);
            Assert.AreEqual(cat2.Id, db.Categorias.FirstOrDefault(c => c.Id == 1).Id);

            var result3 = logic.actualizarSegundaCategoria(token, 1, -1);
            Assert.IsTrue(result3);

            db = new EmsysContext();
            ExtensionEvento e1 = db.ExtensionesEvento.FirstOrDefault(ex => ex.Id == 1);
            Categoria cat3 = e1.SegundaCategoria;
            Assert.IsTrue(cat3 == null);

            var ok3 = logic.liberarExtension(token, 1);
            logic.cerrarSesion(token);
        }


        /// <summary>
        /// Se prueba abrir una extension.
        /// </summary>
        [Test]
        public void AbrirCerrarExtensionTest()
        {
            AppDomain.CurrentDomain.SetData(
            "DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));
            EmsysContext db = new EmsysContext();
            IMetodos logic = new Metodos();
            db.Usuarios.FirstOrDefault(u => u.NombreLogin == "A").Token = null;
            db.SaveChanges();

            // Autenticar.
            var result = logic.autenticarUsuario("A", "A", null);
            string token = result.accessToken;

            // Elegir roles.
            List<DtoZona> lZonas = new List<DtoZona>();
            DtoZona dtoZona1 = new DtoZona() { id = 1 };
            lZonas.Add(dtoZona1);
            DtoRol rol = new DtoRol() { zonas = lZonas, recursos = new List<DtoRecurso>() };


            // Loguear.
            var log = logic.loguearUsuario(token, rol);

            var ok = logic.tomarExtension(token, 1);

            // Sin token.
            try
            {
                logic.getZonasLibresEvento(null, 1);
            }
            catch (TokenInvalidoException)
            {
                Assert.IsTrue(true);
            }
            // Token invalido.
            try
            {
                logic.getZonasLibresEvento("tokenIncorrecto", 1);
            }
            catch (TokenInvalidoException)
            {
                Assert.IsTrue(true);
            }
            // Extension invalida.
            try
            {
                logic.getZonasLibresEvento(token, -1);
            }
            catch (ExtensionInvalidaException)
            {
                Assert.IsTrue(true);
            }

            int cantPrevia = db.Evento.FirstOrDefault(e => e.Id == 1).ExtensionesEvento.Count();
            ICollection<DtoZona> zonas = logic.getZonasLibresEvento(token, 1);
            Assert.IsTrue(zonas.Count() > 0);

            // Sin token.
            try
            {
                logic.abrirExtension(null, 1, zonas.FirstOrDefault().id);
            }
            catch (TokenInvalidoException)
            {
                Assert.IsTrue(true);
            }
            // Token invalido.
            try
            {
                logic.abrirExtension("tokenIncorrecto", 1, zonas.FirstOrDefault().id);
            }
            catch (TokenInvalidoException)
            {
                Assert.IsTrue(true);
            }
            // Extension invalida.
            try
            {
                logic.abrirExtension(token, -1, zonas.FirstOrDefault().id);
            }
            catch (ExtensionInvalidaException)
            {
                Assert.IsTrue(true);
            }
            // Zona invalida.
            try
            {
                logic.abrirExtension(token, 1, -1);
            }
            catch (ZonaInvalidaException)
            {
                Assert.IsTrue(true);
            }

            bool ok2 = logic.abrirExtension(token, 1, zonas.FirstOrDefault().id);
            Assert.IsTrue(ok2);
            db = new EmsysContext();
            Assert.AreEqual(db.Evento.FirstOrDefault().ExtensionesEvento.Count(), cantPrevia + 1);
            Assert.IsTrue(db.Evento.FirstOrDefault().ExtensionesEvento.ToArray()[cantPrevia].Zona.Id == zonas.FirstOrDefault().id);

            int idExtNueva = db.ExtensionesEvento.Max(e => e.Id);
            db.ExtensionesEvento.FirstOrDefault(e => e.Id == idExtNueva).AsignacionesRecursos.Add(new AsignacionRecurso() { Extension = db.ExtensionesEvento.FirstOrDefault(e => e.Id == idExtNueva), Recurso = db.Recursos.FirstOrDefault(), ActualmenteAsignado = true });
            db.SaveChanges();
            var ok5 = logic.tomarExtension(token, idExtNueva);

            // Sin token.
            try
            {
                logic.cerrarExtension(null, idExtNueva);
            }
            catch (TokenInvalidoException)
            {
                Assert.IsTrue(true);
            }
            // Token invalido.
            try
            {
                logic.cerrarExtension("tokenIncorrecto", idExtNueva);
            }
            catch (TokenInvalidoException)
            {
                Assert.IsTrue(true);
            }
            // Extension invalida.
            try
            {
                logic.cerrarExtension(token, -1);
            }
            catch (ExtensionInvalidaException)
            {
                Assert.IsTrue(true);
            }


            var ok3 = logic.cerrarExtension(token, idExtNueva);
            Assert.IsTrue(ok);
            db = new EmsysContext();
            Assert.IsTrue(db.ExtensionesEvento.FirstOrDefault(e => e.Id == idExtNueva).Estado == EstadoExtension.Cerrado);

            var ok4 = logic.liberarExtension(token, 1);
            logic.cerrarSesion(token);
        }



        /// <summary>
        /// Se prueba actualizar una descripcion de extension como despachador.
        /// </summary>
        [Test]
        public void ActualizarDescripcionDespachadorTest()
        {
            AppDomain.CurrentDomain.SetData(
            "DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));
            EmsysContext db = new EmsysContext();
            IMetodos logic = new Metodos();
            db.Usuarios.FirstOrDefault(u => u.NombreLogin == "A").Token = null;
            db.SaveChanges();

            // Autenticar.
            var result = logic.autenticarUsuario("A", "A", null);
            string token = result.accessToken;

            // Elegir roles.
            List<DtoZona> lZonas = new List<DtoZona>();
            DtoZona dtoZona1 = new DtoZona() { id = 1 };
            lZonas.Add(dtoZona1);
            DtoRol rol = new DtoRol() { zonas = lZonas, recursos = new List<DtoRecurso>() };
            

            // Loguear.
            var log = logic.loguearUsuario(token, rol);

            var ok = logic.tomarExtension(token, 1);

            var previo = logic.verInfoEvento(token, 1);
            int cantPrevia = previo.extensiones.FirstOrDefault().descripcionDespachadores.Count();
            DtoActualizarDescripcion descr = new DtoActualizarDescripcion() { idExtension = 1, descripcion = "pruebaDescrDesp" };

            // Sin token.
            try
            {
                logic.actualizarDescripcionDespachador(null, descr);
            }
            catch (TokenInvalidoException)
            {
                Assert.IsTrue(true);
            }
            //// Token invalido.
            try
            {
                logic.actualizarDescripcionDespachador("tokenIncorrecto", descr);
            }
            catch (TokenInvalidoException)
            {
                Assert.IsTrue(true);
            }
            //// Extension invalida.
            try
            {
                logic.actualizarDescripcionDespachador(token, new DtoActualizarDescripcion() { idExtension = -1, descripcion = "cosas" });
            }
            catch (ExtensionInvalidaException)
            {
                Assert.IsTrue(true);
            }

            var ok2 = logic.actualizarDescripcionDespachador(token, descr);
            Assert.IsTrue(ok2);
            var post = logic.verInfoEvento(token, 1);
            var cantPost = post.extensiones.FirstOrDefault().descripcionDespachadores.Count();
            Assert.AreEqual(cantPost, cantPrevia + 1);
            Assert.AreEqual(post.extensiones.FirstOrDefault().descripcionDespachadores.ToArray()[cantPrevia].descripcion, "pruebaDescrDesp");

            var ok4 = logic.liberarExtension(token, 1);
            logic.cerrarSesion(token);
        }


        /// <summary>
        /// Se pureba el get dto video.
        /// </summary>
        [Test]
        public void ExceptionsTest()
        {
            try
            {
                throw new UsuarioNoAutorizadoException();
            }
            catch (UsuarioNoAutorizadoException)
            {
            }

            try
            {
                throw new ExtensionInvalidaException();
            }
            catch (ExtensionInvalidaException)
            {
            }
        }

    }
}
