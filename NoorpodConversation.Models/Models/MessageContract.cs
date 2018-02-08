using SignalGo.Shared.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoorpodConversation.Models
{
    public class MessageContract<T>
    {
        public T Data { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }

        public static MessageContract<T> Fail(string message)
        {
            return new MessageContract<T>() { IsSuccess = false, Message = message };
        }

        public static MessageContract<T> Fail(string message, string stackTrace)
        {
            return new MessageContract<T>() { IsSuccess = false, Message = message, StackTrace = stackTrace };
        }

        public static MessageContract<T> Success(T data)
        {
            return new MessageContract<T>() { IsSuccess = true, Data = data };
        }

        public static MessageContract<T> Success()
        {
            return new MessageContract<T>() { IsSuccess = true };
        }
    }

    public class MessageContract
    {
        public object Data { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public static MessageContract Success()
        {
            return new MessageContract() { IsSuccess = true };
        }
        public static MessageContract Fail(string message)
        {
            return new MessageContract() { IsSuccess = false, Message = message };
        }
    }

    public static class MessageContractExtension
    {
        public static MessageContract Success(this object data)
        {
            return new MessageContract() { IsSuccess = true, Data = data };
        }

        public static MessageContract<T> Success<T>(this T data)
        {
            return new MessageContract<T>() { IsSuccess = true, Data = data };
        }

        public static MessageContract Fail(this string message, string stackTrace)
        {
            return new MessageContract() { IsSuccess = false, Message = message, StackTrace = stackTrace };
        }

        public static MessageContract RunAction(this object obj, Func<MessageContract> run)
        {
            try
            {
                return run();
            }
            catch (Exception ex)
            {
                AutoLogger.LogError(ex, "RunAction");
                return Fail(ex.Message, ex.StackTrace);
            }
        }

        public static MessageContract<T> RunAction<T>(this object obj, Func<MessageContract<T>> run)
        {
            try
            {
                return run();
            }
            catch (Exception ex)
            {
                return MessageContract<T>.Fail(ex.Message, ex.StackTrace);
            }
        }
    }
}
