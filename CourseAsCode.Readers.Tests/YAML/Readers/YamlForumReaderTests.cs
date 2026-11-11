using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseAsCode.Readers.Helper;
using CourseAsCode.Readers.Interfaces;
using CourseAsCode.Readers.YAML.Model;
using CourseAsCode.Readers.YAML.Readers;

namespace CourseAsCode.Readers.Tests.YAML.Readers
{
    [TestFixture]
    public class YamlForumReaderTests : TestBase
    {
        private YamlForumReader _reader;
        private IYamlFileReaderHelper _mockYamlFileReader;
    }
}
