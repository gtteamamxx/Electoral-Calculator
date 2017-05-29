using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectoralCalculator.Models.Database;

namespace ElectoralCalculator.Models.Save
{
    public class ExportManager : IExportManager
    {
        public void SaveFile(ISaveMethod method, StatsNonDb stats)
            => method.Save(stats);
    }
}
