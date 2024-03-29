using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using SkyShoot.XNA.Framework.Design;

namespace SkyShoot.XNA.Framework
{
	[Serializable, StructLayout(LayoutKind.Sequential), TypeConverter(typeof(Vector2Converter))]
    public struct Vector2 : IEquatable<Vector2>
    {
        public float X;
        public float Y;
        private static Vector2 _zero;
        private static Vector2 _one;
        private static Vector2 _unitX;
        private static Vector2 _unitY;
        public static Vector2 Zero
        {
            get
            {
                return _zero;
            }
        }
        public static Vector2 One
        {
            get
            {
                return _one;
            }
        }
        public static Vector2 UnitX
        {
            get
            {
                return _unitX;
            }
        }
        public static Vector2 UnitY
        {
            get
            {
                return _unitY;
            }
        }
        public Vector2(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        public Vector2(float value)
        {
            this.X = this.Y = value;
        }

        public override string ToString()
        {
            CultureInfo currentCulture = CultureInfo.CurrentCulture;
            return string.Format(currentCulture, "{{X:{0} Y:{1}}}", new object[] { this.X.ToString(currentCulture), this.Y.ToString(currentCulture) });
        }

        public bool Equals(Vector2 other)
        {
            return ((this.X == other.X) && (this.Y == other.Y));
        }

        public override bool Equals(object obj)
        {
            bool flag = false;
            if (obj is Vector2)
            {
                flag = this.Equals((Vector2) obj);
            }
            return flag;
        }

        public override int GetHashCode()
        {
            return (this.X.GetHashCode() + this.Y.GetHashCode());
        }

        public float Length()
        {
            float num = (this.X * this.X) + (this.Y * this.Y);
            return (float) Math.Sqrt((double) num);
        }

        public float LengthSquared()
        {
            return ((this.X * this.X) + (this.Y * this.Y));
        }

        public static float Distance(Vector2 value1, Vector2 value2)
        {
            float num2 = value1.X - value2.X;
            float num = value1.Y - value2.Y;
            float num3 = (num2 * num2) + (num * num);
            return (float) Math.Sqrt((double) num3);
        }

        public static void Distance(ref Vector2 value1, ref Vector2 value2, out float result)
        {
            float num2 = value1.X - value2.X;
            float num = value1.Y - value2.Y;
            float num3 = (num2 * num2) + (num * num);
            result = (float) Math.Sqrt((double) num3);
        }

        public static float DistanceSquared(Vector2 value1, Vector2 value2)
        {
            float num2 = value1.X - value2.X;
            float num = value1.Y - value2.Y;
            return ((num2 * num2) + (num * num));
        }

        public static void DistanceSquared(ref Vector2 value1, ref Vector2 value2, out float result)
        {
            float num2 = value1.X - value2.X;
            float num = value1.Y - value2.Y;
            result = (num2 * num2) + (num * num);
        }

        public static float Dot(Vector2 value1, Vector2 value2)
        {
            return ((value1.X * value2.X) + (value1.Y * value2.Y));
        }

        public static void Dot(ref Vector2 value1, ref Vector2 value2, out float result)
        {
            result = (value1.X * value2.X) + (value1.Y * value2.Y);
        }

        public void Normalize()
        {
            float num2 = (this.X * this.X) + (this.Y * this.Y);
            float num = 1f / ((float) Math.Sqrt((double) num2));
            this.X *= num;
            this.Y *= num;
        }

        public static Vector2 Normalize(Vector2 value)
        {
            Vector2 vector;
            float num2 = (value.X * value.X) + (value.Y * value.Y);
            float num = 1f / ((float) Math.Sqrt((double) num2));
            vector.X = value.X * num;
            vector.Y = value.Y * num;
            return vector;
        }

        public static void Normalize(ref Vector2 value, out Vector2 result)
        {
            float num2 = (value.X * value.X) + (value.Y * value.Y);
            float num = 1f / ((float) Math.Sqrt((double) num2));
            result.X = value.X * num;
            result.Y = value.Y * num;
        }

        public static Vector2 Reflect(Vector2 vector, Vector2 normal)
        {
            Vector2 vector2;
            float num = (vector.X * normal.X) + (vector.Y * normal.Y);
            vector2.X = vector.X - ((2f * num) * normal.X);
            vector2.Y = vector.Y - ((2f * num) * normal.Y);
            return vector2;
        }

        public static void Reflect(ref Vector2 vector, ref Vector2 normal, out Vector2 result)
        {
            float num = (vector.X * normal.X) + (vector.Y * normal.Y);
            result.X = vector.X - ((2f * num) * normal.X);
            result.Y = vector.Y - ((2f * num) * normal.Y);
        }

        public static Vector2 Min(Vector2 value1, Vector2 value2)
        {
            Vector2 vector;
            vector.X = (value1.X < value2.X) ? value1.X : value2.X;
            vector.Y = (value1.Y < value2.Y) ? value1.Y : value2.Y;
            return vector;
        }

        public static void Min(ref Vector2 value1, ref Vector2 value2, out Vector2 result)
        {
            result.X = (value1.X < value2.X) ? value1.X : value2.X;
            result.Y = (value1.Y < value2.Y) ? value1.Y : value2.Y;
        }

        public static Vector2 Max(Vector2 value1, Vector2 value2)
        {
            Vector2 vector;
            vector.X = (value1.X > value2.X) ? value1.X : value2.X;
            vector.Y = (value1.Y > value2.Y) ? value1.Y : value2.Y;
            return vector;
        }

        public static void Max(ref Vector2 value1, ref Vector2 value2, out Vector2 result)
        {
            result.X = (value1.X > value2.X) ? value1.X : value2.X;
            result.Y = (value1.Y > value2.Y) ? value1.Y : value2.Y;
        }

        public static Vector2 Clamp(Vector2 value1, Vector2 min, Vector2 max)
        {
            Vector2 vector;
            float x = value1.X;
            x = (x > max.X) ? max.X : x;
            x = (x < min.X) ? min.X : x;
            float y = value1.Y;
            y = (y > max.Y) ? max.Y : y;
            y = (y < min.Y) ? min.Y : y;
            vector.X = x;
            vector.Y = y;
            return vector;
        }

        public static void Clamp(ref Vector2 value1, ref Vector2 min, ref Vector2 max, out Vector2 result)
        {
            float x = value1.X;
            x = (x > max.X) ? max.X : x;
            x = (x < min.X) ? min.X : x;
            float y = value1.Y;
            y = (y > max.Y) ? max.Y : y;
            y = (y < min.Y) ? min.Y : y;
            result.X = x;
            result.Y = y;
        }

        public static Vector2 Lerp(Vector2 value1, Vector2 value2, float amount)
        {
            Vector2 vector;
            vector.X = value1.X + ((value2.X - value1.X) * amount);
            vector.Y = value1.Y + ((value2.Y - value1.Y) * amount);
            return vector;
        }

        public static void Lerp(ref Vector2 value1, ref Vector2 value2, float amount, out Vector2 result)
        {
            result.X = value1.X + ((value2.X - value1.X) * amount);
            result.Y = value1.Y + ((value2.Y - value1.Y) * amount);
        }

        public static Vector2 Barycentric(Vector2 value1, Vector2 value2, Vector2 value3, float amount1, float amount2)
        {
            Vector2 vector;
            vector.X = (value1.X + (amount1 * (value2.X - value1.X))) + (amount2 * (value3.X - value1.X));
            vector.Y = (value1.Y + (amount1 * (value2.Y - value1.Y))) + (amount2 * (value3.Y - value1.Y));
            return vector;
        }

        public static void Barycentric(ref Vector2 value1, ref Vector2 value2, ref Vector2 value3, float amount1, float amount2, out Vector2 result)
        {
            result.X = (value1.X + (amount1 * (value2.X - value1.X))) + (amount2 * (value3.X - value1.X));
            result.Y = (value1.Y + (amount1 * (value2.Y - value1.Y))) + (amount2 * (value3.Y - value1.Y));
        }

        public static Vector2 SmoothStep(Vector2 value1, Vector2 value2, float amount)
        {
            Vector2 vector;
            amount = (amount > 1f) ? 1f : ((amount < 0f) ? 0f : amount);
            amount = (amount * amount) * (3f - (2f * amount));
            vector.X = value1.X + ((value2.X - value1.X) * amount);
            vector.Y = value1.Y + ((value2.Y - value1.Y) * amount);
            return vector;
        }

        public static void SmoothStep(ref Vector2 value1, ref Vector2 value2, float amount, out Vector2 result)
        {
            amount = (amount > 1f) ? 1f : ((amount < 0f) ? 0f : amount);
            amount = (amount * amount) * (3f - (2f * amount));
            result.X = value1.X + ((value2.X - value1.X) * amount);
            result.Y = value1.Y + ((value2.Y - value1.Y) * amount);
        }

        public static Vector2 CatmullRom(Vector2 value1, Vector2 value2, Vector2 value3, Vector2 value4, float amount)
        {
            Vector2 vector;
            float num = amount * amount;
            float num2 = amount * num;
            vector.X = 0.5f * ((((2f * value2.X) + ((-value1.X + value3.X) * amount)) + (((((2f * value1.X) - (5f * value2.X)) + (4f * value3.X)) - value4.X) * num)) + ((((-value1.X + (3f * value2.X)) - (3f * value3.X)) + value4.X) * num2));
            vector.Y = 0.5f * ((((2f * value2.Y) + ((-value1.Y + value3.Y) * amount)) + (((((2f * value1.Y) - (5f * value2.Y)) + (4f * value3.Y)) - value4.Y) * num)) + ((((-value1.Y + (3f * value2.Y)) - (3f * value3.Y)) + value4.Y) * num2));
            return vector;
        }

        public static void CatmullRom(ref Vector2 value1, ref Vector2 value2, ref Vector2 value3, ref Vector2 value4, float amount, out Vector2 result)
        {
            float num = amount * amount;
            float num2 = amount * num;
            result.X = 0.5f * ((((2f * value2.X) + ((-value1.X + value3.X) * amount)) + (((((2f * value1.X) - (5f * value2.X)) + (4f * value3.X)) - value4.X) * num)) + ((((-value1.X + (3f * value2.X)) - (3f * value3.X)) + value4.X) * num2));
            result.Y = 0.5f * ((((2f * value2.Y) + ((-value1.Y + value3.Y) * amount)) + (((((2f * value1.Y) - (5f * value2.Y)) + (4f * value3.Y)) - value4.Y) * num)) + ((((-value1.Y + (3f * value2.Y)) - (3f * value3.Y)) + value4.Y) * num2));
        }

        public static Vector2 Hermite(Vector2 value1, Vector2 tangent1, Vector2 value2, Vector2 tangent2, float amount)
        {
            Vector2 vector;
            float num = amount * amount;
            float num2 = amount * num;
            float num6 = ((2f * num2) - (3f * num)) + 1f;
            float num5 = (-2f * num2) + (3f * num);
            float num4 = (num2 - (2f * num)) + amount;
            float num3 = num2 - num;
            vector.X = (((value1.X * num6) + (value2.X * num5)) + (tangent1.X * num4)) + (tangent2.X * num3);
            vector.Y = (((value1.Y * num6) + (value2.Y * num5)) + (tangent1.Y * num4)) + (tangent2.Y * num3);
            return vector;
        }

        public static void Hermite(ref Vector2 value1, ref Vector2 tangent1, ref Vector2 value2, ref Vector2 tangent2, float amount, out Vector2 result)
        {
            float num = amount * amount;
            float num2 = amount * num;
            float num6 = ((2f * num2) - (3f * num)) + 1f;
            float num5 = (-2f * num2) + (3f * num);
            float num4 = (num2 - (2f * num)) + amount;
            float num3 = num2 - num;
            result.X = (((value1.X * num6) + (value2.X * num5)) + (tangent1.X * num4)) + (tangent2.X * num3);
            result.Y = (((value1.Y * num6) + (value2.Y * num5)) + (tangent1.Y * num4)) + (tangent2.Y * num3);
        }

        public static Vector2 Transform(Vector2 position, Matrix matrix)
        {
            Vector2 vector;
            float num2 = ((position.X * matrix.M11) + (position.Y * matrix.M21)) + matrix.M41;
            float num = ((position.X * matrix.M12) + (position.Y * matrix.M22)) + matrix.M42;
            vector.X = num2;
            vector.Y = num;
            return vector;
        }

        public static void Transform(ref Vector2 position, ref Matrix matrix, out Vector2 result)
        {
            float num2 = ((position.X * matrix.M11) + (position.Y * matrix.M21)) + matrix.M41;
            float num = ((position.X * matrix.M12) + (position.Y * matrix.M22)) + matrix.M42;
            result.X = num2;
            result.Y = num;
        }

        public static Vector2 TransformNormal(Vector2 normal, Matrix matrix)
        {
            Vector2 vector;
            float num2 = (normal.X * matrix.M11) + (normal.Y * matrix.M21);
            float num = (normal.X * matrix.M12) + (normal.Y * matrix.M22);
            vector.X = num2;
            vector.Y = num;
            return vector;
        }

        public static void TransformNormal(ref Vector2 normal, ref Matrix matrix, out Vector2 result)
        {
            float num2 = (normal.X * matrix.M11) + (normal.Y * matrix.M21);
            float num = (normal.X * matrix.M12) + (normal.Y * matrix.M22);
            result.X = num2;
            result.Y = num;
        }

        public static Vector2 Transform(Vector2 value, Quaternion rotation)
        {
            Vector2 vector;
            float num10 = rotation.X + rotation.X;
            float num5 = rotation.Y + rotation.Y;
            float num4 = rotation.Z + rotation.Z;
            float num3 = rotation.W * num4;
            float num9 = rotation.X * num10;
            float num2 = rotation.X * num5;
            float num8 = rotation.Y * num5;
            float num = rotation.Z * num4;
            float num7 = (value.X * ((1f - num8) - num)) + (value.Y * (num2 - num3));
            float num6 = (value.X * (num2 + num3)) + (value.Y * ((1f - num9) - num));
            vector.X = num7;
            vector.Y = num6;
            return vector;
        }

        public static void Transform(ref Vector2 value, ref Quaternion rotation, out Vector2 result)
        {
            float num10 = rotation.X + rotation.X;
            float num5 = rotation.Y + rotation.Y;
            float num4 = rotation.Z + rotation.Z;
            float num3 = rotation.W * num4;
            float num9 = rotation.X * num10;
            float num2 = rotation.X * num5;
            float num8 = rotation.Y * num5;
            float num = rotation.Z * num4;
            float num7 = (value.X * ((1f - num8) - num)) + (value.Y * (num2 - num3));
            float num6 = (value.X * (num2 + num3)) + (value.Y * ((1f - num9) - num));
            result.X = num7;
            result.Y = num6;
        }

        public static void Transform(Vector2[] sourceArray, ref Matrix matrix, Vector2[] destinationArray)
        {
            if (sourceArray == null)
            {
                throw new ArgumentNullException("sourceArray");
            }
            if (destinationArray == null)
            {
                throw new ArgumentNullException("destinationArray");
            }
            if (destinationArray.Length < sourceArray.Length)
            {
                throw new ArgumentException(FrameworkResources.NotEnoughTargetSize);
            }
            for (int i = 0; i < sourceArray.Length; i++)
            {
                float x = sourceArray[i].X;
                float y = sourceArray[i].Y;
                destinationArray[i].X = ((x * matrix.M11) + (y * matrix.M21)) + matrix.M41;
                destinationArray[i].Y = ((x * matrix.M12) + (y * matrix.M22)) + matrix.M42;
            }
        }

        public static void Transform(Vector2[] sourceArray, int sourceIndex, ref Matrix matrix, Vector2[] destinationArray, int destinationIndex, int length)
        {
            if (sourceArray == null)
            {
                throw new ArgumentNullException("sourceArray");
            }
            if (destinationArray == null)
            {
                throw new ArgumentNullException("destinationArray");
            }
            if (sourceArray.Length < (sourceIndex + length))
            {
                throw new ArgumentException(FrameworkResources.NotEnoughSourceSize);
            }
            if (destinationArray.Length < (destinationIndex + length))
            {
                throw new ArgumentException(FrameworkResources.NotEnoughTargetSize);
            }
            while (length > 0)
            {
                float x = sourceArray[sourceIndex].X;
                float y = sourceArray[sourceIndex].Y;
                destinationArray[destinationIndex].X = ((x * matrix.M11) + (y * matrix.M21)) + matrix.M41;
                destinationArray[destinationIndex].Y = ((x * matrix.M12) + (y * matrix.M22)) + matrix.M42;
                sourceIndex++;
                destinationIndex++;
                length--;
            }
        }

        public static void TransformNormal(Vector2[] sourceArray, ref Matrix matrix, Vector2[] destinationArray)
        {
            if (sourceArray == null)
            {
                throw new ArgumentNullException("sourceArray");
            }
            if (destinationArray == null)
            {
                throw new ArgumentNullException("destinationArray");
            }
            if (destinationArray.Length < sourceArray.Length)
            {
                throw new ArgumentException(FrameworkResources.NotEnoughTargetSize);
            }
            for (int i = 0; i < sourceArray.Length; i++)
            {
                float x = sourceArray[i].X;
                float y = sourceArray[i].Y;
                destinationArray[i].X = (x * matrix.M11) + (y * matrix.M21);
                destinationArray[i].Y = (x * matrix.M12) + (y * matrix.M22);
            }
        }

        public static void TransformNormal(Vector2[] sourceArray, int sourceIndex, ref Matrix matrix, Vector2[] destinationArray, int destinationIndex, int length)
        {
            if (sourceArray == null)
            {
                throw new ArgumentNullException("sourceArray");
            }
            if (destinationArray == null)
            {
                throw new ArgumentNullException("destinationArray");
            }
            if (sourceArray.Length < (sourceIndex + length))
            {
                throw new ArgumentException(FrameworkResources.NotEnoughSourceSize);
            }
            if (destinationArray.Length < (destinationIndex + length))
            {
                throw new ArgumentException(FrameworkResources.NotEnoughTargetSize);
            }
            while (length > 0)
            {
                float x = sourceArray[sourceIndex].X;
                float y = sourceArray[sourceIndex].Y;
                destinationArray[destinationIndex].X = (x * matrix.M11) + (y * matrix.M21);
                destinationArray[destinationIndex].Y = (x * matrix.M12) + (y * matrix.M22);
                sourceIndex++;
                destinationIndex++;
                length--;
            }
        }

        public static void Transform(Vector2[] sourceArray, ref Quaternion rotation, Vector2[] destinationArray)
        {
            if (sourceArray == null)
            {
                throw new ArgumentNullException("sourceArray");
            }
            if (destinationArray == null)
            {
                throw new ArgumentNullException("destinationArray");
            }
            if (destinationArray.Length < sourceArray.Length)
            {
                throw new ArgumentException(FrameworkResources.NotEnoughTargetSize);
            }
            float num15 = rotation.X + rotation.X;
            float num8 = rotation.Y + rotation.Y;
            float num7 = rotation.Z + rotation.Z;
            float num6 = rotation.W * num7;
            float num14 = rotation.X * num15;
            float num5 = rotation.X * num8;
            float num13 = rotation.Y * num8;
            float num4 = rotation.Z * num7;
            float num12 = (1f - num13) - num4;
            float num11 = num5 - num6;
            float num10 = num5 + num6;
            float num9 = (1f - num14) - num4;
            for (int i = 0; i < sourceArray.Length; i++)
            {
                float x = sourceArray[i].X;
                float y = sourceArray[i].Y;
                destinationArray[i].X = (x * num12) + (y * num11);
                destinationArray[i].Y = (x * num10) + (y * num9);
            }
        }

        public static void Transform(Vector2[] sourceArray, int sourceIndex, ref Quaternion rotation, Vector2[] destinationArray, int destinationIndex, int length)
        {
            if (sourceArray == null)
            {
                throw new ArgumentNullException("sourceArray");
            }
            if (destinationArray == null)
            {
                throw new ArgumentNullException("destinationArray");
            }
            if (sourceArray.Length < (sourceIndex + length))
            {
                throw new ArgumentException(FrameworkResources.NotEnoughSourceSize);
            }
            if (destinationArray.Length < (destinationIndex + length))
            {
                throw new ArgumentException(FrameworkResources.NotEnoughTargetSize);
            }
            float num14 = rotation.X + rotation.X;
            float num7 = rotation.Y + rotation.Y;
            float num6 = rotation.Z + rotation.Z;
            float num5 = rotation.W * num6;
            float num13 = rotation.X * num14;
            float num4 = rotation.X * num7;
            float num12 = rotation.Y * num7;
            float num3 = rotation.Z * num6;
            float num11 = (1f - num12) - num3;
            float num10 = num4 - num5;
            float num9 = num4 + num5;
            float num8 = (1f - num13) - num3;
            while (length > 0)
            {
                float x = sourceArray[sourceIndex].X;
                float y = sourceArray[sourceIndex].Y;
                destinationArray[destinationIndex].X = (x * num11) + (y * num10);
                destinationArray[destinationIndex].Y = (x * num9) + (y * num8);
                sourceIndex++;
                destinationIndex++;
                length--;
            }
        }

        public static Vector2 Negate(Vector2 value)
        {
            Vector2 vector;
            vector.X = -value.X;
            vector.Y = -value.Y;
            return vector;
        }

        public static void Negate(ref Vector2 value, out Vector2 result)
        {
            result.X = -value.X;
            result.Y = -value.Y;
        }

        public static Vector2 Add(Vector2 value1, Vector2 value2)
        {
            Vector2 vector;
            vector.X = value1.X + value2.X;
            vector.Y = value1.Y + value2.Y;
            return vector;
        }

        public static void Add(ref Vector2 value1, ref Vector2 value2, out Vector2 result)
        {
            result.X = value1.X + value2.X;
            result.Y = value1.Y + value2.Y;
        }

        public static Vector2 Subtract(Vector2 value1, Vector2 value2)
        {
            Vector2 vector;
            vector.X = value1.X - value2.X;
            vector.Y = value1.Y - value2.Y;
            return vector;
        }

        public static void Subtract(ref Vector2 value1, ref Vector2 value2, out Vector2 result)
        {
            result.X = value1.X - value2.X;
            result.Y = value1.Y - value2.Y;
        }

        public static Vector2 Multiply(Vector2 value1, Vector2 value2)
        {
            Vector2 vector;
            vector.X = value1.X * value2.X;
            vector.Y = value1.Y * value2.Y;
            return vector;
        }

        public static void Multiply(ref Vector2 value1, ref Vector2 value2, out Vector2 result)
        {
            result.X = value1.X * value2.X;
            result.Y = value1.Y * value2.Y;
        }

        public static Vector2 Multiply(Vector2 value1, float scaleFactor)
        {
            Vector2 vector;
            vector.X = value1.X * scaleFactor;
            vector.Y = value1.Y * scaleFactor;
            return vector;
        }

        public static void Multiply(ref Vector2 value1, float scaleFactor, out Vector2 result)
        {
            result.X = value1.X * scaleFactor;
            result.Y = value1.Y * scaleFactor;
        }

        public static Vector2 Divide(Vector2 value1, Vector2 value2)
        {
            Vector2 vector;
            vector.X = value1.X / value2.X;
            vector.Y = value1.Y / value2.Y;
            return vector;
        }

        public static void Divide(ref Vector2 value1, ref Vector2 value2, out Vector2 result)
        {
            result.X = value1.X / value2.X;
            result.Y = value1.Y / value2.Y;
        }

        public static Vector2 Divide(Vector2 value1, float divider)
        {
            Vector2 vector;
            float num = 1f / divider;
            vector.X = value1.X * num;
            vector.Y = value1.Y * num;
            return vector;
        }

        public static void Divide(ref Vector2 value1, float divider, out Vector2 result)
        {
            float num = 1f / divider;
            result.X = value1.X * num;
            result.Y = value1.Y * num;
        }

        public static Vector2 operator -(Vector2 value)
        {
            Vector2 vector;
            vector.X = -value.X;
            vector.Y = -value.Y;
            return vector;
        }

        public static bool operator ==(Vector2 value1, Vector2 value2)
        {
            return ((value1.X == value2.X) && (value1.Y == value2.Y));
        }

        public static bool operator !=(Vector2 value1, Vector2 value2)
        {
            if (value1.X == value2.X)
            {
                return !(value1.Y == value2.Y);
            }
            return true;
        }

        public static Vector2 operator +(Vector2 value1, Vector2 value2)
        {
            Vector2 vector;
            vector.X = value1.X + value2.X;
            vector.Y = value1.Y + value2.Y;
            return vector;
        }

        public static Vector2 operator -(Vector2 value1, Vector2 value2)
        {
            Vector2 vector;
            vector.X = value1.X - value2.X;
            vector.Y = value1.Y - value2.Y;
            return vector;
        }

        public static Vector2 operator *(Vector2 value1, Vector2 value2)
        {
            Vector2 vector;
            vector.X = value1.X * value2.X;
            vector.Y = value1.Y * value2.Y;
            return vector;
        }

        public static Vector2 operator *(Vector2 value, float scaleFactor)
        {
            Vector2 vector;
            vector.X = value.X * scaleFactor;
            vector.Y = value.Y * scaleFactor;
            return vector;
        }

        public static Vector2 operator *(float scaleFactor, Vector2 value)
        {
            Vector2 vector;
            vector.X = value.X * scaleFactor;
            vector.Y = value.Y * scaleFactor;
            return vector;
        }

        public static Vector2 operator /(Vector2 value1, Vector2 value2)
        {
            Vector2 vector;
            vector.X = value1.X / value2.X;
            vector.Y = value1.Y / value2.Y;
            return vector;
        }

        public static Vector2 operator /(Vector2 value1, float divider)
        {
            Vector2 vector;
            float num = 1f / divider;
            vector.X = value1.X * num;
            vector.Y = value1.Y * num;
            return vector;
        }

        static Vector2()
        {
            _zero = new Vector2();
            _one = new Vector2(1f, 1f);
            _unitX = new Vector2(1f, 0f);
            _unitY = new Vector2(0f, 1f);
        }
    }
}

