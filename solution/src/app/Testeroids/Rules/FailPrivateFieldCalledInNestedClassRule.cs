namespace Testeroids.Rules
{
    using System;
    using System.Linq;
    using System.Reflection;

    using Testeroids.Aspects;

    /// <summary>
    ///   Test that private fields are not called in nested classes.
    /// </summary>
    [Serializable]
    public class FailPrivateFieldCalledInNestedClassRule : InstanceLevelRule
    {
        #region Public Methods and Operators

        /// <summary>
        /// Checks that there is no private field in given class if this is abstract.
        /// </summary>
        /// <param name="type"> The class to be checked.</param>
        /// <returns>Raises an error if there is any private field in given class if this is abstract.</returns>
        public override bool CompileTimeValidate(Type type)
        {
            try
            {
                const BindingFlags Flags = BindingFlags.NonPublic | BindingFlags.Static;
                if (type.IsAbstract)
                {
                    var fields = type.GetFields(Flags).Where(f => f.IsPrivate).ToList();
                    if (fields.Any())
                    {
                        var field = fields.First();
                        return ErrorService.RaiseError(this.GetType(), type, field.Name + " should be a protected property.\r\n");
                    }
                }
            }
            catch (Exception e)
            {
                return ErrorService.RaiseError(this.GetType(), type, e.Message);
            }

            return true;
        }

        #endregion
    }
}