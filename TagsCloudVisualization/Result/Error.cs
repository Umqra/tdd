namespace ResultOf
{
    public class Error : IError
    {
        public string Message { get; }
        public IError InnerError { get; }

        public Error(string message, IError innerError = null)
        {
            Message = message;
            InnerError = innerError;
        }
    }
}