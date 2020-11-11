using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using MainServerAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.CompilerServices;

namespace MainServerAPI.Network
{
    public class NetworkSocket : INetwork
    {
        

        public NetworkSocket()
        {
            
        }
        
        public void updateProfile(ProfileData profile)
        {
            var stream = NetworkStream();

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
            Request request = WriteAndReadFromServer(s);
            Console.WriteLine(request == null);
            ProfileData profileData = JsonSerializer.Deserialize<ProfileData>(request.o.ToString());
            return profileData;
        }

     

        public Byte[] GetCover(string username)
        {
            var stream = NetworkStream();

            
            
            string s = JsonSerializer.Serialize<Request>(new Request()
            {
                Username =  username,
                requestOperation = RequestOperationEnum.COVER
            });
            
            byte[] dataToServer = Encoding.ASCII.GetBytes(s);
            stream.Write(dataToServer, 0, dataToServer.Length);
            
            
            byte[] fromServer = new byte[16*1024];
            stream.Read(fromServer, 0, fromServer.Length);
            return fromServer;
        }

        public List<byte[]> GetPictures(string username)
        {
            var stream = NetworkStream();

            string s = JsonSerializer.Serialize<Request>(new Request()
            {
                Username =  username,
                requestOperation = RequestOperationEnum.PICTURES
            });
            
            byte[] dataToServer = Encoding.ASCII.GetBytes(s);
            stream.Write(dataToServer, 0, dataToServer.Length);
            
            
            
            List<byte[]> list = new List<byte[]>();
            
            byte[] fromServer = new byte[1024];
            stream.Read(fromServer, 0, fromServer.Length);
            string from = Encoding.ASCII.GetString(fromServer);
            
            int count = Int32.Parse(from);
            for (int i = 0; i < count; i++)
            {
                byte[] read = new byte[1024*1024];
                stream.Read(read, 0, read.Length);
                list.Add(TrimEmptyBytes(read));
            }
            
            return list;
        }

        public byte[] TrimEmptyBytes(byte[] array)
        {
            int i = array.Length - 1;
            while (array[i] == 0)
            {
                --i;
            }

            byte[] bar = new byte[i + 1];
            Array.Copy(array, bar, i+1);
            return bar;
        }

        
        
        
        
        
        
        //Metoder til optimering
        private Request WriteAndReadFromServer(string s)
        {
            var stream = NetworkStream();

            
            
            byte[] dataToServer = Encoding.ASCII.GetBytes(s);
            stream.Write(dataToServer, 0, dataToServer.Length);
            byte[] fromServer = new byte[1024];
            int bytesRead = stream.Read(fromServer, 0, fromServer.Length);

            //Tar Imod Profile gennem sockets
            string response = Encoding.ASCII.GetString(fromServer, 0, bytesRead);
            Console.WriteLine(response);
            Request request = JsonSerializer.Deserialize<Request>(response);
            return request;

        }
        
        
        private static NetworkStream NetworkStream()
        {
            TcpClient tcpClient = new TcpClient("127.0.0.1", 6000);
            NetworkStream stream = tcpClient.GetStream();
            return stream;
        }
    }
}