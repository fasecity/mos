using Mosan;
using System;
using Xunit;

namespace XUnitTestMosan
{
    public class AnnouceTest
    {
        [Fact]
        public void PassTest1()
        {
           var d = DateTime.Now;
            var expected = "high";
            Announcement f = new Announcement(1, d.ToShortDateString(), "HIGH", "Test");

            Assert.Equal(expected, f.Priority);

        }

        [Fact]
        public void FailTest1()
        {
            var d = DateTime.Now;
            var expected = "high";
            Announcement f = new Announcement(1, d.ToShortDateString(), " HIGH ", ". ");

            Assert.Equal(expected, f.Priority);

        }


    }
}
