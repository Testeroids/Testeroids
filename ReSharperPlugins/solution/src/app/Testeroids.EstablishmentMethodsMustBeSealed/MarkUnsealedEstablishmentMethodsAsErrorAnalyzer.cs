

namespace Testeroids.EstablishmentMethodsMustBeSealed
{
    using JetBrains.ReSharper.Psi;
    using JetBrains.ReSharper.Daemon.Stages;
    using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
    using JetBrains.ReSharper.Psi.CSharp.Tree;
    using JetBrains.ReSharper.Psi.Tree;

    [ElementProblemAnalyzer(new[] { typeof(IMethodDeclaration) }, HighlightingTypes = new[] { typeof(EstablishmentMethodShouldBeSealedHighlight)})]
    public class MarkUnsealedEstablishmentMethodsAsErrorAnalyzer : ElementProblemAnalyzer<IMethodDeclaration>
    {
        protected override void Run(IMethodDeclaration element,
                                    ElementProblemAnalyzerData data,
                                    IHighlightingConsumer consumer)
        {
            bool isEstablishmentMethodUnsealed = this.CheckEstablishmentMethodUnsealed(element);
            if (isEstablishmentMethodUnsealed)
            {
                var establishmentMethodShouldBeSealedHighlight = new EstablishmentMethodShouldBeSealedHighlight(element);
                consumer.AddHighlighting(establishmentMethodShouldBeSealedHighlight, element.GetDocumentRange(), element.GetContainingFile());
            }
        }

        private bool CheckEstablishmentMethodUnsealed(IMethodDeclaration element)
        {
            var isContextEstablishmentMethod = element.DeclaredName.StartsWith("Establish");
            var isEstablishContext = element.DeclaredName == "EstablishContext";
            var isPrivate = element.DeclaredElement.AccessibilityDomain.DomainType == AccessibilityDomain.AccessibilityDomainType.PRIVATE;
            return isContextEstablishmentMethod && !isEstablishContext && !element.IsSealed && !element.IsAbstract && !isPrivate;
        }
    }
}
