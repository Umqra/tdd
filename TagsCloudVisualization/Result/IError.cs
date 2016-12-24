namespace ResultOf
{
    public interface IError
    {
        string Message { get; }
        IError InnerError { get; }
    }
}