// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FailNotCalledBaseEstablishContextAspectAttribute.cs" company="Testeroids">
//   � 2012-2013 Testeroids. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Testeroids.Aspects
{
    using System;
    using System.Linq;
    using System.Reflection;

    using PostSharp.Aspects;
    using PostSharp.Aspects.Dependencies;
    using PostSharp.Extensibility;

    /// <summary>
    ///   Test if the EstablishContext Method always calls the base.EstablishContext.
    /// </summary>
    [Serializable]
    [AspectTypeDependency(AspectDependencyAction.Order, AspectDependencyPosition.Before, typeof(ArrangeActAssertAspectAttribute))]
    [MulticastAttributeUsage(MulticastTargets.Class, 
        TargetTypeAttributes = MulticastAttributes.AnyScope | MulticastAttributes.AnyVisibility | MulticastAttributes.NonAbstract | MulticastAttributes.Managed, 
        AllowMultiple = false, Inheritance = MulticastInheritance.Strict)]
    public class FailNotCalledBaseEstablishContextAspectAttribute : InstanceLevelAspect
    {
        #region Constants

        /// <summary>
        ///   The intermediate language code representing the "call" instruction code.
        /// </summary>
        private const int CallInstructionCode = 40;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   The compile time validate.
        /// </summary>
        /// <param name="type"> The class to be checked. </param>
        /// <returns> The System.Boolean. </returns>
        public override bool CompileTimeValidate(Type type)
        {
            if (TypeInvestigationService.GetAllTestMethods(type).Any())
            {
                try
                {
                    var methodInfo = type.GetMethod("EstablishContext", BindingFlags.Instance | BindingFlags.NonPublic);

                    if (methodInfo != null)
                    {
                        var establishContextDeclaringType = methodInfo.DeclaringType;
                        var intermediateLanguage = GetIntermediateLanguageFromMethodInfo(methodInfo);

                        if (establishContextDeclaringType == null)
                        {
                            return false;
                        }

                        var position = 0;
                        var baseEstablishContextCalls = 0;
                        var anotherEstablishContextCalls = 0;

                        while (position < intermediateLanguage.Length)
                        {
                            switch (intermediateLanguage[position++])
                            {
                                case CallInstructionCode:
                                {
                                    var metadataToken = ((intermediateLanguage[position++] |
                                                          (intermediateLanguage[position++] << 8)) |
                                                         (intermediateLanguage[position++] << 0x10)) |
                                                        (intermediateLanguage[position++] << 0x18);
                                    var calledMethod = GetMethodBaseFromIntermediateLanguage(metadataToken, establishContextDeclaringType, methodInfo);
                                    if (calledMethod != null && calledMethod.Name == "EstablishContext")
                                    {
                                        var establishContextCalledClassType = calledMethod.DeclaringType;
                                        if (establishContextCalledClassType != null &&
                                            establishContextDeclaringType.IsSubclassOf(establishContextCalledClassType))
                                        {
                                            baseEstablishContextCalls++;
                                        }
                                        else
                                        {
                                            anotherEstablishContextCalls++;
                                        }
                                    }

                                    break;
                                }
                            }
                        }

                        if (baseEstablishContextCalls == 1)
                        {
                            return true;
                        }

                        if (baseEstablishContextCalls == 0)
                        {
                            if (anotherEstablishContextCalls > 0)
                            {
                                return ErrorService.RaiseError(this.GetType(), type, "The EstablishContext of the '{0}' class calls another EstablishContext than the base.EstablishContext.\r\n\r\nPlease replace EstablishContext call in the EstablishContext method of the '{0}' class.\r\n");
                            }

                            if (establishContextDeclaringType != typeof(ContextSpecificationBase) &&
                                (establishContextDeclaringType.IsGenericType && establishContextDeclaringType.GetGenericTypeDefinition() != typeof(SubjectInstantiationContextSpecification<>)))
                            {
                                return ErrorService.RaiseError(this.GetType(), type, string.Format("The EstablishContext of the '{0}' class does not call the base.EstablishContext.\r\n\r\nPlease add base.EstablishContext in the EstablishContext method of the '{{0}}' class.\r\n", establishContextDeclaringType.Name));
                            }
                        }

                        if (baseEstablishContextCalls > 1)
                        {
                            return ErrorService.RaiseError(this.GetType(), type, "The EstablishContext of the '{0}' class calls the base.EstablishContext many times.\r\n\r\nPlease remove unneeded base.EstablishContext in the EstablishContext method of the '{0}' class.\r\n");
                        }
                    }
                }
                catch (Exception e)
                {
                    return ErrorService.RaiseError(this.GetType(), type, e.Message);
                }
            }

            return base.CompileTimeValidate(type);
        }

        #endregion

        #region Methods

        /// <summary>
        ///   Get intermediate language of passed method.
        /// </summary>
        /// <param name="methodInfo"> The method info. </param>
        /// <returns> The intermediate language as a byte array. </returns>
        /// <exception cref="ArgumentException">Thrown if methodBody is null.</exception>
        private static byte[] GetIntermediateLanguageFromMethodInfo(MethodBase methodInfo)
        {
            var methodBody = methodInfo.GetMethodBody();
            if (methodBody == null)
            {
                throw new ArgumentException("Method Body is null");
            }

            return methodBody.GetILAsByteArray();
        }

        /// <summary>
        /// Get the method from intermediate language.
        /// </summary>
        /// <param name="metadataToken">
        /// The metadata token.
        /// </param>
        /// <param name="establishContextDeclaringType">
        /// The establish context declaring type. Used for generic types.
        /// </param>
        /// <param name="memberInfo">
        /// The method info.
        /// </param>
        /// <returns>
        /// The method gotten from intermediate language out of the metadata token.
        /// </returns>
        private static MethodBase GetMethodBaseFromIntermediateLanguage(
            int metadataToken, 
            Type establishContextDeclaringType, 
            MemberInfo memberInfo)
        {
            try
            {
                var operand = establishContextDeclaringType.IsGenericType
                                  ? memberInfo.Module.ResolveMethod(metadataToken, establishContextDeclaringType.GetGenericArguments(), null)
                                  : memberInfo.Module.ResolveMethod(metadataToken);

                return operand;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion
    }
}