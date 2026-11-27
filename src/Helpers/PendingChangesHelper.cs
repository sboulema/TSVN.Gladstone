using Microsoft.VisualStudio.Extensibility;
using System.Diagnostics;
using TSVN.Models;

namespace TSVN.Helpers;

/// <summary>
/// Retrieves the pending changes of the current Subversion working copy
/// and turns them into a tree that can be shown in the Pending Changes tool window.
/// </summary>
/// <param name="extensibility">The Visual Studio extensibility object, used to read options and open documents.</param>
/// <param name="fileHelper">Helper used to determine the repository root.</param>
/// <param name="commandHelper">Helper passed on to the tree items so they can run TortoiseSVN commands.</param>
public class PendingChangesHelper(
    VisualStudioExtensibility extensibility,
    FileHelper fileHelper,
    CommandHelper commandHelper)
{
    /// <summary>
    /// Gets the pending changes of the working copy as a tree with the repository root as its single top-level item.
    /// </summary>
    /// <param name="clientContext">
    /// The client context used to find the working directory when no solution is open. Can be <see langword="null"/>.
    /// </param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>
    /// The tree of pending changes and the number of changes found.
    /// When the repository root cannot be determined or <c>svn</c> fails, the tree contains only the (empty) root item.
    /// </returns>
    public async Task<PendingChangesModel> GetPendingChanges(
        IClientContext? clientContext = null,
        CancellationToken cancellationToken = default)
    {
        var repositoryRoot = await fileHelper.GetRepositoryRoot(clientContext, cancellationToken);

        var result = await GetPendingChanges(repositoryRoot, cancellationToken);

        var rootItem = new PendingChangeTreeViewItem(extensibility, commandHelper)
        {
            Label = repositoryRoot,
            FilePath = repositoryRoot,
            IsRoot = true,
        };

        var pendingChanges = ParsePendingChanges(result).ToList();

        foreach (var change in pendingChanges)
        {
            ProcessChange(commandHelper, extensibility, rootItem, change, change.FilePathParts);
        }

        SetMoniker(rootItem);

        return new()
        {
            PendingChanges = [rootItem],
            NumberOfPendingChanges = pendingChanges.Count,
        };
    }

    /// <summary>
    /// Parses the output lines of <c>svn status</c> into <see cref="PendingChange"/> objects.
    /// </summary>
    /// <remarks>
    /// Each line starts with a change type character, followed by status columns,
    /// and the relative file path from the ninth character onwards.
    /// Lines that are too short to contain a path are skipped.
    /// </remarks>
    /// <param name="pendingChanges">The raw output lines of <c>svn status</c>. Can be <see langword="null"/>.</param>
    /// <returns>The parsed changes, or an empty sequence when there is nothing to parse.</returns>
    private static IEnumerable<PendingChange> ParsePendingChanges(List<string>? pendingChanges)
        => pendingChanges?
        .Where(change => change.Length > 8)
        .Select(change => new PendingChange
        {
            ChangeType = change.First(),
            RelativeFilePath = change[8..],
            FilePathParts = change[8..].Split('\\', StringSplitOptions.RemoveEmptyEntries),
        }) ?? [];

    /// <summary>
    /// Runs <c>svn status</c> in the repository root and returns its output lines.
    /// </summary>
    /// <remarks>
    /// Unversioned files are left out when the <c>HideUnversioned</c> option is enabled.
    /// Failures are logged and result in an empty list instead of an exception.
    /// </remarks>
    /// <param name="repositoryRoot">The working copy root in which to run the command.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>
    /// One entry per output line, or an empty list when <c>svn</c> could not be started
    /// or exited with a non-zero exit code.
    /// </returns>
    private async Task<List<string>> GetPendingChanges(string repositoryRoot, CancellationToken cancellationToken = default)
    {
        try
        {
            var options = await OptionsHelper.GetOptions(extensibility, cancellationToken);

            var startInfo = new ProcessStartInfo
            {
                FileName = FileHelper.GetSvnExec(),
                WorkingDirectory = repositoryRoot,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
            };

            startInfo.ArgumentList.Add("status");

            if (options.HideUnversioned)
            {
                startInfo.ArgumentList.Add("-q");
            }

            using var proc = Process.Start(startInfo)
                ?? throw new InvalidOperationException("Unable to start svn.exe");

            // Read both streams concurrently so neither can fill up and block the process
            var errorTask = proc.StandardError.ReadToEndAsync(cancellationToken);
            var output = await proc.StandardOutput.ReadToEndAsync(cancellationToken);

            await proc.WaitForExitAsync(cancellationToken);

            if (proc.ExitCode != 0)
            {
                LogHelper.Log("GetPendingChanges", new InvalidOperationException(await errorTask));

                return [];
            }

            return [.. output.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries)];
        }
        catch (Exception e)
        {
            LogHelper.Log("GetPendingChanges", e);
        }

        return [];
    }

    /// <summary>
    /// Adds a change to the tree by walking down the folder structure of its path,
    /// creating any missing nodes on the way.
    /// </summary>
    /// <remarks>
    /// Folder nodes get their own path and no change type.
    /// Only the last path part, the actual file, carries the change type.
    /// </remarks>
    /// <param name="commandHelper">Helper passed on to newly created tree items.</param>
    /// <param name="extensibility">The extensibility object passed on to newly created tree items.</param>
    /// <param name="root">The node below which the remaining path parts are added.</param>
    /// <param name="change">The change being added.</param>
    /// <param name="filePathParts">The path parts of the change that are not yet represented by <paramref name="root"/> or its ancestors.</param>
    private static void ProcessChange(
        CommandHelper commandHelper,
        VisualStudioExtensibility extensibility,
        PendingChangeTreeViewItem root,
        PendingChange change,
        IEnumerable<string> filePathParts)
    {
        if (!filePathParts.Any())
        {
            return;
        }

        var label = filePathParts.First();
        var isLeaf = !filePathParts.Skip(1).Any();

        var child = root.Children
            .SingleOrDefault(x => x.Label == label);

        if (child == null)
        {
            child = new PendingChangeTreeViewItem(extensibility, commandHelper)
            {
                Label = label,
                FilePath = Path.Combine(root.FilePath, label),
                ChangeType = isLeaf ? change.ChangeType : default,
            };

            root.Children.Add(child);
        }

        ProcessChange(commandHelper, extensibility, child, change, filePathParts.Skip(1));
    }

    /// <summary>
    /// Sets the icon of an item and all of its descendants:
    /// a repository icon for the root, a folder icon for items with children and a document icon for files.
    /// </summary>
    /// <param name="item">The item to start from.</param>
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