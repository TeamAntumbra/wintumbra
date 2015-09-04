﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antumbra.Glow.ExtensionFramework;
using Antumbra.Glow.Observer.ScreenInfo;

namespace Antumbra.Glow.ExtensionFramework.Types {
    public abstract class GlowScreenGrabber : GlowExtension//observed by screen processor
        //special type of driver that deals with bitmaps captured from the screen
        //uses a GlowScreenProcessor to determine color to return
    {
        public abstract void AttachObserver(AntumbraScreenInfoObserver observer);
        public int x { get; set; }
        public int y { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public int captureThrottle { get; set; }
        public abstract GlowScreenGrabber Create();
        public sealed override Type GetExtensionType() {
            return typeof(GlowScreenGrabber);
        }
    }
}
