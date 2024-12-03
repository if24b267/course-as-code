package reader;

import model.Quiz;
import org.w3c.dom.*;
import javax.xml.parsers.DocumentBuilderFactory;

public class XMLQuizReader extends QuizReader
{

    @Override
    public Quiz Read(String fileContent)
{
    Quiz quiz = new Quiz();

    try
    {
        Document doc = DocumentBuilderFactory.newInstance().newDocumentBuilder().parse(fileContent);
        NodeList nodes = doc.getElementsByTagName("quiz");

        if (nodes.getLength() > 0)
        {
            Element element = (Element)nodes.item(0);
            quiz.setTitle(element.getElementsByTagName("title").item(0).getTextContent());
        }
    }
    catch (Exception e)
    {
        e.printStackTrace();
    }

    return quiz;
}
}
