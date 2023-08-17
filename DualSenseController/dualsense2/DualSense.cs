using Device.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using DualSenseController.dualsense2.state;

namespace DualSenseController.dualsense2
{
	public class DualSense
	{
		public float JoystickDeadZone { get; set; } = 0;

		/// <summary>
		/// This controller's output state.
		/// </summary>
		public DualSenseOutputState OutputState { get; set; } = new DualSenseOutputState();

		/// <summary>
		/// This controller's most recently polled input state.
		/// </summary>
		public DualSenseInputState InputState { get; private set; } = new DualSenseInputState();

		/// <summary>
		/// State event handler for asynchronous polling.
		/// </summary>
		/// <seealso cref="BeginPolling(uint)"/>
		/// <seealso cref="EndPolling"/>
		public event StatePolledHandler OnStatePolled;

		///// <summary>
		///// Button state changed event handler for asynchronous polling.
		///// </summary>
		///// <seealso cref="BeginPolling(uint)"/>
		///// <seealso cref="EndPolling"/>
		//public event ButtonStateChangedHandler OnButtonStateChanged;


		// IO parameters
		private readonly IDevice underlyingDevice;
		private readonly int readBufferSize;
		private readonly int writeBufferSize;

		public IoMode IoMode { get; private set; }

		private bool _runningThread = false;
		private PeriodicThread _periodicThread;

		public bool isWorkedThread = false;

		public DualSense(IDevice device)
		{
			underlyingDevice = device;
			if (!device.ConnectedDeviceDefinition.WriteBufferSize.HasValue) throw new AccessViolationException("Write Buffer Size is null");
			if (!device.ConnectedDeviceDefinition.ReadBufferSize.HasValue) throw new AccessViolationException("Read Buffer Size is null");
			// read and write buffer size
			readBufferSize = device.ConnectedDeviceDefinition.ReadBufferSize.Value;
			writeBufferSize = device.ConnectedDeviceDefinition.WriteBufferSize.Value;
			// check connection type
			IoMode = readBufferSize == 64 ? IoMode.USB : readBufferSize == 78 ? IoMode.Bluetooth : IoMode.Unknown;
			if (IoMode == IoMode.Unknown)
			{
				throw new InvalidOperationException("Can't initialize device - supported IO modes are USB and Bluetooth.");
			}
		}

		public void Start()
		{
			//_periodicThread = new PeriodicThread(async () => ProcessEachState(await ReadWriteOnceAsync()), 2, true).Start();

			//OnStatePolled?.Invoke(this);
			_periodicThread = new PeriodicThread(async () =>
			{
				if (isWorkedThread) return;
				isWorkedThread = true;
				// read and push data
				InputState = await ReadWriteOnceAsync();
				isWorkedThread = false;
				// invoke event
				if (OnStatePolled != null) OnStatePolled.Invoke(this);
			}, 2, true).Start();
		}

		public async Task<DualSenseInputState> ReadWriteOnceAsync()
		{
			try
			{
				TransferResult result = await underlyingDevice.WriteAndReadAsync(GetOutputDataBytes());
				if (result.BytesTransferred == readBufferSize)
				{
					// this can effectively determine which input packet you've recieved, USB or bluetooth, and offset by the right amount
					int offset = result.Data[0] switch
					{
						0x01 => 1, // USB packet flag
						0x31 => 2, // Bluetooth packet flag
						_ => 0
					};
					return new DualSenseInputState(result.Data.Skip(offset).ToArray(), IoMode, JoystickDeadZone);
				}
				else
				{
					throw new IOException("Failed to read data - buffer size mismatch");
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Error: " + ex.ToString());
				return null;
			}
		}

		/// <summary>
		/// Updates the input and output states once. This operation is blocking.
		/// </summary>
		/// <returns>The polled state, for convenience. This is also updated on the controller instance.</returns>
		public async Task<DualSenseInputState> ReadWriteOnce() { return InputState = await ReadWriteOnceAsync(); }

		///// <summary>
		///// Process a state event. Wraps around user-provided handler since Reactive needs an Action<>.
		///// </summary>
		///// <param name="nextState">The receieved input state</param>
		//private void ProcessEachState(DualSenseInputState nextState)
		//{
		//	DualSenseInputState prevState = InputState;
		//	InputState = nextState;
		//	// don't take up the burden to diff the changes unless someone cares
		//	if (OnButtonStateChanged != null)
		//	{
		//		DualSenseInputStateButtonDelta delta = new DualSenseInputStateButtonDelta(prevState, nextState);
		//		if (delta.HasChanges)
		//		{
		//			OnButtonStateChanged.Invoke(this, delta);
		//		}
		//	}
		//	OnStatePolled?.Invoke(this);
		//}

		/// <summary>
		/// Builds the output byte array that will be sent to the controller.
		/// </summary>
		/// <returns>An array of bytes to send to the controller</returns>
		private byte[] GetOutputDataBytes()
		{
			byte[] bytes = new byte[writeBufferSize];
			byte[] hidBuffer = OutputState.BuildHidOutputBuffer();
			if (IoMode == IoMode.USB)
			{
				bytes[0] = 0x02;
				Array.Copy(hidBuffer, 0, bytes, 1, 47);
			}
			else if (IoMode == IoMode.Bluetooth)
			{
				bytes[0] = 0x31;
				bytes[1] = 0x02;
				Array.Copy(hidBuffer, 0, bytes, 2, 47);
				// make a 32 bit checksum of the first 74 bytes and add it at the end
				uint crcChecksum = CRC32Utils.ComputeCRC32(bytes, 74);
				byte[] checksumBytes = BitConverter.GetBytes(crcChecksum);
				Array.Copy(checksumBytes, 0, bytes, 74, 4);
			}
			else
			{
				throw new InvalidOperationException("Can't send data - supported IO modes are USB and Bluetooth.");
			}
			return bytes;
		}

		public void Release()
		{
			_runningThread = false;

			// close periodic check read data
			if (_periodicThread != null)
			{
				_periodicThread.Abort();
			}

			// close connection
			if (underlyingDevice != null && underlyingDevice.IsInitialized)
			{
				underlyingDevice.Close();
			}
		}
	}
}
