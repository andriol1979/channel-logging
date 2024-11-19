using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace channel_logging.Logging.MSTeams;

public class LoggerFactoryBuilder
{
    private readonly ILoggerFactory _loggerFactory;
    private readonly ChannelLoggingSetting _channelLoggingSetting;
    
    public LoggerFactoryBuilder(IOptions<ChannelLoggingSetting> channelLoggingConfig,
        LogLevel minimumLogLevel = LogLevel.Information)
    {
        _channelLoggingSetting = channelLoggingConfig.Value;
        _loggerFactory = LoggerFactory.Create(loggingBuilder => loggingBuilder
            .SetMinimumLevel(minimumLogLevel)
            .AddConsole());
    }

    public ILoggerFactory Build()
    {
        if (_channelLoggingSetting.EnableLogging)
        {
            _loggerFactory.AddChannelLogger(config =>
            {
                config.LogLevel = GetLogLevel();
                config.Channel = _channelLoggingSetting.WebhookUrl;
            });
        }

        return _loggerFactory;
    }
    
    private LogLevel GetLogLevel()
    {
        switch (_channelLoggingSetting.LogLevel)
        {
            case "Information":
            {
                return LogLevel.Information;
            }
            case "Error":
            {
                return LogLevel.Error;
            }
            case "Warning":
            {
                return LogLevel.Warning;
            }
        }

        return LogLevel.Trace;
    }
}