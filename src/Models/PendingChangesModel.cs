namespace TSVN.Models;

public class PendingChangesModel
{
    public List<PendingChangeTreeViewItem> PendingChanges { get; set; } = [];

    public int NumberOfPendingChanges { get; set; }
}
