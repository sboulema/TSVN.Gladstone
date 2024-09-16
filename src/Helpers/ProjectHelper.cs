using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.ProjectSystem.Query;
using TSVN.Models;

namespace TSVN.Helpers;

#pragma warning disable VSEXTPREVIEW_PROJECTQUERY_TRACKING

public class ProjectHelper(
    VisualStudioExtensibility extensibility,
    CommandHelper commandHelper)
{
    public async Task Subscribe(CancellationToken cancellationToken)
    {
        var projects = await extensibility.Workspaces().QueryProjectsAsync(project => project, cancellationToken);

        var options = await OptionsHelper.GetOptions(extensibility, cancellationToken);

        foreach (var project in projects)
        {
            await project.Files
                .With(f => f.Path)
                .TrackUpdatesAsync(new TrackerObserver(commandHelper, options), cancellationToken);
        }
    }
}

public class TrackerObserver(
    CommandHelper commandHelper,
    Options options) : IObserver<IQueryTrackUpdates<IFileSnapshot>>
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
            ProcessFileUpdate(fileUpdate);
        }
    }

    private async Task ProcessFileUpdate(ItemUpdate<IFileSnapshot> fileUpdate)
    {
        if (options.OnItemAddedAddToSVN && fileUpdate.UpdateType == UpdateType.Added)
        {
            await commandHelper.RunTortoiseSvnCommand(fileUpdate.Current?.Path, "add");
        }
        else if (options.OnItemRenamedRenameInSVN && fileUpdate.UpdateType == UpdateType.Updated)
        {
            await commandHelper.RunTortoiseSvnCommand(fileUpdate.Current?.Path, "rename");
        }
        else if (options.OnItemRemovedRemoveFromSVN &&
            fileUpdate.UpdateType == UpdateType.Removed &&
            fileUpdate.PreviousId != null)
        {
            fileUpdate.PreviousId.TryGetValue("SourceItemName", out var sourceItemName);
            fileUpdate.PreviousId.TryGetValue("ProjectPath", out var projectPath);

            var projectDirectory = Path.GetDirectoryName(projectPath);

            if (string.IsNullOrEmpty(sourceItemName) ||
                string.IsNullOrEmpty(projectPath) ||
                string.IsNullOrEmpty(projectDirectory))
            {
                return;
            }

            var fileUpdatePreviousPath = Path.Combine(projectDirectory, sourceItemName);

            await commandHelper.RunTortoiseSvnCommand(fileUpdatePreviousPath, "remove");
        }
    }
}

#pragma warning restore VSEXTPREVIEW_PROJECTQUERY_TRACKING