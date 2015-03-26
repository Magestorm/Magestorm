using System;
using System.Security.Cryptography;

namespace Helper
{
    public class CryptoRandom : RandomNumberGenerator
    {
        [ThreadStatic]
        private static CryptoRandom _cryptoRandom;

        private static CryptoRandom Random
        {
            get
            {
                return _cryptoRandom ?? (_cryptoRandom = new CryptoRandom());
            }
        }

        private readonly RandomNumberGenerator _randomNumberGenerator;

        public CryptoRandom()
        {
            _randomNumberGenerator = Create();
        }

        public override void GetBytes(Byte[] buffer)
        {
            _randomNumberGenerator.GetBytes(buffer);
        }

        public Double NextDouble()
        {
            Byte[] buffer = new Byte[4];
            _randomNumberGenerator.GetBytes(buffer);
            return (Double)BitConverter.ToUInt32(buffer, 0) / UInt32.MaxValue;
        }

        public Int32 Next(Int32 minValue, Int32 maxValue)
        {
			return ((Int32)System.Math.Ceiling(NextDouble() * (maxValue - (minValue - 1))) + (minValue - 1));
        }

        public Int32 Next()
        {
            return Next(0, Int32.MaxValue);
        }

        public Int32 Next(Int32 maxValue)
        {
            return Next(0, maxValue);
        }

        public static Byte GetByte(Byte min, Byte max)
        {
            return (Byte)Random.Next(min, max);
        }

        public static Int16 GetInt16(Int16 min, Int16 max)
        {
            return (Int16)Random.Next(min, max);
        }

        public static UInt16 GetUInt16(Int16 min, Int16 max)
        {
            return (UInt16)Random.Next(min, max);
        }

        public static Int32 GetInt32(Int32 min, Int32 max)
        {
            return Random.Next(min, max);
        }
    }
}