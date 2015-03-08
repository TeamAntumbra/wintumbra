using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antumbra.Glow.Observer.Settings
{
    public interface SavableObserver
    {
        void NewSettingsUpdate(Guid id, String serializedSettings);
    }
}
