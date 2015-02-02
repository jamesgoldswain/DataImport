using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ExcelImporter.Common;

namespace ExcelImport.Infrastructure
{

    public class Importer
    {
        public static List<string> GetSpreadSheetsToProcess()
        {
            var fileList = new List<string>();
            string directoryPath = Path.Combine(Helpers.GetSetting("FolderToProcess"), "Pending");

            foreach( var file in Directory.GetFiles(directoryPath))
            {
                fileList.Add(file);
            }

            return fileList;
        }
    }
}
