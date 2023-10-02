using Microsoft.VisualStudio.Extensibility;
using Newtonsoft.Json;
using TSVN.Models;

namespace TSVN.Helpers;

public static class OptionsHelper
{
    public static async Task<Options> GetOptions(
        VisualStudioExtensibility extensibility,
        CancellationToken cancellationToken)
    {
        var solutionDirectory = await FileHelper.GetSolutionDirectory(extensibility, cancellationToken);

        if (!Directory.Exists(solutionDirectory))
        {
            return new();
        }

        var settingFilePath = Path.Combine(solutionDirectory, ".vs", "TSVN.json");

        if (!File.Exists(settingFilePath))
        {
            return new();
        }

        var json = await File.ReadAllTextAsync(settingFilePath, cancellationToken);
        return JsonConvert.DeserializeObject<Options>(json) ?? new();
    }

    public static async Task SaveOptions(Options options,
        VisualStudioExtensibility extensibility,
        CancellationToken cancellationToken)
    {
        var solutionDirectory = await FileHelper.GetSolutionDirectory(extensibility, cancellationToken);

        if (!Directory.Exists(solutionDirectory))
        {
            return;
        }

        var optionsFilePath = Path.Combine(solutionDirectory, ".vs", "TSVN.json");

        var json = JsonConvert.SerializeObject(options);

        await File.WriteAllTextAsync(optionsFilePath, json, cancellationToken);
    }
}
