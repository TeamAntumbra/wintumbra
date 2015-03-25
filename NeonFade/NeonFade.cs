﻿using System;
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
        public delegate void NewColorAvail(Color16Bit color);
        public event NewColorAvail NewColorAvailEvent;
        private bool running;
        private Task driver;
        public override bool IsRunning
        {
            get { return this.running; }
        }
        public override Guid id
        {
            get
            {
                return Guid.Parse("9a310fae-2084-4dc5-ae6a-4f664faa1fe8");
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
            this.NewColorAvailEvent += observer.NewColorAvail;
        }

        public override bool Start()
        {
            this.running = true;
            this.driver = new Task(Target);
            this.driver.Start();
            return true;
        }

        public override bool Stop()
        {
            this.running = false;
            if (this.driver != null) {
                if (this.driver.IsCompleted)
                    this.driver.Dispose();
                else {
                    this.driver.Wait(3000);
                    if (this.driver.IsCompleted)
                        this.driver.Dispose();
                    else
                        return false;
                }
            }
            return true;
        }

        private void Target()
        {
            List<Color> colors = new List<Color>();
            colors.Add(Color.Red);
            colors.Add(Color.Teal);
            colors.Add(Color.Blue);
            colors.Add(Color.Yellow);
            colors.Add(Color.Green);
            colors.Add(Color.Purple);
            int index = 0;
            Color prev = Color.Black;
            while (running) {
                Color newColor = colors[index];
                FadeFromTo(new Color16Bit(prev), new Color16Bit(newColor));
                prev = newColor;
                index += 1;
                if (index == colors.Count)
                    index = 0;//wrap around
            }
        }

        private void SendColor(Color16Bit newColor)
        {
            if (NewColorAvailEvent != null)
                NewColorAvailEvent(newColor);
        }

        private void FadeFromTo(Color16Bit col1, Color16Bit col2)
        {
            for (int i = 1; i <= 100; i += 1) {
                double frac = i / 100.0;
                Color16Bit newColor = Mixer.MixColorPercIn(col2, col1, frac);
                SendColor(newColor);
                Thread.Sleep(this.stepSleep);
            }
        }

        public override void RecmmndCoreSettings()
        {
            this.stepSleep = 200;
        }
    }
}