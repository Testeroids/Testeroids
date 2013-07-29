// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MoqSetupGetterWrapper.cs" company="Testeroids">
//   © 2012-2013 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Testeroids.Mocking
{
    using System;
    using System.Linq.Expressions;

    using Moq.Language.Flow;

    internal class MoqSetupGetterWrapper<TMock, TProperty> : IVerifiesInternals, 
                                                             Moq.Language.Flow.ISetupGetter<TMock, TProperty>
        where TMock : class
    {
        #region Fields

        private readonly ISetupGetter<TMock, TProperty> wrappedSetupGetter;

        #endregion

        #region Constructors and Destructors

        public MoqSetupGetterWrapper(ISetupGetter<TMock, TProperty> wrappedSetupGetter, 
                                     LambdaExpression expression, 
                                     IVerifiedMock testeroidsMock)
        {
            this.Expression = expression;
            this.TesteroidsMock = testeroidsMock;
            this.wrappedSetupGetter = wrappedSetupGetter;
        }

        #endregion

        #region Public Properties

        public LambdaExpression Expression { get; set; }

        public IVerifiedMock TesteroidsMock { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <inheritdoc/>
        public IReturnsThrowsGetter<TMock, TProperty> Callback(Action action)
        {
            return this.wrappedSetupGetter.Callback(action);
        }

        /// <inheritdoc/>
        public IReturnsResult<TMock> Returns(TProperty value)
        {
            var returnsResult = this.wrappedSetupGetter.Returns(value);
            return new MoqReturnsResultWrapper<TMock>(this.Expression, returnsResult, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        public IReturnsResult<TMock> Returns(Func<TProperty> valueFunction)
        {
            return this.wrappedSetupGetter.Returns(valueFunction);
        }

        /// <inheritdoc/>
        public IThrowsResult Throws(Exception exception)
        {
            var returnsThrows = this.wrappedSetupGetter.Throws(exception);
            return new MoqThrowsResult(this.Expression, returnsThrows, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        public IThrowsResult Throws<TException>() where TException : Exception, new()
        {
            var returnsThrows = this.wrappedSetupGetter.Throws<TException>();
            return new MoqThrowsResult(this.Expression, returnsThrows, this.TesteroidsMock);
        }

        /// <inheritdoc/>
        public void Verifiable()
        {
            this.wrappedSetupGetter.Verifiable();
        }

        /// <inheritdoc/>
        public void Verifiable(string failMessage)
        {
            this.wrappedSetupGetter.Verifiable(failMessage);
        }

        #endregion
    }
}