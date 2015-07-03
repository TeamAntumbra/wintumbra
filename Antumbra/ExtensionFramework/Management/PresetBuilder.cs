using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antumbra.Glow.Settings;
using Antumbra.Glow.ExtensionFramework.Types;

namespace Antumbra.Glow.ExtensionFramework.Management
{
    public class PresetBuilder
    {
        private ExtensionLibrary lib;
        private Guid HSVDriver = Guid.Parse("8360550b-d599-4f0f-8806-bc323f9ce547");
        private Guid SinDriver = Guid.Parse("31cae25b-72c0-4ffc-860b-234fb931bc15");
        private Guid NeonDriver = Guid.Parse("9a310fae-2084-4dc5-ae6a-4f664faa1fe8");
        private Guid ScreenDriverCoupler = Guid.Parse("70987576-1a00-4a34-b787-4c08516cd1b8");
        private Guid ScreenGrabber = Guid.Parse("15115e91-ed5c-49e6-b7a8-4ebbd4dabb2e");
        private Guid ScreenshotProcesor = Guid.Parse("3eea8b48-82e3-4db4-a04a-2b9865929993");
        private Guid DXGrabber = Guid.Parse("ad1e6255-d9b4-4e6d-995f-a094e6ea5f7b");
        private Guid Saturator = Guid.Parse("2acba4a6-af21-47a9-9551-964a750fea06");
        private Guid Brightener = Guid.Parse("1a271e63-5f7e-43c0-bbb1-7d80d23d8db7");
        public PresetBuilder(ExtensionLibrary lib)
        {
            this.lib = lib;
        }

        public ActiveExtensions GetHSVFadePreset()
        {
            ActiveExtensions result = new ActiveExtensions();
            GlowExtension ext = this.lib.LookupExt(HSVDriver);
            if (ext != null)
                result.ActiveDriver = (GlowDriver)ext;
            return result;
        }

        public ActiveExtensions GetSinFadePreset()
        {
            ActiveExtensions result = new ActiveExtensions();
            GlowExtension ext = this.lib.LookupExt(SinDriver);
            if (ext != null)
                result.ActiveDriver = (GlowDriver)ext;
            return result;
        }

        public ActiveExtensions GetNeonFadePreset()
        {
            ActiveExtensions result = new ActiveExtensions();
            GlowExtension ext = this.lib.LookupExt(NeonDriver);
            if (ext != null)
                result.ActiveDriver = (GlowDriver)ext;
            return result;
        }

        public ActiveExtensions GetMirrorPreset()
        {
            ActiveExtensions result = new ActiveExtensions();
            GlowExtension ext = this.lib.LookupExt(ScreenDriverCoupler);
            if (ext != null)
                result.ActiveDriver = (GlowDriver)ext;
            ext = this.lib.LookupExt(ScreenGrabber);
            if (ext != null)
                result.ActiveGrabber = (GlowScreenGrabber)ext;
            ext = this.lib.LookupExt(ScreenshotProcesor);
            if (ext != null)
                result.ActiveProcessor = (GlowScreenProcessor)ext;
            return result;
        }

        public ActiveExtensions GetSmoothMirrorPreset()
        {
            ActiveExtensions result = new ActiveExtensions();
            GlowExtension ext = this.lib.LookupExt(ScreenDriverCoupler);
            if (ext != null)
                result.ActiveDriver = (GlowDriver)ext;
            ext = this.lib.LookupExt(ScreenGrabber);
            if (ext != null)
                result.ActiveGrabber = (GlowScreenGrabber)ext;
            ext = this.lib.LookupExt(ScreenshotProcesor);
            if (ext != null)
                result.ActiveProcessor = (GlowScreenProcessor)ext;
            return result;
        }

        public ActiveExtensions GetAugmentMirrorPreset()
        {
            ActiveExtensions result = new ActiveExtensions();
            GlowExtension ext = this.lib.LookupExt(ScreenDriverCoupler);
            if (ext != null)
                result.ActiveDriver = (GlowDriver)ext;
            ext = this.lib.LookupExt(ScreenGrabber);
            if (ext != null)
                result.ActiveGrabber = (GlowScreenGrabber)ext;
            ext = this.lib.LookupExt(ScreenshotProcesor);
            if (ext != null)
                result.ActiveProcessor = (GlowScreenProcessor)ext;
            ext = this.lib.LookupExt(Saturator);
            if (ext != null)
                result.ActiveFilters.Add((GlowFilter)ext);
            return result;
        }

        public ActiveExtensions GetGameMirrorPreset()
        {
            ActiveExtensions result = new ActiveExtensions();
            GlowExtension ext = this.lib.LookupExt(ScreenDriverCoupler);
            if (ext != null)
                result.ActiveDriver = (GlowDriver)ext;
            ext = this.lib.LookupExt(ScreenshotProcesor);
            if (ext != null)
                result.ActiveProcessor = (GlowScreenProcessor)ext;
            ext = this.lib.LookupExt(DXGrabber);
            if (ext != null)
                result.ActiveGrabber = (GlowScreenGrabber)ext;
            return result;
        }
    }
}
