namespace Testeroids.EstablishmentMethodsMustBeSealed
{
    using System;

    // ReSharper disable RedundantUsingDirective : since it is very hard with JetBrains' API to discover their features, we'll keep the redundant usings in order to facilitate future maintenance.
    using JetBrains.Application.Progress;
    using JetBrains.ProjectModel;
    using JetBrains.ReSharper.Feature.Services.Bulbs;
    using JetBrains.ReSharper.Intentions.Extensibility;
    using JetBrains.ReSharper.Intentions.Extensibility.Menu;
    using JetBrains.ReSharper.Intentions.Util;
    using JetBrains.ReSharper.Psi.CSharp;
    using JetBrains.ReSharper.Psi.CSharp.Impl.Tree;
    using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
    using JetBrains.ReSharper.Psi.Tree;
    using JetBrains.ReSharper.Refactorings.Util;
    using JetBrains.TextControl;
    using JetBrains.Util;
    // ReSharper restore RedundantUsingDirective
    [QuickFix]
    public class EnforceSealedEstablismentMethodsQuickFix : QuickFixBase
    {
        private readonly EstablishmentMethodShouldBeSealedHighlight highlight;

        private readonly string text;

        /// <summary>
        /// For languages other than C# any inheritor of <see cref="IContextActionDataProvider"/> can 
        /// be injected in this constructor.
        /// </summary>
        public EnforceSealedEstablismentMethodsQuickFix(EstablishmentMethodShouldBeSealedHighlight highlight)
        {
            this.highlight = highlight;
            this.text = "Add sealed modifier";
        }        

        /// <summary>
        /// Check if this action is available at the constructed context.
        ///             Actions could store precalculated info in <paramref name="cache"/> to share it between different actions
        /// </summary>
        /// <remarks>
        /// If we get there, it is always available in any case, because the <see cref="MarkUnsealedEstablishmentMethodsAsErrorAnalyzer"/> deamon already has the logic to only mark method declarations where the fix is available.
        /// </remarks>
        /// <returns>
        /// true if this bulb action is available, false otherwise.
        /// </returns>
        public override bool IsAvailable(IUserDataHolder cache)
        {
            return true;
        }

        /// <summary>
        /// Executes QuickFix or ContextAction. Returns post-execute method.
        /// </summary>
        /// <returns>
        /// Action to execute after document and PSI transaction finish. Use to open TextControls, navigate caret, etc.
        /// </returns>
        protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution,
                                                        IProgressIndicator progress)
        {
            var declaration = this.highlight.GetEstablishmentMethodDeclaration();
            declaration.SetSealed(true);      
            return null;
        }

        /// <summary>
        /// Popup menu item text
        /// </summary>
        public override string Text
        {
            get
            {
                return this.text;
            }
        }
    }
}
