using System;
using System.Security.Cryptography;
using QRCoder;
using System.Text;
using Xunit;


namespace Hotpot.Tests
{
    /// <summary>
    /// Unit tests for the QR code generation functionality.
    /// </summary>
    public class QrCodeTests
    {
        private const string Secret = "JBSWY3DPEHPK3PXP";
        private const string Issuer = "ExampleIssuer";
        private const string Account = "TestAccount";

        /// <summary>
        /// Tests that the ASCII QR code generation returns a non-empty string.
        /// </summary>
        [Fact]
        public void GenerateAscii_ShouldReturnNonEmptyString()
        {
            string asciiQrCode = QRCode.GenerateAscii(Secret, Issuer, Account);
            Assert.False(string.IsNullOrWhiteSpace(asciiQrCode), "ASCII QR Code should not be empty.");
        }

        /// <summary>
        /// Tests that the PNG QR code generation returns a non-empty byte array.
        /// </summary>
        [Fact]
        public void GeneratePng_ShouldReturnNonEmptyByteArray()
        {
            byte[] pngQrCode = QRCode.GeneratePng(Secret, Issuer, Account);
            Assert.NotNull(pngQrCode);
            Assert.True(pngQrCode.Length > 0, "PNG QR Code should not be empty.");
        }

        /// <summary>
        /// Tests that an empty secret throws an exception when generating an ASCII QR code.
        /// </summary>
        [Fact]
        public void GenerateAscii_WithEmptySecret_ShouldThrowException()
        {
            Assert.Throws<ArgumentException>(() => QRCode.GenerateAscii(string.Empty, Issuer, Account));
        }

        /// <summary>
        /// Tests that an empty secret throws an exception when generating a PNG QR code.
        /// </summary>
        [Fact]
        public void GeneratePng_WithEmptySecret_ShouldThrowException()
        {
            Assert.Throws<ArgumentException>(() => QRCode.GeneratePng(string.Empty, Issuer, Account));
        }
    }
}
