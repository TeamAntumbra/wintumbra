using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antumbra.Glow.Logging
{
    public interface Loggable//TODO move
    {
        void AttachEvent(LogMsgObserver observer);
    }
}
