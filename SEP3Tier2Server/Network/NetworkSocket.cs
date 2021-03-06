﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using MainServerAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.CompilerServices;
using SEP3Tier2Server.Data;
using SEP3Tier2Server.Exceptions;
using WebApplication.Data;
using Match = MainServerAPI.Data.Match;

namespace MainServerAPI.Network
{
    public class NetworkSocket : INetwork
    {
        private DataService service;
        public NetworkSocket()
        {
            service = new DataService();
        }
        
        public RequestOperationEnum updateProfile(ProfileData profile)
        {
            var stream = NetworkStream();
            profile.jsonSelf = null;
            profile.jsonPref = null;
            string s = JsonSerializer.Serialize(new Request
            {
            o=JsonSerializer.Serialize(profile),
            requestOperation = RequestOperationEnum.EDITINTRODUCTION,
            });
            
            byte[] dataToServer = Encoding.ASCII.GetBytes(s);
            stream.Write(dataToServer, 0, dataToServer.Length);
            
            byte[] fromServer = new byte[1024];
            stream.Read(fromServer, 0, fromServer.Length);
            byte[] trimEmptyBytes = service.TrimEmptyBytes(fromServer);
            return RequestEnum(trimEmptyBytes);
        }

        public ProfileData GetProfile(string username)
        {

            //Sender requesten om profile til Tier 3, med username som nøgle til database
            string s = JsonSerializer.Serialize(new Request
            {
                o = username,
                requestOperation = RequestOperationEnum.PROFILE,
                Username = ""
            });
            Request request = WriteAndReadFromServer(s);
            var json = service.SplitJson(request);
            ProfileData profileData = JsonSerializer.Deserialize<ProfileData>(json[2]);
            
            profileData.self = JsonSerializer.Deserialize<Details>(json[3]);
            profileData.jsonSelf = json[3];
            if (profileData == null)
            {
                throw new NetworkIssue("ProfileData was null");
            }
            
            return profileData;
        }

        

        public IList<string> getAllProfiles()
        {
            string s = JsonSerializer.Serialize(new Request
            {
                o = "profiles",
                requestOperation = RequestOperationEnum.ALLPROFILES,
                Username = ""
            });
            Request request = WriteAndReadFromServer(s);
            IList<string> Usernames = JsonSerializer.Deserialize<IList<string>>(request.o.ToString());
            if (Usernames == null)
            {
                throw new NetworkIssue("ProfileData was null");
            }

            return Usernames;
        }

     
        public Warning GetWarning(String username)
        {
            string s = JsonSerializer.Serialize(new Request
            {
            Username = username,
            requestOperation = RequestOperationEnum.GETWARNING
            });

            Request request = WriteAndReadFromServer(s);
            Warning warning1 = JsonSerializer.Deserialize<Warning>(request.o.ToString());

            return warning1;
        }

        public RequestOperationEnum RemoveWarning(string username)
        {var stream = NetworkStream();
            Console.Write(username);
            string s = JsonSerializer.Serialize(new Request
            {
                Username= username,
                requestOperation = RequestOperationEnum.REMOVEWARNING,
            
            });
            byte[] dataToServer = Encoding.ASCII.GetBytes(s);
            stream.Write(dataToServer, 0, dataToServer.Length);
            return RequestOperationEnum.SUCCESS;
        }


        public string GetCover(string username)
        {
            var stream = NetworkStream();
            
            //Serializes Request Object
            string s = JsonSerializer.Serialize<Request>(new Request()
            {
                Username =  username,
                requestOperation = RequestOperationEnum.COVER
            });
            //Converts to byte Array
            byte[] dataToServer = Encoding.ASCII.GetBytes(s);
            //Writes to Tier3
            stream.Write(dataToServer, 0, dataToServer.Length);
            //Waits for respond (picture, so large array)
            byte[] fromServer = new byte[1024*1024];
            int read = stream.Read(fromServer, 0, fromServer.Length);
            
            if (read == 1)
            {
                throw new NetworkIssue("Cover picture not found");
            }
            else if (read == 2)
            {
                throw new ServiceUnavailable("Database Connection Lost");
            }
            return service.Base64ImagesToString(fromServer);
        }

        public string GetPictures(string username)
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
            if (count == -1)
            {
                throw new ServiceUnavailable("Lost database connection");
            }
            
            for (int i = 0; i < count; i++)
            {
                byte[] read = new byte[1024*1024];
                stream.Read(read, 0, read.Length);
                list.Add(service.TrimEmptyBytes(read));
            }
            return service.Base64ImagesToString(list);
        }

        public RequestOperationEnum UploadImage(Request request)
        {
            var stream = NetworkStream();
            string json = JsonSerializer.Serialize(request);
            byte[] toServer = Encoding.ASCII.GetBytes(json);

            stream.Write(toServer, 0, toServer.Length);
            
            byte[] fromServer = new byte[1024];
            stream.Read(fromServer, 0, fromServer.Length);
            byte[] trimEmptyBytes = service.TrimEmptyBytes(fromServer);
            string response = Encoding.ASCII.GetString(trimEmptyBytes);
            
            
            Request request1 = JsonSerializer.Deserialize<Request>(response);
            return request1.requestOperation;
        }

        public RequestOperationEnum UpdateCover(Request r)
        {
            var stream = NetworkStream();
            string json = JsonSerializer.Serialize(r);
            byte[] toServer = Encoding.ASCII.GetBytes(json);
            stream.Write(toServer, 0, toServer.Length);

            byte[] fromServer = new byte[1024];
            stream.Read(fromServer, 0, fromServer.Length);
            var trimEmptyBytes = service.TrimEmptyBytes(fromServer);
            string s = Encoding.ASCII.GetString(trimEmptyBytes);
            Request request = JsonSerializer.Deserialize<Request>(s);
            return request.requestOperation;
        }

        public string GetProfilePicture(string username)
        {
            var stream = NetworkStream();
            string s = JsonSerializer.Serialize<Request>(new Request()
            {
                Username =  username,
                requestOperation = RequestOperationEnum.PROFILEPIC
            });
            
            byte[] dataToServer = Encoding.ASCII.GetBytes(s);
            stream.Write(dataToServer, 0, dataToServer.Length);
            
            byte[] fromServer = new byte[1024*1024];
            int read = stream.Read(fromServer, 0, fromServer.Length);
            Console.WriteLine(read+"read 195");
            

            if (read == 1)
            {
                Console.WriteLine("profile pic not found");
                throw new NetworkIssue("Profile Picture was not found");
            }
            else if (read == 2)
            {
                throw new ServiceUnavailable("Lost database connection");
            }
            fromServer= service.TrimEmptyBytes(fromServer);

            return service.Base64ImagesToString(fromServer);
        }

        public RequestOperationEnum UpdateProfilePic(Request r)
        {
            var stream = NetworkStream();
            string json = JsonSerializer.Serialize(r);
            byte[] toServer = Encoding.ASCII.GetBytes(json);
            stream.Write(toServer, 0, toServer.Length);

            byte[] fromServer = new byte[1024];
            stream.Read(fromServer, 0, fromServer.Length);
            var trimEmptyBytes = service.TrimEmptyBytes(fromServer);
            string s = Encoding.ASCII.GetString(trimEmptyBytes);
            Request request = JsonSerializer.Deserialize<Request>(s);
            return request.requestOperation;
        }

        public RequestOperationEnum editProfile(Request request)
        {
            var stream = NetworkStream();
            string json = JsonSerializer.Serialize(request);
            byte[] toServer = Encoding.ASCII.GetBytes(json);
            stream.Write(toServer, 0, toServer.Length);
            
            byte[] fromServer = new byte[1024];
            stream.Read(fromServer, 0, fromServer.Length);
            byte[] trimEmptyBytes = service.TrimEmptyBytes(fromServer);
            string response = Encoding.ASCII.GetString(trimEmptyBytes);

            Request requestResponse = JsonSerializer.Deserialize<Request>(response);
            return requestResponse.requestOperation;
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
                int read = stream.Read(fromServer, 0, fromServer.Length);
                if (read == 1)
                {
                    throw new ServiceUnavailable("Lost database connection");
                }

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

        public void CreateProfile(ProfileData profileData)
        {
            var stream = NetworkStream();
            
            string s = JsonSerializer.Serialize(new Request
            {
                o=profileData,
                requestOperation = RequestOperationEnum.CREATEPROFILE,
            
            });
            byte[] dataToServer = Encoding.ASCII.GetBytes(s);
            stream.Write(dataToServer, 0, dataToServer.Length);
        }

        public void CreatePreference(ProfileData profileData)
        {
            var stream = NetworkStream();
            
            string s = JsonSerializer.Serialize(new Request
            {
                o=profileData,
                requestOperation = RequestOperationEnum.CREATEPREFERENCE,
            
            });
            byte[] dataToServer = Encoding.ASCII.GetBytes(s);
            stream.Write(dataToServer, 0, dataToServer.Length);
        }

        public RequestOperationEnum DeletePhoto(string pictureName)
        {
            var stream = NetworkStream();
            
            string s = JsonSerializer.Serialize(new Request
            {
                o=pictureName,
                requestOperation = RequestOperationEnum.DELETEPHOTO,
            
            });
            byte[] dataToServer = Encoding.ASCII.GetBytes(s);
            stream.Write(dataToServer, 0, dataToServer.Length);
            return RequestOperationEnum.SUCCESS;
        }

        public RequestOperationEnum CreateMatch(Match match)
        {
            var stream = NetworkStream();
            string s = JsonSerializer.Serialize(new Request
            {
                o=match,
                requestOperation = RequestOperationEnum.CREATEMATCH
            });
            byte[] dataToServer = Encoding.ASCII.GetBytes(s);
            stream.Write(dataToServer, 0, dataToServer.Length);
            return RequestOperationEnum.SUCCESS;
        }

        //private methods 
        
        
        
        public ProfileData GetPreference(string username)
        {
            //Sender requesten om preference til Tier 3, med username som nøgle til database
            string s = JsonSerializer.Serialize(new Request
            {
                o = username,
                requestOperation = RequestOperationEnum.GETPREFERENCE,
                
            });
            Request request = WriteAndReadFromServer(s);
            var json = service.SplitJson(request);
            ProfileData profileData = JsonSerializer.Deserialize<ProfileData>(json[2]);
            
            profileData.preferences = JsonSerializer.Deserialize<Details>(json[3]);
            profileData.jsonPref = json[3];

            if (profileData == null)
            {
                throw new NetworkIssue("ProfileData was null");
            }
            
            return profileData;
        }

        

        public Request ValidateLogin(User user)
        {
            var stream = NetworkStream();

            Request request1 = new Request()
            {
                Username = user.username,
                o = user,
                requestOperation = RequestOperationEnum.VALIDATELOGIN
            };
            string serialize = JsonSerializer.Serialize(request1);
            byte[] dataToServer = Encoding.ASCII.GetBytes(serialize);
            stream.Write(dataToServer, 0, dataToServer.Length);
            byte[] fromServer = new byte[1024*1024];
            int bytesRead = stream.Read(fromServer, 0, fromServer.Length);
            
            string response = Encoding.ASCII.GetString(fromServer, 0, bytesRead);
            Request request = JsonSerializer.Deserialize<Request>(response);
            return request;
        }

        public Request RegisterUser(Request request)
        {
            string serialize = JsonSerializer.Serialize(request);
            Request writeAndReadFromServer = WriteAndReadFromServer(serialize);
            
            return writeAndReadFromServer;
        }

        public RequestOperationEnum DeclineMatch(Match match)
        {
            var stream = NetworkStream();
            
            string json = JsonSerializer.Serialize(new Request
            {
                
                o=match,
                requestOperation = RequestOperationEnum.DECLINEMATCH,
            
            });
            byte[] toServer = Encoding.ASCII.GetBytes(json);
            stream.Write(toServer, 0, toServer.Length);

            byte[] fromServer = new byte[1024];
            stream.Read(fromServer, 0, fromServer.Length);
            var trimEmptyBytes = service.TrimEmptyBytes(fromServer);
            string s = Encoding.ASCII.GetString(trimEmptyBytes);
            Request request = JsonSerializer.Deserialize<Request>(s);
            return request.requestOperation;
        }

        public Request ChangePasswordOrUsername(Request request)
        {
            string serialize = JsonSerializer.Serialize(request);
            Request writeAndReadFromServer = WriteAndReadFromServer(serialize);
            return writeAndReadFromServer;
        }

        public Request AddReview(Request request)
        {
            var stream = NetworkStream();

            string json = JsonSerializer.Serialize(request);
            byte[] toServer = Encoding.ASCII.GetBytes(json);
            stream.Write(toServer, 0, toServer.Length);

            byte[] fromServer = new byte[1024];
            stream.Read(fromServer, 0, fromServer.Length);
            var trimEmptyBytes = service.TrimEmptyBytes(fromServer);
            string s = Encoding.ASCII.GetString(trimEmptyBytes);
            Request response = JsonSerializer.Deserialize<Request>(s);
            return response;
        }

        public Request ReportReview(Request request)
        {
            var stream = NetworkStream();

            string json = JsonSerializer.Serialize(request);
            byte[] toServer = Encoding.ASCII.GetBytes(json);
            stream.Write(toServer, 0, toServer.Length);

            byte[] fromServer = new byte[1024];
            stream.Read(fromServer, 0, fromServer.Length);
            var trimEmptyBytes = service.TrimEmptyBytes(fromServer);
            string s = Encoding.ASCII.GetString(trimEmptyBytes);
            Request response = JsonSerializer.Deserialize<Request>(s);
            return response;
        }

        public RequestOperationEnum AcceptMatch(Match match)
        {
            var stream = NetworkStream();
            
            string json = JsonSerializer.Serialize(new Request
            {
                
                o=match,
                requestOperation = RequestOperationEnum.ACCEPTMATCH,
            
            });
            byte[] toServer = Encoding.ASCII.GetBytes(json);
            stream.Write(toServer, 0, toServer.Length);

            byte[] fromServer = new byte[1024];
            stream.Read(fromServer, 0, fromServer.Length);
            var trimEmptyBytes = service.TrimEmptyBytes(fromServer);
            string s = Encoding.ASCII.GetString(trimEmptyBytes);
            Request request = JsonSerializer.Deserialize<Request>(s);
            return request.requestOperation;
        
        }
        

        public IList<PrivateMessage> getAllPrivateMessages(Request request)
        {
            var stream = NetworkStream();
            Console.WriteLine(request.o + "o");
            Console.WriteLine(request.Username + "username");
            Console.WriteLine(request.requestOperation + "requestOperation");

            var serialize = JsonSerializer.Serialize(request);
            byte[] dataToServer = Encoding.ASCII.GetBytes(serialize);
            stream.Write(dataToServer, 0, dataToServer.Length);

            byte[] fromServer = new byte[1024*1024];
            int bytesRead = stream.Read(fromServer, 0, fromServer.Length);

            //Tar Imod request gennem sockets
            string response = Encoding.ASCII.GetString(fromServer, 0, bytesRead);
            Request request1 = JsonSerializer.Deserialize<Request>(response);
            IList<PrivateMessage> messages = JsonSerializer.Deserialize<IList<PrivateMessage>>(request1.o.ToString());

            return messages;
        }


        //private methods 
        
        
        
        public void editPreference(ProfileData profileData)
        {
            var stream = NetworkStream();
            
            string s = JsonSerializer.Serialize(new Request
            {
                o=profileData,
                requestOperation = RequestOperationEnum.EDITPREFERENCE,
            
            });
            byte[] dataToServer = Encoding.ASCII.GetBytes(s);
            stream.Write(dataToServer, 0, dataToServer.Length);
        }

        public void bigEditProfile(ProfileData profileData)
        {
            var stream = NetworkStream();
            
            string s = JsonSerializer.Serialize(new Request
            {
                o=profileData,
                requestOperation = RequestOperationEnum.EDITPROFILE,
            
            });
            byte[] dataToServer = Encoding.ASCII.GetBytes(s);
            stream.Write(dataToServer, 0, dataToServer.Length);
        }

        public void deleteProfile(string username)
        {
            var stream = NetworkStream();
            string s = JsonSerializer.Serialize(new Request
            {
                o=username,
                requestOperation = RequestOperationEnum.DELETEPROFILE,
            
            });
            byte[] dataToServer = Encoding.ASCII.GetBytes(s);
            stream.Write(dataToServer, 0, dataToServer.Length);
        }

        public IList<String> Matches(string username)
        {
            string s = JsonSerializer.Serialize(new Request
            {
                Username = username,
                requestOperation = RequestOperationEnum.MATCHES,
                
            });
            Request request = WriteAndReadFromServer(s);
            IList<String> usernames = JsonSerializer.Deserialize<IList<String>>(request.o.ToString());
            if (usernames == null)
            {
                throw new NetworkIssue("No profiles found");
            }
            return usernames; 
        }
        
        
        
        

        
        
        
        
        
        private NetworkStream NetworkStream()
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
        
        private static RequestOperationEnum RequestEnum(byte[] fromServer)
        {
            string response = Encoding.ASCII.GetString(fromServer);
            Request request = JsonSerializer.Deserialize<Request>(response);
            return request.requestOperation;
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
    }
}