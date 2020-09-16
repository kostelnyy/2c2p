using CCPP.Core.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace CCPP.Core.FileParsers.Xml
{
    public class XmlFileParser : FileParser
    {
        public override string Extension => "xml";

        public override IEnumerable<PaymentTranstaction> ParseContent(Stream content)
        {
            var result = XElement.Load(content)
                .Descendants("Transaction")
                .Select(t => new PaymentTranstaction
                {
                    Id = t.Attribute("id").Value,
                    Amount = decimal.Parse(t.Element("PaymentDetails").Element("Amount").Value),
                    Currency = t.Element("PaymentDetails").Element("CurrencyCode").Value,
                    TransactionDate = DateTime.Parse(t.Element("TransactionDate").Value),
                    Status = MapStatus(t.Element("Status").Value)
                }).ToList();
            return result;

            PaymentTranstactionStatus MapStatus(string input) => input switch
            {
                "Approved" => PaymentTranstactionStatus.Approved,
                "Done" => PaymentTranstactionStatus.Done,
                "Rejected" => PaymentTranstactionStatus.Rejected,
                _ => throw new ArgumentException($"Can't parse status in XML {input}")
            };
        }
    }
}
