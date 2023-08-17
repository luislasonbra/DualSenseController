using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//
using Device.Net;
using Hid.Net.Windows;
using Usb.Net.Windows;

namespace DualSenseController.dualsense
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


	}
}
