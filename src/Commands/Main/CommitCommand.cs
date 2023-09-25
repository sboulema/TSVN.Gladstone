using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.Commands;
using TSVN.Helpers;

namespace TSVN.Commands;

/// <summary>
/// CommitCommand handler.
/// </summary>
[VisualStudioContribution]
internal class CommitCommand : Command
{
    private readonly CommandHelper _commandHelper;

    public CommitCommand(VisualStudioExtensibility extensibility, CommandHelper commandHelper)
        : base(extensibility)
    {
        _commandHelper = commandHelper;
    }

    /// <inheritdoc />
    public override CommandConfiguration CommandConfiguration => new("%TSVN.CommitCommand.DisplayName%")
    {
        // Use this object initializer to set optional parameters for the command. The required parameter,
        // displayName, is set above. DisplayName is localized and references an entry in .vsextension\string-resources.json.
        Icon = new(ImageMoniker.KnownValues.Commit, IconSettings.IconAndText)
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
        await _commandHelper.RunTortoiseSvnCommand(clientContext, "commit", cancellationToken: cancellationToken);
    }
}
