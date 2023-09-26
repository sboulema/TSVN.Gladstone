using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.Commands;
using System.Diagnostics;
using TSVN.Helpers;

namespace TSVN.Commands;

[VisualStudioContribution]
internal class DiskBrowserToolbarCommand : Command
{
    private readonly CommandHelper _commandHelper;
    private readonly FileHelper _fileHelper;

    public DiskBrowserToolbarCommand(VisualStudioExtensibility extensibility,
        CommandHelper commandHelper, FileHelper fileHelper)
        : base(extensibility)
    {
        _commandHelper = commandHelper;
        _fileHelper = fileHelper;
    }

    /// <inheritdoc />
    public override CommandConfiguration CommandConfiguration => new("%TSVN.DiskBrowserToolbarCommand.DisplayName%")
    {
        Icon = new(ImageMoniker.KnownValues.Computer, IconSettings.IconOnly)
    };

    /// <inheritdoc />
    public override Task InitializeAsync(CancellationToken cancellationToken)
    {
        return base.InitializeAsync(cancellationToken);
    }

    /// <inheritdoc />
    public override async Task ExecuteCommandAsync(IClientContext clientContext, CancellationToken cancellationToken)
    {
        var solutionDir = await _fileHelper.GetRepositoryRoot(clientContext, cancellationToken: cancellationToken);

        if (string.IsNullOrEmpty(solutionDir))
        {
            return;
        }

        Process.Start(solutionDir);
    }
}
