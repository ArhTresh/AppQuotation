using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppQuotation.Models
{
    public class Quotation
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public string Price { get; set; }
        public DateTime Date { get; set; }
        public int SourceId { get; set; }
        public Source Source { get; set; }
    }
}
