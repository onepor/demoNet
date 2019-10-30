using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebApi.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public string TestMethod1()
        {
            Controllers.ValuesController valuesController = new Controllers.ValuesController();
            var aa = valuesController.CsWebApiLog();
            return aa;
        }
    }
}
