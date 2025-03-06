namespace TrackerApp.Web.Logging
{
    public class Log<T>
    {
        private readonly ILogger<T> _logger;
        private readonly IWebHostEnvironment _environment;
        private readonly string _logFilePath;
        private readonly int _maxFileSize;

        public Log(ILogger<T> logger, IWebHostEnvironment environment)
        {
            _logger = logger;
            _environment = environment;

            _logFilePath = Path.Combine(_environment.ContentRootPath, "Logging", "log.txt");

            _maxFileSize = 1000000;
        }

        public Task InformationAsync(string message)
        {
            _logger.LogInformation(message);

            return LogToFileAsync(message, LogLevel.Information);
        }

        public Task WarningAsync(string message)
        {
            _logger.LogWarning(message);

            return LogToFileAsync(message, LogLevel.Warning);
        }

        public Task ErrorAsync(string message)
        {
            _logger.LogError(message);

            return LogToFileAsync(message, LogLevel.Error);
        }

        private async Task LogToFileAsync(string message, LogLevel logLevel)
        {
            await CheckFileSizeAsync();

            var logText = $"[{DateTime.Now}]\t[{logLevel}]\t{message}\r\n\r\n";

            using var streamWriter = new StreamWriter(_logFilePath, true);

            await streamWriter.WriteAsync(logText);
        }

        private async Task CheckFileSizeAsync()
        {
            var fileInfo = new FileInfo(_logFilePath);

            if (fileInfo.Length > _maxFileSize)
                await File.WriteAllTextAsync(_logFilePath, "");
        }

    }
}
