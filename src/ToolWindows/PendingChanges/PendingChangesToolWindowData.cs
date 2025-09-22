using Microsoft.VisualStudio.Extensibility.UI;
using System.Runtime.Serialization;
using TSVN.Models;

namespace TSVN.ToolWindows.PendingChanges;

[DataContract]
internal class PendingChangesToolWindowData : NotifyPropertyChangedObject
{
    private List<PendingChangeTreeViewItem> _items = [];
    private string _changesHeader = "Changes";

    [DataMember]
    public List<PendingChangeTreeViewItem> Items
    {
        get => _items;
        set => SetProperty(ref _items, value);
    }

    [DataMember]
    public string ChangesHeader
    {
        get => _changesHeader;
        set => SetProperty(ref _changesHeader, value);
    }
}
