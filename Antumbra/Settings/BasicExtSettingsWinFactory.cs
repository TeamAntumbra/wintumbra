using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antumbra.Glow.ExtensionFramework;
using Antumbra.Glow.ExtensionFramework.Management;

namespace Antumbra.Glow.Settings
{
    public class BasicExtSettingsWinFactory
    {
        private ExtensionLibrary lib;
        public BasicExtSettingsWinFactory(ExtensionLibrary lib)
        {
            this.lib = lib;
        }
        public AntumbraExtSettingsWindow GenerateWindow(Guid id)
        {
            GlowExtension ext = this.lib.findExt(id);
            return new AntumbraExtSettingsWindow(ext);
        }
    }
}
