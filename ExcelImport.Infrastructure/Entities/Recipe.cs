using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExcelImport.Infrastructure.Entities
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PreparationTime { get; set; }
        public string CookingTime { get; set; }
        public string Serves { get; set; }
        public string Ingredients { get; set; }
        public string Method { get; set; }
        public string Keywords { get; set; }
        public string Suggestion { get; set; }
        public List<string> AssociatedAppliances { get; set; }
        public string ProductCategory { get; set; }
        public string MealType { get; set; }
        public string Image{ get; set; }

    }
}
