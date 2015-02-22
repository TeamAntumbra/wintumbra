using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antumbra.Glow.ExtensionFramework;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Windows.Forms;

namespace ManualColorSelector
{
    [Export(typeof(GlowExtension))]
    public class ManualColorSelector : GlowIndependentDriver
    {
        private bool running;
        private MainForm picker;
        private Color lastUpdate;
        public delegate void NewColorAvail(Color newColor, EventArgs args);
        public event NewColorAvail NewColorAvailEvent;
        public override void AttachEvent(AntumbraColorObserver observer)
        {
            this.NewColorAvailEvent += new NewColorAvail(observer.NewColorAvail);
        }
        public override int id { get; set; }
        public override string Name
        {
            get { return "Manual Color Selector"; }
        }

        public override string Author
        {
            get { return "Team Antumbra"; }
        }

        public override string Description
        {
            get { return "A manual color selector for Glow"; }
        }

        public override bool IsDefault
        {
            get { return false; }
        }

        public override Version Version
        {
            get { return new Version("0.0.1"); }
        }

        public override string Website
        {
            get { throw new NotImplementedException(); }
        }

        public override bool Settings()
        {
            return false;
        }

        public override bool IsRunning
        {
            get { return this.running; }
        }

        public override bool Start()
        {
            this.lastUpdate = Color.Empty;
            this.picker = new MainForm();
            this.picker.BackColorChanged += new EventHandler(SendColorEvent);
            this.picker.Show();
            this.running = true;
            return true;
        }

        public void SendColorEvent(Object sender, EventArgs args)
        {
            if (sender == null)//invalid
                return;
            SendColor(((MainForm)sender).sampleColor);
        }

        public void SendColor(Color newColor)
        {
            NewColorAvailEvent(newColor, EventArgs.Empty);
        }

        public override bool Stop()
        {
            bool result = true;//success?
            if (this.picker != null)
                this.picker.Close();
            this.running = false;
            return result;
        }

        public override void RecmmndCoreSettings()
        {
            this.stepSleep = 0;
        }
    }
}
