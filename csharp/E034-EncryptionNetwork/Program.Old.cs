/*
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

_ = Task.Run(ServerWorker);
_ = Task.Run(ClientWorker);

await Task.Delay(-1);
return;

static async Task ClientWorker()
{
    using var socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
    await socket.ConnectAsync(new IPEndPoint(IPAddress.Loopback, 1234));

    using var aes = Aes.Create();
    aes.GenerateKey();
    aes.GenerateIV();

    await using var stream = new NetworkStream(socket);
    await using var crypto = new CryptoStream(stream, aes.CreateEncryptor(), CryptoStreamMode.Write, true);

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
        const string message = "Hello, world!";

        // send encrypted message
        Console.WriteLine($"Client sending message: {message}");
        writer.Write(message);
    }

    await crypto.FlushFinalBlockAsync();
    await Task.Delay(-1);
}

static async Task ServerWorker()
{
    using var listener = new Socket(SocketType.Stream, ProtocolType.Tcp);
    listener.Bind(new IPEndPoint(IPAddress.Any, 1234));
    listener.Listen(1);

    using Socket client = await listener.AcceptAsync();
    await using var stream = new NetworkStream(client);
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

    await using var crypto = new CryptoStream(stream, aes.CreateDecryptor(), CryptoStreamMode.Read, true);
    using (var reader = new BinaryReader(crypto, Encoding.UTF8, true))
    {
        Console.Write("Server received message: ");
        Console.WriteLine(reader.ReadString());
    }

    await Task.Delay(-1);
}
*/
