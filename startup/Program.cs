using System;
using System.Diagnostics;
using System.IO;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Collections.Generic;
using System.Management;
using System.Text.RegularExpressions;
using System.Text;

namespace startup
{
    class Program
    {
        static void Main(string[] args)
        {

            try
            {
                //ExtractSQLEXPR1();
                //ExtractSQLEXPR2();
                InstallSqlEngine();
                //InstallSQLEXP();
                //SetupIIS();
                //InstallSQLEXP();
                //ProcessFolder();
                //InstallAccessDatabaseEngine();
                Console.WriteLine("Done. Press any key to close.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occurred:" + ex.Message);
            }
            Console.ReadLine();

            //ProcessFolder();
            //EnableIIS();
            //EnableDotNet3();
            //EnableDotNet4();
            //InstallSQLEXP();
            //Installdotnet5();
            //Installdotnetcore();
            //InstallAspNetCore();
            //InstallAspNetCoreHosting();
            //InstallAccessDatabaseEngine();
        }
        static void EnableIIS()
        {
            string command = "Install-WindowsFeature -name Web-Server -IncludeManagementTools";
            ProcessStartInfo pStartInfo = new ProcessStartInfo("powershell.exe", "/c " + command);
            pStartInfo.Arguments = " /S /v/qn";
            pStartInfo.CreateNoWindow = true;
            pStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            pStartInfo.UseShellExecute = false;
            Process p = new Process();
            p.StartInfo = pStartInfo;
            p.Start();
        }
        static void EnableDotNet3()
        {
            string command = "DISM.exe /online /enable-feature /all /featurename:NetFx3";
            ProcessStartInfo pStartInfo = new ProcessStartInfo("cmd.exe", "/c " + command);
            pStartInfo.Arguments = "–s –v –qn";
            pStartInfo.CreateNoWindow = true;
            pStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            pStartInfo.UseShellExecute = false;
            Process p = new Process();
            p.StartInfo = pStartInfo;
            p.Start();
        }
        static void EnableDotNet4()
        {
            string command = "DISM.exe /online /enable-feature /all /featurename:NetFx4";
            ProcessStartInfo pStartInfo = new ProcessStartInfo("cmd.exe", "/c " + command);
            Process p = new Process();
            p.StartInfo = pStartInfo;
            p.Start();
        }

        static void ExtractSQLEXP()
        {
            Process p = new Process();
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = Directory.GetCurrentDirectory() + @"\Prerequisites\SQL\SQLEXPR_x64_ENU.exe";//  Application.StartupPath.Trim() + @"\SqlServer2005 Express\SQLEXPR.EXE";
            //psi.Arguments = "/q";
            psi.Arguments = "/q username=\"sa\" companyname=\"Rumtek\" addlocal=ALL  disablenetworkprotocols=\"0\" instancename=\"SQLExpress\" SECURITYMODE=\"SQL\" SAPWD=\"12\"";
            psi.CreateNoWindow = true;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            psi.UseShellExecute = false;
            p.StartInfo = psi;
            p.Start();
        }
        static void InstallSQLEXP()
        {
            Process p = new Process();
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = Directory.GetCurrentDirectory() + @"\Prerequisites\SQL\SQLEXPR_2019\SETUP.exe";//  Application.StartupPath.Trim() + @"\SqlServer2005 Express\SQLEXPR.EXE";
            //psi.Arguments = "/q";
            psi.Arguments = "/q username=\"sa\" companyname=\"Rumtek\" addlocal=ALL  disablenetworkprotocols=\"0\" instancename=\"SQLExpress\" SECURITYMODE=\"SQL\" SAPWD=\"12\"";
            psi.CreateNoWindow = true;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            psi.UseShellExecute = false;
            p.StartInfo = psi;
            p.Start();
        }
        static void Installdotnet5()
        {
            Process p = new Process();
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = Directory.GetCurrentDirectory() + @"\dotnet5\dotnet-runtime-5.0.11-win-x64.exe";//  Application.StartupPath.Trim() + @"\SqlServer2005 Express\SQLEXPR.EXE";
            //-q[n|b|r|f]   Sets user interface (UI) level:
            //n = no UI
            //b = basic UI (progress only, no prompts)
            //r = reduced UI (dialog at the end of installation)
            //f = full UI
            //psi.Arguments = "/qb username=\"sa\" companyname=\"Rumtek\" addlocal=ALL  disablenetworkprotocols=\"0\" instancename=\"SQLExpress\" SECURITYMODE=\"SQL\" SAPWD=\"lock\"";
            p.StartInfo = psi;
            p.Start();
        }
        static void Installdotnetcore()
        {
            Process p = new Process();
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = Directory.GetCurrentDirectory() + @"\dotnetcore\dotnet-runtime-3.1.20-win-x64.exe";//  Application.StartupPath.Trim() + @"\SqlServer2005 Express\SQLEXPR.EXE";
            //-q[n|b|r|f]   Sets user interface (UI) level:
            //n = no UI
            //b = basic UI (progress only, no prompts)
            //r = reduced UI (dialog at the end of installation)
            //f = full UI
            //psi.Arguments = "/qb username=\"sa\" companyname=\"Rumtek\" addlocal=ALL  disablenetworkprotocols=\"0\" instancename=\"SQLExpress\" SECURITYMODE=\"SQL\" SAPWD=\"lock\"";
            p.StartInfo = psi;
            p.Start();
        }
        static void InstallAccessDatabaseEngine()
        {
            Process p = new Process();
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = Directory.GetCurrentDirectory() + @"\Prerequisites\AccessDatabaseEngine\accessdatabaseengine_X64.exe";//  Application.StartupPath.Trim() + @"\SqlServer2005 Express\SQLEXPR.EXE";
            psi.Arguments = "/q";
            psi.CreateNoWindow = true;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            psi.UseShellExecute = false;
            p.StartInfo = psi;
            p.Start();
        }
        static void InstallAspNetCore()
        {
            Process p = new Process();
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = Directory.GetCurrentDirectory() + @"\dotnetcore\aspnetcore-runtime-3.1.20-win-x64.exe";//  Application.StartupPath.Trim() + @"\SqlServer2005 Express\SQLEXPR.EXE";
            //-q[n|b|r|f]   Sets user interface (UI) level:
            //n = no UI
            //b = basic UI (progress only, no prompts)
            //r = reduced UI (dialog at the end of installation)
            //f = full UI
            //psi.Arguments = "/qb username=\"sa\" companyname=\"Rumtek\" addlocal=ALL  disablenetworkprotocols=\"0\" instancename=\"SQLExpress\" SECURITYMODE=\"SQL\" SAPWD=\"lock\"";
            p.StartInfo = psi;
            p.Start();
        }
        static void InstallAspNetCoreHosting()
        {
            Process p = new Process();
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = Directory.GetCurrentDirectory() + @"\dotnetcore\dotnet-hosting-3.1.3-win.exe";//  Application.StartupPath.Trim() + @"\SqlServer2005 Express\SQLEXPR.EXE";
            //-q[n|b|r|f]   Sets user interface (UI) level:
            //n = no UI
            //b = basic UI (progress only, no prompts)
            //r = reduced UI (dialog at the end of installation)
            //f = full UI
            //psi.Arguments = "/qb username=\"sa\" companyname=\"Rumtek\" addlocal=ALL  disablenetworkprotocols=\"0\" instancename=\"SQLExpress\" SECURITYMODE=\"SQL\" SAPWD=\"lock\"";
            p.StartInfo = psi;
            p.Start();
        }

        static void deployApplications(string executableFilePath)
        {
            PowerShell powerShell = null;
            Console.WriteLine(" ");
            Console.WriteLine("Deploying application...");
            try
            {
                using (powerShell = PowerShell.Create())
                {
                    powerShell.AddScript(executableFilePath + " /S /v/qn");

                    Collection<PSObject> PSOutput = powerShell.Invoke();
                    foreach (PSObject outputItem in PSOutput)
                    {

                        if (outputItem != null)
                        {
                            Console.WriteLine(outputItem.BaseObject.GetType().FullName);
                            Console.WriteLine(outputItem.BaseObject.ToString() + "\n");
                        }
                    }

                    if (powerShell.Streams.Error.Count > 0)
                    {
                        string temp = powerShell.Streams.Error.First().ToString();
                        Console.WriteLine("Error: {0}", temp);

                    }
                    else
                        Console.WriteLine("Installation has completed successfully.");

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occured: {0}", ex.InnerException);
                //throw;
            }
            finally
            {
                if (powerShell != null)
                    powerShell.Dispose();
            }

        }
        private static void ProcessFolder()
        {
            const string SOURCEFOLDERPATH = @"\Prerequisites\dotnet";

            if (Directory.Exists(Directory.GetCurrentDirectory() + SOURCEFOLDERPATH))
            {
                Console.WriteLine("Directory exists at: {0}", Directory.GetCurrentDirectory() + SOURCEFOLDERPATH);
                if (Directory.GetFiles(Directory.GetCurrentDirectory() + SOURCEFOLDERPATH, "*.exe").Length > 0)
                {
                    int count = Directory.GetFiles(Directory.GetCurrentDirectory() + SOURCEFOLDERPATH, "*.exe").Length;
                    string[] files = Directory.GetFiles(Directory.GetCurrentDirectory() + SOURCEFOLDERPATH, "*.exe");
                    var files_ord = files.OrderByDescending(i => i).ToArray();
                    foreach (var file in files_ord)
                    {
                        var fileName = Path.GetFileName(file);
                        var fileNameWithPath = Directory.GetCurrentDirectory() + SOURCEFOLDERPATH + "\\" + fileName;
                        Console.WriteLine("File Name: {0}", fileName);
                        Console.WriteLine("File name with path : {0}", fileNameWithPath);
                        //Deploy application  
                        //Console.WriteLine("Wanna install {0} application on this VM?   Press any key to contiune.", fileName);  
                        //Console.ReadKey(); 
                        deployApplications(fileNameWithPath); Console.ReadLine();
                    }
                }

            }
            else
                Console.WriteLine("Directory does not exist at: {0}", SOURCEFOLDERPATH);

        }
        static string SetupIIS()
        {
            // In command prompt run this command to see all the features names which are equivalent to UI features.
            // c:\>dism /online /get-features /format:table 
            var featureNames = new List<string>
                {
                    "IIS-ApplicationDevelopment",
                    "IIS-ISAPIExtensions",
                    "IIS-ISAPIFilter",
                    "IIS-CommonHttpFeatures",
                    "IIS-DefaultDocument",
                    "IIS-HttpErrors",
                    "IIS-StaticContent",
                    "IIS-HealthAndDiagnostics",
                    "IIS-HttpLogging",
                    "IIS-HttpTracing",
                    "IIS-WebServer",
                    "IIS-WebServerRole",
                    "IIS-ManagementConsole",
                };

            Console.WriteLine("Checking the Operating System...\n");

            ManagementObjectSearcher obj = new ManagementObjectSearcher("select * from Win64_OperatingSystem");
            try
            {
                foreach (ManagementObject wmi in obj.Get())
                {
                    string Name = wmi.GetPropertyValue("Caption").ToString();

                    // Remove all non-alphanumeric characters so that only letters, numbers, and spaces are left.
                    // Imp. for 32 bit window server 2008
                    Name = Regex.Replace(Name.ToString(), "[^A-Za-z0-9 ]", "");

                    if (Name.Contains("Server 2012 R2") || Name.Contains("Windows 81"))
                    {
                        featureNames.Add("IIS-ASPNET45");
                        featureNames.Add("IIS-NetFxExtensibility45");
                    }
                    else if (Name.Contains("Server 2008 R2") || Name.Contains("Windows 7"))
                    {
                        featureNames.Add("IIS-ASPNET");
                        featureNames.Add("IIS-NetFxExtensibility");
                    }
                    else if (Name.Contains("Server 2016") || Name.Contains("Windows 10"))
                    {
                        featureNames.Add("IIS-ASPNET45");
                        featureNames.Add("IIS-NetFxExtensibility45");
                        Console.WriteLine("IN!!!!!!!!!!!!!!!!!!!");
                    }
                    else
                    {
                        featureNames.Clear();
                    }

                    string Version = (string)wmi["Version"];
                    string Architecture = (string)wmi["OSArchitecture"];

                    Console.WriteLine("Operating System details:");
                    Console.WriteLine("OS Name: " + Name);
                    Console.WriteLine("Version: " + Version);
                    Console.WriteLine("Architecture: " + Architecture + "\n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occurred:" + ex.Message);
            }

            return Run(
                "dism",
                string.Format(
                    "/NoRestart /Online /Enable-Feature {0}",
                    string.Join(
                        " ",
                        featureNames.Select(name => string.Format("/FeatureName:{0}", name)))));
        }

        static string Run(string fileName, string arguments)
        {
            Console.WriteLine("Enabling IIS features...");
            Console.WriteLine(arguments);

            using (var process = Process.Start(new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                RedirectStandardOutput = true,
                UseShellExecute = false,
            }))
            {
                process.WaitForExit();
                return process.StandardOutput.ReadToEnd();
            }
        }

        private static void InstallSqlEngine()
        {
            SqlExpInstall EI = new SqlExpInstall();
            EI.sqlExpressSetupFileLocation = Path.Combine(Directory.GetCurrentDirectory() + @"\Prerequisites\SQL\SQLEXPR_x64_ENU\", "SETUP.exe");  //Provide location for the Express setup file              
            int pid = EI.InstallExpress();

            // now let's wait till the setup is complete  
            Process installp = Process.GetProcessById(pid);
            if (installp != null)
                installp.WaitForExit();
            int cc = installp.ExitCode;
        }

        static void ExtractSQLEXPR1()
        {
            string command = @"SQL2019-SSEI-Expr.exe /ACTION=Download MEDIAPATH="+ Directory.GetCurrentDirectory() + @"\Prerequisites\SQL " + "/MEDIATYPE=Core /QUIET";
            ProcessStartInfo pStartInfo = new ProcessStartInfo("cmd.exe", "/c " + command);
            pStartInfo.FileName = Directory.GetCurrentDirectory() + @"\Prerequisites\SQL";
            pStartInfo.Arguments = " /S /v/qn";
            pStartInfo.CreateNoWindow = true;
            pStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            pStartInfo.UseShellExecute = false;
            Process p = new Process();
            p.StartInfo = pStartInfo;
            p.Start();
            p.WaitForExit();
        }

        static void ExtractSQLEXPR2()
        {
            string command = @"SQLEXPR_x64_ENU.exe /q /x:" + Directory.GetCurrentDirectory() + @"\Prerequisites\SQL";
            ProcessStartInfo pStartInfo = new ProcessStartInfo("cmd.exe", "/c " + command);
            pStartInfo.FileName = Directory.GetCurrentDirectory() + @"\Prerequisites\SQL";
            pStartInfo.Arguments = " /S /v/qn";
            pStartInfo.CreateNoWindow = true;
            pStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            pStartInfo.UseShellExecute = false;
            Process p = new Process();
            p.StartInfo = pStartInfo;
            p.Start();
            p.WaitForExit();
        }

        static void InstallSQLEXPR2019()
        {
            string command = @"SETUP.EXE /ConfigurationFile=ConfigurationFile.ini";
            ProcessStartInfo pStartInfo = new ProcessStartInfo("powershell.exe", "/c " + command);
            pStartInfo.FileName = Directory.GetCurrentDirectory() + @"\Prerequisites\SQL";
            pStartInfo.Arguments = " /S /v/qn";
            pStartInfo.CreateNoWindow = true;
            pStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            pStartInfo.UseShellExecute = false;
            Process p = new Process();
            p.StartInfo = pStartInfo;
            p.Start();
            p.WaitForExit();
        }
    }

    public class SqlExpInstall
    {
        #region Internal variables  

        //Variables for setup.exe command line  
        private string instanceAction = "Install";                                                          // Required  
        private string instanceFeature = "SQL,Tools";                                                       // Required  
        public string instanceName = "SQLEXPRESS";                                                          // Required  
        public string installSqlDir = "C:\\Program Files\\";                                                // Optional  
        public string installSqlSharedDir = "C:\\Program Files\\";                                          // Optional  
        public string installSqlDataDir = "C:\\Program Files\\Microsoft SQL Server\\";                      // Optional  
        public bool sqlAutoStart = true;                                                                    // Optional  
        public bool sqlBrowserAutoStart = false;                                                            // Optional  
        public string sqlBrowserAccount = "NT AUTHORITY\\SYSTEM";                                           // Optional  
        public string sqlBrowserPassword = "";                                                              // Optional  
        public string sqlAccount = "NT AUTHORITY\\SYSTEM";                                                  // Required  
        public string sqlPassword = "123";                                                             // Required  
        public bool sqlSecurityMode = true;                                                                 // Optional  
        public string saPassword = "123";                                                           // Required when SECURITYMODE=SQL  
        public string sqlCollation = "SQL_Latin1_General_Cp1_CS_AS";                                        // Optional  
        public bool disableNetworkProtocols = true;                                                         // Optional  
        public bool errorReporting = true;                                                                  // Optional  
        public string sqlExpressSetupFileLocation = Path.Combine(@"C:\Downloads", "sqlexpr.exe");   // Required  
        public bool enableRANU = false;                                                                     // Optional  
        public string sysAdminAccount = "Builtin\\Administrators";                                          // Required  
        public string agtSqlAccount = "NT AUTHORITY\\Network Service";                                      // Required  
        public bool sqlServiceLicence = true;                                                               // Required  

        #endregion

        private string BuildCommandLine()
        {
            StringBuilder strCommandLine = new StringBuilder();

            if (!IsNullOrEmpty(instanceAction))
            {
                strCommandLine.Append(" ACTION=\"").Append(instanceAction).Append("\"");
            }

            if (!IsNullOrEmpty(instanceFeature))
            {
                strCommandLine.Append(" FEATURES=\"").Append(instanceFeature).Append("\"");
            }

            if (!IsNullOrEmpty(installSqlDir))
            {
                strCommandLine.Append(" INSTANCENAME=\"").Append(instanceName).Append("\"");
            }

            if (!IsNullOrEmpty(sqlAccount))
            {
                strCommandLine.Append(" SQLSVCACCOUNT=\"").Append(sqlAccount).Append("\"");
            }

            if (!IsNullOrEmpty(sqlPassword))
            {
                strCommandLine.Append(" SQLSVCPASSWORD=\"").Append(sqlPassword).Append("\"");
            }

            if (!IsNullOrEmpty(sysAdminAccount))
            {
                strCommandLine.Append(" SQLSYSADMINACCOUNTS=\"").Append(sysAdminAccount).Append("\"");
            }

            if (!IsNullOrEmpty(agtSqlAccount))
            {
                strCommandLine.Append(" AGTSVCACCOUNT=\"").Append(agtSqlAccount).Append("\"");
            }

            if (sqlSecurityMode == true)
            {
                strCommandLine.Append(" SECURITYMODE=SQL");
            }

            if (!IsNullOrEmpty(saPassword))
            {
                strCommandLine.Append(" SAPWD=\"").Append(saPassword).Append("\"");
            }

            if (!IsNullOrEmpty(sqlCollation))
            {
                strCommandLine.Append(" SQLCOLLATION=\"").Append(sqlCollation).Append("\"");
            }

            if (errorReporting == true)
            {
                strCommandLine.Append(" ERRORREPORTING=1");
            }
            else
            {
                strCommandLine.Append(" ERRORREPORTING=0");
            }

            if (enableRANU == true)
            {
                strCommandLine.Append(" ENABLERANU=1");
            }
            else
            {
                strCommandLine.Append(" ENABLERANU=0");
            }

            if (sqlServiceLicence == true)
            {
                strCommandLine.Append(" IACCEPTSQLSERVERLICENSETERMS=1");
            }
            else
            {
                strCommandLine.Append(" IACCEPTSQLSERVERLICENSETERMS=0");
            }
            strCommandLine.Append(" QUIET=True");

            return strCommandLine.ToString();
        }

        public int InstallExpress()
        {
            Process myProcess = new Process();
            myProcess.StartInfo.FileName = sqlExpressSetupFileLocation;
            myProcess.StartInfo.Arguments = "/q" + BuildCommandLine();
            /*  /q -- Specifies that setup run with no user interface. */
            myProcess.StartInfo.UseShellExecute = false;
            myProcess.Start();
            return myProcess.Id;
        }

        private static bool IsNullOrEmpty(string str)
        {
            return (str == null) || (str == string.Empty);
        }

       

    }
}

