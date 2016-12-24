using System;

namespace Result
{
    public static class Result
    {
        public static Result<T> Ok<T>(T value)
        {
            return new Result<T>(null, value);
        }

        public static Result<T> Fail<T>(IError error)
        {
            return new Result<T>(error);
        }

        public static Result<T> ReplaceError<T>(this Result<T> result, IError newError)
        {
            if (result.IsSuccess)
                return result;
            return Fail<T>(newError);
        }

        public static Result<T> RefineError<T>(this Result<T> result, Func<IError, IError> errorTransformer)
        {
            if (result.IsSuccess)
                return result;
            return Fail<T>(errorTransformer(result.Error));
        }

        public static Result<TOut> Then<TIn, TOut>(this Result<TIn> first, Func<TIn, Result<TOut>> transform)
        {
            return first.IsSuccess ? transform(first.Value) : Fail<TOut>(first.Error);
        }

        public static Result<TOut> SelectMany<TIn, TMid, TOut>(this Result<TIn> first, Func<TIn, Result<TMid>> selector,
            Func<TIn, TMid, TOut> combiner)
        {
            var value = first.Then(selector);
            return value.IsSuccess ? Ok(combiner(first.Value, value.Value)) : Fail<TOut>(value.Error);
        }

        public static Result<TOut> Select<TIn, TOut>(this Result<TIn> result, Func<TIn, Result<TOut>> transform)
        {
            return result.Then(transform);
        }
    }
}