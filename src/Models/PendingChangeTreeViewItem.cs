using Microsoft.VisualStudio.Extensibility.UI;
using System.Runtime.Serialization;

namespace TSVN.Models;

[DataContract]
public class PendingChangeTreeViewItem : NotifyPropertyChangedObject
{
    private string _label = string.Empty;
    private List<PendingChangeTreeViewItem> _children = [];

    [DataMember]
    public string Label
    {
        get => _label;
        set => SetProperty(ref _label, value);
    }

    [DataMember]
    public List<PendingChangeTreeViewItem> Children
    {
        get => _children;
        set => SetProperty(ref _children, value);
    }
}
