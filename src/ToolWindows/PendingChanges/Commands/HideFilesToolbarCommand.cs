using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.Commands;
using TSVN.Helpers;

namespace TSVN.ToolWindows.PendingChanges.Commands;

[VisualStudioContribution]
internal class HideFilesToolbarCommand(
    VisualStudioExtensibility extensibility,
    CommandHelper commandHelper)
    : Command(extensibility)
{

    /// <inheritdoc />
    public override CommandConfiguration CommandConfiguration => new("%TSVN.HideFilesToolbarCommand.DisplayName%")
    {
        Icon = new(ImageMoniker.KnownValues.ShowAllFiles, IconSettings.IconOnly),
        Flags = CommandFlags.CanToggle,
    };

    /// <inheritdoc />
    public override async Task ExecuteCommandAsync(IClientContext clientContext, CancellationToken cancellationToken)
    {
        await commandHelper.RunTortoiseSvnCommand(clientContext, "revert", cancellationToken: cancellationToken);
    }
}
