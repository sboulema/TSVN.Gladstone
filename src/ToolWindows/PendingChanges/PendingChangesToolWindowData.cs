using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.UI;
using System.Runtime.Serialization;
using TSVN.Helpers;
using TSVN.Models;

namespace TSVN.ToolWindows.PendingChanges;

[DataContract]
internal class PendingChangesToolWindowData : NotifyPropertyChangedObject
{
    private IClientContext _clientContext;
    private PendingChangesHelper _pendingChangesHelper;
    private List<PendingChangeTreeViewItem> _items = []; 

    public PendingChangesToolWindowData(IClientContext clientContext, PendingChangesHelper pendingChangesHelper)
    {
        _clientContext = clientContext;
        _pendingChangesHelper = pendingChangesHelper;
    }

    [DataMember]
    public List<PendingChangeTreeViewItem> Items
    {
        get => _items;
        set => SetProperty(ref _items, value);
    }

    public async Task Refresh(CancellationToken cancellationToken)
    {
        var pendingChanges = await _pendingChangesHelper.GetPendingChanges(_clientContext, cancellationToken);
        Items = pendingChanges;
    }
}
