using System;
using System.Collections.Generic;

namespace Antumbra.Glow.Observer.ScreenInfo {

    public interface AntumbraScreenInfoObserver {

        #region Public Methods

        void NewScreenInfoAvail(List<int[, ,]> pixelArrays, EventArgs args);

        #endregion Public Methods
    }
}
