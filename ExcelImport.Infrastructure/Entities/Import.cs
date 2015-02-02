using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace ExcelImport.Infrastructure.Entities
{
    public class Import
    {
        public Import()
        {
            Date = DateTime.Now.ToShortDateString();
            Time = DateTime.Now.ToShortTimeString();
        }

        public string ProductType { get; set; }
        public string Name { get; set; }
        public List<ExpandoObject> Items { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
    }
}
