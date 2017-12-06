using System;
using System.Collections.Generic;

namespace ActivityReport
{
    public class Activity
    {
        public string Id { get; set; }
        public string ParentId { get; set; }
        public Activity Parent { get; set; }
        public List<Entry> Entries { get; } = new List<Entry>();
        public HashSet<Activity> Children { get; } = new HashSet<Activity>(EqualityComparer.Instance);
        public string Operation { get; set; }
        public string Source { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public TimeSpan Duration { get; set; }

        public void AddChild(Activity child)
        {
            child.Parent = this;
            Children.Add(child);
        }

        public class EqualityComparer : IEqualityComparer<Activity>
        {
            public static EqualityComparer Instance { get; } = new EqualityComparer();
            
            public bool Equals(Activity x, Activity y) => string.Equals(x?.Id, y?.Id);

            public int GetHashCode(Activity obj) => (obj.Id?.GetHashCode()).GetValueOrDefault();
        }
    }
}