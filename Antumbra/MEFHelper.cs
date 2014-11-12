using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Antumbra.Glow.ExtensionFramework;

namespace Antumbra.Glow
{
    class MEFHelper
    {
        private CompositionContainer container;
        private String path;

        [ImportMany]
        private IEnumerable<GlowExtension> plugins = null;

        public MEFHelper(String pathToExtensions)
        {
            this.path = pathToExtensions;
            Compose();
            foreach (var plugin in plugins)
                Console.WriteLine("Plugin Found: " + plugin.Name);
        }

        private void Compose()
        {
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new DirectoryCatalog(this.path));
            catalog.Catalogs.Add(new AssemblyCatalog(System.Reflection.Assembly.GetExecutingAssembly()));
            this.container = new CompositionContainer(catalog);
            this.container.SatisfyImportsOnce(this);//get rid of later on
        }
    }
}
