using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antumbra.Glow.ExtensionFramework.Types.Notifiications
{
    public struct Notification//TODO, move to util?
    {
        private String NotiName;
        private String NotiDetails;

        public Notification(String Name, String Details)
        {
            this.NotiName = Name;
            this.NotiDetails = Details;
        }

        public String Name
        { get { return this.NotiName; } }

        public String Details
        { get { return this.NotiDetails; } }
    }
}
