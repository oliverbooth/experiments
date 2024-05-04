using System.IO.Compression;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

await Task.WhenAll(ServerTask(), ClientTask());
await Task.Delay(-1);
return;

static async Task ClientTask()
{
    using var socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
    await socket.ConnectAsync(new IPEndPoint(IPAddress.Loopback, 1234));

    using var aes = Aes.Create();
    aes.GenerateKey();
    aes.GenerateIV();

    await using var stream = new NetworkStream(socket);

    SendAesKey(stream, aes);

    const string message = "Hello";
    Console.WriteLine($"Client sending message: {message}");
    SendEncryptedMessage(Encoding.UTF8.GetBytes(message), stream, aes);

    await Task.Delay(-1);
}

static async Task ServerTask()
{
    using var listener = new Socket(SocketType.Stream, ProtocolType.Tcp);
    listener.Bind(new IPEndPoint(IPAddress.Any, 1234));
    listener.Listen(1);

    using Socket client = await listener.AcceptAsync();
    await using var stream = new NetworkStream(client);
    using var aes = Aes.Create();

    ReadAesKey(stream, aes);
    Console.WriteLine($"Server received message: {Encoding.UTF8.GetString(ReadEncryptedMessage(stream, aes))}");

    await Task.Delay(-1);
}

static void ReadAesKey(Stream stream, SymmetricAlgorithm aes)
{
    using var reader = new BinaryReader(stream, Encoding.UTF8, true);
    aes.Key = reader.ReadBytes(IPAddress.NetworkToHostOrder(reader.ReadInt32()));
    aes.IV = reader.ReadBytes(IPAddress.NetworkToHostOrder(reader.ReadInt32()));
}

static void SendAesKey(Stream stream, SymmetricAlgorithm aes)
{
    using var writer = new BinaryWriter(stream, Encoding.UTF8, true);
    writer.Write(IPAddress.HostToNetworkOrder(aes.Key.Length));
    writer.Write(aes.Key);
    writer.Write(IPAddress.HostToNetworkOrder(aes.IV.Length));
    writer.Write(aes.IV);
}

static byte[] ReadEncryptedMessage(Stream inputStream, SymmetricAlgorithm aes)
{
    using var inputReader = new BinaryReader(inputStream, Encoding.UTF8, true);
    using ICryptoTransform cryptoTransform = aes.CreateDecryptor();

    // read unencrypted length header
    using var buffer = new MemoryStream();
    int length = IPAddress.NetworkToHostOrder(inputReader.ReadInt32());
    Console.WriteLine($"Reading {length} bytes");
    buffer.Write(inputReader.ReadBytes(length));

    // read encrypted data
    Console.WriteLine($"Read {buffer.Length} bytes");
    Console.WriteLine("Decrypting...");
    buffer.Position = 0;
    using var cryptoStream = new CryptoStream(buffer, cryptoTransform, CryptoStreamMode.Read);
    using var gzip = new GZipStream(cryptoStream, CompressionMode.Decompress);
    using var cryptoReader = new BinaryReader(gzip, Encoding.UTF8);
    length = IPAddress.NetworkToHostOrder(cryptoReader.ReadInt32());
    Console.WriteLine($"Need to read {length} bytes");
    return cryptoReader.ReadBytes(length);
}

 static void SendEncryptedMessage(byte[] message, Stream outputStream, SymmetricAlgorithm aes)
{
    using var outputWriter = new BinaryWriter(outputStream, Encoding.UTF8, true);
    using ICryptoTransform cryptoTransform = aes.CreateEncryptor();

    // encrypt data
    using var buffer = new MemoryStream();
    using var cryptoStream = new CryptoStream(buffer, cryptoTransform, CryptoStreamMode.Write);
    using var gzip = new GZipStream(cryptoStream, CompressionMode.Compress);
    using var cryptoWriter = new BinaryWriter(gzip, Encoding.UTF8);
    cryptoWriter.Write(IPAddress.HostToNetworkOrder(message.Length));
    cryptoWriter.Write(message);
    cryptoWriter.Flush();
    cryptoStream.FlushFinalBlock();

    buffer.Position = 0;

    // sent encrypted data with unencrypted length header
    Console.WriteLine($"Writing {buffer.Length} bytes");
    outputWriter.Write(IPAddress.HostToNetworkOrder((int)BitOperations.RoundUpToPowerOf2((uint)buffer.Length)));
    buffer.CopyTo(outputStream);
    outputStream.Flush();
    Console.WriteLine("Message sent");
}
