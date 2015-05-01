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
            int newWidth = orig.width - left - right;
            int newHeight = orig.height - top - bottom;
            int BPP = orig.depth / 8;
            byte[] newBytes = new byte[newWidth * newHeight * BPP];
            for (int row = 0; row < orig.height; row += 1) {
                for (int col = 0; col < orig.width * BPP; col += BPP) {
                    int origIndex = (left * orig.data.Stride) + (row * orig.data.Stride) + (top * BPP) + col;
                    int newIndex = row * newWidth * BPP + col;
                    for (int bit = 0; bit < BPP; bit += 1)
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

        public static bool IsBlackPixel(byte[] pixelInfo)
        {
            int len = pixelInfo.Length;
            if (len == 4 || len == 3) {
                return (pixelInfo[len - 3] == 0 && pixelInfo[len - 2] == 0 && pixelInfo[len - 1] == 0);
            }
            else
                throw new ArgumentException("pixelInfo array not of size 3 or 4 as expected.");
        }

        public static int FindTopBanding(FastBitmap bm)
        {
            int blackRows = 0;
            for (int row = 0; row < bm.height - 1; row += 1) {
                bool blackRow = true;
                for (int col = 0; col < bm.width - 1; col += 1) {
                    byte[] pixel = bm.GetPixel(col, row);
                    int length = pixel.Length;
                    if (!IsBlackPixel(pixel))//not a black pixel
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
            for (int col = 0; col < bm.width - 1; col += 1) {
                bool blackCol = true;
                for (int row = 0; row < bm.height - 1; row += 1) {
                    byte[] pixel = bm.GetPixel(col, row);
                    int len = pixel.Length;
                    if (!IsBlackPixel(pixel))
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
            for (int col = bm.width - 1; col > 0; col -= 1) {//right to left
                bool blackCol = true;
                for (int row = 0; row < bm.height - 1; row += 1) {
                    byte[] pixel = bm.GetPixel(col, row);
                    int len = pixel.Length;
                    if (!IsBlackPixel(pixel))
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
            for (int row = bm.height - 1; row > 0; row -= 1) {//bottom to top
                bool blackRow = true;
                for (int col = 0; col < bm.width - 1; col += 1) {
                    byte[] pixel = bm.GetPixel(col, row);
                    int length = pixel.Length;
                    if (!IsBlackPixel(pixel))//not a black pixel
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
