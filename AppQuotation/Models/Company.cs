using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppQuotation.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Ticker { get; set; }

        public List<Quotation> Quotations { get; set; }
        public Company()
        {
            Quotations = new List<Quotation>();
        }
    }
}
