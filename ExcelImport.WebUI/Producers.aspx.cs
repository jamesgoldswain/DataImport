using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ExcelImport.Infrastructure;
using ExcelImport.Infrastructure.Entities;

namespace ExcelImport.WebUI
{


    public partial class Producers : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var spreadSheets = Infrastructure.Importer.GetSpreadSheetsToProcess();
            var import = new ExcelImport.Infrastructure.Producers();

            foreach (var spreadSheet in spreadSheets)
            {
                var producerImport = new List<Producer>();
                import.Open(spreadSheet);

                // we get all the producers
                var allProducers = import.GetProducersForImport();

                var distinctProducers = allProducers.Where(x => !string.IsNullOrEmpty(x.ProducerName)).OrderBy(x => x.ProducerName).Select(dp => new { ProductType = dp.ProductType, ProducerName = dp.ProducerName }).Distinct().ToList();

                var producerExceptionList = new List<ProducerException>();

                foreach (var producer in distinctProducers)
                {
                    var pe = new ProducerException { ProducerName = producer.ProducerName };

                    var exceptionRules = new List<ExceptionRule>();
                    var producers = allProducers.Where(p => p.ProducerName == producer.ProducerName).ToList();

                    foreach (var p in producers)
                    {

                        if (!string.IsNullOrEmpty(p.Exceptions))
                        {
                            // build the exceptions
                            var exceptions = p.Exceptions.Split(',');

                            foreach (var exceptionType in exceptions)
                            {
                                RuleType ruleType;
                                if (Enum.TryParse(exceptionType.TrimEnd(' '), out ruleType))
                                {
                                    // build the exception
                                    if (ruleType == RuleType.Winemaker)
                                        exceptionRules.Add(new ExceptionRule { RuleType = ruleType, WineMakerId = p.FirstName.Replace(" ", "") + p.LastName.Replace(" ", ""), ProductId = p.Variation });

                                    if (ruleType == RuleType.Address)
                                        try
                                        {
                                            if (p.CoOrdinates.Count == 2)
                                            {
                                                exceptionRules.Add(new ExceptionRule { RuleType = ruleType, WineMakerId = p.FirstName.Replace(" ","") + p.LastName.Replace(" ",""), ProductId = p.Variation, Address = new ProducerAddress { Latitude = p.CoOrdinates[0], Longitude = p.CoOrdinates[1], PostCode = p.PostCode, StreetAddress = p.AddressLine1, Suburb = p.AddressLine2 } });
                                            }
                                            else
                                            {
                                                exceptionRules.Add(new ExceptionRule { RuleType = ruleType, WineMakerId = p.FirstName.Replace(" ", "") + p.LastName.Replace(" ", ""), ProductId = p.Variation, Address = new ProducerAddress { Latitude = string.Empty, Longitude = string.Empty, PostCode = p.PostCode, StreetAddress = p.AddressLine1, Suburb = p.AddressLine2 } });
                                            }
                                        }
                                        catch (Exception ex)
                                        {

                                        }

                                    // build the exception
                                    if (ruleType == RuleType.Biography)
                                        exceptionRules.Add(new ExceptionRule { RuleType = ruleType, ProductBiography = p.Biography, ProductId = p.Variation });

                                    // build the exception
                                    if (ruleType == RuleType.Brand)
                                        exceptionRules.Add(new ExceptionRule { RuleType = ruleType, Brand = p.Brand, ProductId = p.Variation });

                                    // build the exception
                                    if (ruleType == RuleType.ProductType)
                                        exceptionRules.Add(new ExceptionRule { RuleType = ruleType, ProductType = p.ProductType, ProductId = p.Variation });

                                }
                            }

                        }
                    }


                    pe.ExceptionRules = exceptionRules;
                    producerExceptionList.Add(pe);
                }

                var finalListWithExceptions = new List<Producer>();

                
                foreach (var producer in distinctProducers)
                {
                    var perl = allProducers.Where(er => er.ProducerName == producer.ProducerName).FirstOrDefault();

                    if (perl != null)
                    {
                        var pexceptions = producerExceptionList.Where(ex => ex.ProducerName == perl.ProducerName).FirstOrDefault();

                        if (pexceptions != null)
                        {
                            perl.Exceptions = Deserialize(pexceptions);

                            
                        }

                        finalListWithExceptions.Add(perl); 
                    }
                }

                rptRecipes.DataSource = finalListWithExceptions;
                rptRecipes.DataBind();
            }
        }

        public string Deserialize(ProducerException rules)
        {
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            var exceptionJson = jsonSerializer.Serialize(rules);
            return exceptionJson.ToString();
        }


        protected void rptRecipes_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                try
                {
                    var item = (Producer)e.Item.DataItem;

                    ((HtmlInputCheckBox)e.Item.FindControl("chkRecipe")).Value = item.Id.ToString();
                    ((HtmlGenericControl)e.Item.FindControl("divRecipe")).Attributes.Add("data-id", item.Id.ToString());

                    ((Label)e.Item.FindControl("lblRecipe")).Text = item.ProductType + " " + item.Brand + " " + item.ProducerName;
                    ((Label)e.Item.FindControl("lblVariation")).Text = item.Variation;
                    
                    ((Label)e.Item.FindControl("lblProductType")).Text = item.ProductType;
                    ((Label)e.Item.FindControl("lblExceptions")).Text = item.Exceptions;
                    ((Label)e.Item.FindControl("lblBrand")).Text = item.Brand;
                    ((Label)e.Item.FindControl("lblProducerName")).Text = item.ProducerName;
                    ((Label)e.Item.FindControl("lblFirstName")).Text = item.FirstName;

                    ((Label)e.Item.FindControl("lblLastName")).Text = item.LastName;
                    ((Label)e.Item.FindControl("lblAddressLine1")).Text = item.AddressLine1;
                    ((Label)e.Item.FindControl("lblAddressLine2")).Text = item.AddressLine2;
                    ((Label)e.Item.FindControl("lblState")).Text = item.State;
                    ((Label)e.Item.FindControl("lblPostCode")).Text = item.PostCode;
                    ((Label)e.Item.FindControl("lblCountry")).Text = item.Country;

                    string appliances = "";

                    foreach (var appliance in item.CoOrdinates)
                    {
                        appliances += appliance + ",";
                    }

                    ((Label)e.Item.FindControl("lblCoOrdinates")).Text = appliances;
                    ((Label)e.Item.FindControl("lblBiography")).Text = item.Biography;

                }
                catch (Exception ex)
                {
                    
                    throw;
                }

            }
        }
    }
}
