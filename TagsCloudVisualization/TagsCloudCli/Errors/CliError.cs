using ResultOf;

namespace TagsCloudCli.Errors
{
    public abstract class CliError : IError
    {
        public string Message { get; }
        public IError InnerError { get; set; }

        protected CliError(string message, IError innerError = null)
        {
            Message = message;
            InnerError = innerError;
        }
    }
}