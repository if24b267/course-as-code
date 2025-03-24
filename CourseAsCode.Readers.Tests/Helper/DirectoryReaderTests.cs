using CourseAsCode.Readers.Helper;

namespace CourseAsCode.Readers.Tests.Helper
{
	[TestFixture]
	public class DirectoryReaderTests : TestBase
	{
		[Test]
		public void ReadDirectory_ValidEmptyDirectory_ReturnsRootNode()
		{
			// Arrange
			var directoryName = "EmptyDir";
			var emptyDir = CreateTestDirectory(directoryName);

			// Act
			var result = DirectoryReader.ReadDirectory(emptyDir);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result, Is.Not.Null);
				Assert.That(result.Name, Is.EqualTo(directoryName));
				Assert.That(result.IsDirectory, Is.True);
				Assert.That(result.Depth, Is.EqualTo(0));
				Assert.That(result.Children, Is.Empty);
			});
		}

		[Test]
		public void ReadDirectory_DirectoryWithFiles_ReturnsCorrectStructure()
		{
			// Arrange
			var directoryName = "TestDir";
			var testDir = CreateTestDirectory(directoryName);
			var filePath1 = Path.Combine(testDir, "file1.txt");
			var filePath2 = Path.Combine(testDir, "file2.txt");

			File.WriteAllText(filePath1, "Test content 1");
			File.WriteAllText(filePath2, "Test content 2");

			// Act
			var result = DirectoryReader.ReadDirectory(testDir);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result, Is.Not.Null);
				Assert.That(result.Name, Is.EqualTo(directoryName));
				Assert.That(result.IsDirectory, Is.True);
				Assert.That(result.Depth, Is.EqualTo(0));
				Assert.That(result.Children, Has.Count.EqualTo(2));

				var fileNames = result.Children.Select(c => c.Name).ToList();
				Assert.That(fileNames, Does.Contain("file1.txt"));
				Assert.That(fileNames, Does.Contain("file2.txt"));

				Assert.That(result.Children.All(c => !c.IsDirectory), Is.True);
				Assert.That(result.Children.All(c => c.Depth == 1), Is.True);
			});
		}

		[Test]
		public void ReadDirectory_DirectoryWithSubdirectories_ReturnsCorrectStructure()
		{
			// Arrange
			var rootDirName = "RootDir";
			var rootDir = CreateTestDirectory(rootDirName);

			var subDir1 = CreateTestDirectory(Path.Combine(rootDirName, "SubDir1"));
			var subDir2 = CreateTestDirectory(Path.Combine(rootDirName, "SubDir2"));

			var filePath1 = Path.Combine(rootDir, "file1.txt");
			var filePath2 = Path.Combine(subDir1, "file2.txt");

			File.WriteAllText(filePath1, "Test content 1");
			File.WriteAllText(filePath2, "Test content 2");

			// Act
			var result = DirectoryReader.ReadDirectory(rootDir);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result, Is.Not.Null);
				Assert.That(result.Name, Is.EqualTo(rootDirName));
				Assert.That(result.IsDirectory, Is.True);
				Assert.That(result.Depth, Is.EqualTo(0));
				Assert.That(result.Children, Has.Count.EqualTo(3)); // 2 dirs + 1 file

				var rootChildren = result.Children.ToList();
				var fileNodes = rootChildren.Where(c => !c.IsDirectory).ToList();
				var dirNodes = rootChildren.Where(c => c.IsDirectory).ToList();

				Assert.That(fileNodes, Has.Count.EqualTo(1));
				Assert.That(fileNodes[0].Name, Is.EqualTo("file1.txt"));

				Assert.That(dirNodes, Has.Count.EqualTo(2));
				var dirNames = dirNodes.Select(d => d.Name).ToList();
				Assert.That(dirNames, Does.Contain("SubDir1"));
				Assert.That(dirNames, Does.Contain("SubDir2"));

				// Check subdirectory content
				var subDir1Node = dirNodes.First(d => d.Name == "SubDir1");
				Assert.That(subDir1Node.Children, Has.Count.EqualTo(1));
				Assert.That(subDir1Node.Children[0].Name, Is.EqualTo("file2.txt"));
				Assert.That(subDir1Node.Children[0].IsDirectory, Is.False);
				Assert.That(subDir1Node.Children[0].Depth, Is.EqualTo(2));
			});
		}

		[Test]
		public void ReadDirectory_NonExistentDirectory_ThrowsDirectoryNotFoundException()
		{
			// Arrange
			var nonExistentDir = Path.Combine(TempDirectory, "NonExistentDir");

			// Act & Assert
			Assert.Throws<DirectoryNotFoundException>(() =>
				 DirectoryReader.ReadDirectory(nonExistentDir));
		}

		[Test]
		public void ReadDirectory_WithCustomDepth_SetsCorrectDepthValues()
		{
			// Arrange
			var directoryName = "TestDir";
			var testDir = CreateTestDirectory(directoryName);
			var filePath = Path.Combine(testDir, "file.txt");
			File.WriteAllText(filePath, "Test content");
			var initialDepth = 5;

			// Act
			var result = DirectoryReader.ReadDirectory(testDir, initialDepth);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result.Depth, Is.EqualTo(initialDepth));
				Assert.That(result.Children[0].Depth, Is.EqualTo(initialDepth + 1));
			});
		}
	}
}