using Microsoft.VisualStudio.Extensibility.UI;

namespace TSVN.ToolWindows.PendingChanges;

internal class PendingChangesToolWindowControl : RemoteUserControl
{
    public PendingChangesToolWindowControl(object? dataContext, SynchronizationContext? synchronizationContext = null)
        : base(dataContext, synchronizationContext)
    {
    }
}
