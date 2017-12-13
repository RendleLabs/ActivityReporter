using System;
using System.Globalization;
using RendleLabs.Diagnostics.DiagnosticSourceExtensions.ReflectionHelpers;
using Xunit;

namespace RendleLabs.Diagnostics.DiagnosticSourceExtensions.Tests
{
    public class SimplePropertyReflectorTests
    {
        [Fact]
        public void WithIntProperty()
        {
            var o = new {i = 42};
            var spr = SimplePropertyReflector.Generate(o.GetType().GetProperty("i"));
            var s = spr.ToString(o);
            Assert.Equal("42", s);
        }

        [Fact]
        public void WithNullableIntProperty()
        {
            int? i = 42;
            var o = new {i};
            var spr = SimplePropertyReflector.Generate(o.GetType().GetProperty("i"));
            var s = spr.ToString(o);
            Assert.Equal("42", s);
        }

        [Fact]
        public void WithNullableIntPropertyThatIsNull()
        {
            int? i = null;
            var o = new {i};
            var spr = SimplePropertyReflector.Generate(o.GetType().GetProperty("i"));
            var s = spr.ToString(o);
            Assert.Null(s);
        }

        [Fact]
        public void WithStringProperty()
        {
            var o = new {i = "Forty-two"};
            var spr = SimplePropertyReflector.Generate(o.GetType().GetProperty("i"));
            var s = spr.ToString(o);
            Assert.Equal("Forty-two", s);
        }

        [Fact]
        public void WithDateTimeOffsetProperty()
        {
            var now = DateTimeOffset.UtcNow;
            var o = new {now};
            var spr = SimplePropertyReflector.Generate(o.GetType().GetProperty("now"));
            var s = spr.ToString(o);
            Assert.Equal(now.ToString("O", CultureInfo.InvariantCulture), s);
        }
        
        [Fact]
        public void WithDateTimeProperty()
        {
            var now = DateTime.UtcNow;
            var o = new {now};
            var spr = SimplePropertyReflector.Generate(o.GetType().GetProperty("now"));
            var s = spr.ToString(o);
            Assert.Equal(now.ToString("O", CultureInfo.InvariantCulture), s);
        }
        
        [Fact]
        public void WithTimeSpanProperty()
        {
            var now = DateTime.UtcNow - DateTime.UtcNow.Date;
            var o = new {now};
            var spr = SimplePropertyReflector.Generate(o.GetType().GetProperty("now"));
            var s = spr.ToString(o);
            Assert.Equal(now.ToString("c", CultureInfo.InvariantCulture), s);
        }

        [Fact]
        public void WithDateNullableTimeOffsetProperty()
        {
            DateTimeOffset? now = DateTimeOffset.UtcNow;
            var o = new {now};
            var spr = SimplePropertyReflector.Generate(o.GetType().GetProperty("now"));
            var s = spr.ToString(o);
            Assert.Equal(now.Value.ToString("O", CultureInfo.InvariantCulture), s);
        }
        
        [Fact]
        public void WithNullableDateTimeProperty()
        {
            DateTime? now = DateTime.UtcNow;
            var o = new {now};
            var spr = SimplePropertyReflector.Generate(o.GetType().GetProperty("now"));
            var s = spr.ToString(o);
            Assert.Equal(now.Value.ToString("O", CultureInfo.InvariantCulture), s);
        }
        
        [Fact]
        public void WithNullableTimeSpanProperty()
        {
            TimeSpan? now = DateTime.UtcNow - DateTime.UtcNow.Date;
            var o = new {now};
            var spr = SimplePropertyReflector.Generate(o.GetType().GetProperty("now"));
            var s = spr.ToString(o);
            Assert.Equal(now.Value.ToString("c", CultureInfo.InvariantCulture), s);
        }
    }
}