using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.UI;
using System.Runtime.Serialization;
using System.Windows.Media.Imaging;

namespace TSVN.Models;

[DataContract]
public class PendingChangeTreeViewItem : NotifyPropertyChangedObject
{
    private string _label = string.Empty;
    private List<PendingChangeTreeViewItem> _children = [];
    private BitmapSource? _icon;
    private ImageMoniker _moniker;

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

    [DataMember]
    public BitmapSource? Icon
    {
        get => _icon;
        set => SetProperty(ref _icon, value);
    }

    [DataMember]
    public ImageMoniker Moniker
    {
        get => _moniker;
        set => SetProperty(ref _moniker, value);
    }
}
