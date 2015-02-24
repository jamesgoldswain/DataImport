using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using ExcelImport.MVC.Controllers;
using ExcelImporter.Common;
using Raven.Client;

namespace ExcelImport.MVC.Api
{
    [RoutePrefix("api/import")]
    public class ImportController : ApiController
    {
        [Route("list")]
        [HttpGet]
        public List<string> GetImports()
        {
            string dbName = Helpers.GetSetting("DBName");

            using (IDocumentSession session = RavenController.DocumentStore.OpenSession(database: dbName))
            {
                var imports = session.Query<ExcelImport.Infrastructure.Entities.Import>();

                return imports.Select(x => x.Name).ToList();
            }

            return null;
        }
    }
}