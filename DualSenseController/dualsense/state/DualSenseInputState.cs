﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DualSenseController.dualsense.state
{
	/// <summary>
	/// All available input variables for a DualSense controller.
	/// </summary>
	public class DualSenseInputState
	{
		//
		public readonly byte LeftAnalogStickByteX = 0x0, LeftAnalogStickByteY = 0x0;
		public readonly byte RightAnalogStickByteX = 0x0, RightAnalogStickByteY = 0x0;
		public readonly byte L2Byte = 0x0, R2Byte = 0x0;

		/// <summary>
		/// Default constructor, initializes all fields to 0/false/default
		/// </summary>
		internal DualSenseInputState() { }

		/// <summary>
		/// Constructs a DualSenseInputState. Parses the HID input report.
		/// </summary>
		/// <param name="data">The data buffer.</param>
		/// <param name="inputMode">The DualSense's input mode.</param>
		/// <param name="deadZone">The DualSense's joystick deadzone.</param>
		internal DualSenseInputState(byte[] data, IoMode inputMode, float deadZone)
		{
			// Analog inputs bytes
			LeftAnalogStickByteX = data[0]; LeftAnalogStickByteY = data[1];
			RightAnalogStickByteX = data[2]; RightAnalogStickByteY = data[3];

			// Analog inputs
			LeftAnalogStick = ReadAnalogStick(data[0], data[1], deadZone);
			RightAnalogStick = ReadAnalogStick(data[2], data[3], deadZone);
			L2 = ByteConverter.ToUnsignedFloat(L2Byte = GetModeSwitch(inputMode, data, 4, 7));
			R2 = ByteConverter.ToUnsignedFloat(R2Byte = GetModeSwitch(inputMode, data, 5, 8));

			// Buttons
			byte btnBlock1 = GetModeSwitch(inputMode, data, 7, 4);
			byte btnBlock2 = GetModeSwitch(inputMode, data, 8, 5);
			byte btnBlock3 = GetModeSwitch(inputMode, data, 9, 6);
			SquareButton = ByteConverter.HasFlag(btnBlock1, 0x10);
			CrossButton = ByteConverter.HasFlag(btnBlock1, 0x20);
			CircleButton = ByteConverter.HasFlag(btnBlock1, 0x40);
			TriangleButton = ByteConverter.HasFlag(btnBlock1, 0x80);
			DPadUpButton = ReadDPadButton(btnBlock1, 0, 1, 7);
			DPadRightButton = ReadDPadButton(btnBlock1, 1, 2, 3);
			DPadDownButton = ReadDPadButton(btnBlock1, 3, 4, 5);
			DPadLeftButton = ReadDPadButton(btnBlock1, 5, 6, 7);
			L1Button = ByteConverter.HasFlag(btnBlock2, 0x01);
			R1Button = ByteConverter.HasFlag(btnBlock2, 0x02);
			L2Button = ByteConverter.HasFlag(btnBlock2, 0x04);
			R2Button = ByteConverter.HasFlag(btnBlock2, 0x08);
			CreateButton = ByteConverter.HasFlag(btnBlock2, 0x10);
			MenuButton = ByteConverter.HasFlag(btnBlock2, 0x20);
			L3Button = ByteConverter.HasFlag(btnBlock2, 0x40);
			R3Button = ByteConverter.HasFlag(btnBlock2, 0x80);
			LogoButton = ByteConverter.HasFlag(btnBlock3, 0x01);
			TouchpadButton = ByteConverter.HasFlag(btnBlock3, 0x02);
			MicButton = ByteConverter.HasFlag(GetModeSwitch(inputMode, data, 9, -1), 0x04); // not supported on the broken BT protocol, otherwise would likely be in btnBlock3

			// Multitouch
			Touchpad1 = ReadTouchpad(GetModeSwitch(inputMode, data, 32, -1, 4));
			Touchpad2 = ReadTouchpad(GetModeSwitch(inputMode, data, 36, -1, 4));

			// 6-axis
			// gyro directions seem to follow left-hand rule rather than right, so reverse the directions
			Gyro = -ReadAccelAxes(
				GetModeSwitch(inputMode, data, 15, -1, 2),
				GetModeSwitch(inputMode, data, 17, -1, 2),
				GetModeSwitch(inputMode, data, 19, -1, 2)
			);
			Accelerometer = ReadAccelAxes(
				GetModeSwitch(inputMode, data, 21, -1, 2),
				GetModeSwitch(inputMode, data, 23, -1, 2),
				GetModeSwitch(inputMode, data, 25, -1, 2)
			);

			// Misc
			byte batteryByte = GetModeSwitch(inputMode, data, 52, -1);
			byte miscByte = GetModeSwitch(inputMode, data, 53, -1); // this contains various stuff, seems to have both audio and battery info
			BatteryStatus = new BatteryStatus
			{
				IsCharging = ByteConverter.HasFlag(batteryByte, 0x10),
				IsFullyCharged = ByteConverter.HasFlag(batteryByte, 0x20),
				Level = (byte)(batteryByte & 0x0F)
			};
			IsHeadphoneConnected = ByteConverter.HasFlag(miscByte, 0x01);
		}

		// TODO: find a way to differentiate between the "valid" and "broken" BT states - now that stuff is working right,
		// we can always take the USB index. Hold the other one for when we can fix this (or prove it can't break)
		// this seems to be a discovery issue of some kind, other things (like steam and ds4windows) have no problem finding it.
		// seems to be fixed permanently after using DS4Windows but ideally we shouldn't have to have that precondition,
		// and steam was able to handle it fine before that
		/// <summary>
		/// Gets a data byte at the given index based on the input mode.
		/// </summary>
		/// <param name="inputMode">The current input mode.</param>
		/// <param name="data">The data bytes to read from.</param>
		/// <param name="indexIfUsb">The index to access in USB or valid Bluetooth input mode.</param>
		/// <param name="indexIfBt">The index to access in the broken Bluetooth input mode.</param>
		/// <returns>
		/// The data at the given index for the input mode, or 0 if the index is negative (allows defaults for
		/// values that aren't supported in a given mode).
		/// </returns>
		/// <remarks>
		/// This was due to a previous issue where controllers connected over Bluetooth were providing data bytes 
		/// in a different order with some data missing. It resolved itself before I could solve the problem but
		/// keeping this around for when I can find it again. Currently always uses <paramref name="indexIfUsb"/>.
		/// </remarks>
		private byte GetModeSwitch(IoMode inputMode, byte[] data, int indexIfUsb, int indexIfBt)
		{
			return indexIfUsb >= 0 ? data[indexIfUsb] : (byte)0;
			//return InputMode switch
			//{
			//    InputMode.USB => indexIfUsb >= 0 ? readData[indexIfUsb] : (byte)0,
			//    InputMode.Bluetooth => indexIfBt >= 0 ? readData[indexIfBt] : (byte)0,
			//    _ => throw new InvalidOperationException("")
			//};
		}

		/// <summary>
		/// Gets several data bytes at the given index based on the input mode.
		/// </summary>
		/// <param name="inputMode">The current input mode.</param>
		/// <param name="data">The data bytes to read from.</param>
		/// <param name="startIndexIfUsb">The start index in USB or valid Bluetooth input mode.</param>
		/// <param name="startIndexIfBt">The start index in the broken Bluetooth input mode.</param>
		/// <param name="size">The number of bytes to get.</param>
		/// <returns>
		/// <paramref name="size"/> bytes at the given start index for the input mode, or an array of <paramref name="size"/>
		/// 0's if the index is negative.
		/// </returns>
		/// <remarks>
		/// This was due to a previous issue where controllers connected over Bluetooth were providing data bytes 
		/// in a different order with some data missing. It resolved itself before I could solve the problem but
		/// keeping this around for when I can find it again. Currently always uses <paramref name="startIndexIfUsb"/>.
		/// </remarks>
		private byte[] GetModeSwitch(IoMode inputMode, byte[] data, int startIndexIfUsb, int startIndexIfBt, int size)
		{
			return startIndexIfUsb >= 0 ? data.Skip(startIndexIfUsb).Take(size).ToArray() : new byte[size];
			//return InputMode switch
			//{
			//    InputMode.USB => startIndexIfUsb >= 0 ? readData.Skip(startIndexIfUsb).Take(size).ToArray() : new byte[size],
			//    InputMode.Bluetooth => startIndexIfBt >= 0 ? readData.Skip(startIndexIfBt).Take(size).ToArray() : new byte[size],
			//    _ => throw new InvalidOperationException("")
			//};
		}

		/// <summary>
		/// Reads the 2 bytes of an analog stick and silences the dead zone.
		/// </summary>
		/// <param name="x">The x byte.</param>
		/// <param name="y">The y byte.</param>
		/// <returns>A vector for the joystick input.</returns>
		private Vec2 ReadAnalogStick(byte x, byte y, float deadZone)
		{
			float x1 = ByteConverter.ToSignedFloat(x);
			float y1 = -ByteConverter.ToSignedFloat(y);
			return new Vec2
			{
				X = Math.Abs(x1) >= deadZone ? x1 : 0,
				Y = Math.Abs(y1) >= deadZone ? y1 : 0
			};
		}

		/// <summary>
		/// Checks if the DPad lower nibble is one of the 3 values possible for a button.
		/// </summary>
		/// <param name="b">The dpad byte.</param>
		/// <param name="v1">The first value.</param>
		/// <param name="v2">The second value.</param>
		/// <param name="v3">The third value.</param>
		/// <returns>Whether the lower nibble of <paramref name="b"/> is one of the 3 values.</returns>
		private static bool ReadDPadButton(byte b, int v1, int v2, int v3)
		{
			int val = b & 0x0F;
			return val == v1 || val == v2 || val == v3;
		}

		/// <summary>
		/// Reads a touchpad.
		/// </summary>
		/// <param name="bytes">The touchpad's byte array.</param>
		/// <returns>A parsed <see cref="Touch"/>.</returns>
		private static Touch ReadTouchpad(byte[] bytes)
		{
			// force everything into the right byte order; input bytes are LSB-first
			if (!BitConverter.IsLittleEndian)
			{
				bytes = bytes.Reverse().ToArray();
			}
			uint raw = BitConverter.ToUInt32(bytes);
			return new Touch
			{
				X = (raw & 0x000FFF00) >> 8,
				Y = (raw & 0xFFF00000) >> 20,
				IsDown = (raw & 128) == 0,
				Id = bytes[0]
			};
		}

		/// <summary>
		/// Reads 3 axes of the accellerometer.
		/// </summary>
		/// <param name="x">The X axis bytes.</param>
		/// <param name="y">The Y axis bytes.</param>
		/// <param name="z">The Z axis bytes.</param>
		/// <returns>A vector for the gyro axes.</returns>
		private static Vec3 ReadAccelAxes(byte[] x, byte[] y, byte[] z)
		{
			// force everything into the right byte order; assuming that input bytes is little-endian
			if (!BitConverter.IsLittleEndian)
			{
				x = x.Reverse().ToArray();
				y = y.Reverse().ToArray();
				z = z.Reverse().ToArray();
			}
			return new Vec3
			{
				X = -BitConverter.ToInt16(x),
				Y = BitConverter.ToInt16(y),
				Z = BitConverter.ToInt16(z)
			};
		}

		/// <summary>
		/// The left analog stick. Values are from -1 to 1. Positive X is right, positive Y is up.
		/// </summary>
		public Vec2 LeftAnalogStick { get; private set; }

		/// <summary>
		/// The right analog stick. Values are from -1 to 1. Positive X is right, positive Y is up.
		/// </summary>
		public Vec2 RightAnalogStick { get; private set; }

		/// <summary>
		/// L2's analog value, from 0 to 1.
		/// </summary>
		public float L2 { get; private set; }

		/// <summary>
		/// R2's analog value, from 0 to 1.
		/// </summary>
		public float R2 { get; private set; }

		/// <summary>
		/// The status of the square button.
		/// </summary>
		public bool SquareButton { get; private set; }

		/// <summary>
		/// The status of the cross button.
		/// </summary>
		public bool CrossButton { get; private set; }

		/// <summary>
		/// The status of the circle button.
		/// </summary>
		public bool CircleButton { get; private set; }

		/// <summary>
		/// The status of the triangle button.
		/// </summary>
		public bool TriangleButton { get; private set; }

		/// <summary>
		/// The status of the D-pad up button.
		/// </summary>
		public bool DPadUpButton { get; private set; }

		/// <summary>
		/// The status of the D-pad right button.
		/// </summary>
		public bool DPadRightButton { get; private set; }

		/// <summary>
		/// The status of the D-pad down button.
		/// </summary>
		public bool DPadDownButton { get; private set; }

		/// <summary>
		/// The status of the D-pad left button.
		/// </summary>
		public bool DPadLeftButton { get; private set; }

		/// <summary>
		/// The status of the L1 button.
		/// </summary>
		public bool L1Button { get; private set; }

		/// <summary>
		/// The status of the R1 button.
		/// </summary>
		public bool R1Button { get; private set; }

		/// <summary>
		/// The status of the L2 button.
		/// </summary>
		public bool L2Button { get; private set; }

		/// <summary>
		/// The status of the R2 button.
		/// </summary>
		public bool R2Button { get; private set; }

		/// <summary>
		/// The status of the create button.
		/// </summary>
		public bool CreateButton { get; private set; }

		/// <summary>
		/// The status of the menu button.
		/// </summary>
		public bool MenuButton { get; private set; }

		/// <summary>
		/// The status of the L3 button.
		/// </summary>
		public bool L3Button { get; private set; }

		/// <summary>
		/// The status of the R3 button.
		/// </summary>
		public bool R3Button { get; private set; }

		/// <summary>
		/// The status of the PlayStation logo button.
		/// </summary>
		public bool LogoButton { get; private set; }

		/// <summary>
		/// The status of the touchpad button.
		/// </summary>
		public bool TouchpadButton { get; private set; }

		/// <summary>
		/// The status of the mic button.
		/// </summary>
		public bool MicButton { get; private set; }

		/// <summary>
		/// The first touch point.
		/// </summary>
		public Touch Touchpad1 { get; private set; }

		/// <summary>
		/// The second touch point.
		/// </summary>
		public Touch Touchpad2 { get; private set; }

		/// <summary>
		/// The accelerometer's rotational axes. The directions of the axes have been slightly adjusted from the controller's original values
		/// to make them behave nicer with standard Newtonian physics. The signs follow normal right-hand rule with respect to
		/// <see cref="Accelerometer"/>'s axes, e.g. +X rotation means counterclockwise around the +X axis and so on. Unit is unclear, but
		/// magnitude while stationary is about 0.
		/// </summary>
		public Vec3 Gyro { get; private set; }

		/// <summary>
		/// The accelerometer's linear axes. The directions of the axes have been slightly adjusted from the controller's original values
		/// to make them behave nicer with standard Newtonian physics. +X is to the right. +Y is behind the controller (roughly straight down
		/// if the controller is flat on the table). +Z is at the top of the controller (where the USB port is). Unit is unclear, but magnitude
		/// while stationary (e.g. just gravity) is about 8000 +- 100.
		/// </summary>
		public Vec3 Accelerometer { get; private set; }

		/// <summary>
		/// The status of the battery.
		/// </summary>
		public BatteryStatus BatteryStatus { get; private set; }

		/// <summary>
		/// Whether or not headphones are connected to the controller.
		/// </summary>
		public bool IsHeadphoneConnected { get; private set; }
	}
}
