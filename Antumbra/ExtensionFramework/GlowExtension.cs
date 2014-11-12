using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace Antumbra.Glow.ExtensionFramework //NOTE FOR NOW IGNORE THE SEPARATE FILES AND ONLY USE THE STUFF HERE FOR EXTENSIONS!
{
    public interface GlowExtension//basis for all Glow Extensions
    {
        String Name { get; }
        String Author { get; }
        String Version { get; }
        String Description { get; }
        String Type { get; }//one of the following: Decorator, Driver, Notifier, ScreenProcessor
    }

    abstract class GlowDriver : GlowExtension
    {
        abstract public Color getColor();//main driver method (called repeatedly to get colors)
        abstract public String Name();
        abstract public String Author();
        abstract public String Version();
        abstract public String Description();
        public String Type { get { return "Driver"; } }
    }

    abstract class GlowScreenGrabber : GlowDriver//needed to know when to also load screen processor extensions
    {
        abstract public Bitmap getScreen();
        abstract public void captureTarget();//target method for capture thread (defines thread logic)
    }

    abstract class GlowDecorator : GlowExtension
    {
        abstract public Color Decorate(Color origColor);//Returns decorated color
        abstract public String Name();
        abstract public String Author();
        abstract public String Version();
        abstract public String Description();
        public String Type { get { return "Decorator"; } }
    }
    abstract class GlowNotifier : GlowExtension
    {
        abstract public Boolean Update();//returns true if notification exists
        abstract public Boolean Notify(AntumbraCoreDriver controller);
        //run color sequence configured for the notification
        //called after an Update() call returns true
        abstract public String Name();
        abstract public String Author();
        abstract public String Version();
        abstract public String Description();
        public String Type { get { return "Notifier"; } }
    }
    abstract class GlowScreenProcessor : GlowExtension
    {
        abstract public Color Process(Bitmap bm);//returns color based off Bitmap of screen
        abstract public String Name();
        abstract public String Author();
        abstract public String Version();
        abstract public String Description();
        public String Type { get { return "Screen Processor"; } }
    }
}
