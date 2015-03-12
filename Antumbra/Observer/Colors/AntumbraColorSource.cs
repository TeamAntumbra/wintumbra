using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antumbra.Glow.Observer.Colors
{
    public interface AntumbraColorSource
    {
        void AttachObserver(AntumbraColorObserver observer);
    }
}
