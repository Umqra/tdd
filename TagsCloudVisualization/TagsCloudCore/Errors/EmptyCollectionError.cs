using ResultOf;

namespace TagsCloudCore.Errors
{
    public class NoRectanglesError : ITagsCloudCoreError
    {
        public string Message { get; }
        public IError InnerError { get; }

        public NoRectanglesError(string message, IError innerError = null)
        {
            Message = message;
            InnerError = innerError;
        }
    }
}