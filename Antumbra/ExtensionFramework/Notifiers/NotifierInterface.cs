using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Antumbra.Glow.Windows;

namespace Antumbra.Glow.ExtensionFramework.Notifiers
{
    interface NotifierInterface : GlowExtension
    {
        Boolean update();//called in loop, if true is returned the extensions Notify() function will be called   
        void Notify(AntumbraCoreDriver glow);//runs light sequence in reaction to notification using glow (the Antumbra object)
    }
}
