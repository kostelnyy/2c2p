using CCPP.Core.Domain;
using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace CCPP.Core.FileParsers.Csv
{
    public class CsvFileParser : FileParser
    {
        public override string Extension => ".csv";

        public override (IEnumerable<PaymentTransaction> Result, IEnumerable<string> Errors) ParseContent(Stream content)
        {
            using var reader = new StreamReader(content);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            csv.Configuration.HasHeaderRecord = false;
            csv.Configuration.RegisterClassMap<PaymentTransactionCsvMap>();

            var invalidRows = new List<string>();
            var result = new List<PaymentTransaction>();

            while (csv.Read())
            {
                try
                {
                    result.Add(csv.GetRecord<PaymentTransaction>());
                }
                catch
                {
                    invalidRows.Add(csv.Context.RawRecord);
                    if(invalidRows.Count > 10)
                    {
                        break;
                    }
                }
            }

            return (result, invalidRows);
        }
    }
}
