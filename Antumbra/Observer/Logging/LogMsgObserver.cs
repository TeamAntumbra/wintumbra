using System;

namespace Antumbra.Glow.Observer.Logging {

    public interface LogMsgObserver {

        #region Public Methods

        void NewLogMsgAvail(String sourceName, String msg);

        #endregion Public Methods
    }
}
