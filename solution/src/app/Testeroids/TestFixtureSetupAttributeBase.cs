namespace Testeroids
{
    using System;

    /// <summary>
    /// The base type for an attribute denoting a test fixture requiring setup/teardown notifications.
    /// </summary>
    public abstract class TestFixtureSetupAttributeBase : Attribute
    {
        /// <summary>
        /// Called by the framework to register the attribute with the context for setup/teardown notifications.
        /// </summary>
        /// <param name="contextSpecification">
        /// The target context specification instance.
        /// </param>
        public abstract void Register(IContextSpecification contextSpecification);
    }
}