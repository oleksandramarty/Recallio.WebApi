﻿using System.Diagnostics;
using System.Text.Json;
using Recallio.Kernel.Exceptions;

namespace Recallio.Kernel.Extensions;

public static class ExceptionExtension
{
            public static string Serialize(this object data)
        {
            return JsonSerializer.Serialize(data);
        }

        public static string GetError(this Exception e)
        {
            return e.InnerException != null ? $"{e.Message} {e.InnerException.Message}" : e.Message;
        }

        public static string GetExceptionDetails(this Exception e)
        {
            var properties = e.GetType()
                .GetProperties();

            var fields = properties
                .Select(property => new
                {
                    Name = property.Name,
                    Value = property.GetValue(e, null)
                })
                .Select(x => String.Format(
                    "{0} = {1}",
                    x.Name,
                    x.Value != null ? x.Value.ToString() : String.Empty
                ));
            return String.Join("\n", fields);
        }

        public static string GetControllerName(this Exception e)
        {
            try
            {
                var st = new StackTrace(e, true);
                var frame = st.GetFrame(0);

                var path = frame.GetFileName();
                return path.Contains("/") ? path.Split("/").LastOrDefault() : path.Split("\\").LastOrDefault();
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static string GetMethodName(this Exception e)
        {
            var controller = e.GetControllerName().Split(".").FirstOrDefault();
            var details = e.GetExceptionDetails();
            var index = details.IndexOf(controller) + controller.Length + 1;

            var result = "";
            while (details[index] != '(')
            {
                result += details[index];
                index++;
            }

            return result;
        }

        public static int GetLineNumber(this Exception e)
        {
            var st = new StackTrace(e, true);

            var frame = st.GetFrame(0);
            return frame.GetFileLineNumber();
        }

        public static bool IsLoggerException(this Exception e)
        {
            return e is LoggerException loggerException;
        }

        public static LoggerException ToLoggerException(this Exception e)
        {
            return (LoggerException) e;
        }

        public static int GetChildCode(this int code, Exception e)
        {
            if (e.IsLoggerException())
            {
                var temp = e.ToLoggerException().statusCode;
                return code == temp ? code : temp;
            }

            return code;
        }
}