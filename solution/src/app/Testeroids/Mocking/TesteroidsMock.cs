// --------------------------------------------------------------------------------------------------------------------
// <copyright company="Testeroids" file="TesteroidsMock.cs">
//   © 2012-2013 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Testeroids.Mocking
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    using JetBrains.Annotations;

    using Moq;
    using Moq.Language;
    using Moq.Language.Flow;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Will add documentation from base interface once that is finished.")]
    internal class TesteroidsMock : IMock, 
                                    IMockInternals
    {
        #region Fields

        private readonly Mock nakedMock;

        #endregion

        #region Constructors and Destructors

        [PublicAPI]
        public TesteroidsMock(Mock nakedMock)
        {
            this.nakedMock = nakedMock;
        }

        #endregion

        #region Public Properties

        public MockBehavior Behavior
        {
            get
            {
                return this.nakedMock.Behavior;
            }
        }

        public bool CallBase
        {
            get
            {
                return this.nakedMock.CallBase;
            }

            set
            {
                this.nakedMock.CallBase = value;
            }
        }

        public DefaultValue DefaultValue
        {
            get
            {
                return this.nakedMock.DefaultValue;
            }

            set
            {
                this.nakedMock.DefaultValue = value;
            }
        }

        public object Object
        {
            get
            {
                return this.nakedMock.Object;
            }
        }

        #endregion

        #region Explicit Interface Properties

        /// <summary>
        /// Gets a list of all the members which were set up and a <see cref="bool"/> indicating if the given method was verified through a call to <see cref="IMock.Verify"/>.
        /// </summary>
        IEnumerable<Tuple<MemberInfo, bool>> IMockInternals.VerifiedSetups
        {
            get
            {
                return this.GetVerifiedSetups();
            }
        }

        #endregion

        #region Properties

        protected Mock NakedMock
        {
            get
            {
                return this.nakedMock;
            }
        }

        #endregion

        #region Public Methods and Operators

        public IMock<TInterface> As<TInterface>() where TInterface : class
        {
            return new TesteroidsMock<TInterface>(this.nakedMock.As<TInterface>());
        }

        /// <summary>
        /// Reset the counts of all the method calls done previously.
        /// </summary>
        public void ResetAllCallCounts()
        {
            this.NakedMock.ResetAllCalls();
        }

        public void SetReturnsDefault<TReturn>(TReturn value)
        {
            this.nakedMock.SetReturnsDefault(value);
        }

        public void Verify()
        {
            try
            {
                this.nakedMock.Verify();
            }
            catch (MockException e)
            {
                throw new MockSetupMethodNeverUsedException(e);
            }
        }

        public void VerifyAll()
        {
            try
            {
                this.nakedMock.VerifyAll();
            }
            catch (MockException e)
            {
                throw new MockSetupMethodNeverUsedException(e);
            }
        }

        #endregion

        #region Methods

        protected virtual IEnumerable<Tuple<MemberInfo, bool>> GetVerifiedSetups()
        {
            return Enumerable.Empty<Tuple<MemberInfo, bool>>();
        }

        #endregion
    }

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Will add documentation from base interface once that is finished.")]
    internal class TesteroidsMock<T> : TesteroidsMock, 
                                       IMock<T>, 
                                       IVerifiedMock
        where T : class
    {
        #region Fields

        private readonly Mock<T> nakedTypedMock;

        /// <summary>
        /// The dictionary of all registered Setup methods. The <see cref="bool"/> value indicates whether the Setup call has been matched with a <see cref="IMock{T}.Verify(System.Linq.Expressions.Expression{System.Action{T}})"/> call.
        /// </summary>
        private readonly Dictionary<MemberInfo, bool> registeredSetups = new Dictionary<MemberInfo, bool>();

        #endregion

        #region Constructors and Destructors

        public TesteroidsMock()
            : base(new Moq.Mock<T>(MockBehavior.Strict))
        {
            this.nakedTypedMock = (Moq.Mock<T>)this.NakedMock;
        }

        internal TesteroidsMock(Mock<T> nakedMock)
            : base(nakedMock)
        {
            this.nakedTypedMock = nakedMock;
        }

        #endregion

        #region Public Properties

        public new T Object
        {
            get
            {
                return this.nakedTypedMock.Object;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Unregisters the specified expression in order to ignore it in the sanity check which makes sure there is a call verification unit test for each setup.
        /// </summary>
        /// <param name="expression"></param>
        public void UnregisterSetupForVerification(LambdaExpression expression)
        {
            var memberInfo = GetMemberInfoFromExpression(expression);
            this.registeredSetups.Remove(memberInfo);
        }

        #endregion

        #region Explicit Interface Methods

        /// <inheritdoc />
        void IMock<T>.Raise(
            Action<T> eventExpression, 
            EventArgs args)
        {
            this.nakedTypedMock.Raise(eventExpression, args);
        }

        /// <inheritdoc />
        void IMock<T>.Raise(
            Action<T> eventExpression, 
            params object[] args)
        {
            this.nakedTypedMock.Raise(eventExpression, args);
        }

        /// <inheritdoc />
        ISetup<T> IMock<T>.Setup(Expression<Action<T>> expression)
        {
            var setup = this.nakedTypedMock.Setup(expression);

            this.RegisterSetupForVerification(expression);

            // todo : Use a factory to make it testable (?).
            var moqWrappedSetup = new MoqSetupWrapper<T>(setup, expression, this);

            return moqWrappedSetup;
        }

        /// <inheritdoc />
        ISetup<T, TResult> IMock<T>.Setup<TResult>(Expression<Func<T, TResult>> expression)
        {
            var setup = this.nakedTypedMock.Setup(expression);

            this.RegisterSetupForVerification(expression);

            // todo : Use a factory to make it testable (?).
            var moqWrappedSetup = new MoqSetupWrapper<T, TResult>(setup, expression, this);

            return moqWrappedSetup;
        }

        /// <inheritdoc />
        ISetupGetter<T, TProperty> IMock<T>.SetupGet<TProperty>(Expression<Func<T, TProperty>> expression)
        {
            var setupGetter = this.nakedTypedMock.SetupGet(expression);

            this.RegisterSetupForVerification(expression);

            // todo : Use a factory to make it testable (?).
            var moqWrappedSetupGet = new MoqSetupGetterWrapper<T, TProperty>(setupGetter, expression, this);

            return moqWrappedSetupGet;
        }

        /// <inheritdoc />
        IMock<T> IMock<T>.SetupProperty<TProperty>(Expression<Func<T, TProperty>> property)
        {
            this.nakedTypedMock.SetupProperty(property);

            return this;
        }

        /// <inheritdoc />
        IMock<T> IMock<T>.SetupProperty<TProperty>(
            Expression<Func<T, TProperty>> property, 
            TProperty initialValue)
        {
            this.nakedTypedMock.SetupProperty(property, initialValue);

            return this;
        }

        /// <inheritdoc />
        ISetup<T> IMock<T>.SetupSet(Action<T> setterExpression)
        {
            var setupSet = this.nakedTypedMock.SetupSet(setterExpression);

            return setupSet;
        }

        /// <inheritdoc />
        ISetupSetter<T, TProperty> IMock<T>.SetupSet<TProperty>(Action<T> setterExpression)
        {
            var setupSetter = this.nakedTypedMock.SetupSet<TProperty>(setterExpression);

            return setupSetter;
        }

        /// <inheritdoc />
        void IMock<T>.Verify<TResult>(Expression<Func<T, TResult>> expression)
        {
            this.MarkSetUpExpressionAsMatchedByVerifyCall(expression);

            this.nakedTypedMock.Verify(expression);
        }

        /// <inheritdoc />
        void IMock<T>.Verify<TResult>(
            Expression<Func<T, TResult>> expression, 
            Times times)
        {
            this.MarkSetUpExpressionAsMatchedByVerifyCall(expression);

            this.nakedTypedMock.Verify(expression, times);
        }

        /// <inheritdoc />
        void IMock<T>.Verify<TResult>(
            Expression<Func<T, TResult>> expression, 
            string failMessage)
        {
            this.MarkSetUpExpressionAsMatchedByVerifyCall(expression);

            this.nakedTypedMock.Verify(expression, failMessage);
        }

        /// <inheritdoc />
        void IMock<T>.Verify<TResult>(
            Expression<Func<T, TResult>> expression, 
            Times times, 
            string failMessage)
        {
            this.MarkSetUpExpressionAsMatchedByVerifyCall(expression);

            this.nakedTypedMock.Verify(expression, times, failMessage);
        }

        /// <inheritdoc />
        void IMock<T>.Verify(Expression<Action<T>> expression)
        {
            this.MarkSetUpExpressionAsMatchedByVerifyCall(expression);

            this.nakedTypedMock.Verify(expression);
        }

        /// <inheritdoc />
        void IMock<T>.Verify(
            Expression<Action<T>> expression, 
            Times times)
        {
            this.MarkSetUpExpressionAsMatchedByVerifyCall(expression);

            this.nakedTypedMock.Verify(expression, times);
        }

        /// <inheritdoc />
        void IMock<T>.Verify(
            Expression<Action<T>> expression, 
            string failMessage)
        {
            this.MarkSetUpExpressionAsMatchedByVerifyCall(expression);

            this.nakedTypedMock.Verify(expression, failMessage);
        }

        /// <inheritdoc />
        void IMock<T>.Verify(
            Expression<Action<T>> expression, 
            Times times, 
            string failMessage)
        {
            this.MarkSetUpExpressionAsMatchedByVerifyCall(expression);

            this.nakedTypedMock.Verify(expression, times, failMessage);
        }

        /// <inheritdoc />
        void IMock<T>.VerifyGet<TProperty>(Expression<Func<T, TProperty>> expression)
        {
            this.MarkSetUpExpressionAsMatchedByVerifyCall(expression);

            this.nakedTypedMock.VerifyGet(expression);
        }

        /// <inheritdoc />
        void IMock<T>.VerifyGet<TProperty>(
            Expression<Func<T, TProperty>> expression, 
            Times times)
        {
            this.MarkSetUpExpressionAsMatchedByVerifyCall(expression);

            this.nakedTypedMock.VerifyGet(expression, times);
        }

        /// <inheritdoc />
        void IMock<T>.VerifyGet<TProperty>(
            Expression<Func<T, TProperty>> expression, 
            string failMessage)
        {
            this.MarkSetUpExpressionAsMatchedByVerifyCall(expression);

            this.nakedTypedMock.VerifyGet(expression, failMessage);
        }

        /// <inheritdoc />
        void IMock<T>.VerifyGet<TProperty>(
            Expression<Func<T, TProperty>> expression, 
            Times times, 
            string failMessage)
        {
            this.MarkSetUpExpressionAsMatchedByVerifyCall(expression);

            this.nakedTypedMock.VerifyGet(expression, times, failMessage);
        }

        /// <inheritdoc />
        void IMock<T>.VerifySet(Action<T> setterExpression)
        {
            this.nakedTypedMock.VerifySet(setterExpression);
        }

        /// <inheritdoc />
        void IMock<T>.VerifySet(
            Action<T> setterExpression, 
            Times times)
        {
            this.nakedTypedMock.VerifySet(setterExpression, times);
        }

        void IMock<T>.VerifySet(
            Action<T> setterExpression, 
            string failMessage)
        {
            this.nakedTypedMock.VerifySet(setterExpression, failMessage);
        }

        /// <inheritdoc />
        void IMock<T>.VerifySet(
            Action<T> setterExpression, 
            Times times, 
            string failMessage)
        {
            this.nakedTypedMock.VerifySet(setterExpression, times, failMessage);
        }

        /// <inheritdoc />
        ISetupConditionResult<T> IMock<T>.When(Func<bool> condition)
        {
            return this.nakedTypedMock.When(condition);
        }

        #endregion

        #region Methods

        protected override IEnumerable<Tuple<MemberInfo, bool>> GetVerifiedSetups()
        {
            return this.registeredSetups.Select(x => new Tuple<MemberInfo, bool>(x.Key, x.Value));
        }

        private static MemberInfo GetMemberInfoFromExpression(LambdaExpression expression)
        {
            return expression.Body is MethodCallExpression
                       ? ((MethodCallExpression)expression.Body).Method
                       : ((MemberExpression)expression.Body).Member;
        }

        private void MarkSetUpExpressionAsMatchedByVerifyCall(LambdaExpression expression)
        {
            var memberInfo = GetMemberInfoFromExpression(expression);
            this.registeredSetups[memberInfo] = true;
        }

        private void RegisterSetupForVerification(LambdaExpression expression)
        {
            var memberInfo = GetMemberInfoFromExpression(expression);
            if (!this.registeredSetups.ContainsKey(memberInfo))
            {
                this.registeredSetups[memberInfo] = false;
            }
        }

        #endregion
    }
}