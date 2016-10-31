using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Emsys.DataAccesLayer.Core;
using Emsys.DataAccesLayer.Model;
using System.IO;

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
                db.Users.Add(user);
                db.SaveChanges();
                var usuarioAgregado = db.Users.FirstOrDefault(x => x.NombreLogin.Equals("prueba"));
                Assert.IsNotNull(usuarioAgregado);
                db.Users.Remove(user);
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
                context.Unidades_Ejecutoras.Add(new Unidad_Ejecutora() { Nombre = "pruebaUE" });
                context.SaveChanges();
                var unidadEjecutoraAgregada = context.Unidades_Ejecutoras.FirstOrDefault(x => x.Nombre.Equals("pruebaUE"));
                Assert.IsNotNull(unidadEjecutoraAgregada);
                context.Unidades_Ejecutoras.Remove(unidadEjecutoraAgregada);
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
                context.Unidades_Ejecutoras.Add(new Unidad_Ejecutora() { Nombre = "pruebaUEZona" });
                context.SaveChanges();
                var unidadEjecutoraAgregada = context.Unidades_Ejecutoras.FirstOrDefault(x => x.Nombre.Equals("pruebaUEZona"));
                if (unidadEjecutoraAgregada != null)
                {
                    context.Zonas.Add(new Zona() { Nombre = "pruebaZona", UnidadEjecutora = unidadEjecutoraAgregada });
                    context.SaveChanges();
                    var zonaAgregada = context.Zonas.FirstOrDefault(x => x.Nombre.Equals("pruebaZona"));
                    Assert.IsNotNull(zonaAgregada);
                    context.Zonas.Remove(zonaAgregada);
                    context.Unidades_Ejecutoras.Remove(unidadEjecutoraAgregada);
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
                context.Unidades_Ejecutoras.Add(new Unidad_Ejecutora() { Nombre = "pruebaUEZonaSector" });
                context.SaveChanges();
                var unidadEjecutoraAgregada = context.Unidades_Ejecutoras.FirstOrDefault(x => x.Nombre.Equals("pruebaUEZonaSector"));
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
                        context.Unidades_Ejecutoras.Remove(unidadEjecutoraAgregada);
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
                GeoUbicaciones = new List<GeoUbicacion>(),
                Imagenes = new List<Imagen>(),
                Latitud = 0,
                Longitud = 0,
                //Origen_Evento = new Origen_Evento(),
                Videos = new List<Video>(),
                //Departamento = new Departamento(),
                Descripcion = "PruebaDE"
            };

            var ext1 = new Extension_Evento()
            {
                Evento = evento,
                Zona = context.Zonas.FirstOrDefault(),
                Estado = EstadoExtension.Despachado,
                TimeStamp = DateTime.Now,
                Despachador = context.Users.FirstOrDefault(),
                DescripcionDespachador = "2016/07/23 21:30:00\\UsuarioDespachador\\descripcion de evento\\2016/07/23 21:37:00\\UsuarioDespachador2\\otra descripcion de evento\\2016/07/24 10:37:00\\UsuarioDespachador2\\otra mas"
            };

            Origen_Evento oe1 = new Origen_Evento()
            {
                TimeStamp = DateTime.Now,
                TipoOrigen = "test",
                IdOrigen = 1,
                Evento = evento
            };
            context.Origen_Eventos.Add(oe1);
            context.SaveChanges();

            context = new EmsysContext();
            var oe = context.Origen_Eventos.FirstOrDefault();
            Assert.AreEqual(oe.Id, 4);
            Assert.AreNotEqual(oe.TimeStamp, null);
            Assert.AreEqual(oe.TipoOrigen, "test");
            Assert.AreEqual(oe.IdOrigen, 1);
            Assert.AreEqual(oe.Evento.Id, 4);
        }

    }
}
