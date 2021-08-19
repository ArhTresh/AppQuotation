using AppQuotation.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;

namespace AppQuotation
{
    public class QuotationUpdate
    {
                     


        private static readonly HttpClient client = new HttpClient();


        public async Task UpdateAsync()
        {

            using (var db = new ApplicationContext())
            {
                
                var Companies = db.Companies.ToList();
                var Sources = db.Sources.ToList();

                foreach (Source s in Sources)
                {
                    if (s.Name == "Finnhub")
                    {
                        foreach (Company c in Companies)
                        {
                            string json = await client.GetStringAsync(s.BaseApiUrl + c.Ticker);
                            var quote = JsonSerializer.Deserialize<Quote>(json);
                            if (quote.c != 0)
                            {
                                Quotation quotation = new Quotation { CompanyId = c.Id, Date = DateTime.Now, Price = Convert.ToString(quote.c).Replace(',', '.'), SourceId = s.Id };
                                db.Quotations.Add(quotation);
                            }
                        }
                        db.SaveChanges();
                    }

                    else if (s.Name == "MOEX")
                    {
                        XmlDocument xDoc = new XmlDocument();
                        xDoc.Load(s.BaseApiUrl);
                        XmlElement xRoot = xDoc.DocumentElement;

                        foreach (Company c in Companies)
                        {
                            foreach (XmlNode xnode in xRoot)
                            {
                                foreach (XmlNode rows in xnode.ChildNodes)
                                {
                                    foreach (XmlNode row in rows.ChildNodes)
                                    {
                                        if (row.Attributes.Count > 0)
                                        {
                                            XmlNode attr = row.Attributes.GetNamedItem("SECID");
                                            XmlNode attr2 = row.Attributes.GetNamedItem("PREVADMITTEDQUOTE");

                                            if (attr.Value == c.Ticker)
                                            {
                                                Quotation quotation = new Quotation { CompanyId = c.Id, Date = DateTime.Now, Price = Convert.ToString(attr2.Value), SourceId = s.Id };
                                                db.Quotations.Add(quotation);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                await db.SaveChangesAsync();

            }
                
        }
    }
}
