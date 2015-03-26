using System;
using SharpDX;

namespace Helper.Math
{
    public static class MathHelper
    {
        // MathHelper.DegreesToRadians(90f)
        public const Single RightAngleRadians = 1.570794993f;

        private const Single DirectionRad = 11.37777777777778f;
        private const Single PiDegrees = 0.0174532777777778f;
	    private const Single Epsilon = 1.192092896e-07f;

		public static Boolean FloatEqual(Single a, Single b)
		{
			return (System.Math.Abs(a - b) <= Epsilon * System.Math.Max(System.Math.Abs(a), System.Math.Abs(b)));
		}

        public static Single DegreesToRadians(Single degrees)
        {
            return degrees * 0.0174532777f;
        }

        public static Single DirectionToRadians(Single direction)
        {
            return (direction / DirectionRad) * PiDegrees;
        }

        public static Single RadiansToDirection(Single radians)
        {
            return ((radians * DirectionRad) * DirectionRad) * 5.035766f;
        }

        public static Matrix CreateMatrixFromAxisAngle(Vector3 axis, Single angle)
        {
            Matrix matrix;

            Single x = axis.X;
            Single y = axis.Y;
            Single z = axis.Z;
			Single num2 = (Single)System.Math.Sin(angle);
			Single num = (Single)System.Math.Cos(angle);
            Single num11 = x * x;
            Single num10 = y * y;
            Single num9 = z * z;
            Single num8 = x * y;
            Single num7 = x * z;
            Single num6 = y * z;
            matrix.M11 = num11 + (num * (1f - num11));
            matrix.M12 = (num8 - (num * num8)) + (num2 * z);
            matrix.M13 = (num7 - (num * num7)) - (num2 * y);
            matrix.M14 = 0f;
            matrix.M21 = (num8 - (num * num8)) - (num2 * z);
            matrix.M22 = num10 + (num * (1f - num10));
            matrix.M23 = (num6 - (num * num6)) + (num2 * x);
            matrix.M24 = 0f;
            matrix.M31 = (num7 - (num * num7)) + (num2 * y);
            matrix.M32 = (num6 - (num * num6)) - (num2 * x);
            matrix.M33 = num9 + (num * (1f - num9));
            matrix.M34 = 0f;
            matrix.M41 = 0f;
            matrix.M42 = 0f;
            matrix.M43 = 0f;
            matrix.M44 = 1f;

            return matrix;
        }
    }
}
