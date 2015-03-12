using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antumbra.Glow.ExtensionFramework;
using Antumbra.Glow.Observer.Bitmaps;

namespace Antumbra.Glow.ExtensionFramework.Types
{
    public abstract class GlowScreenGrabber : GlowExtension//observed by screen processor
    //special type of driver that deals with bitmaps captured from the screen
    //uses a GlowScreenProcessor to determine color to return
    {
        public abstract void AttachObserver(AntumbraBitmapObserver observer);
        public int x { get; set; }
        public int y { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }
}
