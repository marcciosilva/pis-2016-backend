using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaBase
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //para iniciar la bd si no esta creada
                EmsysContext db = new EmsysContext();
                db.
            }
            catch (Exception e)
            {
                Console.WriteLine("Ocurrio un error: ");
                Console.WriteLine(e.Message);
                throw;
            }



        }
    }
}
