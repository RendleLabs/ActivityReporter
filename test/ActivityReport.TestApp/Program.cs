using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ActivityReport.TestApp
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var directory = args.FirstOrDefault() ?? Environment.CurrentDirectory;
            var activities = await Activities(directory);
            var report = new HtmlReport(activities);
            using (var writer = File.CreateText("activityreport.html"))
            {
                await report.Write(writer);
            }
        }

        private static async Task<Activity[]> Activities(string directory)
        {
            var files = new Files().Find(directory).ToArray();
            var entryParser = new ActivityParser();
            foreach (var fileTask in new FileReader().ReadAll(files))
            {
                entryParser.AddXml(await fileTask);
            }
            return entryParser.TopLevelActivities().ToArray();
        }
    }
}