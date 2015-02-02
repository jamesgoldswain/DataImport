using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExcelImport.Infrastructure.Entities
{
    public class Producer
    {
        public int Id { get; set; }
        public string ProductType { get; set; }
        public string Variation { get; set; }
        public string Exceptions { get; set; }
        public string Brand { get; set; }
        public string ProducerName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Country { get; set; }
        public List<string> CoOrdinates { get; set; }
        public string Biography { get; set; }
        public string State { get; set; }
        public string PostCode { get; set; }
        public ProducerException ProducerExceptions { get; set; }
    }
}
