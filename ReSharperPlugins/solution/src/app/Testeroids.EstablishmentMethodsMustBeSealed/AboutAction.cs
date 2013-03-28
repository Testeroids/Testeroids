using System.Windows.Forms;
using JetBrains.ActionManagement;
using JetBrains.Application.DataContext;

namespace Testeroids.EstablishmentMethodsMustBeSealed
{
  [ActionHandler("Testeroids.EstablishmentMethodsMustBeSealed.About")]
  public class AboutAction : IActionHandler
  {
    public bool Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate)
    {
      // return true or false to enable/disable this action
      return true;
    }

    public void Execute(IDataContext context, DelegateExecute nextExecute)
    {
      MessageBox.Show(
        "Establishment Methods Must Be Sealed\nTesteroids\n\nProvides a quick fix to enforce sealing of context establishment methods",
        "About Establishment Methods Must Be Sealed",
        MessageBoxButtons.OK,
        MessageBoxIcon.Information);
    }
  }
}
