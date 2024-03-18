using TSVN.ToolWindows;
using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.Commands;

namespace TSVN.Commands.Windows;

[VisualStudioContribution]
public class PendingChangesToolWindowCommand : Command
{
    /// <inheritdoc />
    public override CommandConfiguration CommandConfiguration => new("%TSVN.PendingChangesToolWindowCommand.DisplayName%")
    {
        Icon = new(ImageMoniker.KnownValues.PendingChange, IconSettings.IconAndText),
        EnabledWhen = ActivationConstraint.SolutionState(SolutionState.Exists)
    };

    /// <inheritdoc />
    public override async Task ExecuteCommandAsync(IClientContext context, CancellationToken cancellationToken)
    {
        await Extensibility.Shell().ShowToolWindowAsync<PendingChangesToolWindow>(activate: true, cancellationToken);
    }
}
