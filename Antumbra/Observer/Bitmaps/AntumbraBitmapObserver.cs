using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Antumbra.Glow.Utility;

namespace Antumbra.Glow.Observer.Bitmaps {
    public interface AntumbraBitmapObserver {
        void NewBitmapAvail(Bitmap image, EventArgs args);
    }
}
