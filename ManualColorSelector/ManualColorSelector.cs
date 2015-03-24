using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antumbra.Glow.ExtensionFramework;
using Antumbra.Glow.ExtensionFramework.Types;
using Antumbra.Glow.Observer.Colors;
using Antumbra.Glow.Utility;
using System.ComponentModel.Composition;
using System.Windows.Forms;
using System.Reflection;

namespace ManualColorSelector
{
    [Export(typeof(GlowExtension))]
    public class ManualColorSelector : GlowIndependentDriver
    {
        private bool running;
        private MainForm picker;
        private Color16Bit lastUpdate;
        public delegate void NewColorAvail(Color16Bit newColor, EventArgs args);
        public event NewColorAvail NewColorAvailEvent;

        public override Guid id
        {
            get { return Guid.Parse("2305e75c-4b36-4a5c-9b03-0884e4361b4e"); }
        }

        public override void AttachColorObserver(AntumbraColorObserver observer)
        {
            this.NewColorAvailEvent += new NewColorAvail(observer.NewColorAvail);
        }
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
            get { return Assembly.GetExecutingAssembly().GetName().Version; }
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
            this.lastUpdate = null;
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
            SendColor(new Color16Bit(((MainForm)sender).sampleColor));
        }

        public void SendColor(Color16Bit newColor)
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
