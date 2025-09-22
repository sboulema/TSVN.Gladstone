using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.ProjectSystem.Query;
using TSVN.Helpers;
using TSVN.Observers;

namespace TSVN;

/// <summary>
/// Extension entry point for the VisualStudio.Extensibility extension.
/// </summary>
[VisualStudioContribution]
internal class TSVNExtension : Extension
{
    /// <inheritdoc/>
    public override ExtensionConfiguration ExtensionConfiguration => new()
    {
        LoadedWhen = ActivationConstraint.SolutionState(SolutionState.FullyLoaded),
        Metadata = new(
            id: "TSVN.64f925a0-498e-45e3-a544-30ecbd32620d",
            version: ExtensionAssemblyVersion,
            publisherName: "Samir Boulema",
            displayName: "TSVN",
            description: "Control TortoiseSVN from within Visual Studio")
        {
            MoreInfo = "https://github.com/sboulema/TSVN.Gladstone",
            ReleaseNotes = "https://github.com/sboulema/TSVN.Gladstone/releases",
            Tags = ["SVN", "Source control", "Tortoise", "Repository"],
            // TODO: BUG? License file is missing
            //License = "Resources/License.txt",
            Icon = "Resources/TortoiseSVN.png",
            PreviewImage = "Resources/TortoiseSVN.png",
        },
    };

    /// <inheritdoc />
    protected override void InitializeServices(IServiceCollection serviceCollection)
    {
        base.InitializeServices(serviceCollection);

        // You can configure dependency injection here by adding services to the serviceCollection.
        serviceCollection.AddSingleton<CommandHelper>();
        serviceCollection.AddSingleton<FileHelper>();
        serviceCollection.AddSingleton<PendingChangesHelper>();

        // TODO: Pending Changes tool window: Add context menu

        // TODO: Pending Changes tool window: TreeViewItem HorizontalAlignment="Stretch"

        // TODO: Pending Changes tool window: toggle button border

        // TODO: Test automatic file change operations
    }

#pragma warning disable VSEXTPREVIEW_PROJECTQUERY_TRACKING
    protected override async Task OnInitializedAsync(VisualStudioExtensibility extensibility, CancellationToken cancellationToken)
    {
        await base.OnInitializedAsync(extensibility, cancellationToken);

        var commandHelper = ServiceProvider.GetRequiredService<CommandHelper>();

        var projects = await extensibility.Workspaces().QueryProjectsAsync(project => project, cancellationToken);

        foreach (var project in projects)
        {
            await project.Files
                .With(f => f.Path)
                .TrackUpdatesAsync(new TrackerObserver(commandHelper, extensibility, cancellationToken), cancellationToken);
        }
    }
#pragma warning restore VSEXTPREVIEW_PROJECTQUERY_TRACKING
}
