using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

[assembly: InternalsVisibleTo("DbCtl.Connectors.UnitTests")]

namespace DbCtl.Interfaces
{
    /// <summary>
    /// A database change log entry that describes a change to a database.
    /// </summary>
    public class ChangeLogEntry : IEquatable<ChangeLogEntry>
    {
        /// <summary>
        /// Constructs a new change log entry assuming appliedBy as Environment.UserName and ChangeDateTime as DateTime.Now.
        /// </summary>
        /// <param name="filename">Name of the change file conforming to the following regular expression: ^(?<type>(F|B))-(?<version>\d+.\d+.\d+)-(?<description>[\w]+).(ddl|dml|dcl)$</param>
        public ChangeLogEntry(string filename)
        {
            Parse(filename, Environment.UserName, DateTime.Now, CalculateHash(filename));
        }

        /// <summary>
        /// Constructs a new change log entry that will calculate the hash using the provided contents, user and date/time of the change. This constructor will not verify the integrity of the contents stream against the provided filename.
        /// </summary>
        /// <param name="filename">Name of the change file conforming to the following regular expression: ^(?<type>(F|B))-(?<version>\d+.\d+.\d+)-(?<description>[\w]+).(ddl|dml|dcl)$</param>
        /// <param name="appliedBy">Name of the user that applied the change.</param>
        /// <param name="changeDateTime">Date and time when the change was applied to the database.</param>
        /// <param name="contents">A stream of the contents of the file provided in filename.</param>
        public ChangeLogEntry(string filename, string appliedBy, DateTime changeDateTime, Stream contents)
        {
            Parse(filename, appliedBy, changeDateTime, CalculateHash(contents));
        }

        /// <summary>
        /// Version number of this change.
        /// </summary>
        public string Version { get; private set; }
        /// <summary>
        /// Description of the changes made by this migration.
        /// </summary>
        public string Description { get; private set; }
        /// <summary>
        /// Name of the file that originated this change log entry. It is also used to determine the hash, version, description and 
        /// migration type. All underscores are replaced with spaces to generate the description. It must conform to the following 
        /// regular expression: ^(?<type>(F|B))-(?<version>\d+.\d+.\d+)-(?<description>[\w]+).(ddl|dml|dcl)$
        /// </summary>
        public string Filename { get; private set; }
        /// <summary>
        /// MD5 hash of the contents of the migration file.
        /// </summary>
        public string Hash { get; private set; }
        /// <summary>
        /// Date and time when this migration was applied to the database.
        /// </summary>
        public DateTime ChangeDateTime { get; private set; }
        /// <summary>
        /// Name of the user that applied this change.
        /// </summary>
        public string AppliedBy { get; private set; }
        /// <summary>
        /// Type of migration applied to the database, either 'F' or 'B'. That is, forward migration or backward migration, respectively.
        /// </summary>
        public string MigrationType { get; private set; }

        private void Parse(string filename, string appliedBy, DateTime changeDateTime, string hash)
        {
            if (string.IsNullOrEmpty(filename))
                throw new ArgumentNullException(nameof(filename));

            if (string.IsNullOrEmpty(appliedBy))
                throw new ArgumentNullException(nameof(appliedBy));

            var regex = new Regex(@"^(?<typ>(F|B|f|b))-(?<ver>\d+.\d+.\d+)-(?<desc>[\w]+).(ddl|dml|dcl)$");

            var result = regex.Match(filename);
            if (!result.Success)
                throw new Exception($"Failed to parse {filename} to a ChangeLogEntry.");

            Filename = filename;
            MigrationType = result.Groups["typ"].Value.ToUpperInvariant();
            Version = result.Groups["ver"].Value;
            Description = result.Groups["desc"].Value.Replace("_", " ");
            AppliedBy = appliedBy;
            ChangeDateTime = changeDateTime;
            Hash = hash;
        }

        private static string CalculateHash(string filename)
        {
            using var stream = File.OpenRead(filename);
            return CalculateHash(stream);
        }
                
        private static string CalculateHash(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            using var md5 = MD5.Create();
            var hash = md5.ComputeHash(stream);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ChangeLogEntry);
        }

        public bool Equals(ChangeLogEntry other)
        {
            return other != null &&
                   Version == other.Version &&
                   Description == other.Description &&
                   Filename == other.Filename &&
                   Hash == other.Hash &&
                   ChangeDateTime == other.ChangeDateTime &&
                   AppliedBy == other.AppliedBy &&
                   MigrationType == other.MigrationType;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Version, Description, Filename, Hash, ChangeDateTime, AppliedBy, MigrationType);
        }
    }
}
