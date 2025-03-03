using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.Commands;
using TSVN.Helpers;

namespace TSVN.ToolWindows.PendingChanges.Commands;

[VisualStudioContribution]
internal class CommitToolbarCommand(
    VisualStudioExtensibility extensibility,
    CommandHelper commandHelper)
    : Command(extensibility)
{

    /// <inheritdoc />
    public override CommandConfiguration CommandConfiguration => new("%TSVN.CommitToolbarCommand.DisplayName%")
    {
        Icon = new(ImageMoniker.KnownValues.Stash, IconSettings.IconOnly)
    };

    /// <inheritdoc />
    public override async Task ExecuteCommandAsync(IClientContext clientContext, CancellationToken cancellationToken)
    {
        await commandHelper.RunTortoiseSvnCommand(clientContext, "commit", cancellationToken: cancellationToken);
    }
}
