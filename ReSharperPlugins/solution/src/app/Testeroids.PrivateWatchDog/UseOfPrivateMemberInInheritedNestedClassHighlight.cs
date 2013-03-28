namespace Testeroids.PrivateWatchDog
{
    using JetBrains.ReSharper.Daemon;
    using JetBrains.ReSharper.Psi.CSharp.Tree;
    using JetBrains.ReSharper.Psi.Tree;

    [StaticSeverityHighlighting(Severity.ERROR, "Private WatchDog")]
    public class UseOfPrivateMemberInInheritedNestedClassHighlight : IHighlighting
    {
        public IReferenceExpression ReferenceExpression { get; set; }

        public UseOfPrivateMemberInInheritedNestedClassHighlight(IReferenceExpression referenceExpression)
        {
            ReferenceExpression = referenceExpression;
            this.ToolTip = referenceExpression.GetText() + " is a private member and is accessed from an inherited class. This is not allowed.";
            this.ErrorStripeToolTip = "Private member is accessed from an inherited class.";
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