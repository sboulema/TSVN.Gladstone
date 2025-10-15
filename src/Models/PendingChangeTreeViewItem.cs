using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.UI;
using System.Runtime.Serialization;
using TSVN.Helpers;

namespace TSVN.Models;

[DataContract]
public class PendingChangeTreeViewItem : NotifyPropertyChangedObject
{
    private string _label = string.Empty;
    private List<PendingChangeTreeViewItem> _children = [];
    private ImageMoniker _moniker;
    private char _changeType;

    private readonly VisualStudioExtensibility _extensibility;
    private readonly CommandHelper _commandHelper;

    public PendingChangeTreeViewItem(
        VisualStudioExtensibility extensibility, CommandHelper commandHelper)
    {
        _extensibility = extensibility;
        _commandHelper = commandHelper;

        OpenCommand = new AsyncCommand(OpenFile);
        UndoCommand = new AsyncCommand(Undo);
        HistoryCommand = new AsyncCommand(History);
        DiffCommand = new AsyncCommand(Diff);
        BlameCommand = new AsyncCommand(Blame);
    }

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

    [DataMember]
    public bool HasChildren
        => Children.Any();

    [DataMember]
    public IAsyncCommand OpenCommand { get; }

    [DataMember]
    public IAsyncCommand UndoCommand { get; }

    [DataMember]
    public IAsyncCommand HistoryCommand { get; }

    [DataMember]
    public IAsyncCommand DiffCommand { get; }

    [DataMember]
    public IAsyncCommand BlameCommand { get; }

    internal bool IsRoot { get; set; }

    public string FilePath { get; set; } = string.Empty;

    private async Task OpenFile(object? commandParameter, CancellationToken cancellationToken)
        => await _extensibility.Documents().OpenTextDocumentAsync(new(FilePath), cancellationToken);

    private async Task Undo(object? commandParameter, CancellationToken cancellationToken)
        => await _commandHelper.RunTortoiseSvnCommand(FilePath, "revert", cancellationToken: cancellationToken);

    private async Task History(object? commandParameter, CancellationToken cancellationToken)
        => await _commandHelper.RunTortoiseSvnCommand(FilePath, "log", cancellationToken: cancellationToken);

    private async Task Diff(object? commandParameter, CancellationToken cancellationToken)
        => await _commandHelper.RunTortoiseSvnCommand(FilePath, "diff", cancellationToken: cancellationToken);

    private async Task Blame(object? commandParameter, CancellationToken cancellationToken)
        => await _commandHelper.RunTortoiseSvnCommand(FilePath, "blame", cancellationToken: cancellationToken);
}
