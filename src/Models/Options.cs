namespace TSVN.Models;

public class Options
{
    public string RootFolder { get; set; } = string.Empty;

    public bool OnItemAddedAddToSVN { get; set; }

    public bool OnItemRenamedRenameInSVN { get; set; }

    public bool OnItemRemovedRemoveFromSVN { get; set; }

    public bool CloseOnEnd { get; set; }

    public bool HideUnversioned {  get; set; }
}
