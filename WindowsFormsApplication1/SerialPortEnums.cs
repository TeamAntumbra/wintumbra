using System;
using System.ComponentModel;

namespace OpenNETCF.IO.Ports
{
	#region Enumerations

	[Flags]
	public enum SerialErrors
	{
		/// <summary>
		/// An input buffer overflow has occurred. There is either no room in the input buffer,
		/// or a character was received after the end-of-file (EOF) character.
		/// </summary>
		[Description("Receive Overflow")]	RxOver = 1,

		/// <summary>
		/// A character-buffer overrun has occurred. The next character is lost.
		/// </summary>
		[Description("Buffer Overrun")]		Overrun = 2,

		/// <summary>
		/// The hardware detected a parity error.
		/// </summary>
		[Description("Parity Error")]		RxParity = 4,

		/// <summary>
		/// The hardware detected a framing error.
		/// </summary>
		[Description("Framing Error")]		Frame = 8,

		/// <summary>
		/// The application tried to transmit a character, but the output buffer was full.
		/// </summary>
		[Description("Transmit Full")]		TxFull = 256,
	}

	[Flags]
	public enum SerialPinChanges
	{
		[Description("CTS Changed")]	CtsChanged	= 8,
		[Description("DSR Changed")]	DsrChanged	= 16,
		[Description("CD Changed")]		CDChanged	= 32,
		[Description("Break")]			Break		= 64,
		[Description("Ring")]			Ring		= 256,
	}

	[Flags]
	public enum SerialReceived
	{
		[Description("Received Chars")]	ReceivedChars	= 1,
		[Description("End of Receive")]	EofReceived		= 2,
	}

	[Flags]
	public enum StopBits
	{
		[Description("1")]		One			 = 1,
		[Description("1.5")]	OnePointFive = 2,
		[Description("2")]		Two			 = 4,
	}

	public enum Handshake
	{
		[Description("None")]		None				 = 0,
		[Description("Xon/Xoff")]	XOnXOff				 = 1,
		[Description("RTS")]		RequestToSend		 = 2,
		[Description("Xon/RTS")]	RequestToSendXOnXOff = 3,
	}

	public enum Parity
	{
		[Description("None")]	None	= 0,
		[Description("Odd")]	Odd		= 1,
		[Description("Even")]	Even	= 2,
		[Description("Mark")]	Mark	= 3,
		[Description("Space")]	Space	= 4,
	}

	#endregion

	#region Delegates and Events

	public class SerialErrorEventArgs : EventArgs
	{
		private SerialErrors _EventType;

		public SerialErrors EventType
		{
			get { return _EventType; }
		}

		public SerialErrorEventArgs(SerialErrors eventType)
		{
			_EventType = eventType;
		}

		public override string ToString()
		{
			return "UART Error: " + Enum.Format(typeof(SerialErrors), EventType, "G");
		}

	}

	public class SerialPinChangedEventArgs : EventArgs
	{
		private SerialPinChanges _EventType;

		public SerialPinChanges EventType
		{
			get { return _EventType; }
		}

		public SerialPinChangedEventArgs(SerialPinChanges eventType)
		{
			_EventType = eventType;
		}

		public override string ToString()
		{
			return "Serial Pin Changed: " + Enum.Format(typeof(SerialPinChanges), EventType, "G");
		}
	}

	public class SerialReceivedEventArgs : EventArgs
	{
		private SerialReceived _EventType;

		public SerialReceived EventType
		{
			get { return _EventType; }
		}

		public SerialReceivedEventArgs(SerialReceived eventType)
		{
			_EventType = eventType;
		}

		public override string ToString()
		{
			return "Serial Received Event: " + Enum.Format(typeof(SerialReceived), EventType, "G");
		}
	}

	/// <summary>
	/// Represents the method that will handle the event of a SerialPort object.
	/// </summary>
	public delegate void SerialErrorEventHandler(object sender, SerialErrorEventArgs e);

	/// <summary>
	/// Represents the method that will handle the PinChangedEvent event of a SerialPort object.
	/// </summary>
	public delegate void SerialPinChangedEventHandler(object sender, SerialPinChangedEventArgs e);

	/// <summary>
	/// Represents the method that will handle the ReceivedEvent event of a SerialPort object.
	/// </summary>
	public delegate void SerialReceivedEventHandler(object sender, SerialReceivedEventArgs e);

	#endregion
}
