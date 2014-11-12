using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace Antumbra.Glow.ExtensionFramework //NOTE FOR NOW IGNORE THE SEPARATE FILES AND ONLY USE THE STUFF HERE FOR EXTENSIONS!
{
  /*  public interface GlowExtension//basis for all Glow Extensions
    {
        String Name { get; }
        String Author { get; }
        String Version { get; }
        String Description { get; }
        String Type { get; }//one of the following: Decorator, Driver, Notifier, ScreenProcessor
    }*/

    public abstract class GlowExtension
    {
        abstract public String Name { get; }
        abstract public String Author { get; }
        abstract public String Version { get; }
        abstract public String Description { get; }
        abstract public String Type { get; }
    }

    public abstract class GlowDriver : GlowExtension //observed by core
    {
        abstract public Color getColor();//main driver method (called repeatedly to get colors)
        public override String Type { get { return "Driver"; } }
    }

    public abstract class GlowScreenDriver : GlowDriver //observed by screen processor
        //special type of driver that deals with bitmaps captured from the screen
        //uses a GlowScreenProcessor to determine color to return
    {
        //abstract public GlowScreenProcessor ScreenProcessor { get; }//return processor for this screen driver
        abstract public void captureTarget();//target method for capture thread (defines thread logic)
        public sealed override string Type { get { return "Screen Driver"; } }
    }

    public abstract class GlowDecorator : GlowExtension
    {
        abstract public Color Decorate(Color origColor);//Returns decorated color
        public sealed override String Type { get { return "Decorator"; } }
    }
    public abstract class GlowNotifier : GlowExtension //observed by core
    {
        abstract public Boolean Update();//returns true if notification exists
        abstract public Boolean Notify(AntumbraCore controller);
        //run color sequence configured for the notification
        //called after an Update() call returns true
        public sealed override String Type { get { return "Notifier"; } }
    }
    public abstract class GlowScreenProcessor : GlowExtension
    {
        abstract public Color Process(Bitmap bm);//returns color based off Bitmap of screen
        public sealed override String Type { get { return "Screen Processor"; } }
    }
}
