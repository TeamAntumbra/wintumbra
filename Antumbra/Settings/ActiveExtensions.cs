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
        private GlowScreenProcessor _ActiveProcessor;
        public GlowScreenProcessor ActiveProcessor
        {
            get
            {
                return _ActiveProcessor;
            }
            set
            {
                _ActiveProcessor = value;
                Notify();
            }
        }
        private List<GlowDecorator> _ActiveDecorators;
        public List<GlowDecorator> ActiveDecorators
        {
            get { return this._ActiveDecorators; }
            set
            {
                this._ActiveDecorators = value;
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
            this._ActiveDecorators = new List<GlowDecorator>();
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
            result += this.ActiveDriver.id.ToString() + ',';
            result += this.ActiveGrabber.id.ToString() + ',';
            result += this.ActiveProcessor.id.ToString() + ',';
            int count = this.ActiveDecorators.Count;
            for (int i = 0; i < count; i += 1) {
                GlowDecorator dec = this.ActiveDecorators[i];
                result += dec.id.ToString();
                if (i != count - 1)//not the last one
                    result += ' ';
            }
            result += ',';
            count = this.ActiveNotifiers.Count;
            for (int i = 0; i < count; i += 1) {
                GlowNotifier notf = this.ActiveNotifiers[i];
                result += notf.id.ToString();
                if (i != count - 1)//not the last one
                    result += ' ';
            }
            result += ',';
            return result;
        }
    }
}
