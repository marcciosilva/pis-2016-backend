using System;
using TableDependency.Mappers;
using TableDependency.SqlClient;
using TableDependency.Enums;
using Emsys.DataAccesLayer.Model;
using Emsys.DataAccesLayer.Core;
using System.Linq;

namespace SqlDependecyProject
{
    class Program
    {
        private static bool llamo = true;
        static void Main(string[] args)
        {
            try
            {
                //para iniciar la bd si no esta creada
                EmsysContext db = new EmsysContext();                
                var eventos = db.Evento.FirstOrDefault();
                
                //me quedo loopeando
                while (true)
                {
                    if (llamo)
                    {
                        Console.WriteLine("Estoy esperando por modificaciones..");
                        Listener();
                        llamo = false;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Ocurrio un error: ");
                Console.WriteLine(e.Message);
                throw;
            }
        }

        static void Listener()
        {
            var mapper = new ModelToTableMapper<Evento>();
            mapper.AddMapping(model => model.NombreInformante, "NombreInformante");
            _dependency = new SqlTableDependency<Evento>(_connectionString, "Evento", mapper);
            _dependency.OnChanged += _dependency_OnChanged;
            _dependency.OnError += _dependency_OnError;
            _dependency.Start();
        }

        // private static IList<Model.Evento> _stocks;
        private static readonly string _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        // "data source=DESKTOP-T27K22L\\SQLExpressLocal;initial catalog=Prototipo1;integrated security=True";
        private static SqlTableDependency<Evento> _dependency;

        private static void _dependency_OnError(object sender, TableDependency.EventArgs.ErrorEventArgs e)
        {
            throw e.Error;
        }
        private static void _dependency_OnChanged(object sender, TableDependency.EventArgs.RecordChangedEventArgs<Evento> e)
        {
            if (e.ChangeType != ChangeType.None)
            {
                switch (e.ChangeType)
                {
                    case ChangeType.Delete:
                        Console.WriteLine("Boorrroo: " +e.Entity.NombreInformante);
                    //_stocks.Remove(_stocks.FirstOrDefault(c => c.NombreGenerador == e.Entity.NombreGenerador));
                        break;
                    case ChangeType.Insert:
                        Console.WriteLine("Agrego: " + e.Entity.NombreInformante);
                    //_stocks.Add(e.Entity);
                    break;
                    case ChangeType.Update:
                        //var customerIndex = _stocks.IndexOf(
                        //        _stocks.FirstOrDefault(c => c.NombreGenerador == e.Entity.NombreGenerador));
                        //if (customerIndex >= 0) _stocks[customerIndex] = e.Entity;
                        Console.WriteLine("Actualizo: " + e.Entity.NombreInformante);
                        break;
                }
            }
        }
    }
}
