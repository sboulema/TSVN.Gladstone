using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.Commands;
using TSVN.Helpers;

namespace TSVN.Commands;

[VisualStudioContribution]
internal class UnshelveCommand : Command
{
    private readonly CommandHelper _commandHelper;

    public UnshelveCommand(VisualStudioExtensibility extensibility, CommandHelper commandHelper)
        : base(extensibility)
    {
        _commandHelper = commandHelper;
    }

    /// <inheritdoc />
    public override CommandConfiguration CommandConfiguration => new("%TSVN.UnshelveCommand.DisplayName%")
    {
        Icon = new(ImageMoniker.KnownValues.UnshelvePendingChanges, IconSettings.IconAndText),
        Shortcuts = new[] { new CommandShortcutConfiguration(ModifierKey.ControlShift, Key.S, ModifierKey.LeftAlt, Key.P) }
    };

    /// <inheritdoc />
    public override Task InitializeAsync(CancellationToken cancellationToken)
    {
        return base.InitializeAsync(cancellationToken);
    }

    /// <inheritdoc />
    public override async Task ExecuteCommandAsync(IClientContext clientContext, CancellationToken cancellationToken)
    {
        await _commandHelper.RunTortoiseSvnCommand(clientContext, "unshelve", cancellationToken: cancellationToken);
    }
}
