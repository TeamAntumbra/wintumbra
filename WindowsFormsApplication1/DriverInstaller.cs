using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Management;

namespace Antumbra
{
    class DriverInstaller
    {
        [DllImport("setupapi.dll")]
        public static extern bool SetupCopyOEMInf(
            string SourceInfFileName,
            string OEMSourceMediaLocation,
            int OEMSourceMediaType,
            int CopyStyle,
            string DestinationInfFileName,
            int DestinationInfFileNameSize,
            int RequiredSize,
            string DestinationInfFileNameComponent
            );

        [DllImport("newdev.dll")]
        public static extern bool UpdateDriverForPlugAndPlayDevices(
            IntPtr hwndParent,
            string HardwareId,
            string FullInfPath,
            uint InstallFlags,
            bool bRebootRequired
            );


        private void installDriver()
        {
            String infPath = "driver.inf";
            InstallHinfSection(IntPtr.Zero, IntPtr.Zero, infPath, 0);
            /*bool setup = SetupCopyOEMInf(infPath, null, 0, 0, null, 0, 0, null);
            Console.WriteLine("setup: " + setup);
            if (setup)
            {
                foreach (string device in getDevices())
                {
                    Console.WriteLine(UpdateDriverForPlugAndPlayDevices(IntPtr.Zero, device, infPath, 0, false));
                }
            }*/
        }
        private String[] getDevices()
        {
            List<String> devices = new List<String>(); ;
            ManagementObjectCollection collection;
            using (var searcher = new ManagementObjectSearcher(@"Select * From Win32_USBHub"))
                collection = searcher.Get();

            foreach (var device in collection) {
                devices.Add((string)device.GetPropertyValue("DeviceID"));
            }

            collection.Dispose();
            return devices.ToArray();
        }

        [DllImport("Setupapi.dll", EntryPoint = "InstallHinfSection", CallingConvention = CallingConvention.StdCall)]
        public static extern void InstallHinfSection(
            [In] IntPtr hwnd,
            [In] IntPtr ModuleHandle,
            [In, MarshalAs(UnmanagedType.LPWStr)] string CmdLineBuffer,
            int nCmdShow); 
    }
}
