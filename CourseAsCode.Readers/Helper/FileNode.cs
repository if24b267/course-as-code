namespace CourseAsCode.Readers.Helper;

public class FileNode
{
	public string Name { get; set; } // Name of the file or directory
	public bool IsDirectory { get; set; } // True if it's a directory, false if it's a file
	public int Depth { get; set; } // Depth in the directory tree
	public List<FileNode> Children { get; set; } // Child nodes (only populated for directories)

	public FileNode(string name, bool isDirectory, int depth)
	{
		Name = name;
		IsDirectory = isDirectory;
		Depth = depth;
		Children = new List<FileNode>();
	}
}