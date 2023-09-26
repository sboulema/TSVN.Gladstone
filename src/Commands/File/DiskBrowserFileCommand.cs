using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.Commands;
using TSVN.Helpers;

namespace TSVN.Commands;

[VisualStudioContribution]
internal class DiskBrowserFileCommand : Command
{
    private readonly CommandHelper _commandHelper;

    public DiskBrowserFileCommand(VisualStudioExtensibility extensibility,
        CommandHelper commandHelper)
        : base(extensibility)
    {
        _commandHelper = commandHelper;
    }

    /// <inheritdoc />
    public override CommandConfiguration CommandConfiguration => new("%TSVN.DiskBrowserFileCommand.DisplayName%")
    {
        Icon = new(ImageMoniker.KnownValues.Computer, IconSettings.IconAndText)
    };

    /// <inheritdoc />
    public override Task InitializeAsync(CancellationToken cancellationToken)
    {
        return base.InitializeAsync(cancellationToken);
    }

    /// <inheritdoc />
    public override async Task ExecuteCommandAsync(IClientContext clientContext, CancellationToken cancellationToken)
    {
        var filePath = await FileHelper.GetPath(clientContext, cancellationToken);

        if (string.IsNullOrEmpty(filePath))
        {
            return;
        }

        await _commandHelper.StartProcess("explorer.exe", filePath, cancellationToken);
    }
}
