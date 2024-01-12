using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WixSharp;

namespace Build
{
    internal class Program
    {
        private static string projectName = "WPFclient";
        private static string version = "1.0.0";
        static void Main(string[] args)
        {
            var project = new Project()
            {
                Name = projectName,
                UI = WUI.WixUI_ProgressOnly,
                OutDir = "output",
                GUID = new Guid("ec701726-f78a-4bc6-9314-fa809430ac54"),
                MajorUpgrade = MajorUpgrade.Default,
                ControlPanelInfo =
                {
                    Manufacturer=Environment.UserName,
                },
                Dirs = new Dir[]
                {
                    new InstallDir(@"%AppDataFolder%\",
                    new File(@"C:\Users\e.egorov\source\repos\RamWebClient\WPFclient\bin\Debug\WPFclient.exe"),
                    new Dir(@"WPFclient",
                    new DirFiles(@"C:\Users\e.egorov\source\repos\RamWebClient\WPFclient\bin\Debug\*.*")))
                },

            };
            project.Version = new Version(version);

            project.BuildMsi();
        }
    }
}
