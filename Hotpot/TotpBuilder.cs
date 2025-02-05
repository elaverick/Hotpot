namespace TotpAuthLib;

using System;
using System.Security.Cryptography;
using QRCoder;
using System.Text;
using Hotpot;

/// <summary>
/// Builder class for constructing a <see cref="Totp"/> instance in a fluent and easy-to-use manner.
/// </summary>
public class TotpBuilder
{
    private byte[] _secretKey;
    private string _algorithm = "SHA1";
    private int _digits = 6;
    private int _timeStep = 30;
    private int _keyLength = 160;

    /// <summary>
    /// Sets the secret key for the TOTP instance.
    /// </summary>
    /// <param name="secretKey">The shared secret key in byte array format.</param>
    /// <returns>The current instance of <see cref="TotpBuilder"/>.</returns>
    public TotpBuilder WithSecretKey(byte[] secretKey)
    {
        _secretKey = secretKey ?? throw new ArgumentNullException(nameof(secretKey));
        return this;
    }

    /// <summary>
    /// Specifies that a new random secret key should be generated using the given key length (in bits).
    /// </summary>
    /// <param name="keyLength">The length of the key in bits (commonly 160 or 256).</param>
    /// <returns>The current instance of <see cref="TotpBuilder"/>.</returns>
    public TotpBuilder GenerateSecretKey(int keyLength = 160)
    {
        _keyLength = keyLength;
        using var rng = RandomNumberGenerator.Create();
        int keyBytes = keyLength / 8;
        _secretKey = new byte[keyBytes];
        rng.GetBytes(_secretKey);
        return this;
    }

    /// <summary>
    /// Sets the HMAC algorithm used for generating TOTP values.
    /// </summary>
    /// <param name="algorithm">The hashing algorithm (SHA1, SHA256, SHA512).</param>
    /// <returns>The current instance of <see cref="TotpBuilder"/>.</returns>
    public TotpBuilder WithAlgorithm(string algorithm)
    {
        if (string.IsNullOrWhiteSpace(algorithm))
            throw new ArgumentException("Algorithm cannot be null or empty.", nameof(algorithm));
        _algorithm = algorithm;
        return this;
    }

    /// <summary>
    /// Sets the number of digits in the generated OTP.
    /// </summary>
    /// <param name="digits">Must be either 6 or 8.</param>
    /// <returns>The current instance of <see cref="TotpBuilder"/>.</returns>
    public TotpBuilder WithDigits(int digits)
    {
        if (digits != 6 && digits != 8)
            throw new ArgumentException("Digits must be 6 or 8.");
        _digits = digits;
        return this;
    }

    /// <summary>
    /// Sets the time step interval for OTP validity.
    /// </summary>
    /// <param name="timeStep">Must be either 30 or 60 seconds.</param>
    /// <returns>The current instance of <see cref="TotpBuilder"/>.</returns>
    public TotpBuilder WithTimeStep(int timeStep)
    {
        if (timeStep != 30 && timeStep != 60)
            throw new ArgumentException("Time step must be 30 or 60 seconds.");
        _timeStep = timeStep;
        return this;
    }

    /// <summary>
    /// Builds and returns a new <see cref="TotpCreationResult"/> that contains the configured Totp instance and the secret key.
    /// </summary>
    /// <returns>A <see cref="TotpCreationResult"/> with the Totp instance and the secret key used.</returns>
    public TotpCreationResult Build()
    {
        // If the secret key hasn't been provided, generate one automatically.
        if (_secretKey == null || _secretKey.Length == 0)
        {
            GenerateSecretKey(_keyLength);
        }
        // Create the Totp instance with the configured settings.
        Totp totp = new Totp(_secretKey, _algorithm, _digits, _timeStep);
        return new TotpCreationResult(totp, _secretKey);
    }
}
