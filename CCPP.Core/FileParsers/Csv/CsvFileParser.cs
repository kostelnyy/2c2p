using CCPP.Core.Domain;
using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace CCPP.Core.FileParsers.Csv
{
    public class CsvFileParser : FileParser
    {
        public override string Extension => "csv";

        public override IEnumerable<PaymentTranstaction> ParseContent(Stream content)
        {
            using var reader = new StreamReader(content);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            csv.Configuration.HasHeaderRecord = false;
            csv.Configuration.RegisterClassMap<PaymentTransactionCsvMap>();
            var records = csv.GetRecords<PaymentTranstaction>();
            return records.ToList();
        }
    }
}
