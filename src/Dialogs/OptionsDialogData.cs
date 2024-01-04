using Microsoft.VisualStudio.Extensibility.UI;
using Microsoft.Win32;
using System.Runtime.Serialization;
using TSVN.Models;
using TSVN.Resources;

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

        BrowseCommand = new AsyncCommand(Browse);
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

    [DataMember]
    public IAsyncCommand BrowseCommand
    {
        get;
    }

    // TODO: NOT POSSIBLE: How to show a OpenFolderDialog: https://github.com/microsoft/VSExtensibility/issues/291
    // TODO: NOT POSSIBLE: Which dialog to use SaveFileDialog or FolderBrowserDialog: https://stackoverflow.com/questions/76196026/using-folder-browser-in-wpf-net-6-0
    private async Task Browse(object? commandParameter, CancellationToken cancellationToken)
    {
        var dialog = new OpenFolderDialog()
        {
            Title = "Select Working Copy Root Path"
        };

        var success = dialog.ShowDialog();

        if (success != true)
        {
            return;
        }

        RootFolder = Path.GetDirectoryName(dialog.FolderName) ?? string.Empty;
    }

    #region Strings
#pragma warning disable CA1822 // Mark members as static
    [DataMember]
    public string OptionsGroupBoxHeader => TSVNResources.OptionsGroupBoxHeader;

    [DataMember]
    public string WorkingCopyRootFolderLabel => TSVNResources.WorkingCopyRootFolderLabel;

    [DataMember]
    public string OnItemAddedAddToSVNLabel => TSVNResources.OnItemAddedAddToSVNLabel;

    [DataMember]
    public string OnItemRemovedRemoveFromSVNLabel => TSVNResources.OnItemRemovedRemoveFromSVNLabel;

    [DataMember]
    public string OnItemRenamedRenameInSVNLabel => TSVNResources.OnItemRenamedRenameInSVNLabel;

    [DataMember]
    public string CloseOnEndLabel => TSVNResources.CloseOnEndLabel;

    [DataMember]
    public string BrowseButton => TSVNResources.BrowseButton;
#pragma warning restore CA1822 // Mark members as static
    #endregion
}
