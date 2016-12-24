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
            var messages = new List<string>();
            while (error != null)
            {
                messages.Add(error.Message);
                error = error.InnerError;
            }
            return Enumerable.Reverse(messages);
        }
    }
}
