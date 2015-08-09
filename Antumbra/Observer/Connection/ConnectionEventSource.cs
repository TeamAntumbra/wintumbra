using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antumbra.Glow.Observer.Connection
{
    public interface ConnectionEventSource
    {
        void AttachObserver(ConnectionEventObserver observer);
    }
}
