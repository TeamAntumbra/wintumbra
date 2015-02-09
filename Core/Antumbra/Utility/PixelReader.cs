using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Antumbra.Glow.Utility
{
    public class PixelReader : IDisposable
    {
        // TODO: implement bitmap/fastbitmap support
        private readonly FastBitmap _fastBitmap;
        private readonly Rectangle _region;

        public PixelReader(FastBitmap fastBitmap, Rectangle region)
        {
            if (fastBitmap == null)
                throw new ArgumentOutOfRangeException("fastBitmap", "must not be null");
            _fastBitmap = fastBitmap;
            _region = region;
        }

        public PixelReader(FastBitmap fastBitmap)
            : this(fastBitmap, new Rectangle(0, 0, fastBitmap.Bitmap.Width, fastBitmap.Bitmap.Height))
        {

        }

        private readonly bool _ownsFastBitmap;
        public PixelReader(Bitmap bitmap)
            : this(new FastBitmap(bitmap))
        {
            _ownsFastBitmap = true;
        }

        private Color[][] _pixels;
        public PixelReader(Color[][] pixels)
        {
            if (pixels == null)
                throw new ArgumentOutOfRangeException("pixels", "must not be null");
            _pixels = pixels;
        }

        /// <summary>
        /// Pixels [rows][columns]
        /// </summary>
        public Color[][] Pixels
        {
            get
            {
                if (_pixels == null) {
                    _pixels = _fastBitmap.GetPixels(_region.X, _region.Y, _region.Width, _region.Height);
                }
                return _pixels;
            }
        }

        /// <summary>
        /// List of every Nth pixel
        /// </summary>
        /// <param name="n">A number greater than 0</param>
        /// <returns>List of pixel colours</returns>
        public List<Color> GetEveryNthPixel(int n)
        {
            if (_pixels == null) {
                return _fastBitmap.GetPixels(_region.X, _region.Y, _region.Width, _region.Height, n);
            }
            else {
                if (n < 1)
                    throw new ArgumentOutOfRangeException("n", n, "Must be greater than 0");
                int counter = 1; // Keep track of when to return a pixel
                List<Color> colors = new List<Color>(_region.Width * _region.Height / n);
                foreach (Color[] row in Pixels) {
                    for (int j = 0; j < row.Length; j++) {
                        if (counter == n) {
                            counter = 1;
                            colors.Add(row[j]);
                        }
                        else {
                            counter++;
                        }
                    }
                }
                return colors;
            }
        }

        #region IDisposable

        private bool _disposed;
        public void Dispose()
        {
            if (!_disposed) {
                _disposed = true;
                if (_ownsFastBitmap)
                    _fastBitmap.Dispose();
            }
        }

        #endregion
    }
}
