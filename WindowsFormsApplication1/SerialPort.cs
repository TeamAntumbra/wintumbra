//
// Sep 2004		Original idea in VB.Net by Daniel Moth (http://www.danielmoth.com/Blog/)
// Dec 2004		Complete rewrite by Yuri Astrakhan (YuriAstrakhan at gmail dot com):
//					Ported to C#, streaming support, multiple port hardware providers,
//					recreated complete interface compatible with .NET 2.0 Beta1,
//					exception handling, added method descriptions
//

using System;
using System.Text;
using System.IO;
using System.ComponentModel;
using System.Collections.Generic;

using OpenNETCF.IO.Ports.Streams;

namespace OpenNETCF.IO.Ports
{
	public class SerialPort : IDisposable
	{
		#region Constructiors

		public class Consts
		{
			public const string		InitPortName		= "COM4";
			public const int		InitBaudRate		= 9600;
			public const Parity		InitParity			= Parity.None;
			public const int		InitDataBits		= 8;
			public const StopBits	InitStopBits		= StopBits.One;

			public const bool		InitDiscardNull		= false;
			public const bool		InitDtrEnable		= false;
			public const Handshake	InitHandshake		= Handshake.None;
			public const byte		InitParityReplace	= 0;
			public const int		InitReadBufferSize	= 512;
			public const int		InitReadTimeout		= 0;
			public const int		InitReceivedBytesThreshold = 1;
			public const bool		InitRtsEnable		= false;
			public const int		InitWriteBufferSize	= 1024;
			public const int		InitWriteTimeout	= 0;
		}

		/// <summary>
		/// Initializes a new instance of the SerialPort class.
		/// </summary>
		public SerialPort()
			: this( Consts.InitPortName, Consts.InitBaudRate, Consts.InitParity, Consts.InitDataBits, Consts.InitStopBits )
		{ }

		/// <summary>
		/// Initializes a new instance of the SerialPort class using the IContainer object specified.
		/// </summary>
		public SerialPort(System.ComponentModel.IContainer container)
		{
			throw new NotImplementedException();
		}

		public SerialPort(string portName)
			: this( portName, Consts.InitBaudRate, Consts.InitParity, Consts.InitDataBits, Consts.InitStopBits )
		{ }

		public SerialPort(string portName, int baudRate)
			: this( portName, baudRate, Consts.InitParity, Consts.InitDataBits, Consts.InitStopBits )
		{ }

		public SerialPort(string portName, int baudRate, Parity parity)
			: this( portName, baudRate, parity, Consts.InitDataBits, Consts.InitStopBits )
		{ }

		public SerialPort(string portName, int baudRate, Parity parity, int dataBits)
			: this( portName, baudRate, parity, dataBits, Consts.InitStopBits )
		{ }

		public SerialPort( string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits )
		{
			_portName	= portName;
			_baudRate	= baudRate;
			_parity		= parity;
			_dataBits	= dataBits;
			_stopBits	= stopBits;
		}

		/// <summary>
		/// Create SerialPort with specified underlying stream/streamCtrl objects.
		/// </summary>
		/// <param name="stream"></param>
		/// <param name="streamCtrl"></param>
		public SerialPort( Stream stream, ISerialStreamCtrl streamCtrl )
		{
			_isUserProvidedStream		= true;
			_stream						= stream;
			_streamCtrl					= streamCtrl;
			_streamCtrl.ErrorEvent		+= new SerialErrorEventHandler(ForwardErrorEvents);
			_streamCtrl.PinChangedEvent += new SerialPinChangedEventHandler(ForwardPinChangedEvents);
			_streamCtrl.ReceivedEvent	+= new SerialReceivedEventHandler(ForwardReceivedEvents);
		}
		#endregion

		#region Events and Event Forwarders
		/// <summary>
		/// Represents the method that handles the error event of a SerialPort object.
		/// </summary>
		public event SerialErrorEventHandler ErrorEvent;

		/// <summary>
		/// Represents the method that will handle the serial received event of a SerialPort object.
		/// </summary>
		public event SerialReceivedEventHandler ReceivedEvent;

		/// <summary>
		/// Represents the method that will handle the serial pin changed event of a SerialPort object.
		/// </summary>
		public event SerialPinChangedEventHandler PinChangedEvent;

		private void ForwardErrorEvents(object src, SerialErrorEventArgs e)
		{
			if( ErrorEvent != null )
				ErrorEvent( this, e );
		}

		private void ForwardPinChangedEvents(object src, SerialPinChangedEventArgs e)
		{
			if( PinChangedEvent != null )
				PinChangedEvent( this, e );
		}

		private void ForwardReceivedEvents(object src, SerialReceivedEventArgs e)
		{
			if( ReceivedEvent != null )
				ReceivedEvent( this, e );
		}

		#endregion

		#region Methods
		/// <summary>
		/// Closes the port connection, sets to false and disposes of the internal Stream object.
		/// </summary>
		public void Close()
		{
			if( IsOpen )
			{
				_stream.Flush();
				_stream.Close();
				_stream = null;
				_streamCtrl = null;
			}
		}

		/// <summary>
		/// Discards data from the serial driver's receive buffer.
		/// </summary>
		public void DiscardInBuffer()
		{
			AssertOpenPort();
			_streamCtrl.DiscardInBuffer();
		}

		/// <summary>
		/// Discards data from the serial driver's transmit buffer.
		/// </summary>
		public void DiscardOutBuffer()
		{
			AssertOpenPort();
			_streamCtrl.DiscardOutBuffer();
		}

		/// <summary>
		/// Releases the unmanaged resources used by the SerialPort object.
		/// </summary>
		protected void Dispose(bool disposing)
		{
			if( disposing && null != _stream )
				_stream.Close();
		}

		/// <summary>
		/// Releases the unmanaged resources used by the SerialPort object.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
		}

		/// <summary>
		/// Opens a new serial port connection.
		/// </summary>
		public void Open()
		{
			if( IsOpen ) 
				throw new InvalidOperationException("Port is already open");
			
			if( IsUserProvidedStream ) 
				throw new InvalidOperationException("User Provided Stream cannot be re-opened");

			if( SerialStreamSocket.IsCompatible( _portName ))
			{
				SerialStreamSocket devStream = 
					SerialStreamSocket.CreateInstance( _portName );

				_stream		= (Stream) devStream;
				_streamCtrl = (ISerialStreamCtrl) devStream;
			}
			else
			{
				WinStream devStream = new WinStream( 
					_baudRate,
					_dataBits,
					_discardNull,
					_dtrEnable,
					_handshake,
					_parity,
					_parityReplace,
					_portName,
					_readBufferSize,
					_readTimeout,
					_receivedBytesThreshold,
					_rtsEnable,
					_stopBits,
					_writeBufferSize,
					_writeTimeout					
					);

				_stream		= (Stream) devStream;
				_streamCtrl = (ISerialStreamCtrl) devStream;
				_streamCtrl.ErrorEvent		+= new SerialErrorEventHandler(ForwardErrorEvents);
				_streamCtrl.PinChangedEvent += new SerialPinChangedEventHandler(ForwardPinChangedEvents);
				_streamCtrl.ReceivedEvent	+= new SerialReceivedEventHandler(ForwardReceivedEvents);
			}
		}

		/// <summary>
		/// Returns a list of all serial ports.
		/// </summary>
		public static string[] GetPortNames()
		{
			// TODO: currently only supports windows
			return WinStream.GetPortNames();
		}

		/// <summary>
		/// Reads a number of bytes from the SerialPort input buffer and writes those bytes into a character array at a given offset.
		/// </summary>
		public int Read(byte[] buffer, int offset, int count)
		{
			return _stream.Read( buffer, offset, count );
		}

		/// <summary>
		/// Reads a number of bytes from the SerialPort input buffer and writes those bytes into a character array at a given offset.
		/// </summary>
		public int Read(char[] buffer, int offset, int count )
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Synchronously reads one byte from the SerialPort input buffer.
		/// </summary>
		public int ReadByte()
		{
			return _stream.ReadByte();
		}

		/// <summary>
		/// Synchronously reads one character from the SerialPort input buffer.
		/// </summary>
		public int ReadChar()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Reads all immediately available characters, based on the encoding,
		/// in both the stream and the input buffer of the SerialPort object.
		/// </summary>
		public string ReadExisting()
		{
			AssertOpenPort();

			byte[] buf = new byte[ _streamCtrl.BytesToRead ];
			int bufRead = Read( buf, 0, buf.Length );
			return Encoding.GetString(buf, 0, bufRead);
		}

		/// <summary>
		/// Reads up to the NewLine value in the input buffer.
		/// </summary>
		public string ReadLine()
		{
			return ReadTo( NewLine );
		}

		/// <summary>
		/// Reads a string up to the specified value in the input buffer.
		/// </summary>
		public string ReadTo(string value)
		{
			throw new NotImplementedException();
		}

        public byte[] ReadToByte(byte end)
        {
            List<byte> read = new List<byte>();
            int currentInt = this.ReadByte();
            if (currentInt == -1)
            {
                return new byte[0];
            }
            byte current = (byte)currentInt;
            read.Add(current);
            while (current != end)
            {
                current = (byte)this.ReadByte();
                read.Add(current);
            }
            return read.ToArray();
        }

		/// <summary>
		/// Writes data to serial port output.
		/// </summary>
		public void Write(byte[] buffer, int offset, int count)
		{
			AssertOpenPort();
			_stream.Write( buffer, offset, count );
		}

		/// <summary>
		/// Writes the specified string and the NewLine value to the output buffer.
		/// </summary>
		public void WriteLine(string str)
		{
			byte[] buf = Encoding.GetBytes( str + this.NewLine );
			_stream.Write(buf, 0, buf.Length);
		}
		#endregion

		#region Properties

		/// <summary>
		/// True if this instance was created with the user supplied Stream and ISerialStreamCtrl objects
		/// </summary>
		public bool	IsUserProvidedStream
		{
			get { return _isUserProvidedStream; }
		} private bool _isUserProvidedStream = false;

		/// <summary>
		/// Gets the underlying Stream object for a SerialPort object.
		/// </summary>
		public Stream BaseStream
		{
			get
			{
				AssertOpenPort();
				return _stream;
			}
		}
		private	Stream				_stream;
		private ISerialStreamCtrl	_streamCtrl;

		/// <summary>
		/// Gets or sets the serial baud rate.
		/// </summary>
		public int	BaudRate
		{
			get
			{
				if( IsOpen ) _baudRate = _streamCtrl.BaudRate;
				return _baudRate;
			}
			set
			{
				// TODO: value sanity check
				if( IsOpen ) _streamCtrl.BaudRate = value;
				_baudRate = value;
			}
		} private int _baudRate;

		/// <summary>
		/// Gets or sets the break signal state.
		/// </summary>
		public bool BreakState
		{
			get
			{
				AssertOpenPort();
				return _streamCtrl.BreakState;
			}
			set
			{
				AssertOpenPort();
				// TODO: value sanity check
				_streamCtrl.BreakState = value;
			}
		}

		/// <summary>
		/// Gets the number of bytes of data in the receive buffer.
		/// </summary>
		public int	BytesToRead
		{
			get
			{
				AssertOpenPort();
				return _streamCtrl.BytesToRead;
			}
		}

		/// <summary>
		/// Gets the number of bytes of data in the send buffer.
		/// </summary>
		public int	BytesToWrite
		{
			get
			{
				AssertOpenPort();
				return _streamCtrl.BytesToWrite;
			}
		}

		/// <summary>
		/// Gets the state of the carrier detect line for the port.
		/// </summary>
		public bool CDHolding
		{
			get
			{
				AssertOpenPort();
				return _streamCtrl.CDHolding;
			}
		}

		/// <summary>
		/// Gets the state of the Clear-to-Send line.
		/// </summary>
		public bool CtsHolding
		{
			get
			{
				AssertOpenPort();
				return _streamCtrl.CtsHolding;
			}
		}

		/// <summary>
		/// Gets or sets the standard length of databits per byte.
		/// </summary>
		public int	DataBits
		{
			get
			{
				if( IsOpen ) _dataBits = _streamCtrl.DataBits;
				return _dataBits;
			}
			set
			{
				// TODO: value sanity check
				if( IsOpen ) _streamCtrl.DataBits = value;
				_dataBits = value;
			}
		} private int _dataBits;

		/// <summary>
		/// Gets or sets whether null characters are ignored when transmitted between the port and the receive buffer.
		/// </summary>
		public bool DiscardNull
		{
			get
			{
				if( IsOpen ) _discardNull = _streamCtrl.DiscardNull;
				return _discardNull;
			}
			set
			{
				// TODO: value sanity check
				if( IsOpen ) _streamCtrl.DiscardNull = value;
				_discardNull = value;
			}
		} private bool _discardNull = Consts.InitDiscardNull;

		/// <summary>
		/// Gets the state of the Data Set Ready (DSR) signal.
		/// </summary>
		public bool DsrHolding
		{
			get
			{
				AssertOpenPort();
				return _streamCtrl.DsrHolding;
			}
		}

		/// <summary>
		/// Gets or sets enabling of the Data Terminal Ready (DTR) signal during serial communication.
		/// </summary>
		public bool DtrEnable
		{
			get
			{
				if( IsOpen ) _dtrEnable = _streamCtrl.DtrEnable;
				return _dtrEnable;
			}
			set
			{
				// TODO: value sanity check
				if( IsOpen ) _streamCtrl.DtrEnable = value;
				_dtrEnable = value;
			}
		} private bool _dtrEnable = Consts.InitDtrEnable;

		/// <summary>
		/// Gets or sets the character encoding for pre- and post-transmission conversion of text.
		/// </summary>
		public Encoding Encoding
		{
			get
			{
				if( _encoding == null )
					_encoding = new System.Text.ASCIIEncoding();

				return _encoding;
			}
			set
			{
				// TODO: value sanity check
				_encoding = value;
			}
		} private Encoding _encoding;

		/// <summary>
		/// Gets or sets the handshaking protocol for serial port transmission of data.
		/// </summary>
		public Handshake Handshake
		{
			get
			{
				if( IsOpen ) _handshake = _streamCtrl.Handshake;
				return _handshake;
			}
			set
			{
				// TODO: value sanity check
				if( IsOpen ) _streamCtrl.Handshake = value;
				_handshake = value;
			}
		} private Handshake _handshake = Consts.InitHandshake;

		/// <summary>
		/// Gets the open or closed status of the SerialPort object.
		/// </summary>
		public bool IsOpen
		{
			get
			{
				return _streamCtrl != null && _streamCtrl.IsOpen;
			}
		}

		/// <summary>
		/// Gets or sets the value used to interpret the end of a call to the ReadLine and WriteLine methods.
		/// </summary>
		public string NewLine
		{
			get{ return _newLine; }
			set{ _newLine = value; }
		} private string _newLine = Environment.NewLine;

		/// <summary>
		/// Gets or sets the parity-checking protocol.
		/// </summary>
		public Parity Parity
		{
			get
			{
				if( IsOpen ) _parity = _streamCtrl.Parity;
				return _parity;
			}
			set
			{
				// TODO: value sanity check
				if( IsOpen ) _streamCtrl.Parity = value;
				_parity = value;
			}
		} private Parity _parity;

		/// <summary>
		/// Gets or sets the 8-bit character that is used to replace invalid characters in a data stream when a parity error occurs.
		/// </summary>
		public byte ParityReplace
		{
			get
			{
				if( IsOpen ) _parityReplace = _streamCtrl.ParityReplace;
				return _parityReplace;
			}
			set
			{
				// TODO: value sanity check
				if( IsOpen ) _streamCtrl.ParityReplace = value;
				_parityReplace = value;
			}
		} private byte _parityReplace = Consts.InitParityReplace;

		public int	ReadBufferSize
		{
			get
			{
				if( IsOpen ) _readBufferSize = _streamCtrl.ReadBufferSize;
				return _readBufferSize;
			}
			set
			{
				// TODO: value sanity check
				if( IsOpen ) _streamCtrl.ReadBufferSize = value;
				_readBufferSize = value;
			}
		} private int _readBufferSize = Consts.InitReadBufferSize;

		/// <summary>
		/// Gets or sets the port for communications, including but not limited to all available COM ports.
		/// </summary>
		public string PortName
		{
			get
			{
				return _portName;
			}
			set
			{
				// TODO: value sanity check

				if( IsOpen )
					throw new InvalidOperationException("PortName cannot be changed while the port is open");

				_portName = value;
			}
		} private string _portName;

		/// <summary>
		/// Gets or sets the number of milliseconds before a timeout occurs when a read operation does not finish.
		/// </summary>
		public int	ReadTimeout
		{
			get
			{
				if( IsOpen ) _readTimeout = _streamCtrl.ReadTimeout;
				return _readTimeout;
			}
			set
			{
				// TODO: value sanity check
				if( IsOpen ) _streamCtrl.ReadTimeout = value;
				_readTimeout = value;
			}
		} private int _readTimeout = Consts.InitReadTimeout;

		/// <summary>
		/// Gets or sets the number of bytes in the internal input buffer before a ReceivedEvent is fired.
		/// </summary>
		public int	ReceivedBytesThreshold
		{
			get
			{
				if( IsOpen ) _receivedBytesThreshold = _streamCtrl.ReceivedBytesThreshold;
				return _receivedBytesThreshold;
			}
			set
			{
				// TODO: value sanity check
				if( IsOpen ) _streamCtrl.ReceivedBytesThreshold = value;
				_receivedBytesThreshold = value;
			}
		} private int _receivedBytesThreshold = Consts.InitReceivedBytesThreshold;

		/// <summary>
		/// Gets or sets whether the Request to Transmit (RTS) signal is enabled during serial communication.
		/// </summary>
		public bool RtsEnable
		{
			get
			{
				if( IsOpen ) _rtsEnable = _streamCtrl.RtsEnable;
				return _rtsEnable;
			}
			set
			{
				// TODO: value sanity check
				if( IsOpen ) _streamCtrl.RtsEnable = value;
				_rtsEnable = value;
			}
		} private bool _rtsEnable = Consts.InitRtsEnable;

		/// <summary>
		/// Gets or sets the standard number of stopbits per byte.
		/// </summary>
		public StopBits StopBits
		{
			get
			{
				if( IsOpen ) _stopBits = _streamCtrl.StopBits;
				return _stopBits;
			}
			set
			{
				// TODO: value sanity check
				if( IsOpen ) _streamCtrl.StopBits = value;
				_stopBits = value;
			}
		} private StopBits _stopBits;

		public int	WriteBufferSize
		{
			get
			{
				if( IsOpen ) _writeBufferSize = _streamCtrl.WriteBufferSize;
				return _writeBufferSize;
			}
			set
			{
				// TODO: value sanity check
				if( IsOpen ) _streamCtrl.WriteBufferSize = value;
				_writeBufferSize = value;
			}
		} private int _writeBufferSize = Consts.InitWriteBufferSize;

		/// <summary>
		/// Gets or sets the number of milliseconds before a timeout occurs when a write operation does not finish.
		/// </summary>
		public int	WriteTimeout
		{
			get
			{
				if( IsOpen ) _writeTimeout = _streamCtrl.WriteTimeout;
				return _writeTimeout;
			}
			set
			{
				// TODO: value sanity check
				if( IsOpen ) _streamCtrl.WriteTimeout = value;
				_writeTimeout = value;
			}
		} private int _writeTimeout = Consts.InitWriteTimeout;

		#endregion

		#region Utilities
		protected void AssertOpenPort()
		{
//			if( !IsOpen )
//				throw new InvalidOperationException("Serial Port is not open");
		}
		#endregion
	}
}