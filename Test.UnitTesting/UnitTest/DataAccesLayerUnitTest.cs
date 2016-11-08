using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Emsys.DataAccesLayer.Core;
using Emsys.DataAccesLayer.Model;
using NUnit.Framework;

namespace Test.UnitTesting
{
    [TestFixture]
    class DataAccesLayerUnitTest
    {
        /// <summary>
        /// Test que prueba que se pueda registrar un usuario a Identity.
        /// </summary>
        [Test]
        public void RegistrarUsuarioTest()
        {
            using (var db = new EmsysContext())
            {
                var user = new Usuario() { NombreLogin = "prueba", Nombre = "prueba" };
                db.Usuarios.Add(user);
                db.SaveChanges();
                var usuarioAgregado = db.Usuarios.FirstOrDefault(x => x.NombreLogin.Equals("prueba"));
                Assert.IsNotNull(usuarioAgregado);
                db.Usuarios.Remove(user);
            }
        }

        /// <summary>
        /// Test que prueba que se pueda crear una Unidad Ejecutora y agregarla a la base de datos.
        /// </summary>
        [Test]
        public void CrearUnidadEjecutora()
        {
            using (var context = new EmsysContext())
            {
                context.UnidadesEjecutoras.Add(new UnidadEjecutora() { Nombre = "pruebaUE" });
                context.SaveChanges();
                var unidadEjecutoraAgregada = context.UnidadesEjecutoras.FirstOrDefault(x => x.Nombre.Equals("pruebaUE"));
                Assert.IsNotNull(unidadEjecutoraAgregada);
                context.UnidadesEjecutoras.Remove(unidadEjecutoraAgregada);
            }
        }

        /// <summary>
        /// Test que prueba que se pueda crear una Zona y agregarla a la base de datos.
        /// </summary>
        [Test]
        public void CrearZona()
        {
            using (var context = new EmsysContext())
            {
                context.UnidadesEjecutoras.Add(new UnidadEjecutora() { Nombre = "pruebaUEZona" });
                context.SaveChanges();
                var unidadEjecutoraAgregada = context.UnidadesEjecutoras.FirstOrDefault(x => x.Nombre.Equals("pruebaUEZona"));
                if (unidadEjecutoraAgregada != null)
                {
                    context.Zonas.Add(new Zona() { Nombre = "pruebaZona", UnidadEjecutora = unidadEjecutoraAgregada });
                    context.SaveChanges();
                    var zonaAgregada = context.Zonas.FirstOrDefault(x => x.Nombre.Equals("pruebaZona"));
                    Assert.IsNotNull(zonaAgregada);
                    context.Zonas.Remove(zonaAgregada);
                    context.UnidadesEjecutoras.Remove(unidadEjecutoraAgregada);
                }
                else
                {
                    Assert.Fail();
                }
            }
        }

        /// <summary>
        /// Test que prueba que se pueda crear un Sector y agregarlo a la base de datos.
        /// </summary>
        [Test]
        public void CrearSector()
        {
            using (var context = new EmsysContext())
            {
                context.UnidadesEjecutoras.Add(new UnidadEjecutora() { Nombre = "pruebaUEZonaSector" });
                context.SaveChanges();
                var unidadEjecutoraAgregada = context.UnidadesEjecutoras.FirstOrDefault(x => x.Nombre.Equals("pruebaUEZonaSector"));
                if (unidadEjecutoraAgregada != null)
                {
                    context.Zonas.Add(new Zona() { Nombre = "pruebaZonaSector", UnidadEjecutora = unidadEjecutoraAgregada });
                    context.SaveChanges();
                    var zonaAgregada = context.Zonas.FirstOrDefault(x => x.Nombre.Equals("pruebaZonaSector"));
                    if (zonaAgregada != null)
                    {
                        context.Sectores.Add(new Sector() { Nombre = "prueba", Zona = zonaAgregada });
                        context.SaveChanges();
                        var sectorAgregado = context.Sectores.FirstOrDefault(x => x.Nombre.Equals("prueba"));
                        Assert.IsNotNull(sectorAgregado);
                        context.Sectores.Remove(sectorAgregado);
                        context.Zonas.Remove(zonaAgregada);
                        context.UnidadesEjecutoras.Remove(unidadEjecutoraAgregada);
                    }
                    else
                    {
                        Assert.Fail();
                    }
                }
                else
                {
                    Assert.Fail();
                }
            }
        }

        /// <summary>
        /// Test que prueba que se pueda crear un Sector y agregarlo a la base de datos.
        /// </summary>
        [Test]
        public void CrearOrigenEventoTest()
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));
            
            var context = new EmsysContext();

            // Evento y extensiones.
            var sector = new Sector() { Nombre = "sectorDEPrueba", Zona = context.Zonas.FirstOrDefault() };
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
                Zona = context.Zonas.FirstOrDefault(),
                Estado = EstadoExtension.Despachado,
                TimeStamp = DateTime.Now,
                Despachador = context.Usuarios.FirstOrDefault(),
                DescripcionDespachador = "2016/07/23 21:30:00\\UsuarioDespachador\\descripcion de evento\\2016/07/23 21:37:00\\UsuarioDespachador2\\otra descripcion de evento\\2016/07/24 10:37:00\\UsuarioDespachador2\\otra mas"
            };

            OrigenEvento oe1 = new OrigenEvento()
            {
                TimeStamp = DateTime.Now,
                TipoOrigen = "testCOE",
                IdOrigen = 1,
                Evento = evento
            };
            evento.OrigenEvento = oe1;
            context.Evento.Add(evento);
            context.SaveChanges();

            context = new EmsysContext();
            var oe = context.OrigenEventos.FirstOrDefault(o=> o.TipoOrigen == "testCOE");
            Assert.AreNotEqual(oe.TimeStamp, null);
            Assert.AreEqual(oe.IdOrigen, 1);
            Assert.AreEqual(oe.Evento.NombreInformante, "PruebaDE");
        }
    }
}
