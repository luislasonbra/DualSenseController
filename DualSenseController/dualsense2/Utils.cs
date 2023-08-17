using Device.Net;
using Hid.Net.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Usb.Net.Windows;

namespace DualSenseController.dualsense2
{
	public class Utils
	{
		public static readonly uint DUALSENSE_VENDOR_ID = 0x054c; //1356;
		public static readonly uint DUALSENSE_PRODUCT_ID = 0x0ce6; //3302;

		public static IDeviceFactory GetDualsenseFactories()
		{
			//Register the factory for creating Hid devices. 
			var hidFactory = new FilterDeviceDefinition(vendorId: DUALSENSE_VENDOR_ID, productId: DUALSENSE_PRODUCT_ID).CreateWindowsHidDeviceFactory();
			//Register the factory for creating Usb devices.
			var usbFactory = new FilterDeviceDefinition(vendorId: DUALSENSE_VENDOR_ID, productId: DUALSENSE_PRODUCT_ID).CreateWindowsUsbDeviceFactory();
			//Join the factories together so that it picks up either the Hid or USB device
			return hidFactory.Aggregate(usbFactory);
		}

		public static async Task<List<ConnectedDeviceDefinition>> GetDualsenseDevices(IDeviceFactory factories)
		{
			//Get connected device definitions
			List<ConnectedDeviceDefinition> deviceDefinitions = (await factories.GetConnectedDeviceDefinitionsAsync()
				.ConfigureAwait(false)).ToList();
			////No devices were found
			//if (deviceDefinitions.Count == 0) return new List<ConnectedDeviceDefinition>();
			return deviceDefinitions;
		}

		public static async Task<IDevice> InitializeDevice(IDeviceFactory factories, ConnectedDeviceDefinition deviceDefinition)
		{
			//Get the device from its definition
			var device = await factories.GetDeviceAsync(deviceDefinition).ConfigureAwait(false);

			//Initialize the device
			await device.InitializeAsync().ConfigureAwait(false);

			return device;
		}

		public static bool isEqualDeviceDefinitions(List<ConnectedDeviceDefinition> a, List<ConnectedDeviceDefinition> b)
		{
			if ((a == null && b != null) || (a != null && b == null)) return false;
			// is equal
			if (a == null && b == null) return true;
			// check size
			if (a.Count != b.Count) return false;
			// check real diff
			bool isDiff;
			for (int i = 0; i < a.Count; i++)
			{
				isDiff = true;
				for (int j = 0; j < b.Count; j++)
					if (a[i].DeviceId == b[j].DeviceId)
						isDiff = false;
				if (isDiff)
					return false;
			}
			return true;
		}

		public static Color lightbarColor2Color(LightbarColor lightbarColor)
		{
			int r = (int) Math.Round((lightbarColor.R / 1.0f) * 255.0f);
			int g = (int) Math.Round((lightbarColor.G / 1.0f) * 255.0f);
			int b = (int) Math.Round((lightbarColor.B / 1.0f) * 255.0f);
			return Color.FromArgb(255, r, g, b);
		}

		public static LightbarColor color2LightbarColor(Color lightbarColor)
		{
			float r = (lightbarColor.R / 255.0f) * 1.0f;
			float g = (lightbarColor.G / 255.0f) * 1.0f;
			float b = (lightbarColor.B / 255.0f) * 1.0f;
			return new LightbarColor(r, g, b);
		}
	}
}
