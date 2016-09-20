using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Emsys.DataAccesLayer.Core
{
    public class MySqlInitializer : IDatabaseInitializer<EmsysContext>
    {
        public void InitializeDatabase(EmsysContext context)
        {
            if (!context.Database.Exists())
            {
                context.Database.Create();
            }
            else
            {
                //Esta query comprueba que exista la tabla de MigrationHistory
                var migrationHistoryTableExists = ((IObjectContextAdapter)context).ObjectContext.ExecuteStoreQuery<int>(
                    string.Format(
                        "SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = 'dbo' AND table_name= '_MigrationHistory'"));

                if (migrationHistoryTableExists.FirstOrDefault() == 0)
                {
                    context.Database.Delete();
                    context.Database.Create();
                }
            }
        }
    }
}
