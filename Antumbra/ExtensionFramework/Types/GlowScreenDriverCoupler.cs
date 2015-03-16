using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
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
        public delegate void NewColorAvail(Color newCol, EventArgs args);
        public event NewColorAvail NewColorAvailEvent;
        public delegate void NewLogMsg(String source, String msg);
        public event NewLogMsg NewLogMsgAvailEvent;
        public delegate void NewToolbarNotifAvail(int time, String title, String msg, int icon);
        public event NewToolbarNotifAvail NewToolbarNotifAvailEvent;
        public delegate void NewGlowCommandAvail(GlowCommand cmd);
        public event NewGlowCommandAvail NewGlowCommandAvailEvent;
        private int devId;
        private GlowScreenGrabber grabber;
        private GlowScreenProcessor processor;

        public GlowScreenDriverCoupler(GlowScreenGrabber grab, GlowScreenProcessor proc)
        {
            this.grabber = grab;
            this.processor = proc;
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

        public void RegisterDevice(int devId)
        {
            this.devId = devId;
        }

        void GlowCommandObserver.NewGlowCommandAvail(GlowCommand cmd)
        {
            NewGlowCommandAvailEvent(cmd);//pass it up
        }

        public override bool IsRunning
        {
            get { if (null != this.grabber && null != this.processor)
                    return this.grabber.IsRunning && this.processor.IsRunning;
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
        { get { return new Version(0, 1, 0, 0); } }

        public sealed override string Website
        {
            get { return "https://antumbra.io/docs/extensions/framework/GlowScreenDriverCoupler"; }//TODO make docs and change this accordingly
        }

        public override void AttachColorObserver(AntumbraColorObserver observer)
        {
            this.NewColorAvailEvent += new NewColorAvail(observer.NewColorAvail);
        }

        void AntumbraColorObserver.NewColorAvail(Color newCol, EventArgs args)
        {
            NewColorAvailEvent(newCol, args);//pass it up
        }

        public override bool Start()
        {
            if (this.grabber != null && this.processor != null) {
                AttemptToAttachSelfToExt(this.processor);
                if (this.processor.Start()) {
                    if (this.processor is AntumbraBitmapObserver)
                        this.grabber.AttachObserver((AntumbraBitmapObserver)this.processor);
                    this.processor.AttachObserver(this);
                    AttemptToAttachSelfToExt(this.grabber);
                    if (this.grabber.Start()) {
                        return true;
                    }
                }
            }
            return false;
        }

        private void AttemptToAttachSelfToExt(GlowExtension ext)//TODO make a utility / static function / move elsewhere?
        {
            if (ext is Loggable) {
                Loggable log = (Loggable)ext;
                log.AttachObserver(this);
            }
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
            if (this.processor != null)
                this.processor.Stop();
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
    }
}
