using ElectoralCalculator.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectoralCalculator.Models.Save
{
    public interface IExportManager
    {
        void SaveFile(ISaveMethod method, StatsNonDb stats);
    }
}
