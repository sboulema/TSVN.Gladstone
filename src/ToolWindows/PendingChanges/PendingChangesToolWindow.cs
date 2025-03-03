using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.Commands;
using Microsoft.VisualStudio.Extensibility.ToolWindows;
using Microsoft.VisualStudio.RpcContracts.RemoteUI;
using TSVN.Helpers;
using TSVN.Resources;
using TSVN.ToolWindows.PendingChanges.Commands;

namespace TSVN.ToolWindows.PendingChanges;

[VisualStudioContribution]
internal class PendingChangesToolWindow : ToolWindow
{
    private PendingChangesToolWindowData? _dataContext;
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
    public override async Task InitializeAsync(CancellationToken cancellationToken)
    {
        _dataContext = new PendingChangesToolWindowData(null, _pendingChangesHelper);
        await _dataContext.Refresh(cancellationToken);

        return;
    }

    /// <inheritdoc />
    public override Task<IRemoteUserControl> GetContentAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult<IRemoteUserControl>(new PendingChangesToolWindowControl(_dataContext));
    }

    public PendingChangesToolWindowData? DataContext
    {
        get => _dataContext;
    }
}
