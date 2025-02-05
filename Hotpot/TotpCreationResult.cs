using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotpot
{
    /// <summary>
    /// Represents the result of creating a TOTP instance, including the generated secret key.
    /// </summary>
    public class TotpCreationResult
    {
        /// <summary>
        /// Gets the <see cref="Totp"/> instance that was created.
        /// </summary>
        public Totp TotpInstance { get; }

        /// <summary>
        /// Gets the secret key associated with the TOTP instance.
        /// This key should be securely stored by the application.
        /// </summary>
        public byte[] SecretKey { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TotpCreationResult"/> class.
        /// </summary>
        /// <param name="totp">The generated <see cref="Totp"/> instance.</param>
        /// <param name="secretKey">The secret key used for TOTP generation.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="totp"/> or <paramref name="secretKey"/> is null.</exception>
        public TotpCreationResult(Totp totp, byte[] secretKey)
        {
            TotpInstance = totp ?? throw new ArgumentNullException(nameof(totp));
            SecretKey = secretKey ?? throw new ArgumentNullException(nameof(secretKey));
        }

        /// <summary>
        /// Allows implicit conversion of <see cref="TotpCreationResult"/> to <see cref="Totp"/>.
        /// This makes it easier to use the result directly as a <see cref="Totp"/> instance when needed.
        /// </summary>
        /// <param name="result">The <see cref="TotpCreationResult"/> instance to convert.</param>
        public static implicit operator Totp(TotpCreationResult result)
        {
            return result.TotpInstance;
        }
    }
}
