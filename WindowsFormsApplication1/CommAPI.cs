//==========================================================================================
//
//		namespace OpenNETCF.IO.Serial.CommAPI
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
//			Added FlushFileBuffers wrappers to Win32 and CE platforms
//

using System;
using System.Runtime.InteropServices;
using System.Collections.Specialized;

namespace OpenNETCF.IO.Serial
{
	#region API structs and enums

	[StructLayout(LayoutKind.Sequential)]
	internal class CommTimeouts
	{
		public UInt32 ReadIntervalTimeout;
		public UInt32 ReadTotalTimeoutMultiplier;
		public UInt32 ReadTotalTimeoutConstant;
		public UInt32 WriteTotalTimeoutMultiplier;
		public UInt32 WriteTotalTimeoutConstant;
	}

	[StructLayout( LayoutKind.Sequential )]
	internal struct OVERLAPPED
	{
		internal UIntPtr Internal;
		internal UIntPtr InternalHigh;
		internal UInt32 Offset;
		internal UInt32 OffsetHigh;
		internal IntPtr hEvent;
	}

	/// <summary>
	/// Event Flags
	/// </summary>
	[Flags]
	public enum CommEventFlags : int
	{
		/// <summary>
		/// No flags
		/// </summary>
		NONE        = 0x0000, //
		/// <summary>
		/// Event on receive
		/// </summary>
		RXCHAR      = 0x0001, // Any Character received
		/// <summary>
		/// Event when specific character is received
		/// </summary>
		RXFLAG      = 0x0002, // Received specified flag character
		/// <summary>
		/// Event when the transmit buffer is empty
		/// </summary>
		TXEMPTY     = 0x0004, // Tx buffer Empty
		/// <summary>
		/// Event on CTS state change
		/// </summary>
		CTS         = 0x0008, // CTS changed
		/// <summary>
		/// Event on DSR state change
		/// </summary>
		DSR         = 0x0010, // DSR changed
		/// <summary>
		/// Event on RLSD state change
		/// </summary>
		RLSD        = 0x0020, // RLSD changed
		/// <summary>
		/// Event on BREAK
		/// </summary>
		BREAK       = 0x0040, // BREAK received
		/// <summary>
		/// Event on line error
		/// </summary>
		ERR         = 0x0080, // Line status error
		/// <summary>
		/// Event on ring detect
		/// </summary>
		RING        = 0x0100, // ring detected
		/// <summary>
		/// Event on printer error
		/// </summary>
		PERR        = 0x0200, // printer error
		/// <summary>
		/// Event on 80% high-water
		/// </summary>
		RX80FULL    = 0x0400, // rx buffer is at 80%
		/// <summary>
		/// Provider event 1
		/// </summary>
		EVENT1      = 0x0800, // provider event
		/// <summary>
		/// Provider event 2
		/// </summary>
		EVENT2      = 0x1000, // provider event
		/// <summary>
		/// Event on CE power notification
		/// </summary>
		POWER       = 0x2000, // wince power notification
		/// <summary>
		/// Mask for all flags under CE
		/// </summary>
		ALLCE			= 0x3FFF,  // mask of all flags for CE
		/// <summary>
		/// Mask for all flags under desktop Windows
		/// </summary>
		ALLPC			= BREAK | CTS | DSR | ERR | RING | RLSD | RXCHAR | RXFLAG | TXEMPTY
	}

	internal enum EventFlags
	{
		EVENT_PULSE     = 1,
		EVENT_RESET     = 2,
		EVENT_SET       = 3
	}

	/// <summary>
	/// Error flags
	/// </summary>
	[Flags]
	public enum CommErrorFlags : int
	{
		/// <summary>
		/// Receive overrun
		/// </summary>
		RXOVER = 0x0001,
		/// <summary>
		/// Overrun
		/// </summary>
		OVERRUN = 0x0002,
		/// <summary>
		/// Parity error
		/// </summary>
		RXPARITY = 0x0004,
		/// <summary>
		/// Frame error
		/// </summary>
		FRAME = 0x0008,
		/// <summary>
		/// BREAK received
		/// </summary>
		BREAK = 0x0010,
		/// <summary>
		/// Transmit buffer full
		/// </summary>
		TXFULL = 0x0100,
		/// <summary>
		/// IO Error
		/// </summary>
		IOE = 0x0400,
		/// <summary>
		/// Requested mode not supported
		/// </summary>
		MODE = 0x8000
	}

	/// <summary>
	/// Modem status flags
	/// </summary>
	[Flags]
	public enum CommModemStatusFlags : int
	{
		/// <summary>
		/// The CTS (Clear To Send) signal is on.
		/// </summary>
		MS_CTS_ON	= 0x0010,
		/// <summary>
		/// The DSR (Data Set Ready) signal is on.
		/// </summary>
		MS_DSR_ON	= 0x0020,
		/// <summary>
		/// The ring indicator signal is on.
		/// </summary>
		MS_RING_ON	= 0x0040,
		/// <summary>
		/// The RLSD (Receive Line Signal Detect) signal is on.
		/// </summary>
		MS_RLSD_ON	= 0x0080
	}

	/// <summary>
	/// Communication escapes
	/// </summary>
	internal enum CommEscapes : uint
	{
		/// <summary>
		/// Causes transmission to act as if an XOFF character has been received.
		/// </summary>
		SETXOFF		= 1,
		/// <summary>
		/// Causes transmission to act as if an XON character has been received.
		/// </summary>
		SETXON		= 2,
		/// <summary>
		/// Sends the RTS (Request To Send) signal.
		/// </summary>
		SETRTS		= 3,
		/// <summary>
		/// Clears the RTS (Request To Send) signal
		/// </summary>
		CLRRTS		= 4,
		/// <summary>
		/// Sends the DTR (Data Terminal Ready) signal.
		/// </summary>
		SETDTR		= 5,
		/// <summary>
		/// Clears the DTR (Data Terminal Ready) signal.
		/// </summary>
		CLRDTR		= 6,
		/// <summary>
		/// Suspends character transmission and places the transmission line in a break state until the ClearCommBreak function is called (or EscapeCommFunction is called with the CLRBREAK extended function code). The SETBREAK extended function code is identical to the SetCommBreak function. This extended function does not flush data that has not been transmitted.
		/// </summary>
		SETBREAK	= 8,
		/// <summary>
		/// Restores character transmission and places the transmission line in a nonbreak state. The CLRBREAK extended function code is identical to the ClearCommBreak function
		/// </summary>
		CLRBREAK	= 9,
		///Set the port to IR mode.
		SETIR		= 10,
		/// <summary>
		/// Set the port to non-IR mode.
		/// </summary>
		CLRIR		= 11
	}

	/// <summary>
	/// Error values from serial API calls
	/// </summary>
	internal enum APIErrors : int
	{
		/// <summary>
		/// Port not found
		/// </summary>
		ERROR_FILE_NOT_FOUND	= 2,
		/// <summary>
		/// Invalid port name
		/// </summary>
		ERROR_INVALID_NAME		= 123,
		/// <summary>
		/// Access denied
		/// </summary>
		ERROR_ACCESS_DENIED		= 5,
		/// <summary>
		/// invalid handle
		/// </summary>
		ERROR_INVALID_HANDLE	= 6,
		/// <summary>
		/// IO pending
		/// </summary>
		ERROR_IO_PENDING		= 997
	}

	internal enum APIConstants : uint
	{
		WAIT_OBJECT_0   	= 0x00000000,
		WAIT_ABANDONED  	= 0x00000080,
		WAIT_ABANDONED_0	= 0x00000080,
		WAIT_FAILED         = 0xffffffff,
		INFINITE            = 0xffffffff
	}
	#endregion


	[StructLayout(LayoutKind.Sequential)]
	internal class CommStat
	{
		//
		// typedef struct _COMSTAT {
		//     DWORD fCtsHold : 1;
		//     DWORD fDsrHold : 1;
		//     DWORD fRlsdHold : 1;
		//     DWORD fXoffHold : 1;
		//     DWORD fXoffSent : 1;
		//     DWORD fEof : 1;
		//     DWORD fTxim : 1;
		//     DWORD fReserved : 25;
		//     DWORD cbInQue;
		//     DWORD cbOutQue;
		// } COMSTAT, *LPCOMSTAT;
		//
		private BitVector32 bitfield = new BitVector32(0); // UKI added for CLR bitfield support
		public UInt32 cbInQue	= 0;
		public UInt32 cbOutQue	= 0;

		// Helper constants for manipulating the bit fields.

		[Flags]
		private enum commFlags
		{
			fCtsHoldMask  = 0x01,
			fDsrHoldMask  = 0x02,
			fRlsdHoldMask = 0x04,
			fXoffHoldMask = 0x08,
			fXoffSentMask = 0x10,
			fEofMask	  = 0x20,
			fTximMask	  = 0x40

		};

		public bool fCtsHold
		{
			get { return bitfield[(int)commFlags.fCtsHoldMask]; }
			set { bitfield[(int)commFlags.fCtsHoldMask]=value; }
		}
		public bool fDsrHold
		{
			get { return bitfield[(int)commFlags.fDsrHoldMask]; }
			set { bitfield[(int)commFlags.fDsrHoldMask] = value; }
		}
		public bool fRlsdHold
		{
			get { return bitfield[(int)commFlags.fRlsdHoldMask]; }
			set { bitfield[(int)commFlags.fRlsdHoldMask]= value; }
		}
		public bool fXoffHold
		{
			get { return bitfield[(int)commFlags.fXoffHoldMask]; }
			set { bitfield[(int)commFlags.fXoffHoldMask]=value; }
		}
		public bool fXoffSent
		{
			get { return bitfield[(int)commFlags.fXoffSentMask]; }
			set { bitfield[(int)commFlags.fXoffSentMask]= value; }
		}
		public bool fEof
		{
			get { return bitfield[(int)commFlags.fEofMask]; }
			set { bitfield[(int)commFlags.fEofMask] = value; }
		}
		public bool fTxim
		{
			get { return bitfield[(int)commFlags.fTximMask]; }
			set { bitfield[(int)commFlags.fTximMask] = value; }
		}
	}

	#region CommAPI base class
	internal abstract class CommAPI
	{
		// These functions wrap the P/Invoked API calls and:
		// - make the correct call based on whether we're running under the full or compact framework
		// - eliminate empty parameters and defaults
		//
		static internal bool bFullFramework;

		static CommAPI()
		{
			bFullFramework=System.Environment.OSVersion.Platform != PlatformID.WinCE;
		}

		#region ----- virtual function declarations -----
		internal virtual IntPtr CreateFile(string FileName){return (IntPtr)0L;}
		internal virtual IntPtr QueryFile(string FileName){return (IntPtr)0L;}
		internal virtual bool WaitCommEvent(IntPtr hPort, ref CommEventFlags flags){return false;}
		internal virtual bool ClearCommError(IntPtr hPort, ref CommErrorFlags flags, CommStat stat){return false;}
		internal virtual bool GetCommModemStatus(IntPtr hPort, ref uint lpModemStat){return false;}
		internal virtual bool SetCommMask(IntPtr hPort, CommEventFlags dwEvtMask) {return false;}
		internal virtual bool ReadFile(IntPtr hPort, byte[] buffer, int cbToRead, ref Int32 cbRead, IntPtr lpOverlapped) {return false;}
		internal virtual bool WriteFile(IntPtr hPort, byte[] buffer, Int32 cbToWrite, ref Int32 cbWritten, IntPtr lpOverlapped) {return false;}
		internal virtual bool CloseHandle(IntPtr hPort) {return false;}
		internal virtual bool SetupComm(IntPtr hPort, Int32 dwInQueue, Int32 dwOutQueue) {return false;}
		internal virtual bool SetCommState(IntPtr hPort, DCB dcb){return false;}
		internal virtual bool GetCommState(IntPtr hPort, DCB dcb){return false;}
		internal virtual bool SetCommTimeouts(IntPtr hPort, CommTimeouts timeouts) {return false;}
		internal virtual  bool EscapeCommFunction(IntPtr hPort, CommEscapes escape){return false;}
		internal virtual  IntPtr CreateEvent(bool bManualReset, bool bInitialState, string lpName) {return (IntPtr)0L;}
		internal virtual  bool SetEvent(IntPtr hEvent) {return false;}
		internal virtual  bool ResetEvent(IntPtr hEvent) {return false;}
		internal virtual  bool PulseEvent(IntPtr hEvent) {return false;}
		internal virtual  int WaitForSingleObject(IntPtr hHandle, uint dwMilliseconds) {return 0;}
		internal virtual bool GetCommProperties(IntPtr hPort, CommCapabilities commcap) {return false;}
		internal virtual bool FlushFileBuffers(IntPtr hFile) {return false;}

		#endregion

		#region Helper Property

		static internal bool FullFramework
		{
			get{return bFullFramework;}
		}
		#endregion

		#region API Constants
		internal const Int32 INVALID_HANDLE_VALUE = -1;
		internal const UInt32 OPEN_EXISTING = 3;
		internal const UInt32 GENERIC_READ = 0x80000000;
		internal const UInt32 GENERIC_WRITE = 0x40000000;
		internal const UInt32 FILE_FLAG_OVERLAPPED = 0x40000000;
		internal const UInt32 CreateAccess = GENERIC_WRITE | GENERIC_READ;
		#endregion

	}
	#endregion

	#region CE CompactFramework (cf) implementation for CommAPI
	internal class CECommAPI : CommAPI
	{
		override internal  IntPtr CreateFile(string FileName)
		{
			return CECreateFileW(FileName, CreateAccess, 0, IntPtr.Zero, OPEN_EXISTING, 0, IntPtr.Zero);
		}

		// setting AccessMask to 0 returns a handle we can use to query the device without accessing it
		override internal  IntPtr QueryFile(string FileName)
		{
			return CECreateFileW(FileName, 0, 0, IntPtr.Zero, OPEN_EXISTING, 0, IntPtr.Zero);
		}

		override internal  bool WaitCommEvent(IntPtr hPort, ref CommEventFlags flags)
		{
			return Convert.ToBoolean(CEWaitCommEvent(hPort, ref flags, IntPtr.Zero));
		}

		override internal  bool ClearCommError(IntPtr hPort, ref CommErrorFlags flags, CommStat stat)
		{
			return Convert.ToBoolean(CEClearCommError(hPort, ref flags, stat));
		}

		override internal  bool GetCommModemStatus(IntPtr hPort, ref uint lpModemStat)
		{
			return Convert.ToBoolean(CEGetCommModemStatus(hPort, ref lpModemStat));
		}

		override internal  bool SetCommMask(IntPtr hPort, CommEventFlags dwEvtMask)
		{
			return Convert.ToBoolean(CESetCommMask(hPort, dwEvtMask));
		}

		override internal  bool ReadFile(IntPtr hPort, byte[] buffer, int cbToRead, ref Int32 cbRead, IntPtr lpOverlapped)
		{
			return Convert.ToBoolean(CEReadFile(hPort, buffer, cbToRead, ref cbRead, IntPtr.Zero));
		}

		override internal  bool WriteFile(IntPtr hPort, byte[] buffer, Int32 cbToWrite, ref Int32 cbWritten, IntPtr lpOverlapped)
		{
			return Convert.ToBoolean(CEWriteFile(hPort, buffer, cbToWrite, ref cbWritten, IntPtr.Zero));
		}

		override internal  bool CloseHandle(IntPtr hPort)
		{
			return Convert.ToBoolean(CECloseHandle(hPort));
		}

		override internal  bool SetupComm(IntPtr hPort, Int32 dwInQueue, Int32 dwOutQueue)
		{
			return Convert.ToBoolean(CESetupComm(hPort, dwInQueue, dwOutQueue));
		}

		override internal  bool SetCommState(IntPtr hPort, DCB dcb)
		{
			return Convert.ToBoolean(CESetCommState(hPort, dcb));
		}

		override internal  bool GetCommState(IntPtr hPort, DCB dcb)
		{
			return Convert.ToBoolean(CEGetCommState(hPort, dcb));
		}

		override internal  bool SetCommTimeouts(IntPtr hPort, CommTimeouts timeouts)
		{
			return Convert.ToBoolean(CESetCommTimeouts(hPort, timeouts));
		}

		override internal  bool EscapeCommFunction(IntPtr hPort, CommEscapes escape)
		{
			return Convert.ToBoolean(CEEscapeCommFunction(hPort, (uint)escape));
		}

		override internal  IntPtr CreateEvent(bool bManualReset, bool bInitialState, string lpName)
		{
			return CECreateEvent(IntPtr.Zero, Convert.ToInt32(bManualReset), Convert.ToInt32(bInitialState), lpName);
		}

		override internal  bool SetEvent(IntPtr hEvent)
		{
			return Convert.ToBoolean(CEEventModify(hEvent, (uint)EventFlags.EVENT_SET));
		}

		override internal  bool ResetEvent(IntPtr hEvent)
		{
			return Convert.ToBoolean(CEEventModify(hEvent, (uint)EventFlags.EVENT_RESET));
		}

		override internal bool PulseEvent(IntPtr hEvent)
		{
			return Convert.ToBoolean(CEEventModify(hEvent, (uint)EventFlags.EVENT_PULSE));
		}

		override internal int WaitForSingleObject(IntPtr hHandle, uint dwMilliseconds)
		{
			return CEWaitForSingleObject(hHandle, dwMilliseconds);
		}

		override internal  bool GetCommProperties(IntPtr hPort, CommCapabilities commcap)
		{
			return Convert.ToBoolean(CEGetCommProperties(hPort, commcap));
		}

		override internal bool FlushFileBuffers(IntPtr hFile)
		{
			return Convert.ToBoolean(CEFlushFileBuffers(hFile));
		}

		#region Windows CE API imports
// my add
//		[DllImport("coredll.dll", EntryPoint="Sleep", SetLastError = true)]
//		private static extern int Sleep(uint dwMilliseconds);
//
		[DllImport("coredll.dll", EntryPoint="WaitForSingleObject", SetLastError = true)]
		private static extern int CEWaitForSingleObject(IntPtr hHandle, uint dwMilliseconds);

		[DllImport("coredll.dll", EntryPoint="EventModify", SetLastError = true)]
		private static extern int CEEventModify(IntPtr hEvent, uint function);

		[DllImport("coredll.dll", EntryPoint="CreateEvent", SetLastError = true)]
		private static extern IntPtr CECreateEvent(IntPtr lpEventAttributes, int bManualReset, int bInitialState, string lpName);

		[DllImport("coredll.dll", EntryPoint="EscapeCommFunction", SetLastError = true)]
		private static extern int CEEscapeCommFunction(IntPtr hFile, UInt32 dwFunc);

		[DllImport("coredll.dll", EntryPoint="SetCommTimeouts", SetLastError = true)]
		private static extern int CESetCommTimeouts(IntPtr hFile, CommTimeouts timeouts);

		[DllImport("coredll.dll", EntryPoint="GetCommState", SetLastError = true)]
		private static extern int CEGetCommState(IntPtr hFile, DCB dcb);

		[DllImport("coredll.dll", EntryPoint="SetCommState", SetLastError = true)]
		private static extern int CESetCommState(IntPtr hFile, DCB dcb);

		[DllImport("coredll.dll", EntryPoint="SetupComm", SetLastError = true)]
		private static extern int CESetupComm(IntPtr hFile, Int32 dwInQueue, Int32 dwOutQueue);

		[DllImport("coredll.dll", EntryPoint="CloseHandle", SetLastError = true)]
		private static extern int CECloseHandle(IntPtr hObject);

		[DllImport("coredll.dll", EntryPoint="WriteFile", SetLastError = true)]
		private static extern int CEWriteFile(IntPtr hFile, byte[] lpBuffer, Int32 nNumberOfBytesToRead, ref Int32 lpNumberOfBytesRead, IntPtr lpOverlapped);

		[DllImport("coredll.dll", EntryPoint="ReadFile", SetLastError = true)]
		private static extern int CEReadFile(IntPtr hFile, byte[] lpBuffer, Int32 nNumberOfBytesToRead, ref Int32 lpNumberOfBytesRead, IntPtr lpOverlapped);

		[DllImport("coredll.dll", EntryPoint="SetCommMask", SetLastError = true)]
		private static extern int CESetCommMask(IntPtr handle, CommEventFlags dwEvtMask);

		[DllImport("coredll.dll", EntryPoint="GetCommModemStatus", SetLastError = true)]
		private static extern int CEGetCommModemStatus(IntPtr hFile, ref uint lpModemStat);

		[DllImport("coredll.dll", EntryPoint="ClearCommError", SetLastError = true)]
		private static extern int CEClearCommError(IntPtr hFile, ref CommErrorFlags lpErrors, CommStat lpStat);

		[DllImport("coredll.dll", EntryPoint="WaitCommEvent", SetLastError = true)]
		private static extern int CEWaitCommEvent(IntPtr hFile, ref CommEventFlags lpEvtMask, IntPtr lpOverlapped);

		[DllImport("coredll.dll", EntryPoint="CreateFileW", SetLastError = true)]
		private static extern IntPtr CECreateFileW(
			String lpFileName, UInt32 dwDesiredAccess, UInt32 dwShareMode,
			IntPtr lpSecurityAttributes, UInt32 dwCreationDisposition, UInt32 dwFlagsAndAttributes,
			IntPtr hTemplateFile);

		[DllImport("coredll.dll", EntryPoint="GetCommProperties", SetLastError=true)]
		private static extern int CEGetCommProperties(IntPtr hFile, CommCapabilities commcap);

		[DllImport("coredll.dll", EntryPoint="FlushFileBuffers", SetLastError=true)]
		private static extern int CEFlushFileBuffers(IntPtr hFile);
		#endregion

	}
	# endregion

	#region Full Framework (aka Win) implementation for CommAPI
	internal class WinCommAPI:CommAPI
	{
		override internal IntPtr CreateFile(string FileName)
		{
			return WinCreateFileW(FileName, CreateAccess, 0, IntPtr.Zero, OPEN_EXISTING, FILE_FLAG_OVERLAPPED, IntPtr.Zero);
		}

		// setting AccessMask to 0 returns a handle we can use to query the device without accessing it
		override internal IntPtr QueryFile(string FileName)
		{
			return WinCreateFileW(FileName, 0, 0, IntPtr.Zero, OPEN_EXISTING, FILE_FLAG_OVERLAPPED, IntPtr.Zero);
		}

		override internal bool WaitCommEvent(IntPtr hPort, ref CommEventFlags flags)
		{
			return Convert.ToBoolean(WinWaitCommEvent(hPort, ref flags, IntPtr.Zero));
		}

		override internal bool ClearCommError(IntPtr hPort, ref CommErrorFlags flags, CommStat stat)
		{
			return Convert.ToBoolean(WinClearCommError(hPort, ref flags, stat));
		}

		override internal bool GetCommModemStatus(IntPtr hPort, ref uint lpModemStat)
		{
			return Convert.ToBoolean(WinGetCommModemStatus(hPort, ref lpModemStat));
		}

		override internal bool SetCommMask(IntPtr hPort, CommEventFlags dwEvtMask)
		{
			return Convert.ToBoolean(WinSetCommMask(hPort, dwEvtMask));
		}

		override internal bool ReadFile(IntPtr hPort, byte[] buffer, int cbToRead, ref Int32 cbRead, IntPtr lpOverlapped)
		{
			return Convert.ToBoolean(WinReadFile(hPort, buffer, cbToRead, ref cbRead, lpOverlapped));
		}

		override internal bool WriteFile(IntPtr hPort, byte[] buffer, Int32 cbToWrite, ref Int32 cbWritten, IntPtr lpOverlapped)
		{
			return Convert.ToBoolean(WinWriteFile(hPort, buffer, cbToWrite, ref cbWritten, lpOverlapped));
		}

		override internal bool CloseHandle(IntPtr hPort)
		{
			return Convert.ToBoolean(WinCloseHandle(hPort));
		}

		override internal bool SetupComm(IntPtr hPort, Int32 dwInQueue, Int32 dwOutQueue)
		{
			return Convert.ToBoolean(WinSetupComm(hPort, dwInQueue, dwOutQueue));
		}

		override internal bool SetCommState(IntPtr hPort, DCB dcb)
		{
			return Convert.ToBoolean(WinSetCommState(hPort, dcb));
		}

		override internal bool GetCommState(IntPtr hPort, DCB dcb)
		{
			return Convert.ToBoolean(WinGetCommState(hPort, dcb));
		}

		override internal bool SetCommTimeouts(IntPtr hPort, CommTimeouts timeouts)
		{
			return Convert.ToBoolean(WinSetCommTimeouts(hPort, timeouts));
		}

		override internal bool EscapeCommFunction(IntPtr hPort, CommEscapes escape)
		{
			return Convert.ToBoolean(WinEscapeCommFunction(hPort, (uint)escape));
		}

		override internal IntPtr CreateEvent(bool bManualReset, bool bInitialState, string lpName)
		{
			return WinCreateEvent(IntPtr.Zero, Convert.ToInt32(bManualReset), Convert.ToInt32(bInitialState), lpName);
		}

		override internal bool SetEvent(IntPtr hEvent)
		{
			return Convert.ToBoolean(WinSetEvent(hEvent));
		}

		override internal bool ResetEvent(IntPtr hEvent)
		{
			return Convert.ToBoolean(WinResetEvent(hEvent));
		}

		override internal bool PulseEvent(IntPtr hEvent)
		{
			return Convert.ToBoolean(WinPulseEvent(hEvent));
		}

		override internal int WaitForSingleObject(IntPtr hHandle, uint dwMilliseconds)
		{
			return WinWaitForSingleObject(hHandle, dwMilliseconds);
		}

		override internal bool GetCommProperties(IntPtr hPort, CommCapabilities commcap)
		{
			return Convert.ToBoolean(WinGetCommProperties(hPort, commcap));
		}

		override internal bool FlushFileBuffers(IntPtr hFile)
		{
			return Convert.ToBoolean(WinFlushFileBuffers(hFile));
		}

		#region Desktop Windows API imports

		[DllImport("kernel32.dll", EntryPoint="WaitForSingleObject", SetLastError = true)]
		private static extern int WinWaitForSingleObject(IntPtr hHandle, uint dwMilliseconds);

		[DllImport("kernel32.dll", EntryPoint="SetEvent", SetLastError = true)]
		private static extern int WinSetEvent(IntPtr hEvent);

		[DllImport("kernel32.dll", EntryPoint="ResetEvent", SetLastError = true)]
		private static extern int WinResetEvent(IntPtr hEvent);

		[DllImport("kernel32.dll", EntryPoint="PulseEvent", SetLastError = true)]
		private static extern int WinPulseEvent(IntPtr hEvent);

		[DllImport("kernel32.dll", EntryPoint="CreateEvent", SetLastError = true)]
		private static extern IntPtr WinCreateEvent(IntPtr lpEventAttributes, int bManualReset, int bInitialState, string lpName);

		[DllImport("kernel32.dll", EntryPoint="EscapeCommFunction", SetLastError = true)]
		private static extern int WinEscapeCommFunction(IntPtr hFile, UInt32 dwFunc);

		[DllImport("kernel32.dll", EntryPoint="SetCommTimeouts", SetLastError = true)]
		private static extern int WinSetCommTimeouts(IntPtr hFile, CommTimeouts timeouts);

		[DllImport("kernel32.dll", EntryPoint="GetCommState", SetLastError = true)]
		private static extern int WinGetCommState(IntPtr hFile, DCB dcb);

		[DllImport("kernel32.dll", EntryPoint="SetCommState", SetLastError = true)]
		private static extern int WinSetCommState(IntPtr hFile, DCB dcb);

		[DllImport("kernel32.dll", EntryPoint="SetupComm", SetLastError = true)]
		private static extern int WinSetupComm(IntPtr hFile, Int32 dwInQueue, Int32 dwOutQueue);

		[DllImport("kernel32.dll", EntryPoint="CloseHandle", SetLastError = true)]
		private static extern int WinCloseHandle(IntPtr hObject);

		[DllImport("kernel32.dll", EntryPoint="WriteFile", SetLastError = true)]
		private  static extern int WinWriteFile(IntPtr hFile, byte[] lpBuffer, Int32 nNumberOfBytesToRead, ref Int32 lpNumberOfBytesRead, IntPtr lpOverlapped);

		[DllImport("kernel32.dll", EntryPoint="ReadFile", SetLastError = true)]
		private static extern int WinReadFile(IntPtr hFile, byte[] lpBuffer, Int32 nNumberOfBytesToRead, ref Int32 lpNumberOfBytesRead, IntPtr lpOverlapped);

		[DllImport("kernel32.dll", EntryPoint="SetCommMask", SetLastError = true)]
		private static extern int WinSetCommMask(IntPtr handle, CommEventFlags dwEvtMask);

		[DllImport("kernel32.dll", EntryPoint="GetCommModemStatus", SetLastError = true)]
		private  static extern int WinGetCommModemStatus(IntPtr hFile, ref uint lpModemStat);

		[DllImport("kernel32.dll", EntryPoint="ClearCommError", SetLastError = true)]
		private  static extern int WinClearCommError(IntPtr hFile, ref CommErrorFlags lpErrors, CommStat lpStat);

		[DllImport("kernel32.dll", EntryPoint="CreateFileW", SetLastError = true, CharSet = CharSet.Unicode)]
		private static extern IntPtr WinCreateFileW(String lpFileName, UInt32 dwDesiredAccess, UInt32 dwShareMode,
			IntPtr lpSecurityAttributes, UInt32 dwCreationDisposition, UInt32 dwFlagsAndAttributes,
			IntPtr hTemplateFile);

		[DllImport("kernel32.dll", EntryPoint="WaitCommEvent", SetLastError = true)]
		private static extern int WinWaitCommEvent(IntPtr hFile, ref CommEventFlags lpEvtMask, IntPtr lpOverlapped);

		[DllImport("kernel32.dll", EntryPoint="GetCommProperties", SetLastError=true)]
		private static extern int WinGetCommProperties(IntPtr hFile, CommCapabilities commcap);

		[DllImport("kernel32.dll", EntryPoint="FlushFileBuffers", SetLastError=true)]
		private static extern bool WinFlushFileBuffers(IntPtr hFile);

		#endregion
	}
	#endregion
}


