using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.Commands;
using TSVN.Helpers;

namespace TSVN.Commands;

[VisualStudioContribution]
internal class ShelveCommand : Command
{
    private readonly CommandHelper _commandHelper;

    public ShelveCommand(VisualStudioExtensibility extensibility, CommandHelper commandHelper)
        : base(extensibility)
    {
        _commandHelper = commandHelper;
    }

    /// <inheritdoc />
    public override CommandConfiguration CommandConfiguration => new("%TSVN.ShelveCommand.DisplayName%")
    {
        Icon = new(ImageMoniker.KnownValues.ShelvePendingChanges, IconSettings.IconAndText),
        Shortcuts = new[] { new CommandShortcutConfiguration(ModifierKey.ControlShift, Key.S, ModifierKey.LeftAlt, Key.E) }
    };

    /// <inheritdoc />
    public override Task InitializeAsync(CancellationToken cancellationToken)
    {
        return base.InitializeAsync(cancellationToken);
    }

    /// <inheritdoc />
    public override async Task ExecuteCommandAsync(IClientContext clientContext, CancellationToken cancellationToken)
    {
        await _commandHelper.RunTortoiseSvnCommand(clientContext, "shelve", cancellationToken: cancellationToken);
    }
}
