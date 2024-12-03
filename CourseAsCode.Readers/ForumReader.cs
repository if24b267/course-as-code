package reader;

import model.Forum;
import org.w3c.dom.*;
import javax.xml.parsers.DocumentBuilderFactory;

public class XMLForumReader extends ForumReader
{

    @Override
    public Forum Read(String fileContent)
{
    Forum forum = new Forum();

    try
    {
        Document doc = DocumentBuilderFactory.newInstance().newDocumentBuilder().parse(fileContent);
        NodeList nodes = doc.getElementsByTagName("forum");

        if (nodes.getLength() > 0)
        {
            Element element = (Element)nodes.item(0);
            forum.setTitle(element.getElementsByTagName("title").item(0).getTextContent());
            forum.setDescription(element.getElementsByTagName("description").item(0).getTextContent());
        }
    }
    catch (Exception e)
    {
        e.printStackTrace();
    }

    return forum;
}
}
