using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExcelImport.Infrastructure.Entities
{

    [Serializable]
    public class ProducerException
    {
        public ProducerException()
        {
            ExceptionRules = new List<ExceptionRule>();
        }

        public string ProducerName { get; set; }
        public List<ExceptionRule> ExceptionRules { get; set; }
    }

    [Serializable]
    public class ExceptionRule
    {
        public RuleType RuleType { get; set; }
        public string ProductId { get; set; }
        public string WineMakerId { get; set; }
        public ProducerAddress Address { get; set; }
        public string ProductBiography { get; set; }
        public string ProductType { get; set; }
        public string Brand { get; set; }
    }

    [Serializable]
    public class ProducerAddress
    {
        public string StreetAddress { get; set; }
        public string Suburb { get; set; }
        public string PostCode { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }

    /// <summary>
    /// Types of rules
    /// (just to se how JSON deals with enums!)
    /// </summary>
    public enum RuleType
    {
        Winemaker,
        Address,
        Brand,
        Biography,
        ProductType
    }
}
