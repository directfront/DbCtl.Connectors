using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

[assembly: InternalsVisibleTo("DbCtl.Connectors.UnitTests")]

namespace DbCtl.Interfaces
{
    public class ChangeLogEntry : IEquatable<ChangeLogEntry>
    {
        public string Version { get; private set; }
        public string Description { get; private set; }
        public string Filename { get; private set; }
        public string Hash { get; private set; }
        public DateTime ChangeDateTime { get; private set; }
        public string User { get; private set; }
        public string MigrationType { get; private set; }

        internal static ChangeLogEntry Parse(string filename, string user, DateTime changeDateTime, string hash)
        {
            if (string.IsNullOrEmpty(filename))
                throw new ArgumentNullException(nameof(filename));

            var regex = new Regex(@"^(?<mt>(f|b))-(?<ver>\d+.\d+.\d+)-(?<desc>[\w]+).(ddl|dml|dcl)$");

            var result = regex.Match(filename);
            if (!result.Success)
                throw new Exception($"Failed to parse {filename} to a ChangeLogEntry.");

            return new ChangeLogEntry
            {
                MigrationType = result.Groups["mt"].Value.ToUpper(),
                Version = result.Groups["ver"].Value,
                Description = result.Groups["desc"].Value.Replace("_", " "),
                User = user,
                ChangeDateTime = changeDateTime,
                Hash = hash
            };
        }

        public static ChangeLogEntry Parse(string filename)
        {
            return Parse(filename, Environment.UserName, DateTime.Now, CalculateMD5(filename));
        }

        private static string CalculateMD5(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
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
                   User == other.User &&
                   MigrationType == other.MigrationType;
        }

        public override int GetHashCode()
        {
            int hashCode = -1700650778;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Version);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Description);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Filename);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Hash);
            hashCode = hashCode * -1521134295 + ChangeDateTime.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(User);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(MigrationType);
            return hashCode;
        }
    }
}
