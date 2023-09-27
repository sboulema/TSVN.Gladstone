using Microsoft.VisualStudio.Extensibility.UI;
using System.Runtime.Serialization;
using TSVN.Models;

namespace TSVN.Dialogs;

[DataContract]
public class OptionsDialogData : NotifyPropertyChangedObject
{
    private string _rootFolder = string.Empty;
    private bool _onItemAddedAddToSVN;
    private bool _onItemRenamedRenameInSVN;
    private bool _onItemRemovedRemoveFromSVN;
    private bool _closeOnEnd;

    public OptionsDialogData(Options options)
    {
        RootFolder = options.RootFolder;
        OnItemAddedAddToSVN = options.OnItemAddedAddToSVN;
        OnItemRenamedRenameInSVN = options.OnItemRenamedRenameInSVN;
        OnItemRemovedRemoveFromSVN = options.OnItemRemovedRemoveFromSVN;
        CloseOnEnd = options.CloseOnEnd;
    }

    [DataMember]
    public string RootFolder
    {
        get => _rootFolder;
        set => SetProperty(ref _rootFolder, value);
    }

    [DataMember]
    public bool OnItemAddedAddToSVN
    {
        get => _onItemAddedAddToSVN;
        set => SetProperty(ref _onItemAddedAddToSVN, value);
    }

    [DataMember]
    public bool OnItemRenamedRenameInSVN
    {
        get => _onItemRenamedRenameInSVN;
        set => SetProperty(ref _onItemRenamedRenameInSVN, value);
    }

    [DataMember]
    public bool OnItemRemovedRemoveFromSVN
    {
        get => _onItemRemovedRemoveFromSVN;
        set => SetProperty(ref _onItemRemovedRemoveFromSVN, value);
    }

    [DataMember]
    public bool CloseOnEnd
    {
        get => _closeOnEnd;
        set => SetProperty(ref _closeOnEnd, value);
    }
}
