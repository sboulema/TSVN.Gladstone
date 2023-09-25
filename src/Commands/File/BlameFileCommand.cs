using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.Commands;
using TSVN.Helpers;

namespace TSVN.Commands;

[VisualStudioContribution]
internal class BlameFileCommand : Command
{
    private readonly CommandHelper _commandHelper;

    public BlameFileCommand(VisualStudioExtensibility extensibility, CommandHelper commandHelper)
        : base(extensibility)
    {
        _commandHelper = commandHelper;
    }

    /// <inheritdoc />
    public override CommandConfiguration CommandConfiguration => new("%TSVN.BlameFileCommand.DisplayName%")
    {
        Icon = new(ImageMoniker.KnownValues.Comment, IconSettings.IconAndText)
    };

    /// <inheritdoc />
    public override Task InitializeAsync(CancellationToken cancellationToken)
    {
        return base.InitializeAsync(cancellationToken);
    }

    /// <inheritdoc />
    public override async Task ExecuteCommandAsync(IClientContext clientContext, CancellationToken cancellationToken)
    {
        var textView = await clientContext.GetActiveTextViewAsync(cancellationToken);
        var lineNumber = textView?.Selection.ActivePosition.GetContainingLine().LineNumber;

        await _commandHelper.RunTortoiseSvnCommand(clientContext, "blame", $"/line:{lineNumber}",
            isFileCommand: true, cancellationToken: cancellationToken);
    }
}
