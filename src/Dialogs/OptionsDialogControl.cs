using Microsoft.VisualStudio.Extensibility.UI;

namespace TSVN.Dialogs;

public class OptionsDialogControl : RemoteUserControl
{
    public OptionsDialogControl(object? dataContext, SynchronizationContext? synchronizationContext = null)
        : base(dataContext, synchronizationContext)
    {
    }
}
