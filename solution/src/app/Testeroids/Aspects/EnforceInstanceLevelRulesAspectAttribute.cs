namespace Testeroids.Aspects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using PostSharp.Aspects;
    using PostSharp.Aspects.Advices;
    using PostSharp.Extensibility;

    using Testeroids.Rules;

    /// <summary>
    /// Aspect which enforces <see cref="IInstanceLevelRule"/>s and <see cref="IPropertyAccessRule"/>s.
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    [MulticastAttributeUsage(Inheritance = MulticastInheritance.Strict)]
    public class EnforceInstanceLevelRulesAspectAttribute : InstanceLevelAspect
    {
        #region Fields

        /// <summary>
        /// All instance level rules currently enforced by this aspect.
        /// </summary>
        private readonly IEnumerable<IInstanceLevelRule> instanceLevelRules;

        /// <summary>
        /// All property access rules currently enforced by this aspect.
        /// </summary>
        private readonly IEnumerable<IPropertyAccessRule> propertyAccessRules;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EnforceInstanceLevelRulesAspectAttribute"/> class.
        /// </summary>
        public EnforceInstanceLevelRulesAspectAttribute()
        {
            this.instanceLevelRules = new List<IInstanceLevelRule>
                                      {
                                          new FailNotCalledBaseEstablishContextRule(), 
                                          ////new FailPrivateFieldCalledInNestedClassRule(), 
                                          new FailTestFixtureWithoutTestRule(),
                                          new ProhibitGetOnNotInitializedPropertyRule(),
 
                                          // Currently not working as expected, EstablishContext is called multiple times.
                                          // new ProhibitSetOnInitializedPropertyRule()
                                      };

            this.propertyAccessRules = this.instanceLevelRules.OfType<IPropertyAccessRule>().ToArray();
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Checks whether the type is a candidate for the aspect to apply to it.
        /// </summary>
        /// <param name="type">the type to check if it needs the attribute.</param>
        /// <returns><c>true</c> if any of the <see cref="instanceLevelRules"/> attribute needs to be applied to the type; <c>false</c> otherwise.</returns>
        public override bool CompileTimeValidate(Type type)
        {
            if (!TypeInvestigationService.IsContextSpecification(type))
            {
                return false;
            }

            var validates = false;

// ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var rule in this.instanceLevelRules)
            {
                // do not break as some validators raise errors.
                validates = validates | rule.CompileTimeValidate(type);
            }

            return validates;
        }

        /// <summary>
        /// Calls <see cref="IPropertyAccessRule.OnPropertyGet"/> for each <see cref="propertyAccessRules"/>.
        /// </summary>
        /// <param name="args">
        /// Arguments of advices of aspect type <see cref="LocationInterceptionAspect"/>.
        /// </param>
        [OnLocationGetValueAdvice]
        [MulticastPointcut(Targets = MulticastTargets.Property, Attributes = MulticastAttributes.AnyVisibility | MulticastAttributes.Instance)]
        public void OnPropertyGet(LocationInterceptionArgs args)
        {
            foreach (var propertyAccessRule in this.propertyAccessRules)
            {
                propertyAccessRule.OnPropertyGet(args.Location.PropertyInfo);
            }

            args.ProceedGetValue();
        }

        /// <summary>
        /// Calls <see cref="IPropertyAccessRule.OnPropertySet"/> for each <see cref="propertyAccessRules"/>.
        /// </summary>
        /// <param name="args">
        /// Arguments of advices of aspect type <see cref="LocationInterceptionAspect"/>.
        /// </param>
        [OnLocationSetValueAdvice(Master = "OnPropertyGet")]
        public void OnPropertySet(LocationInterceptionArgs args)
        {
            foreach (var propertyAccessRule in this.propertyAccessRules)
            {
                propertyAccessRule.OnPropertySet(args.Location.PropertyInfo);
            }

            args.ProceedSetValue();
        }

        /// <summary>
        /// Initializes the aspect instance. This method is invoked when all system elements of the aspect (like member imports) have completed.
        /// All rules are initialized.
        /// </summary>
        public override void RuntimeInitializeInstance()
        {
            base.RuntimeInitializeInstance();

            foreach (var rule in this.instanceLevelRules)
            {
                rule.Initialize();
            }
        }

        #endregion
    }
}