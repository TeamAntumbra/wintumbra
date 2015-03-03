using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antumbra.Glow.Observer.Bitmaps
{
    public interface AntumbraBitmapSource
    {
        void AttachBitmapObserver(AntumbraBitmapObserver observer);
    }
}
