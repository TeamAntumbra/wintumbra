using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace Antumbra.Glow.Utility
{
    /// <summary>
    /// Provides fast access to the memory of a bitmap object
    /// Using an <strong>unsafe</strong> class, unsafe needs to be allowed in the project for this to work
    /// </summary>
    public unsafe class FastBitmap : IDisposable
    {
        public struct PixelData
        {
            public byte Blue;
            public byte Green;
            public byte Red;
        }

        readonly Bitmap _subject;
        int _subjectHeight;
        int _subjectWidth;
        BitmapData _bitmapData;
        Byte* _pBase = null;
        bool _locked;
        public FastBitmap(Bitmap subjectBitmap)
        {
            System.Diagnostics.Debug.Assert(subjectBitmap != null, "bitmap must not be null");
            this._subject = subjectBitmap;
            LockBitmap();
        }

        public void AcquireLock()
        {
            LockBitmap();
        }

        public void ReleaseLock()
        {
            UnlockBitmap();
        }

        public Bitmap Bitmap
        {
            get
            {
                ReleaseLock();
                return _subject;
            }
        }

        public void SetPixel(int x, int y, Color colour)
        {
            PixelData* p = PixelAt(x, y);
            p->Red = colour.R;
            p->Green = colour.G;
            p->Blue = colour.B;
        }

        /// <summary>
        /// Retrieve a single pixel from the provided point
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public Color GetPixel(Point p)
        {
            return GetPixel(p.X, p.Y);
        }

        /// <summary>
        /// Retrieve a single pixel at the provided x and y coordinates
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Color GetPixel(int x, int y)
        {
            PixelData* p = PixelAt(x, y);
            return Color.FromArgb(p->Red, p->Green, p->Blue);
        }

        /// <summary>
        /// Gets an array of pixels located on the same scan line, depending on the number of lines and width, this can be up to two times faster than retrieving pixels one at a time
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public Color[] GetPixels(int x, int y, int width)
        {
            if (x * sizeof(PixelData) + width * sizeof(PixelData) > _subjectWidth)
                throw new ArgumentOutOfRangeException("width", "x + width extends past the right edge of the image");

            return PixelsAt(x, y, width);
        }

        /// <summary>
        /// Retrieves a region of the bitmap as a single request (faster than loading a single pixel at a time)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public Color[][] GetPixels(int x, int y, int width, int height)
        {
            if (x * sizeof(PixelData) + width * sizeof(PixelData) > _subjectWidth)
                throw new ArgumentOutOfRangeException("width", "x + width extends past the right edge of the image");
            if (y + height > _subjectHeight)
                throw new ArgumentOutOfRangeException("height", "y + height extends past the bottom of the image");

            return PixelsAt(x, y, width, height);
        }

        /// <summary>
        /// Allows conditionally reading pixels from a region based on the next pixel count. 
        /// For only retrieving every Nth pixel, it is better to use GetPixels(x, y, width, height, step).
        /// </summary>
        /// <param name="x">Start X</param>
        /// <param name="y">Start Y</param>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <param name="includePixel">A function that returns true if the pixel (1 based) should be included</param>
        /// <returns>A flat list of pixel colors</returns>
       /* public List<Color> GetPixels(int x, int y, int width, int height, Func<int, bool> includePixel)
        {
            if (x * sizeof(PixelData) + width * sizeof(PixelData) > _subjectWidth)
                throw new ArgumentOutOfRangeException("width", "x + width extends past the right edge of the image");
            if (y + height > _subjectHeight)
                throw new ArgumentOutOfRangeException("height", "y + height extends past the bottom of the image");

            return PixelsAt(x, y, width, height, includePixel);
        }*/

        /// <summary>
        /// Fastest method to retrieve every Nth pixel from a region of the bitmap
        /// </summary>
        /// <param name="x">Start X</param>
        /// <param name="y">Start Y</param>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <param name="step">The step (1 or larger)</param>
        /// <returns>A flat list of pixel colors</returns>
        public List<Color> GetPixels(int x, int y, int width, int height, int step)
        {
            try {
                if (x * sizeof(PixelData) + width * sizeof(PixelData) > _subjectWidth)
                    throw new ArgumentOutOfRangeException("width", "x + width extends past the right edge of the image");
                if (y + height > _subjectHeight)
                    throw new ArgumentOutOfRangeException("height", "y + height extends past the bottom of the image");
                if (step < 1 || step > width * height)
                    throw new ArgumentOutOfRangeException("step", step, "step must be larger than 0 and less than width * height");
                return PixelsAt(x, y, width, height, step);
            }
            catch (Exception) {
                return new List<Color>();
            }
        }


        private void LockBitmap()
        {
            if (!_locked) {
                GraphicsUnit unit = GraphicsUnit.Pixel;
                RectangleF boundsF = _subject.GetBounds(ref unit);
                Rectangle bounds = new Rectangle((int)boundsF.X,
                    (int)boundsF.Y,
                    (int)boundsF.Width,
                    (int)boundsF.Height);

                _subjectHeight = bounds.Height;
                _subjectWidth = (int)boundsF.Width * sizeof(PixelData);
                if (_subjectWidth % 4 != 0) {
                    _subjectWidth = 4 * (_subjectWidth / 4 + 1);
                }

                _bitmapData = _subject.LockBits(bounds, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                _pBase = (Byte*)_bitmapData.Scan0.ToPointer();

                _locked = true;
            }
        }

        private PixelData* PixelAt(int x, int y)
        {
            return (PixelData*)(_pBase + y * _subjectWidth + x * sizeof(PixelData));
        }

        private Color[] PixelsAt(int x, int y, int width)
        {
            Color[] pixels = new Color[width];

            int index = y * _subjectWidth + x * sizeof(PixelData);
            for (int i = 0; i < width; i++) {
                var pixel = (PixelData*)(_pBase + index);
                pixels[i] = Color.FromArgb(pixel->Red, pixel->Green, pixel->Blue);
                index += sizeof(PixelData);
            }

            return pixels;
        }

        private Color[][] PixelsAt(int x, int y, int width, int height)
        {
            Color[][] pixels = new Color[height][];

            int yPos = y * _subjectWidth;
            int startXPos = x * sizeof(PixelData);

            int index = yPos + startXPos;
            for (int i = 0; i < height; i++) {
                pixels[i] = new Color[width];
                for (int j = 0; j < width; j++) {
                    var pixel = (PixelData*)(_pBase + index);
                    pixels[i][j] = Color.FromArgb(pixel->Red, pixel->Green, pixel->Blue);
                    index += sizeof(PixelData);
                }
                yPos += _subjectWidth;
                index = yPos + startXPos;
            }

            return pixels;
        }

        /*private List<Color> PixelsAt(int x, int y, int width, int height, Func<int, bool> includePixel)
        {
            int pixelCount = 0;

            int yPos = y * _subjectWidth;
            int startXPos = x * sizeof(PixelData);
            List<Color> pixels = new List<Color>();
            int index = yPos + startXPos;
            for (int i = 0; i < height; i++) {
                for (int j = 0; j < width; j++) {
                    pixelCount++;
                    if (includePixel(pixelCount)) {
                        PixelData* pixel = (PixelData*)(_pBase + index);
                        pixels.Add(Color.FromArgb(pixel->Red, pixel->Green, pixel->Blue));
                    }
                    index += sizeof(PixelData);

                }
                yPos += _subjectWidth;
                index = yPos + startXPos;
            }
            return pixels;
        }*/

        private List<Color> PixelsAt(int x, int y, int width, int height, int step)
        {
            int pixelCount = 0;

            int yPos = y * _subjectWidth;
            int startXPos = x * sizeof(PixelData);
            List<Color> pixels = new List<Color>(width * height / step);
            int index = yPos + startXPos;
            for (int i = 0; i < height; i++) {
                for (int j = 0; j < width; j++) {
                    pixelCount++;
                    if (pixelCount == step) {
                        pixelCount = 0;
                        PixelData* pixel = (PixelData*)(_pBase + index);
                        pixels.Add(Color.FromArgb(pixel->Red, pixel->Green, pixel->Blue));
                    }
                    index += sizeof(PixelData);

                }
                yPos += _subjectWidth;
                index = yPos + startXPos;
            }
            return pixels;
        }

        private void UnlockBitmap()
        {
            if (_locked) {
                _subject.UnlockBits(_bitmapData);
                _bitmapData = null;
                _pBase = null;

                _locked = false;
            }
        }

        private static int ColorVariationAverage(int a, int b, int c)
        {
            return Math.Abs((a + b + c) / 3);
        }

        /// <summary>
        /// Determines if two pixels match
        /// </summary>
        /// <param name="c1">Color structure representing pixel A</param>
        /// <param name="c2">Color structure representing pixel B</param>
        /// <param name="threshold">The maximum deviation between the two values</param>
        /// <returns>True if the two pixels are within the threshold</returns>
        /// <remarks>
        /// The threshold represents the average difference between the individual color components.
        /// (Abs(R1-R2)+Abs(G1-G2)+Abs(B1-B2))/3 = deviation
        /// </remarks>
        public static bool PixelIsSimilar(Color c1, Color c2, int threshold)
        {
            if (threshold == 0) {
                return c1.Equals(c2);
            }

            // c1 to c2 diff
            Color c1VSc2 = Color.FromArgb(
                Math.Abs(c1.R - c2.R),
                Math.Abs(c1.G - c2.G),
                Math.Abs(c1.B - c2.B)
                );

            int c1VSc2Avg = ColorVariationAverage(c1VSc2.R, c1VSc2.G, c1VSc2.B);
            return c1VSc2Avg <= threshold;
        }

        public Point FindBitmap(Bitmap source, int threshold)
        {
            return FindBitmap(source, new Rectangle(0, 0, 0, 0), threshold);
        }

        public Point FindBitmap(Bitmap source, int threshold, Color transparency)
        {
            return FindBitmap(source, new Rectangle(0, 0, 0, 0), threshold, true, transparency);
        }

        /// <summary>
        /// Attempt to find a bitmap within this Bitmap
        /// </summary>
        /// <param name="source">The bitmap to find within this bitmap</param>
        /// <param name="constraint">The rectangle to search within</param>
        /// <param name="threshold">The pixel deviation theshold</param>
        /// <returns>If source is found, the result is the top left coordinates of the match, otherwise a location of -1,-1 is returned.</returns>
        public Point FindBitmap(Bitmap source, Rectangle constraint, int threshold)
        {
            return FindBitmap(source, constraint, threshold, false, Color.Black);
        }

        /// <summary>
        /// Attempt to find a bitmap within this Bitmap
        /// </summary>
        /// <param name="source">The bitmap to find within this bitmap</param>
        /// <param name="constraint">The rectangle to search within</param>
        /// <param name="threshold">The pixel deviation theshold</param>
        /// <param name="useTransparency">Use a transparency color (when found within source image the pixel is ignored)</param>
        /// <param name="transparentColour">The transparent color to match transparency against</param>
        /// <returns></returns>
        public Point FindBitmap(Bitmap source, Rectangle constraint, int threshold, bool useTransparency, Color transparentColour)
        {
            int height = source.Height;
            int width = source.Width;

            using (FastBitmap fbSrc = new FastBitmap(source)) {
                // This ensures that the colour will match in the == operator.
                transparentColour = Color.FromArgb(transparentColour.R, transparentColour.G, transparentColour.B);

                // Initialise constraint
                // Ensure that the constraint fits within the destination image
                if (constraint.Right > this._bitmapData.Width || constraint.Right == 0) {
                    constraint.Width = this._bitmapData.Width - constraint.X;
                }

                if (constraint.Bottom > this._bitmapData.Height || constraint.Bottom == 0) {
                    constraint.Height = this._bitmapData.Height - constraint.Y;
                }

                // If there isn't enough room for the source image left, then just return not found
                // This fixes a memory access violation under certain circumstances (e.g. counting belts in Teon [more than 27])
                if (constraint.Height < fbSrc.Bitmap.Height || constraint.Width < fbSrc.Bitmap.Width) {
                    return new Point(-1, -1);
                }

                Point matchStart = new Point(-1, -1);
                Point curSrc = new Point(0, 0);
                Point curDest = new Point(constraint.X, constraint.Y);

                while (true) {
                    Color cDest = GetPixel(curDest);
                    Color cSrc = fbSrc.GetPixel(curSrc);

                    if ((useTransparency && cSrc == transparentColour) || PixelIsSimilar(cDest, cSrc, threshold)) {
                        if (curSrc.X == 0 && curSrc.Y == 0) {
                            matchStart.X = curDest.X;
                            matchStart.Y = curDest.Y;
                        }

                        // Have we found the complete image?
                        if (curSrc.X == width - 1 && curSrc.Y == height - 1) {
                            return matchStart;
                        }
                        // Have we finished a full line of matching with the src?
                        if (curSrc.X == width - 1) {
                            curSrc.X = 0;
                            curSrc.Y++;

                            curDest.X = matchStart.X;

                            if (curDest.Y == constraint.Bottom - 1) {
                                return new Point(-1, -1);
                            }

                            curDest.Y++;
                        }
                        // Continue checking line
                        else {
                            // If we have reached the end of dest line - restart the match
                            if (curDest.X == constraint.Right - 1) {
                                matchStart.X = -1;
                                matchStart.Y = -1;
                                curDest.X = constraint.X;
                                curSrc.X = 0;
                                curSrc.Y = 0;

                                if (curDest.Y == constraint.Bottom - 1) {
                                    // Reached end of dest image
                                    return new Point(-1, -1);
                                }

                                // Move to next line
                                curDest.Y++;
                            }
                            else {
                                // Check next pixel
                                curDest.X++;
                                curSrc.X++;
                            }

                        }
                    }
                    else {
                        curSrc.X = 0;
                        curSrc.Y = 0;

                        if (matchStart.X != -1) {
                            curDest.X = matchStart.X;
                            curDest.Y = matchStart.Y;
                        }
                        matchStart.X = -1;
                        matchStart.Y = -1;

                        // If we have reached the end of dest line - restart the match
                        if (curDest.X == constraint.Right - 1) {
                            matchStart.X = -1;
                            matchStart.Y = -1;
                            curDest.X = constraint.X;

                            if (curDest.Y == constraint.Bottom - 1) {
                                // Reached end of dest image
                                return new Point(-1, -1);
                            }

                            // Move to next line
                            curDest.Y++;
                        }
                        else {
                            // Check next pixel
                            curDest.X++;
                        }
                    }
                }
            }
        }

        #region IDisposable Members

        private bool _disposed;
        public void Dispose()
        {
            if (!_disposed) {
                _disposed = true;
                ReleaseLock();
            }
        }

        #endregion
    }
}
