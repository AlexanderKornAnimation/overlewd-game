using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 https://blog.redbluegames.com/version-numbering-for-games-in-unity-and-git-1d05fca83022 
 */

namespace Overlewd
{
    public class GitException : InvalidOperationException
    {
        public GitException(int exitCode, string errors) : base(errors) =>
            this.ExitCode = exitCode;

        public readonly int ExitCode;
    }

    public static class Git
    {
        public static string buildVersion
        {
            get
            {
                var version = Run(@"describe --tags --long --match ""v[0-9]*""");
                // Remove initial 'v' and ending git commit hash.
                version = version.Replace('-', '.');
                version = version.Substring(1, version.LastIndexOf('.') - 1);
                return version;
            }
        }

        /// <summary>
        /// The currently active branch.
        /// </summary>
        public static string branch => Run(@"rev-parse --abbrev-ref HEAD");

        /// <summary>
        /// Returns a listing of all uncommitted or untracked (added) files.
        /// </summary>
        public static string status => Run(@"status --porcelain");

        public static void commit_all(string message) => Run($"add -A && git commit -m \"{message}\"");
        public static string push_force => Run(@"push --force origin HEAD");

        public static string Run(string arguments)
        {
            using (var process = new System.Diagnostics.Process())
            {
                var exitCode = process.Run(@"git", arguments, Application.dataPath,
                    out var output, out var errors);
                if (exitCode == 0)
                {
                    return output;
                }
                else
                {
                    throw new GitException(exitCode, errors);
                }
            }
        }
    }
}
