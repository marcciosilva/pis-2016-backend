using Emsys.DataAccesLayer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emsys.DataAccesLayer.Model;

namespace Util.NotificacionAnalysis
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("COMANDOS:");
            Console.WriteLine("1 => Analisis de tiempo maximo, minimo, promedio de las notificaciones.");

            Console.WriteLine();
            while (true)
            {
                Console.WriteLine("Ingrese un comando: ");
                Console.Write(">");
                var comando = Console.ReadLine();
                if (comando == "1")
                {
                    CapturoDatos();
                }
                else
                {
                    Console.WriteLine("Comando invalido.");
                }
            }
        }

        public static void CapturoDatos()
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

                double tiempoMaximoEsperaNotificacion = 0;
                double tiempoMinimoEsperaNotificacion = 0;
                double tiempoPromedioEnvioNotificacion = 0;

                int nivelRecursion = 0;
                var logs = db.LogNotification.ToList().OrderByDescending(x => x.Id);
                DateTime tiempoMenor = logs.FirstOrDefault().TimeStamp;
                DateTime tiempoMayor = logs.FirstOrDefault().TimeStamp;
                foreach (var item in logs)
                {
                    if (item.Codigo == 906)
                    {
                        //cuento cuantas hay para atras.
                        DateTime tiempoFin = item.TimeStamp;
                        int nivel = 0;
                        DateTime timepoInicio = ObtenerInicio(item, ref nivel);
                        var diferencia = (tiempoFin - timepoInicio).TotalMilliseconds;
                        if (diferencia > tiempoMaximoEsperaNotificacion)
                        {
                            tiempoMaximoEsperaNotificacion = diferencia;
                        }
                        if (diferencia < tiempoMinimoEsperaNotificacion)
                        {
                            tiempoMinimoEsperaNotificacion = diferencia;
                        }
                        if (nivel > nivelRecursion)
                        {
                            nivelRecursion = nivel;
                        }
                        tiempoPromedioEnvioNotificacion = (tiempoPromedioEnvioNotificacion + diferencia) / 2;
                        if (tiempoMenor > item.TimeStamp)
                        {
                            tiempoMenor = item.TimeStamp;
                        }
                        if (tiempoMayor < item.TimeStamp)
                        {
                            tiempoMayor = item.TimeStamp;
                        }
                    }
                }

                double duracionRafaga = (tiempoMayor - tiempoMenor).TotalMilliseconds;
                var cantidadTopics = db.LogNotification.Where(y => y.Codigo == 901).GroupBy(x => x.Topic).Count();
                var topicsRateNotification = db.LogNotification.Where(y => y.Codigo == 901).GroupBy(x => x.Topic);
                var codigosNotificacionesEnviados = db.LogNotification.Where(y => y.Codigo == 901).GroupBy(x => x.CodigoNotificacion);
                var cantidadCodigosNotificaciones = codigosNotificacionesEnviados.Count();
                Console.WriteLine();
                Console.WriteLine("Tiempo maximo de envio: " + tiempoMaximoEsperaNotificacion + " milisenconds.");
                Console.WriteLine("Tiempo minimo de envio: " + tiempoMinimoEsperaNotificacion + " milisenconds.");
                Console.WriteLine("Tiempo promedio de envio: " + tiempoPromedioEnvioNotificacion + " milisenconds.");
                Console.WriteLine("Duracion de rafaga de notificaciones: " + duracionRafaga + " miliseconds.");
                Console.WriteLine("Maximo nivel de recursion " + nivelRecursion + ".");
                Console.WriteLine("Cantidad topics " + cantidadTopics + ".");
                Console.WriteLine("Taza de notificaciones por topic: ");
                foreach (var item in topicsRateNotification)
                {
                    Console.WriteLine("--->  " + item.FirstOrDefault().Topic + " - " + item.Count());
                }
                Console.WriteLine("Cantidad codigos de notificaciones " + cantidadCodigosNotificaciones + ".");
                Console.WriteLine("Taza de notificaciones por codigo de notificacion: ");
                foreach (var item in codigosNotificacionesEnviados)
                {
                    Console.WriteLine("--->  " + item.FirstOrDefault().CodigoNotificacion + " - " + item.Count());
                }
                Console.WriteLine();

                using (Model.AnalysisContextContainer dbDatos= new Model.AnalysisContextContainer())
                {
                    Model.ScreenShotDatos datos = new Model.ScreenShotDatos()
                    {
                        cantidadCodigosNotificaciones = cantidadCodigosNotificaciones,
                        cantidadEnviosError = cantidadEnviosError,
                        cantidadEnviosExitosos = cantidadEnviosExitosos,
                        cantidadEnviosReales = cantidadEnviosReales,
                        cantidadTopics = cantidadTopics,
                        duracionRafaga = duracionRafaga,
                        nivelRecursion = nivelRecursion,
                        tiempoMaximoEsperaNotificacion = tiempoMaximoEsperaNotificacion,
                        tiempoMinimoEsperaNotificacion = tiempoMinimoEsperaNotificacion,
                        tiempoPromedioEnvioNotificacion = tiempoPromedioEnvioNotificacion,
                        TimeStamp = DateTime.Now
                    };
                    dbDatos.ScreenShotDatosSet.Add(datos);
                    dbDatos.SaveChanges();
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
