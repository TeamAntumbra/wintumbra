using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Collections;
using System.Drawing;
using Antumbra.Glow.ExtensionFramework.Types;
using Antumbra.Glow.Observer.Logging;

namespace Antumbra.Glow.ExtensionFramework.Management {
    public class MEFHelper : Loggable {
        public delegate void NewLogMsg(string source, string msg);
        public event NewLogMsg NewLogMsgAvail;

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

        public MEFHelper() {
            AttachObserver(LoggerHelper.GetInstance());
        }

        public Dictionary<Type, List<GlowExtension>> LoadExtensions() {
            Log("Extension Refresh triggered.");
            Dictionary<Type, List<GlowExtension>> ExtensionBank = new Dictionary<Type, List<GlowExtension>>();
            foreach(Type extType in Types) {
                ExtensionBank[extType] = new List<GlowExtension>();
            }
            //add coupler placeholder TODO investigate if needed
            ExtensionBank[typeof(GlowDriver)].Add(new GlowScreenDriverCoupler(null, null));

            DirectoryCatalog catalog = new DirectoryCatalog(EXTENSION_DIR_REL_PATH, "*.glow.dll");
            container = new CompositionContainer(catalog);
            container.ComposeParts(this);
            container.Dispose();

            if(null == FullList) {
                throw new Exception("Loading extensions failed.  extensions == null");//no plugins loaded
            }

            Log("Extension Refresh complete.\nThe Following GlowExtensions were found:\n");

            foreach(GlowExtension extension in FullList) {
                Type type = extension.GetExtensionType();
                Log(type + extension.ToString());
                ExtensionBank[type].Add(extension);
            }

            ExtensionBank[typeof(GlowExtension)] = FullList;
            return ExtensionBank;
        }

        public void AttachObserver(LogMsgObserver observer) {
            NewLogMsgAvail += observer.NewLogMsgAvail;
        }

        private void Log(string msg) {
            if(NewLogMsgAvail != null) {
                NewLogMsgAvail("MEFHelper", msg);
            }
        }
    }
}
