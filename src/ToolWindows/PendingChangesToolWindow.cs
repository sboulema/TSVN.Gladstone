using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.ToolWindows;
using Microsoft.VisualStudio.RpcContracts.RemoteUI;
using TSVN.Helpers;
using TSVN.Resources;

namespace TSVN.ToolWindows;

[VisualStudioContribution]
public class PendingChangesToolWindow : ToolWindow
{
    private PendingChangesToolWindowData? dataContext;
    private PendingChangesHelper _pendingChangesHelper;

    public PendingChangesToolWindow(
        PendingChangesHelper pendingChangesHelper)
    {
        _pendingChangesHelper = pendingChangesHelper;

        Title = TSVNResources.PendingChangesToolWindowTitle;
    }

    /// <inheritdoc />
    public override ToolWindowConfiguration ToolWindowConfiguration => new()
    {
        Placement = ToolWindowPlacement.Floating,
    };

    /// <inheritdoc />
    public override async Task InitializeAsync(CancellationToken cancellationToken)
    {
        dataContext = new PendingChangesToolWindowData(null, _pendingChangesHelper);
        await dataContext.Refresh(null, cancellationToken);

        return;
    }

    /// <inheritdoc />
    public override Task<IRemoteUserControl> GetContentAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult<IRemoteUserControl>(new PendingChangesToolWindowControl(dataContext));
    }
}
