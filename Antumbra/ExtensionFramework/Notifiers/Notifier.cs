using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Antumbra.Glow.ExtensionFramework.Notifiers
{
    abstract class Notifier : GlowExtension//Base for Notifier extensions
    //will be called in loop to update, can send light 'actions' to main drivers queue
    {
        public Boolean update();//called in loop, if true is returned the extensions Notify() function will be called   
        public void Notify(Antumbra glow);//runs light sequence in reaction to notification using glow (the Antumbra object)
        public String Type { get { return "Notifier"; } }
    }
}
