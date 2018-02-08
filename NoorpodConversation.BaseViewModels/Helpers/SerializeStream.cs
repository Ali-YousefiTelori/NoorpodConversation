using Newtonsoft.Json;
using SignalGo.Shared.Log;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace System
{
    public static class SerializeStream
    {
        public static Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public static string GenerateStringFromStream(Stream stream)
        {
            stream.Position = 0;
            StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        public static void SaveSerializeStream<T>(string fileName, T value)
        {
            using (Stream mstream = GenerateStreamFromString(Newtonsoft.Json.JsonConvert.SerializeObject(value)))
            {
                using (var compress = new FileStream(fileName, FileMode.Create))
                {
                    using (System.IO.Compression.GZipStream fs = new System.IO.Compression.GZipStream(compress, System.IO.Compression.CompressionMode.Compress))
                    {
                        mstream.Seek(0, SeekOrigin.Begin);
                        byte[] bytes = new byte[mstream.Length];
                        int read = mstream.Read(bytes, 0, (int)mstream.Length);
                        fs.Write(bytes, 0, read);
                    }
                }
            }
        }

        public static T OpenSerializeStream<T>(string fileName) where T : class
        {
            try
            {
                if (!System.IO.File.Exists(fileName))
                    return null;
                using (var compress = new FileStream(fileName, FileMode.Open))
                {
                    if (compress.Length > 0)
                        using (System.IO.Compression.GZipStream fs = new System.IO.Compression.GZipStream(compress, System.IO.Compression.CompressionMode.Decompress))
                        {
                            using (MemoryStream stream = new MemoryStream())
                            {
                                fs.CopyTo(stream);
                                return JsonConvert.DeserializeObject<T>(GenerateStringFromStream(stream));
                            }
                        }
                    else
                        return null;
                }
            }
            catch (Exception e)
            {
                AutoLogger.LogError(e, "OpenSerializeStream");
                return null;
            }
        }
    }
}
