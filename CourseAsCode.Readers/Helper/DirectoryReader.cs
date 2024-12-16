namespace CourseAsCode.Readers.Helper;

public class DirectoryReader
{
	public static FileNode ReadDirectory(string path, int depth = 0)
	{
		if (!Directory.Exists(path))
		{
			throw new DirectoryNotFoundException($"The directory '{path}' does not exist.");
		}

		var directoryInfo = new DirectoryInfo(path);
		var rootNode = new FileNode(directoryInfo.Name, true, depth);

		foreach (var directory in directoryInfo.GetDirectories())
		{
			// Recursively read subdirectories
			rootNode.Children.Add(ReadDirectory(directory.FullName, depth + 1));
		}

		foreach (var file in directoryInfo.GetFiles())
		{
			// Add files directly
			rootNode.Children.Add(new FileNode(file.Name, false, depth + 1));
		}

		return rootNode;
	}
}