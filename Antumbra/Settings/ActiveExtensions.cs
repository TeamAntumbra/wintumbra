using Antumbra.Glow.ExtensionFramework.Types;
using Antumbra.Glow.Observer.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Antumbra.Glow.Settings {

    [Serializable()]
    public class ActiveExtensions : Configurable, ISerializable {

        #region Private Fields

        private GlowDriver _ActiveDriver;

        private List<GlowFilter> _ActiveFilters;

        private GlowScreenGrabber _ActiveGrabber;

        private List<GlowNotifier> _ActiveNotifiers;

        private List<GlowScreenProcessor> _ActiveProcessors;

        private Guid DriverGuid, GrabberGuid;

        private List<Guid> ProcessorGuids, FilterGuids, NotifierGuids;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Constructor - Create an Empty ActiveExtensions objectS
        /// </summary>
        public ActiveExtensions() {
            this._ActiveProcessors = new List<GlowScreenProcessor>();
            this._ActiveFilters = new List<GlowFilter>();
            this._ActiveNotifiers = new List<GlowNotifier>();
        }

        public ActiveExtensions(Guid DriverGuid, Guid GrabberGuid, Guid[] ProcessorGuids,
                                Guid[] FilterGuids, Guid[] NotifierGuids) {
            this._ActiveProcessors = new List<GlowScreenProcessor>();
            this._ActiveFilters = new List<GlowFilter>();
            this._ActiveNotifiers = new List<GlowNotifier>();
            this.DriverGuid = DriverGuid;
            this.GrabberGuid = GrabberGuid;
            this.ProcessorGuids = ProcessorGuids.ToList();
            this.FilterGuids = FilterGuids.ToList();
            this.NotifierGuids = NotifierGuids.ToList();
        }

        /// <summary>
        /// Load serialized object
        /// </summary>
        /// <param name="info"></param>
        /// <param name="cntx"></param>
        public ActiveExtensions(SerializationInfo info, StreamingContext cntx) {
            try {
                DriverGuid = (Guid)info.GetValue("driverGuid", typeof(Guid));
            } catch(Exception) { }
            try {
                GrabberGuid = (Guid)info.GetValue("grabberGuid", typeof(Guid));
            } catch(Exception) { }
            try {
                ProcessorGuids = new List<Guid>((Guid[])info.GetValue("processorGuids", typeof(Guid[])));
            } catch(Exception) { }
            try {
                FilterGuids = new List<Guid>((Guid[])info.GetValue("filterGuids", typeof(Guid[])));
            } catch(Exception) { }
            try {
                NotifierGuids = new List<Guid>((Guid[])info.GetValue("notifierGuids", typeof(Guid[])));
            } catch(Exception) { }
        }

        #endregion Public Constructors

        #region Public Delegates

        public delegate void ConfigurationUpdate(Configurable obj);

        #endregion Public Delegates

        #region Public Events

        public event ConfigurationUpdate ConfigurationChangedEvent;

        #endregion Public Events

        #region Public Properties

        public GlowDriver ActiveDriver {
            get {
                return this._ActiveDriver;
            }
            set {
                this._ActiveDriver = value;
                Notify();
            }
        }

        public List<GlowFilter> ActiveFilters {
            get { return this._ActiveFilters; }
            set {
                this._ActiveFilters = value;
                Notify();
            }
        }

        public GlowScreenGrabber ActiveGrabber {
            get {
                return this._ActiveGrabber;
            }
            set {
                this._ActiveGrabber = value;
                Notify();
            }
        }

        public List<GlowNotifier> ActiveNotifiers {
            get { return this._ActiveNotifiers; }
            set {
                this._ActiveNotifiers = value;
                Notify();
            }
        }

        public List<GlowScreenProcessor> ActiveProcessors {
            get {
                return _ActiveProcessors;
            }
            set {
                _ActiveProcessors = value;
                Notify();
            }
        }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Attach an observer for Configuration updates
        /// </summary>
        /// <param name="observer"></param>
        public void AttachObserver(ConfigurationObserver observer) {
            this.ConfigurationChangedEvent += observer.ConfigurationUpdate;
        }

        /// <summary>
        /// Serialize this object
        /// </summary>
        /// <param name="info"></param>
        /// <param name="cntx"></param>
        public void GetObjectData(SerializationInfo info, StreamingContext cntx) {
            if(ActiveDriver != null) {
                info.AddValue("driverGuid", ActiveDriver.id);
            }

            if(ActiveGrabber != null) {
                info.AddValue("grabberGuid", ActiveGrabber.id);
            }
            List<Guid> guids = new List<Guid>();
            foreach(GlowScreenProcessor prcr in ActiveProcessors) {
                guids.Add(prcr.id);
            }
            info.AddValue("processorGuids", guids.ToArray());
            guids.Clear();
            foreach(GlowFilter filt in ActiveFilters) {
                guids.Add(filt.id);
            }
            info.AddValue("filterGuids", guids.ToArray());
            guids.Clear();
            foreach(GlowNotifier notf in ActiveNotifiers) {
                guids.Add(notf.id);
            }
            info.AddValue("notifierGuids", guids.ToArray());
        }

        /// <summary>
        /// Load the extensions held by the Guid variables after a Load() call using the ExtensionLibrary
        /// </summary>
        /// <param name="library">The ExtensionLibrary</param>
        public void Init(ExtensionFramework.Management.ExtensionLibrary library) {
            if(!DriverGuid.Equals(Guid.Empty)) {
                ActiveDriver = library.LookupDriver(DriverGuid);
                DriverGuid = Guid.Empty;
            }
            if(!GrabberGuid.Equals(Guid.Empty)) {
                ActiveGrabber = library.LookupGrabber(GrabberGuid);
                GrabberGuid = Guid.Empty;
            }
            if(ActiveProcessors == null) {
                ActiveProcessors = new List<GlowScreenProcessor>();
            }
            foreach(Guid guid in ProcessorGuids) {
                ActiveProcessors.Add(library.LookupProcessor(guid));
            }
            ProcessorGuids.Clear();

            if(ActiveFilters == null) {
                ActiveFilters = new List<GlowFilter>();
            }
            foreach(Guid guid in FilterGuids) {
                ActiveFilters.Add(library.LookupFilter(guid));
            }
            FilterGuids.Clear();

            if(ActiveNotifiers == null) {
                ActiveNotifiers = new List<GlowNotifier>();
            }
            foreach(Guid guid in NotifierGuids) {
                ActiveNotifiers.Add(library.LookupNotifier(guid));
            }
            NotifierGuids.Clear();
        }

        /// <summary>
        /// Notify observers that this object's Configuration has changed
        /// </summary>
        public void Notify() {
            if(ConfigurationChangedEvent != null)
                ConfigurationChangedEvent(this);
        }

        public void Ready() {
            if(ActiveDriver is GlowScreenDriverCoupler) {
                ActiveDriver = new GlowScreenDriverCoupler(ActiveGrabber, ActiveProcessors);
            }
        }

        /// <summary>
        /// Get a String representation of the object
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            String result = "";
            if(this.ActiveDriver != null) {
                result += this.ActiveDriver.id.ToString() + ',';
            }
            if(this.ActiveGrabber != null) {
                result += this.ActiveGrabber.id.ToString() + ',';
            }
            if(ActiveProcessors != null) {
                int count = this.ActiveProcessors.Count;
                for(int i = 0; i < count; i += 1) {
                    GlowScreenProcessor processor = this.ActiveProcessors[i];
                    result += processor.id.ToString();
                    if(i != count - 1)//not the last one
                        result += ' ';
                }
                result += ',';
            }
            if(this.ActiveFilters != null) {
                int count = this.ActiveFilters.Count;
                for(int i = 0; i < count; i += 1) {
                    GlowFilter filt = this.ActiveFilters[i];
                    result += filt.id.ToString();
                    if(i != count - 1)//not the last one
                        result += ' ';
                }
                result += ',';
            }
            if(this.ActiveNotifiers != null) {
                int count = this.ActiveNotifiers.Count;
                for(int i = 0; i < count; i += 1) {
                    GlowNotifier notf = this.ActiveNotifiers[i];
                    result += notf.id.ToString();
                    if(i != count - 1)//not the last one
                        result += ' ';
                }
                result += ',';
            }
            return result;
        }

        #endregion Public Methods
    }
}
