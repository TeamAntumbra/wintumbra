using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.InteropServices;

namespace Antumbra
{
    class SerialConnector
    {
        const int DEAD = 0;
        const int IDLE = 1;
        const int ALIVE = 2;
        private int pid, vid, state;
        private IntPtr ctx, dev;
        public SerialConnector(int vid, int pid)
        {
            var ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr)));
            this.pid = pid;
            this.vid = vid;
            AnCtx_Init(ptr);
            this.ctx = (IntPtr)Marshal.PtrToStructure(ptr, typeof(IntPtr));
            this.dev = IntPtr.Zero;
        }

        public bool setup() //return true if success, else false
        {
            if (findDevice()) {
                return true;
            }
            return false;
        }

        private bool findDevice()//returns true if device is found, else false
        {
            AnDevice_Populate(this.ctx);
            for (int i = 0; i < AnDevice_GetCount(this.ctx); i++) {
                this.dev = AnDevice_Get(this.ctx, i);
                if (AnDevice_Open(this.ctx, this.dev) == 0) {
                    return true;
                }
            }
            return false;
        }

        private void updateState()
        {
            if (this.dev == null)
                this.state = DEAD;
            else
                this.state = AnDevice_State(this.dev);
        }

        public bool send(byte r, byte g, byte b)//return true if success, else false
        {
            //if (state == ALIVE) {
                return AnDevice_SetRGB_S(this.ctx, this.dev, r, g, b) == 0;
           /* }
            return false;*/
        }

        public void close()
        {
             
        }
        [DllImport("libantumbra.dll", CallingConvention = CallingConvention.Cdecl)] 
        public static extern int AnCtx_Init(IntPtr ctx);
        [DllImport("libantumbra.dll", CallingConvention = CallingConvention.Cdecl)] 
        public static extern int AnCtx_Deinit(IntPtr ctx);
        [DllImport("libantumbra.dll", CallingConvention = CallingConvention.Cdecl)] 
        public static extern int AnDevice_Populate(IntPtr ctx);
        [DllImport("libantumbra.dll", CallingConvention = CallingConvention.Cdecl)] 
        public static extern int AnDevice_GetCount(IntPtr ctx);
        [DllImport("libantumbra.dll", CallingConvention = CallingConvention.Cdecl)] 
        public static extern IntPtr AnDevice_Get(IntPtr ctx, int i);
        [DllImport("libantumbra.dll", CallingConvention = CallingConvention.Cdecl)] 
        public static extern void AnDevice_Info(IntPtr dev, UInt16 vid, UInt16 pid, IntPtr serial);
        [DllImport("libantumbra.dll", CallingConvention = CallingConvention.Cdecl)] 
        public static extern int AnDevice_State(IntPtr dev);
        [DllImport("libantumbra.dll", CallingConvention = CallingConvention.Cdecl)] 
        public static extern int AnDevice_Open(IntPtr ctx, IntPtr dev);
        [DllImport("libantumbra.dll", CallingConvention = CallingConvention.Cdecl)] 
        public static extern int AnDevice_Close(IntPtr ctx, IntPtr dev);
        [DllImport("libantumbra.dll", CallingConvention = CallingConvention.Cdecl)] 
        public static extern void AnDevice_Free(IntPtr dev);
        [DllImport("libantumbra.dll", CallingConvention = CallingConvention.Cdecl)] 
        public static extern int AnDevice_SetRGB_S(IntPtr ctx, IntPtr dev, byte r, byte g, byte b);
    }
}
