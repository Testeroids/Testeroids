// --------------------------------------------------------------------------------------------------------------------
// <copyright company="Testeroids" file="TesteroidsMock.cs">
//   © 2012 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Testeroids.Mocking
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    using Moq;
    using Moq.Language;
    using Moq.Language.Flow;

    internal class TesteroidsMock : IMock, IMockInternals
    {
        #region Fields

        private readonly Moq.Mock nakedMock;

        #endregion

        #region Constructors and Destructors

        public TesteroidsMock(Moq.Mock nakedMock)
        {
            this.nakedMock = nakedMock;
        }

        #endregion

        #region Public Properties

        public MockBehavior Behavior
        {
            get { return this.nakedMock.Behavior; }
        }

        public bool CallBase
        {
            get { return this.nakedMock.CallBase; }
            set { this.nakedMock.CallBase = value; }
        }

        public DefaultValue DefaultValue
        {
            get { return this.nakedMock.DefaultValue; }
            set { this.nakedMock.DefaultValue = value; }
        }

        public object Object
        {
            get { return this.nakedMock.Object; }
        }

        #endregion

        #region Explicit Interface Properties

        /// <summary>
        /// Gets a list of all the members which were set up and a <see cref="bool"/> indicating if the given method was verified through a call to <see cref="IMock.Verify"/>.
        /// </summary>
        IEnumerable<Tuple<MemberInfo, bool>> IMockInternals.VerifiedSetups
        {
            get { return this.GetVerifiedSetups(); }
        }

        #endregion

        #region Properties

        protected Moq.Mock NakedMock
        {
            get { return this.nakedMock; }
        }

        #endregion

        #region Public Methods and Operators

        public IMock<TInterface> As<TInterface>() where TInterface : class
        {
            return new TesteroidsMock<TInterface>(this.nakedMock.As<TInterface>());
        }

        public void SetReturnsDefault<TReturn>(TReturn value)
        {
            this.nakedMock.SetReturnsDefault(value);
        }

        public void Verify()
        {
            this.nakedMock.Verify();
        }

        public virtual void VerifyAll()
        {
            this.nakedMock.VerifyAll();
        }

        #endregion

        #region Methods

        protected virtual IEnumerable<Tuple<MemberInfo, bool>> GetVerifiedSetups()
        {
            return Enumerable.Empty<Tuple<MemberInfo, bool>>();
        }

        #endregion
    }

    internal class TesteroidsMock<T> : TesteroidsMock, IMock<T>
        where T : class
    {
        #region Fields

        private readonly Moq.Mock<T> nakedTypedMock;

        private readonly Dictionary<MemberInfo, bool> registeredSetups = new Dictionary<MemberInfo, bool>();

        #endregion

        #region Constructors and Destructors

        public TesteroidsMock()
            : base(new Moq.Mock<T>(MockBehavior.Strict))
        {
            this.nakedTypedMock = (Moq.Mock<T>)this.NakedMock;
        }

        internal TesteroidsMock(Moq.Mock<T> nakedMock)
            : base(nakedMock)
        {
            this.nakedTypedMock = nakedMock;
        }

        #endregion

        #region Public Properties

        public new T Object
        {
            get { return this.nakedTypedMock.Object; }
        }

        #endregion

        #region Public Methods and Operators

        public void Raise(Action<T> eventExpression, EventArgs args)
        {
            this.nakedTypedMock.Raise(eventExpression, args);
        }

        public void Raise(Action<T> eventExpression, params object[] args)
        {
            this.nakedTypedMock.Raise(eventExpression, args);
        }

        public ISetup<T> Setup(Expression<Action<T>> expression)
        {
            this.RegisterSetupForVerification(expression);

            return this.nakedTypedMock.Setup(expression);
        }

        public ISetup<T, TResult> Setup<TResult>(Expression<Func<T, TResult>> expression)
        {
            this.RegisterSetupForVerification(expression);

            return this.nakedTypedMock.Setup(expression);
        }

        public ISetupGetter<T, TProperty> SetupGet<TProperty>(Expression<Func<T, TProperty>> expression)
        {
            this.RegisterSetupForVerification(expression);

            return this.nakedTypedMock.SetupGet(expression);
        }

        public IMock<T> SetupProperty<TProperty>(Expression<Func<T, TProperty>> property)
        {
            this.RegisterSetupForVerification(property);

            this.nakedTypedMock.SetupProperty(property);

            return this;
        }

        public IMock<T> SetupProperty<TProperty>(Expression<Func<T, TProperty>> property, TProperty initialValue)
        {
            this.RegisterSetupForVerification(property);

            this.nakedTypedMock.SetupProperty(property, initialValue);

            return this;
        }

        public ISetupSetter<T, TProperty> SetupSet<TProperty>(Action<T> setterExpression)
        {
            return this.nakedTypedMock.SetupSet<TProperty>(setterExpression);
        }

        public ISetup<T> SetupSet(Action<T> setterExpression)
        {
            return this.nakedTypedMock.SetupSet(setterExpression);
        }

        public void Verify(Expression<Action<T>> expression)
        {
            this.nakedTypedMock.Verify(expression);
        }

        public void Verify(Expression<Action<T>> expression, Times times)
        {
            this.nakedTypedMock.Verify(expression, times);
        }

        public void Verify(Expression<Action<T>> expression, string failMessage)
        {
            this.nakedTypedMock.Verify(expression, failMessage);
        }

        public void Verify(Expression<Action<T>> expression, Times times, string failMessage)
        {
            this.nakedTypedMock.Verify(expression, times, failMessage);
        }

        public void Verify<TResult>(Expression<Func<T, TResult>> expression)
        {
            this.MarkSetUpMethodAsVerified(expression);

            this.nakedTypedMock.Verify(expression);
        }

        public void Verify<TResult>(Expression<Func<T, TResult>> expression, Times times)
        {
            this.MarkSetUpMethodAsVerified(expression);

            this.nakedTypedMock.Verify(expression, times);
        }

        public void Verify<TResult>(Expression<Func<T, TResult>> expression, string failMessage)
        {
            this.MarkSetUpMethodAsVerified(expression);

            this.nakedTypedMock.Verify(expression, failMessage);
        }

        public void Verify<TResult>(Expression<Func<T, TResult>> expression, Times times, string failMessage)
        {
            this.MarkSetUpMethodAsVerified(expression);

            this.nakedTypedMock.Verify(expression, times, failMessage);
        }

        public void VerifyGet<TProperty>(Expression<Func<T, TProperty>> expression)
        {
            this.MarkSetUpMethodAsVerified(expression);

            this.nakedTypedMock.VerifyGet(expression);
        }

        public void VerifyGet<TProperty>(Expression<Func<T, TProperty>> expression, Times times)
        {
            this.MarkSetUpMethodAsVerified(expression);

            this.nakedTypedMock.VerifyGet(expression, times);
        }

        public void VerifyGet<TProperty>(Expression<Func<T, TProperty>> expression, string failMessage)
        {
            this.MarkSetUpMethodAsVerified(expression);

            this.nakedTypedMock.VerifyGet(expression, failMessage);
        }

        public void VerifyGet<TProperty>(Expression<Func<T, TProperty>> expression, Times times, string failMessage)
        {
            this.MarkSetUpMethodAsVerified(expression);

            this.nakedTypedMock.VerifyGet(expression, times, failMessage);
        }

        public void VerifySet(Action<T> setterExpression)
        {
            this.nakedTypedMock.VerifySet(setterExpression);
        }

        public void VerifySet(Action<T> setterExpression, Times times)
        {
            this.nakedTypedMock.VerifySet(setterExpression, times);
        }

        public void VerifySet(Action<T> setterExpression, string failMessage)
        {
            this.nakedTypedMock.VerifySet(setterExpression, failMessage);
        }

        public void VerifySet(Action<T> setterExpression, Times times, string failMessage)
        {
            this.nakedTypedMock.VerifySet(setterExpression, times, failMessage);
        }

        public ISetupConditionResult<T> When(Func<bool> condition)
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

        private void MarkSetUpMethodAsVerified(LambdaExpression expression)
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