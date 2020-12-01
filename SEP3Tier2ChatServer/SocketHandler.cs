﻿using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using MainServerAPI.Data;
using MainServerAPI.Network;

namespace SEP3Tier2ChatServer
{
    public class SocketHandler
    {
        private NetworkStream stream1to2;
        public string username { get; set; }

        public void HandleClient(TcpClient client, List<SocketHandler> clientList)
        {
            stream1to2 = client.GetStream();

            string s = "";
            bool whileTrue = true;


            //Read
            byte[] usernameFromClient = new byte[1024];
            int bytesReadUsername = stream1to2.Read(usernameFromClient, 0, usernameFromClient.Length);
            s = Encoding.ASCII.GetString(usernameFromClient, 0, bytesReadUsername);
            username = s;
            Console.WriteLine(username + "sockethandler");
            while (whileTrue)
            {
                try
                {
                    Console.WriteLine("Jeg str og venter");
                    //Read
                    byte[] dataFromClient = new byte[1024];
                    int bytesRead = stream1to2.Read(dataFromClient, 0, dataFromClient.Length);

                    s = Encoding.ASCII.GetString(dataFromClient, 0, bytesRead);
                    Request request = JsonSerializer.Deserialize<Request>(s);

                    switch (request.requestOperation)
                    {
                        case RequestOperationEnum.GETCONNECTIONS:
                        {
                            //Liste med Connections - Spørg tier 3
                            var stream2to3 = NetworkStream();
                            string send = JsonSerializer.Serialize(new Request()
                            {
                                requestOperation = RequestOperationEnum.GETCONNECTIONS,
                                Username = username
                            });
                            byte[] bytes = Encoding.ASCII.GetBytes(send);
                            Console.WriteLine("det er her(2 -> 3");
                            stream2to3.Write(bytes, 0, bytes.Length);

                            //Få svar fra tier 3 med request( o = Liste af connections)
                            byte[] fromServer = new byte[1024];
                            int read = stream2to3.Read(fromServer, 0, fromServer.Length);
                            stream1to2.Write(fromServer, 0, fromServer.Length);
                            var trimEmptyBytes = TrimEmptyBytes(fromServer);

                            //bruger listen til at checke Antallet af Connections, hvilket svarer til antallet af Images jeg skal hente
                            var connectionses = JsonSerializer.Deserialize<IList<Connections>>(JsonSerializer
                                .Deserialize<Request>(Encoding.ASCII.GetString(trimEmptyBytes)).o.ToString());
                            int amountOfImages = connectionses.Count;

                            //For Looper igennem antallet af billeder. laver array om til base 64 og sender et string array til Tier 1
                            for (int i = 0; i < amountOfImages; i++)
                            {
                                byte[] arraysFromServer = new byte[1024 * 1024];
                                int read2 = stream2to3.Read(arraysFromServer, 0, arraysFromServer.Length);
                                var emptyBytes = TrimEmptyBytes(arraysFromServer);

                                string image = Convert.ToBase64String(emptyBytes);
                                string encoded = String.Format("data:image/gif;base64,{0}", image);

                                var bytes1 = Encoding.ASCII.GetBytes(encoded);
                                stream1to2.Write(bytes1, 0, bytes1.Length);
                            }

                            break;
                        }
                    }
                }

                catch (Exception e)
                {
                    Console.WriteLine(e);
                    client.Close();
                    break;
                }
            }


        }

        private static NetworkStream NetworkStream()
        {
            NetworkStream stream;

            TcpClient tcpClient = new TcpClient("127.0.0.1", 6000);
            stream = tcpClient.GetStream();


            return stream;
        }

        private byte[] TrimEmptyBytes(byte[] array)
        {
            int i = array.Length - 1;
            while (array[i] == 0)
            {
                --i;
            }

            byte[] bar = new byte[i + 1];
            Array.Copy(array, bar, i + 1);
            return bar;
        }
    }
}