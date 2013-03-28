using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.Tree;

namespace Testeroids.PrivateWatchDog
{
    using System;
    using System.Linq;

    using JetBrains.Annotations;
    using JetBrains.ReSharper.Psi;
    using JetBrains.ReSharper.Psi.CSharp;
    using JetBrains.ReSharper.Psi.CSharp.Tree;
    using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve.Managed;
    using JetBrains.ReSharper.Psi.Util;

    //[ElementProblemAnalyzer(new[] { typeof(IAssignmentExpression) }, HighlightingTypes = new[]{ typeof(UseOfPrivateMemberInInheritedNestedClassHighlight)})]
    //public class PrivateAssignedAnalyzer : ElementProblemAnalyzer<IAssignmentExpression>
    //{
    //    protected override void Run(IAssignmentExpression element,
    //                                ElementProblemAnalyzerData data,
    //                                IHighlightingConsumer consumer)
    //    {
    //        consumer.AddHighlighting(new UseOfPrivateMemberInInheritedNestedClassHighlight(element),element.GetDocumentRange(),element.GetContainingFile());
    //    }
    //}

    [ElementProblemAnalyzer(new[] { typeof(IReferenceExpression) }, HighlightingTypes = new[] { typeof(UseOfPrivateMemberInInheritedNestedClassHighlight) })]
    public class PrivateReferencedAnalyzer : ElementProblemAnalyzer<IReferenceExpression>
    {
        protected override void Run(IReferenceExpression element,
                                    ElementProblemAnalyzerData data,
                                    IHighlightingConsumer consumer)
        {

            var isTestFixture = element.GetContainingTypeDeclaration().DeclaredElement.GetAllSuperTypes().Any(o => o.GetLongPresentableName(element.Language) == "Testeroids.IContextSpecification");
            if (isTestFixture)
            {
                bool isPrivate = this.IsUsageIllegal(element);
                if (isPrivate)
                {
                    consumer.AddHighlighting(new UseOfPrivateMemberInInheritedNestedClassHighlight(element), element.GetDocumentRange(), element.GetContainingFile());
                } 
            }
        }

        private bool IsUsageIllegal([NotNull] IReferenceExpression referenceExpression)
        {
            if (referenceExpression == null)
            {
                throw new ArgumentNullException("referenceExpression");
            }

            bool isPrivate = false;
         
            var declaredElement = referenceExpression.Reference.CurrentResolveResult.DeclaredElement;

            var property = declaredElement as IProperty;
            var typeMember = declaredElement as ITypeMember;
                
            AccessibilityDomain accessibilityDomain = null;
            if (property != null)
            {
                var assignment = referenceExpression.GetContainingNode<IAssignmentExpression>();
                if (assignment != null)
                {
                    if (assignment.Dest == referenceExpression)
                    {
                        accessibilityDomain = property.Setter.AccessibilityDomain;
                    }

                    if (assignment.Source == referenceExpression)
                    {
                        accessibilityDomain = property.Getter.AccessibilityDomain;
                    } 
                }
                    
                var initializer = referenceExpression.GetContainingNode<IExpressionInitializer>();
                if (initializer != null)
                {
                    // An initializer could only be used as a getter on the property, because a property cannot be initialized, for it is always declared as member instance, not local variable.
                    accessibilityDomain = property.Getter.AccessibilityDomain;
                }                    
            }
            else
            {
                if (typeMember != null)
                {
                    accessibilityDomain = typeMember.AccessibilityDomain;
                }  
            }

            if (accessibilityDomain != null)
            {
                isPrivate = accessibilityDomain.DomainType == AccessibilityDomain.AccessibilityDomainType.PRIVATE;
            }
            

            bool isSameClass = false;

            if (isPrivate)
            {
                ICSharpTypeDeclaration containingType;
                string className = null;
                
                // find where the referenced member has been declared.
                var resolveResultWithInfo = referenceExpression.Reference.CurrentResolveResult;
                if (resolveResultWithInfo != null)
                {
                    // we will probably crash with "partial". let's handle this later :)
                    var classDeclaration = resolveResultWithInfo.Result.DeclaredElement.GetDeclarations().Single().GetContainingNode<IClassDeclaration>();
                    if (classDeclaration != null)
                    {
                        className = string.Format("{0}.{1}", classDeclaration.DeclaredElement.GetContainingNamespace(), classDeclaration.DeclaredName);
                    }
                }

                containingType = referenceExpression.GetContainingTypeDeclaration();
                

                var referenceClassName = containingType.DeclaredElement.GetContainingNamespace() + "." + containingType.DeclaredName;
                isSameClass = className == referenceClassName;
            }

            return isPrivate && !isSameClass;
        }
    }

    //// // Let's not support it for now.
    //// [ElementProblemAnalyzer(new[] { typeof(IInvocationExpression) }, HighlightingTypes = new[] { typeof(UseOfPrivateMemberInInheritedNestedClassHighlight) })]
    //// public class PrivateInvokedAnalyzer : ElementProblemAnalyzer<IInvocationExpression>
    //// {
    ////     protected override void Run(IInvocationExpression element,
    ////                                 ElementProblemAnalyzerData data,
    ////                                 IHighlightingConsumer consumer)
    ////     {
    ////         consumer.AddHighlighting(new UseOfPrivateMemberInInheritedNestedClassHighlight(element), element.GetDocumentRange(), element.GetContainingFile());
       
    ////     }
    //// }
}
