namespace Testeroids.TriangulationEngine
{
    using System;
    using System.Linq;

    using NUnit.Core;

    public class TriangulatedTestMethodFixture : NUnitTestFixture
    {
        #region Constructors and Destructors

        public TriangulatedTestMethodFixture(Type declaringType)
            : base(declaringType)
        {
            this.AddTplSupport(declaringType);

            var baseType = declaringType.BaseType;
            if (baseType != null)
            {
                this.Parent = new TriangulatedTestMethodFixture(baseType);
            }
        }

        /// <remarks>
        /// HACK : stepping stone : since the aspect is not applied to the fixture being run, we need to apply the same logic somehow : theferore, as a first step, we just re-do the whole logic on the classes marked with the aspect. (i.e. we mimic the behavior of the aspect instead of just using it)
        /// </remarks>
        private void AddTplSupport(Type declaringType)
        {
            var customAttributes = declaringType.GetCustomAttributes(true);
            var tplContextAspect = customAttributes.OfType<TplContextAspectAttribute>().FirstOrDefault();
            if (tplContextAspect != null)
            {
                var testTaskScheduler = new TplTestPlatformHelper.TestTaskScheduler(tplContextAspect.ExecuteTplTasks);

                TplTestPlatformHelper.SetDefaultScheduler(testTaskScheduler);                
            }
        }

        #endregion
    }
}