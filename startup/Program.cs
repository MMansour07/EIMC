using System;
using System.Diagnostics;
using System.ComponentModel;
using System.IO;

namespace startup
{
    class Program
    {
        static void Main(string[] args)
        {
            EnableIIS();
            EnableDotNet3();
            EnableDotNet4();
            InstallSQLEXP();
            Installdotnet5();
            Installdotnetcore();
            InstallAspNetCore();
            InstallAspNetCoreHosting();
            InstallAccessDatabaseEngine();
        }
        static void EnableIIS()
        {
            string command = "Install-WindowsFeature -name Web-Server -IncludeManagementTools";
            ProcessStartInfo pStartInfo = new ProcessStartInfo("powershell.exe", "/c " + command);
            Process p = new Process();
            p.StartInfo = pStartInfo;
            p.Start();
        }
        static void EnableDotNet3()
        {
            string command = "DISM.exe /online /enable-feature /all /featurename:NetFx3";
            ProcessStartInfo pStartInfo = new ProcessStartInfo("cmd.exe", "/c " + command);
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
    }
}
