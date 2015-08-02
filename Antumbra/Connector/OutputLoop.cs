using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antumbra.Glow.Utility;
using Antumbra.Glow.ExtensionFramework;
using Antumbra.Glow.Observer.Colors;

namespace Antumbra.Glow.Connector
{
    public class OutputLoop : AntumbraColorObserver, IDisposable
    {
        private Task outputLoopTask;
        private FPSCalc outputFPS = new FPSCalc();
        private DeviceManager mgr;
        public int id { get; private set; }
        private Color16Bit color;
        private Color16Bit weightedAvg;
        private bool weightingEnabled;
        private double newColorWeight;
        private long index;

        public double FPS {
            get { return Math.Round(outputFPS.FPS, 2); }
        }
        /// <summary>
        /// Synchronisation object
        /// </summary>
        private object sync = new object();

        private bool _active = false;
        /// <summary>
        /// Setting this to false will stop the output thread
        /// </summary>
        /// <remarks>Thread Safe</remarks>
        public bool Active
        {
            get
            {
                lock (sync)
                    return _active;
            }
            set
            {
                lock (sync)
                    _active = value;
            }
        }
        public OutputLoop(DeviceManager mgr, int id)
        {
            this.id = id;
            this.mgr = mgr;
            this.index = long.MinValue;
        }

        public bool Start(bool weightEnabled, double newColorWeight)
        {
            this.outputFPS = new FPSCalc();
            this.weightingEnabled = weightEnabled;
            this.newColorWeight = newColorWeight;
            this.weightedAvg = new Color16Bit(0,0,0);
            this.color = null;
            this._active = true;
            this.outputLoopTask = new Task(target);
            this.outputLoopTask.Start();
            return true;
        }

        private void target()
        {
            while (Active) {
                if (color == null)//no values yet
                    continue;
                if (this.weightingEnabled) {
                    weightedAvg = Mixer.MixColorPercIn(color, weightedAvg, this.newColorWeight);
                    this.mgr.sendColor(weightedAvg, this.id);
                }
                else
                    this.mgr.sendColor(color, this.id);
            }
        }

        private void Stop()
        {
            this._active = false;
            if (this.outputLoopTask != null) {
                this.outputLoopTask.Wait();
                if (this.outputLoopTask.IsCompleted)
                    this.outputLoopTask.Dispose();
                else {
                    this.outputLoopTask.Wait(2000);
                    if (this.outputLoopTask.IsCompleted)
                        this.outputLoopTask.Dispose();
                }
            }
            this.index = long.MinValue;
        }

        public void Dispose()
        {
            this.Stop();
        }

        public void NewColorAvail(Color16Bit newColor, int id, long index)
        {
            if (this.id == id && this.index < index) {
                outputFPS.Tick();
                color = newColor;
            }
        }
    }
}
