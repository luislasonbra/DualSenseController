using Hid.Net;
using Usb.Net;
using Device.Net;

using Nefarius.ViGEm.Client;
using Nefarius.ViGEm.Client.Targets;

using System.Diagnostics;
using DualSenseController.dualsense;
using System;
using DualSenseController.dualsense.state;
using DualSenseController.dualsense.vicontrol;

namespace DualSenseController
{
    public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private readonly uint DUALSENSE_VENDOR_ID = 0x054c; //1356;
		private readonly uint DUALSENSE_PRODUCT_ID = 0x0ce6; //3302;

		private static readonly string defaultValue = "(None)";
		private string[] emulateControllerList = new string[] { defaultValue, "Xbox360" };//, "PS4" };

		private IDevice _currentDevice;
		private IDeviceFactory? deviceFactory;
		private List<ConnectedDeviceDefinition> deviceDefinitions = new List<ConnectedDeviceDefinition>();

		private DualSense _dualSense;
		private PeriodicThread _periodicThread;


		VirtualXbox360 _virtualXbox360;

		private void Form1_Load(object sender, EventArgs e)
		{
			cb_emulator_controller.Items.AddRange(emulateControllerList);
			cb_emulator_controller.SelectedIndex = 0;

			//// loading default config
			//_periodicThread = new PeriodicThread(checkDeviceState, 1000).Start();

			//
			loadingSettings();
		}

		private void Form1_FormClosed(object sender, FormClosedEventArgs e)
		{
			Debug.WriteLine("Closed form");
			if (_periodicThread != null) _periodicThread.Abort();
			
			if (_dualSense != null)
			{
				ResetToDefaultState(_dualSense);
				_dualSense.Release();
			}

			if (_virtualXbox360 != null)
			{
				_virtualXbox360.Release();
				_virtualXbox360 = null;
			}
		}

		private async void closeDevice()
		{
			Debug.WriteLine("Beging Close app");
			if (_periodicThread != null) _periodicThread.Abort();

			//
			_dualSense.OutputState.LightbarBehavior = LightbarBehavior.PulseBlue;
			_dualSense.OutputState.PlayerLed = PlayerLed.None;
			_dualSense.OutputState.R2Effect = TriggerEffect.Default;
			_dualSense.OutputState.L2Effect = TriggerEffect.Default;
			_dualSense.OutputState.MicLed = MicLed.Off;
			await _dualSense.ReadWriteOnce();

			// close virtual controller
			_virtualXbox360.Release();
			_virtualXbox360 = null;

			//
			_dualSense.Release();
			_dualSense = null;

			//
			_currentDevice.Close();

			//
			Debug.WriteLine("Finish Close app");
		}

		// =================================================================================================
		// DEFAULT METHODS
		// =================================================================================================

		private void button1_Click(object sender, EventArgs e)
		{
			if (_dualSense != null)
			{
				closeDevice();
				return;
			}

			int index = cb_devices.SelectedIndex;
			if (index <= 0 || deviceDefinitions == null || deviceDefinitions.Count <= 0) return;

			//
			ConnectedDeviceDefinition deviceDefinition = deviceDefinitions[index - 1];
			if (deviceDefinition == null) return;
			ConnectedDevice(deviceDefinition);
		}

		private void btn_refresh_devices_Click(object sender, EventArgs e)
		{
			loadingSettings();
		}

		// =================================================================================================
		// PRIVATE METHODS
		// =================================================================================================

		private async void checkDeviceState()
		{
			if (deviceFactory == null) deviceFactory = Utils.GetDualsenseFactories();
			deviceDefinitions = await Utils.GetDualsenseDevices(deviceFactory);
			// add default
			cb_devices.Items.Clear();
			cb_devices.Items.Add(emulateControllerList[0]);
			// loading devices
			foreach (var deviceDefinition in deviceDefinitions)
				cb_devices.Items.Add(deviceDefinition.ProductName);
			//
			Debug.WriteLine("deviceDefinitions.Count: " + deviceDefinitions.Count);
		}

		private async void loadingSettings()
		{
			// disable check illegal thread
			CheckForIllegalCrossThreadCalls = false;

			// loading device info
			if (deviceFactory == null) deviceFactory = Utils.GetDualsenseFactories();
			deviceDefinitions = await Utils.GetDualsenseDevices(deviceFactory);

			// add default
			cb_devices.Items.Clear();
			cb_devices.Items.Add(emulateControllerList[0]);

			// loading devices
			foreach (var deviceDefinition in deviceDefinitions)
				cb_devices.Items.Add(deviceDefinition.ProductName);

			//
		}

		private async void ConnectedDevice(ConnectedDeviceDefinition deviceDefinition)
		{
			Debug.WriteLine("deviceDefinition.name: " + deviceDefinition.ProductName);
			_currentDevice = await Utils.InitializeDevice(deviceFactory, deviceDefinition);

			//
			if (_virtualXbox360 != null)
			{
				_virtualXbox360.Release();
				_virtualXbox360 = null;
			}
			

			//
			int wheelPos = 0;

			//
			_dualSense = new DualSense(_currentDevice);
			_virtualXbox360 = new VirtualXbox360(_dualSense);

			// settings
			_dualSense.JoystickDeadZone = 0.1f;
			_dualSense.OutputState = new DualSenseOutputState()
			{
				LightbarBehavior = LightbarBehavior.CustomColor,
				LightbarColor = new LightbarColor(1.0f, 0, 0),

				// led player 1 on
				PlayerLed = PlayerLed.Player1,

				////
				//R2Effect = new TriggerEffect.Vibrate(20, 1, 1, 1),
				//L2Effect = new TriggerEffect.Section(0, 0.5f)
			};
			_dualSense.OnStatePolled += (sender) =>
			{
				wheelPos = ProcessStateLogic(sender.InputState, sender.OutputState, wheelPos);

				//virtualXbox360
				if (_virtualXbox360 != null)
				{
					_virtualXbox360.UpdateButtonStatus();
				}
			};
			_dualSense.OnButtonStateChanged += (sender, delta) =>
			{
				Debug.WriteLine("_dualSense.OnButtonStateChanged: " + delta.TriangleButton);
				//Debug.WriteLine($"Battery: {sender.InputState.BatteryStatus.IsCharging}, {sender.InputState.BatteryStatus.IsFullyCharged}, {sender.InputState.BatteryStatus.Level}");

				float batteryLavel = (sender.InputState.BatteryStatus.Level / 10.0f) * 100.0f;
				Debug.WriteLine("Battery: {0:f2}", batteryLavel);

			};
			_dualSense.Start();
		}

		private static int ProcessStateLogic(DualSenseInputState dss, DualSenseOutputState dso, int wheelPos)
		{
			//Console.Clear();
			//Console.WriteLine($"LS: ({dss.LeftAnalogStick.X:F2}, {dss.LeftAnalogStick.Y:F2})");
			//Console.WriteLine($"RS: ({dss.RightAnalogStick.X:F2}, {dss.RightAnalogStick.Y:F2})");
			//Console.WriteLine($"Triggers: ({dss.L2:F2}, {dss.R2:F2})");
			//Console.WriteLine($"Touch 1: ({dss.Touchpad1.X}, {dss.Touchpad1.Y}, {dss.Touchpad1.IsDown}, {dss.Touchpad1.Id})");
			//Console.WriteLine($"Touch 2: ({dss.Touchpad2.X}, {dss.Touchpad2.Y}, {dss.Touchpad2.IsDown}, {dss.Touchpad2.Id})");
			//Console.WriteLine($"Gyro: ({dss.Gyro.X}, {dss.Gyro.Y}, {dss.Gyro.Z})");
			//Console.WriteLine($"Accel: ({dss.Accelerometer.X}, {dss.Accelerometer.Y}, {dss.Accelerometer.Z}); m={dss.Accelerometer.Magnitude()}");
			//Console.WriteLine($"Headphone: {dss.IsHeadphoneConnected}");
			//Console.WriteLine($"Battery: {dss.BatteryStatus.IsCharging}, {dss.BatteryStatus.IsFullyCharged}, {dss.BatteryStatus.Level}");

			//ListPressedButtons(dss);

			//dso.LeftRumble = Math.Abs(dss.LeftAnalogStick.Y);
			//dso.RightRumble = Math.Abs(dss.RightAnalogStick.Y);

			//dso.LightbarColor = ColorWheel(wheelPos);

			return (wheelPos + 5) % 384;
		}

		private static void ResetToDefaultState(DualSense ds)
		{
			ds.OutputState.LightbarBehavior = LightbarBehavior.PulseBlue;
			ds.OutputState.PlayerLed = PlayerLed.None;
			ds.OutputState.R2Effect = TriggerEffect.Default;
			ds.OutputState.L2Effect = TriggerEffect.Default;
			ds.OutputState.MicLed = MicLed.Off;
			ds.ReadWriteOnce();
		}

		private static LightbarColor ColorWheel(int position)
		{
			int r = 0, g = 0, b = 0;
			switch (position / 128)
			{
				case 0:
					r = 127 - position % 128;   //Red down
					g = position % 128;      // Green up
					b = 0;                  //blue off
					break;
				case 1:
					g = 127 - position % 128;  //green down
					b = position % 128;      //blue up
					r = 0;                  //red off
					break;
				case 2:
					b = 127 - position % 128;  //blue down
					r = position % 128;      //red up
					g = 0;                  //green off
					break;
			}
			return new LightbarColor(r / 255f, g / 255f, b / 255f);
		}

		private static void ListPressedButtons(DualSenseInputState dss)
		{
			IEnumerable<string> pressedButtons = dss.GetType().GetProperties()
				.Where(p => p.Name.EndsWith("Button") && p.PropertyType == typeof(bool))
				.Where(p => (bool)p.GetValue(dss)!)
				.Select(p => p.Name.Replace("Button", ""));
			string joined = string.Join(", ", pressedButtons);
			Debug.WriteLine($"Buttons: {joined}");
		}

	}
}