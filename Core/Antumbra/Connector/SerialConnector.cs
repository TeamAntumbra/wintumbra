using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.InteropServices;

namespace Antumbra.Glow.Connector
{
    class SerialConnector
    {
        const int DEAD = 0;
        const int IDLE = 1;
        const int ALIVE = 2;
        private int pid, vid;
        public int state { private set; get; }
        private IntPtr ctx, devs;
        public SerialConnector(int vid, int pid)
        {
            var ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr)));
            this.pid = pid;
            this.vid = vid;
            AnCtx_Init(ptr);
            this.ctx = (IntPtr)Marshal.PtrToStructure(ptr, typeof(IntPtr));
            //var devsPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr)));
            //this.devs = (IntPtr)Marshal.PtrToStructure(devsPtr, typeof(IntPtr));
            this.devs = IntPtr.Zero;
            this.state = DEAD;
            GetList(ctx, 1);
        }

        public void setup()
        {
            this.devs = GetList(this.ctx, 1);//get up to 5 devices..for now...
        }

        private IntPtr GetList(IntPtr ctx, uint numDevs)
        {
            //var ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr)));
            //this.devs = (IntPtr)Marshal.PtrToStructure(ptr, typeof(IntPtr));
            Console.WriteLine(AnDevice_GetList(this.ctx, out this.devs, numDevs));
            return (IntPtr)Marshal.PtrToStructure(devs, typeof(IntPtr));
        }

        [DllImport("libantumbra.dll", CallingConvention = CallingConvention.Cdecl)] 
        public static extern int AnCtx_Init(IntPtr ctx);
        [DllImport("libantumbra.dll", CallingConvention = CallingConvention.Cdecl)] 
        public static extern int AnCtx_Deinit(IntPtr ctx);
        [DllImport("libantumbra.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int AnDevice_GetList(IntPtr ctx, out IntPtr outdevs, uint outndevs);
        //public static extern void AnDeviceInfo_UsbInfo(IntPtr info, byte bus, byte addr, Int16 vid, Int16 pid);
        //public static extern int AnDevice_Populate(IntPtr ctx);
        [DllImport("libantumbra.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern char[] AnError_String(int e);
        //public static extern int AnDevice_GetCount(IntPtr ctx);
        [DllImport("libantumbra.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int AnDevice_GetList(IntPtr ctx, IntPtr outdevs, int outndevs);
        //public static extern IntPtr AnDevice_Get(IntPtr ctx, int i);
        [DllImport("libantumbra.dll", CallingConvention = CallingConvention.Cdecl)] 
        public static extern void AnDevice_Info(IntPtr dev, UInt16 vid, UInt16 pid, IntPtr serial);
        [DllImport("libantumbra.dll", CallingConvention = CallingConvention.Cdecl)] 
        public static extern int AnDevice_State(IntPtr dev);
        [DllImport("libantumbra.dll", CallingConvention = CallingConvention.Cdecl)] 
        public static extern int AnDevice_Open(IntPtr ctx, IntPtr dev);
        [DllImport("libantumbra.dll", CallingConvention = CallingConvention.Cdecl)] 
        public static extern int AnDevice_Close(IntPtr ctx, IntPtr dev);
        [DllImport("libantumbra.dll", CallingConvention = CallingConvention.Cdecl)] 
        public static extern void AnDevice_FreeList(IntPtr devs);
        [DllImport("libantumbra.dll", CallingConvention = CallingConvention.Cdecl)] 
        public static extern int AnDevice_SetRGB_S(IntPtr ctx, IntPtr dev, byte r, byte g, byte b);
    }
}
