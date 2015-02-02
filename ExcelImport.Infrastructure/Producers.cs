using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using ExcelImport.Infrastructure.Entities;

namespace ExcelImport.Infrastructure
{
    public class Producers
    {
        static LinqToExcel.IExcelQueryFactory excel;
        
        public void Open(string fileName)
        {
            if (excel == null || excel.FileName != fileName)
            {
                excel = new LinqToExcel.ExcelQueryFactory(fileName);
            }
        }

        public List<Producer> GetProducersForImport()
        {
            var producersToImport = new List<Producer>();
          
            var allProducerRows = from pr in excel.WorksheetNoHeader(0)
                                 select pr;


            var importRows = allProducerRows.Skip(1).ToArray();

            int x = 0;

            foreach (var row in importRows)
            {
                producersToImport.Add(
                    new Producer
                        {
                            Id = x,
                            ProductType = row[0].Value.ToString(),
                            Variation = row[1].Value.ToString(),
                            Exceptions = row[2].Value.ToString(),
                            Brand = row[3].Value.ToString(),
                            ProducerName = row[4].Value.ToString(),
                            FirstName = row[5].Value.ToString(),
                            LastName = row[6].Value.ToString(),
                            AddressLine1 = row[7].Value.ToString(),
                            AddressLine2 = row[8].Value.ToString(),
                            State = row[9].Value.ToString(),
                            PostCode = row[10].Value.ToString(),
                            Country = row[11].Value.ToString(),
                            CoOrdinates = row[12].Value.ToString().Split(',').ToList(),
                            Biography = row[13].Value.ToString()
                        });
                x++;
            }

            return producersToImport;
        }
    }
}
