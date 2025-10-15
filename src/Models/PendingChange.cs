using Microsoft.VisualStudio.Extensibility;

namespace TSVN.Models;

public class PendingChange
{
    public char ChangeType { get; set; }

    /// <summary>
    /// Relative file path of the changed file to the repository root.
    /// </summary>
    public string RelativeFilePath { get; set; } = string.Empty;

    public IEnumerable<string> FilePathParts { get; set; } = [];

    public ImageMoniker Moniker { get; set; }
}
