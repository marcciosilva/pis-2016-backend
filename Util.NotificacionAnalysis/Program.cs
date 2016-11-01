using Emsys.DataAccesLayer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emsys.DataAccesLayer.Model;

namespace Util.NotificacionAnalysis
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("COMANDOS:");
            Console.WriteLine("Enter => veo el tiempo la cantidad de envios exitosos, la cantidad de envios reales, cantidad envio error");
            Console.WriteLine("1 => Analisis de tiempo maximo, minimo, promedio de las notificaciones.");

            Console.WriteLine();
            while (true)
            {
                Console.WriteLine("Ingrese un comando: ");
                Console.Write(">");
                var comando = Console.ReadLine();
                if (comando == "")
                {
                    using (EmsysContext db = new EmsysContext())
                    {
                        var cantidadEnviosReales = db.LogNotification.Where(x => x.Codigo == 901).Count();
                        var cantidadEnviosExitosos = db.LogNotification.Where(x => x.Codigo == 906).Count();
                        var cantidadEnviosError = db.LogNotification.Where(x => x.Codigo == 904).Count();
                        if (cantidadEnviosReales != cantidadEnviosExitosos)
                        {
                            Console.WriteLine("Error: la cantidad de envios exitosos es distintas a los envios reales.");
                        }

                        Console.WriteLine("Envios Exitosos: " + cantidadEnviosExitosos);
                        Console.WriteLine("Envios Reales: " + cantidadEnviosReales);
                        Console.WriteLine("Envios Error: " + cantidadEnviosError);
                        Console.WriteLine("Envios Totales(Reales + errores): " + (cantidadEnviosError + cantidadEnviosReales));
                        Console.WriteLine();
                    }
                }
                else if (comando == "1")
                {
                    using (EmsysContext db = new EmsysContext())
                    {
                        double max = 0;
                        double min = 0;
                        double average = 0;
                        int nivelRecursion = 0;
                        var logs = db.LogNotification.ToList().OrderByDescending(x => x.Id);
                        foreach (var item in logs)
                        {
                            if (item.Codigo == 906)
                            {
                                //cuento cuantas hay para atras.
                                DateTime tiempoFin = item.TimeStamp;
                                int nivel = 0;
                                DateTime timepoInicio = ObtenerInicio(item, ref nivel);
                                var diferencia = (tiempoFin - timepoInicio).TotalMilliseconds;
                                if (diferencia > max)
                                {
                                    max = diferencia;
                                }
                                if (diferencia < min)
                                {
                                    min = diferencia;
                                }
                                if (nivel > nivelRecursion)
                                {
                                    nivelRecursion = nivel;
                                }
                                average = (average + diferencia) / 2;
                            }
                        }
                        Console.WriteLine();
                        Console.WriteLine("Tiempo maximo de envio: " + max + " milisenconds.");
                        Console.WriteLine("Tiempo minimo de envio: " + min + " milisenconds.");
                        Console.WriteLine("Tiempo promedop de envio: " + average + " milisenconds.");
                        Console.WriteLine("Maximo nivel de recursion " + nivelRecursion + ".");
                        Console.WriteLine();
                    }
                }
                else
                {
                    Console.WriteLine("Comando invalido.");
                }
            }
        }

        private static DateTime ObtenerInicio(LogNotification item, ref int nivelRecursion)
        {
            if (item.LogNotificationPrevio == null)
            {
                return item.TimeStamp;
            }
            else
            {
                using (EmsysContext db = new EmsysContext())
                {
                    nivelRecursion++;
                    var logAnterior = db.LogNotification.Find(item.LogNotificationPrevio.Id);
                    return ObtenerInicio(logAnterior, ref nivelRecursion);

                }
            }
        }
    }
}
