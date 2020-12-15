using System;
using System.Collections.Generic;
using MainServerAPI.Network;

namespace SEP3Tier2Server.Data
{
    public class DataService
    {
        public byte[] TrimEmptyBytes(byte[] array)
        {
            int i = array.Length - 1;
            Console.WriteLine(array.Length +"trimbytes");
            while (array[i] == 0)
            {
                --i;
            }

            byte[] bar = new byte[i + 1];
            Array.Copy(array, bar, i+1);
            return bar;
        }
        
        public string[] SplitJson(Request request)
        {
            string[] json = request.o.ToString().Split('}');
            json[3] += "}";

            char[] c = json[2].ToCharArray();
            c[0] = '{';
            json[2] = new string(c);
            json[2] += "}";
            return json;
        }

        public string Base64ImagesToString(List<byte[]> b)
        {
            string allImages = "";
            for (int i = 0; i < b.Count; i++)
            {
                string image = Convert.ToBase64String(b[i]);
                string encoded = String.Format("data:image/gif;base64,{0}", image);
                allImages += encoded;
                allImages += "å";
            }

            return allImages;
        }

        public string Base64ImagesToString(byte[] arr)
        {
            var base64 = Convert.ToBase64String(arr);
            var imgSrc = String.Format("data:image/gif;base64,{0}", base64);
            return imgSrc;
        }

    }
}