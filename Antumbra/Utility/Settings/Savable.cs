using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antumbra.Glow.Utility.Settings
{
    public interface Savable
    {
        void Save();
        Object LoadSave(String settings);
        void Reset();
    }
}
