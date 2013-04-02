namespace Testeroids.Mocking
{
    using System;

    using Moq.Language.Flow;

    internal class MoqReturnsThrowsGetterWrapper<T, TResult> : Moq.Language.Flow.IReturnsThrowsGetter<T, TResult>
        where T : class

    {
        private readonly IReturnsThrowsGetter<T, TResult> wrappedReturnsThrows;
                
        public MoqReturnsThrowsGetterWrapper(Moq.Language.Flow.IReturnsThrowsGetter<T, TResult> wrappedReturnsThrows)
        {
            this.wrappedReturnsThrows = wrappedReturnsThrows;            
        }

        /// <inheritdoc/>
        public Moq.Language.Flow.IReturnsResult<T> Returns(TResult value)
        {
            return this.wrappedReturnsThrows.Returns(value);
        }

        /// <inheritdoc/>
        public Moq.Language.Flow.IReturnsResult<T> Returns(Func<TResult> valueFunction)
        {
            return this.wrappedReturnsThrows.Returns(valueFunction);
        }       

        /// <inheritdoc/>
        public IThrowsResult Throws(Exception exception)
        {
            return this.wrappedReturnsThrows.Throws(exception);
        }

        /// <inheritdoc/>
        public IThrowsResult Throws<TException>() where TException : Exception, new()
        {
            return this.wrappedReturnsThrows.Throws<TException>();
        }
    }
}