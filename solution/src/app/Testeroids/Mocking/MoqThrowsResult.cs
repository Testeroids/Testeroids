// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MoqThrowsResult.cs" company="Testeroids">
//   © 2012-2013 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Testeroids.Mocking
{
    using System;
    using System.ComponentModel;
    using System.Linq.Expressions;

    using Moq.Language;
    using Moq.Language.Flow;

    internal class MoqThrowsResult : IThrowsResult, 
                                     IVerifiesInternals
    {
        #region Fields

        private readonly IThrowsResult throwsResult;

        #endregion

        #region Constructors and Destructors

        public MoqThrowsResult(LambdaExpression expression, 
                               IThrowsResult throwsResult, 
                               IVerifiedMock testeroidsMock)
        {
            this.Expression = expression;
            this.throwsResult = throwsResult;
            this.TesteroidsMock = testeroidsMock;
        }

        #endregion

        #region Public Properties

        public LambdaExpression Expression { get; set; }

        public IVerifiedMock TesteroidsMock { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The expected invocation can happen at most specified number of times.
        /// </summary>
        /// <param name="callCount">The number of times to accept calls.</param>
        /// <example>
        /// <code>
        /// var mock = new Mock&lt;ICommand&gt;();
        ///             mock.Setup(foo =&gt; foo.Execute("ping"))
        ///                 .AtMost( 5 );
        /// </code>
        /// </example>
        [Obsolete("To verify this condition, use the overload to Verify that receives Times.AtMost(callCount).")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public IVerifies AtMost(int callCount)
        {
            return this.throwsResult.AtMost(callCount);
        }

        /// <summary>
        /// The expected invocation can happen at most once.
        /// </summary>
        /// <example>
        /// <code>
        /// var mock = new Mock&lt;ICommand&gt;();
        ///             mock.Setup(foo =&gt; foo.Execute("ping"))
        ///                 .AtMostOnce();
        /// </code>
        /// </example>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("To verify this condition, use the overload to Verify that receives Times.AtMostOnce().")]
        public IVerifies AtMostOnce()
        {
            return this.throwsResult.AtMostOnce();
        }

        /// <summary>
        /// Marks the expectation as verifiable, meaning that a call 
        ///             to <see cref="M:Moq.Mock.Verify"/> will check if this particular 
        ///             expectation was met.
        /// </summary>
        /// <example>
        /// The following example marks the expectation as verifiable:
        /// <code>
        /// mock.Expect(x =&gt; x.Execute("ping"))
        ///                 .Returns(true)
        ///                 .Verifiable();
        /// </code>
        /// </example>
        public void Verifiable()
        {
            this.throwsResult.Verifiable();
        }

        /// <summary>
        /// Marks the expectation as verifiable, meaning that a call 
        ///             to <see cref="M:Moq.Mock.Verify"/> will check if this particular 
        ///             expectation was met, and specifies a message for failures.
        /// </summary>
        /// <example>
        /// The following example marks the expectation as verifiable:
        /// <code>
        /// mock.Expect(x =&gt; x.Execute("ping"))
        ///                 .Returns(true)
        ///                 .Verifiable("Ping should be executed always!");
        /// </code>
        /// </example>
        public void Verifiable(string failMessage)
        {
            this.throwsResult.Verifiable(failMessage);
        }

        #endregion
    }
}