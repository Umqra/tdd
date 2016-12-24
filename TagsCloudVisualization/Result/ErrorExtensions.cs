using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResultOf
{
    public static class ErrorExtensions
    {
        public static IEnumerable<string> GenerateTrace(this IError error)
        {
            while (error != null)
            {
                yield return error.Message;
                error = error.InnerError;
            }
        }

        public static IError RootError(this IError error)
        {
            while (error.InnerError != null)
                error = error.InnerError;
            return error;
        }

        public static IError ConcatWithChain(this IError first, IError second)
        {
            second.RootError().InnerError = first;
            return second;
        }

        public static bool Is<T>(this IError error) where T:IError
        {
            return error.GetType() == typeof(T);
        }
    }
}
