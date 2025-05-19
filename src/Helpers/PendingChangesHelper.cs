using Microsoft.VisualStudio.Extensibility;
using System.Diagnostics;
using TSVN.Models;

namespace TSVN.Helpers;

public class PendingChangesHelper(
    VisualStudioExtensibility extensibility,
    FileHelper fileHelper)
{
    public async Task<PendingChangesModel> GetPendingChanges(
        IClientContext clientContext, 
        CancellationToken cancellationToken = default)
    {
        var repositoryRoot = await fileHelper.GetRepositoryRoot(clientContext, cancellationToken);

        var result = await GetPendingChanges(repositoryRoot, cancellationToken);

        var rootItem = new PendingChangeTreeViewItem
        {
            Label = repositoryRoot,
            IsRoot = true,
        };

        var pendingChanges = ParsePendingChanges(result);

        pendingChanges
            .ToList()
            .ForEach(change => ProcessChange(rootItem, change, change.FilePathParts));

        SetMoniker(rootItem);

        return new()
        {
            PendingChanges = [rootItem],
            NumberOfPendingChanges = result.Count,
        };
    }

    private static IEnumerable<PendingChange> ParsePendingChanges(List<string>? pendingChanges)
        => pendingChanges?
        .Where(change => change.Length > 8)
        .Select(change => new PendingChange
        {
            ChangeType = change.First(),
            FilePath = change[8..],
            FilePathParts = change[8..].Split('\\', StringSplitOptions.RemoveEmptyEntries),
        }) ?? [];

    private async Task<List<string>> GetPendingChanges(string repositoryRoot, CancellationToken cancellationToken = default)
    {
        var pendingChanges = new List<string>();

        try
        {
            var options = await OptionsHelper.GetOptions(extensibility, cancellationToken);

            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c cd /D \"{repositoryRoot}\" && \"{FileHelper.GetSvnExec()}\" status" + (options.HideUnversioned ? " -q" : string.Empty),
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                }
            };

            proc.Start();

            while (!proc.StandardOutput.EndOfStream)
            {
                pendingChanges.Add(await proc.StandardOutput.ReadLineAsync(cancellationToken));
            }
        }
        catch (Exception e)
        {
            LogHelper.Log("GetPendingChanges", e);
        }

        return pendingChanges;
    }

    private static void ProcessChange(
        PendingChangeTreeViewItem root,
        PendingChange change,
        IEnumerable<string> filePathParts)
    {
        if (!filePathParts.Any())
        {
            return;
        }

        var label = filePathParts.First();
        var child = root.Children
            .Where(x => x.Label == label)
            .SingleOrDefault();
        
        if (child == null)
        {
            child = new PendingChangeTreeViewItem()
            {
                Label = label,
                ChangeType = change.ChangeType,
                Moniker = change.Moniker,
            };
            root.Children.Add(child);
        }
        
        ProcessChange(child, change, filePathParts.Skip(1));
    }

    private static void SetMoniker(PendingChangeTreeViewItem item)
    {
        if (item.IsRoot)
        {
            item.Moniker = ImageMoniker.KnownValues.Repository;
        }
        else
        {
            item.Moniker = item.Children.Any()
                ? ImageMoniker.KnownValues.FolderClosed
                : ImageMoniker.KnownValues.Document;
        }

        item.Children.ForEach(SetMoniker);
    }
}
