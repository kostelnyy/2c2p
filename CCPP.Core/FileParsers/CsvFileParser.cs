using CCPP.Core.Domain;
using System.Collections.Generic;

namespace CCPP.Core.FileParsers
{
    public class CsvFileParser : FileParser
    {
        public override string Extension => "csv";

        public override IEnumerable<PaymentTranstaction> ParseContent(string content)
        {
            throw new System.NotImplementedException();
        }
    }
}
