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
                var user = new ApplicationUser() { NombreUsuario = "usuarioListarEventoRecurso", Nombre = "usuarioListarEventoRecurso", Contraseña = Passwords.GetSHA1("usuarioListarEventoRecurso"), Grupos_Recursos = new List<Grupo_Recurso>(), Unidades_Ejecutoras = new List<Unidad_Ejecutora>() };
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
                zona1.Unidad_Ejecutora = unidadEjecutora1;
                zona2.Unidad_Ejecutora = unidadEjecutora1;
                zona3.Unidad_Ejecutora = unidadEjecutora2;
                zona4.Unidad_Ejecutora = unidadEjecutora3;
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
                    TimeStamp = DateTime.Now
                };
                var ext2 = new Extension_Evento()
                {
                    Evento = evento,
                    Zona = zona2,
                    Estado = EstadoExtension.Despachado,
                    TimeStamp = DateTime.Now
                };
                var ext3 = new Extension_Evento()
                {
                    Evento = evento,
                    Zona = zona3,
                    Estado = EstadoExtension.Despachado,
                    TimeStamp = DateTime.Now
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
        /// Se llama luego de correr cada test y borra la base de datos.
        /// </summary>
        [SetUp]
        public void limpiarBase()
        {
            using (var context = new EmsysContext())
            {
                foreach (var u in context.Evento)
                {
                    context.Evento.Remove(u);
                }

                foreach (var u in context.Users)
                {
                    context.Users.Remove(u);
                }
                foreach (var gr in context.Grupos_Recursos)
                {
                    context.Grupos_Recursos.Remove(gr);
                }
                foreach (var r in context.Recursos)
                {
                    context.Recursos.Remove(r);
                }
                foreach (var ue in context.Unidades_Ejecutoras)
                {
                    context.Unidades_Ejecutoras.Remove(ue);
                }
                foreach (var sector in context.Sectores)
                {
                    context.Sectores.Remove(sector);
                }

                foreach (var zona in context.Zonas)
                {
                    context.Zonas.Remove(zona);
                }
                context.SaveChanges();
            }
        }
    }
}
