using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.Shell;
using System.Diagnostics;

namespace TSVN.Helpers;

public class CommandHelper(
    VisualStudioExtensibility extensibility,
    FileHelper fileHelper)
{
    private readonly string _tortoiseProcPath = FileHelper.GetTortoiseSvnProc();

    public async Task RunTortoiseSvnCommand(IClientContext clientContext,
        string command, string args = "",
        bool isFileCommand = false,
        CancellationToken cancellationToken = default)
    {
        string? path;

        if (isFileCommand)
        {
            path = await FileHelper.GetPath(clientContext, cancellationToken);
        }
        else
        {
            path = await fileHelper.GetRepositoryRoot(clientContext, cancellationToken: cancellationToken);
        }

        await RunTortoiseSvnCommand(path, command, args, cancellationToken);
    }

    public async Task RunTortoiseSvnCommand(string path,
        string command, string args = "",
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(path))
        {
            return;
        }

        var options = await OptionsHelper.GetOptions(extensibility, cancellationToken);
        var closeOnEnd = options.CloseOnEnd ? 1 : 0;

        await StartProcess(
            _tortoiseProcPath,
            $"/command:{command} /path:\"{path}\" {args} /closeonend:{closeOnEnd}",
            cancellationToken);
    }

    public async Task StartProcess(string application, string args, CancellationToken cancellationToken)
    {
        try
        {
            Process.Start(application, args);
        }
        catch (Exception)
        {
            await extensibility.Shell()
                .ShowPromptAsync("TortoiseSVN not found. Did you install TortoiseSVN?",
                     PromptOptions.OK, cancellationToken);
        }
    }
}
