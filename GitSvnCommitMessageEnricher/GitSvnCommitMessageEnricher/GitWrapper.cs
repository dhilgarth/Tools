using System.Diagnostics;

namespace GitSvnCommitMessageEnricher
{
    /// <summary>
    /// Provides a simple wrapper around the GIT executables. Provides one method per command.
    /// </summary>
    public class GitWrapper
    {
        /// <summary>
        /// Executes the rev-parse command.
        /// </summary>
        /// <param name="args">The arguments (including options) for the command.</param>
        /// <returns>The GIT output for the command.</returns>
        public static string RevParse(string args)
        {
            return ExecuteGitCommand("rev-parse", args);
        }

        /// <summary>
        /// Executes the cat-file command.
        /// </summary>
        /// <param name="args">The arguments (including options) for the command.</param>
        /// <returns>The GIT output for the command.</returns>
        public static string CatFile(string args)
        {
            return ExecuteGitCommand("cat-file", args);
        }

        /// <summary>
        /// Executes an arbitrary git command.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <param name="args">The arguments (including options) for the command.</param>
        /// <returns>The GIT output for the command.</returns>
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
