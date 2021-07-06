using System;
using System.Diagnostics;
using System.IO;

namespace TeamsLauncher
{
    public static class TeamsHelper
    {
        private static readonly string[] _requiredDirectories =
        {
            "Desktop",
            "Downloads"
        };

        public static void Start(string teamsInstanceAlias = null)
        {
            if (teamsInstanceAlias == null)
            {
                if (!File.Exists("instances.cfg"))
                {
                    return;
                }

                var instances = File.ReadAllLines("instances.cfg");

                foreach (var instanceName in instances)
                {
                    LaunchInstance(instanceName, instanceName != "Default");
                }
            }
            else
            {
                var instanceName = teamsInstanceAlias;

                LaunchInstance(instanceName, instanceName != "Default");
            }
        }

        private static void LaunchInstance(string instance, bool overrideUserProfilePath = true)
        {
            var localAppDataPath = Environment.GetEnvironmentVariable("LOCALAPPDATA");

            var teamsPath = $"{localAppDataPath}\\Microsoft\\Teams";

            var processStartInfo = new ProcessStartInfo
            {
                FileName = $"{teamsPath}\\Update.exe",
                Arguments = "--processStart \"Teams.exe\"",
                WorkingDirectory = teamsPath,
                UseShellExecute = false
            };

            if (overrideUserProfilePath)
            {
                var userProfilePath = $"{teamsPath}\\instances\\{instance}";

                CreateDirectories(userProfilePath);

                processStartInfo.EnvironmentVariables["USERPROFILE"] = userProfilePath;
            }

            Process.Start(processStartInfo);
        }

        private static void CreateDirectories(string userProfilePath)
        {
            if (!Directory.Exists(userProfilePath))
            {
                Directory.CreateDirectory(userProfilePath);
            }

            foreach (var requiredDirectory in _requiredDirectories)
            {
                var path = Path.Combine(userProfilePath, requiredDirectory);

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }   
            }
        }
    }
}
