using System;
using System.IO;
using System.Text.RegularExpressions;

namespace GitSvnCommitMessageEnricher
{
    internal class Program
    {
        /// <summary>
        /// Gets the user in the form <c>Firstname Lastname &lt;mail@domain.com&gt;</c>.
        /// </summary>
        /// <param name="commitInfo">
        /// The commit info to get the user from.
        /// </param>
        /// <param name="userType">
        /// Type of the user.
        /// </param>
        /// <returns>
        /// The user in the form <c>Firstname Lastname &lt;mail@domain.com&gt;</c>.
        /// </returns>
        private static string GetUser(string commitInfo, string userType)
        {
            return Regex.Match(commitInfo, @"^" + userType + " ([^>]+>)", RegexOptions.Multiline).Groups[1].Value;
        }

        /// <summary>
        /// Gets the user date time from the commit info for the specified user type.
        /// </summary>
        /// <param name="commitInfo">
        /// The commit info.
        /// </param>
        /// <param name="userType">
        /// Type of the user.
        /// </param>
        /// <returns>
        /// The date time of the commit for the specified user type.
        /// </returns>
        private static DateTimeOffset GetUserDateTime(string commitInfo, string userType)
        {
            var result = Regex.Match(commitInfo, "^" + userType + @" [^>]+> ([0-9]+) (\+)([0-9]{2})([0-9]{2})", RegexOptions.Multiline);
            var timestamp = int.Parse(result.Groups[1].Value);
            var dateTime = new DateTime(1970, 1, 1) + TimeSpan.FromSeconds(timestamp);
            var offsetHours = int.Parse(result.Groups[3].Value);
            var offsetMinutes = int.Parse(result.Groups[4].Value);
            var offsetTotalMinutes = (offsetHours * 60) + offsetMinutes;
            if (result.Groups[2].Value == "-")
                offsetTotalMinutes *= -1;
            return new DateTimeOffset(dateTime, TimeSpan.FromMinutes(offsetTotalMinutes));
        }

        private static void Main(string[] args)
        {
            try
            {
                var revision = GitWrapper.RevParse("HEAD");
                if (GitWrapper.CatFile("-t " + revision) != "commit")
                    return;
                var commitInfo = GitWrapper.CatFile("commit " + revision);
                var author = GetUser(commitInfo, "author");
                var authorDateTime = GetUserDateTime(commitInfo, "author");
                var committer = GetUser(commitInfo, "committer");
                var committerDateTime = GetUserDateTime(commitInfo, "committer");
                var additionalCommitInfo = Environment.NewLine + "-------" + Environment.NewLine + "Committed by git-svn. Additional info about the commit:"
                                           + Environment.NewLine + "Author:    " + author + " @ " + authorDateTime + Environment.NewLine + "Committer: "
                                           + committer + " @ " + committerDateTime;
                File.AppendAllText(args[0], additionalCommitInfo);
            }
            catch (Exception e)
            {
                // Just fail silently and don't change the commit message.
            }
        }
    }
}
