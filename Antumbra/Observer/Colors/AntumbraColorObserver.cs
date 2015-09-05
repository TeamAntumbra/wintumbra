namespace Antumbra.Glow.Observer.Colors {

    public interface AntumbraColorObserver {

        #region Public Methods

        void NewColorAvail(Color16Bit newCol, int id, long index);

        #endregion Public Methods
    }
}
