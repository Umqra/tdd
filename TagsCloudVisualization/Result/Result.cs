using System;

namespace ResultOf
{
    public struct Result<T>
    {
        internal T Value { get; }
        public IError Error { get; }

        internal Result(IError error, T value = default(T))
        {
            Error = error;
            Value = value;
        }

        public bool IsSuccess => Error == null;
    }
}