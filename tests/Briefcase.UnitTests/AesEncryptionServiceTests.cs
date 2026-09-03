using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Briefcase.Infrastructure.Security;

namespace Briefcase.UnitTests;

[TestClass]
public class AesEncryptionServiceTests
{
    private static readonly byte[] TestKey = RandomNumberGenerator.GetBytes(32);
    private static readonly string TestKeyB64 = Convert.ToBase64String(TestKey);

    [TestMethod]
    public void IsEnabled_WithValidKey_ReturnsTrue()
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?> { ["Encryption:Key"] = TestKeyB64 })
            .Build();

        var service = new AesEncryptionService(config);

        Assert.IsTrue(service.IsEnabled);
    }

    [TestMethod]
    public void IsEnabled_WithoutKey_ReturnsFalse()
    {
        var config = new ConfigurationBuilder().Build();

        var service = new AesEncryptionService(config);

        Assert.IsFalse(service.IsEnabled);
    }

    [TestMethod]
    public void StringEncryption_Roundtrip_ReturnsOriginalText()
    {
        var service = new AesEncryptionService(TestKey);
        const string plainText = "Hello, secret world! 12345 🗺️";

        var encrypted = service.Encrypt(plainText);
        Assert.AreNotEqual(plainText, encrypted);
        Assert.IsTrue(encrypted!.StartsWith("ENC:v1:"));

        var decrypted = service.Decrypt(encrypted);
        Assert.AreEqual(plainText, decrypted);
    }

    [TestMethod]
    public void StringDecrypt_LegacyPlaintext_ReturnsInputUnchanged()
    {
        var service = new AesEncryptionService(TestKey);
        const string legacyText = "This is unencrypted legacy text.";

        var decrypted = service.Decrypt(legacyText);

        Assert.AreEqual(legacyText, decrypted);
    }

    [TestMethod]
    public async Task StreamEncryption_Roundtrip_ReturnsOriginalBytes()
    {
        var service = new AesEncryptionService(TestKey);
        var originalData = Encoding.UTF8.GetBytes("File content to be encrypted in chunked stream format.");

        using var inputStream = new MemoryStream(originalData);
        using var encryptedStream = new MemoryStream();

        await service.EncryptStreamAsync(inputStream, encryptedStream);

        encryptedStream.Position = 0;
        using var decryptedStream = new MemoryStream();
        await service.DecryptStreamAsync(encryptedStream, decryptedStream);

        var decryptedData = decryptedStream.ToArray();
        CollectionAssert.AreEqual(originalData, decryptedData);
    }

    [TestMethod]
    public async Task StreamDecrypt_LegacyPlainStream_ReturnsInputUnchanged()
    {
        var service = new AesEncryptionService(TestKey);
        var legacyData = Encoding.UTF8.GetBytes("Unencrypted legacy file stream data.");

        using var inputStream = new MemoryStream(legacyData);
        using var outputStream = new MemoryStream();

        await service.DecryptStreamAsync(inputStream, outputStream);

        var resultData = outputStream.ToArray();
        CollectionAssert.AreEqual(legacyData, resultData);
    }
}
