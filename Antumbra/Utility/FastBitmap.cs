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
        public int height { get; private set; }
        public int width { get; private set; }
        public int depth { get; private set; }
        public BitmapData data { get; private set; }
        public byte[] pxData { get; private set; }
        public bool locked { get; private set; }
        private Bitmap bitmap;
        private IntPtr scan0;

        public FastBitmap(Bitmap bm)
        {
            this.bitmap = bm;
            this.width = this.bitmap.Width;
            this.height = this.bitmap.Height;
            PixelFormat pxFormat = this.bitmap.PixelFormat;
            this.depth = Bitmap.GetPixelFormatSize(pxFormat);
            this.locked = false;
        }

        public Bitmap GetBitmap()
        {
            return this.bitmap;
        }
        /*
         * Locks the underlying Bitmap object for faster manipulation of bits.
         */
        public void Lock()
        {
            if (this.locked)
                return;//already locked
            try {
                this.width = this.bitmap.Width;
                this.height = this.bitmap.Height;
                PixelFormat pxFormat = this.bitmap.PixelFormat;
                this.depth = Bitmap.GetPixelFormatSize(pxFormat);
                if (this.depth != 8 && this.depth != 24 && this.depth != 32) {
                    throw new ArgumentException("Only 8, 24 and 32 bpp images are supported.");
                }
                this.data = this.bitmap.LockBits(new Rectangle(0, 0, this.width, this.height), ImageLockMode.ReadOnly,
                    pxFormat);
                //if (BitConverter.IsLittleEndian)
                  //  this.pxData = (byte[])this.pxData.Reverse<byte>();
                this.pxData = new byte[this.depth / 8 * this.width * this.height];
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
                this.locked = false;
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
            int colorCompCount = this.depth / 8;
            int startLoc = ((y * (this.width-1)) + x) * colorCompCount;
            switch (this.depth) {
                case 32:
                    color = new byte[4];
                    if (BitConverter.IsLittleEndian) {//reverse order
                        color[0] = this.pxData[startLoc];//Alpha
                        color[1] = this.pxData[startLoc + 1];//Red
                        color[2] = this.pxData[startLoc + 2];//Green
                        color[3] = this.pxData[startLoc + 3];//Blue
                    }
                    else {
                        color[0] = this.pxData[startLoc + 3];//Alpha
                        color[1] = this.pxData[startLoc + 2];//Red
                        color[2] = this.pxData[startLoc + 1];//Green
                        color[3] = this.pxData[startLoc];//Blue
                    }
                    return color;
                case 16:
                    color = new byte[3];
                    if (BitConverter.IsLittleEndian) {
                        color[0] = this.pxData[startLoc];
                        color[1] = this.pxData[startLoc + 1];
                        color[2] = this.pxData[startLoc + 2];
                    }
                    else {
                        color[0] = this.pxData[startLoc + 2];//Red
                        color[1] = this.pxData[startLoc + 1];//Green
                        color[2] = this.pxData[startLoc];//Blue
                    }
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

        public void Dispose()
        {
            if (this.locked)
                this.Unlock();
            this.bitmap.Dispose();
        }
    }
}
