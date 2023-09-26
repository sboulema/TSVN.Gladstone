﻿using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.Commands;
using TSVN.Helpers;

namespace TSVN.Commands;

/// <summary>
/// CommitCommand handler.
/// </summary>
[VisualStudioContribution]
internal class RepoBrowserFileCommand : Command
{
    private readonly CommandHelper _commandHelper;

    public RepoBrowserFileCommand(VisualStudioExtensibility extensibility, CommandHelper commandHelper)
        : base(extensibility)
    {
        _commandHelper = commandHelper;
    }

    /// <inheritdoc />
    public override CommandConfiguration CommandConfiguration => new("%TSVN.RepoBrowserFileCommand.DisplayName%")
    {
        Icon = new(ImageMoniker.KnownValues.Repository, IconSettings.IconAndText)
    };

    /// <inheritdoc />
    public override Task InitializeAsync(CancellationToken cancellationToken)
    {
        // Use InitializeAsync for any one-time setup or initialization.
        return base.InitializeAsync(cancellationToken);
    }

    /// <inheritdoc />
    public override async Task ExecuteCommandAsync(IClientContext clientContext, CancellationToken cancellationToken)
    {
        await _commandHelper.RunTortoiseSvnCommand(clientContext, "repobrowser",
            isFileCommand: true, cancellationToken: cancellationToken);
    }
}
