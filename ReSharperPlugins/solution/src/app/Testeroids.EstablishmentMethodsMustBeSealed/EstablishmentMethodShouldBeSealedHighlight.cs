namespace Testeroids.EstablishmentMethodsMustBeSealed
{
    using JetBrains.ReSharper.Daemon;
    using JetBrains.ReSharper.Psi.CSharp.Tree;

    [StaticSeverityHighlighting(Severity.ERROR, "Enforce Sealed Context Establishment Methods")]
    public class EstablishmentMethodShouldBeSealedHighlight : IHighlighting
    {
        private readonly IMethodDeclaration element;

        public EstablishmentMethodShouldBeSealedHighlight(IMethodDeclaration element)
        {
            this.element = element;
            this.ToolTip = string.Format("{0} is not sealed. it is mandatory to seal implementations of context establishment methods.", element.DeclaredName);
            this.ErrorStripeToolTip = "Context establishment method is not sealed.";
        }

        /// <summary>
        /// Returns true if data (PSI, text ranges) associated with highlighting is valid
        /// </summary>
        public bool IsValid()
        {
            return true;
        }

        /// <summary>
        /// Message for this highlighting to show in tooltip and in status bar (if <see cref="P:JetBrains.ReSharper.Daemon.HighlightingAttributeBase.ShowToolTipInStatusBar"/> is <c>true</c>)
        ///             To override the default mechanism of tooltip, mark the implementation class with 
        ///             <see cref="T:JetBrains.ReSharper.Daemon.DaemonTooltipProviderAttribute"/> attribute, and then this property will not be called
        /// </summary>
        public string ToolTip { get; private set; }

        /// <summary>
        /// Message for this highlighting to show in tooltip and in status bar (if <see cref="P:JetBrains.ReSharper.Daemon.HighlightingAttributeBase.ShowToolTipInStatusBar"/> is <c>true</c>)
        /// </summary>
        public string ErrorStripeToolTip { get; private set; }

        /// <summary>
        /// Specifies the offset from the Range.StartOffset to set the cursor to when navigating 
        ///             to this highlighting. Usually returns <c>0</c>
        /// </summary>
        public int NavigationOffsetPatch { get; private set; }

        /// <summary>
        /// Retrieves a reference to the <see cref="IMethodDeclaration"/> marked as unsealed by <see cref="MarkUnsealedEstablishmentMethodsAsErrorAnalyzer"/>.
        /// </summary>
        /// <returns>
        /// The <see cref="IMethodDeclaration"/>.
        /// </returns>
        public IMethodDeclaration GetEstablishmentMethodDeclaration()
        {
            return this.element;
        }
    }
}