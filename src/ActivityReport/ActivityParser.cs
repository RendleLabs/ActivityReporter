using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using JetBrains.Annotations;

namespace ActivityReport
{
    public class ActivityParser
    {
        private readonly List<Entry> _nonActivityEntries = new List<Entry>();
        private readonly ConcurrentDictionary<string, Activity> _activities = new ConcurrentDictionary<string, Activity>();
        
        [PublicAPI]
        public void AddXml(string xmlString) => AddXml(XElement.Parse(xmlString));

        [PublicAPI]
        public void AddXml(XElement xml)
        {
            var entry = new Entry
            {
                Source = xml.Attribute("source")?.Value,
                Key = xml.Attribute("key")?.Value,
            };

            var activityId = xml.Attribute("id")?.Value;
            
            if (string.IsNullOrWhiteSpace(activityId))
            {
                _nonActivityEntries.Add(entry);
                return;
            }

            var activity = _activities.GetOrAdd(activityId, id => new Activity
            {
                Id = id,
                ParentId = xml.Attribute("parentId")?.Value,
                Operation = xml.Attribute("operation")?.Value,
                Source = xml.Attribute("source")?.Value,
                StartTime = ParseDateTimeOffset(xml.Attribute("startTime")?.Value),
                Duration = ParseMilliseconds(xml.Attribute("duration")?.Value)
            });
            
            activity.Entries.Add(entry);

            UpdateNesting(activity);
        }

        private void UpdateNesting(Activity activity)
        {
            if ((!string.IsNullOrWhiteSpace(activity.ParentId)) && _activities.TryGetValue(activity.ParentId, out var parent))
            {
                parent.AddChild(activity);
            }

            foreach (var child in _activities.Values.Where(a => a.ParentId == activity.Id))
            {
                activity.AddChild(child);
            }
        }

        public IEnumerable<Activity> TopLevelActivities()
        {
            return _activities.Values.Where(a => a.Parent == null);
        }

        private static TimeSpan ParseMilliseconds(string milliseconds)
        {
            if (string.IsNullOrWhiteSpace(milliseconds)) return TimeSpan.Zero;
            return double.TryParse(milliseconds, out var ms) ? TimeSpan.FromMilliseconds(ms) : TimeSpan.Zero;
        }

        private static DateTimeOffset ParseDateTimeOffset(string dateString)
        {
            if (string.IsNullOrWhiteSpace(dateString)) return default;
            return DateTimeOffset.TryParse(dateString, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal,
                out var date)
                ? date
                : default;
        }
    }
}