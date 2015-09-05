using System;

namespace Antumbra.Glow.ExtensionFramework {

    /// <summary>
    /// Interface for any GlowExtension
    /// </summary>
    public interface IGlowExtension {

        #region Public Properties

        /// <summary>
        /// The author(s) of the extension.
        /// </summary>
        string Author { get; }

        /// <summary>
        /// A description of the extension.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// The id for the device this extension is associated with
        /// </summary>
        int devId { get; set; }

        /// <summary>
        /// The id for the extension. Assigned at runtime.
        /// </summary>
        Guid id { get; }

        /// <summary>
        /// Returns true if the extension is a default, else false.
        /// </summary>
        bool IsDefault { get; }

        /// <summary>
        /// The running status of the extension.
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// The name of the extension.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The version of the extension.
        /// </summary>
        Version Version { get; }

        /// <summary>
        /// A website for information regarding the extension.
        /// </summary>
        string Website { get; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Get the Type of extension this is
        /// </summary>
        /// <returns>The extension Type</returns>
        Type GetExtensionType();

        /// <summary>
        /// Try to open the settings window for this extension
        /// </summary>
        /// <returns>return true if window opened, else false</returns>
        bool Settings();

        /// <summary>
        /// Start this Extension
        /// </summary>
        bool Start();

        /// <summary>
        /// Stop/clean up after this Extension
        /// </summary>
        bool Stop();

        #endregion Public Methods
    }
}
