using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ActivityReport
{
    public class ActivityHtml
    {
        private readonly DateTimeOffset _start;
        private readonly DateTimeOffset _end;

        public ActivityHtml(DateTimeOffset start, DateTimeOffset end)
        {
            _start = start;
            _end = end;
        }

        public async Task Write(TextWriter writer, Activity activity, bool collapsed, int index)
        {
            var (leftMargin, rightMargin) = Margins(activity);
            var level = CountLevel(activity.Parent);
            await writer.WriteAsync($@"<div class=""split palette-{index % 4} level-{level}");
            if (collapsed)
            {
                await writer.WriteAsync(" collapsed");
            }

            await writer.WriteLineAsync($@""" style=""margin-left:{CssPercent(leftMargin)};margin-right:{CssPercent(rightMargin)}"">");
            await writer.WriteLineAsync(@"<div class=""split-label"">");
            await writer.WriteLineAsync($@"<button class=""toggle-button subs-{activity.Children.Count}"">&gt;</button>");
            await writer.WriteLineAsync($@"<span title=""{Title(activity)}"">{activity.Operation}</span>");
            await writer.WriteLineAsync("</div>");
            await writer.WriteLineAsync($@"<div class=""spacer"" title=""{Title(activity)}"">{activity.Duration.TotalMilliseconds:N}ms</div>");
            
            if (activity.Children.Any())
            {
                await writer.WriteLineAsync(@"<div class=""sub"">");
                foreach (var child in activity.Children.OrderBy(a => a.StartTime))
                {
                    await Write(writer, child, true, index);
                }

                await writer.WriteLineAsync("</div>");
            }

            await writer.WriteLineAsync("</div>");
            if (level == 0)
            {
                await writer.WriteLineAsync("<hr>");
            }
        }

        private static string CssPercent(double value) => value < double.Epsilon ? "0" : $"{value}%";

        private (double, double) Margins(Activity activity)
        {
            var (parentStart, parentEnd, parentDuration) = ParentValues(activity.Parent);
            var offsetStart = (activity.StartTime - parentStart).TotalMilliseconds;
            var offsetEnd = (parentEnd - (activity.StartTime + activity.Duration)).TotalMilliseconds;
            var leftMargin = (offsetStart / parentDuration) * 100;
            var rightMargin = (offsetEnd / parentDuration) * 100;
            return (leftMargin, rightMargin);
        }

        private (DateTimeOffset start, DateTimeOffset end, double duration) ParentValues(Activity parent)
        {
            if (parent == null) return (_start, _end, (_end - _start).TotalMilliseconds);
            var end = parent.StartTime + parent.Duration;
            return (parent.StartTime, end, (end - parent.StartTime).TotalMilliseconds);
        }

        private static int CountLevel(Activity parent)
        {
            return parent == null ? 0 : 1 + CountLevel(parent.Parent);
        }

        private static string Title(Activity split)
        {
            return $"{split.Duration.TotalMilliseconds:N}ms, [{split.Source}][{split.Operation}]";
        }
    }
}