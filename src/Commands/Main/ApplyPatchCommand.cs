﻿using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.Commands;
using Microsoft.Win32;
using TSVN.Helpers;

namespace TSVN.Commands;

[VisualStudioContribution]
internal class ApplyPatchCommand(
    VisualStudioExtensibility extensibility,
    CommandHelper commandHelper,
    FileHelper fileHelper) : Command(extensibility)
{
    /// <inheritdoc />
    public override CommandConfiguration CommandConfiguration => new("%TSVN.ApplyPatchCommand.DisplayName%")
    {
        Icon = new(ImageMoniker.KnownValues.Stash, IconSettings.IconAndText),
        Shortcuts = [new CommandShortcutConfiguration(ModifierKey.ControlShift, Key.S, ModifierKey.LeftAlt, Key.H)]
    };

    /// <inheritdoc />
    public override Task InitializeAsync(CancellationToken cancellationToken)
    {
        return base.InitializeAsync(cancellationToken);
    }

    /// <inheritdoc />
    public override async Task ExecuteCommandAsync(IClientContext clientContext, CancellationToken cancellationToken)
    {
        var solutionDir = await fileHelper.GetRepositoryRoot(clientContext, cancellationToken: cancellationToken);

        if (string.IsNullOrEmpty(solutionDir))
        {
            return;
        }

        var openFileDialog = new OpenFileDialog
        {
            Filter = "Patch Files (.patch)|*.patch|All Files (*.*)|*.*",
            FilterIndex = 1,
            Multiselect = false
        };

        var success = openFileDialog.ShowDialog();

        if (success != true)
        {
            return;
        }

        await commandHelper.StartProcess(
            "TortoiseMerge.exe",
            $"/diff:\"{openFileDialog.FileName}\" /patchpath:\"{solutionDir}\"",
            cancellationToken);
    }
}
