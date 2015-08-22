using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Antumbra.Glow.Utility {
    class FPSCalc //FPSCalculator class from the Afterglow project
    {
        Stopwatch clock = new Stopwatch();
        long frameCount;

        const int MAXSAMPLES = 100;
        int tickindex = 0;
        long ticksum = 0;
        long[] ticklist = new long[MAXSAMPLES];

        /* need to zero out the ticklist array before starting */
        /* average will ramp up until the buffer is full */
        /* returns average ticks per frame over the MAXSAMPPLES last frames */
        //http://stackoverflow.com/questions/87304/calculating-frames-per-second-in-a-game/87732#87732
        double CalcAverageTick(long newtick) {
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

        object lockObj = new object();
        double _fps = 0;
        double _frameTimeMS = 0;

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

        public void Tick() {
            frameCount++;
            var averageTick = CalcAverageTick(clock.ElapsedTicks) / Stopwatch.Frequency;
            lock(lockObj) {
                FPS = 1.0 / averageTick;
                FrameTimeInMilliseconds = averageTick * 1000.0;
            }
            clock.Restart();
        }
    }
}
