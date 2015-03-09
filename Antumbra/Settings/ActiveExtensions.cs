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
        public GlowDriver ActiveDriver;
        public GlowScreenGrabber ActiveGrabber;
        public GlowScreenProcessor ActiveProcessor;
        public List<GlowDecorator> ActiveDecorators;
        public List<GlowNotifier> ActiveNotifiers;
        public ActiveExtensions()
        {
            this.ActiveDecorators = new List<GlowDecorator>();
            this.ActiveNotifiers = new List<GlowNotifier>();
        }

        public void AttachConfigurationObserver(ConfigurationObserver observer)
        {
            this.ConfigurationChangedEvent += observer.ConfigurationUpdate;
        }
    }
}
