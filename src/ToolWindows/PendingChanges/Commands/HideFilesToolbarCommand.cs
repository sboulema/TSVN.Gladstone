using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.Commands;
using TSVN.Helpers;

namespace TSVN.ToolWindows.PendingChanges.Commands;

[VisualStudioContribution]
internal class HideFilesToolbarCommand(VisualStudioExtensibility extensibility)
    : Command(extensibility)
{
    private readonly VisualStudioExtensibility _extensibility = extensibility;

    /// <inheritdoc />
    public override CommandConfiguration CommandConfiguration => new("%TSVN.HideFilesToolbarCommand.DisplayName%")
    {
        Icon = new(ImageMoniker.KnownValues.ShowAllFiles, IconSettings.IconOnly),
        Flags = CommandFlags.CanToggle,
    };

    /// <inheritdoc />
    public override async Task ExecuteCommandAsync(IClientContext clientContext, CancellationToken cancellationToken)
    {
        var options = await OptionsHelper.GetOptions(_extensibility, cancellationToken);

        options.HideUnversioned = !options.HideUnversioned;

        await OptionsHelper.SaveOptions(options, _extensibility, cancellationToken);
    }
}
