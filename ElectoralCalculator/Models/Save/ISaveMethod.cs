using ElectoralCalculator.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectoralCalculator.Models.Save
{
    public interface ISaveMethod
    {
        void Save(StatsNonDb stats);
    }
}
