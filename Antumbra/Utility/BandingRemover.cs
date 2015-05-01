using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Antumbra.Glow.Observer.Bitmaps;

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
        public static Bitmap ReplaceBandingWithTransparent(Bitmap orig)
        {
            orig.MakeTransparent(Color.FromArgb(255, 0, 0, 0));
            return orig;
        }
        /*
         * Removes all banding from passed image,
         * however this function takes in the byte array of pixel data
         * found using the Bitmap.LockBits() method of Bitmap manipulation.
         */
        public static FastBitmap RemoveBanding(FastBitmap orig)
        {
            int top, bottom, left, right;
            try {
                top = FindTopBanding(orig);
                bottom = FindBottomBanding(orig);
                left = FindLeftBanding(orig);
                right = FindRightBanding(orig);
            }
            catch (Exception e) {
                throw e;
            }
            if (top == 0 && bottom == 0 && left == 0 && right == 0)//no cropping needed
                return orig;
            Console.WriteLine(top + " " + bottom + " " + left + " " + right);
            //else crop and return cropped
            int newWidth = orig.Width - left - right;
            int newHeight = orig.Height - top - bottom;
            Bitmap cropped = new Bitmap(newWidth, newHeight);
            orig.GetBitmap().MakeTransparent(Color.FromArgb(255,0,0,0));
            Console.WriteLine(orig.GetPixel(5, 5));
            int BPP = orig.Depth / 8;
            byte[] newBytes = new byte[newWidth * newHeight * BPP];
            for (int row = 0; row < orig.Height -1; row += 1) {
                for (int col = 0; col < (orig.Width - 1) * BPP; col += BPP) {
                    int origIndex = (left * orig.Data.Stride) + (row * orig.Data.Stride) + (top * BPP) + col;
                    int newIndex = row * newWidth * BPP + col;
                    for (int bit = 0; bit < BPP; bit += 1)
                        newBytes[newIndex + bit] = orig.PxData[origIndex + bit];
                }
            }
            Bitmap resultBm = new Bitmap(newWidth, newHeight);
            BitmapData resultData = resultBm.LockBits(new Rectangle(0, 0, newWidth, newHeight),
                ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            Marshal.Copy(newBytes, 0, resultData.Scan0, newBytes.Length);
            orig.Dispose();
            return new FastBitmap(resultBm);
        }

        public static bool IsBlack(Color color)
        {
            return color.R == 0 && color.G == 0 && color.B == 0;
        }

        public static int FindTopBanding(FastBitmap bm)
        {
            int blackRows = 0;
            for (int row = 0; row < bm.Height - 1; row += 1) {
                bool blackRow = true;
                for (int col = 0; col < bm.Width - 1; col += 1) {
                    Color color = bm.GetPixel(col, row);
                    if (!IsBlack(color))//not a black pixel
                        blackRow = false;
                }
                if (blackRow)
                    blackRows += 1;
                else
                    return blackRows;
            }
            return blackRows;
        }

        public static int FindLeftBanding(FastBitmap bm)
        {
            int blackCols = 0;
            for (int col = 0; col < bm.Width - 1; col += 1) {
                bool blackCol = true;
                for (int row = 0; row < bm.Height - 1; row += 1) {
                    Color color = bm.GetPixel(col, row);
                    if (!IsBlack(color))
                        blackCol = false;
                }
                if (blackCol)
                    blackCols += 1;
                else
                    return blackCols;
            }
            return blackCols;
        }

        public static int FindRightBanding(FastBitmap bm)
        {
            int blackCols = 0;
            for (int col = bm.Width - 1; col > 0; col -= 1) {//right to left
                bool blackCol = true;
                for (int row = 0; row < bm.Height - 1; row += 1) {
                    Color color = bm.GetPixel(col, row);
                    if (!IsBlack(color))
                        blackCol = false;
                }
                if (blackCol)
                    blackCols += 1;
                else
                    return blackCols;
            }
            return blackCols;
        }

        public static int FindBottomBanding(FastBitmap bm)
        {
            int blackRows = 0;
            for (int row = bm.Height - 1; row > 0; row -= 1) {//bottom to top
                bool blackRow = true;
                for (int col = 0; col < bm.Width - 1; col += 1) {
                    Color pixel = bm.GetPixel(col, row);
                    if (!IsBlack(pixel))//not a black pixel
                        blackRow = false;
                }
                if (blackRow)
                    blackRows += 1;
                else
                    return blackRows;
            }
            return blackRows;
        }
    }
}
