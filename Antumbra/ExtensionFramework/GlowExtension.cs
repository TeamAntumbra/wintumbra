using System;
using System.ComponentModel;

namespace Antumbra.Glow.ExtensionFramework {

    /// <summary>
    /// Abstract definition of a Glow Extension
    /// </summary>
    public abstract class GlowExtension : IGlowExtension, IDisposable {

        #region Public Properties

        /// <summary>
        /// The author of this extension
        /// </summary>
        [ReadOnly(true)]
        public abstract string Author { get; }

        /// <summary>
        /// A description of this extension
        /// </summary>
        [ReadOnly(true)]
        public abstract string Description { get; }

        /// <summary>
        /// The id of the device associated with this extension
        /// </summary>
        public abstract int devId { get; set; }

        /// <summary>
        /// The id of the current extension
        /// </summary>
        [ReadOnly(true)]
        public abstract Guid id { get; }

        /// <summary>
        /// Returns true if the extension is a default, else false.
        /// </summary>
        [ReadOnly(true)]
        public abstract bool IsDefault { get; }

        /// <summary>
        /// The running status of this extension
        /// </summary>
        [ReadOnly(true)]
        public abstract bool IsRunning { get; }

        /// <summary>
        /// The name of the current extension
        /// </summary>
        [ReadOnly(true)]
        public abstract string Name { get; }

        /// <summary>
        /// The version of this extension
        /// </summary>
        [ReadOnly(true)]
        public abstract Version Version { get; }

        /// <summary>
        /// A website for further information
        /// </summary>
        [ReadOnly(true)]
        public abstract string Website { get; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Clean up any used resources
        /// </summary>
        public abstract void Dispose();

        /// <summary>
        /// Get the Type of this extension
        /// </summary>
        /// <returns>The Type of this extension</returns>
        public abstract Type GetExtensionType();

        /// <summary>
        /// Try to open the settings window for this extension
        /// </summary>
        /// <returns>true if window opened, else false</returns>
        public abstract bool Settings();

        /// <summary>
        /// Start this extension
        /// </summary>
        /// <returns>true if success, else false</returns>
        public abstract bool Start();

        /// <summary>
        /// Stop this extension, clean up
        /// </summary>
        /// <returns>true if success, else false</returns>
        public abstract bool Stop();

        public override string ToString() {
            return this.Name + " - " + this.Version.ToString();
        }

        #endregion Public Methods
    }
}
