using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antumbra.Glow.Settings
{
    public enum SettingValue
    {
        X = 0,
        Y = 1,
        WIDTH = 2,
        HEIGHT = 3,
        BOUNDX = 4,
        BOUNDY = 5,
        BOUNDWIDTH = 6,
        BOUNDHEIGHT = 7,
        MAXBRIGHTNESS = 8,
        WEIGHTINGENABLED = 9,
        NEWCOLORWEIGHT = 10,
        REDBIAS = 11,
        GREENBIAS = 12,
        BLUEBIAS = 13,
        STEPSLEEP = 14,
        CAPTURETHROTTLE = 15
    }

    public class SettingsDelta
    {
        public int id;
        public Dictionary<SettingValue, Object> changes = new Dictionary<SettingValue,object>();
    }
}
