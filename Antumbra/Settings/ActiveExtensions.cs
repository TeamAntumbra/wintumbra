using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Antumbra.Glow.ExtensionFramework.Types;
using Antumbra.Glow.Observer.Configuration;

namespace Antumbra.Glow.Settings
{
    [Serializable()]
    public class ActiveExtensions : Configurable, ISerializable
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
        private Guid DriverGuid, GrabberGuid;
        private List<Guid> ProcessorGuids, FilterGuids, NotifierGuids;

        /// <summary>
        /// Constructor - Create an Empty ActiveExtensions objectS
        /// </summary>
        public ActiveExtensions()
        {
            this._ActiveFilters = new List<GlowFilter>();
            this._ActiveNotifiers = new List<GlowNotifier>();
        }

        /// <summary>
        /// Notify observers that this object's Configuration has changed
        /// </summary>
        public void Notify()
        {
            if (ConfigurationChangedEvent != null)
                ConfigurationChangedEvent(this);
        }

        /// <summary>
        /// Attach an observer for Configuration updates
        /// </summary>
        /// <param name="observer"></param>
        public void AttachObserver(ConfigurationObserver observer)
        {
            this.ConfigurationChangedEvent += observer.ConfigurationUpdate;
        }

        /// <summary>
        /// Get a String representation of the object
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            String result = "";
            if (this.ActiveDriver != null) {
                result += this.ActiveDriver.id.ToString() + ',';
            }
            if (this.ActiveGrabber != null) {
                result += this.ActiveGrabber.id.ToString() + ',';
            }
            if (ActiveProcessors != null) {
                int count = this.ActiveProcessors.Count;
                for (int i = 0; i < count; i += 1) {
                    GlowScreenProcessor processor = this.ActiveProcessors[i];
                    result += processor.id.ToString();
                    if (i != count - 1)//not the last one
                        result += ' ';
                }
                result += ',';
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

        /// <summary>
        /// Load the extensions held by the Guid variables after a Load() call
        /// using the ExtensionLibrary
        /// </summary>
        /// <param name="library">The ExtensionLibrary</param>
        public void Init(ExtensionFramework.Management.ExtensionLibrary library)
        {
            ActiveDriver = (GlowDriver)library.findExt(DriverGuid);
            ActiveGrabber = (GlowScreenGrabber)library.findExt(GrabberGuid);
            foreach (Guid guid in ProcessorGuids) {
                ActiveProcessors.Add((GlowScreenProcessor)library.findExt(guid));
            }
            foreach (Guid guid in FilterGuids) {
                ActiveFilters.Add((GlowFilter)library.findExt(guid));
            }
            foreach (Guid guid in NotifierGuids) {
                ActiveNotifiers.Add((GlowNotifier)library.findExt(guid));
            }
        }

        /// <summary>
        /// Load serialized object
        /// </summary>
        /// <param name="info"></param>
        /// <param name="cntx"></param>
        public ActiveExtensions(SerializationInfo info, StreamingContext cntx)
        {
            DriverGuid = (Guid)info.GetValue("driverGuid", typeof(Guid));
            GrabberGuid = (Guid)info.GetValue("grabberGuid", typeof(Guid));
            ProcessorGuids = (List<Guid>)info.GetValue("processorGuids", typeof(List<Guid>));
            FilterGuids = (List<Guid>)info.GetValue("filterGuids", typeof(List<Guid>));
            NotifierGuids = (List<Guid>)info.GetValue("notifierGuids", typeof(List<Guid>));
        }

        /// <summary>
        /// Serialize this object
        /// </summary>
        /// <param name="info"></param>
        /// <param name="cntx"></param>
        public void GetObjectData(SerializationInfo info, StreamingContext cntx)
        {
            info.AddValue("driverGuid", ActiveDriver.id);
            info.AddValue("grabberGuid", ActiveGrabber.id);
            List<Guid> guids = new List<Guid>();
            foreach (GlowScreenProcessor prcr in ActiveProcessors) {
                guids.Add(prcr.id);
            }
            info.AddValue("processorGuids", guids);
            guids.Clear();
            foreach (GlowFilter filt in ActiveFilters) {
                guids.Add(filt.id);
            }
            info.AddValue("filterGuids", guids);
            guids.Clear();
            foreach (GlowNotifier notf in ActiveNotifiers) {
                guids.Add(notf.id);
            }
            info.AddValue("notifierGuids", guids);
        }
    }
}
