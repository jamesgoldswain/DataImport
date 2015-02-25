﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Web.Mvc;
using ExcelImport.Infrastructure;
using ExcelImport.Infrastructure.Constants;
using ExcelImport.Infrastructure.Entities;
using ExcelImport.MVC.Hubs;
using ExcelImporter.Common;
using Microsoft.AspNet.SignalR;
using Raven.Client;
using System.Linq;

namespace ExcelImport.MVC.Controllers
{
    public class ImportProcessController : ApiControllerWithHub<ImportStatusHub>
    {
        public class ImportStatusUpdate
        {
            public string Status { get; set; }
            public int Percentage { get; set; }
            public bool Success { get; set; }
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
            string spreadSheetPath = Path.Combine(Helpers.GetSetting(ImportConstants.FOLDER_TO_PROCESS), ImportConstants.PENDING_FOLDER, fileName);

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

                    var importRows = GetRows(spreadSheet);

                    var import = new Import();

                    import.ProductType = spreadSheet.Name();
                    import.Name = fileName;
                    import.Items = importRows;

                    result.Name = import.Name;
                    result.Total = import.Items.Count;

                    importStatusUpdate.Status = "saving to DB";
                    importStatusUpdate.Percentage = 0;
                    Hub.Clients.All.addMessage(importStatusUpdate);

                    using (IDocumentSession session = RavenController.DocumentStore.OpenSession(database: dbName))
                    {
                        session.Store(import);
                        session.SaveChanges();
                        result.Success = true;

                        importStatusUpdate.Status = "complete";
                        importStatusUpdate.Percentage = 100;
                        importStatusUpdate.Success = result.Success;
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


        private static List<ExpandoObject> GetRows(ExcelSheet spreadSheet)
        {
            var spreadSheets = spreadSheet.GetAllSheets();

            var sheetsInDocumentToProcess = new List<string>();

            foreach (var sheet in spreadSheets)
            {
                var headers = spreadSheet.GetExcelHeaders(sheet);
                if (headers.Any())
                {
                    // this sheet has headings
                    sheetsInDocumentToProcess.Add(sheet);
                }
            }

            if (sheetsInDocumentToProcess.Any())
            {
                var allRowsImport = new List<ExpandoObject>();
                // there are relations, so lets build them
                foreach (var sheet in sheetsInDocumentToProcess)
                {
                    List<ExpandoObject> importVariants = spreadSheet.GetRows(sheet);
                    allRowsImport.AddRange(importVariants);
                }


                var rootItems = from t in allRowsImport where ((IDictionary<string, object>)t)["Parent"].ToString() == string.Empty select t;



                foreach (var rootItem in rootItems)
                {
                    dynamic x = new ExpandoObject();
                    dynamic item = rootItem;

                    // set the root
                    x.Root = item.Category;

                    // get the root children
                    var rootItemsRelations = from t in allRowsImport where ((IDictionary<string, object>)t)["Parent"].ToString() == item.Category.ToString() select t;

                    x.Items = rootItemsRelations;

                    foreach (dynamic rootItemsRelation in x.Items)
                    {


                        dynamic rootItemsRelationItem = rootItemsRelation;
                        dynamic y = new ExpandoObject();

                        var rootItemsRelationsSub = from t in allRowsImport where ((IDictionary<string, object>)t)["Parent"].ToString() == rootItemsRelationItem.Category.ToString() select t;

                        y.Items = rootItemsRelationsSub;
                        rootItemsRelation.Items = y.Items;
                    }
                    return new List<ExpandoObject> { x };
                }
            }
            else
            {
                return spreadSheet.GetRows(0);
            }
            return null;
        }
    }

}
