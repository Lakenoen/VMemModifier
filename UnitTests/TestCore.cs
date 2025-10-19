using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMemReaderCore;

namespace UnitTests
{
    public class TestCore
    {
        [Fact]
        public void BmSearchTest()
        {
            string text = "I'm heading back to Colorado tomorrow after being down in Santa Barbara over the weekend for the festival there. I will be making October plans once there and will try to arrange so I'm back here for the birthday if possible. I'll let you know as soon as I know the doctor's appointment schedule and my flight plans.";
            string emptyText = "";
            string substring1 = "over";
            string substring2 = "October";
            string substring3 = "plans";
            string substring4 = "I'm h";
            string substring5 = "user";
            string substring6 = "";
            string substring7 = " as soon as I know th";

            var positions = SearchAlgorithms.bmSearch(Encoding.UTF8.GetBytes(text).ToList(), Encoding.UTF8.GetBytes(substring1).ToList());
            string res = text.Substring((int)positions.ElementAt(0), substring1.Length);
            Assert.Equal(substring1, res);

            positions = SearchAlgorithms.bmSearch(Encoding.UTF8.GetBytes(text).ToList(), Encoding.UTF8.GetBytes(substring2).ToList());
            res = text.Substring((int)positions.ElementAt(0), substring2.Length);
            Assert.Equal(substring2, res);

            positions = SearchAlgorithms.bmSearch(Encoding.UTF8.GetBytes(text).ToList(), Encoding.UTF8.GetBytes(substring7).ToList());
            res = text.Substring((int)positions.ElementAt(0), substring7.Length);
            Assert.Equal(substring7, res);

            positions = SearchAlgorithms.bmSearch(Encoding.UTF8.GetBytes(text).ToList(), Encoding.UTF8.GetBytes(substring3).ToList());
            res = text.Substring((int)positions.ElementAt(0), substring3.Length);
            Assert.Equal(substring3, res);

            positions = SearchAlgorithms.bmSearch(Encoding.UTF8.GetBytes(text).ToList(), Encoding.UTF8.GetBytes(substring4).ToList());
            res = text.Substring((int)positions.ElementAt(0), substring4.Length);
            Assert.Equal(substring4, res);

            positions = SearchAlgorithms.bmSearch(Encoding.UTF8.GetBytes(text).ToList(), Encoding.UTF8.GetBytes(substring5).ToList());
            Assert.Empty(positions);

            positions = SearchAlgorithms.bmSearch(Encoding.UTF8.GetBytes(text).ToList(), Encoding.UTF8.GetBytes(substring6).ToList());
            Assert.Empty(positions);

            positions = SearchAlgorithms.bmSearch(Encoding.UTF8.GetBytes(emptyText).ToList(), Encoding.UTF8.GetBytes(substring1).ToList());
            Assert.Empty(positions);
        }

        [Fact]
        public void SearchRegExTest()
        {
            string text = "I'm heading back to Colorado tomorrow after being down in Santa Barbara over the weekend for the festival there. I will be making October plans once there and will try to arrange so I'm back here for the birthday if possible. I'll let you know as soon as I know the doctor's appointment schedule and my flight plans.";
            string emptyText = "";
            string substring1 = "\\w*rrow";
            string substring2 = "po[a-z]{1}sib(l?)e";
            string substring3 = "";
            var sizes = new List<int>();

            var positions = SearchAlgorithms.SearchRegEx(Encoding.UTF8.GetBytes(text).ToList(), Encoding.UTF8.GetBytes(substring1).ToList(), sizes);
            string res = text.Substring((int)positions.ElementAt(0), sizes.First());
            Assert.Equal("tomorrow", res);

            positions = SearchAlgorithms.SearchRegEx(Encoding.UTF8.GetBytes(text).ToList(), Encoding.UTF8.GetBytes(substring2).ToList(), sizes);
            res = text.Substring((int)positions.ElementAt(0), sizes.First());
            Assert.Equal("possible", res);

            positions = SearchAlgorithms.SearchRegEx(Encoding.UTF8.GetBytes(text).ToList(), Encoding.UTF8.GetBytes(substring3).ToList(), sizes);
            Assert.Equal(317, positions.Count);

            positions = SearchAlgorithms.SearchRegEx(Encoding.UTF8.GetBytes(emptyText).ToList(), Encoding.UTF8.GetBytes(substring2).ToList(), sizes);
            Assert.Empty(positions);
        }
    }
}
