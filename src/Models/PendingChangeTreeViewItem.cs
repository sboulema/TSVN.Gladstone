using Microsoft.VisualStudio.Extensibility.UI;
using System.Runtime.Serialization;

namespace TSVN.Models;

public class PendingChangeTreeViewItem : NotifyPropertyChangedObject
{
    private string _label = string.Empty;
    private List<PendingChangeTreeViewItem> _items = new();

    [DataMember]
    public string Label
    {
        get => _label;
        set => SetProperty(ref _label, value);
    }

    [DataMember]
    public List<PendingChangeTreeViewItem> Children
    {
        get => _items;
        set => SetProperty(ref _items, value);
    }
}
