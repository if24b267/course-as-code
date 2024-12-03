package reader;

import model.Assignment;
import org.w3c.dom.*;
import javax.xml.parsers.DocumentBuilderFactory;

public class XMLAssignmentReader extends AssignmentReader
{

    @Override
    public Assignment Read(String fileContent)
{
    Assignment assignment = new Assignment();

    try
    {
        Document doc = DocumentBuilderFactory.newInstance().newDocumentBuilder().parse(fileContent);
        NodeList nodes = doc.getElementsByTagName("assignment");

        if (nodes.getLength() > 0)
        {
            Element element = (Element)nodes.item(0);
            assignment.setTitle(element.getElementsByTagName("title").item(0).getTextContent());
            assignment.setDescription(element.getElementsByTagName("description").item(0).getTextContent());
        }
    }
    catch (Exception e)
    {
        e.printStackTrace();
    }

    return assignment;
}
}
