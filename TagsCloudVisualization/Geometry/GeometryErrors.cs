using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ResultOf;

namespace Geometry
{
    public class DegenerateError : IError
    {
        public string Message { get; }
        public IError InnerError { get; set; }

        public DegenerateError(string message, IError error = null)
        {
            Message = message;
            InnerError = error;
        }
    }
}
