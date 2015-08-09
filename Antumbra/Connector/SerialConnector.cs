﻿using System;
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

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="vid"></param>
        /// <param name="pid"></param>
        public SerialConnector(int vid, int pid)
        {
            this.ctx = IntPtr.Zero;
            this.pid = pid;
            this.vid = vid;
            this.ctx = AnCtx_InitReturn(out err);
            UpdateDeviceList();
        }

        /// <summary>
        /// Get the device info pointer for a specified index
        /// </summary>
        /// <param name="index">The Glow's index</param>
        /// <returns></returns>
        public IntPtr GetDeviceInfo(int index)
        {
            return AnDevice_IndexOpaqueList(this.devs, index);
        }

        /// <summary>
        /// Open a Glow device connection
        /// </summary>
        /// <param name="info"></param>
        /// <param name="outerr"></param>
        /// <returns></returns>
        public IntPtr OpenDevice(IntPtr info, out int outerr)
        {
            return AnDevice_OpenReturn(this.ctx, info, out outerr);
        }

        /// <summary>
        /// Update device list
        /// </summary>
        /// <returns></returns>
        public int UpdateDeviceList()
        {
            this.devs = AnDevice_GetOpaqueList(this.ctx, out outndevs, out err);
            return outndevs;
        }

        /// <summary>
        /// Send a color set command to a physical Glow device
        /// </summary>
        /// <param name="index"></param>
        /// <param name="dev"></param>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <returns> (See error.c in libantumbra repo)
        /// 0 - Success
        /// 1 - Disconnected
        /// 2 - Memory allocation failed
        /// 3 - libusb error
        /// 4 - Device in wrong state for operation
        /// 5 - Value out of range
        /// 6 - Command unsupported
        /// 7 - Command failure
        /// 8 - Unspecified protocol error
        /// </returns>
        public int SetDeviceColor(int index, IntPtr dev, UInt16 r, UInt16 g, UInt16 b)
        {
            LightInfo info;
            AnLight_Info_S(this.ctx, dev, out info);
            return AnLight_Set_S(this.ctx, dev, out info, r, g, b);
        }

        /// <summary>
        /// Close a Glow device connection
        /// </summary>
        /// <param name="dev">Device pointer</param>
        public void CloseDevice(IntPtr dev)
        {
            AnDevice_Close(this.ctx, dev);
        }

        /// <summary>
        /// Free the list of devices
        /// </summary>
        public void FreeList()
        {
            AnDevice_FreeOpaqueList(this.devs);
        }

        /// <summary>
        /// Free context
        /// </summary>
        public void FreeCtx()
        {
            AnCtx_Deinit(this.ctx);
        }

        [DllImport("libantumbra.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AnCtx_Deinit(IntPtr ctx);
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
