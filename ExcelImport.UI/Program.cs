using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ExcelImport.Infrastructure;
using ExcelImporter.Common;

namespace ExcelImport.UI
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileName = "Liquor_Barons_Producer_Details.xlsx";
            string spreadSheetPath = Path.Combine(Helpers.GetSetting("FolderToProcess"), "Pending", fileName);

            if (File.Exists(spreadSheetPath))
            {
                FileInfo fi = new FileInfo(spreadSheetPath);
                var spreadSheet = new ExcelSheet();
                spreadSheet.Open(fileName);
                foreach (var header in spreadSheet.GetExcelHeaders("Products"))
                {
                    Console.WriteLine(header);
                }
            }

            Console.Read();
        }
    }
}
