using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antumbra.Glow.Observer.Logging
{
    public interface Loggable
    {
        void AttachLogObserver(LogMsgObserver observer);
    }
}
