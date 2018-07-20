using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Microsoft.Web.Administration;
using NewNodeChecker.Database;
using NewNodeChecker.Models;
using Microsoft.Win32;
using System.Data.SqlClient;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.DirectoryServices;
using NewNodeChecker.Properties;

namespace NewNodeChecker
{

    public class Program
    {
        static readonly List<string> CodeExtensions = new List<string>() { ".dll", ".exe" };
        static readonly string DefinationSettingName = Properties.Settings.Default.DefinationSetting.ToLower();
        static readonly string CurrentMachineName = System.Environment.MachineName;
        static DefinationSetting _definationSetting;

        static int _serverLogId = 0;
        [STAThread]
        static void Main(string[] args)
        {
            //Create or Update Database
            CreateOrUpdateDatabase();

            //Create Defination
            using (var db = new LogDbContext())
            {
                _definationSetting = new DefinationSetting() { Name = DefinationSettingName };
                db.DefinationSettings.AddOrUpdate(x => x.Name, _definationSetting);
                db.SaveChanges();
            }

            using (var db = new LogDbContext())
            {
                //Delete Old ServerLogs
                List<ServerLog> lstServerLogs = (from s in db.ServerLogs
                                                 where s.MachineName == CurrentMachineName
                                                 select s).ToList();
                if (lstServerLogs.Count > 0)
                {
                    db.ServerLogs.RemoveRange(lstServerLogs);
                    db.SaveChanges();
                }

                //Create new ServerLog
                ServerLog oServerLog = new ServerLog
                {
                    Ip = LocalIPAddress(),
                    MachineName = CurrentMachineName,
                    StartDateTime = DateTime.Now
                };

                db.ServerLogs.Add(oServerLog);
                db.SaveChanges();
                _serverLogId = oServerLog.Id;

            }

            LogPortsAvailability();

            //Log file content for this file windows\system32\drivers\etc\hosts
            LogHostsFileContent();

            //Log installed apps 32 bit
            LogInstalledApps(Constants.InstalledApps32BitRegKey);

            var is64Bit = CheckIfPathExistsInRegistry(Constants.InstalledApps64BitRegKey);
            var isWindowsUpdate = CheckIfPathExistsInRegistry(Constants.WindowsUpdatesRegKey);

            //Log installed apps 64 bit
            if (is64Bit)
            {
                LogInstalledApps(Constants.InstalledApps64BitRegKey);
            }
            //Log Windows updates
            if (isWindowsUpdate)
            {
                LogInstalledApps(Constants.WindowsUpdatesRegKey);

            }


            Version iiSversion = GetIisVersion();
            if (iiSversion.Major == 6)
                LogIis6WebsitesInfo(_serverLogId, CurrentMachineName);
            else if (iiSversion.Major > 6)
                LogIis7WebsitesInfo(_serverLogId);

            LogSqlAccess();

            LogURLsHit();

            using (var db = new LogDbContext())
            {
                var oServerLog = (from s in db.ServerLogs
                                  where s.Id == _serverLogId
                                  select s).SingleOrDefault();

                if (oServerLog != null)
                {
                    oServerLog.EndDateTime = DateTime.Now;
                    db.SaveChanges();

                }
            }
        }

        private static bool CheckIfPathExistsInRegistry(string path)
        {
            bool isExists = Directory.Exists(path);
            if (!isExists)
            {
                try
                {
                    var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                    hklm.Close();
                    isExists = true;
                }
                catch
                {

                }
            }

            return isExists;
        }
        private static void LogPortsAvailability()
        {
            Console.WriteLine(Resources.LogPortsAvailability);
            RunStepLog steplog = new RunStepLog();
            int stepId = 0;
            using (var db = new LogDbContext())
            {
                steplog.ServerLogId = _serverLogId;
                steplog.StepName = Resources.LogPortsAvailability;
                steplog.StartDateTime = DateTime.Now;
                db.RunStepLog.Add(steplog);
                db.SaveChanges();
                stepId = steplog.Id;
            }


            List<PortInfoDefination> portInfo = GetPortsInfo();
            foreach (PortInfoDefination port in portInfo)
            {
                CheckProtAvailability(port.PortNo, port.Ip4Address, port.Id);
            }

            using (var db = new LogDbContext())
            {
                var oStepLog = (from s in db.RunStepLog
                                where s.Id == stepId
                                select s).SingleOrDefault();

                if (oStepLog != null)
                {
                    oStepLog.EndDateTime = DateTime.Now;
                    db.SaveChanges();

                }
            }
        }
        static void LogIis6WebsitesInfo(int serverLogId, string machine)
        {
            Console.WriteLine("LogWebsitesInfo");

            RunStepLog steplog = new RunStepLog();
            int stepId = 0;
            using (var db = new LogDbContext())
            {
                steplog.ServerLogId = serverLogId;
                steplog.StepName = "LogWebsitesInfo IIS6";
                steplog.StartDateTime = DateTime.Now;
                db.RunStepLog.Add(steplog);
                db.SaveChanges();
                stepId = steplog.Id;
            }

            string path = "IIS://localhost/W3SVC";
            try
            {
                using (DirectoryEntry w3Svc = new DirectoryEntry(path))
                {
                    foreach (DirectoryEntry entry in w3Svc.Children)
                    {
                        if (entry.SchemaClassName == "IIsWebServer")
                        {
                            WebSiteLog sitelog = new WebSiteLog();
                            string websiteName = (string)entry.Properties["ServerComment"].Value;
                            string physpath = GetPath(entry);
                            string name = entry.Name;
                            string state = entry.Properties["ServerState"].Value.ToString();
                            using (var db = new LogDbContext())
                            {

                                sitelog.ServerLogId = serverLogId;
                                sitelog.SiteName = websiteName;
                                sitelog.AppPhysicalPath = GetPath(entry);
                                db.WebSiteLogs.Add(sitelog);
                                db.SaveChanges();

                            }
                            LogWebSitesInfo(physpath, sitelog.Id);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            using (var db = new LogDbContext())
            {
                var oStepLog = (from s in db.RunStepLog
                                where s.Id == stepId
                                select s).SingleOrDefault();

                if (oStepLog != null)
                {
                    oStepLog.EndDateTime = DateTime.Now;
                    db.SaveChanges();

                }
            }

        }

        static void LogIis7WebsitesInfo(int serverLogId)
        {
            Console.WriteLine("LogWebSitesInfo");
            RunStepLog steplog = new RunStepLog();
            int stepId = 0;
            using (var db = new LogDbContext())
            {
                steplog.ServerLogId = serverLogId;
                steplog.StepName = "LogWebsitesInfo IIS7";
                steplog.StartDateTime = DateTime.Now;
                db.RunStepLog.Add(steplog);
                db.SaveChanges();
                stepId = steplog.Id;
            }
            using (ServerManager oServerManager = new ServerManager())
            {

                foreach (var site in oServerManager.Sites)
                {


                    foreach (Application app in site.Applications)
                    {

                        foreach (var virtualRoot in app.VirtualDirectories)
                        {

                            //app.ApplicationPoolName.
                            var firstOrDefault = site.Bindings.FirstOrDefault(b => b.Protocol == "http");
                            int webSiteLogId = 0;
                            using (var db = new LogDbContext())
                            {
                                var oServerLog = (from s in db.ServerLogs
                                                  where s.Id == serverLogId
                                                  select s).SingleOrDefault();
                                WebSiteLog oWebSiteLog = new WebSiteLog() { ServerLog = oServerLog };
                                oWebSiteLog.SiteName = site.Name;
                                //  site.VirtualDirectoryDefaults

                                oWebSiteLog.VirtualDirectoryName = (virtualRoot.Path == "/") ? app.Path : virtualRoot.Path;

                                oWebSiteLog.AppPhysicalPath = virtualRoot.PhysicalPath;
                                db.WebSiteLogs.Add(oWebSiteLog);
                                db.SaveChanges();
                                webSiteLogId = oWebSiteLog.Id;
                                LogWebSitesInfo(virtualRoot.PhysicalPath, webSiteLogId);
                            }
                        }
                    }
                }
            }
            using (var db = new LogDbContext())
            {
                var oServerLog = (from s in db.RunStepLog
                                  where s.Id == stepId
                                  select s).SingleOrDefault();

                if (oServerLog != null)
                {
                    oServerLog.EndDateTime = DateTime.Now;
                    db.SaveChanges();

                }
            }

        }
        static void LogWebSitesInfo(string path, int webSiteLogId)
        {

            //  Version version =  GetIISVersion();
            //Console.WriteLine("IIS Version is  " + version.ToString());

            //Console.WriteLine("LogWebSitesInfo");
            //  bool x = Directory.Exists(Path.Combine(virtualRoot.PhysicalPath, "bin"));
            bool x = Directory.Exists(Path.Combine(path, "bin"));
            try
            {
                if (x)
                {
                    string[] fileEntries1 = Directory.GetFiles(path);
                    // Directory.GetFiles(virtualRoot.PhysicalPath);
                    string[] fileEntries = Directory.GetFiles(Path.Combine(path, "bin"));
                    // Directory.GetFiles(Path.Combine(virtualRoot.PhysicalPath, "bin"));

                    foreach (var fileentry in fileEntries)
                    {
                        FileInfo filInfo = new FileInfo(fileentry);

                        if (filInfo.Exists)
                        {

                            using (var db = new LogDbContext())
                            {
                                var oWebSiteLog = (from s in db.WebSiteLogs
                                                   where s.Id == webSiteLogId
                                                   select s).SingleOrDefault();
                                WebSiteFileLog oWebSiteFileLog = new WebSiteFileLog() { WebSiteLog = oWebSiteLog };
                                oWebSiteFileLog.Extension = filInfo.Extension;
                                oWebSiteFileLog.FileName = filInfo.Name;
                                oWebSiteFileLog.LastModificationDate = filInfo.LastWriteTime;
                                oWebSiteFileLog.PhysicalPath = filInfo.FullName;
                                oWebSiteFileLog.Size = filInfo.Length;
                                if (CodeExtensions.Contains(filInfo.Extension.ToLower()))
                                {
                                    FileVersionInfo myFileVersionInfo =
                                        FileVersionInfo.GetVersionInfo(fileentry);
                                    oWebSiteFileLog.BuildNo = myFileVersionInfo.FileVersion;
                                }

                                db.WebSiteFileLogs.Add(oWebSiteFileLog);
                                db.SaveChanges();
                            }
                        }
                    }


                }


                //bool exists = Directory.Exists(virtualRoot.PhysicalPath);
                bool exists = Directory.Exists(path);
                if (exists && Properties.Settings.Default.IsModelNode)
                {
                    LogConfig(webSiteLogId, 0, path);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }



        public static string LocalIPAddress()
        {
            IPHostEntry host;
            string localIP = "";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    break;
                }
            }
            return localIP;
        }
        private static void LogHostsFileContent()
        {
            Console.WriteLine("LogHostsFileContent");
            RunStepLog steplog = new RunStepLog();
            int stepId = 0;
            using (var db = new LogDbContext())
            {
                steplog.ServerLogId = _serverLogId;
                steplog.StepName = "LogHostsFileContent";
                steplog.StartDateTime = DateTime.Now;
                db.RunStepLog.Add(steplog);
                db.SaveChanges();
                stepId = steplog.Id;
            }
            using (var db = new LogDbContext())
            {
                var oServerLog = (from s in db.ServerLogs
                                  where s.Id == _serverLogId
                                  select s).SingleOrDefault();

                HostsFileLog oHostsFileLog = new HostsFileLog() { ServerLog = oServerLog };
                try
                {
                    string fileContent = File.ReadAllText(Path.Combine(Environment.SystemDirectory, @"drivers\etc\hosts"));
                    oHostsFileLog.FileContent = fileContent;

                }
                catch (Exception ex)
                {
                    oHostsFileLog.Exception = ex.StackTrace;
                }


                db.HostsFileLogs.Add(oHostsFileLog);
                db.SaveChanges();
            }
            using (var db = new LogDbContext())
            {
                var oStepLog = (from s in db.RunStepLog
                                where s.Id == stepId
                                select s).SingleOrDefault();

                if (oStepLog != null)
                {
                    oStepLog.EndDateTime = DateTime.Now;
                    db.SaveChanges();

                }
            }
        }
        static void LogInstalledApps(string uninstallKey)
        {
            Console.WriteLine("LogInstalledApps");
            Console.WriteLine("LogInstalledApps " + uninstallKey);
            RunStepLog steplog = new RunStepLog();
            int stepId = 0;
            using (var db = new LogDbContext())
            {
                steplog.ServerLogId = _serverLogId;
                steplog.StepName = "LogInstalledApps";
                steplog.StartDateTime = DateTime.Now;
                db.RunStepLog.Add(steplog);
                db.SaveChanges();
                stepId = steplog.Id;
            }

            //   Console.WriteLine(uninstallKey);
            try
            {


                using (var db = new LogDbContext())
                {
                    var oServerLog = (from s in db.ServerLogs
                                      where s.Id == _serverLogId
                                      select s).SingleOrDefault();

                    using (RegistryKey rk = Registry.LocalMachine.OpenSubKey(uninstallKey))
                    {
                        string strProgram, displayName, displayVersion, installDate, installSource;
                        foreach (string skName in rk.GetSubKeyNames())
                        {
                            using (RegistryKey sk = rk.OpenSubKey(skName))
                            {
                                try
                                {
                                    strProgram = displayName = displayVersion = installDate = installSource = string.Empty;
                                    if (sk.GetValue("DisplayName") != null)
                                    {
                                        displayName = sk.GetValue("DisplayName").ToString();
                                    }

                                    if (sk.GetValue("DisplayVersion") != null)
                                    {
                                        displayVersion = sk.GetValue("DisplayVersion").ToString();
                                    }

                                    if (sk.GetValue("InstallDate") != null)
                                    {
                                        installDate = sk.GetValue("InstallDate").ToString();
                                    }

                                    if (sk.GetValue("InstallSource") != null)
                                    {
                                        installSource = sk.GetValue("InstallSource").ToString();
                                    }

                                    strProgram = string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\"", displayName,
                                        displayVersion, installDate, installSource);
                                    if (!strProgram.Equals("\"\",\"\",\"\",\"\""))
                                    {
                                        InstalledAppLog oInstalledAppsLog = new InstalledAppLog
                                        {
                                            ServerLog = oServerLog,
                                            DisplayName = displayName
                                        };

                                        if (!string.IsNullOrEmpty(displayVersion))
                                            oInstalledAppsLog.DisplayVersion = displayVersion;

                                        if (!string.IsNullOrEmpty(installDate))
                                        {
                                            DateTime d;
                                            if (
                                                DateTime.TryParseExact(
                                                    installDate,
                                                    @"yyyyMMdd",
                                                    CultureInfo.InvariantCulture,
                                                    DateTimeStyles.AssumeUniversal,
                                                    out d))
                                                oInstalledAppsLog.InstallDate = d;
                                        }

                                        if (!string.IsNullOrEmpty(installSource))
                                            oInstalledAppsLog.InstallSource = installSource;

                                        db.InstalledAppsLogs.Add(oInstalledAppsLog);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                        }
                    }
                    db.SaveChanges();
                }
            }

            catch (Exception ex2)
            {
                Console.WriteLine(ex2.Message);
            }
            using (var db = new LogDbContext())
            {
                var oStepLog = (from s in db.RunStepLog
                                where s.Id == stepId
                                select s).SingleOrDefault();

                if (oStepLog != null)
                {
                    oStepLog.EndDateTime = DateTime.Now;
                    db.SaveChanges();

                }
            }
        }
        static void LogSqlAccess()
        {
            Console.WriteLine("LogSqlAccess");

            RunStepLog steplog = new RunStepLog();
            int stepId = 0;
            using (var db = new LogDbContext())
            {
                steplog.ServerLogId = _serverLogId;
                steplog.StepName = "LogSqlAccess";
                steplog.StartDateTime = DateTime.Now;
                db.RunStepLog.Add(steplog);
                db.SaveChanges();
                stepId = steplog.Id;
            }
            using (var db = new LogDbContext())
            {
                var oServerLog = (from s in db.ServerLogs

                                  where s.Id == _serverLogId
                                  select s).SingleOrDefault();

                var lstSqlConnectionDefinations =
                    (from p in db.SqlConnectionDefinations
                     where p.IsEnabled && p.DefinationSettingId.Equals(_definationSetting.Id)
                     select p);


                foreach (SqlConnectionDefination oSqlConnectionDefination in lstSqlConnectionDefinations)
                {
                    SqlTransResultLog oSqlTransResultLogs = new SqlTransResultLog() { SqlConnectionDefination = oSqlConnectionDefination, ServerLog = oServerLog };


                    string connectionString = oSqlConnectionDefination.SqlConnection;
                    string queryString = oSqlConnectionDefination.SqlStatment;
                    try
                    {
                        using (SqlConnection connection =
                          new SqlConnection(connectionString))
                        {

                            SqlCommand command = new SqlCommand(queryString, connection);


                            connection.Open();

                            SqlDataReader reader = command.ExecuteReader();


                            oSqlTransResultLogs.RowsCount = reader.FieldCount;
                            oSqlTransResultLogs.ServerLog = oServerLog;

                            oSqlTransResultLogs.Status = true;
                            reader.Close();
                            connection.Close();

                            db.SqlTransResultLogs.Add(oSqlTransResultLogs);
                        }
                    }
                    catch (Exception ex)
                    {
                        using (var db1 = new LogDbContext())
                        {
                            oSqlTransResultLogs.Exception = ex.Message;
                            oSqlTransResultLogs.Status = false;
                            db.SqlTransResultLogs.Add(oSqlTransResultLogs);

                        }

                    }

                }

                db.SaveChanges();

            }
            using (var db = new LogDbContext())
            {
                var oStepLog = (from s in db.RunStepLog
                                where s.Id == stepId
                                select s).SingleOrDefault();

                if (oStepLog != null)
                {
                    oStepLog.EndDateTime = DateTime.Now;
                    db.SaveChanges();

                }
            }
        }
        static void CheckProtAvailability(int portNumber, string ip, int portId)
        {
            PortResultLog oPortResultLog = null; ;
            using (var db = new LogDbContext())
            {
                try
                {
                    var oServerLog = (from s in db.ServerLogs
                                      where s.Id == _serverLogId
                                      select s).SingleOrDefault();

                    oPortResultLog = new PortResultLog() { ServerLog = oServerLog, PortId = portId };

                    IPAddress ipa = null;
                    if (!IPAddress.TryParse(ip, out ipa))
                    {
                        ipa = System.Net.Dns.Resolve(ip).AddressList[0];
                    }


                    System.Net.Sockets.Socket sock =
                        new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork,
                                                      System.Net.Sockets.SocketType.Stream,
                                                      System.Net.Sockets.ProtocolType.Tcp);

                    sock.Connect(ipa, portNumber);


                    oPortResultLog.IsOpened = sock.Connected == true;
                    sock.Close();

                }
                catch (Exception ex)
                {
                    oPortResultLog.Exception = ex.StackTrace;
                    oPortResultLog.IsOpened = false;
                }
                db.PortResultLogs.Add(oPortResultLog);
                db.SaveChanges();


            }
        }
        static List<PortInfoDefination> GetPortsInfo()
        {

            using (var db = new LogDbContext())
            {
                var lstPorts =
                    (from p in db.PortInfoDefinations
                     where p.IsEnabled
                     && p.DefinationSettingId.Equals(_definationSetting.Id)
                     select p).ToList();

                return lstPorts;
            }
        }
        static void LogConfig(int siteLogId, int taskLogId, string virtualDir)
        {
            ConfigFileLog configfile = new ConfigFileLog();
            using (var db = new LogDbContext())
            {
                try
                {
                    string[] files = System.IO.Directory.GetFiles(virtualDir, "*.config");

                    foreach (string file in files)
                    {
                        configfile.ConfigFileName = Path.GetFileName(file);
                        XDocument xDoc = XDocument.Parse(File.ReadAllText(file));
                        configfile.ConfigFileContent = xDoc.ToString();


                        configfile.ConfigFileName = Path.GetFileName(file);



                        configfile.LastModificationDate = System.IO.File.GetLastWriteTime(file);
                        if (siteLogId > 0)
                            configfile.WebSiteLogId = siteLogId;


                        configfile.Type = "Site";
                        db.ConfigFileLogs.Add(configfile);
                        db.SaveChanges();
                        var configId = configfile.Id;
                        LogConfigConnectionStrings(configId, xDoc);
                        LogIp(configId, xDoc, configfile.ConfigFileContent);
                        LogConfigLinks(configId, xDoc, configfile.ConfigFileContent);
                        //LogEndPoints(ConfigID, xDoc);

                    }

                }
                catch (Exception ex)
                {

                    configfile.Exception = ex.StackTrace;

                    db.ConfigFileLogs.Add(configfile);
                    db.SaveChanges();
                }

            }
        }
        static void LogConfigConnectionStrings(int configId, XDocument configContent)
        {
            //  Console.WriteLine("ConnectionStrings");
            try
            {
                Regex ipRegex = new Regex(@"connectionString=""([^""]*)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                MatchCollection regMatchCollection = ipRegex.Matches(configContent.ToString());
                foreach (Match regMatch in regMatchCollection)
                {
                    string connString = regMatch.Groups[1].Value;

                    if (connString.Contains("Entity"))
                    {
                        Regex entityFrmRegex = new Regex("data source=([^;]*);initial catalog=([^;]*);user id=([^;]*);password=([^;]*);");
                        Match macth = entityFrmRegex.Match(connString);
                        if (regMatch.Success)
                        {
                            connString = string.Format("server={0};DataBase={1};UID={2};PWD={3};", macth.Groups[1].Value, macth.Groups[2].Value, macth.Groups[3].Value, macth.Groups[4].Value);
                        }
                    }
                    using (var db = new LogDbContext())
                    {


                        int connId = (from u in db.SqlConnectionDefinations
                                      join p in db.SqlTransResultLogs on
                                      u.Id equals p.SqlConnectionDefinationId
                                      where u.SqlConnection == connString
                                      && u.DefinationSettingId == _definationSetting.Id
                                      && p.ServerLogId == _serverLogId
                                      select u.Id).SingleOrDefault();

                        SqlConnectionDefination sqlConDef = new SqlConnectionDefination();
                        ConfigConnectionStringLog conficonnections = new ConfigConnectionStringLog();

                        if (connId == 0)
                        {
                            sqlConDef.SqlConnection = connString;
                            sqlConDef.SqlStatment = "select getdate()";
                            sqlConDef.Name = "Config connection String";

                            sqlConDef.IsEnabled = true;

                            sqlConDef.DefinationSettingId = _definationSetting.Id;
                            db.SqlConnectionDefinations.Add(sqlConDef);

                            db.SaveChanges();

                            conficonnections.ConfigFileLogId = configId;
                            conficonnections.ConnectionSting = connString;// element.ToString();
                            conficonnections.SqlConnectionDefinationId = sqlConDef.Id;
                            db.ConfigConnectionStringLog.Add(conficonnections);
                            db.SaveChanges();
                        }


                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        static void LogConfigLinks(int configId, XDocument configContent, string content)
        {
            // Console.WriteLine("ConfigLink");
            try
            {
                ConfigLinksDefinition logLinks = new ConfigLinksDefinition();
                ConfigURLBridge bridge = new ConfigURLBridge();
                Regex urlRx = new Regex(@"(http|ftp|https)://([\w+?\.\w+])+([a-zA-Z0-9\~\!\@\#\$\%\^\&\*\(\)_\-\=\+\\\/\?\.\:\;\'\,]*)?", RegexOptions.IgnoreCase);

                MatchCollection matches = urlRx.Matches(content);

                using (var db = new LogDbContext())
                {
                    if (matches.Count > 0)
                    {
                        for (int i = 0; i < matches.Count; i++)
                        {
                            string url = matches[i].Value;

                            if (!url.ToLower().Contains("go.microsoft.com/fwlink/?linkid=") &&
                                !url.ToLower().Contains("schemas.microsoft.com/applicationinsights/2013/settings"))
                            {

                                logLinks.Links = matches[i].Value;
                                logLinks.DefinationSettingId = _definationSetting.Id;
                                logLinks.Name = url;

                                db.ConfigLinksDefinition.Add(logLinks);

                                db.SaveChanges();
                                bridge.ConfigId = configId;
                                bridge.URLId = logLinks.Id;
                                db.ConfigURLBridge.Add(bridge);
                                db.SaveChanges();
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException);

            }
        }
        static void LogIp(int configId, XDocument configContent, string content)
        {
            ConfigIPLog ConfigIP = new ConfigIPLog();
            using (var db = new LogDbContext())
            {

                //      Regex ipRX = new Regex(@"(Version=)?""(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})""", RegexOptions.IgnoreCase);
                Regex ipRX = new Regex(@"key=""([^\\""]*)""\s*value=""(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})""", RegexOptions.IgnoreCase);
                //    MatchCollection ipMatches = ipRX.Matches(Content);
                MatchCollection ipMatches = ipRX.Matches(content);

                if (ipMatches.Count > 0)
                {

                    for (int i = 0; i < ipMatches.Count; i++)
                    {
                        // if (String.IsNullOrEmpty(ipMatches[i].Groups[1].Value))
                        // Console.WriteLine(ipMatches[i].Groups[2].Value);
                        //{
                        // IEnumerable<XElement> appsetting = ConfigContent.Root.Element("appSettings").Elements();
                        //   XElement IP = XElement.Parse(ipMatches[i].Groups[1].Value);

                        ConfigIP.IP = ipMatches[i].Groups[2].Value;
                        ConfigIP.Key = ipMatches[i].Groups[1].Value;
                        ConfigIP.ConfigFileLogId = configId;
                        db.ConfigIPLog.Add(ConfigIP);
                        db.SaveChanges();
                        // }
                    }


                }
            }
        }
        static void LogURLsHit()
        {
            Console.WriteLine(Resources.LogURLsHit);
            RunStepLog steplog = new RunStepLog();
            int stepId = 0;
            using (var db = new LogDbContext())
            {
                steplog.ServerLogId = _serverLogId;
                steplog.StepName = Resources.LogURLsHit;
                steplog.StartDateTime = DateTime.Now;
                db.RunStepLog.Add(steplog);
                db.SaveChanges();
                stepId = steplog.Id;
            }
            List<ConfigLinksDefinition> lstConfigLinksDefinition = new List<ConfigLinksDefinition>();

            using (var db = new LogDbContext())
            {
                lstConfigLinksDefinition = (from p in db.ConfigLinksDefinition
                                            where p.DefinationSettingId == _definationSetting.Id
                                            select p).ToList();
            }



            foreach (ConfigLinksDefinition link in lstConfigLinksDefinition)
            {
                using (var db = new LogDbContext())
                {
                    ConfigLinksLog responseLog = new ConfigLinksLog();
                    try
                    {
                        Uri uri = new Uri(link.Links);

                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                        request.Method = WebRequestMethods.Http.Head;
                        request.KeepAlive = false;
                        request.ProtocolVersion = HttpVersion.Version10;
                        //request.Method = "GET";
                        request.Timeout = Properties.Settings.Default.TimeOut;
                        responseLog.ConfigLinksDefinitionId = link.Id;
                        responseLog.ServerLogId = _serverLogId;
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();


                        responseLog.Status = response.StatusCode == HttpStatusCode.OK ? "Succeed" : "Failed";

                        db.ConfigLinksLog.Add(responseLog);
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        responseLog.ConfigLinksDefinitionId = link.Id;
                        responseLog.Exception = ex.Message;
                        responseLog.Status = "Failed";
                        db.ConfigLinksLog.Add(responseLog);
                        responseLog.ServerLogId = _serverLogId;
                        db.SaveChanges();
                    }

                }




            }

            using (var db = new LogDbContext())
            {
                var oStepLog = (from s in db.RunStepLog
                                where s.Id == stepId
                                select s).SingleOrDefault();

                if (oStepLog != null)
                {
                    oStepLog.EndDateTime = DateTime.Now;
                    db.SaveChanges();

                }
            }

        }

        public static Version GetIisVersion()
        {
            using (RegistryKey key =
            Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\InetStp", false))
            {
                if (key == null) return new Version(0, 0);
                int majorVersion = (int)key.GetValue("MajorVersion", -1);
                int minorVersion = (int)key.GetValue("MinorVersion", -1);
                if (majorVersion == -1 || minorVersion == -1) return new Version(0, 0);
                return new Version(majorVersion, minorVersion);
            }
        }

        static String GetPath(DirectoryEntry iisWebServer)
        {
            foreach (DirectoryEntry iisEntity in iisWebServer.Children)
            {
                if (iisEntity.SchemaClassName == "IIsWebVirtualDir")
                    return iisEntity.Properties["Path"].Value.ToString();
            }
            return null;
        }
        static void CreateOrUpdateDatabase()
        {

            try
            {
                var configurator = new NewNodeChecker.Migrations.Configuration();
               
                DbMigrator migrator = new DbMigrator(configurator);
                migrator.Configuration.AutomaticMigrationsEnabled = true;
                migrator.Configuration.AutomaticMigrationDataLossAllowed = true;

                
                migrator.Update();
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
