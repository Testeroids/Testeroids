using System.Collections.Generic;
using System.Linq;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Intentions.Extensibility.Menu;
using JetBrains.Util;

namespace Testeroids.PrivateWatchDog.PrivateSetterForProperties
{
    using System;

    using JetBrains.Application.Progress;
    using JetBrains.ProjectModel;
    using JetBrains.ReSharper.Psi;
    using JetBrains.TextControl;

    [QuickFix]
    public sealed class MakePropertySetterPrivateQuickFix : QuickFixBase
    {
        private readonly PropertySetterMustBePrivateInTestContextSpecificationHighlighting highlighting;

        private string text;

        public MakePropertySetterPrivateQuickFix(PropertySetterMustBePrivateInTestContextSpecificationHighlighting highlighting)
        {
            this.highlighting = highlighting;
            this.text = "Make setter private.";
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
            this.highlighting.PropertyDeclaration.AccessorDeclarations.First(o => o.Kind == AccessorKind.SETTER).SetAccessRights(AccessRights.PRIVATE);

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
            return true;
        }
    }
}
