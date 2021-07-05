using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Dialog;

namespace CritPathTest
{
    [TestClass]
    public class UnitTest1
    {
        CriticalPath criticalPath = new CriticalPath(@"D:\Visual studio projects\JohnsonCrit\Dialog\bin\Debug\ExamExample.csv", "D:\\Visual studio projects\\JohnsonCrit\\Dialog\\bin\\Debug\\Res.csv");
        [TestMethod]
        public void CriticalPathFindingTesting()
        {
            criticalPath.ReadData();
            criticalPath.CalculatingPaths();
            var testing = criticalPath.FindCriticalPath();
            Assert.IsNotNull(testing);
        }
        [TestMethod]
        public void CriticalPathLengthTesting()
        {
            int pathlen = 29;
            criticalPath.ReadData();
            criticalPath.CalculatingPaths();
            var testing = criticalPath.FindCriticalPath();
            Assert.AreEqual(testing[0].length, pathlen);
        }
    }
}
