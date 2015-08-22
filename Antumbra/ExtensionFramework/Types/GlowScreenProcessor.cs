using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Antumbra.Glow.Observer.Bitmaps;
using Antumbra.Glow.Observer.Colors;

namespace Antumbra.Glow.ExtensionFramework.Types {
    public abstract class GlowScreenProcessor : GlowExtension, AntumbraBitmapObserver, AntumbraColorSource {
        public abstract void NewBitmapAvail(Bitmap image, EventArgs args);
        public abstract void AttachObserver(AntumbraColorObserver observer);
        public abstract void SetArea(int x, int y, int width, int height);
        public abstract GlowScreenProcessor Create();
        public sealed override Type GetExtensionType() {
            return typeof(GlowScreenProcessor);
        }
    }
}
