using Emsys.DataAccesLayer.Core;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emsys.DataAccesLayer.Metodos
{
    public class Autentificacion
    {
        public string Registrar(string nombre, string pass) {
            string resp;
            var manager = new EmsysUserManager();
            var user = new IdentityUser() { UserName = nombre };
            IdentityResult result = manager.Create(user, pass);

            if (result.Succeeded)
            {
                resp= "Exito!";
            }
            else
            {
                resp = result.Errors.FirstOrDefault();
            }
            return resp;
        }
    }
}
