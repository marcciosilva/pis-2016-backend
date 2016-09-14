using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Emsys.DataAccesLayer.Core;
using Emsys.DataAccesLayer.Model;

namespace Servicios.Controllers
{
    public class ZonasController : ApiController
    {
        private EmsysContext db = new EmsysContext();

        // GET: api/Zonas
        public IQueryable<Zona> GetZona()
        {
            return db.Zona;
        }

        // GET: api/Zonas/5
        [ResponseType(typeof(Zona))]
        public IHttpActionResult GetZona(string id)
        {
            Zona zona = db.Zona.Find(id);
            if (zona == null)
            {
                return NotFound();
            }

            return Ok(zona);
        }

        // PUT: api/Zonas/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutZona(string id, Zona zona)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != zona.NombreZona)
            {
                return BadRequest();
            }

            db.Entry(zona).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ZonaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Zonas
        [ResponseType(typeof(Zona))]
        public IHttpActionResult PostZona(Zona zona)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Zona.Add(zona);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ZonaExists(zona.NombreZona))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = zona.NombreZona }, zona);
        }

        // DELETE: api/Zonas/5
        [ResponseType(typeof(Zona))]
        public IHttpActionResult DeleteZona(string id)
        {
            Zona zona = db.Zona.Find(id);
            if (zona == null)
            {
                return NotFound();
            }

            db.Zona.Remove(zona);
            db.SaveChanges();

            return Ok(zona);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ZonaExists(string id)
        {
            return db.Zona.Count(e => e.NombreZona == id) > 0;
        }
    }
}