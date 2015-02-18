using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Capture.Hook.Common
{
    internal interface IOverlay: IOverlayElement
    {
        List<IOverlayElement> Elements { get; set; }
    }
}
