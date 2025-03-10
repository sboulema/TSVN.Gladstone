using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.Commands;
using TSVN.Helpers;

namespace TSVN.ToolWindows.PendingChanges.Commands;

[VisualStudioContribution]
internal class RefreshToolbarCommand(
    VisualStudioExtensibility extensibility,
    PendingChangesHelper pendingChangesHelper)
    : Command(extensibility)
{
    /// <inheritdoc />
    public override CommandConfiguration CommandConfiguration => new("%TSVN.RefreshToolbarCommand.DisplayName%")
    {
        Icon = new(ImageMoniker.KnownValues.Refresh, IconSettings.IconOnly)
    };

    /// <inheritdoc />
    public override async Task ExecuteCommandAsync(IClientContext clientContext, CancellationToken cancellationToken)
    {
        var toolWindow = clientContext
            .Extensibility
            .Shell()
            .GetToolWindow<PendingChangesToolWindow>() as PendingChangesToolWindow;

        if (toolWindow?.DataContext == null)
        {
            return;
        }

        await toolWindow.DataContext.Refresh(clientContext, pendingChangesHelper, cancellationToken);
    }
}
