using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Antumbra.Glow.ExtensionFramework
{
    public interface GlowExtension//basis for all Glow Extensions
    {
        String Name { get; }
        String Author { get; }
        String Version { get; }
        String Description { get; }
        String Type { get; }//one of the following: Decorator, Driver, Notifier, ScreenProcessor
    }
}
