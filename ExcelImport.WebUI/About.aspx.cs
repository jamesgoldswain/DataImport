using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Util;

namespace ExcelImport.WebUI
{

    public partial class BasePage : System.Web.UI.Page
    {
        private void Page_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            if (ex is HttpRequestValidationException)
            {
                Server.ClearError();
                for (int i = 0; i < HttpContext.Current.Request.Form.Keys.Count; i++)
                {
                    if (!IsValid(HttpContext.Current.Request.Form[i]))
                    {
                        Response.Write(HttpContext.Current.Request.Form.AllKeys[i] + " is not valid");
                    }

                }
            }

        }

        private bool IsValid(dynamic input)
        {
            string value = input.ToString();
            if (value.Contains("script"))
                return false;
            return true;
        }
    }

    public partial class About : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

    }
}
