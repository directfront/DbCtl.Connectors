using System;
using System.Text.RegularExpressions;

namespace DbCtl.Connectors
{
    /// <summary>
    /// Provides functionality to parse the filename into its constituents namely MigrationType, Version and Description.
    /// </summary>
    public static class FilenameParser
    {
        private const string RegexPattern = @"^(?<mt>(F|B|f|b))-(?<ver>(?<major>0|[1-9]\d*)\.(?<minor>0|[1-9]\d*)\.(?<patch>0|[1-9]\d*)(?:-(?<prerelease>(?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*)(?:\.(?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*))*))?(?:\+(?<buildmetadata>[0-9a-zA-Z-]+(?:\.[0-9a-zA-Z-]+)*))?)-(?<desc>[\w]+).(ddl|dml|dcl)$";

        /// <summary>
        /// Parses the filename to the MigrationType, Version and Description.
        /// </summary>
        /// <param name="filename">Filename to parse without the path.</param>
        /// <returns>MigrationType, Version and Description.</returns>
        public static (MigrationType Type, string Version, string Description) Parse(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                throw new ArgumentNullException(nameof(filename));

            var regex = new Regex(RegexPattern);
            var result = regex.Match(filename);

            if (!result.Success)
                throw new Exception($"Failed to parse {filename}.");

            return (
                result.Groups["mt"].Value.ToUpperInvariant() == "F" ? MigrationType.Forward : MigrationType.Backward,
                result.Groups["ver"].Value,
                result.Groups["desc"].Value.Replace("_", " ")
            );
        }
    }
}
