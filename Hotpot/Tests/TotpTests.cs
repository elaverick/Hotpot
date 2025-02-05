using System;
using Xunit;
using Hotpot;
using System.Security.Cryptography;

namespace Hotpot.Tests
{
    public class TotpTests
    {
        private readonly byte[] secretKey = new byte[20]; // 160-bit key

        public TotpTests()
        {
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(secretKey);
        }

        [Theory]
        [InlineData("SHA1")]
        [InlineData("SHA256")]
        [InlineData("SHA512")]
        public void GenerateOtp_ShouldGenerateValidOtp_WithDifferentAlgorithms(string algorithm)
        {
            var totp = new Totp(secretKey, algorithm);
            var otp = totp.GenerateOtp(DateTime.UtcNow);
            Assert.NotNull(otp);
            Assert.Equal(6, otp.Length);
        }

        [Theory]
        [InlineData(30)]
        [InlineData(60)]
        public void GenerateOtp_ShouldGenerateValidOtp_WithDifferentTimeSteps(int timeStep)
        {
            var totp = new Totp(secretKey, "SHA1", 6, timeStep);
            var otp = totp.GenerateOtp(DateTime.UtcNow);
            Assert.NotNull(otp);
            Assert.Equal(6, otp.Length);
        }

        [Fact]
        public void Constructor_ShouldThrowException_OnInvalidDigits()
        {
            Assert.Throws<ArgumentException>(() => new Totp(secretKey, "SHA1", 5));
        }

        [Fact]
        public void Constructor_ShouldThrowException_OnInvalidTimeStep()
        {
            Assert.Throws<ArgumentException>(() => new Totp(secretKey, "SHA1", 6, 45));
        }

        [Fact]
        public void Constructor_ShouldThrowException_OnInvalidAlgorithm()
        {
            Assert.Throws<ArgumentException>(() => new Totp(secretKey, "MD5"));
        }

        [Fact]
        public void GenerateOtp_ShouldProduceSameOtp_ForSameTimestamp()
        {
            var totp = new Totp(secretKey);
            DateTime timestamp = DateTime.UtcNow;
            string otp1 = totp.GenerateOtp(timestamp);
            string otp2 = totp.GenerateOtp(timestamp);
            Assert.Equal(otp1, otp2);
        }
    }
}
