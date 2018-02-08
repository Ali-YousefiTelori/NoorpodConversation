using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace System
{
    public static class AutoLogger
    {
        //static AutoLogger()
        //{
        //    ForceLog = true;
        //}
#if(DEBUG)
        static bool IsDebug = true;
#else
        static bool IsDebug = false;

#endif
        public static Func<StackTrace[]> GetAllThreadStacks { get; set; }

        static AutoLogger()
        {
            try
            {
                ApplicationDirectory = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            }
            catch
            {

            }
        }
        static void GetOneStackTraceText(StackTrace stackTrace, StringBuilder builder)
        {
            builder.AppendLine("<------------------------------StackTrace One Begin------------------------------>");

            StackFrame[] stackFrames = stackTrace.GetFrames();  // get method calls (frames)

            // write call stack method names
            foreach (StackFrame stackFrame in stackFrames)
            {
                var method = stackFrame.GetMethod();
                if (IsDebug)
                {
                    builder.AppendLine("<---Method Begin--->");
                    builder.AppendLine("File Name: " + stackFrame.GetFileName());
                    builder.AppendLine("Line Number: " + stackFrame.GetFileLineNumber());
                    builder.AppendLine("Column Number: " + stackFrame.GetFileColumnNumber());
                }


                builder.AppendLine("Name: " + method.Name);
                builder.AppendLine("Class: " + method.DeclaringType.Name);
                var param = method.GetParameters();
                builder.AppendLine("Params Count: " + param.Length);
                int i = 1;
                foreach (var p in param)
                {
                    builder.AppendLine("Param " + i + ":" + p.ParameterType.Name);
                    i++;
                }
                if (IsDebug)
                    builder.AppendLine("<---Method End--->");
            }
            builder.AppendLine("<------------------------------StackTrace One End------------------------------>");
        }

        static string GetFullStack()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<------------------------------Force Full Stack Trace Started------------------------------>");
            try
            {
                GetOneStackTraceText(new StackTrace(true), builder);
                if (GetAllThreadStacks != null)
                {
                    foreach (var stackTrace in GetAllThreadStacks())
                    {
                        GetOneStackTraceText(stackTrace, builder);
                    }
                }
            }
            catch (Exception e)
            {
                builder.AppendLine("Error Force Full Stack Trace: " + GetAllInner(e));
            }
            builder.AppendLine("<------------------------------Force Full Stack Trace Started------------------------------>");
            return builder.ToString();
        }

        public static void LogText(string text)
        {
            StringBuilder str = new StringBuilder();
            str.AppendLine("<Text Log Start>");
            str.AppendLine(text);
            //str.AppendLine(GetFullStack());
            str.AppendLine("<Text Log End>");
            string fileName = Path.Combine(ApplicationDirectory, "Error Logs.log");
            try
            {
                using (var stream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    stream.Seek(0, SeekOrigin.End);
                    byte[] bytes = Encoding.UTF8.GetBytes(System.Environment.NewLine + str.ToString());
                    stream.Write(bytes, 0, bytes.Length);
                }
            }
            catch
            {

            }
        }

        public static bool ForceLog { get; set; }
        public static string ApplicationDirectory;
        public static void LogError(Exception e, string title, bool canSave = false)
        {
            try
            {
                //#if (!DEBUG)
                //                if (!canSave && !ForceLog)
                //                    return;
                //#endif
                StringBuilder str = new StringBuilder();
                str.AppendLine(title);
                str.AppendLine(GetAllInner(e));
                str.AppendLine(GetFullStack());
                str.AppendLine("Time : " + DateTime.Now.ToString());
                str.AppendLine("--------------------------------------------------------------------------------------------------");
                str.AppendLine("--------------------------------------------------------------------------------------------------");
                string fileName = Path.Combine(ApplicationDirectory, "Error Logs.log");

                try
                {
                    using (FileStream stream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                    {
                        stream.Seek(0, SeekOrigin.End);
                        byte[] bytes = Encoding.UTF8.GetBytes(System.Environment.NewLine + str.ToString());
                        stream.Write(bytes, 0, bytes.Length);
                    }

                }
                catch
                {

                }
            }
            catch
            {

            }
        }

        static string GetAllInner(Exception e)
        {
            string msg = "Start Exception:" + System.Environment.NewLine;
            while (e != null)
            {
                msg += "<---------Start One--------->" + System.Environment.NewLine + GetTextMessageFromException(e) + System.Environment.NewLine + "<---------End One--------->" + System.Environment.NewLine;
                e = e.InnerException;
            }
            return msg + "End Exception";
        }

        static string GetTextMessageFromException(Exception e)
        {
            if (e == null)
                return "No Exception";
            else
            {
                if (e.Message == null)
                {
                    if (e.StackTrace == null)
                    {
                        return "Null Error";
                    }
                    else
                    {
                        return "FaghatStack : " + e.StackTrace;
                    }
                }
                else
                {
                    if (e.StackTrace == null)
                    {
                        return "Stack Is Null But Message: " + e.Message;
                    }
                    else
                    {
                        return "*Message: " + e.Message + System.Environment.NewLine + "*Stack: " + e.StackTrace;
                    }
                }
            }
        }

    }
}
