using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CourseAsCode.Model;
using CourseAsCode.Model.Activities;
using CourseAsCode.Model.Enums;
using CourseAsCode.Writers.XML.Helper.Interfaces;

namespace CourseAsCode.Writers.XML.Helper;

public class FileHelper : IFileHelper
{
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

    public string GetFileAreaFromEnum(FileArea fileArea)
    {
        return fileArea switch
        {
            FileArea.Overview => "overviewfiles",
            FileArea.CourseContent => "content",
            _ => "content",
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
            _ => "course",
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
            _ => "unknown",
        };
    }

    /// <summary>
    /// Attempts to discover file ids referenced by an activity.
    /// Conservative reflection-based approach:
    /// - checks common int/nullable int properties named ItemId or Id
    /// - inspects common nested properties (Picture, File, Files, Attachments, Content)
    /// - enumerates enumerable properties and extracts ints or ItemId/Id from their items
    /// Returns distinct, positive ids.
    /// Replace with a deterministic lookup if you have a canonical file registry.
    /// </summary>
    public IEnumerable<int> GetFileIdsForActivity(Course course, Activity activity)
    {
        if (activity == null)
            return Array.Empty<int>();

        var ids = new List<int>();
        var type = activity.GetType();

        // direct ItemId or Id property on activity
        AddIntPropertyIfPresent(activity, type, "ItemId", ids);
        AddIntPropertyIfPresent(activity, type, "Id", ids);

        // check common named properties
        var candidateNames = new[] { "Picture", "File", "Files", "Attachments", "Content", "ContentFiles" };
        foreach (var name in candidateNames)
        {
            var prop = type.GetProperty(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (prop == null) continue;

            var val = prop.GetValue(activity);
            if (val == null) continue;

            if (val is System.Collections.IEnumerable enumerable && !(val is string))
            {
                foreach (var item in enumerable)
                {
                    if (item == null) continue;
                    if (item is int ii) ids.Add(ii);
                    else
                    {
                        AddIntPropertyIfPresent(item, item.GetType(), "ItemId", ids);
                        AddIntPropertyIfPresent(item, item.GetType(), "Id", ids);
                    }
                }
            }
            else
            {
                AddIntPropertyIfPresent(val, val.GetType(), "ItemId", ids);
                AddIntPropertyIfPresent(val, val.GetType(), "Id", ids);
            }
        }

        // scan all enumerable properties for potential file objects / ids
        foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (prop.PropertyType == typeof(string)) continue;

            if (typeof(System.Collections.IEnumerable).IsAssignableFrom(prop.PropertyType))
            {
                var val = prop.GetValue(activity) as System.Collections.IEnumerable;
                if (val == null) continue;

                foreach (var item in val)
                {
                    if (item == null) continue;
                    if (item is int ii) ids.Add(ii);
                    else
                    {
                        AddIntPropertyIfPresent(item, item.GetType(), "ItemId", ids);
                        AddIntPropertyIfPresent(item, item.GetType(), "Id", ids);
                    }
                }
            }
        }

        return ids.Where(i => i > 0).Distinct().ToList();
    }

    private static void AddIntPropertyIfPresent(object obj, Type type, string propertyName, List<int> ids)
    {
        var p = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
        if (p == null) return;

        if (p.PropertyType == typeof(int))
        {
            var v = (int)p.GetValue(obj);
            if (v > 0) ids.Add(v);
        }
        else if (p.PropertyType == typeof(int?))
        {
            var v = (int?)p.GetValue(obj);
            if (v.HasValue && v.Value > 0) ids.Add(v.Value);
        }
    }
}
