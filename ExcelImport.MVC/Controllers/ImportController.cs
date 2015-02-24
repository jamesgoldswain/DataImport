using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ExcelImport.Infrastructure.Entities;
using ExcelImporter.Common;
using Raven.Client;

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
