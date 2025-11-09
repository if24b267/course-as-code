using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Model.Enums;
using CourseAsCode.Model.Enums.Wikis;

namespace CourseAsCode.Writers.Tests.Fakes
{
    public static class FakeWikiFactory
    {
        /// <summary>
        /// Creates a basic Wiki with minimal configuration matching the backup example
        /// </summary>
        /// <returns>A Wiki object with default settings</returns>
        public static Wiki CreateBasicWiki()
        {
            var wiki = new Wiki
            {
                Name = "Test_Wiki",
                Description = "A test wiki for demonstration",
                Intro = "",
                IntroFormat = FormattingFormat.HTML,
                FirstPageTitle = "Test_First_Page_Wiki",
                WikiMode = WikiMode.Collaborative,
                DefaultFormat = WikiFormat.Html,
                ForceFormat = false,
                EditBegin = 0,
                EditEnd = 0,
                ActivityId = 1,
                ModuleId = 10,
                ModuleName = "wiki",
                Context = Model.Enums.Context.Module,
                GroupMode = GroupMode.SeparateGroups,
                VisibleToStudents = true,
                DisplayDescription = false,
                TimeCreated = DateTime.Now,
                TimeModified = DateTime.Now
            };

            // Create a default subwiki with first page
            var subwiki = new WikiSubwiki
            {
                SubwikiId = 1,
                GroupId = 0,
                UserId = 0,
                Pages = new List<WikiPage>
                {
                    new WikiPage
                    {
                        PageId = 1,
                        Title = "Test_First_Page_Wiki",
                        CachedContent = "<p>First Edit</p>",
                        TimeCreated = DateTime.Now,
                        TimeModified = DateTime.Now,
                        TimeRendered = DateTime.Now,
                        UserId = 2,
                        PageViews = 5,
                        ReadOnly = false,
                        Versions = new List<WikiPageVersion>
                        {
                            new WikiPageVersion
                            {
                                VersionId = 1,
                                Content = "",
                                ContentFormat = WikiFormat.Html,
                                Version = 0,
                                TimeCreated = DateTime.Now,
                                UserId = 2
                            },
                            new WikiPageVersion
                            {
                                VersionId = 2,
                                Content = "<p>First Edit</p>",
                                ContentFormat = WikiFormat.Html,
                                Version = 1,
                                TimeCreated = DateTime.Now.AddMinutes(1),
                                UserId = 2
                            }
                        }
                    }
                }
            };

            wiki.Subwikis.Add(subwiki);

            return wiki;
        }

        /// <summary>
        /// Creates a Wiki with minimal configuration for testing
        /// </summary>
        public static Wiki CreateMinimalWiki()
        {
            return new Wiki
            {
                Name = "Minimal Wiki",
                Description = "Minimal test wiki",
                FirstPageTitle = "Home",
                WikiMode = WikiMode.Collaborative,
                DefaultFormat = WikiFormat.Html,
                ActivityId = 1,
                ModuleId = 10,
                ModuleName = "wiki",
                Context = Model.Enums.Context.Module
            };
        }
    }
}
