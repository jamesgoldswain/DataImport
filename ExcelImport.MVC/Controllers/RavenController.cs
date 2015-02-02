using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExcelImporter.Common;
using Raven.Client;
using Raven.Client.Document;

namespace ExcelImport.MVC.Controllers
{
    public class RavenController : Controller
    {
        public new IDocumentSession Session { get; set; }
        private static IDocumentStore _documentStore;

        public static IDocumentStore DocumentStore
        {
            get
            {
                string ravenDBurl = Helpers.GetSetting("RavenDBUrl");

                if (_documentStore != null) return _documentStore;
                lock (typeof(RavenController))
                {
                    if (_documentStore != null) return _documentStore;

                    _documentStore = new DocumentStore
                        {
                            Url = ravenDBurl
                        }.Initialize();
                }
                return _documentStore;
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Session = DocumentStore.OpenSession();
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            using (Session)
            {
                if(Session != null && filterContext.Exception == null)
                    Session.SaveChanges();
            }
        }

        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding)
        {
            return base.Json(data, contentType, contentEncoding, JsonRequestBehavior.AllowGet);
        }
    }
}
