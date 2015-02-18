using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Capture.Hook.Common
{
    public interface IOverlayElement : ICloneable
    {
        bool Hidden { get; set; }

        void Frame();
    }
}
