using SignalGo.Shared.Log;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NoorpodConversation.Helper
{
    public static class SocketMessageHelper
    {
        public static byte[] CreateMesssage(byte[] bytes)
        {
            var len = BitConverter.GetBytes(bytes.Length);
            List<byte> data = new List<byte>();
            data.AddRange(len);
            data.AddRange(bytes);
            return data.ToArray();
        }

        public static byte[] ReadMessage(Stream stream)
        {
            List<byte> len = new List<byte>();
            int readCount = 0;
            while (readCount < 4)
            {
                var rl = 4;
                if (rl > 4 - readCount)
                    rl = 4 - readCount;
                byte[] rLen = new byte[rl];
                var rc = stream.Read(rLen, 0, rl);
                if (rc == 0 && readCount == 0)
                    break;
                len.AddRange(rLen.ToList().GetRange(0, rc));
                readCount += rc;
            }
            if (readCount != 0 && readCount != 4)
                AutoLogger.LogText("it is bug :" + readCount);
            if (readCount == 0)
                return null;
            int contentLenght = BitConverter.ToInt32(len.ToArray(), 0);
            int lengthReaded = 0;
            List<byte> readed = new List<byte>();
            while (lengthReaded < contentLenght)
            {
                var readLen = 1024 * 20;
                if (readLen > contentLenght - lengthReaded)
                    readLen = contentLenght - lengthReaded;
                byte[] bytes = new byte[readLen];
                readCount = stream.Read(bytes, 0, bytes.Length);
                if (readCount == 0)
                {
                    AutoLogger.LogText("FatalError ReadMessage");
                }
                readed.AddRange(bytes.ToList().GetRange(0, readCount));
                lengthReaded += readCount;
            }
            if (contentLenght != lengthReaded)
            {
                AutoLogger.LogText("FatalError ReadMessage contentLenght: " + contentLenght + " lengthReaded: " + lengthReaded);
            }
            if (readed.Count == 0)
                return null;
            return readed.ToArray();
        }

        //public static byte[] ReadMessage(byte[] bytes)
        //{

        //}
    }
}
