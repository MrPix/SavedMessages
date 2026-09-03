namespace Briefcase.Domain.Interfaces;

public interface IEncryptionService
{
    /// <summary>
    /// Returns true if server-side encryption key is configured and valid.
    /// </summary>
    bool IsEnabled { get; }

    /// <summary>
    /// Encrypts plain text string into a Base64-encoded payload (12-byte IV + ciphertext + 16-byte GCM tag).
    /// Returns input as-is if disabled or null.
    /// </summary>
    string? Encrypt(string? plainText);

    /// <summary>
    /// Decrypts a Base64-encoded payload into plain text string.
    /// Returns input as-is if not valid ciphertext or if disabled.
    /// </summary>
    string? Decrypt(string? cipherText);

    /// <summary>
    /// Encrypts a stream into a chunked AES-GCM stream.
    /// </summary>
    Task EncryptStreamAsync(Stream inputStream, Stream outputStream, CancellationToken cancellationToken = default);

    /// <summary>
    /// Decrypts a chunked AES-GCM stream into a plaintext stream.
    /// </summary>
    Task DecryptStreamAsync(Stream inputStream, Stream outputStream, CancellationToken cancellationToken = default);
}
