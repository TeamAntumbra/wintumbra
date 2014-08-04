using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antumbra
{
    class SerialConnector
    {
        OpenNETCF.IO.Ports.SerialPort serial;
        private bool ready;
        private String port;

        public SerialConnector(String port)
        {
            Console.WriteLine("new serial port");
            this.port = port;
            this.open();
        }

        private void open()
        {
            this.serial = new OpenNETCF.IO.Ports.SerialPort(port);
            this.serial.ReadTimeout = 250;
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
                    /*this.serial.Close();
                    this.serial = new OpenNETCF.IO.Ports.SerialPort(this.port);*/
                    this.open();
                }
                catch (System.UnauthorizedAccessException)
                {
                    Console.WriteLine("not ready");
                    this.ready = false;
                }
                catch (System.InvalidOperationException)
                {
                    this.ready = false;
                }
            }
            return this.ready;
        }

        public void notReady()
        {
            this.ready = false;
        }

        public bool send(byte[] data)
        {
            if (this.isReady())
            {
                this.serial.Write(data, 0, data.Length);
                Console.Write("Sent: ");
                foreach (byte current in data)
                {
                    Console.Write("{0:X}, ", current);
                }
                Console.Write("\nRecieved: ");
                byte[] recieved = this.serial.ReadToByte(0x7F);
                if (recieved.Length == 0)//nothing recieved
                {
                    Console.WriteLine("Nothing recieved.");
                    this.ready = false;
                    return false;
                }
                foreach (byte current in recieved)
                {
                    Console.Write("{0:X}, ", current);
                }
                Console.WriteLine("");
                return true;//success
            }
            return false;//fail
        }

        public void setBaud(int baud)
        {
            this.serial.BaudRate = baud;
        }

        public void close()
        {
            this.serial.Close();
            this.serial.Dispose();
            this.serial = null;
        }
    }
}
