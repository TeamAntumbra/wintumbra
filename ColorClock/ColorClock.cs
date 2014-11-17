using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antumbra.Glow.ExtensionFramework;
using System.Drawing;
using System.Threading;
using System.ComponentModel.Composition;

namespace ColorClock
{
    [Export(typeof(GlowExtension))]
    public class ColorClock : GlowDriver
    {
        public delegate void NewColorAvail(object sender, EventArgs args);
        public event NewColorAvail NewColorAvailEvent;
        private Thread driver;
        public override string Name
        {
            get { return "Color Clock"; }
        }

        public override string Author
        {
            get { return "Team Antumbra"; }
        }

        public override string Description
        {
            get { return "A time based color clock with unique colors "
                       + "for every second of the day."; }
        }

        public override string Version
        {
            get { return "V0.0.1"; }
        }

        public override void AttachEvent(AntumbraColorObserver observer)
        {
            this.NewColorAvailEvent += new NewColorAvail(observer.NewColorAvail);
        }

        public override bool ready()
        {
            this.driver = new Thread(new ThreadStart(driverTarget));
            return true;
        }

        public override bool start()
        {
            this.driver.Start();
            return true;
        }

        private void driverTarget()
        {
            while (true) {
                if (NewColorAvailEvent == null) {}//no one is listening, do nothing...
                else
                    NewColorAvailEvent(getTimeColor(DateTime.Now), EventArgs.Empty);
            }
        }

        private Color getTimeColor(DateTime time)
        {
            double secondsFromStart = (time - DateTime.Today).TotalSeconds;//0-86400
            if (secondsFromStart < 10800) {//000 to 100
                double fraction = secondsFromStart / 10800.0;//0-1 in this section
                return Color.FromArgb((int)(fraction * 255), 0, 0);
            }
            else if (secondsFromStart < 21600) {//100 to 010
                double fraction = (secondsFromStart-10800) / 10800.0;//0-1 in this section
                return Color.FromArgb((int)((1 - fraction) * 255), (int)(255*fraction),0);
            }
            else if (secondsFromStart < 32400) {//010 to 110
                double fraction = (secondsFromStart - 21600) / 10800.0;//0-1 in this section
                return Color.FromArgb((int)(255 * fraction), 255, 0);
            }
            else if (secondsFromStart < 43200) {//110 to 111
                double fraction = (secondsFromStart - 32400) / 10800.0;//0-1 in this section
                return Color.FromArgb(255, 255, (int)(255 * fraction));
            }
            else if (secondsFromStart < 54000) {//111 to 011
                double fraction = (secondsFromStart - 43200) / 10800.0;//0-1 in this section
                return Color.FromArgb((int)((1 - fraction) * 255), 255, 255);
            }
            else if (secondsFromStart < 64800) {//011 to 001
                double fraction = (secondsFromStart - 54000) / 10800.0;//0-1 in this section
                return Color.FromArgb(0,(int)((1-fraction)*255), (int)(255*fraction));
            }
            else if (secondsFromStart < 75600) {//001 to 101
                double fraction = (secondsFromStart - 64800) / 10800.0;//0-1 in this section
                return Color.FromArgb((int)((1 - fraction) * 255), 0, 255);
            }
            else {//101 to 000
                double fraction = secondsFromStart / 86400.0;
                return Color.FromArgb((int)((1 - fraction) * 255), 0, (int)((1 - fraction) * 255));
            }
        }
    }
}
