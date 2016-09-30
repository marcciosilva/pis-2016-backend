using Emsys.DataAccesLayer.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emsys.Logs
{
    public class Log
    {
        public static void AgregarLog(string IdUsuario, string terminal, string modulo, string Entidad, int idEntidad, string accion, string detalles, int codigo)
        {
            try
            {
                if (IdUsuario != null)
                {
                    using (EmsysContext db = new EmsysContext())
                    {
                        Emsys.DataAccesLayer.Model.Log log = new Emsys.DataAccesLayer.Model.Log();
                        log.Usuario = IdUsuario;
                        log.TimeStamp = DateTime.Now;
                        log.Terminal = terminal;
                        log.Modulo = modulo;
                        log.Entidad = Entidad;
                        log.idEntidad = idEntidad;
                        log.Accion = accion;
                        log.Detalles = detalles;
                        log.Codigo = codigo;
                        log.EsError = false;
                        db.Logs.Add(log);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "Errores"))
                {
                    Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "Errores");
                }
                string ruta = string.Format("{0}Errores\\{1}", AppDomain.CurrentDomain.BaseDirectory, DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss").Replace(" ", "").Replace(":", "_") + ".txt");

                StreamWriter fs = File.CreateText(ruta);
                fs.Write("Mensaje: " + " error al registrar un log " + e.Message + "\n" +
                        "HelpLink: " + e.HelpLink + "\n" +
                        "Hresult: " + e.HResult + "\n" +
                        "Innerexception: " + e.InnerException + "\n" +
                        "Source: " + e.Source + "\n" +
                        "StackTrace: " + e.StackTrace + "\n" +
                        "TargetSite: " + e.TargetSite + "\n"
                        );
                fs.Close();
               
            }
        }


        public static void AgregarLogError(string IdUsuario, string terminal, string modulo, string Entidad, int idEntidad, string accion, string detalles, int codigo)
        {
            try
            {
                using (EmsysContext db = new EmsysContext())
                {
                    Emsys.DataAccesLayer.Model.Log log = new Emsys.DataAccesLayer.Model.Log();
                    log.Usuario = IdUsuario;
                    log.TimeStamp = DateTime.Now;
                    log.Terminal = terminal;
                    log.Modulo = modulo;
                    log.Entidad = Entidad;
                    log.idEntidad = idEntidad;
                    log.Accion = accion;
                    log.Detalles = detalles;
                    log.Codigo = codigo;
                    log.EsError = false;
                    db.Logs.Add(log);
                    db.SaveChanges();

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.Message);
                if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "Errores"))
                {
                    Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "Errores");
                }
                string ruta = string.Format("{0}Errores\\{1}", AppDomain.CurrentDomain.BaseDirectory, DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss").Replace(" ", "").Replace(":", "_") + ".txt");

                StreamWriter fs = File.CreateText(ruta);
                fs.Write("Mensaje: " + " error al registrar un log " + e.Message + "\n" +
                        "HelpLink: " + e.HelpLink + "\n" +
                        "Hresult: " + e.HResult + "\n" +
                        "Innerexception: " + e.InnerException + "\n" +
                        "Source: " + e.Source + "\n" +
                        "StackTrace: " + e.StackTrace + "\n" +
                        "TargetSite: " + e.TargetSite + "\n"
                        );
                fs.Close();
            }
        }
    }
}
