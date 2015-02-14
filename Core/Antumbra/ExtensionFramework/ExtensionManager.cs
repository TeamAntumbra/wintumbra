using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antumbra.Glow.Settings;
using System.Drawing;

namespace Antumbra.Glow.ExtensionFramework
{
    /// <summary>
    /// Manages (through use of MEFHelper) the Extensions for use with Glow(s)
    /// </summary>
    public class ExtensionManager : AntumbraColorObserver//TODO add observer for notifiers
    {
        public delegate void NewColorAvail(Color newColor, EventArgs args);
        public event NewColorAvail NewColorAvailEvent;
        /// <summary>
        /// MEFHelper used to find and load the extensions.
        /// </summary>
        private MEFHelper MEFHelper;
        private DeviceSettings settings;
        public int id { get; private set; }
        public GlowDriver ActiveDriver { get; set; }
        public GlowScreenGrabber ActiveGrabber { get; set; }
        public GlowScreenProcessor ActiveProcessor { get; set; }
        public List<GlowDecorator> ActiveDecorators { get; set; }
        public List<GlowNotifier> ActiveNotifiers { get; set; }
        public ExtensionManager(string path, int id, DeviceSettings settings)
        {
            this.settings = settings;
            this.id = id;
            this.MEFHelper = new MEFHelper(path);
            this.ActiveDriver = this.MEFHelper.GetDefaultDriver();
            this.ActiveGrabber = this.MEFHelper.GetDefaultGrabber();
            this.ActiveProcessor = this.MEFHelper.GetDefaultProcessor();
            this.ActiveDecorators = this.MEFHelper.GetDefaultDecorators();
            this.ActiveNotifiers = this.MEFHelper.GetDefaultNotifiers();
        }

        public string GetSetupDesc()
        {
            if (this.ActiveDriver == null)
                return "The current driver is null. Extension loading probably failed.";
            string result = "Driver: ";
            if (ActiveDriver is GlowScreenDriverCoupler)
                result += ActiveDriver.ToString()
                    + "\nGrabber: " + ActiveGrabber.ToString() + "\nProcessor: "
                    + ActiveProcessor.ToString();
            else
                result += ActiveDriver.ToString();
            result += "\nDecorators: ";
            foreach (var dec in ActiveDecorators)
                result += dec.ToString() + ' ';
            result += "\nNotifiers: ";
            foreach (var notf in ActiveNotifiers)
                result += notf.ToString() + ' ';
            return result;
        }

        public void SendSetToRecommended()
        {
            this.ActiveDriver.RecmmndCoreSettings();
            this.ActiveGrabber.RecmmndCoreSettings();
        }

        public void AttachEvent(AntumbraColorObserver observer)
        {
            NewColorAvailEvent += observer.NewColorAvail;
        }

        void AntumbraColorObserver.NewColorAvail(Color newColor, EventArgs args)
        {
            if (ActiveDecorators.Count == 0) {
                NewColorAvailEvent(newColor, args);//no decoration to do
                return;
            }
            if (this.settings.compoundDecoration) {
                foreach (var dec in ActiveDecorators)//decorate
                    newColor = dec.Decorate(newColor);
                NewColorAvailEvent(newColor, args);
                return;
            }
            //average decorators output
            int r = 0, g = 0, b = 0;
            foreach (var dec in ActiveDecorators) {
                Color decorated = dec.Decorate(newColor);
                r += decorated.R;
                g += decorated.G;
                b += decorated.B;
            }
            int count = ActiveDecorators.Count;
            NewColorAvailEvent(Color.FromArgb(r / count, g / count, b / count), args);
        }

        public bool LoadingFailed()
        {
            return this.MEFHelper.failed;
        }

        public bool Start()
        {
            if (!Verify())
                return false;
            this.ActiveDriver.AttachEvent(this);
            this.ActiveDriver.Start();
            foreach (var dec in ActiveDecorators)
                dec.Start();
            foreach (var notf in ActiveNotifiers)
                notf.Start();
            return true;
        }

        private bool Verify()
        {
            if (this.settings == null)
                return false;
            if (ActiveDriver is GlowScreenDriverCoupler) {//screen based driver selected
                if (null == ActiveGrabber || null == ActiveProcessor) {//no grabber or processor set
                    return false;
                }
                ActiveGrabber.x = this.settings.x;//set screen related settings for grabber
                ActiveGrabber.y = this.settings.y;
                ActiveGrabber.width = this.settings.width;
                ActiveGrabber.height = this.settings.height;
                ActiveDriver = new GlowScreenDriverCoupler(ActiveGrabber, ActiveProcessor);
            }
            ActiveDriver.stepSleep = settings.stepSleep;
            return true;
        }

        public bool Stop()
        {
            this.ActiveDriver.Stop();//coupler will stop grabber & processor
            foreach (var dec in ActiveDecorators)
                dec.Stop();
            foreach (var notf in ActiveNotifiers)
                notf.Stop();
            return true;
        }
    }
}
