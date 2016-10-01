﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http;
using Emsys.DataAccesLayer.Model;

namespace Servicios.Filtros
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly string[] PermisosEtiqueta;

        public CustomAuthorizeAttribute(params string[] permisos) {
            PermisosEtiqueta = permisos;
        }
        protected override bool IsAuthorized(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            bool autorizado = false;
            //voy a obtener el usuario del token
            IEnumerable<string> token;
            actionContext.Request.Headers.TryGetValues("auth", out token);
            using (Emsys.DataAccesLayer.Core.EmsysContext db=new Emsys.DataAccesLayer.Core.EmsysContext()) {
                var usuario = db.Users.Where(x=>x.Token==token.FirstOrDefault()).FirstOrDefault();
                if (usuario != null) {
                    foreach (var item in PermisosEtiqueta)
                    {
                        foreach (ApplicationRole ar in usuario.ApplicationRoles)
                        {
                            foreach (Permiso p in ar.Permisos)
                            {
                                if (item==p.Clave) {
                                    autorizado = true;
                                }
                            }
                        }
                    }
                }
                else {
                    return false;
                }
            }            
            return autorizado;
        }
    }
}
