using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Windows.Forms;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Antumbra.Glow.ExtensionFramework
{
    /// <summary>
    /// Abstract definition of a Glow Extension
    /// </summary>
    public abstract class GlowExtension : IGlowExtension, IDisposable
    {
        public override string ToString()
        {
            return this.Name + " - " + this.Version.ToString();
        }

        /// <summary>
        /// The id of the current extension
        /// </summary>
        [ReadOnly(true)]
        public abstract Guid id { get; }

        /// <summary>
        /// The id of the device associated with this extension
        /// </summary>
        [ReadOnly(true)]
        public abstract int devId { get; }

        /// <summary>
        /// The name of the current extension
        /// </summary>
        [ReadOnly(true)]
        public abstract string Name { get; }
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
        /// A website for further information
        /// </summary>
        [ReadOnly(true)]
        public abstract string Website { get; }
        /// <summary>
        /// The version of this extension
        /// </summary>
        [ReadOnly(true)]
        public abstract Version Version { get; }

        /// <summary>
        /// The running status of this extension
        /// </summary>
        [ReadOnly(true)]
        public abstract bool IsRunning { get; }

        /// <summary>
        /// Returns true if the extension is a default, else false.
        /// </summary>
        [ReadOnly(true)]
        public abstract bool IsDefault { get; }

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

        /// <summary>
        /// Try to open the settings window for this extension
        /// </summary>
        /// <returns>true if window opened, else false</returns>
        public abstract bool Settings();

        /// <summary>
        /// Create instance for use
        /// </summary>
        /// <returns></returns>
        public abstract GlowExtension Create();//TODO move this into inner factory object for MEF/Library use rather than holding an entire copy

        public abstract void Dispose();
    }
}
