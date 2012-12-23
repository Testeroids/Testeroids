// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MoqExtensions.cs" company="Testeroids">
//   © 2012 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Testeroids
{
    using System;
    using System.Linq.Expressions;

    using Moq;

    /// <summary>
    /// Extensions to the <see cref="IMock{T}"/> type related to Testeroids.
    /// </summary>
    public static class MoqExtensions
    {
        #region Public Methods and Operators

        /// <summary>
        ///   Method used to verify that a method is called uniquely once during Because.
        /// </summary>
        /// <param name="mock"> The mock instance on which the method must be run. </param>
        /// <param name="contextSpecification">
        /// The context/specification instance which is running the test (normally "this").
        /// </param>
        /// <param name="verifyMethod"> The method whose call number must be check. </param>
        /// <typeparam name="T"> Type of the mock. </typeparam>
        public static void VerifyCalledOnceDuringBecause<T>(
            this IMock<T> mock,
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
        public static void VerifyNumberOfCallsDuringBecause<T>(
            this IMock<T> mock,
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