using System;

namespace Antumbra.Glow.ExtensionFramework.Types.Notifiications {

    public struct Notification//TODO, move to util?
    {
        #region Private Fields

        private String NotiDetails;
        private String NotiName;

        #endregion Private Fields

        #region Public Constructors

        public Notification(String Name, String Details) {
            this.NotiName = Name;
            this.NotiDetails = Details;
        }

        #endregion Public Constructors

        #region Public Properties

        public String Details { get { return this.NotiDetails; } }
        public String Name { get { return this.NotiName; } }

        #endregion Public Properties
    }
}
