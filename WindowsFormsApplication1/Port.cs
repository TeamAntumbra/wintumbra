//==========================================================================================
//
//		OpenNETCF.IO.Serial.Port
//		Copyright (c) 2003, OpenNETCF.org
//
//		This library is free software; you can redistribute it and/or modify it under
//		the terms of the OpenNETCF.org Shared Source License.
//
//		This library is distributed in the hope that it will be useful, but
//		WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
//		FITNESS FOR A PARTICULAR PURPOSE. See the OpenNETCF.org Shared Source License
//		for more details.
//
//		You should have received a copy of the OpenNETCF.org Shared Source License
//		along with this library; if not, email licensing@opennetcf.org to request a copy.
//
//		If you wish to contact the OpenNETCF Advisory Board to discuss licensing, please
//		email licensing@opennetcf.org.
//
//		For general enquiries, email enquiries@opennetcf.org or visit our website at:
//		http://www.opennetcf.org
//
//==========================================================================================

//
// Dec 04:	Yuri Astrakhan (YuriAstrakhan at gmail dot com)
//			Changed access modificers from private to internal on 
//			hPort, rxBufferSize, rxFIFO, txBufferSize, ptxBuffer, rxBufferBusy, and m_CommAPI.
//

using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Text;
using System.Collections;

namespace OpenNETCF.IO.Serial
{
	/// <summary>
	/// Exceptions throw by the OpenNETCF.IO.Serial class
	/// </summary>
	public class CommPortException : Exception
	{
		/// <summary>
		/// Default CommPortException
		/// </summary>
		/// <param name="desc"></param>
		public CommPortException(string desc) : base(desc) {}
	}

	/// <summary>
	/// A class wrapper for serial port communications
	/// </summary>
	public class Port : IDisposable
	{
		[DllImport("kernel32", EntryPoint="LocalAlloc", SetLastError=true)]
		internal static extern IntPtr LocalAlloc(int uFlags, int uBytes);

		[DllImport("kernel32", EntryPoint="LocalFree", SetLastError=true)]
		internal static extern IntPtr LocalFree(IntPtr hMem);

		#region delegates and events
		/// <summary>
		/// Raised on all enabled communication events
		/// </summary>
		public delegate void CommEvent();
		/// <summary>
		/// Raised when the communication state changes
		/// </summary>
		public delegate void CommChangeEvent(bool NewState);
		/// <summary>
		/// Raised during any communication error
		/// </summary>
		public delegate void CommErrorEvent(string Description);
		/// <summary>
		///  A communication error has occurred
		/// </summary>
		public event CommErrorEvent OnError;
		/// <summary>
		/// Serial data has been received
		/// </summary>
		public event CommEvent DataReceived;
//		/// <summary>
//		/// Overrun of the transmit buffer
//		/// </summary>
//		public event CommEvent RxOverrun;
		/// <summary>
		/// Transmit complete
		/// </summary>
		public event CommEvent TxDone;
		/// <summary>
		/// Set flag character was in the receive stream
		/// </summary>
		public event CommEvent FlagCharReceived;
		/// <summary>
		/// Power change event has occurred
		/// </summary>
		public event CommEvent PowerEvent;
		/// <summary>
		/// Serial buffer's high-water level has been exceeded
		/// </summary>
		public event CommEvent HighWater;
		/// <summary>
		/// DSR state has changed
		/// </summary>
		public event CommChangeEvent DSRChange;
		/// <summary>
		/// Ring signal has been detected
		/// </summary>
		public event CommChangeEvent RingChange;
		/// <summary>
		/// CTS state has changed
		/// </summary>
		public event CommChangeEvent CTSChange;
		/// <summary>
		/// RLSD state has changed
		/// </summary>
		public event CommChangeEvent RLSDChange;
		#endregion

		#region ##### variable declarations #####
		private string portName;
		internal IntPtr hPort = (IntPtr)CommAPI.INVALID_HANDLE_VALUE;

		// default Rx buffer is 1024 bytes
		internal int rxBufferSize = 1024;
		internal Queue rxFIFO;
		private int rthreshold = 1;

		// default Tx buffer is 1024 bytes
		internal int txBufferSize = 1024;
		private byte[] txBuffer;
		internal int ptxBuffer	= 0;
		private int sthreshold = 1;

		internal Mutex rxBufferBusy = new Mutex();
		private int inputLength;

		private DCB dcb = new DCB();
		private DetailedPortSettings portSettings;

		private Thread eventThread;
		private ManualResetEvent threadStarted = new ManualResetEvent(false);

		private IntPtr closeEvent;
		private string closeEventName = "CloseEvent";

		private int		rts			= 0;
		private bool	rtsavail	= false;
		private int		dtr			= 0;
		private bool	dtravail	= false;
		private int		brk			= 0;
		private int		setir		= 0;
		private bool	isOpen		= false;

		private IntPtr txOverlapped = IntPtr.Zero;
		private IntPtr rxOverlapped = IntPtr.Zero;

		internal CommAPI m_CommAPI;
		/// <summary>
		/// stores port's capabilities - capabilities can only be retreived not set
		/// </summary>
		public readonly CommCapabilities Capabilities = new CommCapabilities();

		#endregion

		private void Init()
		{
			// create the API class based on the target
			if (System.Environment.OSVersion.Platform != PlatformID.WinCE)
				m_CommAPI=new IO.Serial.WinCommAPI();
			else
				m_CommAPI=new IO.Serial.CECommAPI();

			// create a system event for synchronizing Closing
			closeEvent = m_CommAPI.CreateEvent(true, false, closeEventName);

			rxFIFO = new Queue(rxBufferSize);
			txBuffer = new byte[txBufferSize];
			portSettings = new DetailedPortSettings();
		}

		#region constructors
		/// <summary>
		/// Create a serial port class.  The port will be created with defualt settings.
		/// </summary>
		/// <param name="PortName">The port to open (i.e. "COM1:")</param>
		public Port(string PortName)
		{
			this.PortName = PortName;
			Init();
		}

		/// <summary>
		/// Create a serial port class.
		/// </summary>
		/// <param name="PortName">The port to open (i.e. "COM1:")</param>
		/// <param name="InitialSettings">BasicPortSettings to apply to the new Port</param>
		public Port(string PortName, BasicPortSettings InitialSettings)
		{
			this.PortName = PortName;
			Init();

			//override default ettings
			portSettings.BasicSettings = InitialSettings;
		}

		/// <summary>
		/// Create a serial port class.
		/// </summary>
		/// <param name="PortName">The port to open (i.e. "COM1:")</param>
		/// <param name="InitialSettings">DetailedPortSettings to apply to the new Port</param>
		public Port(string PortName, DetailedPortSettings InitialSettings)
		{
			this.PortName = PortName;
			Init();

			//override default ettings
			portSettings = InitialSettings;
		}

		/// <summary>
		/// Create a serial port class.
		/// </summary>
		/// <param name="PortName">The port to open (i.e. "COM1:")</param>
		/// <param name="RxBufferSize">Receive buffer size, in bytes</param>
		/// <param name="TxBufferSize">Transmit buffer size, in bytes</param>
		public Port(string PortName, int RxBufferSize, int TxBufferSize)
		{
			rxBufferSize = RxBufferSize;
			txBufferSize = TxBufferSize;
			this.PortName = PortName;
			Init();
		}

		/// <summary>
		/// Create a serial port class.
		/// </summary>
		/// <param name="PortName">The port to open (i.e. "COM1:")</param>
		/// <param name="InitialSettings">BasicPortSettings to apply to the new Port</param>
		/// <param name="RxBufferSize">Receive buffer size, in bytes</param>
		/// <param name="TxBufferSize">Transmit buffer size, in bytes</param>
		public Port(string PortName, BasicPortSettings InitialSettings, int RxBufferSize, int TxBufferSize)
		{
			rxBufferSize = RxBufferSize;
			txBufferSize = TxBufferSize;
			this.PortName = PortName;
			Init();

			//override default ettings
			portSettings.BasicSettings = InitialSettings;
		}

		/// <summary>
		/// Create a serial port class.
		/// </summary>
		/// <param name="PortName">The port to open (i.e. "COM1:")</param>
		/// <param name="InitialSettings">DetailedPortSettings to apply to the new Port</param>
		/// <param name="RxBufferSize">Receive buffer size, in bytes</param>
		/// <param name="TxBufferSize">Transmit buffer size, in bytes</param>
		public Port(string PortName, DetailedPortSettings InitialSettings, int RxBufferSize, int TxBufferSize)
		{
			rxBufferSize = RxBufferSize;
			txBufferSize = TxBufferSize;
			this.PortName = PortName;
			Init();

			//override default ettings
			portSettings = InitialSettings;
		}
		#endregion

		// since the event thread blocks until the port handle is closed
		// implement both a Dispose and destrucor to make sure that we
		// clean up as soon as possible
		/// <summary>
		/// Dispose the object's resources
		/// </summary>
		public void Dispose()
		{
			if(isOpen)
				this.Close();
		}

		/// <summary>
		/// Class destructor
		/// </summary>
		~Port()
		{
			if(isOpen)
				this.Close();
		}

		/// <summary>
		/// The name of the Port (i.e. "COM1:")
		/// </summary>
		public string PortName
		{
			get
			{
				return portName;
			}
			set
			{
				if(! CommAPI.FullFramework)
				{
					// for CE, ensure the port name is colon terminated "COMx:"
					if(! value.EndsWith(":"))
					{
						portName = value + ":";
						return;
					}
				}

				portName = value;
			}
		}

		/// <summary>
		/// Returns whether or not the port is currently open
		/// </summary>
		public bool IsOpen
		{
			get
			{
				return isOpen;
			}
		}

		/// <summary>
		/// Open the current port
		/// </summary>
		/// <returns>true if successful, false if it fails</returns>
		public bool Open()
		{
			if(isOpen) return false;

			if(CommAPI.FullFramework)
			{
				// set up the overlapped tx IO
				//				AutoResetEvent are = new AutoResetEvent(false);
				OVERLAPPED o = new OVERLAPPED();
				txOverlapped = LocalAlloc(0x40, Marshal.SizeOf(o));
				o.Offset = 0;
				o.OffsetHigh = 0;
				o.hEvent = IntPtr.Zero;
				Marshal.StructureToPtr(o, txOverlapped, true);
			}

			hPort = m_CommAPI.CreateFile(portName);

			if(hPort == (IntPtr)CommAPI.INVALID_HANDLE_VALUE)
			{
				int e = Marshal.GetLastWin32Error();

				if(e == (int)APIErrors.ERROR_ACCESS_DENIED)
				{
					// port is unavailable
					return false;
				}

				// ClearCommError failed!
				string error = String.Format("CreateFile Failed: {0}", e);
				throw new CommPortException(error);
			}


			isOpen = true;

			// set queue sizes
			m_CommAPI.SetupComm(hPort, rxBufferSize, txBufferSize);

			// transfer the port settings to a DCB structure
			dcb.BaudRate = (uint)portSettings.BasicSettings.BaudRate;
			dcb.ByteSize = portSettings.BasicSettings.ByteSize;
			dcb.EofChar = (sbyte)portSettings.EOFChar;
			dcb.ErrorChar = (sbyte)portSettings.ErrorChar;
			dcb.EvtChar = (sbyte)portSettings.EVTChar;
			dcb.fAbortOnError = portSettings.AbortOnError;
			dcb.fBinary = true;
			dcb.fDsrSensitivity = portSettings.DSRSensitive;
			dcb.fDtrControl = (DCB.DtrControlFlags)portSettings.DTRControl;
			dcb.fErrorChar = portSettings.ReplaceErrorChar;
			dcb.fInX = portSettings.InX;
			dcb.fNull = portSettings.DiscardNulls;
			dcb.fOutX = portSettings.OutX;
			dcb.fOutxCtsFlow = portSettings.OutCTS;
			dcb.fOutxDsrFlow = portSettings.OutDSR;
			dcb.fParity = (portSettings.BasicSettings.Parity == Parity.none) ? false : true;
			dcb.fRtsControl = (DCB.RtsControlFlags)portSettings.RTSControl;
			dcb.fTXContinueOnXoff = portSettings.TxContinueOnXOff;
			dcb.Parity = (byte)portSettings.BasicSettings.Parity;

			dcb.StopBits = (byte)portSettings.BasicSettings.StopBits;
			dcb.XoffChar = (sbyte)portSettings.XoffChar;
			dcb.XonChar = (sbyte)portSettings.XonChar;

			dcb.XonLim = dcb.XoffLim = (ushort)(rxBufferSize / 10);

			m_CommAPI.SetCommState(hPort, dcb);

			// store some state values
			brk = 0;
			dtr = dcb.fDtrControl == DCB.DtrControlFlags.Enable ? 1 : 0;
			rts = dcb.fRtsControl == DCB.RtsControlFlags.Enable ? 1 : 0;

			// set the Comm timeouts
			CommTimeouts ct = new CommTimeouts();

			// reading we'll return immediately
			// this doesn't seem to work as documented
			ct.ReadIntervalTimeout = uint.MaxValue; // this = 0xffffffff
			ct.ReadTotalTimeoutConstant = 0;
			ct.ReadTotalTimeoutMultiplier = 0;

			// writing we'll give 5 seconds
			ct.WriteTotalTimeoutConstant = 5000;
			ct.WriteTotalTimeoutMultiplier = 0;

			m_CommAPI.SetCommTimeouts(hPort, ct);

			// read the ports capabilities
			bool status=GetPortProperties();

			// start the receive thread
			eventThread = new Thread(new ThreadStart(CommEventThread));
			eventThread.Priority = ThreadPriority.Highest;
			eventThread.Start();

			// wait for the thread to actually get spun up
			threadStarted.WaitOne();

			return true;
		}

		/// <summary>
		/// Query the current port's capabilities without accessing it. You can only call the Close()
		/// method after reading the capabilities. This method does neither initialize nor Open() the
		/// port.
		/// </summary>
		///
		/// <example>
		///
		/// </example>
		public bool Query()
		{
			if(isOpen) return false;

			hPort = m_CommAPI.QueryFile(portName);

			if(hPort == (IntPtr)CommAPI.INVALID_HANDLE_VALUE)
			{
				int e = Marshal.GetLastWin32Error();

				if(e == (int)APIErrors.ERROR_ACCESS_DENIED)
				{
					// port is unavailable
					return false;
				}

				// ClearCommError failed!
				string error = String.Format("CreateFile Failed: {0}", e);
				throw new CommPortException(error);
			}


			// read the port's capabilities
			bool status=GetPortProperties();

			return true;
		}

		// parameters without closing and reopening the port
		/// <summary>
		/// Updates communication settings of the port
		/// </summary>
		/// <returns>true if successful, false if it fails</returns>
		private bool UpdateSettings()
		{
			if(!isOpen) return false;

			// transfer the port settings to a DCB structure
			dcb.BaudRate = (uint)portSettings.BasicSettings.BaudRate;
			dcb.ByteSize = portSettings.BasicSettings.ByteSize;
			dcb.EofChar = (sbyte)portSettings.EOFChar;
			dcb.ErrorChar = (sbyte)portSettings.ErrorChar;
			dcb.EvtChar = (sbyte)portSettings.EVTChar;
			dcb.fAbortOnError = portSettings.AbortOnError;
			dcb.fBinary = true;
			dcb.fDsrSensitivity = portSettings.DSRSensitive;
			dcb.fDtrControl = (DCB.DtrControlFlags)portSettings.DTRControl;
			dcb.fErrorChar = portSettings.ReplaceErrorChar;
			dcb.fInX = portSettings.InX;
			dcb.fNull = portSettings.DiscardNulls;
			dcb.fOutX = portSettings.OutX;
			dcb.fOutxCtsFlow = portSettings.OutCTS;
			dcb.fOutxDsrFlow = portSettings.OutDSR;
			dcb.fParity = (portSettings.BasicSettings.Parity == Parity.none) ? false : true;
			dcb.fRtsControl = (DCB.RtsControlFlags)portSettings.RTSControl;
			dcb.fTXContinueOnXoff = portSettings.TxContinueOnXOff;
			dcb.Parity = (byte)portSettings.BasicSettings.Parity;
			dcb.StopBits = (byte)portSettings.BasicSettings.StopBits;
			dcb.XoffChar = (sbyte)portSettings.XoffChar;
			dcb.XonChar = (sbyte)portSettings.XonChar;

			dcb.XonLim = dcb.XoffLim = (ushort)(rxBufferSize / 10);

			return m_CommAPI.SetCommState(hPort, dcb);

		}

		/// <summary>
		/// Close the current serial port
		/// </summary>
		/// <returns>true indicates success, false indicated failure</returns>
		public bool Close()
		{

			if(txOverlapped != IntPtr.Zero)
			{
				LocalFree(txOverlapped);
				txOverlapped = IntPtr.Zero;
			}

			if(!isOpen) return false;

			isOpen = false; // to help catch intentional close

			if(m_CommAPI.CloseHandle(hPort))
			{
				m_CommAPI.SetEvent(closeEvent);

				isOpen = false;

				hPort = (IntPtr)CommAPI.INVALID_HANDLE_VALUE;

				m_CommAPI.SetEvent(closeEvent);

				return true;
			}

			return false;
		}

		/// <summary>
		/// The Port's output buffer.  Set this property to send data.
		/// </summary>
		public byte[] Output
		{
			set
			{
				if(!isOpen)
					throw new CommPortException("Port not open");

				int written = 0;

				// more than threshold amount so send without buffering
				if(value.GetLength(0) > sthreshold)
				{
					// first send anything already in the buffer
					if(ptxBuffer > 0)
					{
						m_CommAPI.WriteFile(hPort, txBuffer, ptxBuffer, ref written, txOverlapped);
						ptxBuffer = 0;
					}

					m_CommAPI.WriteFile(hPort, value, (int)value.GetLength(0), ref written, txOverlapped);
				}
				else
				{
					// copy it to the tx buffer
					value.CopyTo(txBuffer, (int)ptxBuffer);
					ptxBuffer += (int)value.Length;

					// now if the buffer is above sthreshold, send it
					if(ptxBuffer >= sthreshold)
					{
						m_CommAPI.WriteFile(hPort, txBuffer, ptxBuffer, ref written, txOverlapped);
						ptxBuffer = 0;
					}
				}
			}
		}

		/// <summary>
		/// The Port's input buffer.  Incoming data is read from here and a read will pull InputLen bytes from the buffer
		/// <seealso cref="InputLen"/>
		/// </summary>
		public byte[] Input
		{
			get
			{
				if(!isOpen) return null;

				int dequeueLength = 0;

				// lock the rx FIFO while reading
				rxBufferBusy.WaitOne();

				// how much data are we *actually* going to return from the call?
				if(inputLength == 0)
					dequeueLength = rxFIFO.Count;  // pull the entire buffer
				else
					dequeueLength = (inputLength < rxFIFO.Count) ? inputLength : rxFIFO.Count;

				byte[] data = new byte[dequeueLength];

				// dequeue the data
				for(int p = 0 ; p < dequeueLength ; p++)
					data[p] = (byte)rxFIFO.Dequeue();

				// release the mutex so the Rx thread can continue
				rxBufferBusy.ReleaseMutex();

				return data;
			}
		}

		/// <summary>
		/// The length of the input buffer
		/// </summary>
		public int InputLen
		{
			get
			{
				return inputLength;
			}
			set
			{
				inputLength = value;
			}
		}

		/// <summary>
		/// The actual amount of data in the input buffer
		/// </summary>
		public int InBufferCount
		{
			get
			{
				if(!isOpen) return 0;

				return rxFIFO.Count;
			}
		}

		/// <summary>
		/// The actual amount of data in the output buffer
		/// </summary>
		public int OutBufferCount
		{
			get
			{
				if(!isOpen) return 0;

				return ptxBuffer;
			}
		}

		/// <summary>
		/// The number of bytes that the receive buffer must exceed to trigger a Receive event
		/// </summary>
		public int RThreshold
		{
			get
			{
				return rthreshold;
			}
			set
			{
				rthreshold = value;
			}
		}

		/// <summary>
		/// The number of bytes that the transmit buffer must exceed to trigger a Transmit event
		/// </summary>
		public int SThreshold
		{
			get
			{
				return sthreshold;
			}
			set
			{
				sthreshold = value;
			}
		}

		/// <summary>
		/// Send or check for a communications BREAK event
		/// </summary>
		public bool Break
		{
			get
			{
				if(!isOpen) return false;

				return (brk == 1);
			}
			set
			{
				if(!isOpen) return;
				if(brk < 0) return;
				if(hPort == (IntPtr)CommAPI.INVALID_HANDLE_VALUE) return;

				if (value)
				{
					if (m_CommAPI.EscapeCommFunction(hPort, CommEscapes.SETBREAK))
						brk = 1;
					else
						throw new CommPortException("Failed to set break!");
				}
				else
				{
					if (m_CommAPI.EscapeCommFunction(hPort, CommEscapes.CLRBREAK))
						brk = 0;
					else
						throw new CommPortException("Failed to clear break!");
				}
			}
		}

		/// <summary>
		/// Returns whether or not the current port support a DTR signal
		/// </summary>
		public bool DTRAvailable
		{
			get
			{
				return dtravail;
			}
		}

		/// <summary>
		/// Gets or sets the current DTR line state (true = 1, false = 0)
		/// </summary>
		public bool DTREnable
		{
			get
			{
				return (dtr == 1);
			}
			set
			{
				if(dtr < 0) return;
				if(hPort == (IntPtr)CommAPI.INVALID_HANDLE_VALUE) return;

				if (value)
				{
					if (m_CommAPI.EscapeCommFunction(hPort, CommEscapes.SETDTR))
						dtr = 1;
					else
						throw new CommPortException("Failed to set DTR!");
				}
				else
				{
					if (m_CommAPI.EscapeCommFunction(hPort, CommEscapes.CLRDTR))
						dtr = 0;
					else
						throw new CommPortException("Failed to clear DTR!");
				}
			}
		}

		/// <summary>
		/// Returns whether or not the current port support an RTS signal
		/// </summary>
		public bool RTSAvailable
		{
			get
			{
				return rtsavail;
			}
		}

		/// <summary>
		/// Gets or sets the current RTS line state (true = 1, false = 0)
		/// </summary>
		public bool RTSEnable
		{
			get
			{
				return (rts == 1);
			}
			set
			{
				if(rts < 0) return;
				if(hPort == (IntPtr)CommAPI.INVALID_HANDLE_VALUE) return;

				if (value)
				{
					if (m_CommAPI.EscapeCommFunction(hPort, CommEscapes.SETRTS))
						rts = 1;
					else
						throw new CommPortException("Failed to set RTS!");
				}
				else
				{
					if (m_CommAPI.EscapeCommFunction(hPort, CommEscapes.CLRRTS))
						rts = 0;
					else
						throw new CommPortException("Failed to clear RTS!");
				}
			}
		}
		/// <summary>
		/// Gets or sets the com port for IR use (true = 1, false = 0)
		/// </summary>

		public bool IREnable
		{
			get
			{
				return (setir == 1);
			}
			set
			{
				if(setir < 0) return;
				if(hPort == (IntPtr)CommAPI.INVALID_HANDLE_VALUE) return;

				if (value)
				{
					if (m_CommAPI.EscapeCommFunction(hPort, CommEscapes.SETIR))
						setir = 1;
					else
						throw new CommPortException("Failed to set IR!");
				}
				else
				{
					if (m_CommAPI.EscapeCommFunction(hPort, CommEscapes.CLRIR))
						setir = 0;
					else
						throw new CommPortException("Failed to clear IR!");
				}
			}
		}

		/// <summary>
		/// Get or Set the Port's DetailedPortSettings
		/// </summary>
		public DetailedPortSettings DetailedSettings
		{
			get
			{
				return portSettings;
			}
			set
			{
				portSettings = value;
				UpdateSettings();
			}
		}

		/// <summary>
		/// Get or Set the Port's BasicPortSettings
		/// </summary>
		public BasicPortSettings Settings
		{
			get
			{
				return portSettings.BasicSettings;
			}
			set
			{
				portSettings.BasicSettings = value;
				UpdateSettings();
			}
		}

		/// <summary>
		/// <code>GetPortProperties initializes the commprop member of the port object</code>
		/// </summary>
		/// <returns></returns>
		private bool GetPortProperties()
		{
			bool success;

			success=m_CommAPI.GetCommProperties(hPort,Capabilities);

			return (success);
		}

		private void CommEventThread()
		{
			CommEventFlags	eventFlags	= new CommEventFlags();
			byte[]			readbuffer	= new Byte[rxBufferSize];
			int				bytesread	= 0;
			AutoResetEvent	rxevent		= new AutoResetEvent(false);

			// specify the set of events to be monitored for the port.
			if(CommAPI.FullFramework)
			{
				m_CommAPI.SetCommMask(hPort, CommEventFlags.ALLPC);

				// set up the overlapped IO
				OVERLAPPED o = new OVERLAPPED();
				rxOverlapped = LocalAlloc(0x40, Marshal.SizeOf(o));
				o.Offset = 0;
				o.OffsetHigh = 0;
				o.hEvent = rxevent.Handle;
				Marshal.StructureToPtr(o, rxOverlapped, true);
			}
			else
			{
				m_CommAPI.SetCommMask(hPort, CommEventFlags.ALLCE);
			}

			try
			{
				// let Open() know we're started
				threadStarted.Set();

				#region >>>> thread loop <<<<
				while(hPort != (IntPtr)CommAPI.INVALID_HANDLE_VALUE)
				{
					// wait for a Comm event
					if(!m_CommAPI.WaitCommEvent(hPort, ref eventFlags))
					{
						int e = Marshal.GetLastWin32Error();

						if(e == (int)APIErrors.ERROR_IO_PENDING)
						{
							// IO pending so just wait and try again
							rxevent.WaitOne();
							Thread.Sleep(0);
							continue;
						}

						if(e == (int)APIErrors.ERROR_INVALID_HANDLE)
						{
							// Calling Port.Close() causes hPort to become invalid
							// Since Thread.Abort() is unsupported in the CF, we must
							// accept that calling Close will throw an error here.

							// Close signals the closeEvent, so wait on it
							// We wait 1 second, though Close should happen much sooner
							int eventResult = m_CommAPI.WaitForSingleObject(closeEvent, 1000);

							if(eventResult == (int)APIConstants.WAIT_OBJECT_0)
							{
								// the event was set so close was called
								hPort = (IntPtr)CommAPI.INVALID_HANDLE_VALUE;

								// reset our ResetEvent for the next call to Open
								threadStarted.Reset();

								if(isOpen) // this should not be the case...if so, throw an exception for the owner
								{
									string error = String.Format("Wait Failed: {0}", e);
									throw new CommPortException(error);
								}

								return;
							}
						}

						// WaitCommEvent failed
						// 995 means an exit was requested (thread killed)
						if(e == 995)
						{
							return;
						}
						else
						{
							string error = String.Format("Wait Failed: {0}", e);
							throw new CommPortException(error);
						}
					}

					// Re-specify the set of events to be monitored for the port.
					if(CommAPI.FullFramework)
					{
						m_CommAPI.SetCommMask(hPort, CommEventFlags.ALLPC);
					}
					else
					{
						m_CommAPI.SetCommMask(hPort, CommEventFlags.ALLCE);
					}

					// check the event for errors
					#region >>>> error checking <<<<
					if(((uint)eventFlags & (uint)CommEventFlags.ERR) != 0)
					{
						CommErrorFlags errorFlags = new CommErrorFlags();
						CommStat commStat = new CommStat();

						// get the error status
						if(!m_CommAPI.ClearCommError(hPort, ref errorFlags, commStat))
						{
							// ClearCommError failed!
							string error = String.Format("ClearCommError Failed: {0}", Marshal.GetLastWin32Error());
							throw new CommPortException(error);
						}

						if(((uint)errorFlags & (uint)CommErrorFlags.BREAK) != 0)
						{
							// BREAK can set an error, so make sure the BREAK bit is set an continue
							eventFlags |= CommEventFlags.BREAK;
						}
						else
						{
							// we have an error.  Build a meaningful string and throw an exception
							StringBuilder s = new StringBuilder("UART Error: ", 80);
							if ((errorFlags & CommErrorFlags.FRAME) != 0)
							{ s = s.Append("Framing,");	}
							if ((errorFlags & CommErrorFlags.IOE) != 0)
							{ s = s.Append("IO,"); }
							if ((errorFlags & CommErrorFlags.OVERRUN) != 0)
							{ s = s.Append("Overrun,"); }
							if ((errorFlags & CommErrorFlags.RXOVER) != 0)
							{ s = s.Append("Receive Overflow,"); }
							if ((errorFlags & CommErrorFlags.RXPARITY) != 0)
							{ s = s.Append("Parity,"); }
							if ((errorFlags & CommErrorFlags.TXFULL) != 0)
							{ s = s.Append("Transmit Overflow,"); }

							// no known bits are set
							if(s.Length == 12)
							{ s = s.Append("Unknown"); }

							// raise an error event
							if(OnError != null)
								OnError(s.ToString());

							continue;
						}
					} // if(((uint)eventFlags & (uint)CommEventFlags.ERR) != 0)
					#endregion

					#region >>>> Receive data subsection <<<<
					// check for RXCHAR
					if((eventFlags & CommEventFlags.RXCHAR) != 0)
					{
						do
						{
							// make sure the port handle is valid
							if(hPort == (IntPtr)CommAPI.INVALID_HANDLE_VALUE)
							{
								bytesread = 0;
								break;
							}

							// data came in, put it in the buffer and set the event
							if (!m_CommAPI.ReadFile(hPort, readbuffer, rxBufferSize, ref bytesread, rxOverlapped))
							{
								string errString = String.Format("ReadFile Failed: {0}", Marshal.GetLastWin32Error());
								if(OnError != null)
									OnError(errString);

								return;
							}
							if (bytesread >= 1)
							{
								// take the mutex
								rxBufferBusy.WaitOne();

								// put the data into the fifo
								// this *may*  be a perf problem and needs testing
								for(int b = 0 ; b < bytesread ; b++)
									rxFIFO.Enqueue(readbuffer[b]);

								// get the FIFO length
								int fifoLength = rxFIFO.Count;

								// release the mutex
								rxBufferBusy.ReleaseMutex();

								// fire the DataReceived event every RThreshold bytes
								if((DataReceived != null) && (rthreshold != 0) && (fifoLength >= rthreshold))
								{
									DataReceived();
								}
							}
						} while (bytesread > 0);
					} // if((eventFlags & CommEventFlags.RXCHAR) != 0)
					#endregion

					#region >>>> line status checking <<<<
					// check for status changes
					uint status = 0;
					m_CommAPI.GetCommModemStatus(hPort, ref status);

					// check the CTS
					if(((uint)eventFlags & (uint)CommEventFlags.CTS) != 0)
					{
						if(CTSChange != null)
							CTSChange((status & (uint)CommModemStatusFlags.MS_CTS_ON) != 0);
					}

					// check the DSR
					if(((uint)eventFlags & (uint)CommEventFlags.DSR) != 0)
					{
						if(DSRChange != null)
							DSRChange((status & (uint)CommModemStatusFlags.MS_DSR_ON) != 0);
					}

					// check for a RING
					if(((uint)eventFlags & (uint)CommEventFlags.RING) != 0)
					{
						if(RingChange != null)
							RingChange((status & (uint)CommModemStatusFlags.MS_RING_ON) != 0);
					}

					// check for a RLSD
					if(((uint)eventFlags & (uint)CommEventFlags.RLSD) != 0)
					{
						if(RLSDChange != null)
							RLSDChange((status & (uint)CommModemStatusFlags.MS_RLSD_ON) != 0);
					}

					// check for TXEMPTY
					if(((uint)eventFlags & (uint)CommEventFlags.TXEMPTY) != 0)
						if(TxDone != null) { TxDone(); }

					// check for RXFLAG
					if(((uint)eventFlags & (uint)CommEventFlags.RXFLAG) != 0)
						if(FlagCharReceived != null) { FlagCharReceived(); }

					// check for POWER
					if(((uint)eventFlags & (uint)CommEventFlags.POWER) != 0)
						if(PowerEvent != null) { PowerEvent(); }

					// check for high-water state
					if((eventFlags & CommEventFlags.RX80FULL) != 0)
						if(HighWater != null) { HighWater(); }
					#endregion
				} // while(true)
				#endregion
			} // try
			catch(Exception e)
			{
				if(rxOverlapped != IntPtr.Zero)
					LocalFree(rxOverlapped);

				if(OnError != null)
					OnError(e.Message);

				return;
			}
		}
	}
}
