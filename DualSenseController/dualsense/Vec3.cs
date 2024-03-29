﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DualSenseController.dualsense
{
	/// <summary>
	/// A 3D vector
	/// </summary>
	public struct Vec3
	{
		public float X, Y, Z;

		public float Magnitude()
		{
			return (float)Math.Sqrt(X * X + Y * Y + Z * Z);
		}

		public Vec3 Normalize()
		{
			float m = Magnitude();
			return new Vec3 { X = X / m, Y = Y / m, Z = Z / m };
		}

		public static Vec3 operator -(Vec3 v)
		{
			return new Vec3 { X = -v.X, Y = -v.Y, Z = -v.Z };
		}
	}
}
