using System.Diagnostics;

namespace Antumbra.Glow.Utility {

    internal class FPSCalc //FPSCalculator class from the Afterglow project
    {
        #region Private Fields

        private const int MAXSAMPLES = 100;
        private double _fps = 0;
        private double _frameTimeMS = 0;
        private Stopwatch clock = new Stopwatch();
        private long frameCount;
        private object lockObj = new object();
        private int tickindex = 0;
        private long[] ticklist = new long[MAXSAMPLES];
        private long ticksum = 0;

        #endregion Private Fields

        /* need to zero out the ticklist array before starting */
        /* average will ramp up until the buffer is full */
        /* returns average ticks per frame over the MAXSAMPPLES last frames */

        #region Public Properties

        public double FPS {
            get {
                lock(lockObj)
                    return _fps;
            }
            private set {
                _fps = value;
            }
        }

        public double FrameTimeInMilliseconds {
            get {
                lock(lockObj)
                    return _frameTimeMS;
            }
            private set {
                _frameTimeMS = value;
            }
        }

        #endregion Public Properties

        #region Public Methods

        public void Tick() {
            frameCount++;
            var averageTick = CalcAverageTick(clock.ElapsedTicks) / Stopwatch.Frequency;
            lock(lockObj) {
                FPS = 1.0 / averageTick;
                FrameTimeInMilliseconds = averageTick * 1000.0;
            }
            clock.Restart();
        }

        #endregion Public Methods

        #region Private Methods

        //http://stackoverflow.com/questions/87304/calculating-frames-per-second-in-a-game/87732#87732
        private double CalcAverageTick(long newtick) {
            ticksum -= ticklist[tickindex];  /* subtract value falling off */
            ticksum += newtick;              /* add new value */
            ticklist[tickindex] = newtick;   /* save new value so it can be subtracted later */
            if(++tickindex == MAXSAMPLES)    /* inc buffer index */
                tickindex = 0;

            /* return average */
            if(frameCount < MAXSAMPLES) {
                return (double)ticksum / frameCount;
            } else {
                return (double)ticksum / MAXSAMPLES;
            }
        }

        #endregion Private Methods
    }
}
