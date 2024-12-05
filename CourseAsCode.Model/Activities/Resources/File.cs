using System.ComponentModel;
using CourseAsCode.Model.Activities;

namespace CourseAsCode.Model.Resources;

public enum License
{
	// TODO: Determine possible values
}

public enum FileArea
{
	OverviewFiles
	// TODO: Determine possible values
}

public class File : Activity
{
	public int Size { get; set; }
	// maybe change this to file extension?
	public string Type { get; set; }
	
	[Description("name of the file that is to be included in the backup")]
	public string FileName { get; set; }
	[Description("path to the file that is to be included in the backup")]
	public string FilePath { get; set; }
	[Description("file of the size, in bytes")]
	public int FileSize { get; set; }
	
	// Defaults
	public class SensibleDefaults
	{
		[Description("name of the user who uploaded the file")]
		public string Author { get; set; }
		[Description("the license under which the file is distributed")]
		public License License { get; set; }
	}

	public class SystemDefaults
	{
		[Description("SHA1 Hash of the file content")]
		public string ContentHash { get; set; }
		[Description("the date and time that the file was created")]
		public DateTime TimeCreated { get; set; }
		[Description("the date and time that the file was last modified")]
		public DateTime TimeModified { get; set; }
		[Description("Unknown Purpose (possibly WHERE the file is created: user's private files, course files, etc.)")]
		public FileArea FileArea { get; set; }
		[Description("the itemid is needed if the file area in question is not already uniquely identified by the contextid + component + filearea")]
		public int ItemId { get; set; }
	}
}