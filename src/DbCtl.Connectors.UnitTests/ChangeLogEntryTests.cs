using NUnit.Framework;
using System;
using System.IO;
using System.Text;

namespace DbCtl.Connectors.UnitTests
{
    [TestFixture]
    public class When_constructing_a_change_log_entry
    {
        private const string _AppliedBy = "joesoap";
        private const string _Hash = "ed076287532e86365e841e92bfc50d8c";
        private DateTime _ChangeDateTime = new DateTime(2020, 12, 13, 07, 25, 05);

        [Test]
        public void It_should_throw_an_exception_when_filename_is_not_provided()
        {
            Assert.Throws<ArgumentNullException>(() => new ChangeLogEntry(null));
        }

        [Test]
        public void It_should_throw_an_exception_when_filename_or_applied_by_or_stream_parameters_are_not_provided()
        {
            using var stream = new MemoryStream();
            Assert.Throws<ArgumentNullException>(() => new ChangeLogEntry(null, _AppliedBy, _ChangeDateTime, stream));
            Assert.Throws<ArgumentNullException>(() => new ChangeLogEntry("F-1.0.2-Initialise_database.ddl", null, _ChangeDateTime, stream));
            Assert.Throws<ArgumentNullException>(() => new ChangeLogEntry("F-1.0.2-Initialise_database.ddl", null, _ChangeDateTime, null));
        }

        [Test]
        public void It_should_throw_an_exception_when_filename_does_not_match_expected_convention()
        {
            using var stream = new MemoryStream();
            var exception = Assert.Throws<Exception>(() => new ChangeLogEntry("invalid-filename", _AppliedBy, _ChangeDateTime, stream));
            Assert.AreEqual("Failed to parse invalid-filename.", exception.Message);
        }

        [Test]
        public void It_should_create_a_change_log_entry_from_the_filename()
        {
            const string filename = "F-1.0.2-Initialise_database.ddl";
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes("Hello World!"));

            var entry = new ChangeLogEntry(filename, _AppliedBy, _ChangeDateTime, stream);

            Assert.AreEqual(MigrationType.Forward, entry.MigrationType);
            Assert.AreEqual("1.0.2", entry.Version);
            Assert.AreEqual("Initialise database", entry.Description);
            Assert.AreEqual(_AppliedBy, entry.AppliedBy);
            Assert.AreEqual(_ChangeDateTime, entry.ChangeDateTime);
            Assert.AreEqual(_Hash, entry.Hash);
            Assert.AreEqual(filename, entry.Filename);
        }
    }
}