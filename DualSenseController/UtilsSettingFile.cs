using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DualSenseController.dualsense2;

namespace DualSenseController
{
	public class UtilsSettingFile
	{
		public static string LightbarColor2String(LightbarColor lightbarColor)
		{
			return string.Format("{0};{1};{2}", lightbarColor.R, lightbarColor.G, lightbarColor.B);
		}

		public static LightbarColor String2LightbarColor(string lightbarColor)
		{
			string[] colors = lightbarColor.Split(';');
			return new LightbarColor(float.Parse(colors[0]), float.Parse(colors[1]), float.Parse(colors[2]));
		}

		public static string PlayerLed2String(PlayerLed playerLed)
		{
			switch (playerLed)
			{
				case PlayerLed.Player1: return "player1";
				case PlayerLed.Player2: return "player2";
				case PlayerLed.Player3: return "player3";
				case PlayerLed.Player4: return "player4";
			}
			return "player1";
		}

		public static PlayerLed String2PlayerLed(string playerLed)
		{
			switch (playerLed.ToLower())
			{
				case "player1": return PlayerLed.Player1;
				case "player2": return PlayerLed.Player2;
				case "player3": return PlayerLed.Player3;
				case "player4": return PlayerLed.Player4;
			}
			return PlayerLed.Player1;
		}

		public static string PlayerLedBrightness2String(PlayerLedBrightness playerLedBrightness)
		{
			switch (playerLedBrightness)
			{
				case PlayerLedBrightness.Low: return "low";
				case PlayerLedBrightness.Medium: return "medium";
				case PlayerLedBrightness.High: return "high";
			}
			return "high";
		}

		public static PlayerLedBrightness String2PlayerLedBrightness(string playerLedBrightness)
		{
			switch (playerLedBrightness.ToLower())
			{
				case "low": return PlayerLedBrightness.Low;
				case "medium": return PlayerLedBrightness.Medium;
				case "high": return PlayerLedBrightness.High;
			}
			return PlayerLedBrightness.High;
		}
	}
}
