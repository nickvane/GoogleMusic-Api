using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace GoogleMusic.Api.Helpers
{
    public class Json
    {
        public static T Deserialize<T>(String data)
        {
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(data));
            var serializer = new DataContractJsonSerializer(typeof(T));
            return (T)serializer.ReadObject(ms);
        }
    }
}
