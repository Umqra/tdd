using ResultOf;

namespace TagsCloudCore.Errors
{
    public abstract class TagsCloudCoreError : IError
    {
        public string Message { get; }
        public IError InnerError { get; set; }

        protected TagsCloudCoreError(string message, IError error = null)
        {
            Message = message;
            InnerError = error;
        }
    }
}
