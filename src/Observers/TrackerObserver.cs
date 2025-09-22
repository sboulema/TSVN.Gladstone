using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.ProjectSystem.Query;
using TSVN.Helpers;

namespace TSVN.Observers;

#pragma warning disable VSEXTPREVIEW_PROJECTQUERY_TRACKING

public class TrackerObserver(
    CommandHelper commandHelper,
    VisualStudioExtensibility extensibility,
    CancellationToken cancellationToken) : IObserver<IQueryTrackUpdates<IFileSnapshot>>
{
    public void OnCompleted()
    {
    }

    public void OnError(Exception error)
    {
    }

    public void OnNext(IQueryTrackUpdates<IFileSnapshot> value)
    {
        foreach (var fileUpdate in value.Updates)
        {
            _ = ProcessFileUpdate(fileUpdate);
        }
    }

    private async Task ProcessFileUpdate(ItemUpdate<IFileSnapshot> fileUpdate)
    {
        var options = await OptionsHelper.GetOptions(extensibility, cancellationToken);

        if (options.OnItemAddedAddToSVN &&
            fileUpdate.UpdateType == UpdateType.Added &&
            !string.IsNullOrEmpty(fileUpdate.Current?.Path))
        {
            await commandHelper.RunTortoiseSvnCommand(fileUpdate.Current.Path, "add");
        }
        else if (options.OnItemRenamedRenameInSVN &&
            fileUpdate.UpdateType == UpdateType.Updated &&
            !string.IsNullOrEmpty(fileUpdate.Current?.Path))
        {
            var fileUpdatePreviousPath = GetFileUpdatePreviousPath(fileUpdate);

            // File was not renamed
            if (fileUpdate.Current.Path == fileUpdatePreviousPath)
            {
                return;
            }

            // Temporarily rename the new file to the old file 
            File.Move(fileUpdate.Current.Path, fileUpdatePreviousPath);

            // So that we can svn rename it properly
            await commandHelper.StartProcess(FileHelper.GetSvnExec(), $"mv {fileUpdatePreviousPath} {fileUpdate.Current.Path}", default);
        }
        else if (options.OnItemRemovedRemoveFromSVN && fileUpdate.UpdateType == UpdateType.Removed)
        {
            var fileUpdatePreviousPath = GetFileUpdatePreviousPath(fileUpdate);

            await commandHelper.RunTortoiseSvnCommand(fileUpdatePreviousPath, "remove");
        }
    }

    private static string GetFileUpdatePreviousPath(ItemUpdate<IFileSnapshot> fileUpdate)
    {
        if (fileUpdate.PreviousId == null)
        {
            return string.Empty;
        }

        fileUpdate.PreviousId.TryGetValue("SourceItemName", out var sourceItemName);
        fileUpdate.PreviousId.TryGetValue("ProjectPath", out var projectPath);

        var projectDirectory = Path.GetDirectoryName(projectPath);

        if (string.IsNullOrEmpty(sourceItemName) ||
            string.IsNullOrEmpty(projectPath) ||
            string.IsNullOrEmpty(projectDirectory))
        {
            return string.Empty;
        }

        return Path.Combine(projectDirectory, sourceItemName);
    }
}

#pragma warning restore VSEXTPREVIEW_PROJECTQUERY_TRACKING