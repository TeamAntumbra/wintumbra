using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimDX.Direct3D9;
using System.Drawing;

namespace SlimDXCapture
{
    class SlimDXCapturer
    {
        private Device dxDev;
        public SlimDXCapturer()
        {
            SlimDX.Configuration.AddResultWatch(ResultCode.NotAvailable, SlimDX.ResultWatchFlags.AlwaysIgnore);
            PresentParameters pp = new PresentParameters();
            pp.Windowed = true;
            pp.SwapEffect = SwapEffect.Discard;
            this.dxDev = new Device(new Direct3D(), 0, DeviceType.Hardware, IntPtr.Zero, CreateFlags.MixedVertexProcessing, pp);
        }

        public Surface getScreenShot(int width, int height)
        {
            Surface s = Surface.CreateOffscreenPlain(this.dxDev, width, height, Format.A8R8G8B8, Pool.SystemMemory);
            dxDev.GetFrontBufferData(0, s);
            dxDev.Dispose();
            return s;
        }
    }
}
