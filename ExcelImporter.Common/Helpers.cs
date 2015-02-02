using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace ExcelImporter.Common
{
    public class Helpers
    {
        public static string GetSetting(string settingName)
        {
            string value = ConfigurationManager.AppSettings[settingName].ToString();
            return value;
        }

    }
}
