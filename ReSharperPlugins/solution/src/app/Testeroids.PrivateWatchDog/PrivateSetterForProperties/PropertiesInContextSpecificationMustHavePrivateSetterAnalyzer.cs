using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.Tree;

namespace Testeroids.PrivateWatchDog.PrivateSetterForProperties
{
    using System.Linq;

    using JetBrains.ReSharper.Psi;
    using JetBrains.ReSharper.Psi.CSharp.Tree;
    using JetBrains.ReSharper.Psi.Util;

    [ElementProblemAnalyzer(new[] { typeof(IPropertyDeclaration) })]
    public class PropertiesInContextSpecificationMustHavePrivateSetterAnalyzer : ElementProblemAnalyzer<IPropertyDeclaration>
    {
        protected override void Run(IPropertyDeclaration propertyDeclaration,
                                    ElementProblemAnalyzerData data,
                                    IHighlightingConsumer consumer)
        {
            var containingClass = propertyDeclaration.GetContainingNode<IClassDeclaration>();

            var isInAContextSpecification = containingClass != null && containingClass.DeclaredElement.GetAllSuperTypes().Any(o => o.GetLongPresentableName(propertyDeclaration.Language) == "Testeroids.IContextSpecification");
            if (isInAContextSpecification)
            {
                if (propertyDeclaration.DeclaredElement.Setter != null && propertyDeclaration.DeclaredElement.Setter.AccessibilityDomain.DomainType != AccessibilityDomain.AccessibilityDomainType.PRIVATE)
                {
                    consumer.AddHighlighting(new PropertySetterMustBePrivateInTestContextSpecificationHighlighting(propertyDeclaration), propertyDeclaration.GetDocumentRange(), propertyDeclaration.GetContainingFile());
                }
            }
        }
    }
}
