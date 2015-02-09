using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antumbra.Glow.ExtensionFramework
{
    /// <summary>
    /// Manages (through use of MEFHelper) the Extensions for use with Glow
    /// </summary>
    public class ExtensionManager
    {
        /// <summary>
        /// MEFHelper used to find and load the extensions.
        /// </summary>
        private MEFHelper MEFHelper;
        private AntumbraCore core;
        public GlowDriver ActiveDriver { get; set; }
        public GlowScreenGrabber ActiveGrabber { get; set; }
        public GlowScreenProcessor ActiveProcessor { get; set; }
        public List<GlowDecorator> ActiveDecorators { get; set; }
        public List<GlowNotifier> ActiveNotifiers { get; set; }

        public ExtensionManager(AntumbraCore core, string extDirPath)
        {
            this.core = core;//TODO switch out with settings struct / obj
            this.MEFHelper = new MEFHelper(extDirPath);
            this.ActiveDriver = this.GetDefaultDriver();
            this.ActiveGrabber = this.GetDefaultScreenGrabber();
            this.ActiveProcessor = this.GetDefaultScreenProcessor();
            this.ActiveDecorators = new List<GlowDecorator>();
            foreach (var dec in this.GetDefaultDecorators())
                this.ActiveDecorators.Add(dec);
            this.ActiveNotifiers = new List<GlowNotifier>();
            foreach (var notf in this.GetDefaultNotifiers())
                this.ActiveNotifiers.Add(notf);
        }

        public bool LoadingFailed()
        {
            return this.MEFHelper.failed;
        }

        public bool Start()
        {
            if (!Verify())
                return false;
            this.ActiveDriver.Start();
            foreach (var dec in ActiveDecorators)
                dec.Start();
            foreach (var notf in ActiveNotifiers)
                notf.Start();
            return true;
        }

        private bool Verify()
        {
            if (ActiveDriver is GlowScreenDriverCoupler) {//screen based driver selected
                if (null == ActiveGrabber || null == ActiveProcessor) {//no grabber or processor set
                    return false;
                }
                ActiveGrabber.x = this.core.pollingX;
                ActiveGrabber.y = this.core.pollingY;
                ActiveGrabber.width = this.core.pollingWidth;
                ActiveGrabber.height = this.core.pollingHeight;
                ActiveDriver = new GlowScreenDriverCoupler(ActiveGrabber, ActiveProcessor);
            }
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

        public List<GlowDriver> AvailDrivers
        {
            get
            {
                return this.MEFHelper.AvailDrivers;
            }
        }

        public List<GlowScreenGrabber> AvailScreenGrabbers
        {
            get
            {
                return this.MEFHelper.AvailScreenDrivers;
            }
        }

        public List<GlowScreenProcessor> AvailScreenProcessors
        {
            get
            {
                return this.MEFHelper.AvailScreenProcessors;
            }
        }

        public List<GlowDecorator> AvailDecorators
        {
            get
            {
                return this.MEFHelper.AvailDecorators;
            }
        }

        public List<GlowNotifier> AvailNotifiers
        {
            get
            {
                return this.MEFHelper.AvailNotifiers;
            }
        }

        public GlowDriver GetDefaultDriver()
        {
            foreach (var dvr in this.MEFHelper.AvailDrivers)
                if (dvr.IsDefault)
                    return dvr;
            return null;
        }

        public GlowScreenGrabber GetDefaultScreenGrabber()
        {
            foreach (var gbbr in this.MEFHelper.AvailScreenDrivers)
                if (gbbr.IsDefault)
                    return gbbr;
            return null;
        }

        public GlowScreenProcessor GetDefaultScreenProcessor()
        {
            foreach (var pcsr in this.MEFHelper.AvailScreenProcessors)
                if (pcsr.IsDefault)
                    return pcsr;
            return null;
        }

        public List<GlowDecorator> GetDefaultDecorators()
        {
            List<GlowDecorator> result = new List<GlowDecorator>();
            foreach (var dc in this.MEFHelper.AvailDecorators)
                if (dc.IsDefault)
                    result.Add(dc);
            return result;
        }

        public List<GlowNotifier> GetDefaultNotifiers()
        {
            List<GlowNotifier> result = new List<GlowNotifier>();
            foreach (var ntf in this.MEFHelper.AvailNotifiers)
                if (ntf.IsDefault)
                    result.Add(ntf);
            return result;
        }
    }
}
