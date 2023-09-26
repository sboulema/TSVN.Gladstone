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
            ToolbarChild.Command<ShowChangesCommand>(),
            ToolbarChild.Command<UpdateCommand>(),
            ToolbarChild.Command<CommitCommand>(),
            ToolbarChild.Separator,
            ToolbarChild.Command<ShowLogCommand>(),
            ToolbarChild.Command<DiskBrowserCommand>(),
            ToolbarChild.Command<RepoBrowserCommand>(),
            ToolbarChild.Separator,
            ToolbarChild.Command<CreatePatchCommand>(),
            ToolbarChild.Command<ApplyPatchCommand>(),
            ToolbarChild.Command<ShelveCommand>(),
            ToolbarChild.Command<UnshelveCommand>(),
            ToolbarChild.Separator,
            ToolbarChild.Command<BranchCommand>(),
            ToolbarChild.Command<SwitchCommand>(),
            ToolbarChild.Command<MergeCommand>(),
            ToolbarChild.Separator,
            ToolbarChild.Command<RevertCommand>(),
            ToolbarChild.Command<UpdateToRevisionCommand>(),
            ToolbarChild.Command<CleanupCommand>(),
            ToolbarChild.Command<LockCommand>(),
            ToolbarChild.Command<UnlockCommand>(),
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
            MenuChild.Command<DiskBrowserFileCommand>(),
            MenuChild.Command<RepoBrowserFileCommand>(),
            MenuChild.Separator,
            MenuChild.Command<BlameFileCommand>(),
            MenuChild.Separator,
            MenuChild.Command<MergeFileCommand>(),
            MenuChild.Command<UpdateToRevisionFileCommand>(),
            MenuChild.Command<PropertiesFileCommand>(),
            MenuChild.Separator,
            MenuChild.Command<UpdateFileCommand>(),
            MenuChild.Command<CommitFileCommand>(),
            MenuChild.Command<RevertFileCommand>(),
            MenuChild.Command<AddFileCommand>(),
            MenuChild.Command<DifferencesFileCommand>(),
            MenuChild.Command<DiffPreviousFileCommand>(),
            MenuChild.Command<DeleteFileCommand>(),
            MenuChild.Command<LockFileCommand>(),
            MenuChild.Command<UnlockFileCommand>(),
            MenuChild.Command<RenameFileCommand>(),
        },
    };

    // TODO: Place command in the Editor window
    // https://github.com/microsoft/VSExtensibility/issues/130

    // TODO: How to make sub menus?
    // https://github.com/microsoft/VSExtensibility/issues/65

    // TODO: Place menu in the Solution Explorer
    // https://github.com/microsoft/VSExtensibility/issues/260

    // TODO: Command keyboard shortcuts

    // TODO: Are toolbar commands icon only?

    // TODO: Fill string-resource.json with command labels
}
