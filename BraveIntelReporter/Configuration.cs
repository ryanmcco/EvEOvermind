﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace BraveIntelReporter
{
    public static class Configuration
    {
        /// <summary>
        /// How often to check for changes in the log file in milliseconds.  Can be set in intelGlobalConfig.xml. 
        /// </summary>
        public static int MonitorFrequency = 500;
        /// <summary>
        /// Frequency to check the server for global configuration updates in minutes.
        /// </summary>
        public static int ConfigCheckFrequency = 30;
        /// <summary>
        /// URL of the intel map.
        /// </summary>
        public static string MapURL = string.Empty;
        /// <summary>
        /// URL of the Planning map.
        /// </summary>
        public static string PlanningMapURL = string.Empty;
        /// <summary>
        /// URL of the Jump map.
        /// </summary>
        public static string JumpMapURL = string.Empty;
        /// <summary>
        /// URL of the search feature.
        /// </summary>
        public static string searchURL = string.Empty;
        /// <summary>
        /// URL of the veterans feature.
        /// </summary>
        public static string veteranURL = string.Empty;
        /// <summary>
        /// URL of the leaderboard feature.
        /// </summary>
        public static string leaderboardURL = string.Empty;
        /// <summary>
        /// URL of the 1v1 analysis feature.
        /// </summary>
        public static string onev1URL = string.Empty;
        /// <summary>
        /// URL of the ship fit analysis feature.
        /// </summary>
        public static string shipFitURL = string.Empty;
        /// <summary>
        /// URL of the add intel feature.
        /// </summary>
        public static string dscanURL = string.Empty;
        public static string dscanAPIURL = string.Empty;
        public static string settingsURL = string.Empty;
        /// <summary>
        /// Server to relay intel to.
        /// </summary>
        public static Uri ReportServer;
        /// <summary>
        /// Default directory of eve chat logs
        /// </summary>
        public static readonly string DefaultLogDirectory = System.IO.Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "EVE", "logs", "Chatlogs");
        /// <summary>
        /// Directory of eve chat logs
        /// </summary>
        public static string LogDirectory = DefaultLogDirectory;
        /// <summary>
        /// Room names to search for in the chat directory
        /// </summary>
        public static List<string> RoomsToMonitor;
        /// <summary>
        /// Retrieved from the server side webpage by the user.  Token that confirms identity with core.
        /// </summary>
        public static string AuthToken = string.Empty;
        /// <summary>
        /// Run on windows startup
        /// </summary>
        public static bool RunOnStartup = false;
        /// <summary>
        /// Windows startup registry key
        /// </summary>
        public static RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        /// <summary>
        /// Determines if this is the first time the user has run the application
        /// </summary>
        public static bool FirstRun = true;
        /// <summary>
        /// Folder to save application configuration files.
        /// </summary>
        public static string MyFolder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "EVE", "BraveIntelReporter");
        public static bool Verbose = true;
        public static bool SetEveToBackground = false;
        public static bool CheckClipboard = false;
        public static bool EnableReporting = true;

        public static bool GetConfig(out string report)
        {
            if (!Directory.Exists(MyFolder)) Directory.CreateDirectory(MyFolder);
            report = string.Empty;
            string localreport = string.Empty;
            string globalreport = string.Empty;
            GetLocalConfig(out localreport);
            bool globalsuccessful = GetGlobalConfig(out globalreport);
            report = localreport + "\r\n" + globalreport;
            return globalsuccessful;
        }

        private static void GetLocalConfig(out string report)
        {
            
            if (File.Exists(System.IO.Path.Combine(MyFolder, "IntelReporterLocalSettings.xml")))
            {
                FirstRun = false;
                XmlDocument configFile = new XmlDocument();
                configFile.Load(System.IO.Path.Combine(MyFolder, "IntelReporterLocalSettings.xml"));
                if (configFile.SelectSingleNode("BraveReporterSettings/LogDirectory") != null)
                    LogDirectory = configFile.SelectSingleNode("BraveReporterSettings/LogDirectory").InnerText;
                if (configFile.SelectSingleNode("BraveReporterSettings/AuthToken") != null)
                    AuthToken = configFile.SelectSingleNode("BraveReporterSettings/AuthToken").InnerText;
                if (configFile.SelectSingleNode("BraveReporterSettings/Verbose") != null)
                    Verbose = bool.Parse(configFile.SelectSingleNode("BraveReporterSettings/Verbose").InnerText);
                if (configFile.SelectSingleNode("BraveReporterSettings/SetEveToBackground") != null)
                    SetEveToBackground = bool.Parse(configFile.SelectSingleNode("BraveReporterSettings/SetEveToBackground").InnerText);
                if (configFile.SelectSingleNode("BraveReporterSettings/EnableReporting") != null)
                    EnableReporting = bool.Parse(configFile.SelectSingleNode("BraveReporterSettings/EnableReporting").InnerText);
                if (configFile.SelectSingleNode("BraveReporterSettings/CheckClipboard") != null)
                   CheckClipboard = bool.Parse(configFile.SelectSingleNode("BraveReporterSettings/CheckClipboard").InnerText);
                if (rkApp.GetValue("BraveIntelReporter") != null)
                    RunOnStartup = true;
                report = "Loaded local settings.";
            }
            else report = "Local settings not found. Using defaults.";
        }
        internal static bool GetGlobalConfig(out string report)
        {
            WebClient client = new WebClient();
            //get a copy of the latest intel xml, replace the local copy with it
            try
            {
                string configurlstring = "https://test.eveovermind.com/getConfig.php?token=" + AuthToken;
                Console.WriteLine("configurlstring " + configurlstring);
                client.DownloadFile(configurlstring, Path.Combine(MyFolder, "intelGlobalConfig.xml"));
                report = "Global config updated.";
            }
            catch (Exception)
            {
                if (!File.Exists(System.IO.Path.Combine(MyFolder, "intelGlobalConfig.xml")))
                {
                    report = "Could not load global config file.";
                    return false;
                }
                else report = "Failed to retrieve global config.  Using last known settings.";
            }
            RoomsToMonitor = new List<string>();
            XmlDocument configFile = new XmlDocument();
            //open the local copy
            configFile.Load(System.IO.Path.Combine(MyFolder, "intelGlobalConfig.xml"));
            //parse the xml for chatrooms
            foreach (XmlNode node in configFile.SelectNodes("BraveReporterSettings/chatrooms/chatroom"))
                //only type==intel chat rooms and then add it to the rooms to monitor list (created above)
                if (node.Attributes["type"].InnerText == "intel") RoomsToMonitor.Add(node.InnerText);
            ReportServer = new Uri(configFile.SelectSingleNode("BraveReporterSettings/IntelServer").InnerText);
            MonitorFrequency = int.Parse(configFile.SelectSingleNode("BraveReporterSettings/MonitorFrequency").InnerText);
            if (MonitorFrequency < 500) MonitorFrequency = 500;
            if (configFile.SelectSingleNode("BraveReporterSettings/MapLink") != null)
                MapURL = configFile.SelectSingleNode("BraveReporterSettings/MapLink").InnerText;
            if (configFile.SelectSingleNode("BraveReporterSettings/PlanningMapLink") != null)
                PlanningMapURL = configFile.SelectSingleNode("BraveReporterSettings/PlanningMapLink").InnerText;
            if (configFile.SelectSingleNode("BraveReporterSettings/JumpMapURL") != null)
                JumpMapURL = configFile.SelectSingleNode("BraveReporterSettings/JumpMapURL").InnerText;
            if (configFile.SelectSingleNode("BraveReporterSettings/searchURL") != null)
                searchURL = configFile.SelectSingleNode("BraveReporterSettings/searchURL").InnerText;
            if (configFile.SelectSingleNode("BraveReporterSettings/veteranURL") != null)
                veteranURL = configFile.SelectSingleNode("BraveReporterSettings/veteranURL").InnerText;
            if (configFile.SelectSingleNode("BraveReporterSettings/leaderboardURL") != null)
                leaderboardURL = configFile.SelectSingleNode("BraveReporterSettings/leaderboardURL").InnerText;
            if (configFile.SelectSingleNode("BraveReporterSettings/onev1URL") != null)
                onev1URL = configFile.SelectSingleNode("BraveReporterSettings/onev1URL").InnerText;
            if (configFile.SelectSingleNode("BraveReporterSettings/shipFitURL") != null)
                shipFitURL = configFile.SelectSingleNode("BraveReporterSettings/shipFitURL").InnerText;
            if (configFile.SelectSingleNode("BraveReporterSettings/dscanURL") != null)
                dscanURL = configFile.SelectSingleNode("BraveReporterSettings/dscanURL").InnerText;

            if (configFile.SelectSingleNode("BraveReporterSettings/dscanAPI") != null)
                dscanAPIURL = configFile.SelectSingleNode("BraveReporterSettings/dscanAPI").InnerText;

            if (configFile.SelectSingleNode("BraveReporterSettings/settingsURL") != null)
                settingsURL = configFile.SelectSingleNode("BraveReporterSettings/settingsURL").InnerText;

            return true;
        }

        internal static void Save()
        {
            Save(RunOnStartup);
        }

        internal static void Save(bool runOnStartup)
        {
            if (runOnStartup != Configuration.RunOnStartup)
            {
                if (runOnStartup) 
                    rkApp.SetValue("BraveIntelReporter", Application.ExecutablePath.ToString());
                else 
                    rkApp.DeleteValue("BraveIntelReporter", false);
                Configuration.RunOnStartup = runOnStartup;
            }
            XmlDocument localSettings = new XmlDocument();
            XmlNode rootNode = localSettings.CreateElement("BraveReporterSettings");
            localSettings.AppendChild(rootNode);

            XmlNode logdir = localSettings.CreateElement("LogDirectory");
            logdir.InnerText = Configuration.LogDirectory;
            rootNode.AppendChild(logdir);

            XmlNode authtoken = localSettings.CreateElement("AuthToken");
            authtoken.InnerText = Configuration.AuthToken;
            rootNode.AppendChild(authtoken);

            XmlNode setToBackGround = localSettings.CreateElement("SetEveToBackground");
            setToBackGround.InnerText = Configuration.SetEveToBackground.ToString();
            rootNode.AppendChild(setToBackGround);

            XmlNode verbose = localSettings.CreateElement("Verbose");
            verbose.InnerText = Configuration.Verbose.ToString();
            rootNode.AppendChild(verbose);

            XmlNode enableReporting = localSettings.CreateElement("EnableReporting");
            enableReporting.InnerText = Configuration.EnableReporting.ToString();
            rootNode.AppendChild(enableReporting);

            XmlNode checkClipboard = localSettings.CreateElement("CheckClipboard");
            checkClipboard.InnerText = Configuration.CheckClipboard.ToString();
            rootNode.AppendChild(checkClipboard);

            localSettings.Save(System.IO.Path.Combine(MyFolder, "IntelReporterLocalSettings.xml"));

        }
    }


}
