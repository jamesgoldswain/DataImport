using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using ExcelImport.Infrastructure.Entities;

namespace ExcelImport.Infrastructure
{
    public class Recipes
    {
        static LinqToExcel.IExcelQueryFactory excel;
        
        public void Open(string fileName)
        {
            if (excel == null || excel.FileName != fileName)
            {
                excel = new LinqToExcel.ExcelQueryFactory(fileName);
            }
        }

        public List<Recipe> GetRecipesForImport()
        {
            var recipesToImport = new List<Recipe>();
          
            var allRecipeRows = from pr in excel.WorksheetNoHeader(0)
                                 select pr;


            var importRows = allRecipeRows.Skip(1).ToArray();
            int x = 0;
            foreach (var row in importRows)
            {

                recipesToImport.Add(
                    new Recipe
                        {
                            Id = x,
                            Name = row[0].Value.ToString(),
                            Description = row[1].Value.ToString(),
                            PreparationTime = row[2].Value.ToString(),
                            CookingTime = row[3].Value.ToString(),
                            Serves = row[4].Value.ToString(),
                            Ingredients = row[5].Value.ToString().Replace("<p>", "<ul><li>").Replace("</p>", "</ul>").Replace("<br>", "<li>").Replace("<br />", "<li>"),
                            Method = row[6].Value.ToString().Replace("<p>", "<ol><li>").Replace("</p>", "</ol>").Replace("<br>", "<li>").Replace("<br />", "<li>")
                                .Replace("1.","")
                                .Replace("2.", "")
                                .Replace("3.", "")
                                .Replace("4.", "")
                                .Replace("5.", "")
                                .Replace("6.", "")
                                .Replace("7.", "")
                                .Replace("8.", "")
                                .Replace("9.", "")
                                .Replace("10.", "")
                                .Replace("11.", ""),
                            Keywords = row[7].Value.ToString(),
                            Suggestion = row[8].Value.ToString(),
                            AssociatedAppliances = row[9].Value.ToString().Split(',').ToList(),
                            ProductCategory = row[10].Value.ToString(),
                            MealType = row[11].Value.ToString(),
                            Image = row[12].Value.ToString()
                        });
                x++;
            }

            return recipesToImport;
        }
    }
}
