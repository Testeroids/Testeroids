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
        IEnumerable<Tuple<object, bool>> IMockInternals.VerifiedSetups
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
            catch (MockException e) // why the heck is MockVerificationException internal ??
            {
                throw new SetupWasNeverUsedException("Some setups were applied to the mock, but the members were never actually used:\r\n\r\n" + e.Message + "\r\nIf this is intentionnal, do not call .Verifiable() or .EnforceUsage() on the setup.", e);
            }
        }

        public void VerifyAll()
        {
            this.nakedMock.VerifyAll();
        }

        #endregion

        #region Methods

        protected virtual IEnumerable<Tuple<object, bool>> GetVerifiedSetups()
        {
            return Enumerable.Empty<Tuple<object, bool>>();
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
        private readonly Dictionary<object, bool> registeredSetups = new Dictionary<object, bool>();

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

        public void Raise(Action<T> eventExpression,
                          EventArgs args)
        {
            this.nakedTypedMock.Raise(eventExpression, args);
        }

        public void Raise(Action<T> eventExpression,
                          params object[] args)
        {
            this.nakedTypedMock.Raise(eventExpression, args);
        }

        public Moq.Language.Flow.ISetup<T> Setup(Expression<Action<T>> expression)
        {
            var setup = this.nakedTypedMock.Setup(expression);

            this.RegisterSetupForVerification(expression);

            // todo : Use a factory to make it testable (?).
            var moqWrappedSetup = new MoqSetupWrapper<T>(setup, expression, this);

            return moqWrappedSetup;
        }

        public Moq.Language.Flow.ISetup<T, TResult> Setup<TResult>(Expression<Func<T, TResult>> expression)
        {
            var setup = this.nakedTypedMock.Setup(expression);

            this.RegisterSetupForVerification(expression);

            // todo : Use a factory to make it testable (?).
            var moqWrappedSetup = new MoqSetupWrapper<T, TResult>(setup, expression, this);

            return moqWrappedSetup;
        }

        public ISetupGetter<T, TProperty> SetupGet<TProperty>(Expression<Func<T, TProperty>> expression)
        {
            var setupGetter = this.nakedTypedMock.SetupGet(expression);

            this.RegisterSetupForVerification(expression);

            // todo : Use a factory to make it testable (?).
            var moqWrappedSetup = new MoqSetupGetterWrapper<T, TProperty>(setupGetter, expression, this);

            return moqWrappedSetup;
        }

        public IMock<T> SetupProperty<TProperty>(Expression<Func<T, TProperty>> property)
        {
            this.nakedTypedMock.SetupProperty(property);

            return this;
        }

        public IMock<T> SetupProperty<TProperty>(Expression<Func<T, TProperty>> property,
                                                 TProperty initialValue)
        {
            this.nakedTypedMock.SetupProperty(property, initialValue);

            return this;
        }

        public ISetupSetter<T, TProperty> SetupSet<TProperty>(Action<T> setterExpression)
        {
            var setupSetter = this.nakedTypedMock.SetupSet<TProperty>(setterExpression);

            this.RegisterSetupForVerification(setterExpression);

            var moqWrappedSetup = new MoqSetupSetterWrapper<T, TProperty>(setupSetter, setterExpression, this);

            return moqWrappedSetup;
        }

        public Moq.Language.Flow.ISetup<T> SetupSet(Action<T> setterExpression)
        {
            var setupSet = this.nakedTypedMock.SetupSet(setterExpression);
            
            // ((Moq.IProxyCall)(setupSet)).SetupExpression would have been perfect, but IProxyCall is internal. we could also retrieve it by reflection in order to be able to find out the name of the property which was set.
            // HACK : since we can't get an Expression, we'll just store the Action<T> and the ToString() representation of the ISetup<T> object returned by SetupSet in a Tuple. the ToString representation is just so we can have a proper error message if we need to throw a MockNotVerifiedException.

            var hackyObject = new Tuple<object,string>(setterExpression, setupSet.ToString());
            this.RegisterSetupForVerification(hackyObject);

            var moqWrappedSetup = new MoqSetupSetterWrapper<T>(setupSet, hackyObject, this);                     

            return moqWrappedSetup;
        }

        public void Verify(Expression<Action<T>> expression)
        {
            this.MarkSetUpExpressionAsMatchedByVerifyCall(expression);

            this.nakedTypedMock.Verify(expression);
        }

        public void Verify(Expression<Action<T>> expression,
                           Times times)
        {
            this.MarkSetUpExpressionAsMatchedByVerifyCall(expression);

            this.nakedTypedMock.Verify(expression, times);
        }

        public void Verify(Expression<Action<T>> expression,
                           string failMessage)
        {
            this.MarkSetUpExpressionAsMatchedByVerifyCall(expression);

            this.nakedTypedMock.Verify(expression, failMessage);
        }

        public void Verify(Expression<Action<T>> expression,
                           Times times,
                           string failMessage)
        {
            this.MarkSetUpExpressionAsMatchedByVerifyCall(expression);

            this.nakedTypedMock.Verify(expression, times, failMessage);
        }

        public void Verify<TResult>(Expression<Func<T, TResult>> expression)
        {
            this.MarkSetUpExpressionAsMatchedByVerifyCall(expression);

            this.nakedTypedMock.Verify(expression);
        }

        public void Verify<TResult>(Expression<Func<T, TResult>> expression,
                                    Times times)
        {
            this.MarkSetUpExpressionAsMatchedByVerifyCall(expression);

            this.nakedTypedMock.Verify(expression, times);
        }

        public void Verify<TResult>(Expression<Func<T, TResult>> expression,
                                    string failMessage)
        {
            this.MarkSetUpExpressionAsMatchedByVerifyCall(expression);

            this.nakedTypedMock.Verify(expression, failMessage);
        }

        public void Verify<TResult>(Expression<Func<T, TResult>> expression,
                                    Times times,
                                    string failMessage)
        {
            this.MarkSetUpExpressionAsMatchedByVerifyCall(expression);

            this.nakedTypedMock.Verify(expression, times, failMessage);
        }

        public void VerifyGet<TProperty>(Expression<Func<T, TProperty>> expression)
        {
            this.MarkSetUpExpressionAsMatchedByVerifyCall(expression);

            this.nakedTypedMock.VerifyGet(expression);
        }

        public void VerifyGet<TProperty>(Expression<Func<T, TProperty>> expression,
                                         Times times)
        {
            this.MarkSetUpExpressionAsMatchedByVerifyCall(expression);

            this.nakedTypedMock.VerifyGet(expression, times);
        }

        public void VerifyGet<TProperty>(Expression<Func<T, TProperty>> expression,
                                         string failMessage)
        {
            this.MarkSetUpExpressionAsMatchedByVerifyCall(expression);

            this.nakedTypedMock.VerifyGet(expression, failMessage);
        }

        public void VerifyGet<TProperty>(Expression<Func<T, TProperty>> expression,
                                         Times times,
                                         string failMessage)
        {
            this.MarkSetUpExpressionAsMatchedByVerifyCall(expression);

            this.nakedTypedMock.VerifyGet(expression, times, failMessage);
        }

        public void VerifySet(Action<T> setterExpression)
        {
            this.nakedTypedMock.VerifySet(setterExpression);
        }

        public void VerifySet(Action<T> setterExpression,
                              Times times)
        {
            this.nakedTypedMock.VerifySet(setterExpression, times);
        }

        public void VerifySet(Action<T> setterExpression,
                              string failMessage)
        {
            this.nakedTypedMock.VerifySet(setterExpression, failMessage);
        }

        public void VerifySet(Action<T> setterExpression,
                              Times times,
                              string failMessage)
        {
            this.nakedTypedMock.VerifySet(setterExpression, times, failMessage);
        }

        public ISetupConditionResult<T> When(Func<bool> condition)
        {
            return this.nakedTypedMock.When(condition);
        }

        #endregion

        #region Methods

        protected override IEnumerable<Tuple<object, bool>> GetVerifiedSetups()
        {
            return this.registeredSetups.Select(x => new Tuple<object, bool>(x.Key, x.Value));
        }

        private void MarkSetUpExpressionAsMatchedByVerifyCall(LambdaExpression expression)
        {
            var memberInfo = expression.Body is MethodCallExpression
                                 ? ((MethodCallExpression)expression.Body).Method
                                 : ((MemberExpression)expression.Body).Member;
            this.registeredSetups[memberInfo] = true;
        }

        /// <summary>
        /// Unregisters the specified expression in order to ignore it in the sanity check which makes sure there is a call verification unit test for each setup.
        /// </summary>
        /// <param name="expression"></param>
        public void UnregisterSetupForVerification(object expression)
        {
            var lambdaExpression = expression as LambdaExpression;
            MemberInfo memberInfo;
            if (lambdaExpression != null)
            {
                memberInfo = lambdaExpression.Body is MethodCallExpression ? ((MethodCallExpression)lambdaExpression.Body).Method : ((MemberExpression)lambdaExpression.Body).Member;
                this.registeredSetups.Remove(memberInfo);   
            }
            else
            {
                this.registeredSetups.Remove(expression);
            }
        }

        private void RegisterSetupForVerification(object expression)
        {
            var lambdaExpression = expression as LambdaExpression;
            object setupExpression;
            if (lambdaExpression != null)
            {
                setupExpression = lambdaExpression.Body is MethodCallExpression
                                 ? ((MethodCallExpression)lambdaExpression.Body).Method
                                 : ((MemberExpression)lambdaExpression.Body).Member;
            }
            else
            {
                setupExpression = expression;
            }

            if (!this.registeredSetups.ContainsKey(setupExpression))
            {
                this.registeredSetups[setupExpression] = false;
            }
        }

        #endregion
    }
}