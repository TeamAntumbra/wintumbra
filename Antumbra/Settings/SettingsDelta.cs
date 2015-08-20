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
        public Dictionary<SettingValue, object> changes = new Dictionary<SettingValue, object>();

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("SettingsDelta Changes:\t");
            var i = 0;
            var last = changes.Count - 1;
            foreach (KeyValuePair<SettingValue, object> change in changes) {
                sb.Append(change.Key).Append(" : ").Append(change.Value);
                if (i != last) {
                    sb.Append(", ");
                    i += 1;
                }
            }
            return sb.ToString();
        }
    }
}
