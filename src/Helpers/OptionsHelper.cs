using Newtonsoft.Json;
using TSVN.Models;

namespace TSVN.Helpers;

public class OptionsHelper
{
    private readonly FileHelper _fileHelper;

    public OptionsHelper(FileHelper fileHelper)
    {
        _fileHelper = fileHelper;
    }

    public async Task<Options> GetOptions(CancellationToken cancellationToken)
    {
        var solutionDirectory = await _fileHelper.GetSolutionDirectory(cancellationToken);

        if (!File.Exists(solutionDirectory))
        {
            return new();
        }

        var settingFilePath = Path.Combine(solutionDirectory, ".vs", "%TSVN.Options.ApplicationName%.json");

        if (!File.Exists(settingFilePath))
        {
            return new();
        }

        var json = await File.ReadAllTextAsync(settingFilePath, cancellationToken);
        return JsonConvert.DeserializeObject<Options>(json) ?? new();
    }

    public async Task SaveOptions(Options options, CancellationToken cancellationToken)
    {
        var solutionDirectory = await _fileHelper.GetSolutionDirectory(cancellationToken);

        if (!File.Exists(solutionDirectory))
        {
            return;
        }

        var optionsFilePath = Path.Combine(solutionDirectory, ".vs", "%TSVN.Options.ApplicationName%.json");

        var json = JsonConvert.SerializeObject(options);

        await File.WriteAllTextAsync(optionsFilePath, json, cancellationToken);
    }
}
