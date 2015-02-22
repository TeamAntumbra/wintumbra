using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antumbra.Glow.Connector
{
    class OutputLoopManager
    {
        private List<OutputLoop> loops;
        public OutputLoopManager() {
            this.loops = new List<OutputLoop>();
        }

        public OutputLoop CreateAndAddLoop(DeviceManager mgr, int id)
        {
            var loop = new OutputLoop(mgr, id);
            this.loops.Add(loop);
            return loop;
        }

        public OutputLoop FindLoopOrReturnNull(int id) {
            OutputLoop result = null;
            foreach (var loop in loops)
                if (loop.id == id)
                    result = loop;
            return result;
        }

        public string GetSpeedsStr()
        {
            string result = "";
            if (loops.Count == 0)
                result = "No output loops found.";
            else
                foreach (var loop in loops)
                    result += "ID: " + loop.id + " - " + Math.Round(loop.FPS, 3) + " hz.\n";
            return result;
        }
    }
}
