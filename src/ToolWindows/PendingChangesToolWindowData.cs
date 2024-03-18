using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.UI;
using System.Runtime.Serialization;
using TSVN.Helpers;
using TSVN.Models;

namespace TSVN.ToolWindows;

[DataContract]
internal class PendingChangesToolWindowData : NotifyPropertyChangedObject
{
    private IClientContext _clientContext;
    private PendingChangesHelper _pendingChangesHelper;
    private List<PendingChangeTreeViewItem> _items = new(); 

    public PendingChangesToolWindowData(IClientContext clientContext, PendingChangesHelper pendingChangesHelper)
    {
        _clientContext = clientContext;
        _pendingChangesHelper = pendingChangesHelper;

        RefreshCommand = new AsyncCommand(Refresh);
    }

    [DataMember]
    public List<PendingChangeTreeViewItem> Items
    {
        get => _items;
        set => SetProperty(ref _items, value);
    }

    [DataMember]
    public IAsyncCommand RefreshCommand
    {
        get;
    }

    public async Task Refresh(object? commandParameter, CancellationToken cancellationToken)
    {
        var pendingChanges = await _pendingChangesHelper.GetPendingChanges(_clientContext, cancellationToken);
        Items = pendingChanges;
    }
}
