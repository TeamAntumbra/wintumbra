using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Antumbra.Glow.Utility
{
    public class FastBitmap : IDisposable
    {
        private Bitmap bitmap;
        private BitmapData data;
        private bool locked;
        private byte[] pxData;
        private IntPtr scan0;
        public int height { get; private set; }
        public int width { get; private set; }
        private int depth;
        public FastBitmap(Bitmap bm)
        {
            this.bitmap = bm;
        }
        /*
         * Locks the underlying Bitmap object for faster manipulation of bits.
         */
        public void Lock()
        {
            try {
                Size size = this.bitmap.Size;
                this.width = size.Width;
                this.height = size.Height;
                PixelFormat pxFormat = this.bitmap.PixelFormat;
                this.depth = Bitmap.GetPixelFormatSize(pxFormat);
                if (this.depth != 8 && this.depth != 24 && this.depth != 32) {
                    throw new ArgumentException("Only 8, 24 and 32 bpp images are supported.");
                }
                this.data = this.bitmap.LockBits(new Rectangle(new Point(0, 0), size), ImageLockMode.ReadOnly,
                    pxFormat);
                int bpp = this.depth / 8;
                this.pxData = new byte[bpp * this.width * this.height];
                this.scan0 = this.data.Scan0;
                Marshal.Copy(this.scan0, this.pxData, 0, this.pxData.Length);
                this.locked = true;
            }
            catch(Exception e) {
                //TODO
                throw e;
            }
        }
        /*
         * Unlocks the BitmapData for the underlying Bitmap object.
         */
        public void Unlock()
        {
            try {
                Marshal.Copy(this.pxData, 0, this.scan0, this.pxData.Length);
                this.bitmap.UnlockBits(this.data);
            }
            catch (Exception e) {
                //TODO
                throw e;
            }
        }
        /*
         * Returns color info in a byte array of form [(ALPHA,) RED, GREEN, BLUE]
         * for the pixel found @ location (x,y).
         */
        public byte[] GetPixel(int x, int y) {
            byte[] color;
            int bpp = this.depth / 8;
            int startLoc = (y * this.width + x) * bpp;
            switch (this.depth) {
                case 32:
                    color = new byte[4];
                    color[0] = this.pxData[startLoc + 3];//Alpha
                    color[1] = this.pxData[startLoc + 2];//Red
                    color[2] = this.pxData[startLoc + 1];//Green
                    color[3] = this.pxData[startLoc];//Blue
                    return color;
                case 16:
                    color = new byte[3];
                    color[0] = this.pxData[startLoc + 2];//Red
                    color[1] = this.pxData[startLoc + 1];//Green
                    color[2] = this.pxData[startLoc];//Blue
                    return color;
                case 8:
                    color = new byte[3];
                    byte value = this.pxData[startLoc];//all the same on 8 bit
                    color[0] = value;
                    color[1] = value;
                    color[2] = value;
                    return color;
                default:
                    throw new Exception("invalid bitmap depth found in FastBitmap");
            }
        }

        void IDisposable.Dispose()
        {
            if (this.locked)
                this.Unlock();
            this.bitmap.Dispose();
        }
    }
}
