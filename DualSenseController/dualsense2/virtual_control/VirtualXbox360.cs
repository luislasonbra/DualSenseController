using Device.Net;
using DualSenseController.dualsense2.state;
using Nefarius.ViGEm.Client;
using Nefarius.ViGEm.Client.Targets;
using Nefarius.ViGEm.Client.Targets.Xbox360;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DualSenseController.dualsense2.virtual_control
{
    public class VirtualXbox360
    {
		ViGEmClient client = new ViGEmClient();

		uint _virtualControllerID;
		IXbox360Controller controller;

		private const float recipInputPosResolution = 1 / 127f;
		private const float recipInputNegResolution = 1 / 128f;
		private const int outputResolution = 32767 - (-32768);

		private DualSense dualSense;

		public VirtualXbox360(DualSense dualSense)
        {
			this.dualSense = dualSense;
			//update virtualControllerID
			++_virtualControllerID;
			// prepares a new emulate controller
			controller = client.CreateXbox360Controller();
			// brings the x360 online
			controller.Connect();
			// register feedback received
			controller.FeedbackReceived += X360_FeedbackReceived;
		}

		private void X360_FeedbackReceived(object sender, Xbox360FeedbackReceivedEventArgs e)
		{
			if (dualSense == null) return;

			// SetVibration(e.SmallMotor, e.LargeMotor);
			//Debug.WriteLine("vibration: " + Math.Abs((e.SmallMotor / 255.0f) * 1.0f));


			float left = Math.Abs((e.SmallMotor / 255.0f) * 1.0f);
			float right = Math.Abs((e.LargeMotor / 255.0f) * 1.0f);

			dualSense.OutputState.LeftRumble = left;
			dualSense.OutputState.RightRumble = right;
		}

		// https://github.com/Ryochan7/DS4Windows/blob/master/DS4Windows/DS4Control/Xbox360OutDevice.cs#L61
		public void UpdateButtonStatus()
		{
			if (dualSense == null) return;

			//
			DualSenseInputState inputState = dualSense.InputState;

			// byte LX, byte LY
			short LX = inputState.LeftAnalogStickByteX;
			short LY = inputState.LeftAnalogStickByteY;
			// byte RX, byte RY
			short RX = inputState.RightAnalogStickByteX;
			short RY = inputState.RightAnalogStickByteY;
			// byte L2, byte R2
			byte L2 = ByteConverter.UnsignedToByte(inputState.L2);
			byte R2 = ByteConverter.UnsignedToByte(inputState.R2);

			//
			
			// byte Button_DPad_State
			// byte Button2_Status

			//
			controller.SetAxisValue(Xbox360Axis.LeftThumbX, AxisScale(LX, false));
			controller.SetAxisValue(Xbox360Axis.LeftThumbY, AxisScale(LY, true));

			controller.SetAxisValue(Xbox360Axis.RightThumbX, AxisScale(RX, false));
			controller.SetAxisValue(Xbox360Axis.RightThumbY, AxisScale(RY, true));

			controller.SetSliderValue(Xbox360Slider.LeftTrigger, L2);
			controller.SetSliderValue(Xbox360Slider.RightTrigger, R2);

			// buttons
			ushort tempButtons = 0;

			// L1 AND R1
			if (inputState.L1Button) tempButtons |= Xbox360Button.LeftShoulder.Value;
			if (inputState.R1Button) tempButtons |= Xbox360Button.RightShoulder.Value;
			// L3, R3
			if (inputState.L3Button) tempButtons |= Xbox360Button.LeftThumb.Value;
			if (inputState.R3Button) tempButtons |= Xbox360Button.RightThumb.Value;

			// Select, Start
			if (inputState.CreateButton) tempButtons |= Xbox360Button.Back.Value;
			if (inputState.MenuButton) tempButtons |= Xbox360Button.Start.Value;

			// Logo
			if (inputState.LogoButton) tempButtons |= Xbox360Button.Guide.Value;

			// DPAD
			if (inputState.DPadUpButton) tempButtons |= Xbox360Button.Up.Value;
			if (inputState.DPadDownButton) tempButtons |= Xbox360Button.Down.Value;
			if (inputState.DPadLeftButton) tempButtons |= Xbox360Button.Left.Value;
			if (inputState.DPadRightButton) tempButtons |= Xbox360Button.Right.Value;

			// CONTROL
			if (inputState.TriangleButton) tempButtons |= Xbox360Button.Y.Value;
			if (inputState.CrossButton) tempButtons |= Xbox360Button.A.Value;
			if (inputState.SquareButton) tempButtons |= Xbox360Button.X.Value;
			if (inputState.CircleButton) tempButtons |= Xbox360Button.B.Value;

			// set control buttons
			controller.SetButtonsFull(tempButtons);
		}

		public void Release()
		{
			if (controller != null) { controller.Disconnect(); }
			if (client != null) { client.Dispose(); }
		}

		private short AxisScale(int Value, bool Flip)
		{
			//unchecked
			Value -= 0x80;
			float recipRun = Value >= 0 ? recipInputPosResolution : recipInputNegResolution;

			float temp = Value * recipRun;
			//if (Flip) temp = (temp - 0.5f) * -1.0f + 0.5f;
			if (Flip) temp = -temp;
			temp = (temp + 1.0f) * 0.5f;
			return (short)(temp * outputResolution + (-32768));
		}
	}
}
