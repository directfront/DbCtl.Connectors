using NUnit.Framework;
using System;

namespace DbCtl.Connectors.UnitTests
{
    [TestFixture]
    public class When_calling_parse_on_filename_parser
    {
        [Test]
        public void It_should_throw_an_exception_when_the_filename_does_not_match_the_regex()
        {
            var filename = "invalid-filename";
            var exception = Assert.Throws<Exception>(() => FilenameParser.Parse(filename));
            Assert.AreEqual("Failed to parse invalid-filename.", exception.Message);
        }

        [Test]
        public void It_should_parse_the_filename_into_its_constituents()
        {
            var filename = "f-1.0.1-alpha.beta-my_change_description.ddl";
            var parts = FilenameParser.Parse(filename);

            Assert.AreEqual(MigrationType.Forward, parts.Type);
            Assert.AreEqual("1.0.1-alpha.beta", parts.Version);
            Assert.AreEqual("my change description", parts.Description);
        }
    }
}
