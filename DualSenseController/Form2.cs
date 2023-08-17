using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Device.Net;
using DualSenseController.dualsense2;
using DualSenseController.dualsense2.state;
using DualSenseController.dualsense2.virtual_control;

namespace DualSenseController
{
	// https://hardwaretester.com/gamepad
	public partial class Form2 : Form
	{
		// Controller
		//private static readonly string _defaultTitleApp = "DualSense (PS5)";
		private static readonly string _defaultTitleApp = "DualSense (x360)";

		private static readonly string defaultValue = "(None)";

		private IDevice _activeDevice;
		private IDeviceFactory deviceFactory;
		private List<ConnectedDeviceDefinition> deviceDefinitions = new List<ConnectedDeviceDefinition>();

		private bool isConnected = false;
		private PeriodicThread _periodicThread;

		private DualSense _dualSense;
		private VirtualXbox360 _virtualXbox360;

		private PlayerLed _playerLed = PlayerLed.Player1;
		private PlayerLedBrightness _playerLedBrightness = PlayerLedBrightness.High;

		/*
		LightbarBehavior = LightbarBehavior.CustomColor;
		LightbarColor = new LightbarColor(1.0f, 0, 0);
		*/
		private LightbarBehavior _lightbarBehavior = LightbarBehavior.CustomColor;
		private LightbarColor _lightbarColor = new LightbarColor(1.0f, 0, 0);

		private string _rootPaths;
		private static string _settingAppFile;

		public Form2()
		{
			InitializeComponent();

			//
			_rootPaths = AppDomain.CurrentDomain.BaseDirectory;
			_settingAppFile = Path.Combine(_rootPaths, "config.ini");
		}

		private void Form2_Load(object sender, EventArgs e)
		{
			Text = _defaultTitleApp;

			//Check if drivers are installed
			if (!UtilsDrivers.CheckInstalledDrivers())
			{
				//if (!ShowInTaskbar) { Application_ShowHideWindow(); }
				//await Message_InstallDrivers();
				string message = "Welcome to DualSenseController, it seems like you have not yet installed the required drivers to use this application, please make sure that you have installed the required drivers.\n\nIf you just installed the drivers and this message shows up restart your PC.\n\nAfter some Windows updates you may need to reinstall the drivers to work.";
				MessageBox.Show(this, "Install the drivers", message, MessageBoxButtons.OK, MessageBoxIcon.Error);
				Environment.Exit(0);
				return;
			}

			//Check installed driver versions
			if (!UtilsDrivers.CheckDriversVersion())
			{
				//if (!ShowInTaskbar) { Application_ShowHideWindow(); }
				//await Message_UpdateDrivers();
				string message = "Welcome to DualSenseController, it seems like you have not yet installed the required drivers to use this application, please make sure that you have installed the required drivers.\n\nIf you just installed the drivers and this message shows up restart your PC.\n\nAfter some Windows updates you may need to reinstall the drivers to work.";
				MessageBox.Show(this, "Update the drivers", message, MessageBoxButtons.OK, MessageBoxIcon.Error);
				Environment.Exit(0);
				return;
			}

			// loading settings
			loadingSettingsApp();

			// set periodic function
			_periodicThread = new PeriodicThread(() => refreshDevices(true), 1000).Start();
			loadingSettings();

			// loading user custom settings
			loadingUserSettings();
		}

		private void Form2_FormClosing(object sender, FormClosingEventArgs e)
		{
			e.Cancel = true;
			exitApplication();
		}

		private void btn_refresh_devices_Click(object sender, EventArgs e)
		{
			refreshDevices();
		}

		private void btn_connection_disconnection_Click(object sender, EventArgs e)
		{
			cb_devices.Enabled = false;
			btn_refresh_devices.Enabled = false;

			if (isConnected)
			{
				DisconnectDevice();
				// update
				updateState();
				//
				//MessageBox.Show(this, "Controller Disconnected", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			int index = cb_devices.SelectedIndex;
			if (index <= 0 || deviceDefinitions == null || deviceDefinitions.Count <= 0)
			{
				cb_devices.Enabled = true;
				btn_refresh_devices.Enabled = true;
				MessageBox.Show(this, "Not controller selected", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}
			Debug.WriteLine("index: " + index + ", deviceDefinitions.Count: " + deviceDefinitions.Count);

			// update
			updateState();

			//
			ConnectDevice(deviceDefinitions[index - 1]);
			//
			//MessageBox.Show(this, "Controller Connected", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void btn_lightbar_color_Click(object sender, EventArgs e)
		{
			if (_dualSense == null) return;

			//btn_lightbar_color
			ColorDialog colorDialog = new ColorDialog();
			colorDialog.Color = Utils.lightbarColor2Color(_lightbarColor);
			if (colorDialog.ShowDialog() == DialogResult.OK)
			{
				btn_lightbar_color.BackColor = colorDialog.Color;
				_lightbarColor = Utils.color2LightbarColor(colorDialog.Color);

				// send command to controller
				sendCommandToController();
			}
		}

		private void tsbtn_help_Click(object sender, EventArgs e)
		{
			About about = new About();
			about.ShowDialog();
		}

		// ========================================================
		private void loadingSettingsApp()
		{
			if (!File.Exists(_settingAppFile)) return;

			// loading file settings
			IniFile settings = new IniFile(_settingAppFile);

			// player_leds
			IniFile.IniValue playerLedValue = settings.getRootValue("player_leds");
			if (playerLedValue != null) _playerLed = UtilsSettingFile.String2PlayerLed(playerLedValue.value);

			// player_led_brightness
			IniFile.IniValue playerLedBrightness = settings.getRootValue("player_led_brightness");
			if (playerLedBrightness != null) _playerLedBrightness = UtilsSettingFile.String2PlayerLedBrightness(playerLedBrightness.value);

			// lightbar_color
			IniFile.IniValue lightbarColor = settings.getRootValue("lightbar_color");
			if (lightbarColor != null) _lightbarColor = UtilsSettingFile.String2LightbarColor(lightbarColor.value);

			// update
			loadingUserSettings();
		}

		private void savedSettingsApp()
		{
			bool createdFile = !File.Exists(_settingAppFile);

			// saved settings
			IniFile settings = new IniFile(_settingAppFile, createdFile);
			settings.setRootValue("player_leds", UtilsSettingFile.PlayerLed2String(_playerLed));
			settings.setRootValue("player_led_brightness", UtilsSettingFile.PlayerLedBrightness2String(_playerLedBrightness));
			settings.setRootValue("lightbar_color", UtilsSettingFile.LightbarColor2String(_lightbarColor));
			settings.Save();
		}

		private void loadingUserSettings()
		{
			checkButtonState();

			//
			btn_lightbar_color.BackColor = Utils.lightbarColor2Color(_lightbarColor);

			//
			rb_player1.Checked = _playerLed == PlayerLed.Player1;
			rb_player2.Checked = _playerLed == PlayerLed.Player2;
			rb_player3.Checked = _playerLed == PlayerLed.Player3;
			rb_player4.Checked = _playerLed == PlayerLed.Player4;

			//
			rb_player_brightness_low.Checked = _playerLedBrightness == PlayerLedBrightness.Low;
			rb_player_brightness_medium.Checked = _playerLedBrightness == PlayerLedBrightness.Medium;
			rb_player_brightness_high.Checked = _playerLedBrightness == PlayerLedBrightness.High;

			//
			rb_player1.CheckedChanged += rb_player_CheckedChanged;
			rb_player2.CheckedChanged += rb_player_CheckedChanged;
			rb_player3.CheckedChanged += rb_player_CheckedChanged;
			rb_player4.CheckedChanged += rb_player_CheckedChanged;

			//
			rb_player_brightness_low.CheckedChanged += rb_player_CheckedChanged;
			rb_player_brightness_medium.CheckedChanged += rb_player_CheckedChanged;
			rb_player_brightness_high.CheckedChanged += rb_player_CheckedChanged;
		}

		private void rb_player_CheckedChanged(object? sender, EventArgs e)
		{
			_playerLed = rb_player1.Checked ? PlayerLed.Player1 : _playerLed;
			_playerLed = rb_player2.Checked ? PlayerLed.Player2 : _playerLed;
			_playerLed = rb_player3.Checked ? PlayerLed.Player3 : _playerLed;
			_playerLed = rb_player4.Checked ? PlayerLed.Player4 : _playerLed;

			//
			_playerLedBrightness = rb_player_brightness_low.Checked ? PlayerLedBrightness.Low : _playerLedBrightness;
			_playerLedBrightness = rb_player_brightness_medium.Checked ? PlayerLedBrightness.Medium : _playerLedBrightness;
			_playerLedBrightness = rb_player_brightness_high.Checked ? PlayerLedBrightness.High : _playerLedBrightness;

			// send command to controller
			sendCommandToController();
		}

		private async void sendCommandToController()
		{
			if (_dualSense == null) return;
			_dualSense.OutputState.PlayerLed = _playerLed;
			_dualSense.OutputState.PlayerLedBrightness = _playerLedBrightness;
			// send lightbar Color
			_dualSense.OutputState.LightbarBehavior = _lightbarBehavior;
			_dualSense.OutputState.LightbarColor = _lightbarColor;
			await _dualSense.ReadWriteOnce();
		}

		private async Task ResetToDefaultState(DualSense ds)
		{
			if (ds == null) return;
			// default settings
			ds.OutputState.LightbarBehavior = LightbarBehavior.PulseBlue;
			ds.OutputState.PlayerLed = PlayerLed.None;
			ds.OutputState.R2Effect = TriggerEffect.Default;
			ds.OutputState.L2Effect = TriggerEffect.Default;
			ds.OutputState.MicLed = MicLed.Off;
			await ds.ReadWriteOnce();
		}

		private void checkButtonState()
		{
			rb_player1.Enabled = isConnected;
			rb_player2.Enabled = isConnected;
			rb_player3.Enabled = isConnected;
			rb_player4.Enabled = isConnected;

			//
			rb_player_brightness_low.Enabled = isConnected;
			rb_player_brightness_medium.Enabled = isConnected;
			rb_player_brightness_high.Enabled = isConnected;

			//
			btn_lightbar_color.Enabled = isConnected;
		}

		private async void exitApplication()
		{
			if (_periodicThread != null)
			{
				_periodicThread.Abort();
				_periodicThread = null;
			}

			//
			await ResetToDefaultState(_dualSense);

			//
			DisconnectDevice();

			// saved settings
			savedSettingsApp();

			// exit app
			Environment.Exit(0);
		}

		private void updateState()
		{
			isConnected = !isConnected;
			btn_connection_disconnection.Text = isConnected ? "Disconnect" : "Connect";
		}

		private void UpdateDisplayDevices()
		{
			// add default
			cb_devices.Items.Clear();
			cb_devices.Items.Add(defaultValue);
			cb_devices.SelectedIndex = 0;

			// loading devices
			foreach (var deviceDefinition in deviceDefinitions)
				cb_devices.Items.Add(deviceDefinition.ProductName);
		}

		private async void loadingSettings()
		{
			// loading device info
			if (deviceFactory == null) deviceFactory = Utils.GetDualsenseFactories();
			deviceDefinitions = await Utils.GetDualsenseDevices(deviceFactory);

			// update display devices
			UpdateDisplayDevices();
		}

		private async void refreshDevices(bool isThread = false)
		{
			if (deviceFactory == null) return;
			if (!isThread) btn_refresh_devices.Enabled = false;

			//
			if (isThread) CheckForIllegalCrossThreadCalls = false;

			// loading device info
			List<ConnectedDeviceDefinition> dDefinitions = await Utils.GetDualsenseDevices(deviceFactory);
			if (!Utils.isEqualDeviceDefinitions(deviceDefinitions, dDefinitions))
			{
				// enable refresh button
				if (!isThread) btn_refresh_devices.Enabled = true;

				// check active devices
				if (_activeDevice != null)
				{
					bool isConnected = false;
					foreach (var deviceDefinition in dDefinitions)
					{
						if (deviceDefinition.DeviceId == _activeDevice.DeviceId)
						{
							isConnected = true;
							break;
						}
					}
					if (!isConnected)
					{
						_activeDevice.Close();
						_activeDevice = null;

						//
						DisconnectDevice();
						// update
						updateState();
					}
				}

				// new devices list
				deviceDefinitions = dDefinitions;
				// update display devices
				UpdateDisplayDevices();
			}

			// enable refresh button
			if (!isThread) btn_refresh_devices.Enabled = true;
		}

		private async void ConnectDevice(ConnectedDeviceDefinition deviceDefinition)
		{
			Debug.WriteLine("deviceDefinition.name: " + deviceDefinition.ProductName);
			_activeDevice = await Utils.InitializeDevice(deviceFactory, deviceDefinition);

			// reset title
			Text = _defaultTitleApp;
			tspb_battery_status.Text = "0%";

			// initialize dual sense
			_dualSense = new DualSense(_activeDevice);
			_virtualXbox360 = new VirtualXbox360(_dualSense);

			// settings
			_dualSense.JoystickDeadZone = 0.1f;
			_dualSense.OutputState = new DualSenseOutputState()
			{
				// custom lightbar
				LightbarBehavior = _lightbarBehavior,
				LightbarColor = _lightbarColor,

				// led player 1 on
				PlayerLed = _playerLed,
				PlayerLedBrightness = _playerLedBrightness,

				////
				//R2Effect = new TriggerEffect.Vibrate(20, 1, 1, 1),
				//L2Effect = new TriggerEffect.Section(0, 0.5f)
			};
			_dualSense.OnStatePolled += (sender) =>
			{
				if (sender == null || sender.InputState == null)
				{
					Text = _defaultTitleApp;
					tspb_battery_status.Text = "Unknow";
					return;
				}

				//Debug.WriteLine("Put commands");
				//wheelPos = ProcessStateLogic(sender.InputState, sender.OutputState, wheelPos);

				// battery state
				if (sender.InputState.BatteryStatus.IsFullyCharged)
				{
					Text = string.Format("{0} (Fully charged)", _defaultTitleApp);
					tspb_battery_status.Text = "Fully charged";
				}
				else if (sender.InputState.BatteryStatus.IsCharging)
				{
					float batteryPercent = Math.Max(0, Math.Min(sender.InputState.BatteryStatus.Level * 10.0f + 5.0f, 100));
					Text = string.Format("{0} (Charging {1}% ...)", _defaultTitleApp, batteryPercent);
					tspb_battery_status.Text = string.Format("Charging {0}% ...", batteryPercent);
				}
				else
				{
					float batteryPercent = Math.Max(0, Math.Min(sender.InputState.BatteryStatus.Level * 10.0f + 5.0f, 100));
					Text = string.Format("{0} (Discharging {1}%)", _defaultTitleApp, batteryPercent);
					tspb_battery_status.Text = string.Format("Discharging {0}% ...", batteryPercent);
				}

				//virtualXbox360
				if (_virtualXbox360 != null) _virtualXbox360.UpdateButtonStatus();
			};
			_dualSense.Start();

			//
			checkButtonState();
		}

		private async void DisconnectDevice()
		{
			Debug.WriteLine("DisconnectDevice");

			// default controller settings
			await ResetToDefaultState(_dualSense);

			// dispose dualsense class
			if (_dualSense != null)
			{
				_dualSense.Release();
				_dualSense = null;
			}

			if (_virtualXbox360 != null)
			{
				_virtualXbox360.Release();
				_virtualXbox360 = null;
			}

			//
			cb_devices.Enabled = true;
			btn_refresh_devices.Enabled = true;

			//
			checkButtonState();
		}
	}
}
