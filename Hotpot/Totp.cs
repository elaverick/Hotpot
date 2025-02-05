using System;
using System.Security.Cryptography;

namespace Hotpot
{
    /// <summary>
    /// Generates Time-based One-Time Passwords (TOTP) as per RFC 6238.
    /// </summary>
    public class Totp
    {
        private readonly byte[] _secretKey;
        private readonly int _digits;
        private readonly int _timeStep;
        private readonly HMAC _hmac;

        /// <summary>
        /// Initializes a new instance of the <see cref="Totp"/> class.
        /// </summary>
        /// <param name="secretKey">The shared secret key.</param>
        /// <param name="algorithm">The HMAC algorithm to use (SHA1, SHA256, SHA512).</param>
        /// <param name="digits">The number of digits in the generated OTP (6 or 8).</param>
        /// <param name="timeStep">The time step in seconds (30 or 60).</param>
        public Totp(byte[] secretKey, string algorithm = "SHA1", int digits = 6, int timeStep = 30)
        {
            _secretKey = secretKey ?? throw new ArgumentNullException(nameof(secretKey));
            _digits = (digits == 6 || digits == 8) ? digits : throw new ArgumentException("Digits must be 6 or 8");
            _timeStep = (timeStep == 30 || timeStep == 60) ? timeStep : throw new ArgumentException("Time step must be 30 or 60 seconds");
            _hmac = algorithm.ToUpper() switch
            {
                "SHA1" => new HMACSHA1(_secretKey),
                "SHA256" => new HMACSHA256(_secretKey),
                "SHA512" => new HMACSHA512(_secretKey),
                _ => throw new ArgumentException("Invalid algorithm specified.")
            };
        }

        /// <summary>
        /// Generates a one-time password based on the given timestamp.
        /// </summary>
        /// <param name="timestamp">The timestamp for which to generate the OTP.</param>
        /// <returns>The generated OTP as a string.</returns>
        public string GenerateOtp(DateTime timestamp)
        {
            long unixTime = new DateTimeOffset(timestamp).ToUnixTimeSeconds();
            long counter = unixTime / _timeStep;
            return GenerateOtpFromCounter(counter);
        }

        /// <summary>
        /// Generates an OTP based on a counter value.
        /// </summary>
        /// <param name="counter">The counter derived from the timestamp.</param>
        /// <returns>The generated OTP as a string.</returns>
        private string GenerateOtpFromCounter(long counter)
        {
            byte[] counterBytes = BitConverter.GetBytes(counter);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(counterBytes);

            byte[] hash = _hmac.ComputeHash(counterBytes);
            int offset = hash[^1] & 0x0F;
            int binaryCode =
                ((hash[offset] & 0x7F) << 24) |
                ((hash[offset + 1] & 0xFF) << 16) |
                ((hash[offset + 2] & 0xFF) << 8) |
                (hash[offset + 3] & 0xFF);

            int otp = binaryCode % (int)Math.Pow(10, _digits);
            return otp.ToString().PadLeft(_digits, '0');
        }
    }
}
