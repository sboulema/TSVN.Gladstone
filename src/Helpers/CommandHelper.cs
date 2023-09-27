using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.Shell;
using System.Diagnostics;

namespace TSVN.Helpers;

public class CommandHelper
{
    private readonly VisualStudioExtensibility _extensibility;
    private readonly FileHelper _fileHelper;
    private readonly string _tortoiseProcPath;

    public CommandHelper(VisualStudioExtensibility extensibility,
        FileHelper fileHelper)
    {
        _extensibility = extensibility;
        _fileHelper = fileHelper;
        _tortoiseProcPath = FileHelper.GetTortoiseSvnProc();
    }

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
            path = await _fileHelper.GetRepositoryRoot(clientContext, cancellationToken: cancellationToken);
        }

        if (string.IsNullOrEmpty(path))
        {
            return;
        }

        var options = await OptionsHelper.GetOptions(_extensibility, cancellationToken);
        var closeOnEnd = options.CloseOnEnd ? 1 : 0;

        // TODO: TEST Not yet implemented a way to handle Options
        // https://github.com/microsoft/VSExtensibility/issues/262

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
            await _extensibility.Shell()
                .ShowPromptAsync("TortoiseSVN not found. Did you install TortoiseSVN?",
                     PromptOptions.OK, cancellationToken);
        }
    }
}
