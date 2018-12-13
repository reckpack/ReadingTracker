using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReadingTracker;

namespace Test
{
    [TestClass]
    public class RecordTests
    {
        [TestMethod]
        public void constructor1_test()
        {
            Record r = new Record();
            Assert.AreEqual(r.GetID(), -1);
        }

        [TestMethod]
        public void constructor2_test()
        {
            Assert.Fail();
        }
    }
}
