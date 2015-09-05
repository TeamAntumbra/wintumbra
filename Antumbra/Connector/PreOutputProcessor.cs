using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antumbra.Glow.Observer.Colors;
using Antumbra.Glow.Observer.Configuration;
using Antumbra.Glow.Settings;
using Antumbra.Glow.Observer.Logging;

namespace Antumbra.Glow.Connector {
    public class PreOutputProcessor : ConfigurationObserver, AntumbraColorObserver, AntumbraColorSource, Loggable {
        public delegate void NewLogMsg(String source, String msg);
        public event NewLogMsg NewLogMsgAvail;
        public delegate void NewColor(Color16Bit color, int id, long index);
        public event NewColor NewColorAvailEvent;
        public bool manualMode;

        private Color16Bit Black;
        private Dictionary<int, OutputSettings> AllDeviceSettings;
        private Dictionary<int, long> OutputIndexes;
        private Dictionary<int, Color16Bit> Colors;

        public PreOutputProcessor() {
            manualMode = false;
            AttachObserver(LoggerHelper.GetInstance());
            AllDeviceSettings = new Dictionary<int, OutputSettings>();
            OutputIndexes = new Dictionary<int, long>();
            Colors = new Dictionary<int, Color16Bit>();
            Black.red = 0;
            Black.green = 0;
            Black.blue = 0;
        }

        public void ConfigurationUpdate(Configurable config) {
            if(config is DeviceSettings) {
                DeviceSettings settings = (DeviceSettings)config;

                OutputSettings devSettings;
                devSettings.MaxBrightness = settings.maxBrightness;
                devSettings.redBias = Convert.ToInt16(settings.redBias << 8);
                devSettings.greenBias = Convert.ToInt16(settings.greenBias << 8);
                devSettings.blueBias = Convert.ToInt16(settings.blueBias << 8);
                var avgBias = (Math.Abs(settings.redBias) + Math.Abs(settings.greenBias) + Math.Abs(settings.blueBias) / 3);
                devSettings.whiteBalanceMin = Convert.ToUInt16(avgBias);
                devSettings.weightingEnabled = settings.weightingEnabled;
                devSettings.newColorWeight = settings.newColorWeight;

                AllDeviceSettings[settings.id] = devSettings;
            }
        }

        public void NewColorAvail(Color16Bit newCol, int id, long index) {
            OutputSettings settings;
            if(!AllDeviceSettings.TryGetValue(id, out settings)) {
                Log("OutputSettings for device not found! Color sent plain. ID: " + id);
                AnnounceColor(newCol, id, index);
                return;
            }

            if(index == long.MinValue) {
                OutputIndexes.Remove(id);
            }

            long prevIndex;
            if(!OutputIndexes.TryGetValue(id, out prevIndex)) {
                OutputIndexes[id] = index;
            } else if(prevIndex >= index) {
                // Invalid index
                Log("Color recieved out of order! Color BLOCKED! Target ID: " + id +
                    " with index " + index + " and last index " + prevIndex);
                return;
            }

            if(Color16BitUtil.GetAvgBrightness(newCol) < 50) {
                AnnounceColor(Black, id, index);
            }

            // Either first run or valid index
            int red = newCol.red;
            int green = newCol.green;
            int blue = newCol.blue;
            // White balance
            if(Color16BitUtil.GetAvgBrightness(newCol) > settings.whiteBalanceMin) {
                red += settings.redBias;
                green += settings.greenBias;
                blue += settings.blueBias;
            }
            newCol = Color16BitUtil.FunnelIntoColor(red, green, blue);

            // Scale brightness
            try {
                newCol = Color16BitUtil.ScaleColor(newCol, settings.MaxBrightness);
            } catch(ArgumentException ex) {
                Log(ex.Message + '\n' + ex.StackTrace);
            }

            // Add to weighted average
            if(settings.weightingEnabled && !manualMode) {
                Color16Bit prev;
                if(Colors.TryGetValue(id, out prev)) {
                    newCol = Utility.Mixer.MixColorPercIn(newCol, prev, settings.newColorWeight);
                }
            }

            AnnounceColor(newCol, id, index);
        }

        private void AnnounceColor(Color16Bit color, int id, long index) {
            if(NewColorAvailEvent != null) {
                Colors[id] = color;
                NewColorAvailEvent(color, id, index);
            }
        }

        public void AttachObserver(LogMsgObserver observer) {
            NewLogMsgAvail += observer.NewLogMsgAvail;
        }

        public void AttachObserver(AntumbraColorObserver observer) {
            NewColorAvailEvent += observer.NewColorAvail;
        }

        private void Log(String msg) {
            if(NewLogMsgAvail != null) {
                NewLogMsgAvail("PreOutputProcessor", msg);
            }
        }

        private struct OutputSettings {
            public double MaxBrightness;
            public Int16 redBias, greenBias, blueBias;
            public UInt16 whiteBalanceMin;
            public bool weightingEnabled;
            public double newColorWeight;
        }
    }
}
