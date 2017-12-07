using System;
using System.Collections.Generic;

namespace ActivityReport
{
    public class Activity
    {
        private readonly HashSet<KeyValuePair<string, string>> _tags = new HashSet<KeyValuePair<string, string>>();
        private readonly HashSet<Activity> _children = new HashSet<Activity>(EqualityComparer.Instance);
        
        public string Id { get; set; }
        public string ParentId { get; set; }
        public Activity Parent { get; set; }
        public List<DiagnosticSourceEvent> Entries { get; } = new List<DiagnosticSourceEvent>();
        public IReadOnlyCollection<Activity> Children => _children;
        public IReadOnlyCollection<KeyValuePair<string, string>> Tags => _tags;
        public string Operation { get; set; }
        public string Source { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public TimeSpan Duration { get; set; }

        public void AddChild(Activity child)
        {
            child.Parent = this;
            _children.Add(child);
        }

        public void AddTag(string key, string value)
        {
            if (key == null) return;
            _tags.Add(new KeyValuePair<string, string>(key, value));
        }

        public class EqualityComparer : IEqualityComparer<Activity>
        {
            public static EqualityComparer Instance { get; } = new EqualityComparer();
            
            public bool Equals(Activity x, Activity y) => string.Equals(x?.Id, y?.Id);

            public int GetHashCode(Activity obj) => (obj.Id?.GetHashCode()).GetValueOrDefault();
        }
    }
}