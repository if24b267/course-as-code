using CourseAsCode.Model.Enums;
using CourseAsCode.Writers.XML.Helper.Interfaces;

namespace CourseAsCode.Writers.XML.Helper;

public class FileHelper : IFileHelper
{
	#region Singleton
	private static FileHelper? _instance;

	public static FileHelper Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new FileHelper();
			}

			return _instance;
		}
	}
	#endregion


	public string GetFileAreaFromEnum(FileArea fileArea)
	{
		return fileArea switch
		{
			FileArea.Overview => "overviewfiles",
			FileArea.CourseContent => "content",
			_ => "content"
		};
	}


	public string GetComponentFromEnum(Component component)
	{
		return component switch
		{
			Component.Course => "course",
			Component.Resource => "mod_resource",
			Component.Forum => "mod_forum",
			Component.Glossary => "mod_glossary",
			Component.HtmlBlock => "block_html",
			_ => "course"
		};
	}


	public string GetLicenseFromEnum(License license)
	{
		return license switch
		{
			License.None => "unknown",
			License.AllRightsReserved => "allrightsreserved",
			License.PublicDomain => "public",
			License.CreativeCommons => "cc-4.0",
			License.CreativeCommonsNonCommercial => "cc-nc-4.0",
			License.CreativeCommonsNoDerivatives => "cc-nd-4.0",
			License.CreativeCommonsNonCommercialNoDerivatives => "cc-nc-nd-4.0",
			License.CreativeCommonsNonCommercialShareAlike => "cc-nc-sa-4.0",
			License.CreativeCommonsShareAlike => "cc-sa-4.0",
			_ => "unknown"
		};
	}
}