﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antumbra
{
    class SerialConnector
    {
        OpenNETCF.IO.Ports.SerialPort serial;
        bool ready;

        public SerialConnector(String port)
        {
            this.serial = new OpenNETCF.IO.Ports.SerialPort(port);
            try
            {
                this.serial.Open();
                this.ready = true;
            }
            catch (System.UnauthorizedAccessException)//not available
            {
                this.ready = false;
            }
        }

        public bool isReady()
        {
            if (this.ready) { }
            else//not ready
            {
                try//try to make ready
                {
                    this.serial.Open();
                    this.ready = true;
                }
                catch (System.UnauthorizedAccessException)
                {
                    Console.WriteLine("not ready");
                    this.ready = false;
                }
            }
            return this.ready;
        }

        public bool send(byte[] data)
        {
            if (this.isReady())
            {
                this.serial.Write(data, 0, data.Length);
                foreach (byte current in data)
                {
                    Console.WriteLine("{0:X}", current);
                }
                foreach (byte current in this.serial.ReadToByte(0x7E))
                {
                    Console.WriteLine("{0:X}", current);
                }
                return true;//success
            }
            return false;//fail
        }

        public void close()
        {
            if(this.isReady())
                this.serial.Close();
        }
    }
}
