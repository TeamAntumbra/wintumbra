using Antumbra.Glow.Observer.Colors;
using Antumbra.Glow.Observer.Configuration;
using Antumbra.Glow.Observer.Logging;
using Antumbra.Glow.Settings;
using System;
using System.Collections.Generic;

namespace Antumbra.Glow.Connector {

    public class PreOutputProcessor : ConfigurationObserver, AntumbraColorObserver, AntumbraColorSource, Loggable {

        #region Public Fields

        public bool manualMode;

        #endregion Public Fields

        #region Private Fields

        private Dictionary<int, OutputSettings> AllDeviceSettings;

        private Color16Bit Black;

        private Dictionary<int, Color16Bit> Colors;

        private Dictionary<int, long> OutputIndexes;

        #endregion Private Fields

        #region Public Constructors

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

        #endregion Public Constructors

        #region Public Delegates

        public delegate void NewColor(Color16Bit color, int id, long index);

        public delegate void NewLogMsg(String source, String msg);

        #endregion Public Delegates

        #region Public Events

        public event NewColor NewColorAvailEvent;

        public event NewLogMsg NewLogMsgAvail;

        #endregion Public Events

        #region Public Methods

        public void AttachObserver(LogMsgObserver observer) {
            NewLogMsgAvail += observer.NewLogMsgAvail;
        }

        public void AttachObserver(AntumbraColorObserver observer) {
            NewColorAvailEvent += observer.NewColorAvail;
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

        #endregion Public Methods

        #region Private Methods

        private void AnnounceColor(Color16Bit color, int id, long index) {
            if(NewColorAvailEvent != null) {
                Colors[id] = color;
                NewColorAvailEvent(color, id, index);
            }
        }

        private void Log(String msg) {
            if(NewLogMsgAvail != null) {
                NewLogMsgAvail("PreOutputProcessor", msg);
            }
        }

        #endregion Private Methods

        #region Private Structs

        private struct OutputSettings {

            #region Public Fields

            public double MaxBrightness;
            public double newColorWeight;
            public Int16 redBias, greenBias, blueBias;
            public bool weightingEnabled;
            public UInt16 whiteBalanceMin;

            #endregion Public Fields
        }

        #endregion Private Structs
    }
}
