using System.Collections.Generic;
using CourseAsCode.Readers.Helper;
using NUnit.Framework;
using YamlDotNet.Serialization;

namespace CourseAsCode.Readers.Tests.Helper
{
    [TestFixture]
    public class YamlMergerTests
    {
        private static readonly IDeserializer Deserializer =
            new DeserializerBuilder().Build();

        [Test]
        public void MergeYamlStrings_MergesDifferentProperties()
        {
            const string baseYaml = "name: Course\n";
            const string overrideYaml = "visible: true\n";

            var result = YamlMerger.MergeYamlStrings(baseYaml, overrideYaml);
            var data = Deserializer.Deserialize<Dictionary<string, object>>(result);

            Assert.That(data.ContainsKey("name"), Is.True);
            Assert.That(data.ContainsKey("visible"), Is.True);
            Assert.That(data["name"].ToString(), Is.EqualTo("Course"));
            Assert.That(data["visible"].ToString(), Is.EqualTo("true"));
        }

        [Test]
        public void MergeYamlStrings_OverridesExistingValue()
        {
            const string baseYaml = "name: Original\n";
            const string overrideYaml = "name: Updated\n";

            var result = YamlMerger.MergeYamlStrings(baseYaml, overrideYaml);
            var data = Deserializer.Deserialize<Dictionary<string, object>>(result);

            Assert.That(data["name"].ToString(), Is.EqualTo("Updated"));
        }

        [Test]
        public void MergeYamlStrings_EmptyBase_ReturnsOverride()
        {
            const string baseYaml = "";
            const string overrideYaml = "name: NewCourse\n";

            var result = YamlMerger.MergeYamlStrings(baseYaml, overrideYaml);

            Assert.That(result, Is.EqualTo(overrideYaml));
        }

        [Test]
        public void MergeYamlStrings_EmptyOverride_ReturnsBase()
        {
            const string baseYaml = "name: Original\n";
            const string overrideYaml = "";

            var result = YamlMerger.MergeYamlStrings(baseYaml, overrideYaml);

            Assert.That(result, Is.EqualTo(baseYaml));
        }

        [Test]
        public void MergeYamlStrings_DeepMergesNestedProperties()
        {
            const string baseYaml =
                "settings:\n  visible: true\n  language: en\n";

            const string overrideYaml =
                "settings:\n  language: de\n";

            var result = YamlMerger.MergeYamlStrings(baseYaml, overrideYaml);
            var data = Deserializer.Deserialize<Dictionary<string, Dictionary<string, object>>>(result);

            Assert.That(data["settings"]["visible"].ToString(), Is.EqualTo("true"));
            Assert.That(data["settings"]["language"].ToString(), Is.EqualTo("de"));
        }
    }
}
