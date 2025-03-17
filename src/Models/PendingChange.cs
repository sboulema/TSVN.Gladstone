using Microsoft.VisualStudio.Extensibility;

namespace TSVN.Models;

public class PendingChange
{
    public char ChangeType { get; set; }

    public string FilePath { get; set; } = string.Empty;

    public IEnumerable<string> FilePathParts { get; set; } = [];

    public ImageMoniker Moniker { get; set; }
}
