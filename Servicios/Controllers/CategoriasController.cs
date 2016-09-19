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
    public class CategoriasController : ApiController
    {
        private EmsysContext db = new EmsysContext();

        // GET: api/Categorias
        public IQueryable<Categoria> GetCategoria()
        {
            return db.Categoria;
        }

        // GET: api/Categorias/5
        [ResponseType(typeof(Categoria))]
        public IHttpActionResult GetCategoria(string id)
        {
            Categoria categoria = db.Categoria.Find(id);
            if (categoria == null)
            {
                return NotFound();
            }

            return Ok(categoria);
        }

        // PUT: api/Categorias/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCategoria(string id, Categoria categoria)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != categoria.Codigo)
            {
                return BadRequest();
            }

            db.Entry(categoria).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoriaExists(id))
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

        // POST: api/Categorias
        [ResponseType(typeof(Categoria))]
        public IHttpActionResult PostCategoria(Categoria categoria)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Categoria.Add(categoria);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (CategoriaExists(categoria.Codigo))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = categoria.Codigo }, categoria);
        }

        // DELETE: api/Categorias/5
        [ResponseType(typeof(Categoria))]
        public IHttpActionResult DeleteCategoria(string id)
        {
            Categoria categoria = db.Categoria.Find(id);
            if (categoria == null)
            {
                return NotFound();
            }

            db.Categoria.Remove(categoria);
            db.SaveChanges();

            return Ok(categoria);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CategoriaExists(string id)
        {
            return db.Categoria.Count(e => e.Codigo == id) > 0;
        }
    }
}