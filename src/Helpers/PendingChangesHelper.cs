using Microsoft.VisualStudio.Extensibility;
using System.Diagnostics;
using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
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
            Moniker = ImageMoniker.KnownValues.Repository,
        };

        var pendingChanges = ParsePendingChanges(result);

        pendingChanges.ToList().ForEach(change => ProcessChange(rootItem, change, change.FilePathParts, repositoryRoot));

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
            Moniker = ImageMoniker.KnownValues.FolderClosed,
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
        IEnumerable<string> filePathParts,
        string repositoryRoot)
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
                //Icon = GetIcon(repositoryRoot, pathParts),
                Moniker = change.Moniker,
            };
            root.Children.Add(child);
        }
        
        ProcessChange(child, change, filePathParts.Skip(1), repositoryRoot);
    }

    private static BitmapSource? GetIcon(string repositoryRoot, IEnumerable<string> pathParts)
    {
        var filePath = Path.Combine(repositoryRoot, string.Join('\\', pathParts));

        if (string.IsNullOrEmpty(filePath))
        {
            return null;
        }

        var icon = Icon.ExtractAssociatedIcon(filePath);

        if (icon == null)
        {
            return null;
        }

        return ToImageSource(icon);
    }

    private static BitmapSource ToImageSource(Icon icon)
        => Imaging.CreateBitmapSourceFromHIcon(
            icon.Handle,
            Int32Rect.Empty,
            BitmapSizeOptions.FromEmptyOptions());
}
