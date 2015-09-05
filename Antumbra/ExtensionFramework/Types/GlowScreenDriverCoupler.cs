using Antumbra.Glow.Observer.Colors;
using Antumbra.Glow.Observer.GlowCommands;
using Antumbra.Glow.Observer.Logging;
using Antumbra.Glow.Observer.ToolbarNotifications;
using System;
using System.Collections.Generic;

namespace Antumbra.Glow.ExtensionFramework.Types {

    public class GlowScreenDriverCoupler : GlowDriver, AntumbraColorObserver, Loggable, LogMsgObserver,
                                           ToolbarNotificationSource, ToolbarNotificationObserver,
                                           GlowCommandObserver, GlowCommandSender
        //generates color using a GlowScreenGrabber
        //and a GlowScreenProcessor
    {
        #region Private Fields

        private GlowScreenGrabber grabber;

        private List<GlowScreenProcessor> processors;

        #endregion Private Fields

        #region Public Constructors

        public GlowScreenDriverCoupler(GlowScreenGrabber grabber, List<GlowScreenProcessor> processors) {
            this.grabber = grabber;
            this.processors = processors;
        }

        #endregion Public Constructors

        #region Public Delegates

        public delegate void NewColorAvail(Color16Bit newCol, int id, long index);

        public delegate void NewGlowCommandAvail(GlowCommand cmd);

        public delegate void NewLogMsg(String source, String msg);

        public delegate void NewToolbarNotifAvail(int time, String title, String msg, int icon);

        #endregion Public Delegates

        #region Public Events

        public event NewColorAvail NewColorAvailEvent;

        public event NewGlowCommandAvail NewGlowCommandAvailEvent;

        public event NewLogMsg NewLogMsgAvailEvent;

        public event NewToolbarNotifAvail NewToolbarNotifAvailEvent;

        #endregion Public Events

        #region Public Properties

        public sealed override string Author { get { return "Team Antumbra"; } }

        public sealed override string Description {
            get {
                return "A GlowDriver that uses a GlowScreenGrabber and "
                     + "a GlowScreenProcessor to generate colors";
            }
        }

        public override int devId { get; set; }

        public override Guid id {
            get { return Guid.Parse("70987576-1a00-4a34-b787-4c08516cd1b8"); }
        }

        public sealed override bool IsDefault {
            get { return true; }
        }

        public override bool IsRunning {
            get {
                if(null != this.grabber && null != this.processors) {
                    bool processorsRunning = false;
                    foreach(GlowScreenProcessor processor in processors) {
                        if(processor.IsRunning) {
                            processorsRunning = true;
                            break;
                        }
                    }
                    return this.grabber.IsRunning && processorsRunning;
                }
                return false;
            }
        }

        public sealed override string Name { get { return "Glow Screen Driver Coupler"; } }

        public sealed override Version Version {
            get { return new Version(0, 1, 1, 0); }
        }

        public sealed override string Website {
            get { return "https://wintumbra.rtfd.org"; }
        }

        #endregion Public Properties

        #region Public Methods

        void AntumbraColorObserver.NewColorAvail(Color16Bit newCol, int id, long index) {
            if(NewColorAvailEvent != null)
                NewColorAvailEvent(newCol, id, index);//pass it up
        }

        public override void AttachColorObserver(AntumbraColorObserver observer) {
            this.NewColorAvailEvent += new NewColorAvail(observer.NewColorAvail);
        }

        public void AttachObserver(LogMsgObserver observer) {
            NewLogMsgAvailEvent += observer.NewLogMsgAvail;
        }

        public void AttachObserver(ToolbarNotificationObserver observer) {
            NewToolbarNotifAvailEvent += observer.NewToolbarNotifAvail;
        }

        public void AttachObserver(GlowCommandObserver observer) {
            NewGlowCommandAvailEvent += observer.NewGlowCommandAvail;
        }

        public override GlowDriver Create() {
            return this;
        }

        public override void Dispose() {
            if(grabber != null) {
                grabber.Dispose();
            }

            if(processors != null) {
                foreach(GlowScreenProcessor processor in processors) {
                    processor.Dispose();
                }
            }
        }

        void GlowCommandObserver.NewGlowCommandAvail(GlowCommand cmd) {
            NewGlowCommandAvailEvent(cmd);//pass it up
        }

        public void NewLogMsgAvail(String source, String msg) {
            NewLogMsgAvailEvent(source, msg);
        }

        public override void RecmmndCoreSettings() {
            this.stepSleep = 0;
        }

        public override bool Settings() {
            return false;
        }

        public override bool Start() {
            if(this.grabber != null && this.processors != null) {
                AttemptToAttachSelfToExt(this.processors);
                foreach(GlowScreenProcessor processor in processors) {
                    if(processor.Start()) {
                        grabber.AttachObserver(processor);
                        processor.AttachObserver(this);
                    } else {
                        // Stop those started before failing, then report failure to start
                        for(int i = 0; i < processors.IndexOf(processor); i += 1) {
                            processors[i].Stop();
                        }
                        return false;
                    }
                }
                AttemptToAttachSelfToExt(grabber);
                return grabber.Start();
            }
            return false;
        }

        public override bool Stop() {
            if(processors != null) {
                foreach(GlowScreenProcessor processor in processors) {
                    processor.Stop();
                }
            }
            if(this.grabber != null)
                this.grabber.Stop();
            return true;
        }

        void ToolbarNotificationObserver.NewToolbarNotifAvail(int time, String title, String msg, int icon) {
            NewToolbarNotifAvailEvent(time, title, msg, icon);//pass it up
        }

        #endregion Public Methods

        #region Private Methods

        private void AttemptToAttachSelfToExt(List<GlowScreenProcessor> exts)//TODO make a utility / static function / move elsewhere?
        {
            foreach(GlowExtension ext in exts) {
                AttemptToAttachSelfToExt(ext);
            }
        }

        private void AttemptToAttachSelfToExt(GlowExtension ext)//TODO make a utility / static function / move elsewhere?
        {
            if(ext is ToolbarNotificationSource) {
                ToolbarNotificationSource src = (ToolbarNotificationSource)ext;
                src.AttachObserver(this);
            }
            if(ext is GlowCommandSender) {
                GlowCommandSender sender = (GlowCommandSender)ext;
                sender.AttachObserver(this);
            }
        }

        #endregion Private Methods
    }
}
