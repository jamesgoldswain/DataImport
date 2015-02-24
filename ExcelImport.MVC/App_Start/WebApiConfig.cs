using System.Linq;
using System.Web.Http;

namespace ExcelImport.MVC
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{id}",
                new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
               "GetSpreadSheetsToProcess",
               "api/{controller}/{action}",
               new { controller = "File", action = "GetSpreadSheetsToProcess" });

            config.Routes.MapHttpRoute(
               "Delete",
               "api/{controller}/{action}/{fileToDelete}",
               new { controller = "File", action = "Delete", fileToDelete = RouteParameter.Optional });

            config.Routes.MapHttpRoute(
               "GetSpreadSheetHeaders",
               "api/{controller}/{action}/{fileName}",
               new { controller = "File", action = "GetSpreadSheetHeaders", fileName = RouteParameter.Optional });


            //api/importprocess/Process
            config.Routes.MapHttpRoute(
               "Process",
               "api/{controller}/{action}/{fileName}",
               new { controller = "ImportProcess", action = "Process", fileName = RouteParameter.Optional });


            var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);
        }
    }
}
