using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace channel_logging.Logging.MSTeams
{
    internal sealed class ChannelLoggerProvider : ILoggerProvider
    {
        private readonly ChannelLoggerConfiguration _config;
        private readonly ConcurrentDictionary<string, ChannelLogger> _loggers;

        public ChannelLoggerProvider(ChannelLoggerConfiguration config)
        {
            _config = config;
            _loggers = new ConcurrentDictionary<string, ChannelLogger>();
        }

        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, name => new ChannelLogger(name, _config));
        }

        public void Dispose()
        {
            _loggers.Clear();
        }
    }
}
