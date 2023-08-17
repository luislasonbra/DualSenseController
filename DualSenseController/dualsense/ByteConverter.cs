﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DualSenseController.dualsense
{
	/// <summary>
	/// help conversion between bytes and more useful formats.
	/// </summary>
	public static class ByteConverter
	{
		/// <summary>
		/// Converts a byte to the corresponding signed float.
		/// </summary>
		/// <param name="b">The byte value</param>
		/// <returns>The byte, scaled and translated to floating point value between -1 and 1.</returns>
		public static float ToSignedFloat(byte b)
		{
			return (b / 255.0f - 0.5f) * 2.0f;
		}

		/// <summary>
		/// Converts a byte to the corresponding unsigned float.
		/// </summary>
		/// <param name="b">The byte value</param>
		/// <returns>The byte, scaled to a floating point value between 0 and 1.</returns>
		public static float ToUnsignedFloat(byte b)
		{
			return b / 255.0f;
		}

		/// <summary>
		/// Checks whether the provided flag's bits are set on this byte. Similar to <see cref="Enum.HasFlag(Enum)"/>.
		/// </summary>
		/// <param name="b">The byte value</param>
		/// <param name="flag">The flag to check</param>
		/// <returns>Whether all the bits of the flag are set on the byte.</returns>
		public static bool HasFlag(byte b, byte flag)
		{
			return (b & flag) == flag;
		}

		/// <summary>
		/// Converts an unsigned float to the corresponding byte.
		/// </summary>
		/// <param name="f">The float value</param>
		/// <returns>The float, clamped and scaled between 0 and 255.</returns>
		public static byte UnsignedToByte(float f)
		{
			return (byte)(Math.Clamp(f, 0, 1) * 255);
		}
	}
}
