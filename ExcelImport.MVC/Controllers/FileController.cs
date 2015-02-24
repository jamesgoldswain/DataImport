using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using ExcelImport.Infrastructure;
using ExcelImport.Infrastructure.Constants;
using ExcelImport.Infrastructure.Entities;
using ExcelImporter.Common;
using Newtonsoft.Json;
using Raven.Client;

namespace ExcelImport.MVC.Controllers
{
    public class FileController : ApiController
    {
        [System.Web.Mvc.AcceptVerbs("POST", "HEAD")]
        public Task<IEnumerable<string>> PostMultipartStream()
        {
            if (Request.Content.IsMimeMultipartContent())
            {

                string fullPath = Path.Combine(Helpers.GetSetting(ImportConstants.FOLDER_TO_PROCESS), ImportConstants.PENDING_FOLDER);
                MyMultipartFormDataStreamProvider streamProvider = new MyMultipartFormDataStreamProvider(fullPath);
                var task = Request.Content.ReadAsMultipartAsync(streamProvider).ContinueWith(t =>
                {
                    if (t.IsFaulted || t.IsCanceled)
                        throw new HttpResponseException(HttpStatusCode.InternalServerError);

                    var fileInfo = streamProvider.FileData.Select(i =>
                    {
                        var info = new FileInfo(i.LocalFileName);
                        return "File uploaded as " + info.FullName + " (" + info.Length + ")";
                    });
                    return fileInfo;

                });
                return task;
            }
            else
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, "Invalid Request!"));
            }
        }

        [System.Web.Http.HttpGet]
        public List<string> GetSpreadSheetsToProcess()
        {
            var fileList = new List<string>();
            string directoryPath = Path.Combine(Helpers.GetSetting(ImportConstants.FOLDER_TO_PROCESS), "Pending");

            foreach (var file in Directory.GetFiles(directoryPath))
            {
                FileInfo fi = new FileInfo(file);
                fileList.Add(fi.Name);
            }

            return fileList;
        }

        [System.Web.Http.HttpGet]
        public bool Delete(string fileToDelete)
        {

            string directoryPath = Path.Combine(Helpers.GetSetting(ImportConstants.FOLDER_TO_PROCESS), ImportConstants.PENDING_FOLDER);
            string deletedDirectoryPath = Path.Combine(Helpers.GetSetting(ImportConstants.FOLDER_TO_PROCESS), ImportConstants.DELETED_FOLDER);

            var file = Directory.GetFiles(directoryPath).Where(f => f == Path.Combine(directoryPath, fileToDelete));
           
            if (file.Any())
            {
                if (File.Exists(Path.Combine(deletedDirectoryPath, fileToDelete)))
                {
                    File.Move(Path.Combine(deletedDirectoryPath, fileToDelete), Path.Combine(deletedDirectoryPath, DateTime.Now.ToFileTimeUtc().ToString() + '_' + fileToDelete));
                }
                File.Move(Path.Combine(directoryPath, fileToDelete), Path.Combine(deletedDirectoryPath, fileToDelete));
                return true;
            }
            return false;
        }

        [System.Web.Http.HttpGet]
        public ActionResult GetSpreadSheetHeaders(string fileName)
        {
            string spreadSheetPath = Path.Combine(Helpers.GetSetting(ImportConstants.FOLDER_TO_PROCESS), ImportConstants.PENDING_FOLDER, fileName);

            if (File.Exists(spreadSheetPath))
            {
                FileInfo fi = new FileInfo(spreadSheetPath);
                var spreadSheet = new ExcelSheet();

                spreadSheet.Open(spreadSheetPath);

                var headers = spreadSheet.GetExcelHeaders(0);
                var firstLineValues = spreadSheet.GetFirstLineValues(0);

                try
                {
                    dynamic employee = new ExpandoObject();

                    for (var i = 0; i < headers.Count; i++)
                    {
                        ((IDictionary<string, object>)employee)[headers[i]] = firstLineValues[i];

                        if (headers[i].ToLower() == "id")
                        {
                            // lets check whether we have any entries relating to this in another spreadsheet
                            var secondSheetHeaders = spreadSheet.GetExcelHeaders(0);
                            if (secondSheetHeaders != null)
                            {
                                var secondSheetValues = spreadSheet.GetFirstLineValues(0);
                                dynamic secondSheetsValues = new ExpandoObject();

                                for (var x = 0; x < secondSheetHeaders.Count; x++)
                                {
                                    ((IDictionary<string, object>)secondSheetsValues)[secondSheetHeaders[x]] = secondSheetValues[x];

                                }
                                ((IDictionary<string, object>)employee)["Variants"] = secondSheetsValues;
                            }
                        }
                    }

                    string jsonText = "{\"data\": " + JsonConvert.SerializeObject(employee) + "}";
                    //var obj = JsonConvert.DeserializeObject<ExpandoObject>(jsonText, new ExpandoObjectConverter());

                    return new MyResult().GetJson(employee);
                }
                catch (Exception ex)
                {
                    
                    throw ex;
                }


            }

            return null;
        }

        public static bool SetAsProcessed(string fileName)
        {
            string spreadSheetPath = Path.Combine(Helpers.GetSetting(ImportConstants.FOLDER_TO_PROCESS), ImportConstants.PENDING_FOLDER, fileName);
            string importCompletePath = Path.Combine(Helpers.GetSetting(ImportConstants.FOLDER_TO_PROCESS), ImportConstants.PROCESSED_FOLDER, fileName);

            if (File.Exists(spreadSheetPath))
            {
                FileInfo fi = new FileInfo(spreadSheetPath);
                try 
                {
                    fi.MoveTo(importCompletePath);
                }
                catch (Exception ex)
                {

                    throw ex;
                }

            }

            return true;
        }
    }

    public class MyResult : Controller
    {
        public JsonResult GetJson(dynamic employee)
        {
            return Json(employee);
        }
    }

    public class MyMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {
        public MyMultipartFormDataStreamProvider(string path)
            : base(path)
        {

        }

        public override string GetLocalFileName(System.Net.Http.Headers.HttpContentHeaders headers)
        {
            string fileName;
            if (!string.IsNullOrWhiteSpace(headers.ContentDisposition.FileName))
            {
                fileName = headers.ContentDisposition.FileName;
            }
            else
            {
                fileName = Guid.NewGuid().ToString() + ".data";
            }
            return fileName.Replace("\"", string.Empty);
        }
    }
}
