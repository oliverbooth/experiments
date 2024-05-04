using System.Security.Cryptography;
using System.Text;

await using Stream stream = await EncryptMessageAsync();
await DecryptMessageAsync(stream);

await Task.Delay(-1);
return;

static async Task<Stream> EncryptMessageAsync()
{
    using var aes = Aes.Create();
    aes.GenerateKey();
    aes.GenerateIV();

    using ICryptoTransform encryptor = aes.CreateEncryptor();
    var stream = new MemoryStream();
    await using var crypto = new CryptoStream(stream, encryptor, CryptoStreamMode.Write, true);

    await using (var writer = new BinaryWriter(stream, Encoding.UTF8, true))
    {
        Console.WriteLine($"Client sending key: {BitConverter.ToString(aes.Key)}");
        Console.WriteLine($"Client sending IV: {BitConverter.ToString(aes.IV)}");

        // send key and iv unencrypted
        writer.Write(aes.Key.Length);
        writer.Write(aes.Key);
        writer.Write(aes.IV.Length);
        writer.Write(aes.IV);
    }

    await using (var writer = new BinaryWriter(crypto, Encoding.UTF8, true))
    {
        // send encrypted message
        byte[] message = "Hello, world!"u8.ToArray();

        writer.Write(message.Length);
        Console.WriteLine($"Writing {message.Length} bytes");
        writer.Write(message);
    }

    await crypto.FlushFinalBlockAsync();

    stream.Position = 0;
    return stream;
}

static async Task DecryptMessageAsync(Stream stream)
{
    using var aes = Aes.Create();

    using (var reader = new BinaryReader(stream, Encoding.UTF8, true))
    {
        int keyLength = reader.ReadInt32();
        aes.Key = reader.ReadBytes(keyLength);

        int ivLength = reader.ReadInt32();
        aes.IV = reader.ReadBytes(ivLength);

        Console.WriteLine($"Server received key: {BitConverter.ToString(aes.Key)}");
        Console.WriteLine($"Server received IV: {BitConverter.ToString(aes.IV)}");
    }

    using ICryptoTransform decryptor = aes.CreateDecryptor();
    await using var crypto = new CryptoStream(stream, decryptor, CryptoStreamMode.Read, true);
    using (var reader = new BinaryReader(crypto, Encoding.UTF8, true))
    {
        int length = reader.ReadInt32();
        Console.WriteLine($"Reading {length} bytes");

        byte[] message = reader.ReadBytes(length);
        Console.WriteLine($"Server received message: {Encoding.UTF8.GetString(message)}");
    }
}
