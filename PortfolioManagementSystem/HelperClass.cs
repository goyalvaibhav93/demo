using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PortfolioManagementSystem
{
    public class HelperClass
    {
        public DateTime DateTimeResolve(double timeStamp)
        {
            DateTime date = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return(date.AddMilliseconds(timeStamp).ToLocalTime());
        }

        public void PostJsonData(string baseAddress, string jsonString)
        {
            var http = (HttpWebRequest)WebRequest.Create(new Uri(baseAddress));
            http.Accept = "application/json";
            http.ContentType = "application/json";
            http.Method = "POST";

            string parsedContent = jsonString;
            System.Windows.MessageBox.Show(parsedContent);
            ASCIIEncoding encoding = new ASCIIEncoding();
            Byte[] bytes = encoding.GetBytes(parsedContent);
            Stream newStream = http.GetRequestStream();
            newStream.Write(bytes, 0, bytes.Length);
            newStream.Close();
            var response = http.GetResponse();
            
            var stream = response.GetResponseStream();
            var sr = new StreamReader(stream);
            var content = sr.ReadToEnd();

            MessageBox.Show("Data Posted!");
        }

        public Stream GenerateStreamFromJsonString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public string GenerateJsonStringFromStream(Stream mStream)
        {
            mStream.Seek(0, SeekOrigin.Begin);
            byte[] buffer = new byte[mStream.Length];
            mStream.Read(buffer, 0, (int)mStream.Length);
            string jsonString = System.Text.Encoding.UTF8.GetString(buffer);
            return jsonString;
        }

        public string DownloadJsonString(string baseAddress)
        {
            WebClient webClient = new WebClient();
            return webClient.DownloadString(baseAddress);
        }

        public T UnserializeObjectFromJsonStream<T>(Stream jsonStream)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));

            return (T)serializer.ReadObject(jsonStream);
        }

        public List<T> UnserializeListObjectFromJsonStream<T>(Stream jsonStream)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<T>));

            return (List<T>)serializer.ReadObject(jsonStream);
        }

        public MemoryStream SerializeObjectToJsonStream<T>(T listToSerialize)
        {
            MemoryStream mStream = new MemoryStream();

            DataContractJsonSerializer serializer =
                new DataContractJsonSerializer(typeof(T));
            serializer.WriteObject(mStream, listToSerialize);

            return mStream;
        }

        public MemoryStream SerializeListObjectToJsonStream<T>(List<T> listToSerialize)
        {
            MemoryStream mStream = new MemoryStream();

            DataContractJsonSerializer serializer =
                new DataContractJsonSerializer(typeof(List<T>));
            serializer.WriteObject(mStream, listToSerialize);

            return mStream;
        }
    }
}
