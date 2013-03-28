namespace SharpMocker
{
    using System.Windows.Forms;

    using JetBrains.ActionManagement;
    using JetBrains.Application.DataContext;

    [ActionHandler("SharpMocker.About")]
    public class AboutAction : IActionHandler
    {
        #region Public Methods and Operators

        public void Execute(IDataContext context, DelegateExecute nextExecute)
        {
            MessageBox.Show(
                "SharpMocker\nTesteroids\n\nDeclares and instanciates mocks to pass to a constructor.", 
                "About SharpMocker", 
                MessageBoxButtons.OK, 
                MessageBoxIcon.Information);
        }

        public bool Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate)
        {
            // return true or false to enable/disable this action
            return true;
        }

        #endregion
    }
}