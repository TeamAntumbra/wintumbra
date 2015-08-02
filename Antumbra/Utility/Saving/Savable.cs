using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antumbra.Glow.Utility.Saving
{
    public interface Savable
    {
        void Save();
        void Load();
        void Reset();
    }
}
