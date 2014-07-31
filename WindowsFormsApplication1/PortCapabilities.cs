//==========================================================================================
//
//		OpenNETCF.IO.Serial.PortCapabilities
//		Copyright (c) 2004, OpenNETCF.org
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
using System.Collections.Specialized;
namespace OpenNETCF.IO.Serial 
{
	//
	// Serial provider type.
	//
/// <summary>
/// SEP enumerates known serial provider types. Currently SERIALCOMM is the only 
/// provider in this enumeration.
/// </summary>
	[Flags]
	public enum SEP 
	{
		/// <summary>
		/// SERIALCOMM is the only service provider supported by serial APIs.
		/// </summary>
		SEP_SERIALCOMM   = 0x00000001
	};
	//
	// Provider SubTypes
	//
	/// <summary>
	/// PST enumerates the provider subtypes supported by the WIN32 serial APIs. PST indicates which
	/// Port is used for serial communication. Ports can either be physical or logical devices.
	/// </summary>
	public enum PST 
	{
		/// <summary>
		/// no provider subtype specified
		/// </summary>
		PST_UNSPECIFIED     = 0x00000000,
		/// <summary>
		/// RS232 Port
		/// </summary>
		PST_RS232           = 0x00000001,
		/// <summary>
		/// parallel port
		/// </summary>
		PST_PARALLELPORT    = 0x00000002,
		/// <summary>
		/// RS422 Port
		/// </summary>
		PST_RS422           = 0x00000003,
		/// <summary>
		/// RS423 Port
		/// </summary>
		PST_RS423           = 0x00000004,
		/// <summary>
		/// RS449 Port
		/// </summary>
		PST_RS449           = 0x00000005,
		/// <summary>
		/// Modem
		/// </summary>
		PST_MODEM           = 0x00000006,
		/// <summary>
		/// Fax
		/// </summary>
		PST_FAX             = 0x00000021,
		/// <summary>
		/// Scanner
		/// </summary>
		PST_SCANNER         = 0x00000022,
		/// <summary>
		/// unspecified network bridge
		/// </summary>
		PST_NETWORK_BRIDGE  = 0x00000100,
		/// <summary>
		/// DEC's LAT Port
		/// </summary>
		PST_LAT             = 0x00000101,
		/// <summary>
		/// Telnet connection
		/// </summary>
		PST_TCPIP_TELNET    = 0x00000102,
		/// <summary>
		/// X.25 standard
		/// </summary>
		PST_X25             = 0x00000103
	};

	//
	// Provider capabilities flags.
	//
/// <summary>
/// PCF enumerates the provider capabilites supported by the specified COMx: Port. This enumeration
/// is used internaly only. Access to this bitfield information is provided through attributes of the
/// CommProp class.
/// </summary>
	[Flags]
	internal enum PCF
	{
		PCF_DTRDSR       = 0x0001,
		PCF_RTSCTS       = 0x0002,
		PCF_RLSD         = 0x0004,
		PCF_PARITY_CHECK = 0x0008,
		PCF_XONXOFF      = 0x0010,
		PCF_SETXCHAR     = 0x0020,
		PCF_TOTALTIMEOUTS= 0x0040,
		PCF_INTTIMEOUTS  = 0x0080,
		PCF_SPECIALCHARS = 0x0100,
		PCF_16BITMODE    = 0x0200
	};

	//
	// Comm provider settable parameters.
	//
/// <summary>
/// SP 
/// </summary>
	[Flags]
	internal enum SP
	{
		SP_PARITY        = 0x0001,
		SP_BAUD          = 0x0002,
		SP_DATABITS      = 0x0004,
		SP_STOPBITS      = 0x0008,
		SP_HANDSHAKING   = 0x0010,
		SP_PARITY_CHECK  = 0x0020,
		SP_RLSD          = 0x0040
	};

	//
	// Settable baud rates in the provider.
	//
	/// <summary>
	/// baud rates settable by Comm API 
	/// </summary>
	[Flags]
	public enum BAUD
	{
		/// <summary>
		/// 75 bits per second
		/// </summary>
		BAUD_075         = 0x00000001,
		/// <summary>
		/// 110 bits per second
		/// </summary>
		BAUD_110         = 0x00000002,
		/// <summary>
		/// 134.5 bits per second
		/// </summary>
		BAUD_134_5       = 0x00000004,
		/// <summary>
		/// 150 bits per second
		/// </summary>
		BAUD_150         = 0x00000008,
		/// <summary>
		/// 300 bits per second
		/// </summary>
		BAUD_300         = 0x00000010,
		/// <summary>
		/// 600 bits per second
		/// </summary>
		BAUD_600         = 0x00000020,
		/// <summary>
		/// 1,200 bits per second
		/// </summary>
		BAUD_1200        = 0x00000040,
		/// <summary>
		/// 1,800 bits per second
		/// </summary>
		BAUD_1800        = 0x00000080,
		/// <summary>
		/// 2,400 bits per second
		/// </summary>
		BAUD_2400        = 0x00000100,
		/// <summary>
		/// 4,800 bits per second
		/// </summary>
		BAUD_4800        = 0x00000200,
		/// <summary>
		/// 7,200 bits per second
		/// </summary>
		BAUD_7200        = 0x00000400,
		/// <summary>
		/// 9,600 bits per second
		/// </summary>
		BAUD_9600        = 0x00000800,
		/// <summary>
		/// 14,400 bits per second
		/// </summary>
		BAUD_14400       = 0x00001000,
		/// <summary>
		/// 19,200 bits per second
		/// </summary>
		BAUD_19200       = 0x00002000,
		/// <summary>
		/// 38,400 bits per second
		/// </summary>
		BAUD_38400       = 0x00004000,
		/// <summary>
		/// 56 Kbits per second
		/// </summary>
		BAUD_56K         = 0x00008000,
		/// <summary>
		/// 129 Kbits per second
		/// </summary>
		BAUD_128K        = 0x00010000,
		/// <summary>
		/// 115,200 bits per second
		/// </summary>
		BAUD_115200      = 0x00020000,
		/// <summary>
		/// 57,600 bits per second
		/// </summary>
		BAUD_57600       = 0x00040000,
		/// <summary>
		/// User defined bitrates
		/// </summary>
		BAUD_USER        = 0x10000000
	};
	//
	// Settable Data Bits
	//

	[Flags]
	internal enum DB 
	{
		DATABITS_5       = 0x0001,
		DATABITS_6       = 0x0002,
		DATABITS_7       = 0x0004,
		DATABITS_8       = 0x0008,
		DATABITS_16      = 0x0010,
		DATABITS_16X     = 0x0020
	};
	//
	// Settable Stop and Parity bits.
	//
	[Flags]
	internal enum SB 
	{
		STOPBITS_10      = 0x00010000,
		STOPBITS_15      = 0x00020000,
		STOPBITS_20      = 0x00040000,
		PARITY_NONE      = 0x01000000,
		PARITY_ODD       = 0x02000000,
		PARITY_EVEN      = 0x04000000,
		PARITY_MARK      = 0x08000000,
		PARITY_SPACE     = 0x10000000
	};
	//
	// Set dwProvSpec1 to COMMPROP_INITIALIZED to indicate that wPacketLength
	// is valid when calling GetCommProperties().
	//
	[Flags]
	internal enum CPS:uint 
	{
		COMMPROP_INITIALIZED= 0xE73CF52E
	};
	/// <summary>
	/// Container for all available information on port's capabilties 
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public class CommCapabilities 
	{
		private UInt16 wPacketLength;
		private UInt16 wPacketVersion;
		/// <summary>
		/// Indicates which services are supported by the port. SP_SERIALCOMM is specified for communication
		/// providers, including modem providers.
		/// </summary>
		public IO.Serial.SEP dwServiceMask;
		private UInt32 dwReserved1;
		/// <summary>
		/// Specifies the maximum size, in bytes, of the driver's internal output buffer. A value of zero
		/// indicates that no maximum value is imposed by the driver.
		/// </summary>
		[CLSCompliant(false)]
		public UInt32 dwMaxTxQueue;
		/// <summary>
		/// Specifies the maximum size, in bytes, of the driver's internal input buffer. A value of zero
		/// indicates that no maximum value is imposed by the driver.
		/// </summary>
		[CLSCompliant(false)]
		public UInt32 dwMaxRxQueue;
		/// <summary>
		/// Specifies the maximum baud rate, in bits per second (bps).
		/// </summary>
		public IO.Serial.BAUD dwMaxBaud;
		/// <summary>
		/// Specifies the communication provider type.
		/// </summary>
		public IO.Serial.PST dwProvSubType;
		private BitVector32 dwProvCapabilities;
		private BitVector32 dwSettableParams;
		private BitVector32 dwSettableBaud;
		private BitVector32 dwSettableStopParityData;
		/// <summary>
		/// Specifies the size, in bytes, of the driver's internal output buffer. A value of zero indicates 
		/// that the value is unavailable.
		/// </summary>
		[CLSCompliant(false)]
		public UInt32 dwCurrentTxQueue;
		/// <summary>
		/// Specifies the size, in bytes, of the driver's internal input buffer. A value of zero indicates 
		/// that the value is unavailable.
		/// </summary>
		[CLSCompliant(false)]
		public UInt32 dwCurrentRxQueue;
		private IO.Serial.CPS dwProvSpec1;
		private UInt32 dwProvSpec2;
		private UInt16 wcProvChar;

		internal CommCapabilities()
		{
			this.wPacketLength=(ushort)Marshal.SizeOf(this);
			this.dwProvSpec1=CPS.COMMPROP_INITIALIZED;

			dwProvCapabilities=new BitVector32(0);
			dwSettableParams=new BitVector32(0);
			dwSettableBaud=new BitVector32(0);
			dwSettableStopParityData=new BitVector32(0);
		}
		
		//
		// We need to have to define reserved fields in the CommCapabilties class definition
		// to preserve the size of the 
		// underlying structure to match the Win32 structure when it is 
		// marshaled. Use these fields to suppress compiler warnings.
		//
		internal void _SuppressCompilerWarnings()
		{
			wPacketVersion +=0;
			dwReserved1 +=0;
			dwProvSpec1 +=0;
			dwProvSpec2 +=0;
			wcProvChar +=0;
		}
		
		// Provider Capabilties
		/// <summary>
		/// Port supports special 16-bit mode
		/// </summary>
		public bool Supports16BitMode 
		{
			get { return dwProvCapabilities[(int)PCF.PCF_16BITMODE]; }
		}
		
		/// <summary>
		/// Port supports DTR (Data Terminal ready) and DSR (Data Set Ready) flow control
		/// </summary>
		public bool SupportsDtrDts 
		{
			get { return dwProvCapabilities[(int)PCF.PCF_DTRDSR]; }
		}

		/// <summary>
		/// Port supports interval timeouts
		/// </summary>
		public bool SupportsIntTimeouts 
		{
			get { return dwProvCapabilities[(int)PCF.PCF_INTTIMEOUTS]; }
		}

		/// <summary>
		/// Port supports parity checking
		/// </summary>
		public bool SupportsParityCheck
		{
			get { return dwProvCapabilities[(int)PCF.PCF_PARITY_CHECK]; }
		}

		/// <summary>
		/// Port supports RLSD (Receive Line Signal Detect)
		/// </summary>
		public bool SupportsRlsd 
		{
			get { return dwProvCapabilities[(int)PCF.PCF_RLSD]; }
		}
		
		/// <summary>
		/// Port supports RTS (Request To Send) and CTS (Clear To Send) flowcontrol
		/// </summary>
		public bool SupportsRtsCts 
		{
			get { return dwProvCapabilities[(int)PCF.PCF_RTSCTS]; }
		}

		/// <summary>
		/// Port supports user definded characters for XON and XOFF
		/// </summary>
		public bool SupportsSetXChar 
		{
			get { return dwProvCapabilities[(int)PCF.PCF_SETXCHAR]; }
		}

		/// <summary>
		/// Port supports special characters
		/// </summary>
		public bool SupportsSpecialChars 
		{
			get { return dwProvCapabilities[(int)PCF.PCF_SPECIALCHARS]; }
		}

		/// <summary>
		/// Port supports total and elapsed time-outs
		/// </summary>
		public bool SupportsTotalTimeouts 
		{
			get { return dwProvCapabilities[(int)PCF.PCF_TOTALTIMEOUTS]; }
		}

		/// <summary>
		/// Port supports XON/XOFF flow control
		/// </summary>
		public bool SupportsXonXoff 
		{
			get { return dwProvCapabilities[(int)PCF.PCF_XONXOFF]; }
		}

		// Settable Params
		/// <summary>
		/// Baud rate can be set
		/// </summary>
		public bool SettableBaud 
		{
			get { return dwSettableParams[(int)SP.SP_BAUD]; }
		}
		
		/// <summary>
		/// Number of data bits can be set
		/// </summary>
		public bool SettableDataBits 
		{
			get { return dwSettableParams[(int)SP.SP_DATABITS]; }
		}

		/// <summary>
		/// Handshake protocol can be set
		/// </summary>
		public bool SettableHandShaking 
		{
			get { return dwSettableParams[(int)SP.SP_HANDSHAKING]; }
		}

		/// <summary>
		/// Number of parity bits can be set
		/// </summary>
		public bool SettableParity 
		{
			get { return dwSettableParams[(int)SP.SP_PARITY]; }
		}

		/// <summary>
		/// Parity check can be enabled/disabled
		/// </summary>
		public bool SettableParityCheck 
		{
			get { return dwSettableParams[(int)SP.SP_PARITY_CHECK]; }
		}
		/// <summary>
		/// Receive Line Signal detect can be enabled/disabled
		/// </summary>
		public bool SettableRlsd 
		{
			get { return dwSettableParams[(int)SP.SP_RLSD]; }
		}
		/// <summary>
		/// Number of stop bits can be set
		/// </summary>
		public bool SettableStopBits 
		{
			get { return dwSettableParams[(int)SP.SP_STOPBITS]; }
		}

		// Settable Databits
		/// <summary>
		/// Port supports 5 data bits
		/// </summary>
		public bool Supports5DataBits 
		{
			get { return dwSettableStopParityData[(int)DB.DATABITS_5]; }
		}

		/// <summary>
		/// Port supports 6 data bits
		/// </summary>
		public bool Supports6DataBits 
		{
			get { return dwSettableStopParityData[(int)DB.DATABITS_6]; }
		}

		/// <summary>
		/// Port supports 7 data bits
		/// </summary>
		public bool Supports7DataBits 
		{
			get { return dwSettableStopParityData[(int)DB.DATABITS_7]; }
		}
		
		/// <summary>
		/// Port supports 8 data bits
		/// </summary>
		public bool Supports8DataBits 
		{
			get { return dwSettableStopParityData[(int)DB.DATABITS_8]; }
		}
		
		/// <summary>
		/// Port supports 16 data bits
		/// </summary>
		public bool Supports16DataBits 
		{
			get { return dwSettableStopParityData[(int)DB.DATABITS_16]; }
		}
		
		/// <summary>
		/// Port supports special wide data path through serial hardware lines
		/// </summary>
		public bool Supports16XDataBits 
		{
			get { return dwSettableStopParityData[(int)DB.DATABITS_16X]; }
		}

		// Settable Stop
		
		/// <summary>
		/// Port supports even parity
		/// </summary>
		public bool SupportsParityEven 
		{
			get { return dwSettableStopParityData[(int)SB.PARITY_EVEN]; }
		}
		
		/// <summary>
		/// Port supports mark parity
		/// </summary>
		public bool SupportsParityMark 
		{
			get { return dwSettableStopParityData[(int)SB.PARITY_MARK]; }
		}

		/// <summary>
		/// Port supports none parity
		/// </summary>
		public bool SupportsParityNone 
		{
			get { return dwSettableStopParityData[(int)SB.PARITY_NONE]; }
		}

		/// <summary>
		/// Port supports odd parity
		/// </summary>
		public bool SupportsParityOdd 
		{
			get { return dwSettableStopParityData[(int)SB.PARITY_ODD]; }
		}

		/// <summary>
		/// Port supports space parity
		/// </summary>
		public bool SupportsParitySpace 
		{
			get { return dwSettableStopParityData[(int)SB.PARITY_SPACE]; }
		}

		/// <summary>
		/// Port supports 1 stop bit
		/// </summary>
		public bool SupportsStopBits10 
		{
			get { return dwSettableStopParityData[(int)SB.STOPBITS_10]; }
		}
		
		/// <summary>
		/// Port supports 1.5 stop bits
		/// </summary>
		public bool SupportsStopBits15 
		{
			get { return dwSettableStopParityData[(int)SB.STOPBITS_15]; }
		}

		/// <summary>
		/// Port supports 2 stop bits
		/// </summary>
		public bool SupportsStopBits20 
		{
			get { return dwSettableStopParityData[(int)SB.STOPBITS_20]; }
		}

		// settable Baud Rates
		/// <summary>
		/// Port can be set to 75 bits per second
		/// </summary>
		public bool HasBaud75
		{
			get { return dwSettableBaud[(int)BAUD.BAUD_075];}
		}
		/// <summary>
		/// Port can be set to 110 bits per second
		/// </summary>
		public bool HasBaud110
		{
			get { return dwSettableBaud[(int)BAUD.BAUD_110];}
		}
		/// <summary>
		/// Port can be set to 134.5 bits per second
		/// </summary>
		public bool HasBaud134_5
		{
			get { return dwSettableBaud[(int)BAUD.BAUD_134_5];}
		}
		/// <summary>
		/// Port can be set to 150 bits per second
		/// </summary>
		public bool HasBaud150
		{
			get { return dwSettableBaud[(int)BAUD.BAUD_150];}
		}

		/// <summary>
		/// Port can be set to 300 bits per second
		/// </summary>
		public bool HasBaud300
		{
			get { return dwSettableBaud[(int)BAUD.BAUD_300];}
		}

		/// <summary>
		/// Port can be set to 600 bits per second
		/// </summary>
		public bool HasBaud600
		{
			get { return dwSettableBaud[(int)BAUD.BAUD_600];}
		}

		/// <summary>
		/// Port can be set to 1,200 bits per second
		/// </summary>
		public bool HasBaud1200
		{
			get { return dwSettableBaud[(int)BAUD.BAUD_1200];}
		}
		
		/// <summary>
		/// Port can be set to 2,400 bits per second
		/// </summary>
		public bool HasBaud2400
		{
			get { return dwSettableBaud[(int)BAUD.BAUD_2400];}
		}

		/// <summary>
		/// Port can be set to 4,800 bits per second
		/// </summary>
		public bool HasBaud4800
		{
			get { return dwSettableBaud[(int)BAUD.BAUD_4800];}
		}

		/// <summary>
		/// Port can be set to 7,200 bits per second
		/// </summary>
		public bool HasBaud7200
		{
			get { return dwSettableBaud[(int)BAUD.BAUD_7200];}
		}

		/// <summary>
		/// Port can be set to 9,600 bits per second
		/// </summary>
		public bool HasBaud9600
		{
			get { return dwSettableBaud[(int)BAUD.BAUD_9600];}
		}

		/// <summary>
		/// Port can be set to 14,400 bits per second
		/// </summary>
		public bool HasBaud14400
		{
			get { return dwSettableBaud[(int)BAUD.BAUD_14400];}
		}

		/// <summary>
		/// Port can be set to 19,200 bits per second
		/// </summary>
		public bool HasBaud19200
		{
			get { return dwSettableBaud[(int)BAUD.BAUD_19200];}
		}

		/// <summary>
		/// Port can be set to 38,400 bits per second
		/// </summary>
		public bool HasBaud38400
		{
			get { return dwSettableBaud[(int)BAUD.BAUD_38400];}
		}

		/// <summary>
		/// Port can be set to 56 Kbits per second
		/// </summary>
		public bool HasBaud56K
		{
			get { return dwSettableBaud[(int)BAUD.BAUD_56K];}
		}
		
		/// <summary>
		/// Port can be set to 128 Kbits per second
		/// </summary>
		public bool HasBaud128K
		{
			get { return dwSettableBaud[(int)BAUD.BAUD_128K];}
		}
		
		/// <summary>
		/// Port can be set to 115,200 bits per second
		/// </summary>
		public bool HasBaud115200
		{
			get { return dwSettableBaud[(int)BAUD.BAUD_115200];}
		}
		
		/// <summary>
		/// Port can be set to 57,600 bits per second
		/// </summary>
		public bool HasBaud57600
		{
			get { return dwSettableBaud[(int)BAUD.BAUD_57600];}
		}

		/// <summary>
		/// Port can be set to user defined bit rate
		/// </summary>
		public bool HasBaudUser
		{
			get { return dwSettableBaud[(int)BAUD.BAUD_USER];}
		}

	};

	
}