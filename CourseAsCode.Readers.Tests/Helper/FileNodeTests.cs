using CourseAsCode.Readers.Helper;

namespace CourseAsCode.Readers.Tests.Helper
{
	[TestFixture]
	public class FileNodeTests
	{
		[Test]
		public void Constructor_InitializesProperties()
		{
			// Arrange
			string name = "testNode";
			bool isDirectory = true;
			int depth = 2;

			// Act
			var fileNode = new FileNode(name, isDirectory, depth);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(fileNode.Name, Is.EqualTo(name));
				Assert.That(fileNode.IsDirectory, Is.EqualTo(isDirectory));
				Assert.That(fileNode.Depth, Is.EqualTo(depth));
				Assert.That(fileNode.Children, Is.Not.Null);
				Assert.That(fileNode.Children, Is.Empty);
			});
		}

		[Test]
		public void Children_CanAddChildNodes()
		{
			// Arrange
			var parentNode = new FileNode("parent", true, 0);
			var childNode1 = new FileNode("child1", false, 1);
			var childNode2 = new FileNode("child2", true, 1);

			// Act
			parentNode.Children.Add(childNode1);
			parentNode.Children.Add(childNode2);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(parentNode.Children, Has.Count.EqualTo(2));
				Assert.That(parentNode.Children[0], Is.EqualTo(childNode1));
				Assert.That(parentNode.Children[1], Is.EqualTo(childNode2));
			});
		}

		[Test]
		public void FileNode_SupportsNestedHierarchy()
		{
			// Arrange
			var rootNode = new FileNode("root", true, 0);
			var level1Node = new FileNode("level1", true, 1);
			var level2Node = new FileNode("level2", true, 2);
			var fileNode = new FileNode("file.txt", false, 3);

			// Act
			level2Node.Children.Add(fileNode);
			level1Node.Children.Add(level2Node);
			rootNode.Children.Add(level1Node);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(rootNode.Children, Has.Count.EqualTo(1));
				Assert.That(rootNode.Children[0].Name, Is.EqualTo("level1"));
				Assert.That(rootNode.Children[0].Children, Has.Count.EqualTo(1));
				Assert.That(rootNode.Children[0].Children[0].Name, Is.EqualTo("level2"));
				Assert.That(rootNode.Children[0].Children[0].Children, Has.Count.EqualTo(1));
				Assert.That(rootNode.Children[0].Children[0].Children[0].Name, Is.EqualTo("file.txt"));
				Assert.That(rootNode.Children[0].Children[0].Children[0].IsDirectory, Is.False);
			});
		}
	}
}