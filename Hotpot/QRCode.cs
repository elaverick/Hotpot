using QRCoder;

namespace Hotpot
{
    public class QRCode
    {
        /// <summary>
        /// Generates an ASCII representation of a QR code for the OTP URI.
        /// </summary>
        /// <param name="secret">The shared secret key for the OTP.</param>
        /// <param name="issuer">The issuer of the OTP (e.g., company or service name).</param>
        /// <param name="account">The account name associated with the OTP.</param>
        /// <param name="algorithm">The hashing algorithm (SHA1, SHA256, SHA512).</param>
        /// <param name="digits">The number of digits in the OTP.</param>
        /// <param name="period">The time period in seconds for OTP validity.</param>
        /// <returns>A string containing the ASCII representation of the QR code.</returns>
        public static string GenerateAscii(string secret, string issuer, string account, string algorithm = "SHA1", int digits = 6, int period = 30)
        {
            if(String.IsNullOrEmpty(secret))
                throw new ArgumentException(nameof(secret));
            if(String.IsNullOrEmpty(issuer))
                throw new ArgumentException(nameof(issuer));
            if(String.IsNullOrEmpty(account))
                throw new ArgumentException(nameof(account));

            using var qrGenerator = new QRCodeGenerator();
            string otpUrl = $"otpauth://totp/{account}?secret={secret}&issuer={issuer}&algorithm={algorithm}&digits={digits}&period={period}";
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(otpUrl, QRCodeGenerator.ECCLevel.M);
            AsciiQRCode qrCode = new(qrCodeData);
            return qrCode.GetGraphicSmall(false, true);
        }

        /// <summary>
        /// Generates an ASCII representation of a QR code for the OTP URI.
        /// </summary>
        /// <param name="secret">The shared secret key for the OTP.</param>
        /// <param name="issuer">The issuer of the OTP (e.g., company or service name).</param>
        /// <param name="account">The account name associated with the OTP.</param>
        /// <param name="algorithm">The hashing algorithm (SHA1, SHA256, SHA512).</param>
        /// <param name="digits">The number of digits in the OTP.</param>
        /// <param name="period">The time period in seconds for OTP validity.</param>
        /// <returns>A string containing the ASCII representation of the QR code.</returns>
        public static string GenerateAscii(byte[] secret, string issuer, string account, string algorithm = "SHA1", int digits = 6, int period = 30)
        {
            string encoded = Base32.Encode(secret);
            return GenerateAscii(encoded, issuer, account, algorithm, digits, period);
        }

        /// <summary>
        /// Generates a PNG representation of a QR code for the OTP URI.
        /// </summary>
        /// <param name="secret">The shared secret key for the OTP.</param>
        /// <param name="issuer">The issuer of the OTP (e.g., company or service name).</param>
        /// <param name="account">The account name associated with the OTP.</param>
        /// <param name="algorithm">The hashing algorithm (SHA1, SHA256, SHA512).</param>
        /// <param name="digits">The number of digits in the OTP.</param>
        /// <param name="period">The time period in seconds for OTP validity.</param>
        /// <returns>A byte array containing the PNG representation of the QR code.</returns>
        public static byte[] GeneratePng(string secret, string issuer, string account, string algorithm = "SHA1", int digits = 6, int period = 30)
        {
            if (String.IsNullOrEmpty(secret))
                throw new ArgumentException(nameof(secret));
            if (String.IsNullOrEmpty(issuer))
                throw new ArgumentException(nameof(issuer));
            if (String.IsNullOrEmpty(account))
                throw new ArgumentException(nameof(account));

            using var qrGenerator = new QRCodeGenerator();
            string otpUrl = $"otpauth://totp/{account}?secret={secret}&issuer={issuer}&algorithm={algorithm}&digits={digits}&period={period}";
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(otpUrl, QRCodeGenerator.ECCLevel.M);
            using var qrCode = new PngByteQRCode(qrCodeData);
            return qrCode.GetGraphic(20);
        }

        /// <summary>
        /// Generates a PNG representation of a QR code for the OTP URI.
        /// </summary>
        /// <param name="secret">The shared secret key for the OTP.</param>
        /// <param name="issuer">The issuer of the OTP (e.g., company or service name).</param>
        /// <param name="account">The account name associated with the OTP.</param>
        /// <param name="algorithm">The hashing algorithm (SHA1, SHA256, SHA512).</param>
        /// <param name="digits">The number of digits in the OTP.</param>
        /// <param name="period">The time period in seconds for OTP validity.</param>
        /// <returns>A byte array containing the PNG representation of the QR code.</returns>
        public static byte[] GeneratePng(byte[] secret, string issuer, string account, string algorithm = "SHA1", int digits = 6, int period = 30)
        {
            string encoded = Base32.Encode(secret);
            return GeneratePng(encoded, issuer, account, algorithm, digits, period);
        }
    }
}
