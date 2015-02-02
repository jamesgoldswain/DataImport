using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ExcelImport.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Import.Test
{
    [TestClass]
    public class ReflectionTests
    {

        [TestMethod]
        public void GetAll()
        {
            Assembly assembly = Assembly.LoadFrom(@"D:\Projects\LiquorBarons.EPiServer\Dev\Release\CMS\bin\Vivid.Episerver.Template.Domain.dll");

            string classType = string.Format("{0}.{1}", "Vivid.Episerver.Template.Domain.Products", "Cider");
            var productClassType = assembly.GetType(classType);

            if (productClassType != null)
            {
 
                var productSearchFields = productClassType.GetSearchFields().Select(prop => prop.Name).ToList();

                foreach (var searchField in productSearchFields)
                {

                   
                }
            }

            Assert.AreEqual(productClassType, null);
        }
    }
}
