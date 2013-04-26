// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Helper.cs" company="Testeroids">
//   © 2012-2013 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Testeroids
{
    using System;
    using System.Globalization;
    using System.Reflection;

    using Testeroids.TriangulationEngine;

    /// <summary>
    ///   A class designed to help with accessing private constructors/properties.
    /// </summary>
    public static class Helper
    {
        #region Public Methods and Operators

        /// <summary>
        /// Constructs an object which has a private constructor.
        /// </summary>
        /// <param name="specification">
        /// The specification object.
        /// </param>
        /// <param name="args">
        /// The arguments to pass to the constructor.
        /// </param>
        /// <typeparam name="T">
        /// The concrete object type.
        /// </typeparam>
        /// <returns>
        /// The constructed object.
        /// </returns>
        /// <exception cref="Exception">
        /// Returns any exception thrown by <see cref="Activator.CreateInstance(System.Type,System.Reflection.BindingFlags,System.Reflection.Binder,object[],System.Globalization.CultureInfo)"/>. Unwraps <see cref="TargetInvocationException"/>.
        /// </exception>
        public static T InstancePrivateObject<T>(
            this IContextSpecification specification, 
            params object[] args)
        {
            try
            {
                return (T)Activator.CreateInstance(
                    typeof(T), 
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.CreateInstance, 
                    null, 
                    args, 
                    null);
            }
            catch (TargetInvocationException exception)
            {
                if (exception.InnerException != null)
                {
                    throw exception.InnerException;
                }

                throw;
            }
        }

        /// <summary>
        ///   The invoke private.
        /// </summary>
        /// <param name="target"> The target object which we want to invoke. </param>
        /// <param name="methodName"> The method Name. </param>
        /// <param name="args"> The args to pass to the method. </param>
        public static void InvokePrivate(
            object target, 
            string methodName, 
            object[] args = null)
        {
            try
            {
                target.GetType().InvokeMember(
                    methodName, 
                    BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod, 
                    null, 
                    target, 
                    args);
            }
            catch (TargetInvocationException exception)
            {
                if (exception.InnerException != null)
                {
                    throw exception.InnerException;
                }

                throw;
            }
        }

        /// <summary>
        ///   Sets a private field.
        /// </summary>
        /// <param name="target"> The target object. </param>
        /// <param name="fieldName"> The name of the field to set. </param>
        /// <param name="newValue"> The new value for the field. </param>
        public static void SetPrivateField(
            object target, 
            string fieldName, 
            object newValue)
        {
            try
            {
                const BindingFlags BindingFlags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField;
                var type = target.GetType();
                var fieldInfo = type.GetField(fieldName, BindingFlags);

                if (fieldInfo == null)
                {
                    throw new ArgumentException(string.Format(@"Field with name '{0}' not found on {1}", fieldName, type.Name), @"fieldName");
                }

                fieldInfo.SetValue(
                    target, 
                    newValue, 
                    BindingFlags, 
                    null, 
                    CultureInfo.InvariantCulture);
            }
            catch (TargetInvocationException exception)
            {
                if (exception.InnerException != null)
                {
                    throw exception.InnerException;
                }

                throw;
            }
        }

        /// <summary>
        ///   Sets a private property.
        /// </summary>
        /// <param name="target"> The target object. </param>
        /// <param name="propertyName"> The name of the property to set. </param>
        /// <param name="newValue"> The new value for the property. </param>
        /// <exception cref="Exception">An exception, in case the property setter throws one.</exception>
        public static void SetPrivateProperty(
            object target, 
            string propertyName, 
            object newValue)
        {
            try
            {
                var type = target.GetType();
                var propertyInfo = type.GetProperty(propertyName);
                if (propertyInfo == null)
                {
                    throw new ArgumentException(string.Format(@"Property with name '{0}' not found on {1}", propertyName, type.Name), @"propertyName");
                }

                propertyInfo.SetValue(
                    target, 
                    newValue, 
                    BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, 
                    null, 
                    null, 
                    CultureInfo.InvariantCulture);
            }
            catch (TargetInvocationException exception)
            {
                if (exception.InnerException != null)
                {
                    throw exception.InnerException;
                }

                throw;
            }
        }

        #endregion
    }
}