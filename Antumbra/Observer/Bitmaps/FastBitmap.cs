using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Antumbra.Glow.Observer.Bitmaps
{
    public class FastBitmap : IDisposable
    {
        public int Height { get; private set; }
        public int Width { get; private set; }
        public int Depth { get; private set; }
        public BitmapData Data { get; private set; }
        public byte[] PxData { get; private set; }
        public bool locked { get; private set; }
        public PixelFormat PxFormat { get; private set; }
        private Bitmap bitmapCopy, bitmap;
        private IntPtr scan0;
        public Object sync = new Object();

        public FastBitmap(Bitmap bm)
        {
            this.bitmap = bm;
            this.bitmapCopy = (Bitmap)bm.Clone();
            this.Width = this.bitmap.Width;
            this.Height = this.bitmap.Height;
            this.PxFormat = this.bitmap.PixelFormat;
            this.Depth = Bitmap.GetPixelFormatSize(PxFormat);
            this.locked = false;
        }

        public Bitmap GetBitmap()
        {
            return this.bitmapCopy;
        }
        /*
         * Locks the underlying Bitmap object for faster manipulation of bits.
         */
        public void Lock()
        {
            if (this.locked)
                return;//already locked
            try {
                if (this.Depth != 8 && this.Depth != 24 && this.Depth != 32) {
                    throw new ArgumentException("Only 8, 24 and 32 bpp images are supported.");
                }
                try {
                    lock (this.sync) {
                        this.Data = this.bitmap.LockBits(new Rectangle(0, 0, this.Width - 1, this.Height - 1),
                            ImageLockMode.ReadWrite, this.PxFormat);
                    }
                }
                catch (System.ArgumentException) {
                    //already locked
                    return;
                }
                this.PxData = new byte[(this.Depth / 8) * this.Width * this.Height];
                this.scan0 = this.Data.Scan0;
                Marshal.Copy(this.scan0, this.PxData, 0, this.PxData.Length-1);
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
                Marshal.Copy(this.PxData, 0, this.scan0, this.PxData.Length);
                this.bitmap.UnlockBits(this.Data);
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
        public Color GetPixel(int x, int y) {
            byte red, green, blue;
            int alpha = -1;
            int colorCompCount = this.Depth / 8;
            int startLoc = y * this.Data.Stride + x * colorCompCount;
            switch (this.Depth) {
                case 32:
                    if (BitConverter.IsLittleEndian) {//reverse order
                        alpha = this.PxData[startLoc + 3];//Alpha
                        red = this.PxData[startLoc + 2];//Red
                        green = this.PxData[startLoc + 1];//Green
                        blue = this.PxData[startLoc];//Blue
                    }
                    else {
                        alpha = this.PxData[startLoc];//Alpha
                        red = this.PxData[startLoc + 1];//Red
                        green = this.PxData[startLoc + 2];//Green
                        blue = this.PxData[startLoc + 3];//Blue
                    }
                    break;
                case 16:
                    if (BitConverter.IsLittleEndian) {
                        red = this.PxData[startLoc + 2];//Red
                        green = this.PxData[startLoc + 1];//Green
                        blue = this.PxData[startLoc];//Blue
                    }
                    else {
                        red = this.PxData[startLoc];
                        green = this.PxData[startLoc + 1];
                        blue = this.PxData[startLoc + 2];
                    }
                    break;
                case 8:
                    byte value = this.PxData[startLoc];//all the same on 8 bit
                    red = value;
                    green = value;
                    blue = value;
                    break;
                default:
                    throw new Exception("invalid bitmap depth found in FastBitmap");
            }
            if (alpha == -1)
                return Color.FromArgb(red, green, blue);
            return Color.FromArgb(alpha, red, green, blue);
        }

        public void Dispose()
        {
            if (this.locked)
                this.Unlock();
            this.bitmap.Dispose();
        }
    }
}
