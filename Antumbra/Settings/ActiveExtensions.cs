using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antumbra.Glow.ExtensionFramework.Types;
using Antumbra.Glow.Observer.Configuration;

namespace Antumbra.Glow.Settings
{
    public class ActiveExtensions : Configurable
    {
        public delegate void ConfigurationUpdate(Configurable obj);
        public event ConfigurationUpdate ConfigurationChangedEvent;
        private GlowDriver _ActiveDriver;
        public GlowDriver ActiveDriver
        {
            get
            {
                return this._ActiveDriver;
            }
            set
            {
                this._ActiveDriver = value;
                Notify();
            }
        }
        private GlowScreenGrabber _ActiveGrabber;
        public GlowScreenGrabber ActiveGrabber
        {
            get
            {
                return this._ActiveGrabber;
            }
            set
            {
                this._ActiveGrabber = value;
                Notify();
            }
        }
        private List<GlowScreenProcessor> _ActiveProcessors;
        public List<GlowScreenProcessor> ActiveProcessors
        {
            get
            {
                return _ActiveProcessors;
            }
            set
            {
                _ActiveProcessors = value;
                Notify();
            }
        }
        private List<GlowFilter> _ActiveFilters;
        public List<GlowFilter> ActiveFilters
        {
            get { return this._ActiveFilters; }
            set
            {
                this._ActiveFilters = value;
                Notify();
            }
        }
        private List<GlowNotifier> _ActiveNotifiers;
        public List<GlowNotifier> ActiveNotifiers
        {
            get { return this._ActiveNotifiers; }
            set
            {
                this._ActiveNotifiers = value;
                Notify();
            }
        }
        public ActiveExtensions()
        {
            this._ActiveFilters = new List<GlowFilter>();
            this._ActiveNotifiers = new List<GlowNotifier>();
        }

        public void Notify()
        {
            if (ConfigurationChangedEvent != null)
                ConfigurationChangedEvent(this);
        }

        public void AttachObserver(ConfigurationObserver observer)
        {
            this.ConfigurationChangedEvent += observer.ConfigurationUpdate;
        }

        public override string ToString()
        {
            String result = "";
            if (this.ActiveDriver != null) {
                result += this.ActiveDriver.id.ToString() + ',';
            }
            if (this.ActiveGrabber != null) {
                result += this.ActiveGrabber.id.ToString() + ',';
            }
            if (ActiveProcessor != null) {
                result += this.ActiveProcessor.id.ToString() + ',';
            }
            if (this.ActiveFilters != null) {
                int count = this.ActiveFilters.Count;
                for (int i = 0; i < count; i += 1) {
                    GlowFilter filt = this.ActiveFilters[i];
                    result += filt.id.ToString();
                    if (i != count - 1)//not the last one
                        result += ' ';
                }
                result += ',';
            }
            if (this.ActiveNotifiers != null) {
                int count = this.ActiveNotifiers.Count;
                for (int i = 0; i < count; i += 1) {
                    GlowNotifier notf = this.ActiveNotifiers[i];
                    result += notf.id.ToString();
                    if (i != count - 1)//not the last one
                        result += ' ';
                }
                result += ',';
            }
            return result;
        }
    }
}
