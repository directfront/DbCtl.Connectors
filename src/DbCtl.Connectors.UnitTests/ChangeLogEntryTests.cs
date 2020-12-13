using DbCtl.Interfaces;
using NUnit.Framework;
using System;

namespace DbCtl.Connectors.UnitTests
{
    [TestFixture]
    public class When_calling_parse_on_change_log_entry
    {
        private const string _User = "joesoap";
        private const string _Hash = "abc123";
        private DateTime _ChangeDateTime = new DateTime(2020, 12, 13, 07, 25, 05);

        [Test]
        public void It_should_throw_an_exception_when_filename_is_not_provided()
        {
            Assert.Throws<ArgumentNullException>(() => ChangeLogEntry.Parse(null));
        }

        [Test]
        public void It_should_throw_an_exception_when_filename_does_not_match_conventions()
        {
            var exception = Assert.Throws<Exception>(() => ChangeLogEntry.Parse("invalid", _User, _ChangeDateTime, _Hash));
            Assert.AreEqual("Failed to parse invalid to a ChangeLogEntry.", exception.Message);
        }

        [Test]
        public void It_should_create_a_change_log_entry_from_the_filename()
        {
            const string filename = "f-1.0.2-Initialise_database.ddl";
            
            var entry = ChangeLogEntry.Parse(filename, _User, _ChangeDateTime, _Hash);

            Assert.AreEqual("F", entry.MigrationType);
            Assert.AreEqual("1.0.2", entry.Version);
            Assert.AreEqual("Initialise database", entry.Description);
            Assert.AreEqual(_User, entry.User);
            Assert.AreEqual(_ChangeDateTime, entry.ChangeDateTime);
            Assert.AreEqual(_Hash, entry.Hash);
        }
    }
}