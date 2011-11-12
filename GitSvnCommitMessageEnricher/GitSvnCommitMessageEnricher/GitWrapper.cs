using System.Diagnostics;

namespace GitSvnCommitMessageEnricher
{
    /// <summary>
    /// Provides a simple wrapper around the GIT executables. Provides one method per command.
    /// </summary>
    public class GitWrapper
    {
        public static string RevParse(string args)
        {
            return ExecuteGitCommand("rev-parse", args);
        }

        public static string CatFile(string args)
        {
            return ExecuteGitCommand("cat-file", args);
        }

        public static string ExecuteGitCommand(string command, string args)
        {
            var process = new Process();
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.FileName = "git";
            process.StartInfo.Arguments = command + " " + args.Trim();
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            return process.StandardOutput.ReadToEnd().Trim();
        }
    }
}
