using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.InteropServices;
using System.ComponentModel.Composition;
using System.Drawing;
using Antumbra.Glow.ExtensionFramework;
using Antumbra.Glow.ExtensionFramework.Types;
using Antumbra.Glow.Observer.Colors;
using Antumbra.Glow.Utility;

namespace NeonFade
{
    [Export(typeof(GlowExtension))]
    public class NeonFade : GlowIndependentDriver
    {
        public delegate void NewColorAvail(Color16Bit color, int id, long index);
        public event NewColorAvail NewColorAvailEvent;

        private bool running;
        private int deviceId;
        private long index;
        private Task driver;

        public override bool IsRunning
        {
            get { return running; }
        }

        public override Guid id
        {
            get
            {
                return Guid.Parse("9a310fae-2084-4dc5-ae6a-4f664faa1fe8");
            }
        }

        public override int devId
        {
            get
            {
                return deviceId;
            }
            set
            {
                deviceId = value;
            }
        }

        public override string Name
        {
            get { return "Neon Fade"; }
        }

        public override string Author
        {
            get { return "Team Antumbra"; }
        }

        public override string Description
        {
            get { return "A fade through hand-picked neon colors."; }
        }

        public override Version Version
        {
            get { return new Version(0,1,1); }
        }

        public override bool Settings()
        {
            return false;
        }

        public override bool IsDefault
        {
            get { return false; }
        }

        public override string Website
        {
            get { throw new NotImplementedException(); }
        }

        public override void AttachColorObserver(Antumbra.Glow.Observer.Colors.AntumbraColorObserver observer)
        {
            NewColorAvailEvent += observer.NewColorAvail;
        }

        public override GlowDriver Create()
        {
            return new NeonFade();
        }

        public override bool Start()
        {
            index = long.MinValue;
            running = true;
            driver = new Task(Target);
            driver.Start();
            return true;
        }

        public override bool Stop()
        {
            running = false;
            if (driver != null) {
                if (driver.IsCompleted)
                    driver.Dispose();
                else {
                    driver.Wait(3000);
                    if (driver.IsCompleted)
                        driver.Dispose();
                    else
                        return false;
                }
            }
            return true;
        }

        public override void Dispose()
        {
            if (driver != null) {
                driver.Dispose();
            }
        }

        private void Target()
        {
            List<Color16Bit> colors = new List<Color16Bit>();
            colors.Add(Color16BitUtil.FromRGBColor(Color.Red));
            colors.Add(Color16BitUtil.FromRGBColor(Color.Teal));
            colors.Add(Color16BitUtil.FromRGBColor(Color.Blue));
            colors.Add(Color16BitUtil.FromRGBColor(Color.Yellow));
            colors.Add(Color16BitUtil.FromRGBColor(Color.Green));
            colors.Add(Color16BitUtil.FromRGBColor(Color.Purple));
            int index = 0;
            Color16Bit prev = new Color16Bit();
            while (running) {
                Color16Bit newColor = colors[index];
                FadeFromTo(prev, newColor);
                prev = newColor;
                index += 1;
                if (index == colors.Count)
                    index = 0;//wrap around
            }
        }

        private void SendColor(Color16Bit newColor)
        {
            if (NewColorAvailEvent != null)
                NewColorAvailEvent(newColor, deviceId, index++);
        }

        private void FadeFromTo(Color16Bit col1, Color16Bit col2)
        {
            for (double frac = 0; frac <= 1; frac += .001) {
                if (!running)
                    return;//cancel fade, we've been stopped
                Color16Bit newColor = Mixer.MixColorPercIn(col2, col1, frac);
                SendColor(newColor);
                Thread.Sleep(this.stepSleep);
            }
        }

        public override void RecmmndCoreSettings()
        {
            stepSleep = 200;
        }
    }
}
