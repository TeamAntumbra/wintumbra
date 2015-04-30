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
    /*
     * A BandingRemover is an object which can detect and remove
     * black bands from the permimeter of a Bitmap. This is commonly
     * found when watching content which was not designed for the
     * aspect ratio of a user's display.
     */
    public static class BandingRemover
    {
        /*
         * Same as previous method, removes all banding from passed image,
         * however this function takes in the byte array of pixel data
         * found using the Bitmap.LockBits() method of Bitmap manipulation.
         */
        public static FastBitmap RemoveBanding(FastBitmap orig)
        {
            int top = -1, bottom = -1, left = -1, right = -1;//amount of rows/cols to remove in each dir
            bool topBuilding = false;//done building the top bar yet?
            for (int row = 0; row < orig.height; row += 1) {//for each row
                bool blackRow = true;
                int leftBlack = 0;
                bool leftBuilding = true;
                int rightBlack = 0;
                for (int col = 0; col < orig.width; col += 1) {//for each column
                    byte[] pixel = orig.GetPixel(row, col);//pixel info
                    int length = pixel.Length;
                    if (pixel[length - 3] == 0 && pixel[length - 2] == 0 && pixel[length -1] == 0) {//black pixel
                        if (leftBuilding)//left side black border still being built
                            left += 1;
                        rightBlack += 1;
                    }
                    else {
                        topBuilding = false;
                        blackRow = false;
                        leftBuilding = false;
                        rightBlack = 0;
                    }
                }
                if (blackRow) {
                    if (topBuilding)
                        top += 1;
                    bottom += 1;
                }
                else
                    bottom = 0;
                if (left == -1)//has yet to be set
                    left = leftBlack;
                else
                    left = Math.Min(left, leftBlack);
            }
            if (top <= 0 && bottom <= 0 && left <= 0 && right <= 0)//no change
                return orig;
            //else crop and return cropped
            int newWidth = orig.width - left - right;
            int newHeight = orig.height - top - bottom;
            byte[] newBytes = new byte[newWidth * newHeight * orig.BPP];
            for (int row = 0; row < orig.height; row += 1) {
                for (int col = 0; col < orig.width * orig.BPP; col += orig.BPP) {
                    int origIndex = (left * orig.data.Stride) + (row * orig.data.Stride) + (top * orig.BPP) + col;
                    int newIndex = row * newWidth * orig.BPP + col;
                    for (int bit = 0; bit < orig.BPP; bit += 1)
                        newBytes[newIndex + bit] = orig.pxData[origIndex + bit];
                }
            }
            Bitmap resultBm = new Bitmap(newWidth, newHeight);
            BitmapData resultData = resultBm.LockBits(new Rectangle(0, 0, newWidth, newHeight),
                ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            Marshal.Copy(newBytes, 0, resultData.Scan0, newBytes.Length);
            orig.Dispose();
            return new FastBitmap(resultBm);
        }
    }
}
