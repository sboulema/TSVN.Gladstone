using Microsoft.VisualStudio.Extensibility;
using System.Diagnostics;
using TSVN.Models;

namespace TSVN.Helpers;

public class PendingChangesHelper(
    VisualStudioExtensibility extensibility,
    FileHelper fileHelper)
{
    public async Task<List<PendingChangeTreeViewItem>> GetPendingChanges(IClientContext clientContext, CancellationToken cancellationToken = default)
    {
        var repositoryRoot = await fileHelper.GetRepositoryRoot(clientContext, cancellationToken);

        var pendingChanges = await GetPendingChanges(repositoryRoot, cancellationToken);

        var rootItem = new PendingChangeTreeViewItem
        {
            Label = repositoryRoot
        };

        foreach (var change in pendingChanges)
        {
            if (change.Length <= 8)
            {
                continue;
            }

            var pathParts = change[8..].Split('\\', StringSplitOptions.RemoveEmptyEntries);

            ProcessChange(rootItem, pathParts);
        }

        return [rootItem];
    }

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

    private static void ProcessChange(PendingChangeTreeViewItem root, IEnumerable<string> pathParts)
    {
        if (!pathParts.Any())
        {
            return;
        }

        var label = pathParts.First();
        var child = root.Children
            .Where(x => x.Label == label)
            .SingleOrDefault();
        
        if (child == null)
        {
            child = new PendingChangeTreeViewItem()
            {
                Label = label
            };
            root.Children.Add(child);
        }
        
        ProcessChange(child, pathParts.Skip(1));
    }
}
