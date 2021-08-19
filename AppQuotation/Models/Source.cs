using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppQuotation.Models
{
    public class Source
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string BaseApiUrl { get; set; }

        public List<Quotation> Quotations { get; set; }
        public Source()
        {
            Quotations = new List<Quotation>();
        }
    }
}
