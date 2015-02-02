using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ExcelImport.Infrastructure;
using ExcelImport.Infrastructure.Entities;

namespace ExcelImport.WebUI
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var spreadSheets = Infrastructure.Importer.GetSpreadSheetsToProcess();
            var import = new Recipes();

            foreach (var spreadSheet in spreadSheets)
            {
                
                import.Open(spreadSheet);
                var recipes = import.GetRecipesForImport();
                rptRecipes.DataSource = recipes;//.OrderBy(x=>x.MealType);
                rptRecipes.DataBind();
            }
        }

        protected void rptRecipes_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var item = (Recipe)e.Item.DataItem;

                ((HtmlInputCheckBox)e.Item.FindControl("chkRecipe")).Value = item.Id.ToString();
                ((HtmlGenericControl)e.Item.FindControl("divRecipe")).Attributes.Add("id", item.Id.ToString());

                ((Label)e.Item.FindControl("lblRecipe")).Text = item.Name;
                ((Label)e.Item.FindControl("lblDescription")).Text = item.Description;
                ((Label)e.Item.FindControl("lblPreparationTime")).Text = item.PreparationTime;
                ((Label)e.Item.FindControl("lblDescription")).Text = item.Description;
                ((Label)e.Item.FindControl("lblCookingTime")).Text = item.CookingTime;

                ((Label)e.Item.FindControl("lblServes")).Text = item.Serves;
                ((Label)e.Item.FindControl("lblIngredients")).Text = item.Ingredients;
                ((Label)e.Item.FindControl("lblMethod")).Text = item.Method;
                ((Label)e.Item.FindControl("lblKeywords")).Text = item.Keywords;
                ((Label)e.Item.FindControl("lblSuggestion")).Text = item.Suggestion;

                string appliances = "";

                foreach (var appliance in item.AssociatedAppliances)
                {
                    appliances += appliance + ",";
                }

                ((Label)e.Item.FindControl("lblAssociatedAppliances")).Text = appliances;
                ((Label)e.Item.FindControl("lblProductCategory")).Text = item.ProductCategory;
                ((Label)e.Item.FindControl("lblMealType")).Text = item.MealType;
                ((Label)e.Item.FindControl("lblImage")).Text = string.Format("/Global/RecipeImages/{0}.jpg", item.Image);

            }
        }
    }
}
