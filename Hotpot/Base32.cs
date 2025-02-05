using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotpot
{
    public static class Base32
    {
        private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";

        /// <summary>
        /// Encodes a byte array into a Base32 string.
        /// </summary>
        /// <param name="data">The byte array to encode.</param>
        /// <returns>The Base32-encoded string.</returns>
        public static string Encode(byte[] data)
        {
            if (data.Length == 0) return string.Empty;
            StringBuilder output = new();
            int bitBuffer = 0, bitCount = 0;

            foreach (byte b in data)
            {
                bitBuffer = (bitBuffer << 8) | b;
                bitCount += 8;
                while (bitCount >= 5)
                {
                    output.Append(Alphabet[(bitBuffer >> (bitCount - 5)) & 0x1F]);
                    bitCount -= 5;
                }
            }

            if (bitCount > 0)
            {
                output.Append(Alphabet[(bitBuffer << (5 - bitCount)) & 0x1F]);
            }
            return output.ToString();
        }

        /// <summary>
        /// Decodes a Base32-encoded string into a byte array.
        /// </summary>
        /// <param name="encoded">The Base32-encoded string.</param>
        /// <returns>The decoded byte array.</returns>
        public static byte[] Decode(string encoded)
        {
            if (string.IsNullOrWhiteSpace(encoded)) return Array.Empty<byte>();
            int bitBuffer = 0, bitCount = 0;
            List<byte> output = new();

            foreach (char c in encoded.ToUpperInvariant())
            {
                if (c == '=') continue;
                int value = Alphabet.IndexOf(c);
                if (value < 0) throw new FormatException("Invalid Base32 character");

                bitBuffer = (bitBuffer << 5) | value;
                bitCount += 5;
                if (bitCount >= 8)
                {
                    output.Add((byte)(bitBuffer >> (bitCount - 8)));
                    bitCount -= 8;
                }
            }
            return output.ToArray();
        }
    }
}
