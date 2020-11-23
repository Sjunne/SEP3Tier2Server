using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using MainServerAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.CompilerServices;
using SEP3Tier2Server.Exceptions;

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
            ProfileData profileData = JsonSerializer.Deserialize<ProfileData>(request.o.ToString());
            if (profileData == null)
            {
                throw new NetworkIssue("ProfileData was null");
            }
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
            if (fromServer[0] == 0)
            {
                throw new NetworkIssue("Cover-byte array was empty");
            }
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
            
            int count = Int32.Parse(from.ToCharArray()[0].ToString());
            for (int i = 0; i < count; i++)
            {
                byte[] read = new byte[1024*1024];
                stream.Read(read, 0, read.Length);
                list.Add(TrimEmptyBytes(read));
            }
            
            if (fromServer[0] == 0)
            {
                throw new NetworkIssue("Cover-byte array was empty");
            }
            
            return list;
        }

        public void UploadImage(Request request)
        {
            var stream = NetworkStream();
            string json = JsonSerializer.Serialize(request);
            byte[] toServer = Encoding.ASCII.GetBytes(json);
            stream.Write(toServer, 0, toServer.Length);
        }

        public void UpdateCover(string pictureName)
        {
            var stream = NetworkStream();
            string json = JsonSerializer.Serialize(new Request()
            {
                Username = "Maria",
                o = pictureName,
                requestOperation = RequestOperationEnum.UPDATECOVER
            });
            byte[] toServer = Encoding.ASCII.GetBytes(json);
            stream.Write(toServer, 0, toServer.Length);
        }

        public byte[] GetProfilePicture(string username)
        {
            var stream = NetworkStream();
            string s = JsonSerializer.Serialize<Request>(new Request()
            {
                Username =  username,
                requestOperation = RequestOperationEnum.PROFILEPIC
            });
            
            byte[] dataToServer = Encoding.ASCII.GetBytes(s);
            stream.Write(dataToServer, 0, dataToServer.Length);
            
            
            byte[] fromServer = new byte[16*1024];
            stream.Read(fromServer, 0, fromServer.Length);
            if (fromServer[0] == 0)
            {
                throw new NetworkIssue("Cover-byte array was empty");
            }
            return fromServer;
        }

        public void UpdateProfilePic(string pictureName)
        {
            var stream = NetworkStream();
            string json = JsonSerializer.Serialize(new Request()
            {
                Username = "Maria",
                o = pictureName,
                requestOperation = RequestOperationEnum.UPDATEPROFILEPIC
            });
            byte[] toServer = Encoding.ASCII.GetBytes(json);
            stream.Write(toServer, 0, toServer.Length);        
        }

        public void editProfile(Request request)
        {
            var stream = NetworkStream();
            string json = JsonSerializer.Serialize(request);
            byte[] toServer = Encoding.ASCII.GetBytes(json);
            stream.Write(toServer, 0, toServer.Length);
        }

        public IList<Review> GetReviews(string username)
        {
            Request request = new Request()
            {
                Username = username,
                requestOperation = RequestOperationEnum.REVIEWS
            };
            string json = JsonSerializer.Serialize(request);
            var stream = NetworkStream();


            byte[] dataToServer = Encoding.ASCII.GetBytes(json);
            stream.Write(dataToServer, 0, dataToServer.Length);
            byte[] fromServer = new byte[1024*1024*2];
            int bytesRead = stream.Read(fromServer, 0, fromServer.Length);

            string s = Encoding.ASCII.GetString(fromServer, 0, bytesRead);
            Int32 number = JsonSerializer.Deserialize<Int32>(s);
            
            IList<Review> reviews = new List<Review>();
            for (int i = 0; i < number; i++)
            {
                stream.Read(fromServer, 0, fromServer.Length);
                
                string image = Convert.ToBase64String(fromServer);
                string encoded = String.Format("data:image/gif;base64,{0}", image);

                int read1 = stream.Read(fromServer, 0, fromServer.Length);
                string s1 = Encoding.ASCII.GetString(fromServer, 0, read1);
                Request deserialize = JsonSerializer.Deserialize<Request>(s1);
                Review review = JsonSerializer.Deserialize<Review>(deserialize.o.ToString());

                review.image = encoded;
                reviews.Add(review);

            }
            return reviews;
        }

        private byte[] TrimEmptyBytes(byte[] array)
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
            byte[] fromServer = new byte[1024*1024];
            int bytesRead = stream.Read(fromServer, 0, fromServer.Length);

            //Tar Imod request gennem sockets
            string response = Encoding.ASCII.GetString(fromServer, 0, bytesRead);
            Request request = JsonSerializer.Deserialize<Request>(response);
            return request;

        }
        
        
        private static NetworkStream NetworkStream()
        {
            NetworkStream stream;

            try
            {
                TcpClient tcpClient = new TcpClient("127.0.0.1", 6000);
                stream = tcpClient.GetStream();

            }
            catch (InvalidCastException)
            {
                throw new NetworkIssue("No connection to tier 3");
            }
            catch (SocketException e)
            {
                throw new NetworkIssue("Tier 3 not started");
            }
            
            return stream;
        }
    }
}