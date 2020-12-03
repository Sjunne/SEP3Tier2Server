using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using MainServerAPI.Network;

namespace SEP3Tier2ChatServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting server..");

            IPAddress ip = IPAddress.Parse("127.0.0.1");
            TcpListener listener = new TcpListener(ip, 4500);
            listener.Start();
            
            Console.WriteLine("Server started..");
            List<SocketHandler> clientList = new List<SocketHandler>();

            while (true)
            {
                try
                {
                    TcpClient client = listener.AcceptTcpClient();
                    Console.WriteLine("Client connected..");
                

                    SocketHandler sh = new SocketHandler();
                    clientList.Add(sh);
                    Thread thread = new Thread(() => sh.HandleClient(client, clientList));
                    thread.IsBackground = true;
                    thread.Start();
                }
                catch (Exception e)
                {
                    
                }
                
            }
            
            
        }
    }
    
    
}