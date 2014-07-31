//==========================================================================================
//
//		namespace OpenNETCF.IO.Serial.PortSettings
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

using System;
using System.Runtime.InteropServices;

namespace OpenNETCF.IO.Serial
{
	#region namespace enumerations
	/// <summary>
	/// Common ASCII Control Codes
	/// </summary>
	public enum ASCII : byte
	{
		/// <summary>
		/// NULL
		/// </summary>
		NULL = 0x00,
		/// <summary>
		/// Start of Heading
		/// </summary>
		SOH  = 0x01,  
		/// <summary>
		/// Start of Text
		/// </summary>
		STX = 0x02,  
		/// <summary>
		/// End of Text
		/// </summary>
		ETX = 0x03,
		/// <summary>
		/// End of Transmission
		/// </summary>
		EOT = 0x04,  
		/// <summary>
		/// Enquiry
		/// </summary>
		ENQ = 0x05,
		/// <summary>
		/// Acknowledge
		/// </summary>
		ACK	= 0x06,
		/// <summary>
		/// Bell
		/// </summary>
		BELL = 0x07,
		/// <summary>
		/// Backspace
		/// </summary>
		BS  = 0x08,
		/// <summary>
		/// Horizontal tab
		/// </summary>
		HT  = 0x09,
		/// <summary>
		/// Line Feed
		/// </summary>
		LF  = 0x0A,
		/// <summary>
		/// Vertical tab
		/// </summary>
		VT  = 0x0B,
		/// <summary>
		/// Form Feed
		/// </summary>
		FF  = 0x0C,
		/// <summary>
		/// Carriage Return
		/// </summary>
		CR  = 0x0D,
		/// <summary>
		/// Shift out
		/// </summary>
		SO  = 0x0E,
		/// <summary>
		/// Shift in
		/// </summary>
		SI  = 0x0F,
		/// <summary>
		/// Device Control 1
		/// </summary>
		DC1 = 0x11,
		/// <summary>
		/// Device Control 2
		/// </summary>
		DC2 = 0x12,
		/// <summary>
		/// Device Control 3
		/// </summary>
		DC3 = 0x13,
		/// <summary>
		/// Device Control 4
		/// </summary>
		DC4 = 0x14,
		/// <summary>
		/// No Acknowledge
		/// </summary>
		NAK = 0x15,
		/// <summary>
		/// Synchronization
		/// </summary>
		SYN = 0x16,
		/// <summary>
		/// End of Transmission Block
		/// </summary>
		ETB = 0x17,
		/// <summary>
		/// Cancel
		/// </summary>
		CAN = 0x18,
		/// <summary>
		/// End of Medium
		/// </summary>
		EM  = 0x19,
		/// <summary>
		/// Substitute Character
		/// </summary>
		SUB = 0x1A,
		/// <summary>
		/// Escape
		/// </summary>
		ESC = 0x1B,
		/// <summary>
		/// Field Separator
		/// </summary>
		FS  = 0x1C,
		/// <summary>
		/// Group Separator
		/// </summary>
		GS  = 0x1D,
		/// <summary>
		/// Record Separator
		/// </summary>
		RS  = 0x1E,
		/// <summary>
		/// Unit Separator
		/// </summary>
		US  = 0x1F,
		/// <summary>
		/// Spare
		/// </summary>
		SP  = 0x20,
		/// <summary>
		/// Delete
		/// </summary>
		DEL = 0x7F
	}

	/// <summary>
	/// Common serial handshaking protocols
	/// </summary>
	public enum Handshake
	{
		/// <summary>
		/// No handshaking
		/// </summary>
		none,
		/// <summary>
		/// XOn/XOff handshaking
		/// </summary>
		XonXoff,
		/// <summary>
		/// CTS/RTS
		/// </summary>
		CtsRts,
		/// <summary>
		/// DSR/DTR
		/// </summary>
		DsrDtr
	}

	/// <summary>
	/// Parity
	/// </summary>
	public enum Parity 
	{
		/// <summary>
		/// No parity
		/// </summary>
		none	= 0,
		/// <summary>
		/// Odd parity
		/// </summary>
		odd		= 1,
		/// <summary>
		/// Even parity
		/// </summary>
		even	= 2,
		/// <summary>
		/// Mark parity
		/// </summary>
		mark	= 3,
		/// <summary>
		/// Space parity
		/// </summary>
		space	= 4
	};

	/// <summary>
	/// Stop bits
	/// </summary>
	public enum StopBits
	{
		/// <summary>
		/// One stop bit
		/// </summary>
		one				= 0,
		/// <summary>
		/// 1.5 stop bits
		/// </summary>
		onePointFive	= 1,
		/// <summary>
		/// Two stop bits
		/// </summary>
		two				= 2
	};

	/// <summary>
	/// DTR Flow Control
	/// </summary>
	public enum DTRControlFlows
	{
		/// <summary>
		/// Disabled
		/// </summary>
		disable		= 0x00,
		/// <summary>
		/// Enabled
		/// </summary>
		enable		= 0x01,
		/// <summary>
		/// Determined by handshaking
		/// </summary>
		handshake	= 0x02
	}

	/// <summary>
	/// RTS Flow Control
	/// </summary>
	public enum RTSControlFlows
	{
		/// <summary>
		/// Disabled
		/// </summary>
		disable		= 0x00,
		/// <summary>
		/// Enabled
		/// </summary>
		enable		= 0x01,
		/// <summary>
		/// Determined by handshaking
		/// </summary>
		handshake	= 0x02,
		/// <summary>
		/// Toggle
		/// </summary>
		toggle		= 0x03
	}

	/// <summary>
	/// CE-supported baud rates (check your hardware for actual availability)
	/// </summary>
	public enum BaudRates : int
	{
		/// <summary>
		/// 110bpb
		/// </summary>
		CBR_110    = 110,
		/// <summary>
		/// 300bps
		/// </summary>
		CBR_300    = 300,
		/// <summary>
		/// 600bps
		/// </summary>
		CBR_600    = 600,
		/// <summary>
		/// 1200bps
		/// </summary>
		CBR_1200   = 1200,
		/// <summary>
		/// 2400bps
		/// </summary>
		CBR_2400   = 2400,
		/// <summary>
		/// 4800bps
		/// </summary>
		CBR_4800   = 4800,
		/// <summary>
		/// 9600bps
		/// </summary>
		CBR_9600   = 9600,
		/// <summary>
		/// 14.4kbps
		/// </summary>
		CBR_14400  = 14400,
		/// <summary>
		/// 19.2kbps
		/// </summary>
		CBR_19200  = 19200,
		/// <summary>
		/// 38.4kbps
		/// </summary>
		CBR_38400  = 38400,
		/// <summary>
		/// 56kbps
		/// </summary>
		CBR_56000  = 56000,
		/// <summary>
		/// 57.6kbps
		/// </summary>
		CBR_57600  = 57600,
		/// <summary>
		/// 115kbps
		/// </summary>
		CBR_115200 = 115200,
		/// <summary>
		/// 128kbps
		/// </summary>
		CBR_128000 = 128000,
		/// <summary>
		/// 225kbps
		/// </summary>
		CBR_230400 = 230400,
		/// <summary>
		/// 256kbps
		/// </summary>
		CBR_256000 = 256000,
		/// <summary>
		/// 450kbps
		/// </summary>
		CBR_460800= 460800,
		/// <summary>
		/// 900kbps
		/// </summary>
		CBR_921600 = 921600,
	}
	#endregion

	/// <summary>
	/// Used for manipulating several basic Port settings of a Port class
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public class BasicPortSettings
	{
		/// <summary>
		/// Baud rate (default = 19200bps)
		/// </summary>
		public BaudRates	BaudRate	= BaudRates.CBR_19200;
		/// <summary>
		/// Byte Size of data (default = 8)
		/// </summary>
		public byte			ByteSize	= 8;
		/// <summary>
		/// Data Parity (default = none)
		/// </summary>
		public Parity		Parity		= Parity.none;
		/// <summary>
		/// Number of stop bits (default = 1)
		/// </summary>
		public StopBits		StopBits	= StopBits.one;
	}

	/// <summary>
	/// Used for manipulating all settings of a Port class
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public class DetailedPortSettings
	{
		/// <summary>
		/// Create a DetailedPortSettings class
		/// </summary>
		public DetailedPortSettings()
		{
			BasicSettings = new BasicPortSettings();
			Init();
		}

		/// <summary>
		/// These are the default port settings
		/// override Init() to create new defaults (i.e. common handshaking)
		/// </summary>
		protected virtual void Init()
		{
			BasicSettings.BaudRate	= BaudRates.CBR_19200;
			BasicSettings.ByteSize	= 8;
			BasicSettings.Parity	= Parity.none;
			BasicSettings.StopBits	= StopBits.one;

			OutCTS				= false;
			OutDSR				= false;
			DTRControl			= DTRControlFlows.disable;
			DSRSensitive		= false;
			TxContinueOnXOff	= true;
			OutX				= false;
			InX					= false;
			ReplaceErrorChar	= false;
			RTSControl			= RTSControlFlows.disable;
			DiscardNulls		= false;
			AbortOnError		= false;
			XonChar				= (char)ASCII.DC1;
			XoffChar			= (char)ASCII.DC3;		
			ErrorChar			= (char)ASCII.NAK;
			EOFChar				= (char)ASCII.EOT;
			EVTChar				= (char)ASCII.NULL;	
		}

		/// <summary>
		/// Basic port settings
		/// </summary>
		public BasicPortSettings	BasicSettings;
		/// <summary>
		/// Specifies if the CTS (clear-to-send) signal is monitored for output flow control. If this member is TRUE and CTS is turned off, output is suspended until CTS is sent again.
		/// </summary>
		public bool					OutCTS				= false;
		/// <summary>
		/// Specifies if the DSR (data-set-ready) signal is monitored for output flow control. If this member is TRUE and DSR is turned off, output is suspended until DSR is sent again. 
		/// </summary>
		public bool					OutDSR				= false;
		/// <summary>
		/// Specifies the DTR (data-terminal-ready) flow control.
		/// </summary>
		public DTRControlFlows		DTRControl			= DTRControlFlows.disable;
		/// <summary>
		/// Specifies if the communications driver is sensitive to the state of the DSR signal. If this member is TRUE, the driver ignores any bytes received, unless the DSR modem input line is high.
		/// </summary>
		public bool					DSRSensitive		= false;
		/// <summary>
		/// Specifies if transmission stops when the input buffer is full and the driver has transmitted the XoffChar character. If this member is TRUE, transmission continues after the input buffer has come within XoffLim bytes of being full and the driver has transmitted the XoffChar character to stop receiving bytes. If this member is FALSE, transmission does not continue until the input buffer is within XonLim bytes of being empty and the driver has transmitted the XonChar character to resume reception.
		/// </summary>
		public bool					TxContinueOnXOff	= true;
		/// <summary>
		/// Specifies if XON/XOFF flow control is used during transmission. If this member is TRUE, transmission stops when the XoffChar character is received and starts again when the XonChar character is received.
		/// </summary>
		public bool					OutX				= false;
		/// <summary>
		/// Specifies if XON/XOFF flow control is used during reception. If this member is TRUE, the XoffChar character is sent when the input buffer comes within XoffLim bytes of being full, and the XonChar character is sent when the input buffer comes within XonLim bytes of being empty
		/// </summary>
		public bool					InX					= false;
		/// <summary>
		/// Specifies if bytes received with parity errors are replaced with the character specified by the ErrorChar member. If this member is TRUE and the fParity member is TRUE, replacement occurs.
		/// </summary>
		public bool					ReplaceErrorChar	= false;
		/// <summary>
		/// Specifies the RTS (request-to-send) flow control. If this value is zero, the default is RTS_CONTROL_HANDSHAKE. The following table shows possible values for this member.
		/// </summary>
		public RTSControlFlows		RTSControl			= RTSControlFlows.disable;
		/// <summary>
		/// Specifies if null bytes are discarded. If this member is TRUE, null bytes are discarded when received. 
		/// </summary>
		public bool					DiscardNulls		= false;
		/// <summary>
		/// Specifies if read and write operations are terminated if an error occurs. If this member is TRUE, the driver terminates all read and write operations with an error status if an error occurs. The driver will not accept any further communications operations until the application has acknowledged the error by calling the ClearError function.
		/// </summary>
		public bool					AbortOnError		= false;
		/// <summary>
		/// Specifies the value of the XON character for both transmission and reception
		/// </summary>
		public char					XonChar				= (char)ASCII.DC1;
		/// <summary>
		/// Specifies the value of the XOFF character for both transmission and reception.
		/// </summary>
		public char					XoffChar			= (char)ASCII.DC3;
		/// <summary>
		/// Specifies the value of the character used to replace bytes received with a parity error.
		/// </summary>
		public char					ErrorChar			= (char)ASCII.NAK;
		/// <summary>
		/// Specifies the value of the character used to signal the end of data. 
		/// </summary>
		public char					EOFChar				= (char)ASCII.EOT;
		/// <summary>
		/// Specifies the value of the character used to signal an event.
		/// </summary>
		public char					EVTChar				= (char)ASCII.NULL;	
	}

	/// <summary>
	/// A common implementation of DetailedPortSettings for non handshaking
	/// </summary>
	public class HandshakeNone : DetailedPortSettings
	{
		/// <summary>
		/// Initialize the port
		/// </summary>
		protected override void Init()
		{
			base.Init ();

			OutCTS = false;
			OutDSR = false;
			OutX = false;
			InX	= false;
			RTSControl = RTSControlFlows.enable;
			DTRControl = DTRControlFlows.enable;
			TxContinueOnXOff = true;
			DSRSensitive = false;			
		}
	}

	/// <summary>
	/// A common implementation of DetailedPortSettings for XON/XOFF handshaking
	/// </summary>
	public class HandshakeXonXoff : DetailedPortSettings
	{
		/// <summary>
		/// Initialize the port
		/// </summary>
		protected override void Init()
		{
			base.Init ();
			
			OutCTS = false;
			OutDSR = false;
			OutX = true;
			InX	= true;
			RTSControl = RTSControlFlows.enable;
			DTRControl = DTRControlFlows.enable;
			TxContinueOnXOff = true;
			DSRSensitive = false;			
			XonChar = (char)ASCII.DC1; 
			XoffChar = (char)ASCII.DC3;
		}
	}

	/// <summary>
	/// A common implementation of DetailedPortSettings for CTS/RTS handshaking
	/// </summary>
	public class HandshakeCtsRts : DetailedPortSettings
	{
		/// <summary>
		/// Initialize the port
		/// </summary>
		protected override void Init()
		{
			base.Init ();

			OutCTS = true;
			OutDSR = false;
			OutX = false;
			InX	= false;
			RTSControl = RTSControlFlows.handshake;
			DTRControl = DTRControlFlows.enable;
			TxContinueOnXOff = true;
			DSRSensitive = false;			
		}
	}

	/// <summary>
	/// A common implementation of DetailedPortSettings for DSR/DTR handshaking
	/// </summary>
	public class HandshakeDsrDtr : DetailedPortSettings
	{
		/// <summary>
		/// Initialize the port
		/// </summary>
		protected override void Init()
		{
			base.Init ();
			
			OutCTS = false;
			OutDSR = true;
			OutX = false;
			InX	= false;
			RTSControl = RTSControlFlows.enable;
			DTRControl = DTRControlFlows.handshake;
			TxContinueOnXOff = true;
			DSRSensitive = false;			
		}
	}
}
