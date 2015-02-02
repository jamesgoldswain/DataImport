using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using ExcelImport.Infrastructure;
using ExcelImporter.Common;
using Newtonsoft.Json;

namespace ExcelImport.MVC.Controllers
{
    public class ImportController : Controller
    {
        //
        // GET: /Import/

        public ActionResult Index()
        {
            return View();
        }

    }
}
