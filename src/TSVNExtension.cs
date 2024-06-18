using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.Extensibility;
using TSVN.Helpers;

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
            License = "Resources/LICENSE",
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

        // TODO: NOT POSSIBLE: How to subscribe to events: https://github.com/microsoft/VSExtensibility/issues/286
        // - VS.Events.ProjectItemsEvents.AfterAddProjectItems
        // - VS.Events.ProjectItemsEvents.AfterRenameProjectItems
        // - VS.Events.ProjectItemsEvents.AfterRemoveProjectItems

        // TODO: Add "Pending Changes" ToolWindow
    }
}
