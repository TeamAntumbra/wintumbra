using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.InteropServices;
using System.IO;

namespace Antumbra.Glow.Connector
{
    class SerialConnector
    {
        private int pid, vid, err, outndevs;
        private IntPtr ctx, devs;
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct LightInfo
        {
            byte endpoint;
        }
        public SerialConnector(int vid, int pid)
        {
            this.ctx = IntPtr.Zero;
            this.pid = pid;
            this.vid = vid;
            this.ctx = AnCtx_InitReturn(out err);
            UpdateDeviceList();
        }

        public IntPtr GetDeviceInfo(int index)
        {
            return AnDevice_IndexOpaqueList(this.devs, index);
        }

        public IntPtr OpenDevice(IntPtr info, out int outerr)
        {
            return AnDevice_OpenReturn(this.ctx, info, out outerr);
        }

        public int UpdateDeviceList()
        {
            this.devs = AnDevice_GetOpaqueList(this.ctx, out outndevs, out err);
            return outndevs;
        }

        public int SetDeviceColor(int index, IntPtr dev, byte r, byte g, byte b)
        {
            LightInfo info;
            AnLight_Info_S(this.ctx, dev, out info);
            byte[] rArray = {r};
            UInt16 red = (UInt16)((r / 255.0) * UInt16.MaxValue);//convert to UInt16s
            UInt16 green = (UInt16)((g / 255.0) * UInt16.MaxValue);
            UInt16 blue = (UInt16)((b / 255.0) * UInt16.MaxValue);
            return AnLight_Set_S(this.ctx, dev, out info, red, green, blue);
        }

        public void CloseDevice(IntPtr dev)
        {
            AnDevice_Close(this.ctx, dev);
        }

        public void FreeList()
        {
            AnDevice_FreeOpaqueList(this.devs);
        }

        [DllImport("libantumbra.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr AnCtx_InitReturn(out int outerr);
        [DllImport("libantumbra.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr AnDevice_GetOpaqueList(IntPtr ctx, out int outndevs, out int outerr);
        [DllImport("libantumbra.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr AnDevice_OpenReturn(IntPtr ctx, IntPtr info, out int err);
        [DllImport("libantumbra.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr AnDevice_IndexOpaqueList(IntPtr list, int index);
        [DllImport("libantumbra.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int AnLight_Set_S(IntPtr ctx, IntPtr dev, out LightInfo info,
                             UInt16 r, UInt16 g, UInt16 b);
        [DllImport("libantumbra.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AnDevice_FreeOpaqueList(IntPtr devs);
        [DllImport("libantumbra.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AnDevice_Close(IntPtr ctx, IntPtr dev);
        [DllImport("libantumbra.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int AnLight_Info_S(IntPtr ctx, IntPtr dev, out LightInfo info);

    }
}
