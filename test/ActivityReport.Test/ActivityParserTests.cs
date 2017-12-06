using System;
using System.Linq;
using System.Xml.Linq;
using Xunit;

namespace ActivityReport.Test
{
    public class ActivityParserTests
    {
        [Fact]
        public void LoadsNestedActivities()
        {
            var parser = new ActivityParser();
            parser.AddXml(Xml1);
            parser.AddXml(Xml2);
            parser.AddXml(Xml3);
            parser.AddXml(Xml4);
            var actual = parser.TopLevelActivities().ToList();
            Assert.Collection(actual, a =>
            {
                Assert.Equal("|85239d86-498cf83c8feb6846.", a.Id);
                Assert.Collection(a.Children, c =>
                {
                    Assert.Equal("|85239d86-498cf83c8feb6846.1.", c.Id);
                    Assert.Equal(a, c.Parent);
                });
            });
        }

        private const string Xml1 = @"<?xml version=""1.0"" encoding=""utf-8""?>
<entry source=""Bar"" key=""Outer.Start"" operation=""Outer"" startTime=""2017-12-05T13:45:55.3311878Z""" +@" duration=""13.8722"" id=""|85239d86-498cf83c8feb6846."" parentId="""" />"
            ;

        private const string Xml2 = @"<?xml version=""1.0"" encoding=""utf-8""?>
<entry source=""Bar"" key=""Inner.Start"" operation=""Inner"" startTime=""2017-12-05T13:45:55.3332979Z"" duration=""10.8695"" id=""|85239d86-498cf83c8feb6846.1."" parentId=""|85239d86-498cf83c8feb6846."" />"
            ;

        private const string Xml3 = @"<?xml version=""1.0"" encoding=""utf-8""?>
<entry source=""Bar"" key=""Inner.Stop"" operation=""Inner"" startTime=""2017-12-05T13:45:55.3332979Z"" duration=""10.8695"" id=""|85239d86-498cf83c8feb6846.1."" parentId=""|85239d86-498cf83c8feb6846."" />"
            ;

        private const string Xml4 = @"<?xml version=""1.0"" encoding=""utf-8""?>
<entry source=""Bar"" key=""Outer.Stop"" operation=""Outer"" startTime=""2017-12-05T13:45:55.3311878Z"" duration=""13.8722"" id=""|85239d86-498cf83c8feb6846."" parentId="""" />"
            ;
    }
}