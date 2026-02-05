using Serilog;
using Serilog.Core;
using Serilog.Events;
using System.Runtime.InteropServices;

namespace Valghalla.Integration.Saml
{
    /// <summary>
    /// SAML2 Logger Configuration for debugging SAML2 authentication flows.
    /// Supports Linux disk logging with automatic log rotation and cleanup.
    /// </summary>
    public static class Saml2LoggerConfiguration
    {
        /// <summary>
        /// Configures Serilog for SAML2 authentication debugging.
        /// </summary>
        /// <param name="logDirectory">Directory where logs will be stored (Linux-compatible path)</param>
        /// <param name="retentionDays">Number of days to retain log files (default: 7 days)</param>
        /// <returns>Configured LoggerConfiguration</returns>
        public static LoggerConfiguration ConfigureSaml2Logging(
            this LoggerConfiguration configuration,
            string logDirectory = "./logs/saml2",
            int retentionDays = 7)
        {
            // Resolve to absolute path if relative
            var absoluteLogDirectory = Path.IsPathRooted(logDirectory) 
                ? logDirectory 
                : Path.Combine(AppContext.BaseDirectory, logDirectory);

            // Create logs directory if it doesn't exist
            try
            {
                if (!Directory.Exists(absoluteLogDirectory))
                {
                    Directory.CreateDirectory(absoluteLogDirectory);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to create SAML2 log directory {absoluteLogDirectory}: {ex.Message}");
                // Fallback to temp directory if cannot create primary
                absoluteLogDirectory = Path.Combine(Path.GetTempPath(), "valghalla-saml2");
                Directory.CreateDirectory(absoluteLogDirectory);
            }

            var outputTemplate = "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] [{Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}";

            return configuration
                .MinimumLevel.Is(LogEventLevel.Debug)
                .Enrich.WithProperty("Component", "SAML2")
                .Enrich.FromLogContext()
                .WriteTo.File(
                    path: Path.Combine(absoluteLogDirectory, "saml2-.log"),
                    outputTemplate: outputTemplate,
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: retentionDays,
                    shared: true,
                    fileSizeLimitBytes: 104857600, // 100 MB
                    rollOnFileSizeLimit: true,
                    encoding: System.Text.Encoding.UTF8);
        }

        /// <summary>
        /// Cleans up old SAML2 log files based on retention policy.
        /// Should be called periodically (e.g., from a background service).
        /// </summary>
        /// <param name="logDirectory">Directory containing SAML2 logs</param>
        /// <param name="retentionDays">Number of days to retain log files</param>
        public static void CleanupOldLogs(
            string logDirectory = "./logs/saml2",
            int retentionDays = 7)
        {
            try
            {
                if (!Directory.Exists(logDirectory))
                    return;

                var cutoffDate = DateTime.UtcNow.AddDays(-retentionDays);
                var saml2LogFiles = Directory.GetFiles(logDirectory, "saml2-*.log");

                foreach (var file in saml2LogFiles)
                {
                    var fileInfo = new FileInfo(file);
                    
                    // Delete files older than retention period
                    if (fileInfo.LastWriteTimeUtc < cutoffDate)
                    {
                        try
                        {
                            File.Delete(file);
                        }
                        catch (IOException ex)
                        {
                            // Log file might be in use, skip silently
                            System.Diagnostics.Debug.WriteLine($"Could not delete log file {file}: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error cleaning up SAML2 logs: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets the default log directory for the current platform (Linux-compatible).
        /// For development environments, uses ./logs/saml2. For production, uses platform-specific paths.
        /// </summary>
        public static string GetDefaultLogDirectory()
        {
            // Check if we're in development (app directory contains "bin" or "Debug"/"Release")
            var isDevelopment = AppContext.BaseDirectory.Contains("bin", StringComparison.OrdinalIgnoreCase);
            
            if (isDevelopment)
            {
                // Development: use local logs directory (works on all platforms)
                return Path.Combine(AppContext.BaseDirectory.Split("bin")[0], "logs", "saml2");
            }

            // Production paths
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                // Linux: /var/log/valghalla/saml2
                return "/var/log/valghalla/saml2";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // Windows: AppData\Logs\saml2
                var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                return Path.Combine(appData, "Valghalla", "Logs", "saml2");
            }
            else
            {
                // Fallback: current app directory
                return Path.Combine(AppContext.BaseDirectory, "logs", "saml2");
            }
        }
    }
}
