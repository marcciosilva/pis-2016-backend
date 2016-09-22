using Emsys.DataAccesLayer.Metodos;
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
            string nombre, pass;
            Console.WriteLine("Nombre de usuario:");
            nombre = Console.ReadLine();
            Console.WriteLine("Password:");
            pass = Console.ReadLine();
            Autentificacion a = new Autentificacion();

            Console.WriteLine(a.Registrar(nombre, pass));
            Console.ReadLine();
        }
    }
}
