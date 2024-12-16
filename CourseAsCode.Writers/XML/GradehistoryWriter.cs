using System.Xml;

namespace CourseAsCode.Writers.XML;

public class GradehistoryWriter : XmlWriterBase
{
	public void WriteGradehistory(string outputPath)
	{
		string filePath = Path.GetFullPath(Path.Combine(outputPath, "gradehistory.xml"));
		using (XmlWriter writer = XmlWriter.Create(filePath, _settings))
		{
			writer.WriteStartDocument();
			writer.WriteStartElement("grade_history");

			WriteGradeGrades(writer);

			writer.WriteEndElement(); // Close <grade_history>
			writer.WriteEndDocument();
		}
	}

	private void WriteGradeGrades(XmlWriter writer)
	{
		writer.WriteStartElement("grade_grades");
		writer.WriteString(" ");
		writer.WriteEndElement(); // Close <grade_grades>
	}
}