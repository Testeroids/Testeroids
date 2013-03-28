namespace SharpMocker
{
    using JetBrains.ActionManagement;
    using JetBrains.Application;
    using JetBrains.UI.ToolWindowManagement;

    [ToolWindowDescriptor(ContextMenuActionGroupId = "KeyboardHelperWindow", Text = "Keyboard Helper", ProductNeutralId = "D4EE8F74-3C45-4575-AEDF-9D38D5EFC51B", Type = ToolWindowType.SingleInstance, VisibilityPersistenceScope = ToolWindowVisibilityPersistenceScope.Global, InitialDocking = ToolWindowInitialDocking.Floating)]
    public class ArgumentsGenerationSelectionDescriptor : ToolWindowDescriptor
    {
        public ArgumentsGenerationSelectionDescriptor(IApplicationDescriptor applicationDescriptor)
            : base(applicationDescriptor)
        {
        }
    }

    [ActionHandler("SharpMocker.ShowKeyboardHelper")]
    public class KeyboardHelperAction : ActivateToolWindowActionHandler<ArgumentsGenerationSelectionDescriptor>
    {
        
    }
}