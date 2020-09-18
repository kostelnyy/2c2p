using CCPP.Core.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace CCPP.Core.FileParsers.Xml
{
    public class XmlFileParser : FileParser
    {
        public override string Extension => ".xml";

        public override (IEnumerable<PaymentTransaction> Result, IEnumerable<string> Errors) ParseContent(Stream content)
        {
            var transtactions = XElement.Load(content)
                .Descendants("Transaction");

            var errors = new List<string>();
            var result = new List<PaymentTransaction>();

            foreach(var transaction in transtactions)
            {
                try
                {
                    result.Add(ParseFromXElement(transaction));
                }
                catch
                {
                    errors.Add(transaction.ToString());
                }
            }

            return (result, errors);

            static PaymentTransaction ParseFromXElement(XElement t)
            {
                var result = new PaymentTransaction
                {
                    Id = t.Attribute("id").Value,
                    Amount = decimal.Parse(t.Element("PaymentDetails").Element("Amount").Value),
                    Currency = t.Element("PaymentDetails").Element("CurrencyCode").Value,
                    TransactionDate = DateTime.Parse(t.Element("TransactionDate").Value),
                    Status = MapStatus(t.Element("Status").Value)
                };
                return result;
            }

            static PaymentTransactionStatus MapStatus(string input) => input switch
            {
                "Approved" => PaymentTransactionStatus.A,
                "Done" => PaymentTransactionStatus.D,
                "Rejected" => PaymentTransactionStatus.R,
                _ => throw new ArgumentException($"Can't parse status in XML {input}")
            };
        }
    }
}
