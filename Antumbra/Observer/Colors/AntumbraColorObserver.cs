using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Antumbra.Glow.Observer.Colors
{
    public interface AntumbraColorObserver
    {
        void NewColorAvail(Color newCol, EventArgs args);
    }
}
