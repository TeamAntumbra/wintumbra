﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antumbra.Glow.ToolbarNotifications
{
    public interface ToolbarNotificationSource
    {
        void AttachEvent(ToolbarNotificationObserver observer);
    }
}