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
            description: "Control TortoiseSVN from within Visual Studio"),
    };

    /// <inheritdoc />
    protected override void InitializeServices(IServiceCollection serviceCollection)
    {
        base.InitializeServices(serviceCollection);

        // You can configure dependency injection here by adding services to the serviceCollection.
        serviceCollection.AddSingleton<CommandHelper>();
        serviceCollection.AddSingleton<FileHelper>();

        // TODO: How to subscribe to events?

        // TODO: Add "Pending Changes" ToolWindow
    }
}
