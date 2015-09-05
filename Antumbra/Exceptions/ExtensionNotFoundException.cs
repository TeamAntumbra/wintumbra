using System;

namespace Antumbra.Glow.Exceptions {

    internal class ExtensionNotFoundException : Exception {

        #region Public Constructors

        public ExtensionNotFoundException(Guid id) :
            base("The extension with GUID: " + id.ToString() + " was not found.") {
            //do nothing, use parent constructor
        }

        #endregion Public Constructors
    }
}
