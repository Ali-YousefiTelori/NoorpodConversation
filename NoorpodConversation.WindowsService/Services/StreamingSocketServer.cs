using NoorpodConversation.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace NoorpodConversation.Services
{
    public class StreamingSocketServer
    {
        public void Start()
        {
            TcpListener server = null;

            // Set the TcpListener on port 13000.
            int port = 13020;
            var ipAddress = Dns.GetHostEntry("localhost").AddressList[0];
            // TcpListener server = new TcpListener(port);
            server = new TcpListener(IPAddress.Parse("185.86.180.208"), port);

            // Start listening for client requests.
            server.Start();
            AutoLogger.LogText("Connected");



            // Enter the listening loop.
            while (true)
            {
                AddClient(server.AcceptTcpClient());
            }
        }
        List<TcpClient> _Clients = new List<TcpClient>();
        object lockObj = new object();
        List<TcpClient> Clients
        {
            get
            {
                return _Clients;
            }
        }

        public void AddClient(TcpClient client)
        {
            Task task = new Task(() =>
            {
                try
                {
                    Clients.Add(client);
                    // Buffer for reading data
                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();

                    // Loop to receive all the data sent by the client.
                    while (true)
                    {
                        if (!Clients.ToArray().Contains(client) || !client.Connected)
                        {
                            client.Close();
                            return;
                        }
                        var data = SocketMessageHelper.ReadMessage(stream);
                        if (data == null)
                        {
                            Thread.Sleep(1000);
                            continue;
                        }
                        // Translate data bytes to a ASCII string.
                        foreach (var item in Clients.ToArray())
                        {
                            if (item != client && item.Connected)
                                SendClientData(item, data);
                        }
                    }

                }
                catch (Exception ex)
                {
                    if (client.Connected)
                        AutoLogger.LogError(ex, "AddClient");
                    else
                    {
                        client.Dispose();
                        Clients.Remove(client);
                        AutoLogger.LogText("client disconnected");
                    }
                }
            });
            task.Start();
        }

        public void SendClientData(TcpClient client, byte[] data)
        {
            Task task = new Task(() =>
            {
                try
                {
                    if (!Clients.ToArray().Contains(client))
                        return;
                    if (!client.Connected)
                        return;
                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();
                    data = SocketMessageHelper.CreateMesssage(data);
                    stream.Write(data, 0, data.Length);
                }
                catch (Exception ex)
                {
                    if (client.Connected)
                        AutoLogger.LogError(ex, "SendClientData");
                }
            });
            task.Start();
        }
    }
}