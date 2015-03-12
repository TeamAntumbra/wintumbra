using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antumbra.Glow.Exceptions
{
    class ExtensionNotFoundException : Exception
    {
        public ExtensionNotFoundException(Guid id) :
            base("The extension with GUID: " + id.ToString() + " was not found.")
        {
            //do nothing, use parent constructor
        }
    }
}
