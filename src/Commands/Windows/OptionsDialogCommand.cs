using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.Commands;
using Microsoft.VisualStudio.RpcContracts.Notifications;
using TSVN.Dialogs;
using TSVN.Helpers;
using TSVN.Resources;

namespace TSVN.Commands;

[VisualStudioContribution]
internal class OptionsDialogCommand(
    VisualStudioExtensibility extensibility,
    ProjectHelper projectHelper) : Command(extensibility)
{
    /// <inheritdoc />
    public override CommandConfiguration CommandConfiguration => new("%TSVN.OptionsDialogCommand.DisplayName%")
    {
        Icon = new(ImageMoniker.KnownValues.Settings, IconSettings.IconAndText),
        EnabledWhen = ActivationConstraint.SolutionState(SolutionState.Exists)
    };

    /// <inheritdoc />
    public override Task InitializeAsync(CancellationToken cancellationToken)
    {
        projectHelper.Subscribe(cancellationToken);

        return base.InitializeAsync(cancellationToken);
    }

    /// <inheritdoc />
    public override async Task ExecuteCommandAsync(IClientContext clientContext, CancellationToken cancellationToken)
    {
        var options = await OptionsHelper.GetOptions(Extensibility, cancellationToken);

        var optionsDialogData = new OptionsDialogData(Extensibility, options);

        var dialogResult = await Extensibility.Shell().ShowDialogAsync(
            new OptionsDialogControl(optionsDialogData),
            TSVNResources.OptionsDialogTitle,
            new(DialogButton.OKCancel, DialogResult.OK),
            cancellationToken);

        if (dialogResult == DialogResult.Cancel)
        {
            return;
        }

        options.RootFolder = optionsDialogData.RootFolder;
        options.OnItemAddedAddToSVN = optionsDialogData.OnItemAddedAddToSVN;
        options.OnItemRenamedRenameInSVN = optionsDialogData.OnItemRenamedRenameInSVN;
        options.OnItemRemovedRemoveFromSVN = optionsDialogData.OnItemRemovedRemoveFromSVN;
        options.CloseOnEnd = optionsDialogData.CloseOnEnd; 

        await OptionsHelper.SaveOptions(options, Extensibility, cancellationToken);
    }
}
