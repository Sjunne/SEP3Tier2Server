using System;
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
        private string username { get; set; }

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
            Console.WriteLine(username);
            while (whileTrue)
            {
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
                        stream2to3.Write(bytes,0,bytes.Length);
                        
                        //Få svar fra tier 3 med request( o = Liste af connections)
                        byte[] fromServer = new byte[1024];
                        int read = stream2to3.Read(fromServer, 0, fromServer.Length);
                        stream1to2.Write(fromServer, 0, fromServer.Length);
                        
                        
                        
                        
                        

                        //Liste med Images
                        break;
                    }
                }
                
                
                
                
                
                

                
            }

           
            client.Close();
        }
        private static NetworkStream NetworkStream()
        {
            NetworkStream stream;
                
            TcpClient tcpClient = new TcpClient("127.0.0.1", 6000);
            stream = tcpClient.GetStream();

                
             
            return stream;
        }
    }
    
}