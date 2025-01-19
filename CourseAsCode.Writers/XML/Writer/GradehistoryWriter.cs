using System.Xml;

namespace CourseAsCode.Writers.XML.Writer;

public class GradehistoryWriter : XmlWriterBase
{
	public void WriteGradehistory(Model.Course course, string outputPath)
	{
		string filePath = Path.GetFullPath(Path.Combine(outputPath, "gradehistory.xml"));
		using XmlWriter writer = XmlWriter.Create(filePath, Settings);
		writer.WriteStartDocument();
		writer.WriteStartElement("grade_history");

		WriteGradeGradesSection(writer, course);

		writer.WriteEndElement(); // Close <grade_history>
		writer.WriteEndDocument();
	}


	/// <summary>
	/// writes the `grade_grades` section of the gradehistory.xml file
	/// </summary>
	/// <param name="writer">XmlWriter initialized on the correct document</param>
	/// <param name="course">initialized Moodle Course</param>
	private void WriteGradeGradesSection(XmlWriter writer, Model.Course course)
	{
		writer.WriteStartElement("grade_grades");
		writer.WriteString(" ");
		writer.WriteEndElement(); // Close <grade_grades>
	}
}