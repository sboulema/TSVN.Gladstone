using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.Commands;
using TSVN.Helpers;

namespace TSVN.Commands;

[VisualStudioContribution]
internal class RevertCommand : Command
{
    private readonly CommandHelper _commandHelper;

    public RevertCommand(VisualStudioExtensibility extensibility, CommandHelper commandHelper)
        : base(extensibility)
    {
        _commandHelper = commandHelper;
    }

    /// <inheritdoc />
    public override CommandConfiguration CommandConfiguration => new("%TSVN.RevertCommand.DisplayName%")
    {
        Icon = new(ImageMoniker.KnownValues.Undo, IconSettings.IconAndText),
        Shortcuts = new[] { new CommandShortcutConfiguration(ModifierKey.ControlShift, Key.S, ModifierKey.LeftAlt, Key.R) }
    };

    /// <inheritdoc />
    public override Task InitializeAsync(CancellationToken cancellationToken)
    {
        return base.InitializeAsync(cancellationToken);
    }

    /// <inheritdoc />
    public override async Task ExecuteCommandAsync(IClientContext clientContext, CancellationToken cancellationToken)
    {
        await _commandHelper.RunTortoiseSvnCommand(clientContext, "revert", cancellationToken: cancellationToken);
    }
}
