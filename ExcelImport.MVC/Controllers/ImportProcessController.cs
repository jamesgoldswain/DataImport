using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using ExcelImport.Infrastructure;
using ExcelImport.Infrastructure.Entities;
using ExcelImport.MVC.Hubs;
using ExcelImporter.Common;
using Microsoft.AspNet.SignalR;
using Raven.Client;
using Microsoft.AspNet.SignalR.Hubs;
using System.Net.Http;
using System.Linq;

namespace ExcelImport.MVC.Controllers
{
    public class ImportProcessController : ApiControllerWithHub<ImportStatusHub>
    {
        public class ImportStatusUpdate
        {
            public string Status { get; set; }
            public int Percentage { get; set; }
        }

        public class ImportResult
        {
            public ImportResult()
            {
                Success = false;
                Total = 0;
            }

            public string Name { get; set; }
            public string Message { get; set; }
            public bool Success { get; set; }
            public int Total { get; set; }
        }

        [System.Web.Http.HttpGet]
        public ActionResult Process(string fileName)
        {
            string spreadSheetPath = Path.Combine(Helpers.GetSetting("FolderToProcess"), "Pending", fileName);

            var result = new ImportResult();

            if (File.Exists(spreadSheetPath))
            {
                FileInfo fi = new FileInfo(spreadSheetPath);
                var spreadSheet = new ExcelSheet();
                spreadSheet.Open(spreadSheetPath);

                var importStatusUpdate = new ImportStatusUpdate();
                importStatusUpdate.Status = "Importing ...";
                importStatusUpdate.Percentage = 0;

                try
                {
                    string dbName = Helpers.GetSetting("DBName");

                    var importRows = spreadSheet.GetRows(0);

                    importStatusUpdate.Status = string.Format("Found {0} products, looking for variants", importRows.Count);

                    try
                    {
                        List<ExpandoObject> importVariants = spreadSheet.GetRows(1);

                        foreach (dynamic row in importRows)
                        {
                            var query = from t in importVariants
                                where
                                    ((IDictionary<string, object>) t)["ParentProductId"].ToString() == row.Id.ToString()
                                select t;

                            //var productVariants = importVariants.AsQueryable().Where(r => r.ParentProductId == row.Id);
                            if (query.Any())
                            {
                                ((IDictionary<string, object>) row)["Variants"] = query.ToList();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // no variants
                    }

                    importStatusUpdate.Percentage = 50;
                    Hub.Clients.All.AddMessage(importStatusUpdate);

                    var import = new Import();

                    import.ProductType = spreadSheet.Name();
                    import.Name = fileName;
                    import.Items = importRows;

                    result.Name = import.Name;
                    result.Total = import.Items.Count;

                    importStatusUpdate.Status = "saving to DB";
                    importStatusUpdate.Percentage = 80;
                    Hub.Clients.All.addMessage(importStatusUpdate);

                    using (IDocumentSession session = RavenController.DocumentStore.OpenSession(database: dbName))
                    {
                        session.Store(import);
                        session.SaveChanges();
                        result.Success = true;

                        importStatusUpdate.Status = "complete";
                        importStatusUpdate.Percentage = 100;
                        Hub.Clients.All.addMessage(importStatusUpdate);
                        
                    }

                    FileController.SetAsProcessed(fileName);

                    return new MyResult().GetJson(result);

                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                    
                }

            }

            return new MyResult().GetJson(result);
        }
    }

}
