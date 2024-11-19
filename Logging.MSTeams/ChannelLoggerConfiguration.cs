
using Microsoft.Extensions.Logging;

namespace channel_logging.Logging.MSTeams
{
    internal class ChannelLoggerConfiguration
    {
        public LogLevel LogLevel { get; set; } = LogLevel.Warning;
        public int EventId { get; set; } = 0;
        public string Channel { get; set; }
    }
}
