using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ActivityReport
{
    public class HtmlReport
    {
        private readonly IList<Activity> _activities;
        private readonly ActivityHtml _activityHtml;

        public HtmlReport(IEnumerable<Activity> activities)
        {
            _activities = activities.Where(e => e.StartTime != default && e.Duration != TimeSpan.Zero)
                .OrderBy(e => e.StartTime).ToArray();
            var start = _activities.Min(e => e.StartTime);
            var end = _activities.Max(e => e.StartTime + e.Duration);
            _activityHtml = new ActivityHtml(start, end);
        }

        public async Task Write(TextWriter writer)
        {
            await writer.WriteLineAsync(@"<!DOCTYPE html><html><head>");
            await writer.WriteLineAsync(@"<style>");
            await writer.WriteLineAsync(HtmlTemplate.Css);
            await writer.WriteLineAsync(HtmlTemplate.Palette);
            await writer.WriteLineAsync(@"</style></head>");
            await writer.WriteLineAsync(@"<body>");
            await writer.WriteLineAsync(@"<div class=""root"">");

            foreach (var (activity, index) in _activities.Select((e, i) => (e, i)))
            {
                await _activityHtml.Write(writer, activity, true, index);
            }

            await writer.WriteLineAsync(@"</div>");
            await writer.WriteLineAsync(@"<script>");
            await writer.WriteLineAsync(HtmlTemplate.Js);
            await writer.WriteLineAsync(@"</script>");
            await writer.WriteLineAsync(@"</body>");
            await writer.WriteLineAsync(@"</html>");
        }
    }
}