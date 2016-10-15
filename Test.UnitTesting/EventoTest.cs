using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emsys.DataAccesLayer.Core;
using NUnit.Framework;
using Emsys.LogicLayer;
using Emsys.LogicLayer.Utils;
using Emsys.LogicLayer.ApplicationExceptions;
using System.IO;
using Emsys.DataAccesLayer.Model;
using DataTypeObject;
using System.Data.SqlClient;
using System.Data.Entity.Validation;

namespace Test.UnitTesting
{
    [TestFixture]
    class EventoTest
    {

        /// <summary>
        /// Se prueba que al realizar listar eventos se traigan todos los eventos 
        /// asociados al recurso con el que se logueo el usuario.
        /// </summary>
        [Test]
        public void listarEventosTest()
        {
            using (var context = new EmsysContext())
            {
                AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));

                // Se crea un usuario con un recurso asociado en la BD.
                var user = new Usuario() { NombreLogin = "usuarioListarEventoRecurso", Nombre = "usuarioListarEventoRecurso", Contraseña = Passwords.GetSHA1("usuarioListarEventoRecurso"), Grupos_Recursos = new List<Grupo_Recurso>(), Unidades_Ejecutoras = new List<Unidad_Ejecutora>() };
                var recursoDisponible = new Recurso() { Codigo = "recursoListarEvento", Estado = EstadoRecurso.Disponible, EstadoAsignacion = EstadoAsignacionRecurso.Libre, Extensiones_Eventos = new List<Extension_Evento>() };
                var gr = new Grupo_Recurso() { Nombre = "grPrueba", Recursos = new List<Recurso>() };
                var zona1 = new Zona() { Nombre = "zona1" };
                var zona2 = new Zona() { Nombre = "zona2" };
                var zona3 = new Zona() { Nombre = "zona3" };
                var zona4 = new Zona() { Nombre = "zona4" };
                var unidadEjecutora1 = new Unidad_Ejecutora() { Nombre = "uePrueba", Zonas = new List<Zona>() };
                var unidadEjecutora2 = new Unidad_Ejecutora() { Nombre = "uePrueba2", Zonas = new List<Zona>() };
                var unidadEjecutora3 = new Unidad_Ejecutora() { Nombre = "uePrueba3", Zonas = new List<Zona>() };
                unidadEjecutora1.Zonas.Add(zona1);
                unidadEjecutora1.Zonas.Add(zona2);
                unidadEjecutora2.Zonas.Add(zona3);
                unidadEjecutora3.Zonas.Add(zona4);
                zona1.UnidadEjecutora = unidadEjecutora1;
                zona2.UnidadEjecutora = unidadEjecutora1;
                zona3.UnidadEjecutora = unidadEjecutora2;
                zona4.UnidadEjecutora = unidadEjecutora3;
                user.Unidades_Ejecutoras.Add(unidadEjecutora1);
                user.Unidades_Ejecutoras.Add(unidadEjecutora2);
                gr.Recursos.Add(recursoDisponible);
                user.Grupos_Recursos.Add(gr);
                context.Zonas.Add(zona1);
                context.Zonas.Add(zona2);
                context.Zonas.Add(zona3);
                context.Zonas.Add(zona4);
                context.Unidades_Ejecutoras.Add(unidadEjecutora1);
                context.Unidades_Ejecutoras.Add(unidadEjecutora2);
                context.Unidades_Ejecutoras.Add(unidadEjecutora3);
                context.Recursos.Add(recursoDisponible);
                context.Grupos_Recursos.Add(gr);
                context.Users.Add(user);
                context.SaveChanges();

                // Evento y extensiones
                var sector = new Sector() { Nombre = "sectorPruebaLE", Zona = zona1 };
                var catEvento = new Categoria() { Clave = "catPruebaListarEvento", Activo = true, Codigo = "catPrueba", Prioridad = NombrePrioridad.Media };
                var evento = new Evento()
                {
                    Estado = EstadoEvento.Enviado,
                    Categoria = catEvento,
                    TimeStamp = DateTime.Now,
                    FechaCreacion = DateTime.Now,
                    Sector = sector,
                    EnProceso = true
                };
                var ext1 = new Extension_Evento()
                {
                    Evento = evento,
                    Zona = zona1,
                    Estado = EstadoExtension.Despachado,
                    TimeStamp = DateTime.Now,
                    Despachador = user
                };
                var ext2 = new Extension_Evento()
                {
                    Evento = evento,
                    Zona = zona2,
                    Estado = EstadoExtension.Despachado,
                    TimeStamp = DateTime.Now,
                    Despachador = user
                };
                var ext3 = new Extension_Evento()
                {
                    Evento = evento,
                    Zona = zona3,
                    Estado = EstadoExtension.Despachado,
                    TimeStamp = DateTime.Now,
                    Despachador = user
                };
                IMetodos logica = new Metodos();
                
                // Obtengo token de usuario. 
                var autent = logica.autenticarUsuario("usuarioListarEventoRecurso", "usuarioListarEventoRecurso");
                string token = autent.access_token;

                // Se prueba que se listen las extensiones asociadas a un recurso
                recursoDisponible.Extensiones_Eventos.Add(ext1);
                recursoDisponible.Extensiones_Eventos.Add(ext2);
                try
                {
                    context.SaveChanges();
                } catch (DbEntityValidationException e)
                {
                    throw e;
                }
                
                List<DtoRecurso> lRecurso = new List<DtoRecurso>();
                DtoRecurso dtoRecurso = new DtoRecurso() { id = recursoDisponible.Id, codigo = "recursoListarEvento" };
                lRecurso.Add(dtoRecurso);
                DtoRol rol = new DtoRol() { recursos = lRecurso, zonas = new List<DtoZona>() };

                if (logica.loguearUsuario(token, rol))
                {
                    List<DtoEvento> listaEventos =  logica.listarEventos(token).ToList();
                    int cantExt = listaEventos.FirstOrDefault().extensiones.Count;
                    Assert.IsTrue(cantExt == 2);
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
                var user = new Usuario() { NombreLogin = "usuarioDE", Nombre = "usuarioDE", Contraseña = Passwords.GetSHA1("usuarioDE"), Grupos_Recursos = new List<Grupo_Recurso>(), Unidades_Ejecutoras = new List<Unidad_Ejecutora>() };
                var recursoDisponible = new Recurso() { Codigo = "recursoDE", Estado = EstadoRecurso.Disponible, EstadoAsignacion = EstadoAsignacionRecurso.Libre, Extensiones_Eventos = new List<Extension_Evento>() };
                var gr = new Grupo_Recurso() { Nombre = "grDEPrueba", Recursos = new List<Recurso>() };
                var zona1 = new Zona() { Nombre = "zonaDE1" };
                var unidadEjecutora1 = new Unidad_Ejecutora() { Nombre = "ueDEPrueba", Zonas = new List<Zona>() };

                unidadEjecutora1.Zonas.Add(zona1);
                zona1.UnidadEjecutora = unidadEjecutora1;
                user.Unidades_Ejecutoras.Add(unidadEjecutora1);

                gr.Recursos.Add(recursoDisponible);
                user.Grupos_Recursos.Add(gr);
                context.Zonas.Add(zona1);

                context.Unidades_Ejecutoras.Add(unidadEjecutora1);
                context.Recursos.Add(recursoDisponible);
                context.Grupos_Recursos.Add(gr);
                context.Users.Add(user);
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
                    Zona = zona1,
                    Estado = EstadoExtension.Despachado,
                    TimeStamp = DateTime.Now,
                    Despachador = user,
                    DescripcionDespachador = "2016/07/23 21:30:00\\UsuarioDespachador\\descripcion de evento\\2016/07/23 21:37:00\\UsuarioDespachador2\\otra descripcion de evento\\2016/07/24 10:37:00\\UsuarioDespachador2\\otra mas"
                };

                IMetodos logica = new Metodos();

                // Obtengo token de usuario. 
                var autent = logica.autenticarUsuario("usuarioDE", "usuarioDE");
                string token = autent.access_token;
                
                recursoDisponible.Extensiones_Eventos.Add(ext1);
                try
                {
                    context.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    throw e;
                }

                List<DtoRecurso> lRecurso = new List<DtoRecurso>();
                DtoRecurso dtoRecurso = new DtoRecurso() { id = recursoDisponible.Id, codigo = "recursoListarEvento" };
                lRecurso.Add(dtoRecurso);
                DtoRol rol = new DtoRol() { recursos = lRecurso, zonas = new List<DtoZona>() };

                if (logica.loguearUsuario(token, rol))
                {
                    DtoEvento dtoEvento = logica.verInfoEvento(token, evento.Id);
                    DtoExtension dtoExt = dtoEvento.extensiones.FirstOrDefault();

                    // Compruebo que existan los 3 dtoDescripcion
                    Assert.AreEqual(dtoExt.descripcion_despachadores.Count, 3);

                    // Comrpuebo que los dtos tengan el texto correspondiente
                    DateTime fecha1 = DateTime.Parse("2016/07/23 21:30:00");
                    DateTime fecha2 = DateTime.Parse("2016/07/23 21:37:00");
                    DateTime fecha3 = DateTime.Parse("2016/07/24 10:37:00");
                    for (int i = 0; i < 3; i++)
                    {
                        DtoDescripcion dtoDesc = dtoExt.descripcion_despachadores.ElementAt(i);
                        switch (i)
                        {
                            case 0:
                                Assert.AreEqual(dtoDesc.fecha, fecha1);
                                Assert.AreEqual(dtoDesc.usuario, "UsuarioDespachador");
                                Assert.AreEqual(dtoDesc.texto, "descripcion de evento");
                                break;
                            case 1:
                                Assert.AreEqual(dtoDesc.fecha, fecha2);
                                Assert.AreEqual(dtoDesc.usuario, "UsuarioDespachador2");
                                Assert.AreEqual(dtoDesc.texto, "otra descripcion de evento");
                                break;
                            case 2:
                                Assert.AreEqual(dtoDesc.fecha, fecha3);
                                Assert.AreEqual(dtoDesc.usuario, "UsuarioDespachador2");
                                Assert.AreEqual(dtoDesc.texto, "otra mas");
                                break;
                            default:
                                break;
                        }
                    }
                }
                
            }
            

        }

    }
}
