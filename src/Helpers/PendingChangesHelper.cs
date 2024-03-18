using Microsoft.VisualStudio.Extensibility;
using System.Diagnostics;
using TSVN.Models;

namespace TSVN.Helpers;

public class PendingChangesHelper
{
    private readonly VisualStudioExtensibility _extensibility;
    private readonly FileHelper _fileHelper;

    public PendingChangesHelper(VisualStudioExtensibility extensibility,
        FileHelper fileHelper)
    {
        _extensibility = extensibility;
        _fileHelper = fileHelper;
    }

    public async Task<List<PendingChangeTreeViewItem>> GetPendingChanges(IClientContext clientContext, CancellationToken cancellationToken = default)
    {
        var repositoryRoot = await _fileHelper.GetRepositoryRoot(clientContext, cancellationToken);

        var pendingChanges = await GetPendingChanges(repositoryRoot, cancellationToken);

        var rootItem = new PendingChangeTreeViewItem
        {
            Label = $"Changes ({pendingChanges.Count})"
        };

        var repositoryRootItem = new PendingChangeTreeViewItem
        {
            Label = repositoryRoot
        };

        foreach (var change in pendingChanges)
        {
            if (change.Length <= 8)
            {
                continue;
            }

            var pathParts = change[8..].Split('\\');

            ProcessChange(repositoryRootItem, pathParts);
        }

        rootItem.Children.Add(repositoryRootItem);

        return new() { rootItem };
    }

    private async Task<List<string>> GetPendingChanges(string repositoryRoot, CancellationToken cancellationToken = default)
    {
        var pendingChanges = new List<string>();

        try
        {
            var options = await OptionsHelper.GetOptions(_extensibility, cancellationToken);

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
                pendingChanges.Add(await proc.StandardOutput.ReadLineAsync());
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
        if (pathParts.Any())
        {
            var label = pathParts.First();
            var child = root.Children.Where(x => x.Label == label).SingleOrDefault();
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
}
