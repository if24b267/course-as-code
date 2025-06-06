using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.Logic
{
    public class FileService : IFileService
    {
        private readonly ILogger<FileService> _logger;

        /// <summary>
        /// Creates a new instance of the FileService class
        /// </summary>
        /// <param name="logger">Logger instance specific to FileService</param>
        /// <exception cref="ArgumentNullException">Thrown if any dependency is null</exception>
        public FileService(ILogger<FileService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        /// <summary>
        /// Checks if a file exists at the specified path.
        /// </summary>
        /// <param name="path">The path to the file.</param>
        /// <returns><c>true</c> if the file exists; otherwise, <c>false</c>.</returns>
        public bool Exists(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                _logger.LogWarning("Path cannot be null or whitespace");
                return false;
            }

            try
            {
                return File.Exists(path);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unknown error while checking if file exists at path '{path}'");
                return false;
            }
        }


        /// <summary>
        /// Opens an existing file for reading.
        /// </summary>
        /// <param name="path">The path to the file to be opened.</param>
        /// <returns>A <see cref="FileStream"/> for the specified file, or <c>null</c> if an error occurs.</returns>
        public FileStream OpenRead(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                _logger.LogWarning("Path cannot be null or whitespace");
                return null;
            }

            try
            {
                return File.OpenRead(path);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Couldn't open file at path '{path}'");
                return null;
            }
        }


        /// <summary>
        /// Copies an existing file to a new file.
        /// </summary>
        /// <param name="sourcePath">The path of the file to copy.</param>
        /// <param name="destinationPath">The path of the destination file.</param>
        /// <param name="overwrite"><c>true</c> to allow an existing file to be overwritten; otherwise, <c>false</c>.</param>
        /// <returns><c>true</c> if the copy operation was successful; otherwise, <c>false</c>.</returns>
        public bool Copy(string sourcePath, string destinationPath, bool overwrite)
        {
            if (string.IsNullOrWhiteSpace(sourcePath))
            {
                _logger.LogWarning("Source path cannot be null or whitespace");
                return false;
            }
            if (string.IsNullOrWhiteSpace(destinationPath))
            {
                _logger.LogWarning("Destination path cannot be null or whitespace");
                return false;
            }

            try
            {
                File.Copy(sourcePath, destinationPath, overwrite);
                _logger.LogInformation("Successfully copied file from '{SourcePath}' to '{DestinationPath}'. Overwrite: {Overwrite}", sourcePath, destinationPath, overwrite);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Couldn't copy file '{sourcePath}' to '{destinationPath}'");
                return false;
            }
        }


        /// <summary>
        /// <para>Creates a directory at the specified path.</para>
        /// <para>If the directory already exists, this method does nothing (and returns true).</para>
        /// </summary>
        /// <param name="path">The path of the directory to create.</param>
        /// <returns><c>true</c> if the directory was successfully created or already existed; otherwise, <c>false</c>.</returns>
        public bool CreateDirectory(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                _logger.LogWarning("Path cannot be null or whitespace");
                return false;
            }

            try
            {
                Directory.CreateDirectory(path);
                _logger.LogInformation($"Successfully ensured directory exists at path '{path}'");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Couldn't create directory at path '{path}'");
                return false;
            }
        }
    }
}
