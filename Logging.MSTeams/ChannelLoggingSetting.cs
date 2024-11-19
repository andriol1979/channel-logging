namespace channel_logging.Logging.MSTeams;

public class ChannelLoggingSetting
{
    public bool EnableLogging { get; set; }
    public string WebhookUrl { get; set; }
    public string LogLevel { get; set; } = "Information";
}