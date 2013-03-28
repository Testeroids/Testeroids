namespace Testeroids.PrivateWatchDog.PrivateSetterForProperties
{
    using JetBrains.ReSharper.Daemon;
    using JetBrains.ReSharper.Psi.CSharp.Tree;

    [StaticSeverityHighlighting(Severity.ERROR, "Private WatchDog")]
    public class PropertySetterMustBePrivateInTestContextSpecificationHighlighting : IHighlighting
    {
        public IPropertyDeclaration PropertyDeclaration { get; set; }

        public PropertySetterMustBePrivateInTestContextSpecificationHighlighting(IPropertyDeclaration propertyDeclaration)
        {
            this.PropertyDeclaration = propertyDeclaration;
            this.ToolTip = "Properties used in a test context are only allowed to have a private setter.";
            this.ErrorStripeToolTip = "Property setter must be private.";
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
    }
}