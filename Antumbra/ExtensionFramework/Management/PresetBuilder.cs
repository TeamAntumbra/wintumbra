using Antumbra.Glow.Settings;
using System;

namespace Antumbra.Glow.ExtensionFramework.Management {

    public class PresetBuilder {

        #region Private Fields

        private readonly Guid[] Empty = new Guid[0];
        private Guid Brightener = Guid.Parse("1a271e63-5f7e-43c0-bbb1-7d80d23d8db7");
        private Guid HSVDriver = Guid.Parse("8360550b-d599-4f0f-8806-bc323f9ce547");
        private ExtensionLibrary lib;
        private Guid NeonDriver = Guid.Parse("9a310fae-2084-4dc5-ae6a-4f664faa1fe8");
        private Guid Saturator = Guid.Parse("2acba4a6-af21-47a9-9551-964a750fea06");
        private Guid ScreenDriverCoupler = Guid.Parse("70987576-1a00-4a34-b787-4c08516cd1b8");
        private Guid ScreenshotProcesor = Guid.Parse("3eea8b48-82e3-4db4-a04a-2b9865929993");
        private Guid SinDriver = Guid.Parse("31cae25b-72c0-4ffc-860b-234fb931bc15");
        private Guid SlimDXGrabber = Guid.Parse("ad1e6255-d9b4-4e6d-995f-a094e6ea5f7b");

        #endregion Private Fields

        #region Public Constructors

        public PresetBuilder(ExtensionLibrary lib) {
            this.lib = lib;
        }

        #endregion Public Constructors

        #region Public Methods

        public ActiveExtensions GetAugmentMirrorPreset() {
            Guid[] processors = { ScreenshotProcesor };
            Guid[] filters = { Saturator, Brightener };
            ActiveExtensions result = new ActiveExtensions(ScreenDriverCoupler, SlimDXGrabber, processors, filters, Empty);
            result.Init(lib);
            return result;
        }

        public ActiveExtensions GetHSVFadePreset() {
            ActiveExtensions result = new ActiveExtensions(HSVDriver, Guid.Empty, Empty, Empty, Empty);
            result.Init(lib);
            return result;
        }

        public ActiveExtensions GetMirrorPreset() {
            Guid[] processors = { ScreenshotProcesor };
            ActiveExtensions result = new ActiveExtensions(ScreenDriverCoupler, SlimDXGrabber, processors, Empty, Empty);
            result.Init(lib);
            return result;
        }

        public ActiveExtensions GetNeonFadePreset() {
            ActiveExtensions result = new ActiveExtensions(NeonDriver, Guid.Empty, Empty, Empty, Empty);
            result.Init(lib);
            return result;
        }

        public ActiveExtensions GetSinFadePreset() {
            ActiveExtensions result = new ActiveExtensions(SinDriver, Guid.Empty, Empty, Empty, Empty);
            result.Init(lib);
            return result;
        }

        #endregion Public Methods
    }
}
