using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.Shell;
using Microsoft.VisualStudio.ProjectSystem.Query;
using Microsoft.Win32;
using System.Diagnostics;
using System.Threading;

namespace TSVN.Helpers;

public class FileHelper(VisualStudioExtensibility extensibility)
{
    private const string DEFAULT_PROC_PATH = @"C:\Program Files\TortoiseSVN\bin\TortoiseProc.exe";

    public static string GetTortoiseSvnProc()
    {
        var path = GetRegKeyValue();

        return !string.IsNullOrEmpty(path)
            ? path : DEFAULT_PROC_PATH;
    }

    public static string GetSvnExec()
        => GetTortoiseSvnProc().Replace("TortoiseProc.exe", "svn.exe");

    public async Task<string> GetRepositoryRoot(
        IClientContext? clientContext = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Override any logic with the solution specific Root Folder setting
            var options = await OptionsHelper.GetOptions(extensibility, cancellationToken);

            if (!string.IsNullOrEmpty(options.RootFolder))
            {
                return options.RootFolder;
            }

            var workingDirectory = await GetWorkingDirectory(clientContext, cancellationToken);

            // No solution or file open, we have no way of determining repository root.
            // Fail silently.
            if (string.IsNullOrEmpty(workingDirectory))
            {
                return string.Empty;
            }

            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c cd /D \"{workingDirectory}\" && \"{GetSvnExec()}\" info --show-item wc-root",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                }
            };

            proc.Start();

            var errors = string.Empty;

            while (!proc.StandardError.EndOfStream)
            {
                errors += await proc.StandardError.ReadLineAsync();
            }

            while (!proc.StandardOutput.EndOfStream)
            {
                options.RootFolder = await proc.StandardOutput.ReadLineAsync() ?? string.Empty;
            }

            await OptionsHelper.SaveOptions(options, extensibility, cancellationToken);

            if (string.IsNullOrEmpty(options.RootFolder))
            {
                await ShowMissingSolutionDirMessage(cancellationToken);
            }

            return options.RootFolder;
        }
        catch (Exception e)
        {
            LogHelper.Log(exception: e);
        }

        await ShowMissingSolutionDirMessage(cancellationToken);

        return string.Empty;
    }

    public static async Task<string> GetSolutionDirectory(VisualStudioExtensibility extensibility,
        CancellationToken cancellationToken)
    {
        var result = await extensibility
            .Workspaces()
            .QuerySolutionAsync(solution => solution.With(solution => solution.Directory), cancellationToken);

        return result.FirstOrDefault()?.Directory ?? string.Empty;
    }

    /// <summary>
    /// Get the path of the file on which to act upon. 
    /// This can be different depending on where the TSVN context menu was used
    /// </summary>
    /// <returns>File path</returns>
    public static async Task<string> GetPath(IClientContext clientContext,
        CancellationToken cancellationToken)
    {
        var selectedPath = await clientContext.GetSelectedPathAsync(cancellationToken);

        if (selectedPath != null)
        {
            return selectedPath.LocalPath;
        }

        var textView = await clientContext.GetActiveTextViewAsync(cancellationToken);

        return textView?.FilePath ?? string.Empty;
    }

    /// <summary>
    /// Try to find the current working directory, either by open document or by open solution
    /// </summary>
    /// <returns></returns>
    private async Task<string> GetWorkingDirectory(IClientContext? clientContext, CancellationToken cancellationToken)
    {
        var solutionDirectory = await GetSolutionDirectory(extensibility, cancellationToken);

        if (!string.IsNullOrEmpty(solutionDirectory))
        {
            return solutionDirectory;
        }

        if (clientContext == null)
        {
            return string.Empty;
        }

        var textView = await clientContext.GetActiveTextViewAsync(cancellationToken);

        if (!string.IsNullOrEmpty(textView?.FilePath))
        {
            return Path.GetDirectoryName(textView?.FilePath) ?? string.Empty;
        }

        return string.Empty;
    }

    private async Task<bool> ShowMissingSolutionDirMessage(CancellationToken cancellationToken)
        => await extensibility.Shell()
            .ShowPromptAsync("Unable to determine the solution directory location." +
                             "Please manually set the directory location in the settings.",
                             PromptOptions.OK, cancellationToken);

    private static string GetRegKeyValue()
    {
        var localMachineKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine,
            Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32);

        return localMachineKey
            .OpenSubKey(@"SOFTWARE\TortoiseSVN")
            ?.GetValue("ProcPath", DEFAULT_PROC_PATH)
            ?.ToString() ?? string.Empty;
    }
}
