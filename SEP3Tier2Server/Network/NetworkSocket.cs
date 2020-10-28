﻿using System;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using MainServerAPI.Data;

namespace MainServerAPI.Network
{
    public class NetworkSocket : INetwork
    {
        
        private TcpClient tcpClient;
        private NetworkStream stream;
        
        public NetworkSocket()
        {
            tcpClient = new TcpClient("127.0.0.1", 6000);
            stream = tcpClient.GetStream();
        }
        
        public void updateProfile(ProfileData profile)
        {
            string s = JsonSerializer.Serialize(new Request
            {
            o=profile,
            requestOperation = RequestOperationEnum.EDITINTRODUCTION,
            
            });
            byte[] dataToServer = Encoding.ASCII.GetBytes(s);
            stream.Write(dataToServer, 0, dataToServer.Length);
        }

        public ProfileData GetProfile(string username)
        {
            //Sender requesten om profile til Tier 3, med username som nøgle til database
            string s = JsonSerializer.Serialize(new Request
            {
                o = username,
                requestOperation = RequestOperationEnum.PROFILE,
            
            });
            byte[] dataToServer = Encoding.ASCII.GetBytes(s);
            stream.Write(dataToServer, 0, dataToServer.Length);
            byte[] fromServer = new byte[1024];
            int bytesRead = stream.Read(fromServer, 0, fromServer.Length);
            
            //Tar Imod Profile gennem sockets
            string response = Encoding.ASCII.GetString(fromServer, 0, bytesRead);
            Console.WriteLine(response);
            Request request = JsonSerializer.Deserialize<Request>(response);
            ProfileData profileData = JsonSerializer.Deserialize<ProfileData>(request.o.ToString());
            return profileData;
        }
    }
}