using System.Net;
using System.Net.Sockets;
using System.Text;

_ = Task.Run(ClientWorker);
_ = Task.Run(ServerWorker);

await Task.Delay(-1); // keep app open
return;

static async Task ClientWorker()
{
    using var client = new Socket(SocketType.Dgram, ProtocolType.Udp);
    EndPoint endpoint = new IPEndPoint(IPAddress.Loopback, 1234);

    var message = new byte[11];
    Encoding.ASCII.GetBytes("Hello World", message);

    Console.WriteLine("[CLIENT] Sending message to server");
    await client.SendToAsync(message, SocketFlags.None, endpoint);

    Console.WriteLine("[CLIENT] Waiting for response");
    message = new byte[11];
    await client.ReceiveFromAsync(message, SocketFlags.None, endpoint);

    Console.WriteLine($"[CLIENT] Received response: \"{Encoding.ASCII.GetString(message)}\"");
}

static async Task ServerWorker()
{
    using var server = new Socket(SocketType.Dgram, ProtocolType.Udp);
    EndPoint endpoint = new IPEndPoint(IPAddress.Any, 1234);
    server.Bind(endpoint);

    var buffer = new byte[11];
    SocketReceiveMessageFromResult result = await server.ReceiveMessageFromAsync(buffer, SocketFlags.None, endpoint);

    Console.WriteLine($"[SERVER] Server received \"{Encoding.ASCII.GetString(buffer)}\". Sending the same message back");
    await server.SendToAsync(buffer, SocketFlags.None, result.RemoteEndPoint);
}
