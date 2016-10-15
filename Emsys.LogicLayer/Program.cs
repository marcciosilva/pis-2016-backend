using Emsys.DataAccesLayer.Core;
using Emsys.DataAccesLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Emsys.LogicLayer
{
    class Program
    {
        public static void Main(string[] args)
        {
            using (var context = new EmsysContext())
            {
                IMetodos logica = new Metodos();
                DateTime ahora;
                Console.WriteLine("Started...");
                while (true)
                {
                    Console.WriteLine("Sleep...");
                    Thread.Sleep(1 * 60 * 1000);
                    ahora = DateTime.Now;
                    foreach (Usuario user in context.Users)
                    {
                        if (user.Token != null && user.UltimoSignal != null)
                        {
                            if ((ahora.Subtract(user.UltimoSignal.Value)).Minutes > 1)
                            {
                                logica.cerrarSesion(user.Token);
                            }
                        }
                    }
                }
            }
        }
    }
}
