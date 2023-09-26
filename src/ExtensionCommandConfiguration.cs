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
            ToolbarChild.Command<ShowChangesToolbarCommand>(),
            ToolbarChild.Command<UpdateToolbarCommand>(),
            ToolbarChild.Command<CommitToolbarCommand>(),
            ToolbarChild.Separator,
            ToolbarChild.Command<ShowLogToolbarCommand>(),
            ToolbarChild.Command<DiskBrowserToolbarCommand>(),
            ToolbarChild.Command<RepoBrowserToolbarCommand>(),
            ToolbarChild.Separator,
            ToolbarChild.Command<CreatePatchToolbarCommand>(),
            ToolbarChild.Command<ApplyPatchToolbarCommand>(),
            ToolbarChild.Command<ShelveToolbarCommand>(),
            ToolbarChild.Command<UnshelveToolbarCommand>(),
            ToolbarChild.Separator,
            ToolbarChild.Command<BranchToolbarCommand>(),
            ToolbarChild.Command<SwitchToolbarCommand>(),
            ToolbarChild.Command<MergeToolbarCommand>(),
            ToolbarChild.Separator,
            ToolbarChild.Command<RevertToolbarCommand>(),
            ToolbarChild.Command<UpdateToRevisionToolbarCommand>(),
            ToolbarChild.Command<CleanupToolbarCommand>(),
            ToolbarChild.Command<LockToolbarCommand>(),
            ToolbarChild.Command<UnlockToolbarCommand>(),
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
        Placements = new[]
        {
            // Node.SourceControlGroup
            CommandPlacement.FromVsctParent(new Guid("{d309f791-903f-11d0-9efc-00a0c911004f}"), 527),
            // ProjectNode.SourceControlGroup
            CommandPlacement.FromVsctParent(new Guid("{d309f791-903f-11d0-9efc-00a0c911004f}"), 534),
            // SolutionNode.SourceControlGroup
            CommandPlacement.FromVsctParent(new Guid("{d309f791-903f-11d0-9efc-00a0c911004f}"), 535),
        },
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

    // TODO: Place menu in the Solution Explorer
    // https://github.com/microsoft/VSExtensibility/issues/260

    // TODO: How to add menu keyboard letters 
}
