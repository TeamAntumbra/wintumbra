using Antumbra.Glow.ExtensionFramework.Types;
using Antumbra.Glow.Observer.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace Antumbra.Glow.ExtensionFramework.Management {

    public class MEFHelper : Loggable {

        #region Private Fields

        private const string EXTENSION_DIR_REL_PATH = "./Extensions/";

        private readonly Type[] Types = {
                                            typeof(GlowDriver),
                                            typeof(GlowScreenGrabber),
                                            typeof(GlowScreenProcessor),
                                            typeof(GlowFilter),
                                            typeof(GlowNotifier)
                                        };

        private CompositionContainer container;

        [ImportMany]
        private List<GlowExtension> FullList;

        #endregion Private Fields

        #region Public Constructors

        public MEFHelper() {
            AttachObserver(LoggerHelper.GetInstance());
        }

        #endregion Public Constructors

        #region Public Delegates

        public delegate void NewLogMsg(string source, string msg);

        #endregion Public Delegates

        #region Public Events

        public event NewLogMsg NewLogMsgAvail;

        #endregion Public Events

        #region Public Methods

        public void AttachObserver(LogMsgObserver observer) {
            NewLogMsgAvail += observer.NewLogMsgAvail;
        }

        public Dictionary<Type, List<GlowExtension>> LoadExtensions() {
            Log("Extension Refresh triggered.");
            Dictionary<Type, List<GlowExtension>> ExtensionBank = new Dictionary<Type, List<GlowExtension>>();
            foreach(Type extType in Types) {
                ExtensionBank[extType] = new List<GlowExtension>();
            }
            //add coupler placeholder TODO investigate if needed
            ExtensionBank[typeof(GlowDriver)].Add(new GlowScreenDriverCoupler(null, null));

            DirectoryCatalog catalog;
            try {
                catalog = new DirectoryCatalog(EXTENSION_DIR_REL_PATH, "*.glow.dll");
            } catch(System.IO.DirectoryNotFoundException) {
                System.IO.Directory.CreateDirectory(EXTENSION_DIR_REL_PATH);
                catalog = new DirectoryCatalog(EXTENSION_DIR_REL_PATH, "*.glow.dll");
            }
            try {
                container = new CompositionContainer(catalog);
                container.ComposeParts(this);
                container.Dispose();
            } catch(Exception ex) {
                Log(ex.Message + '\n' + ex.StackTrace);
            }

            if(null == FullList) {
                Log("Loading extensions failed.  extensions == null");//no plugins loaded
            } else {
                Log("Extension Refresh complete.\nThe Following GlowExtensions were found:\n");

                foreach(GlowExtension extension in FullList) {
                    Type type = extension.GetExtensionType();
                    Log(type + extension.ToString());
                    ExtensionBank[type].Add(extension);
                }
                ExtensionBank[typeof(GlowExtension)] = FullList;
            }

            return ExtensionBank;
        }

        #endregion Public Methods

        #region Private Methods

        private void Log(string msg) {
            if(NewLogMsgAvail != null) {
                NewLogMsgAvail("MEFHelper", msg);
            }
        }

        #endregion Private Methods
    }
}
