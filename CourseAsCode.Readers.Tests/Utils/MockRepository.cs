using CourseAsCode.Model;
using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Readers.Interfaces;
using CourseAsCode.Readers.YAML.Model;
using CourseAsCode.Readers.YAML.Readers;

namespace CourseAsCode.Readers.Tests.Utils
{
	public static class MockRepository
	{
		public static YamlSectionReader CreateMockSectionReader()
		{
			var mockActivityReader = Substitute.For<IActivityReader>();

			// Configure the mock to return different activities based on the input path
			mockActivityReader.Read(Arg.Is<string>(s => s.Contains("url")))
				 .Returns(callInfo =>
				 {
					 var path = callInfo.Arg<string>();
					 return new URL
					 {
						 ActivityId = 1,
						 Name = "Mock URL",
						 Description = "Mock URL Description",
						 ExternalUrl = "https://example.com"
					 };
				 });

			mockActivityReader.Read(Arg.Is<string>(s => s.Contains("file")))
				 .Returns(callInfo =>
				 {
					 var path = callInfo.Arg<string>();
					 return new MoodleFile
					 {
						 ActivityId = 2,
						 Name = "Mock File",
						 Description = "Mock File Description"
					 };
				 });

			// Create a real section reader with the mock activity reader
			// Create a mock YamlFileReaderHelper
				var mockYamlFileReader = Substitute.For<IYamlFileReaderHelper>();
				return new YamlSectionReader(mockActivityReader, mockYamlFileReader);
		}

		public static IModelObjectReader<T> CreateMockModelObjectReader<T>() where T : class
		{
			var mockReader = Substitute.For<IModelObjectReader<T>>();
			if (typeof(T) == typeof(Section))
			{
				var section = new Section
				{
					Name = "Mock Section",
					Summary = "Mock Summary"
				};
				mockReader.Read(Arg.Any<string>()).Returns(section as T);
			}
			return mockReader;
		}

		public static YamlCourse CreateSampleYamlCourse()
		{
			return new YamlCourse
			{
				CourseName = "Test Course",
				ShortName = "TC",
				Summary = "Test course summary",
				Description = "Test course description",
				Visible = true,
				Category = "TestCategory",
				CategoryId = 1,
				CategoryDescription = "Test category description",
				StartDate = new DateTime(2024, 1, 1),
				EndDate = new DateTime(2024, 12, 31),
				IdNumber = 12345,
				ShowGrades = true,
				GroupMode = Model.Enums.GroupMode.NoGroups,
				ForceGroupMode = false,
				EnableCompletionTracking = true,
				NumberOfAnnouncements = 5,
				ForcedLanguage = "en",
				ShowActivityDates = true,
				ShowCompletionConditions = true
			};
		}
	}
}