using System.Collections.Generic;
using System.Linq;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Intentions.Extensibility.Menu;
using JetBrains.Util;

namespace Testeroids.PrivateWatchDog
{
    using System;

    using JetBrains.Application;
    using JetBrains.Application.Progress;
    using JetBrains.ProjectModel;
    using JetBrains.ReSharper.Psi;
    using JetBrains.ReSharper.Psi.CSharp.Tree;
    using JetBrains.TextControl;
    using JetBrains.UI.StatusBar;

    [QuickFix]
    public sealed class MakeAccessedPrivatePropertyGetterProtectedQuickFix : QuickFixBase
    {
        private readonly UseOfPrivateMemberInInheritedNestedClassHighlight highlight;

        private string text;

        private IDeclaredElement declaredElement;

        public MakeAccessedPrivatePropertyGetterProtectedQuickFix(UseOfPrivateMemberInInheritedNestedClassHighlight highlight)
        {
            this.highlight = highlight;
            this.text = "Fix access rights to allow accessing the getter from inherited classes.";
            var referenceExpression = this.highlight.ReferenceExpression;
            this.declaredElement = referenceExpression.Reference.CurrentResolveResult.DeclaredElement;
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
            var propertyDeclaration = this.declaredElement.GetDeclarations().Single() as IPropertyDeclaration;
            
            if (propertyDeclaration != null)
            {
                propertyDeclaration.SetAccessRights(AccessRights.PROTECTED);

                // same as propertyDeclaration, therefore : no need to specify it.
                propertyDeclaration.AccessorDeclarations.Single(o => o.Kind == AccessorKind.GETTER).SetAccessRights(AccessRights.NONE);

                propertyDeclaration.AccessorDeclarations.Single(o => o.Kind == AccessorKind.SETTER).SetAccessRights(AccessRights.PRIVATE);

                Shell.Instance.GetComponent<IStatusBar>().SetText(string.Format("The {0} property setter was not private. The declaration has been fixed to only allow setting the property in the private scope.", propertyDeclaration.DeclaredName), true);
            }

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

        /// <summary>
        /// Check if this action is available at the constructed context.
        ///             Actions could store precalculated info in <paramref name="cache"/> to share it between different actions
        /// </summary>
        /// <returns>
        /// true if this bulb action is available, false otherwise.
        /// </returns>
        public override bool IsAvailable(IUserDataHolder cache)
        {        
            return this.declaredElement as IProperty != null;
        }
    }
}
