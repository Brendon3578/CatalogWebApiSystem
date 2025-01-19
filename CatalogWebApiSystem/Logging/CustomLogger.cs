
namespace CatalogWebApiSystem.Logging
{
    public class CustomLogger : ILogger
    {
        private readonly string _loggerName;
        private readonly CustomLoggerProviderConfiguration _loggerConfig;
        private readonly string _logFilePath;

        public CustomLogger(string loggerName, CustomLoggerProviderConfiguration loggerConfig)
        {
            _loggerName = loggerName;
            _loggerConfig = loggerConfig;

            string currentDir = AppDomain.CurrentDomain.BaseDirectory;
            string fileName = "log.txt";
            string relativePath = Path.Combine(currentDir, $@"..\..\..\Log\{fileName}");
            string fullPath = Path.GetFullPath(relativePath);


            if (!Directory.Exists(Path.GetDirectoryName(fullPath)))
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);

            if (!File.Exists(fullPath))
                File.Create(fullPath).Dispose();

            _logFilePath = fullPath;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel == _loggerConfig.LogLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            var message = $"{logLevel}: {eventId.Id} - {formatter(state, exception)}";

            WriteTextInFile(message);
        }

        private void GetLogPath()
        {

        }

        private void WriteTextInFile(string message)
        {

            using StreamWriter sw = new(_logFilePath, true);

            try
            {
                sw.WriteLine(message);
                sw.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
