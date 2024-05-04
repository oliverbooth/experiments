using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

new Thread(ServerWorker).Start();
new Thread(ClientWorker).Start();
return;

static void ClientWorker()
{
    using var socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
    socket.Connect(new IPEndPoint(IPAddress.Loopback, 1234));

    using var stream = new NetworkStream(socket);

    // read public key from server
    using var reader = new BinaryReader(stream);
    using var writer = new BinaryWriter(stream);
    using var rsa = new RSACryptoServiceProvider(2048);
    rsa.ImportParameters(new RSAParameters
    {
        Modulus = reader.ReadBytes(reader.ReadInt32()),
        Exponent = reader.ReadBytes(reader.ReadInt32())
    });

    while (Console.ReadLine() is { } line)
    {
        // encrypt line and send to server
        byte[] encrypted = rsa.Encrypt(Encoding.UTF8.GetBytes(line), RSAEncryptionPadding.OaepSHA1);
        writer.Write(encrypted.Length);
        writer.Write(encrypted);
    }
}

static void ServerWorker()
{
    using var socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
    socket.Bind(new IPEndPoint(IPAddress.Any, 1234));
    socket.Listen(1);

    // generate RSA key pair
    using var rsa = new RSACryptoServiceProvider(2048);
    RSAParameters publicKey = rsa.ExportParameters(false);

    // accept client socket
    using Socket client = socket.Accept();
    using var stream = new NetworkStream(client);

    // write public key
    using var writer = new BinaryWriter(stream);
    writer.Write(publicKey.Modulus!.Length);
    writer.Write(publicKey.Modulus);
    writer.Write(publicKey.Exponent!.Length);
    writer.Write(publicKey.Exponent);

    while (true)
    {
        // read encrypted line from client
        using var reader = new BinaryReader(new NetworkStream(client));
        int length = reader.ReadInt32();
        byte[] encrypted = reader.ReadBytes(length);
        byte[] decrypted = rsa.Decrypt(encrypted, RSAEncryptionPadding.OaepSHA1);
        Console.WriteLine($"Client sent: {Encoding.UTF8.GetString(decrypted)}");
    }
}
