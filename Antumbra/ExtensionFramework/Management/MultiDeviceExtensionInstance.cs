using Antumbra.Glow.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antumbra.Glow.ExtensionFramework.Management
{
    public class MultiDeviceExtensionInstance : ExtensionInstance
    {
        public MultiDeviceExtensionInstance(List<int> ids, ActiveExtensions actives)
            : base(ids.First<int>(), actives)
        {
            if (ids.Count == actives.ActiveProcessors.Count) {
                for (var i = 0; i < ids.Count; i += 1) {
                    actives.ActiveProcessors[i].devId = ids[i];
                }
            }
        }
    }
}
