using System;
using System.Diagnostics;
using System.IO;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;

namespace startup
{
    class Program
    {
        static void Main(string[] args)
        {
            //ProcessFolder();
            EnableIIS();
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
        static void InstallSQLEXP()
        {
            Process p = new Process();
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = Directory.GetCurrentDirectory() + @"\SqlServer2016 Express\SQLEXPR_x64_ENU";//  Application.StartupPath.Trim() + @"\SqlServer2005 Express\SQLEXPR.EXE";
            //-q[n|b|r|f]   Sets user interface (UI) level:
            //n = no UI
            //b = basic UI (progress only, no prompts)
            //r = reduced UI (dialog at the end of installation)
            //f = full UI
            //psi.Arguments = "/qb username=\"sa\" companyname=\"Rumtek\" addlocal=ALL  disablenetworkprotocols=\"0\" instancename=\"SQLExpress\" SECURITYMODE=\"SQL\" SAPWD=\"lock\"";
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
            psi.FileName = Directory.GetCurrentDirectory() + @"\AccessDatabaseEngine\AccessDatabaseEngine.exe";//  Application.StartupPath.Trim() + @"\SqlServer2005 Express\SQLEXPR.EXE";
            //-q[n|b|r|f]   Sets user interface (UI) level:
            //n = no UI
            //b = basic UI (progress only, no prompts)
            //r = reduced UI (dialog at the end of installation)
            //f = full UI
            //psi.Arguments = "/qb username=\"sa\" companyname=\"Rumtek\" addlocal=ALL  disablenetworkprotocols=\"0\" instancename=\"SQLExpress\" SECURITYMODE=\"SQL\" SAPWD=\"lock\"";
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
            const string SOURCEFOLDERPATH = @"\dotnet5";

            if (Directory.Exists(Directory.GetCurrentDirectory()+ SOURCEFOLDERPATH))
            {
                Console.WriteLine("Directory exists at: {0}", SOURCEFOLDERPATH);
                if (Directory.GetFiles(Directory.GetCurrentDirectory() + SOURCEFOLDERPATH, "*.exe").Length > 0)
                {
                    int count = Directory.GetFiles(Directory.GetCurrentDirectory() + SOURCEFOLDERPATH, "*.exe").Length;
                    string[] files = Directory.GetFiles(Directory.GetCurrentDirectory() + SOURCEFOLDERPATH, "*.exe");

                    foreach (var file in files)
                    {
                        var fileName = Path.GetFileName(file);
                        var fileNameWithPath = Directory.GetCurrentDirectory() + SOURCEFOLDERPATH + "\\" + fileName;
                        Console.WriteLine("File Name: {0}", fileName);
                        Console.WriteLine("File name with path : {0}", fileNameWithPath);
                        //Deploy application  
                        Console.WriteLine("Wanna install {0} application on this VM?   Press any key to contiune.", fileName);  
                        Console.ReadKey(); deployApplications(fileNameWithPath); Console.ReadLine();
                    }
                }

            }
            else
                Console.WriteLine("Directory does not exist at: {0}", SOURCEFOLDERPATH);

        }
        //public static void DeployApplications(string executableFilePath)
        //{
        //    PowerShell powerShell = null;
        //    Console.WriteLine(" ");
        //    Console.WriteLine("Deploying application...");
        //    try
        //    {
        //        using (powerShell = PowerShell.Create())
        //        {
        //            //here “executableFilePath” need to use in place of “  
        //            //'C:\\ApplicationRepository\\FileZilla_3.14.1_win64-setup.exe'”  
        //            //but I am using the path directly in the script.  
        //            powerShell.AddScript("$setup=Start-Process '"+executableFilePath+"' -ArgumentList ' / S ' -Wait -PassThru");  
        //            Collection < PSObject > PSOutput = powerShell.Invoke(); foreach (PSObject outputItem in PSOutput)
        //            {
        //                if (outputItem != null)
        //                {
        //                    Console.WriteLine(outputItem.BaseObject.GetType().FullName);
        //                    Console.WriteLine(outputItem.BaseObject.ToString() + "\n");
        //                }
        //            }

        //            if (powerShell.Streams.Error.Count > 0)
        //            {
        //                string temp = powerShell.Streams.Error.First().ToString();
        //                Console.WriteLine("Error: {0}", temp);

        //            }
        //            else
        //                Console.WriteLine("Installation has completed successfully.");

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Error occured: {0}", ex.InnerException);
        //        //throw;  
        //    }
        //    finally
        //    {
        //        if (powerShell != null)
        //            powerShell.Dispose();
        //    }

        //}
    }
}

