using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Import.Test
{
    [TestClass]
    public class ServerTests
    {
        [TestMethod]
        public void GetSites()
        {
            var iisService = new DataImport.Service.IIS.ServerService();
            iisService.GetSites("localhost");


        }
    }
}
