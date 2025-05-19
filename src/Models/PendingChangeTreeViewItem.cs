using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.UI;
using System.Runtime.Serialization;

namespace TSVN.Models;

[DataContract]
public class PendingChangeTreeViewItem : NotifyPropertyChangedObject
{
    private string _label = string.Empty;
    private List<PendingChangeTreeViewItem> _children = [];
    private ImageMoniker _moniker;
    private char _changeType;

    [DataMember]
    public string Label
    {
        get => _label;
        set => SetProperty(ref _label, value);
    }

    [DataMember]
    public char ChangeType
    {
        get => _changeType;
        set => SetProperty(ref _changeType, value);
    }

    [DataMember]
    public List<PendingChangeTreeViewItem> Children
    {
        get => _children;
        set => SetProperty(ref _children, value);
    }

    [DataMember]
    public ImageMoniker Moniker
    {
        get => _moniker;
        set => SetProperty(ref _moniker, value);
    }

    public bool IsRoot { get; set; }
}
