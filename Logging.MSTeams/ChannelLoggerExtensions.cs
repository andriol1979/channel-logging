using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;

namespace channel_logging.Logging.MSTeams
{
    internal static class ChannelLoggerExtensions
    {
        /// <summary>
        /// depends on <see cref="loggerFactory"/> to send logs to specific Teams webhook channel.
        /// </summary>
        /// <param name="configure"></param>
        /// <param name="configure">Setting Teams webhook channel url and <see cref="LogLevel"/></param>
        /// <returns></returns>
        public static ILoggerFactory AddChannelLogger(this ILoggerFactory loggerFactory, Action<ChannelLoggerConfiguration> configure)
        {
            if (configure == null) throw new ArgumentNullException(nameof(configure));

            var config = new ChannelLoggerConfiguration();
            configure(config);

            loggerFactory.AddProvider(new ChannelLoggerProvider(config));

            return loggerFactory;
        }

        /// <summary>
        /// Send all king of <see cref="LogLevel"/> logs to single Teams webhook channel.
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="configure">Setting Teams webhook channel url. No needs to set <see cref="LogLevel"/>.</param>
        /// <returns></returns>
        public static ILoggerFactory AddSingleChannelLogger(this ILoggerFactory loggerFactory, Action<ChannelLoggerConfiguration> configure)
        {
            if (configure == null) throw new ArgumentNullException(nameof(configure));

            var config = new ChannelLoggerConfiguration();
            configure(config);

            GetEnumValues<LogLevel>().ToList().ForEach(level =>
            {
                var levelConfig = new ChannelLoggerConfiguration()
                {
                    LogLevel = level,
                    Channel = config.Channel
                };
                loggerFactory.AddProvider(new ChannelLoggerProvider(levelConfig));
            });

            return loggerFactory;

            IEnumerable<T> GetEnumValues<T>() => Enum.GetValues(typeof(T)).Cast<T>();
        }
        
        public static ILoggingBuilder AddColorConsoleLogger(
            this ILoggingBuilder builder)
        {
            builder.AddConfiguration();

            builder.Services.TryAddEnumerable(
                ServiceDescriptor.Singleton<ILoggerProvider, ChannelLoggerProvider>());

            LoggerProviderOptions.RegisterProviderOptions
                <ChannelLoggerConfiguration, ChannelLoggerProvider>(builder.Services);

            return builder;
        }

        public static ILoggingBuilder AddColorConsoleLogger(
            this ILoggingBuilder builder,
            Action<ChannelLoggerConfiguration> configure)
        {
            builder.AddColorConsoleLogger();
            builder.Services.Configure(configure);

            return builder;
        }
    }
}
