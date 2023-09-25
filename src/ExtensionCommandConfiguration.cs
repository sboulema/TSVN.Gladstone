using Microsoft.VisualStudio.Extensibility.Commands;
using Microsoft.VisualStudio.Extensibility;
using TSVN.Commands;

namespace TSVN;

internal class ExtensionCommandConfiguration
{
    [VisualStudioContribution]
    public static ToolbarConfiguration ToolBar => new("%TSVN.ToolBar.DisplayName%")
    {
        Children = new[]
        {
            ToolbarChild.Command<CommitCommand>(),
            // TODO: Add toolbar children
        },
    };

    [VisualStudioContribution]
    public static MenuConfiguration MainMenu => new("%TSVN.MainMenu.DisplayName%")
    {
        Placements = new[] { CommandPlacement.KnownPlacements.ExtensionsMenu },
        Children = new[]
        {
            MenuChild.Command<ShowChangesCommand>(),
            MenuChild.Command<UpdateCommand>(),
            MenuChild.Command<CommitCommand>(),
            MenuChild.Separator,
            MenuChild.Command<ShowLogCommand>(),
            MenuChild.Command<DiskBrowserCommand>(),
            MenuChild.Command<RepoBrowserCommand>(),
            MenuChild.Separator,
            MenuChild.Command<CreatePatchCommand>(),
            MenuChild.Command<ApplyPatchCommand>(),
            MenuChild.Command<ShelveCommand>(),
            MenuChild.Command<UnshelveCommand>(),
            MenuChild.Separator,
            MenuChild.Command<BranchCommand>(),
            MenuChild.Command<SwitchCommand>(),
            MenuChild.Command<MergeCommand>(),
            MenuChild.Separator,
            MenuChild.Command<RevertCommand>(),
            MenuChild.Command<UpdateToRevisionCommand>(),
            MenuChild.Command<CleanupCommand>(),
            MenuChild.Command<LockCommand>(),
            MenuChild.Command<UnlockCommand>(),
            MenuChild.Separator,
            MenuChild.Menu(FileMenu)
        },
    };

    [VisualStudioContribution]
    public static MenuConfiguration FileMenu => new("%TSVN.FileMenu.DisplayName%")
    {
        Placements = new[] { CommandPlacement.KnownPlacements.ExtensionsMenu },
        Children = new[]
        {
            MenuChild.Command<ShowLogFileCommand>(),
            // Disk browser
            // Repo browser
            MenuChild.Separator,
            MenuChild.Command<BlameFileCommand>(),
            MenuChild.Separator,
            MenuChild.Command<MergeFileCommand>(),
            // Update to revision
            // Properties
            MenuChild.Separator,
            // Update
            // Commit
            // Revert
            MenuChild.Command<AddFileCommand>(),
            // Show differences
            // Diff with previous version
            // Delete
            // Get lock
            // Release lock
            // Rename
        },
    };

    // TODO: Place command in the Editor window
    // https://github.com/microsoft/VSExtensibility/issues/130

    // TODO: How to make sub menus?
    // https://github.com/microsoft/VSExtensibility/issues/65

    // TODO: Place menu in the Solution Explorer
    // https://github.com/microsoft/VSExtensibility/issues/260

    // TODO: Command keyboard shortcuts
}
