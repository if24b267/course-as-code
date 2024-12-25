using CourseAsCode.Model.Enums;

namespace CourseAsCode.Writers.XML.Writer.MoodleBackupWriter;

public class MoodleBackupWriterHelper
{
    #region Singleton
    private static MoodleBackupWriterHelper? _instance;
    
    public static MoodleBackupWriterHelper Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new MoodleBackupWriterHelper();
            }

            return _instance;
        }
    }
    #endregion
    
    

}