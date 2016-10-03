using Emsys.DataAccesLayer.Core;
using Emsys.DataAccesLayer.Metodos;
using Emsys.DataAccesLayer.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaAutenticacion
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            string nombre, pass;
            Console.WriteLine("Nombre de usuario:");
            nombre = Console.ReadLine();
            Console.WriteLine("Password:");
            pass = Console.ReadLine();
            Autentificacion a = new Autentificacion();
            Console.WriteLine(a.Registrar(nombre, pass));
            Console.ReadLine();
            */
            
            EmsysContext context = new EmsysContext();
            //EmsysUserManager man = new EmsysUserManager();
            ApplicationUser user = context.Users.FirstOrDefault();

            
            context.Logs.Add(new Emsys.DataAccesLayer.Model.Log() { TimeStamp = new DateTime(2010, 8, 18), idEntidad = 1, Usuario= user, Modulo = "hola.. bebeh" });
            context.SaveChanges();
            
            Console.WriteLine(user.Logs.FirstOrDefault().Modulo);
            Console.ReadLine();
        }
    }
}
