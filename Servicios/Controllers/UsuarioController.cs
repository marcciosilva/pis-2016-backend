using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Emsys.DataAccesLayer.Model;

namespace Emsys.ServiceLayer.Controllers
{
    public class UsuarioController : ApiController
    {
        private EmsysContext db = new EmsysContext();

        // GET: api/Usuario
        //[RequireHttps]
        public IQueryable<Usuario> GetUsuario()
        {
            return db.Usuario;
        }

       // [RequireHttps]
        // GET: api/Usuario/5
        [ResponseType(typeof(Usuario))]
        public async Task<IHttpActionResult> GetUsuario(string id)
        {
            Usuario usuario = await db.Usuario.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }

        // PUT: api/Usuario/5
       // [RequireHttps]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUsuario(string id, Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != usuario.NombreUsuario)
            {
                return BadRequest();
            }

            db.Entry(usuario).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
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

        // POST: api/Usuario
    //    [RequireHttps]
        [ResponseType(typeof(Usuario))]
        public async Task<IHttpActionResult> PostUsuario(Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Usuario.Add(usuario);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UsuarioExists(usuario.NombreUsuario))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = usuario.NombreUsuario }, usuario);
        }

        // DELETE: api/Usuario/5
      //  [RequireHttps]
        [ResponseType(typeof(Usuario))]
        public async Task<IHttpActionResult> DeleteUsuario(string id)
        {
            Usuario usuario = await db.Usuario.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            db.Usuario.Remove(usuario);
            await db.SaveChangesAsync();

            return Ok(usuario);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UsuarioExists(string id)
        {
            return db.Usuario.Count(e => e.NombreUsuario == id) > 0;
        }
    }
}