using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antumbra.Glow.View;

namespace Antumbra.Glow.Controller
{
    public class WhiteBalanceWindowController
    {
        private WhiteBalanceWindow view;
        public WhiteBalanceWindowController()
        {
            this.view = new WhiteBalanceWindow();
        }

        public void Show()
        {
            if (this.view == null || this.view.IsDisposed)
                this.view = new WhiteBalanceWindow();
        }
    }
}
