using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;
using Emsys.DataAccesLayer.Core;
using System.Data.Entity;
using System.IO;
using System.Reflection;

namespace Test.UnitTesting
{
    [SetUpFixture]
    class Configuracion
    {
        [OneTimeSetUp]
        public void ConfiguracionInicial()
        {
            string nombreBD = "EmsysBackendTestingDB";
            //string scriptInicial = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"App_Data\DatosBD.sql");
            //string scriptInicial = Environment.CurrentDirectory + "\\App_Data\\DatosBD.sql";
            string scriptInicial = AppDomain.CurrentDomain.BaseDirectory + "\\App_Data\\DatosBD.sql";
            string connectionString = String.Format(@"Data Source=(LocalDB)\MSSQLLocalDB;Integrated Security=True;MultipleActiveResultSets=True;");
            
            using (var con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();

                    // Se crea la BD y si existe una se borra.
                    CrearBD(con, nombreBD);

                    // Se carga la BD con el script inicial.
                    using (var context = new EmsysContext()) 
                    {
                        Database db = context.Database;
                        db.Initialize(false);
                        context.SaveChanges();
                    }

                    EjecutarSQL(con, scriptInicial);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        private void CrearBD(SqlConnection con, string nombreBD)
        {
            try
            {
                Create(con, nombreBD);
            }
            catch (Exception ex)
            {
                if (ex.Message.ToLower().Contains("already exists. choose a different database name"))
                {
                    string scriptBorrar = @"USE master;
                        GO
                        ALTER DATABASE " + nombreBD + @" SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                        GO
                        DROP DATABASE [" + nombreBD + "]";
                    EjecutarSQL(con, "", scriptBorrar);                    
                    Create(con, nombreBD);
                }
            }
        }

        private static void Create(SqlConnection con, string nombreBD)
        {
            using (var cmd = new SqlCommand("CREATE DATABASE " + nombreBD, con))
            {
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
            }

            con.ChangeDatabase(nombreBD);
        }

        private void EjecutarSQL(SqlConnection con, string nomreArchivoSQL, string sql = null)
        {
            try
            {
                string script;
                if (String.IsNullOrEmpty(sql))
                {
                    script = System.IO.File.ReadAllText(nomreArchivoSQL);
                }
                else
                {
                    script = sql;
                }

                IEnumerable<string> commandString = Regex.Split(script, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);

                foreach (var command in commandString)
                {
                    if (command.Trim() != "")
                    {
                        using (var c = new SqlCommand(command, con))
                        {
                            c.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
