using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Briefcase.Domain.Interfaces;

namespace Briefcase.Infrastructure.Security;

public class AesEncryptionService : IEncryptionService
{
    private const string StringPrefix = "ENC:v1:";
    private static readonly byte[] StreamMagicHeader = "ENC1"u8.ToArray(); // 4 bytes
    private const int ChunkSizeBytes = 64 * 1024; // 64 KB plaintext chunks
    private const int NonceSizeBytes = 12; // 96-bit nonce for AES-GCM
    private const int TagSizeBytes = 16;   // 128-bit tag for AES-GCM

    private readonly byte[]? _key;

    public bool IsEnabled => _key is not null && _key.Length == 32;

    public AesEncryptionService(IConfiguration configuration)
    {
        var keyString = configuration["Encryption:Key"];
        if (!string.IsNullOrWhiteSpace(keyString))
        {
            try
            {
                var bytes = Convert.FromBase64String(keyString.Trim());
                if (bytes.Length == 32)
                {
                    _key = bytes;
                }
            }
            catch
            {
                // Key is invalid Base64 or wrong size — leave disabled
            }
        }
    }

    public AesEncryptionService(byte[] key)
    {
        if (key.Length == 32)
        {
            _key = key;
        }
    }

    public string? Encrypt(string? plainText)
    {
        if (string.IsNullOrEmpty(plainText) || !IsEnabled)
            return plainText;

        try
        {
            var plainBytes = Encoding.UTF8.GetBytes(plainText);
            var nonce = RandomNumberGenerator.GetBytes(NonceSizeBytes);
            var tag = new byte[TagSizeBytes];
            var cipherBytes = new byte[plainBytes.Length];

            using var aes = new AesGcm(_key!, TagSizeBytes);
            aes.Encrypt(nonce, plainBytes, cipherBytes, tag);

            // Format: Nonce (12B) + Tag (16B) + Ciphertext (N B)
            var combined = new byte[NonceSizeBytes + TagSizeBytes + cipherBytes.Length];
            Buffer.BlockCopy(nonce, 0, combined, 0, NonceSizeBytes);
            Buffer.BlockCopy(tag, 0, combined, NonceSizeBytes, TagSizeBytes);
            Buffer.BlockCopy(cipherBytes, 0, combined, NonceSizeBytes + TagSizeBytes, cipherBytes.Length);

            return StringPrefix + Convert.ToBase64String(combined);
        }
        catch
        {
            return plainText;
        }
    }

    public string? Decrypt(string? cipherText)
    {
        if (string.IsNullOrEmpty(cipherText) || !IsEnabled)
            return cipherText;

        if (!cipherText.StartsWith(StringPrefix, StringComparison.Ordinal))
            return cipherText; // Return as-is for legacy plaintext

        try
        {
            var payloadB64 = cipherText[StringPrefix.Length..];
            var combined = Convert.FromBase64String(payloadB64);

            if (combined.Length < NonceSizeBytes + TagSizeBytes)
                return cipherText;

            var nonce = combined[..NonceSizeBytes];
            var tag = combined[NonceSizeBytes..(NonceSizeBytes + TagSizeBytes)];
            var cipherBytes = combined[(NonceSizeBytes + TagSizeBytes)..];
            var plainBytes = new byte[cipherBytes.Length];

            using var aes = new AesGcm(_key!, TagSizeBytes);
            aes.Decrypt(nonce, cipherBytes, tag, plainBytes);

            return Encoding.UTF8.GetString(plainBytes);
        }
        catch
        {
            return cipherText; // Fallback to raw text if decryption fails
        }
    }

    public async Task EncryptStreamAsync(Stream inputStream, Stream outputStream, CancellationToken cancellationToken = default)
    {
        if (!IsEnabled)
        {
            await inputStream.CopyToAsync(outputStream, cancellationToken);
            return;
        }

        // Write Magic Header
        await outputStream.WriteAsync(StreamMagicHeader, cancellationToken);

        using var aes = new AesGcm(_key!, TagSizeBytes);
        var buffer = new byte[ChunkSizeBytes];
        int bytesRead;

        while ((bytesRead = await ReadExactOrAvailableAsync(inputStream, buffer, cancellationToken)) > 0)
        {
            var plainChunk = buffer.AsSpan(0, bytesRead);
            var nonce = RandomNumberGenerator.GetBytes(NonceSizeBytes);
            var tag = new byte[TagSizeBytes];
            var cipherChunk = new byte[bytesRead];

            aes.Encrypt(nonce, plainChunk, cipherChunk, tag);

            // Chunk format: [Int32 Payload Length] [12B Nonce] [16B Tag] [N B Ciphertext]
            var payloadLen = NonceSizeBytes + TagSizeBytes + bytesRead;
            await outputStream.WriteAsync(BitConverter.GetBytes(payloadLen), cancellationToken);
            await outputStream.WriteAsync(nonce, cancellationToken);
            await outputStream.WriteAsync(tag, cancellationToken);
            await outputStream.WriteAsync(cipherChunk, cancellationToken);
        }
    }

    public async Task DecryptStreamAsync(Stream inputStream, Stream outputStream, CancellationToken cancellationToken = default)
    {
        if (!IsEnabled)
        {
            await inputStream.CopyToAsync(outputStream, cancellationToken);
            return;
        }

        // Read magic header
        var header = new byte[StreamMagicHeader.Length];
        var headerBytesRead = await ReadExactOrAvailableAsync(inputStream, header, cancellationToken);

        if (headerBytesRead < StreamMagicHeader.Length || !header.SequenceEqual(StreamMagicHeader))
        {
            // Not encrypted or corrupted — pass through header bytes and copy rest
            if (headerBytesRead > 0)
            {
                await outputStream.WriteAsync(header.AsMemory(0, headerBytesRead), cancellationToken);
            }
            await inputStream.CopyToAsync(outputStream, cancellationToken);
            return;
        }

        using var aes = new AesGcm(_key!, TagSizeBytes);
        var intBuffer = new byte[4];

        while (await ReadExactAsync(inputStream, intBuffer, cancellationToken))
        {
            var payloadLen = BitConverter.ToInt32(intBuffer, 0);
            if (payloadLen < NonceSizeBytes + TagSizeBytes)
                break;

            var payload = new byte[payloadLen];
            if (!await ReadExactAsync(inputStream, payload, cancellationToken))
                break;

            var nonce = payload.AsSpan(0, NonceSizeBytes);
            var tag = payload.AsSpan(NonceSizeBytes, TagSizeBytes);
            var cipherChunk = payload.AsSpan(NonceSizeBytes + TagSizeBytes);
            var plainChunk = new byte[cipherChunk.Length];

            aes.Decrypt(nonce, cipherChunk, tag, plainChunk);
            await outputStream.WriteAsync(plainChunk, cancellationToken);
        }
    }

    private static async Task<int> ReadExactOrAvailableAsync(Stream stream, byte[] buffer, CancellationToken cancellationToken)
    {
        var totalRead = 0;
        while (totalRead < buffer.Length)
        {
            var read = await stream.ReadAsync(buffer.AsMemory(totalRead, buffer.Length - totalRead), cancellationToken);
            if (read == 0) break;
            totalRead += read;
        }
        return totalRead;
    }

    private static async Task<bool> ReadExactAsync(Stream stream, byte[] buffer, CancellationToken cancellationToken)
    {
        var read = await ReadExactOrAvailableAsync(stream, buffer, cancellationToken);
        return read == buffer.Length;
    }
}
