using System;

namespace OpenNETCF.IO.Ports.Streams
{
	public interface ISerialStreamCtrl : IDisposable
	{
		/// <summary>Gets or sets the serial baud rate.</summary>
		int  BaudRate { get; set; }

		/// <summary>Gets or sets the break signal state. </summary>
		bool BreakState { get; set; }

		/// <summary>Gets the number of bytes of data in the receive buffer.</summary>
		int  BytesToRead { get; }

		/// <summary>Gets the number of bytes of data in the send buffer.</summary>
		int  BytesToWrite { get; }
		
		/// <summary>Gets the state of the carrier detect line for the port.</summary>
		bool CDHolding { get; }

		/// <summary>Gets the state of the Clear-to-Send line. </summary>
		bool CtsHolding { get; }

		/// <summary>Gets or sets the standard length of databits per byte.</summary>
		int  DataBits { get; set; }

		/// <summary>Gets or sets whether null characters are ignored when transmitted between the port and the receive buffer.</summary>
		bool DiscardNull { get; set; }

		/// <summary>Gets the state of the Data Set Ready (DSR) signal. </summary>
		bool DsrHolding { get; }

		/// <summary>Gets or sets enabling of the Data Terminal Ready (DTR) signal during serial communication. </summary>
		bool DtrEnable { get; set; }
			
		/// <summary>Gets or sets the handshaking protocol for serial port transmission of data.</summary>
		Handshake Handshake { get; set; }

		/// <summary>Gets the open or closed status of the SerialPort object.</summary>
		bool IsOpen { get; }

		/// <summary>Gets or sets the parity-checking protocol.</summary>
		Parity Parity { get; set; }

		/// <summary>Gets or sets the 8-bit character that is used to replace invalid characters in a data stream when a parity error occurs. </summary>
		byte ParityReplace { get; set; }

		int  ReadBufferSize { get; set; }

		/// <summary>Gets or sets the number of milliseconds before a timeout occurs when a read operation does not finish. </summary>
		int  ReadTimeout { get; set; }

		/// <summary>Gets or sets the number of bytes in the internal input buffer before a ReceivedEvent is fired.</summary>
		int  ReceivedBytesThreshold { get; set; }

		/// <summary>Gets or sets whether the Request to Transmit (RTS) signal is enabled during serial communication. </summary>
		bool RtsEnable { get; set; }

		/// <summary>Gets or sets the standard number of stopbits per byte.</summary>
		StopBits StopBits { get; set; }

		int  WriteBufferSize { get; set; }

		/// <summary>Gets or sets the number of milliseconds before a timeout occurs when a write operation does not finish. </summary>
		int  WriteTimeout { get; set; }

		/// <summary>Represents the method that handles the error event of a SerialPort object.</summary>
		event SerialErrorEventHandler ErrorEvent;
		/// <summary>Represents the method that will handle the serial received event of a SerialPort object.</summary>
		event SerialReceivedEventHandler ReceivedEvent;
		/// <summary>Represents the method that will handle the serial pin changed event of a SerialPort object.</summary>
		event SerialPinChangedEventHandler PinChangedEvent;

		/// <summary>Discards data from the serial driver's receive buffer.</summary>
		void DiscardInBuffer();
		/// <summary>Discards data from the serial driver's transmit buffer.</summary>
		void DiscardOutBuffer();
	}
}