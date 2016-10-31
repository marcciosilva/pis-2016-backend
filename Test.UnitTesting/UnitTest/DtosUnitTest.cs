using DataTypeObject;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Test.UnitTesting
{
    [TestFixture]
    public class DtosUnitTest
    {
      
        /// <summary>
        /// Prueba dto.
        /// </summary>
        [Test]
        public void DtoActualizarDescripcionParametroTest()
        {
            DtoDescripcion desc = new DtoDescripcion() { descripcion = "desc", fecha = DateTime.Now, origen = OrigenDescripcion.Recurso, usuario = "usuario" };

            var dto = new DtoActualizarDescripcion() {descripcion = "algo", idExtension = 1 };

            Assert.IsTrue(dto.idExtension == 1);
            Assert.IsTrue(dto.descripcion == "algo");
        }

        /// <summary>
        /// Prueba dto.
        /// </summary>
        [Test]
        public void DtoAsignacionRecursoTest()
        {
            var dto = new DtoAsignacionRecurso() { id = 1, recurso = "recurso1", descripcion = new List<DtoDescripcion>(), fechaArribo = DateTime.Now, actualmenteAsignado = false };

            Assert.IsTrue((dto.id == 1) && (dto.recurso == "recurso1") && (dto.descripcion.Count == 0) && (dto.fechaArribo != null) && (dto.actualmenteAsignado == false));
        }

        /// <summary>
        /// Prueba dto.
        /// </summary>
        [Test]
        public void DtoAudioTest()
        {
            var dto = new DtoAudio() { id = 1, idAudio = 1, usuario = "usuario", fechaEnvio = DateTime.Now, idExtension = 1 };

            Assert.IsTrue((dto.id == 1) && (dto.idAudio == 1) && (dto.usuario == "usuario") && (dto.fechaEnvio != null) && (dto.idExtension == 1));
        }

        /// <summary>
        /// Prueba dto.
        /// </summary>
        [Test]
        public void DtoCategoriaTest()
        {
            var dto = new DtoCategoria() { id = 1, codigo = "codigo", clave = "clave", prioridad = "alta", activo = false };

            Assert.IsTrue((dto.id == 1) && (dto.clave == "clave") && (dto.codigo == "codigo") && (dto.prioridad == "alta") && (dto.activo == false));
        }

        /// <summary>
        /// Prueba dto.
        /// </summary>
        [Test]
        public void DtoConsultaExternaTest()
        {
            var dto = new DtoConsultaExterna() { param1 = "uno", param2 = "dos", param3 = "tres" };

            Assert.IsTrue((dto.param1 == "uno") && (dto.param2 == "dos") && (dto.param3 == "tres"));
        }

        /// <summary>
        /// Prueba dto.
        /// </summary>
        [Test]
        public void DtoDescripcionTest()
        {
            var dto = new DtoDescripcion() { usuario = "usuario", descripcion = "texto", origen = OrigenDescripcion.Recurso };

            Assert.IsTrue((dto.usuario == "usuario") && (dto.descripcion == "texto") && (dto.origen == OrigenDescripcion.Recurso));
        }

        /// <summary>
        /// Prueba dto.
        /// </summary>
        [Test]
        public void DtoEventoTest()
        {
            var dto = new DtoEvento()
            {
                audios = new List<DtoAudio>(),
                calle = "calle",
                categoria = null,
                creador = "creador",
                departamento = "departamento",
                descripcion = "descripcion",
                enProceso = false,
                esquina = "esquina",
                estado = "estado",
                extensiones = new List<DtoExtension>(),
                fechaCreacion = DateTime.Now,
                id = 1,
                imagenes = new List<DtoImagen>(),
                informante = "informante",
                latitud = 10,
                longitud = 11,
                numero = "1",
                sector = "sector",
                telefono = "tel",
                timeStamp = DateTime.Now,
                videos = new List<DtoVideo>()
            };

            Assert.AreEqual(dto.calle, "calle");
            Assert.AreEqual(dto.categoria, null);
            Assert.AreEqual(dto.creador, "creador");
            Assert.AreEqual(dto.departamento, "departamento");
            Assert.AreEqual(dto.descripcion, "descripcion");
            Assert.AreEqual(dto.enProceso, false);
            Assert.AreEqual(dto.esquina, "esquina");
            Assert.AreEqual(dto.estado, "estado");
            Assert.AreEqual(dto.id, 1);
            Assert.AreEqual(dto.informante, "informante");
            Assert.AreEqual(dto.latitud, 10);
            Assert.AreEqual(dto.longitud, 11);
            Assert.AreEqual(dto.numero, "1");
            Assert.AreEqual(dto.sector, "sector");
            Assert.AreEqual(dto.telefono, "tel");
            Assert.IsNotNull(dto.fechaCreacion);
            Assert.IsNotNull(dto.timeStamp);
            Assert.AreEqual(dto.audios.Count, 0);
            Assert.AreEqual(dto.videos.Count, 0);
            Assert.AreEqual(dto.imagenes.Count, 0);
            Assert.AreEqual(dto.extensiones.Count, 0);
        }

        /// <summary>
        /// Prueba dto.
        /// </summary>
        [Test]
        public void DtoExtensionTest()
        {
            var dto = new DtoExtension()
            {
                audios = new List<DtoAudio>(),
                asignacionesRecursos = new List<DtoAsignacionRecurso>(),
                descripcionDespachadores = new List<DtoDescripcion>(),
                descripcionSupervisor = "descsuper",
                despachador = "despachador",
                geoUbicaciones = new List<DtoGeoUbicacion>(),
                recursos = new List<string>(),
                segundaCategoria = null,
                zona = new DtoZona(),               
                estado = "estado",              
                id = 1,
                imagenes = new List<DtoImagen>(),
                timeStamp = DateTime.Now,
                videos = new List<DtoVideo>()
            };

            Assert.AreEqual(dto.descripcionSupervisor, "descsuper");
            Assert.AreEqual(dto.despachador, "despachador");
            Assert.AreEqual(dto.segundaCategoria, null);
            Assert.AreNotEqual(dto.zona, null);
            Assert.AreEqual(dto.estado, "estado");
            Assert.AreEqual(dto.id, 1);
            Assert.IsNotNull(dto.timeStamp);
            Assert.AreEqual(dto.audios.Count, 0);
            Assert.AreEqual(dto.videos.Count, 0);
            Assert.AreEqual(dto.imagenes.Count, 0);
            Assert.AreEqual(dto.recursos.Count, 0);
            Assert.AreEqual(dto.asignacionesRecursos.Count, 0);
            Assert.AreEqual(dto.geoUbicaciones.Count, 0);
            Assert.AreEqual(dto.descripcionDespachadores.Count, 0);
        }

        /// <summary>
        /// Prueba dto.
        /// </summary>
        [Test]
        public void DtogeoUbicacionTest()
        {
            var dto = new DtoGeoUbicacion() { usuario = "usuario", fechaEnvio = DateTime.Now };

            Assert.IsTrue((dto.usuario == "usuario") && (dto.fechaEnvio != null));
        }

        /// <summary>
        /// Prueba dto.
        /// </summary>
        [Test]
        public void DtoImagenTest()
        {
            var dto = new DtoImagen() { id = 1, usuario = "usuario", fechaEnvio = DateTime.Now };

            Assert.IsTrue((dto.usuario == "usuario") && (dto.fechaEnvio != null) && (dto.fechaEnvio != null));
        }

        /// <summary>
        /// Prueba dto.
        /// </summary>
        [Test]
        public void DtoOrigenEventotest()
        {
            var dto = new DtoOrigenEvento() { id = 1, idOrigen = 1, tipoOrigen = "tipo" };

            Assert.IsTrue((dto.id == 1) && (dto.idOrigen == 1) && (dto.tipoOrigen == "tipo"));
        }

        /// <summary>
        /// Prueba dto.
        /// </summary>
        [Test]
        public void DtoVideoTest()
        {
            var dto = new DtoVideo() { id = 1, usuario = "usuario", fechaEnvio = DateTime.Now };

            Assert.IsTrue((dto.usuario == "usuario") && (dto.fechaEnvio != null) && (dto.fechaEnvio != null));
        }

        /// <summary>
        /// Prueba dto.
        /// </summary>
        [Test]
        public void DtoZonaTest()
        {
            var dto = new DtoZona() { id = 1, nombre = "nombre", nombreUe = "ue" };

            Assert.IsTrue((dto.id == 1) && (dto.nombre == "nombre") && (dto.nombreUe == "ue"));
        }

        /// <summary>
        /// Prueba dto.
        /// </summary>
        [Test]
        public void DtoRespuestaExternaTest()
        {
            var dto = new DtoRespuestaExterna("uno", "dos", "tres", "DDD", "EEE", "FFF", "GGG", "HHH", "III", "JJJ");
            Assert.AreEqual(dto.field1, "uno");
            Assert.AreEqual(dto.field2, "dos");
            Assert.AreEqual(dto.field3, "tres");
            Assert.AreEqual(dto.field4, "DDD");
            Assert.AreEqual(dto.field5, "EEE");
            Assert.AreEqual(dto.field6, "FFF");
            Assert.AreEqual(dto.field7, "GGG");
            Assert.AreEqual(dto.field8, "HHH");
            Assert.AreEqual(dto.field9, "III");
            Assert.AreEqual(dto.field10, "JJJ");
        }        
    }
}