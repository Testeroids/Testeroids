namespace Testeroids
{
    using System;
    using System.Linq.Expressions;

    using JetBrains.Annotations;

    using Moq;

    /// <summary>
    /// Extensions to the <see cref="ITesteroidsMock{T}"/> type related to Testeroids.
    /// </summary>
    public static class MockExtensions
    {
        #region Public Methods and Operators

        /// <summary>
        /// Sets up <see cref="IEquatable{T}"/> on passed mock and returns it.
        /// </summary>
        /// <typeparam name="T">The mocked object's type.</typeparam>
        /// <param name="mock">The mock to setup as <see cref="IEquatable{T}"/>.</param>
        /// <returns>Returns the <paramref name="mock"/> implementing an additional interface, <see cref="IEquatable{T}"/>.</returns>
        public static ITesteroidsMock<T> AsEquatable<T>(this ITesteroidsMock<T> mock) where T : class
        {
            mock.As<IEquatable<T>>()
                .Setup(o => o.Equals(It.IsAny<T>()))
                .Returns<T>(o => object.ReferenceEquals(mock.Object, o));

            return mock;
        }

        /// <summary>
        ///   Method used to verify that a method is called uniquely once during Because.
        /// </summary>
        /// <param name="mock"> The mock instance on which the method must be run. </param>
        /// <param name="contextSpecification">
        /// The context/specification instance which is running the test (normally "this").
        /// </param>
        /// <param name="verifyMethod"> The method whose call number must be check. </param>
        /// <typeparam name="T"> Type of the mock. </typeparam>
        [PublicAPI]
        [Obsolete("Testeroids now automatically resets the calls just before calling the Because() method.")]
        public static void VerifyCalledOnceDuringBecause<T>(
            this ITesteroidsMock<T> mock, 
            ContextSpecificationBase contextSpecification, 
            Expression<Action<T>> verifyMethod) where T : class
        {
            mock.VerifyNumberOfCallsDuringBecause(contextSpecification, verifyMethod, Times.Never(), Times.Once());
        }

        /// <summary>
        /// Method used to verify the number of times a method is called during Because.
        /// </summary>
        /// <param name="mock">
        /// The mock instance on which the method must be run. 
        /// </param>
        /// <param name="contextSpecification">
        /// The context/specification instance which is running the test (normally "this").
        /// </param>
        /// <param name="expression">
        /// The method whose call number must be check. 
        /// </param>
        /// <param name="numberBefore">
        /// The number the method has been called before Because. 
        /// </param>
        /// <param name="numberAfter">
        /// The number the method has been called after Because. 
        /// </param>
        /// <typeparam name="T">
        /// Type of the mock. 
        /// </typeparam>
        [PublicAPI]
        [Obsolete("Testeroids now automatically resets the calls just before calling the Because() method.")]
        public static void VerifyNumberOfCallsDuringBecause<T>(
            this ITesteroidsMock<T> mock, 
            ContextSpecificationBase contextSpecification, 
            Expression<Action<T>> expression, 
            Times numberBefore, 
            Times numberAfter) where T : class
        {
            mock.Verify(expression, numberBefore);

            contextSpecification.Because();

            mock.Verify(expression, numberAfter);
        }

        #endregion
    }
}