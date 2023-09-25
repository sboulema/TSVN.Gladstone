using Microsoft.VisualStudio.ApplicationInsights;
using System.Diagnostics;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace TSVN.Helpers;

public static class LogHelper
{
    private static TelemetryClient? _client;
    private const string InstrumentationKey = "3baac5da-a1cb-461f-a82b-ff3d96ddef68";

    public static void GetClient()
    {
        _client = new TelemetryClient();
        _client.Context.Session.Id = Guid.NewGuid().ToString();
        _client.InstrumentationKey = InstrumentationKey;
        _client.Context.Component.Version = GetExecutingAssemblyVersion().ToString();
        _client.Context.User.Id = GetUserId();
    }

    public static void Log(string? message = null, Exception? exception = null)
    {
        if (_client == null)
        {
            GetClient();
        }

        if (_client == null)
        {
            return;
        }

        var properties = new Dictionary<string, string>
        {
            { "version", GetExecutingAssemblyVersion().ToString() },
            { "message", JsonSerializer.Serialize(message) }
        };

        if (exception == null)
        {
            _client.TrackEvent(message, properties);
        }
        else
        {
            _client.TrackException(exception, properties);
        }
    }

    private static Version GetExecutingAssemblyVersion()
    {
        var version = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);

        return new Version(
            version.ProductMajorPart,
            version.ProductMinorPart,
            version.ProductBuildPart,
            version.ProductPrivatePart);
    }

    private static string GetUserId()
    {
        var enc = Encoding.UTF8.GetBytes(Environment.UserName + Environment.MachineName);
        var hash = MD5.Create().ComputeHash(enc);
        return Convert.ToBase64String(hash);
    }
}
