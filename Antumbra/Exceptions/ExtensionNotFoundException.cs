using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antumbra.Glow.Exceptions
{
    class ExtensionNotFoundException : Exception
    {
        public ExtensionNotFoundException(String extensionName) :
            base("The extension " + extensionName + " was not found.")
        {
            //do nothing, use parent constructor
        }
    }
}
