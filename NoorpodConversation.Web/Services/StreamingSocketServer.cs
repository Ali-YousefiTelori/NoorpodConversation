using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace NoorpodConversation.Web.Services
{
    public class StreamingSocketServer
    {
        public void Start()
        {
            TcpListener server = null;

            // Set the TcpListener on port 13000.
            Int32 port = 13020;
            var ipAddress = Dns.GetHostEntry("82.102.13.99").AddressList[0];
            // TcpListener server = new TcpListener(port);
            server = new TcpListener(ipAddress, port);

            // Start listening for client requests.
            server.Start();



            // Enter the listening loop.
            while (true)
            {
                AddClient(server.AcceptTcpClient());
            }
        }

        List<TcpClient> clients = new List<TcpClient>();

        public void AddClient(TcpClient client)
        {
            Task task = new Task(() =>
            {
                try
                {
                    clients.Add(client);
                    // Buffer for reading data
                    byte[] bytes = new byte[6000];

                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();

                    // Loop to receive all the data sent by the client.
                    while (true)
                    {
                        var readCount = stream.Read(bytes, 0, bytes.Length);
                        if (readCount == 0)
                        {
                            Thread.Sleep(1000);
                            continue;
                        }
                        // Translate data bytes to a ASCII string.
                        foreach (var item in clients)
                        {
                            if (item != client)
                                SendClientData(item, bytes.ToList().GetRange(0, readCount).ToArray());
                        }
                    }

                    // Shutdown and end connection
                    client.Close();
                }
                catch (Exception ex)
                {

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
                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();
                    stream.Write(data, 0, data.Length);
                }
                catch (Exception ex)
                {

                }
            });
            task.Start();
        }
    }
}