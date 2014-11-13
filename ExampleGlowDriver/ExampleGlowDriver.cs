using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Antumbra.Glow.ExtensionFramework;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;//might not be needed TODO
using System.Threading;

namespace ExampleGlowDriver
{
    [Export(typeof(GlowExtension))]
    public class ExampleGlowDriver : GlowIndependentDriver
    {
        //public event EventHandler NewColor;//occurs when a new color is available
        private Thread driver;
        private List<IObserver<Color>> observers;

        public ExampleGlowDriver()
        {
            this.observers = new List<IObserver<Color>>();
        }

        public override IDisposable Subscribe(IObserver<Color> observer)
        {
            if (!this.observers.Contains(observer))
                this.observers.Add(observer);
            return new Unsubscriber(this.observers, observer);
        }

        private class Unsubscriber : IDisposable
        {
            private List<IObserver<Color>> _observers;
            private IObserver<Color> _observer;

            public Unsubscriber(List<IObserver<Color>> observers, IObserver<Color> observer)
            {
                this._observers = observers;
                this._observer = observer;
            }

            public void Dispose()
            {
                if (_observer != null && _observers.Contains(_observer))
                    _observers.Remove(_observer);
            }
        }

        public override bool start()
        {
        /*    EventHandler handler = NewColor;
            if (handler != null) {
                handler(this, EventArgs.Empty);
            }
            return true;*/
            this.driver = new Thread(new ThreadStart(threadLogic));
            this.driver.Start();
            return true;
        }

        private void threadLogic()
        {
            Random rnd = new Random();
            while (true) {
                //do stuff (logic of driver)
                Color result = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
                //report exceptions with .OnError(Exception)
                //report new color to observers
                foreach (var observer in this.observers) {
                    observer.OnNext(result);
                }
            }
        }

        public bool stop()
        {
            foreach (var observer in observers.ToArray())
                if (observers.Contains(observer))
                    observer.OnCompleted();
            observers.Clear();
            this.driver.Abort();
            this.driver = null;
            return true;
        }

        public override string Name
        {
            get { return "Example Glow Driver"; }
        }

        public override string Author
        {
            get { return "Team Antumbra"; }
        }

        public override string Version
        {
            get { return "V0.1.0"; }
        }

        public override string Description
        {
            get { return "A super simple implementation example " +
                         "of a Glow Driver extension that always " +
                         "returns Random Colors! :)"; }
        }
    }
}
