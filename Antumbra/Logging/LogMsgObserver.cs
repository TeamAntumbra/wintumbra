using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antumbra.Glow.Logging
{
    public interface LogMsgObserver//TODO move
    {
        void NewLogMsgAvail(String sourceName, String msg);
    }
}
