using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DualSenseController.dualsense2
{
	/// <summary>
	/// A 2D vector
	/// </summary>
	public struct Vec2
	{
		public float X, Y;

		public float Magnitude()
		{
			return (float)Math.Sqrt(X * X + Y * Y);
		}

		public Vec2 Normalize()
		{
			float m = Magnitude();
			return new Vec2 { X = X / m, Y = Y / m };
		}

		public static Vec2 operator -(Vec2 v)
		{
			return new Vec2 { X = -v.X, Y = -v.Y };
		}
	}
}
