using System;

namespace ResultOf
{
    public static class Result
    {
        public static Result<T> Ok<T>(T value)
        {
            return new Result<T>(null, value);
        }

        public static Result<None> Ok()
        {
            return new Result<None>(null);
        }

        public static Result<T> Fail<T>(IError error)
        {
            return new Result<T>(error);
        }

        public static Result<T> AsResult<T>(this T value)
        {
            return Ok(value);
        }

        public static Result<None> AsNoneResult<T>(this Result<T> result)
        {
            return new Result<None>(result.Error);
        }

        public static Result<T> RefineError<T>(this Result<T> result, Func<IError, IError> errorTransformer)
        {
            if (result.IsSuccess)
                return result;
            return Fail<T>(errorTransformer(result.Error));
        }

        public static Result<TOut> Then<TIn, TOut>(this Result<TIn> result, Func<TIn, Result<TOut>> transform)
        {
            return result.IsSuccess ? transform(result.Value) : Fail<TOut>(result.Error);
        }

        public static Result<TOut> Then<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> transform)
        {
            return result.Then(value =>
            {
                try
                {
                    return transform(value).AsResult();
                }
                catch (Exception exception)
                {
                    return Fail<TOut>(new Error(exception.Message));
                }
            });
        }

        public static Result<T> Then<T>(this Result<T> result, Action<T> action)
        {
            return result.Then(value =>
            {
                action(value);
                return value;
            });
        }

        public static Result<T> OnFail<T>(this Result<T> result, Action<IError> failureAction)
        {
            if (!result.IsSuccess)
                failureAction(result.Error);
            return result;
        }

        public static Result<TOut> PassErrorThrough<TIn, TOut>(this Result<TIn> first, Result<TOut> second)
        {
            if (first.IsSuccess)
                return second;
            if (second.IsSuccess)
                return Fail<TOut>(first.Error);
            return Fail<TOut>(first.Error.ConcatWithChain(second.Error));
        }
    }
}