namespace CourseAsCode.Readers.Helper;

public class DirectoryReader
{
	private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

	// Keep static method for backward compatibility during transition
	public static FileNode ReadDirectory(string path, int depth = 0)
	{
		_logger.Debug($"Reading directory: {path}, Depth: {depth}");
		if (!Directory.Exists(path))
		{
			throw new DirectoryNotFoundException($"The directory '{path}' does not exist.");
		}

		var directoryInfo = new DirectoryInfo(path);
		var rootNode = new FileNode(directoryInfo.Name, true, depth);

		foreach (var directory in directoryInfo.GetDirectories())
		{
			_logger.Debug($"Found directory: {directory.Name} - reading it");
			// Recursively read subdirectories
			rootNode.Children.Add(ReadDirectory(directory.FullName, depth + 1));
		}

		_logger.Debug($"Found {directoryInfo.GetFiles().Length} files in {directoryInfo.Name} - reading");
		foreach (var file in directoryInfo.GetFiles())
		{
			// Add files directly
			rootNode.Children.Add(new FileNode(file.Name, false, depth + 1));
		}

		return rootNode;
	}

	// Instance method for DI
	public FileNode Read(string path, int depth = 0)
	{
		return ReadDirectory(path, depth);
	}
}