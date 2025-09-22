using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.Commands;
using Microsoft.VisualStudio.Extensibility.ToolWindows;
using Microsoft.VisualStudio.RpcContracts.RemoteUI;
using TSVN.Helpers;
using TSVN.Resources;
using TSVN.ToolWindows.PendingChanges.Commands;

namespace TSVN.ToolWindows.PendingChanges;

[VisualStudioContribution]
internal class PendingChangesToolWindow(PendingChangesHelper pendingChangesHelper) : ToolWindow
{
    private readonly PendingChangesToolWindowData _dataContext = new();

    /// <inheritdoc />
    public override ToolWindowConfiguration ToolWindowConfiguration => new()
    {
        Placement = ToolWindowPlacement.Floating,
        Toolbar = new ToolWindowToolbar(Toolbar)
    };

    [VisualStudioContribution]
    private static ToolbarConfiguration Toolbar => new("%ToolWindowSample.MyToolWindow.Toolbar.DisplayName%")
    {
        Children = [
            ToolbarChild.Command<CommitToolbarCommand>(),
            ToolbarChild.Command<RevertToolbarCommand>(),
            ToolbarChild.Command<HideFilesToolbarCommand>(),
            ToolbarChild.Command<RefreshToolbarCommand>(),
        ],
    };

    /// <inheritdoc />
    public override Task InitializeAsync(CancellationToken cancellationToken)
    {
        Title = TSVNResources.PendingChangesToolWindowTitle;
        
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public override Task<IRemoteUserControl> GetContentAsync(CancellationToken cancellationToken)
        => Task.FromResult<IRemoteUserControl>(new PendingChangesToolWindowControl(_dataContext));

    public override async Task OnShowAsync(CancellationToken cancellationToken)
        => await Refresh(cancellationToken);

    public async Task Refresh(
        CancellationToken cancellationToken,
        IClientContext? clientContext = null)
    {
        var result = await pendingChangesHelper.GetPendingChanges(clientContext, cancellationToken);

        _dataContext.Items = result.PendingChanges;
        _dataContext.ChangesHeader = $"Changes ({result.NumberOfPendingChanges})";
    }
}
