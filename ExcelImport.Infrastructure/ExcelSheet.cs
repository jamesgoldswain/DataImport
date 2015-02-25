using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using ExcelImport.Infrastructure.Entities;
using ExcelImport.Infrastructure.Mapping;
using LinqToExcel;
using LinqToExcel.Domain;

namespace ExcelImport.Infrastructure
{
    public class ExcelSheet
    {
        static LinqToExcel.IExcelQueryFactory excel;
        
        public void Open(string fileName)
        {
            if (excel == null || excel.FileName != fileName)
            {
                excel = new ExcelQueryFactory(fileName);
                excel.DatabaseEngine = DatabaseEngine.Ace;
            }
        }

        public List<string> GetExcelHeaders(int index)
        {
            var rowHeaders = new List<string>();

            var allRows = from pr in excel.Worksheet(index)
                          select pr;


            Row firstRow = allRows.FirstOrDefault();

            if (firstRow != null)
            {
                rowHeaders.AddRange(firstRow.ColumnNames);
            }

            return rowHeaders;
        }

        public List<string> GetExcelHeaders(string name)
        {
            var rowHeaders = new List<string>();

            var allRows = from pr in excel.Worksheet(name)
                                 select pr;


            Row firstRow = allRows.FirstOrDefault();

            if (firstRow != null)
            {
                rowHeaders.AddRange(firstRow.ColumnNames);
            }

            return rowHeaders;
        }

        public List<string> GetFirstLineValues(int index)
        {
            var rowHeaders = new List<string>();


            var allRows = from pr in excel.Worksheet(0)
                          select pr;


            var firstRow = allRows.ToArray().FirstOrDefault();

            if (firstRow != null)
            {
                for (var i = 0; i < firstRow.ColumnNames.Count(); i++)
                {
                    rowHeaders.Add(firstRow[i].Value.ToString());
                }
            }

            return rowHeaders;
        }

        public List<string> GetFirstLineValues(string name)
        {
            var rowHeaders = new List<string>();


            var allRows = from pr in excel.Worksheet(name)
                          select pr;


            var firstRow = allRows.ToArray().FirstOrDefault();

            if (firstRow != null)
            {
                for (var i = 0; i < firstRow.ColumnNames.Count(); i++)
                {
                    rowHeaders.Add(firstRow[i].Value.ToString());
                }
            }

            return rowHeaders;
        }

        public string Name()
        {
            return excel.GetWorksheetNames().FirstOrDefault();
        }

        public List<string> GetAllSheets()
        {
            return excel.GetWorksheetNames().ToList();
        }

        public List<ExpandoObject> GetRows(int index)
        {
            var headers = GetExcelHeaders(0);

            var rowItems = new List<ExpandoObject>();

            var allRows = from pr in excel.Worksheet(0)
                          select pr;

            var importRows = allRows.ToArray().ToList();

            foreach (var row in importRows)
            {
                if (row[0].Value != null)
                {

                    if (!string.IsNullOrEmpty(row[0].Value.ToString()))
                    {
                        var employee = new ExpandoObject();
                        for (var i = 0; i < headers.Count; i++)
                        {
                            ((IDictionary<string, object>)employee)[headers[i]] = row[i].Value;
                        }

                        rowItems.Add(employee);
                    }
                }
            }

            return rowItems;
        }

        public List<ExpandoObject> GetRows(string name)
        {
            var headers = GetExcelHeaders(name);

            var rowItems = new List<ExpandoObject>();

            var allRows = from pr in excel.Worksheet(name)
                          select pr;

            var importRows = allRows.ToArray().ToList();

            foreach (var row in importRows)
            {
                if (row[0].Value != null)
                {
                    
                    if (!string.IsNullOrEmpty(row[0].Value.ToString()))
                    {
                        var employee = new ExpandoObject();
                        for (var i = 0; i < headers.Count; i++)
                        {
                            ((IDictionary<string, object>)employee)[headers[i]] = row[i].Value;
                        }

                        rowItems.Add(employee);
                    }
                }
            }

            return rowItems;
        }

    }
}
