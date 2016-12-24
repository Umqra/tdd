using ResultOf;

namespace TagsCloudCore.Errors
{
    public class NoRectanglesError : TagsCloudCoreError
    {
        public NoRectanglesError(string message, IError error = null) : base(message, error)
        {
        }
    }
}