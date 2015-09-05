using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antumbra.Glow.Observer.Configuration {
    public interface ConfigurationChanger {
        void ConfigChange(Settings.SettingsDelta Delta);
    }
}
