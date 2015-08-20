using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antumbra.Glow.Observer.Logging;
using Antumbra.Glow.Observer.ToolbarNotifications;
using Antumbra.Glow.Observer.GlowCommands;
using Antumbra.Glow.Observer.Colors;
using Antumbra.Glow.Observer.Bitmaps;

namespace Antumbra.Glow.ExtensionFramework.Types
{
    public class GlowScreenDriverCoupler : GlowDriver, AntumbraColorObserver, Loggable, LogMsgObserver,
                                           ToolbarNotificationSource, ToolbarNotificationObserver,
                                           GlowCommandObserver, GlowCommandSender
    //generates color using a GlowScreenGrabber
    //and a GlowScreenProcessor
    {
        public delegate void NewColorAvail(Color16Bit newCol, int id, long index);
        public event NewColorAvail NewColorAvailEvent;
        public delegate void NewLogMsg(String source, String msg);
        public event NewLogMsg NewLogMsgAvailEvent;
        public delegate void NewToolbarNotifAvail(int time, String title, String msg, int icon);
        public event NewToolbarNotifAvail NewToolbarNotifAvailEvent;
        public delegate void NewGlowCommandAvail(GlowCommand cmd);
        public event NewGlowCommandAvail NewGlowCommandAvailEvent;
        public override int devId { get; set; }

        private GlowScreenGrabber grabber;
        private List<GlowScreenProcessor> processors;

        public GlowScreenDriverCoupler(GlowScreenGrabber grabber, List<GlowScreenProcessor> processors)
        {
            this.grabber = grabber;
            this.processors = processors;
        }

        public void AttachObserver(LogMsgObserver observer)
        {
            NewLogMsgAvailEvent += observer.NewLogMsgAvail;
        }

        public void NewLogMsgAvail(String source, String msg)
        {
            NewLogMsgAvailEvent(source, msg);
        }

        void ToolbarNotificationObserver.NewToolbarNotifAvail(int time, String title, String msg, int icon)
        {
            NewToolbarNotifAvailEvent(time, title, msg, icon);//pass it up
        }

        public void AttachObserver(ToolbarNotificationObserver observer)
        {
            NewToolbarNotifAvailEvent += observer.NewToolbarNotifAvail;
        }

        public void AttachObserver(GlowCommandObserver observer)
        {
            NewGlowCommandAvailEvent += observer.NewGlowCommandAvail;
        }

        void GlowCommandObserver.NewGlowCommandAvail(GlowCommand cmd)
        {
            NewGlowCommandAvailEvent(cmd);//pass it up
        }

        public override bool IsRunning
        {
            get
            {
                if (null != this.grabber && null != this.processors) {
                    bool processorsRunning = false;
                    foreach (GlowScreenProcessor processor in processors) {
                        if (processor.IsRunning) {
                            processorsRunning = true;
                            break;
                        }
                    }
                    return this.grabber.IsRunning && processorsRunning;
                }
                return false;
            }
        }
        public sealed override string Name
        { get { return "Glow Screen Driver Coupler"; } }
        public sealed override string Author
        { get { return "Team Antumbra"; } }
        public sealed override string Description
        { get {
            return "A GlowDriver that uses a GlowScreenGrabber and "
                 + "a GlowScreenProcessor to generate colors";
            }
        }

        public override Guid id
        {
            get { return Guid.Parse("70987576-1a00-4a34-b787-4c08516cd1b8"); }
        }

        public sealed override bool IsDefault
        {
            get { return true; }
        }
        public sealed override Version Version
        {
            get { return new Version(0, 1, 1, 0); }
        }

        public sealed override string Website
        {
            get { return "https://wintumbra.rtfd.org"; }
        }

        public override void AttachColorObserver(AntumbraColorObserver observer)
        {
            this.NewColorAvailEvent += new NewColorAvail(observer.NewColorAvail);
        }

        void AntumbraColorObserver.NewColorAvail(Color16Bit newCol, int id, long index)
        {
            if(NewColorAvailEvent != null)
                NewColorAvailEvent(newCol, id, index);//pass it up
        }

        public override bool Start()
        {
            if (this.grabber != null && this.processors != null) {
                AttemptToAttachSelfToExt(this.processors);
                foreach (GlowScreenProcessor processor in processors) {
                    if (processor.Start()) {
                        grabber.AttachObserver(processor);
                    }
                    else {
                        // Stop those started before failing, then report failure to start
                        for (int i = 0; i < processors.IndexOf(processor); i += 1) {
                            processors[i].Stop();
                        }
                        return false;
                    }
                    processor.AttachObserver(this);
                }
                AttemptToAttachSelfToExt(grabber);
                return grabber.Start();
            }
            return false;
        }

        private void AttemptToAttachSelfToExt(List<GlowScreenProcessor> exts)//TODO make a utility / static function / move elsewhere?
        {
            foreach (GlowExtension ext in exts) {
                AttemptToAttachSelfToExt(ext);
            }
        }

        private void AttemptToAttachSelfToExt(GlowExtension ext)//TODO make a utility / static function / move elsewhere?
        {
            if (ext is ToolbarNotificationSource) {
                ToolbarNotificationSource src = (ToolbarNotificationSource)ext;
                src.AttachObserver(this);
            }
            if (ext is GlowCommandSender) {
                GlowCommandSender sender = (GlowCommandSender)ext;
                sender.AttachObserver(this);
            }

        }

        public override bool Stop()
        {
            if (processors != null) {
                foreach (GlowScreenProcessor processor in processors) {
                    processor.Stop();
                }
            }
            if (this.grabber != null)
                this.grabber.Stop();
            return true;
        }

        public override void RecmmndCoreSettings()
        {
            this.stepSleep = 0;
        }

        public override bool Settings()
        {
            return false;
        }

        public override GlowDriver Create()
        {
            return this;
        }

        public override void Dispose()
        {
            if (grabber != null) {
                grabber.Dispose();
            }

            if (processors != null) {
                foreach (GlowScreenProcessor processor in processors) {
                    processor.Dispose();
                }
            }
        }
    }
}
