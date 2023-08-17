using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DualSenseController
{
	public class UtilsDrivers
	{
		//Check if drivers are installed
		public static bool CheckInstalledDrivers()
		{
			try
			{
				bool virtualBusDriver = EnumerateDevicesStore("ViGEmBus.inf").Any();
				//bool hidHideDriver = EnumerateDevicesStore("HidHide.inf").Any();
				//bool ds3ControllerDriver = EnumerateDevicesStore("Ds3Controller.inf").Any();
				//bool fakerInputDriver = EnumerateDevicesStore("FakerInput.inf").Any();
				//return virtualBusDriver && hidHideDriver && ds3ControllerDriver && fakerInputDriver;
				return virtualBusDriver;
			}
			catch
			{
				Debug.WriteLine("Failed to check installed drivers.");
				return false;
			}
		}

		//Check driver version
		public static bool CheckDriversVersion()
		{
			try
			{
				foreach (FileInfo infNames in EnumerateDevicesStore("ViGEmBus.inf"))
				{
					string availableVersion = File.ReadAllLines(@"Drivers\ViGEmBus\x64\ViGEmBus.inf").FirstOrDefault(x => x.StartsWith("DriverVer"));
					string installedVersion = File.ReadAllLines(infNames.FullName).FirstOrDefault(x => x.StartsWith("DriverVer"));
					Debug.WriteLine("ViGEmBus: " + installedVersion + " / " + availableVersion);
					if (availableVersion != installedVersion) { return false; } else { break; }
				}

				//foreach (FileInfo infNames in EnumerateDevicesStore("HidHide.inf"))
				//{
				//	string availableVersion = File.ReadAllLines(@"Drivers\HidHide\x64\HidHide.inf").FirstOrDefault(x => x.StartsWith("DriverVer"));
				//	string installedVersion = File.ReadAllLines(infNames.FullName).FirstOrDefault(x => x.StartsWith("DriverVer"));
				//	//Debug.WriteLine("HidHide: " + installedVersion + " / " + availableVersion);
				//	if (availableVersion != installedVersion) { return false; } else { break; }
				//}

				//foreach (FileInfo infNames in EnumerateDevicesStore("Ds3Controller.inf"))
				//{
				//	string availableVersion = File.ReadAllLines(@"Drivers\Ds3Controller\Ds3Controller.inf").FirstOrDefault(x => x.StartsWith("DriverVer"));
				//	string installedVersion = File.ReadAllLines(infNames.FullName).FirstOrDefault(x => x.StartsWith("DriverVer"));
				//	//Debug.WriteLine("Ds3Controller: " + installedVersion + " / " + availableVersion);
				//	if (availableVersion != installedVersion) { return false; } else { break; }
				//}

				//foreach (FileInfo infNames in EnumerateDevicesStore("FakerInput.inf"))
				//{
				//	string availableVersion = File.ReadAllLines(@"Drivers\FakerInput\x64\FakerInput.inf").FirstOrDefault(x => x.StartsWith("DriverVer"));
				//	string installedVersion = File.ReadAllLines(infNames.FullName).FirstOrDefault(x => x.StartsWith("DriverVer"));
				//	//Debug.WriteLine("FakerInput: " + installedVersion + " / " + availableVersion);
				//	if (availableVersion != installedVersion) { return false; } else { break; }
				//}

				Debug.WriteLine("Drivers seem to be up to date.");
				return true;
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Failed to check driver version: " + ex.Message);
				return true;
			}
		}

		public static List<FileInfo> EnumerateDevicesStore(string infFileName)
		{
			try
			{
				string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
				string path = Path.Combine(folderPath, "System32\\DriverStore\\FileRepository");
				DirectoryInfo directoryInfo = new DirectoryInfo(path);
				return (from x in directoryInfo.GetFiles("*.inf", SearchOption.AllDirectories)
						where x.Name.ToLower() == infFileName.ToLower()
						orderby x.CreationTime descending
						select x).ToList();
			}
			catch (Exception ex)
			{
				Console.WriteLine("Failed enumerating devices store: " + ex.Message);
				return new List<FileInfo>();
			}
		}
	}
}
