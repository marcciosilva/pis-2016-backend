namespace Emsys.DataAccesLayer.Core
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Initializer : MigrateDatabaseToLatestVersion<EmsysContext, Configuration>
    {
    }
}