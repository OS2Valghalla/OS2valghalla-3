using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Valghalla.Integration.Saml
{
    /// <summary>
    /// Background service that periodically cleans up old SAML2 log files.
    /// Runs daily at a configured time to maintain disk space.
    /// </summary>
    public class Saml2LogCleanupService : BackgroundService
    {
        private readonly ILogger<Saml2LogCleanupService> logger;
        private readonly string logDirectory;
        private readonly int retentionDays;
        private readonly TimeSpan cleanupInterval;
        private readonly TimeSpan initialDelay;

        /// <summary>
        /// Initializes a new instance of the Saml2LogCleanupService.
        /// </summary>
        /// <param name="logger">Logger for cleanup operations</param>
        /// <param name="logDirectory">Directory containing SAML2 logs</param>
        /// <param name="retentionDays">Number of days to retain log files (default: 7)</param>
        /// <param name="cleanupTimeUtc">Time of day to run cleanup in UTC (default: 02:00)</param>
        public Saml2LogCleanupService(
            ILogger<Saml2LogCleanupService> logger,
            string? logDirectory = null,
            int retentionDays = 7,
            string cleanupTimeUtc = "02:00")
        {
            this.logger = logger;
            this.logDirectory = logDirectory ?? Saml2LoggerConfiguration.GetDefaultLogDirectory();
            this.retentionDays = retentionDays;
            this.cleanupInterval = TimeSpan.FromHours(24); // Run daily

            // Parse cleanup time (HH:mm format)
            if (!TimeOnly.TryParse(cleanupTimeUtc, out var cleanupTime))
            {
                cleanupTime = TimeOnly.Parse("02:00");
            }

            // Calculate delay until next cleanup time
            var now = DateTime.UtcNow;
            var nextCleanup = DateTime.UtcNow.Date.Add(cleanupTime.ToTimeSpan());
            
            if (nextCleanup <= now)
            {
                nextCleanup = nextCleanup.AddDays(1);
            }

            this.initialDelay = nextCleanup - now;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("[SAML2] Log cleanup service starting. Log directory: {LogDirectory}, Retention: {RetentionDays} days", 
                logDirectory, retentionDays);

            // Wait until the first scheduled cleanup time
            await Task.Delay(initialDelay, stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    logger.LogDebug("[SAML2] Running scheduled log cleanup");
                    PerformCleanup();
                    logger.LogInformation("[SAML2] Log cleanup completed successfully");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "[SAML2] Error during log cleanup: {ErrorMessage}", ex.Message);
                }

                // Wait for next cleanup cycle (daily)
                await Task.Delay(cleanupInterval, stoppingToken);
            }

            logger.LogInformation("[SAML2] Log cleanup service stopped");
        }

        private void PerformCleanup()
        {
            if (!Directory.Exists(logDirectory))
            {
                logger.LogDebug("[SAML2] Log directory does not exist: {LogDirectory}", logDirectory);
                return;
            }

            var cutoffDate = DateTime.UtcNow.AddDays(-retentionDays);
            var saml2LogFiles = Directory.GetFiles(logDirectory, "saml2-*.log");
            var deletedCount = 0;
            var failedCount = 0;

            foreach (var file in saml2LogFiles)
            {
                var fileInfo = new FileInfo(file);

                // Delete files older than retention period
                if (fileInfo.LastWriteTimeUtc < cutoffDate)
                {
                    try
                    {
                        File.Delete(file);
                        logger.LogDebug("[SAML2] Deleted old log file: {FileName}", Path.GetFileName(file));
                        deletedCount++;
                    }
                    catch (IOException ex)
                    {
                        // Log file might be in use, skip but log the error
                        logger.LogWarning(ex, "[SAML2] Could not delete log file {FileName}: {ErrorMessage}", 
                            Path.GetFileName(file), ex.Message);
                        failedCount++;
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "[SAML2] Unexpected error deleting log file {FileName}: {ErrorMessage}", 
                            Path.GetFileName(file), ex.Message);
                        failedCount++;
                    }
                }
            }

            if (deletedCount > 0 || failedCount > 0)
            {
                logger.LogInformation("[SAML2] Log cleanup results - Deleted: {DeletedCount}, Failed: {FailedCount}", 
                    deletedCount, failedCount);
            }
            else
            {
                logger.LogDebug("[SAML2] No old log files to delete");
            }
        }
    }
}
