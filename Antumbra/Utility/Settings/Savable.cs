using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antumbra.Glow.Utility.Settings
{
    public interface Savable
    {
        void SaveSettings();
        void LoadSettings(String settings);
        void ResetSettings();
    }
}
